<%@ Page Title="Admission process" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="AdmissionFormParentDetails.aspx.cs" Inherits="AdmissionFormParentDetails" ViewStateMode="Enabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <script src="../PopCalendar2008/PopCalendarAjaxNet.js" type="text/javascript"></script>
    <script src="../PopCalendar2008/PopCalendarFunctionsAjaxNet.js" type="text/javascript"></script>
    <div style="width: 97%" align="center">
        <div id="divAdmissionSteps" runat="server">
            <table>
                <tr>
                    <td>
                        <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="2">
                        </Wizard:AdmissionSteps>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nifty" align="center">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0"
                width="100%">
                <tbody>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="2">
                            Admission Form
                            <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                                ShowSummary="false" />
                        </td>
                        <td class="borderBtm ErrMsg" align="right" colspan="2">
                            NOTE: Fields with yellow background are mandatory.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" runat="server" align="center" id="tdErrorMessage" class="ClsHilightBGB"
                            visible="false">
                            <asp:Label ID="lblError" runat="server" Text="" class="LblNrmlB" Style="border-width: 0px;
                                font-weight: bold;"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trSiblingDetailsForPP" runat="server" visible="false" style="text-align:left;">
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td colspan="2" style="height:10px;"></td>
                                </tr>
                                <tr>
                                     <td colspan="2" align="left" class="TextNormalB borderBtm" style="height: 15px">
                                        Sibling Details: Brother / sister (not cousins) presently studying in Pawar Public School
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height:10px;"></td>
                                </tr>
                                <tr>
                                   <td class="TxtNormal" style="font-size: 10pt;width:150px;">
                                                   Add Sibling Details :
                                   </td>
                                   <td align="left">
                                         <asp:CheckBox ID="chkAddSiblingDetails" runat="server" Onclick="HideSiblingDetails()" CssClass="LblSmlRslt" />                
                                   </td>
                                </tr>
                                <tr id="trSiblingDetails" style="text-align:center;" enableviewstate="true">
                                    <td style="text-align:left;" colspan="2">
                                        <table align="center" style="width:100%; margin:0px auto;">  
                                             <tr>
                                                <td class="TxtNormal" style="font-size: 10pt;">
                                                     Standard :
                                                 </td>
                                                 <td align="left" style="text-align:left; float:left;">
                                                     <asp:DropDownList ID="cmbStandard" runat="server" CssClass="TxtNormalAdmission" 
                                                         Width="120px" AutoPostBack="true" ViewStateMode="Enabled" BackColor="#ffffa0"
                                                         onselectedindexchanged="cmbStandard_SelectedIndexChanged">
                                                     </asp:DropDownList>
                                                     <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateSiblingStandard" ErrorMessage="Sibling Standard should be selected."></asp:CustomValidator>
                                                     <asp:CustomValidator ID="CustomValidator2" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateSiblingDivision" ErrorMessage="Sibling Division should be selected."></asp:CustomValidator>
                                                     <asp:CustomValidator ID="CustomValidator3" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateSiblingName" ErrorMessage="Sibling Student Name should not be blank."></asp:CustomValidator>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td class="TxtNormal" style="font-size: 10pt">
                                                     Division :
                                                 </td>
                                                 <td style="text-align:left;" align="left">  
                                                     <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                     <ContentTemplate>
                                                                 <asp:DropDownList ID="cmbDivision" runat="server" ViewStateMode="Enabled" CssClass="TxtNormalAdmission" BackColor="#ffffa0">
                                                                 </asp:DropDownList>                                                                              
                                                      </ContentTemplate>
                                                      <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                      </Triggers>
                                                     </asp:UpdatePanel>
                                                 </td>
                                             </tr>
                                             <tr>
                                                 <td class="TxtNormal" style="font-size: 10pt">
                                                     Student Name :
                                                 </td>
                                                 <td style="text-align:left;" align="left">
                                                     <asp:TextBox ID="txtSiblingName" runat="server" Style="width: 300px;" BackColor="#ffffa0"
                                                             CssClass="TxtNormalAdmission" ViewStateMode="Enabled" MaxLength="50"></asp:TextBox>
                                                 </td>
                                             </tr>                      
                                         </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height:10px;"></td>
                                </tr>
                                <tr id="trTwinSelection" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt;">
                                       Twins Selection :                                        
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkIsTwins" runat="server" CssClass="LblSmlRslt" />
                                    </td>
                                </tr>  
                              </table>
                        </td>
                    </tr>                   
                    <tr>
                        <td align="left" class="TextNormalB borderBtm" colspan="4" style="height: 15px">
                            Information About The Parent
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="TxtNormal" colspan="4" style="font-size: 10pt">
                            <table width="100%">
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Father's Name:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="2" style="font-size: 10pt">
                                        <table cellpadding="0" cellspacing="1" width="100%">
                                            <tr>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFSurname" MaxLength="50" onkeypress="return AllowOnlyNameFormat(event)"
                                                        onblur="formatName(this)" />
                                                    <asp:RequiredFieldValidator ID="reqValtxtFSurname" runat="server" ErrorMessage="Father's Last Name should not be blank." Display="None" Enabled="false" ControlToValidate="txtFSurname"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmissionMandAdmission" ID="txtFName" onkeypress="return AllowOnlyNameFormat(event)"
                                                        MaxLength="50" BackColor="#ffffa0" onblur="formatName(this)" />
                                                    <asp:RequiredFieldValidator ID="reqFName" runat="server" ErrorMessage="Father's First Name should not be blank."
                                                        Display="None" ControlToValidate="txtFName"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFFatherName" MaxLength="50" onkeypress="return AllowOnlyNameFormat(event)"
                                                        onblur="formatName(this)" />
                                                    <%--<asp:RequiredFieldValidator ID="reqValtxtFFatherName" runat="server" ControlToValidate="txtFFatherName"
                                                        Display="None" Enabled="false" ErrorMessage="Father's Father Name should not be blank."></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <%-- <td class="TxtNormal">
													<asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFAge" Width="50px" MaxLength="2"
														onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
														onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
														ondrop="event.returnValue=false" />
												</td>--%>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Last Name)
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (First Name)
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Father's Name)
                                                </td>
                                                <%--<td class="TxtNormal" align="center">
													(Age)
												</td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Mother's Name:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="2" style="font-size: 10pt">
                                        <table width="100%">
                                            <tr>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMSurname" MaxLength="50" onkeypress="return AllowOnlyNameFormat(event)"
                                                        ReadOnly="false" onblur="formatName(this)" />
                                                    <asp:RequiredFieldValidator ID="reqValtxtMSurname" runat="server" ControlToValidate="txtMSurname"
                                                        Display="None" Enabled="false" ErrorMessage="Mother's Last Name should not be blank."></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmissionMandAdmission" ID="txtMName" onkeypress="return AllowOnlyNameFormat(event)"
                                                        MaxLength="50" ReadOnly="false" BackColor="#ffffa0" onblur="formatName(this)" />
                                                    <asp:RequiredFieldValidator ID="reqMName" runat="server" ErrorMessage="Mother's First Name should not be blank."
                                                        Display="None" ControlToValidate="txtMName"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMHName" MaxLength="50" onkeypress="return AllowOnlyNameFormat(event)"
                                                        ReadOnly="false" onblur="formatName(this)" />
                                                    <%--<asp:RequiredFieldValidator ID="reqValtxtMHName" runat="server" ControlToValidate="txtMHName"
                                                        Display="None" Enabled="false" ErrorMessage="Husband's Name should not be blank."></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Last Name)
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (First Name)
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Husband's Name)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image2" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                            Width="140px" />
                                    </td>
                                    <td align="left">
                                        <strong>FATHER</strong>
                                    </td>
                                    <td align="left">
                                        <strong>MOTHER</strong>
                                    </td>
                                </tr>
                                <%--<tr>
									<td>
										Name:
									</td>
									<td>
										<asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFatherName" MaxLength="50" />
									</td>
									<td>
										<asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMotherName" MaxLength="50" />
									</td>
								</tr>--%>
                                <tr id="trSPSParentDOB" runat="server" visible="false">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Date Of Birth:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFDOB" runat="server" CssClass="MidTxtNormalAdmission" />
                                        <rjs:PopCalendar ID="PopCalendar5" runat="server"
                                        Control="txtFDOB" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                        To-Today="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMDOB" runat="server" CssClass="MidTxtNormalAdmission" />
                                        <rjs:PopCalendar ID="PopCalendar1" runat="server"
                                        Control="txtMDOB" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                        To-Today="true" />
                                    </td>
                                </tr>
                                 <tr id="trSPSAadharCardNo" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Aadhar Card Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFAadharCard" runat="server" 
                                            CssClass="MidTxtNormalAdmission" MaxLength="12" 
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />  
                                        <asp:RequiredFieldValidator ID="reqValFAadharCard" runat="server" ControlToValidate="txtFAadharCard"
                                            Display="None" Enabled="false" ErrorMessage="Father's Aadhar Card Number should not be blank."></asp:RequiredFieldValidator>                                      
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtFAadharCard" ErrorMessage="Father's Aadhar Card Number should be of 12 digits." CssClass="ClsMdtStar"
                                            ValidationExpression="^.{12}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMAadharCard" runat="server" 
                                            CssClass="MidTxtNormalAdmission" MaxLength="12" 
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />
                                        <asp:RequiredFieldValidator ID="reqValMAadharCard" runat="server" ControlToValidate="txtMAadharCard"
                                            Display="None" Enabled="false" ErrorMessage="Mothers's Aadhar Card Number should not be blank."></asp:RequiredFieldValidator>                                        
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                            ErrorMessage="Aadhar Card Number of father and mother should not be same." 
                                            Display="None" ControlToValidate="txtFAadharCard" 
                                            ControlToCompare="txtMAadharCard" Operator="NotEqual"></asp:CompareValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtMAadharCard" ErrorMessage="Mother's Aadhar Card Number should be of 12 digits." CssClass="ClsMdtStar"
                                            ValidationExpression="^.{12}$"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Name on Aadhar Card :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFNameOnAadharCard" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />  
                                        <asp:RequiredFieldValidator ID="reqValFNameOnAadhar" runat="server" ControlToValidate="txtFNameOnAadharCard"
                                            Display="None" Enabled="false" ErrorMessage="Father's Name On Aadhar Card should not be blank."></asp:RequiredFieldValidator>                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMNameOnAadharCard" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="reqValMNameOnAadhar" runat="server" ControlToValidate="txtMNameOnAadharCard"
                                            Display="None" Enabled="false" ErrorMessage="Mothers's Name On Aadhar Card should not be blank."></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                            ErrorMessage="Details added for field 'Name on Aadhar Card' for father and mother should not be same." 
                                            Display="None" ControlToValidate="txtFNameOnAadharCard" 
                                            ControlToCompare="txtMNameOnAadharCard" Operator="NotEqual"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr id="trSPSParentPanNo" runat="server" visible="false">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Pan Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFPanNo" runat="server" CssClass="MidTxtNormalAdmission" />
                                        <%--<asp:RequiredFieldValidator ID="reqValPANNO" runat="server" ControlToValidate="txtFPanNo"
                                            Display="None" Enabled="false" ErrorMessage="Father's Pan Number should not be blank."></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMPanNo" runat="server" CssClass="MidTxtNormalAdmission" />
                                    </td>
                                </tr>
                                <tr id="trSPSParentMobile" runat="server" visible="false">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Mobile Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFMobileNo" runat="server" CssClass="MidTxtNormalAdmission" /> 
                                        <%--<asp:RequiredFieldValidator ID="reqValFMobileNo" runat="server" ControlToValidate="txtFMobileNo"
                                            Display="None" Enabled="false" ErrorMessage="Father's Mobile Number should not be blank."></asp:RequiredFieldValidator>--%>                                      
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMMobileNo" runat="server" CssClass="MidTxtNormalAdmission" />                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Educational Qualification:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFQuali" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="reqValFQuali" runat="server" ControlToValidate="txtFQuali"
                                            Display="None" Enabled="false" ErrorMessage="Father's Educational Qualification should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMQuali" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="reqValMQuali" runat="server" ControlToValidate="txtMQuali"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Educational Qualification should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Mother Tongue:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFMotherTounge" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="reqValFMotherTounge" runat="server" ControlToValidate="txtFMotherTounge"
                                            Display="None" Enabled="false" ErrorMessage="Father's Mother Tongue should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMMotherTounge" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="reqValtxtMMotherTounge" runat="server" ControlToValidate="txtMMotherTounge"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Mother Tongue should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Other Languages Spoken:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFLangSpoken" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="reqValFLangSpoken" runat="server" ControlToValidate="txtFLangSpoken"
                                            Display="None" Enabled="false" ErrorMessage="Father's Other Languages Spoken should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMLangSpoken" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="reqValMLangSpoken" runat="server" ControlToValidate="txtMLangSpoken"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Other Languages Spoken should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trSPSParentbloodGroup" runat="server" visible="false">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Blood Group:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbFBloodGroup" runat="server" CssClass="TxtNormalAdmission" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="reqValFBloodGroup" runat="server" ControlToValidate="cmbFBloodGroup"
                                            Display="None" Enabled="false" ErrorMessage="Father's Blood Group should not be blank." InitialValue="0"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbMBloodGroup" runat="server" CssClass="TxtNormalAdmission" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Religion:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbFReligion" runat="server" CssClass="TxtNormalAdmission" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValFReligion" runat="server" ControlToValidate="cmbFReligion"
                                            Display="None" Enabled="false" ErrorMessage="Father's Religion should not be blank." InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbMReligion" runat="server" CssClass="TxtNormalAdmission" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValMReligion" runat="server" ControlToValidate="cmbMReligion"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Religion should not be blank." InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Nationality :
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtFNationality" MaxLength="50" />   
                                        <asp:RequiredFieldValidator ID="reqValtxtFNationality" runat="server" ControlToValidate="txtFNationality"
                                            Display="None" Enabled="false" ErrorMessage="Father's Nationality should not be blank."></asp:RequiredFieldValidator>                                     
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="TxtNormalAdmission" ID="txtMNationality" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="reqValMNationality" runat="server" ControlToValidate="txtMNationality"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Nationality should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Occupation:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbFOccupation" runat="server" CssClass="TxtNormalAdmissionMandAdmission" ViewStateMode="Enabled"
                                            BackColor="#ffffa0">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="cmp_valFOcc" runat="server" ControlToValidate="cmbFOccupation"
                                            Display="None" ErrorMessage="Father's Occupation should be selected." Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbMOccupation" runat="server" CssClass="TxtNormalAdmission" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValcmbMOccupation" runat="server" ControlToValidate="cmbMOccupation"
                                            Display="None" Enabled="false" ErrorMessage="Mother's Occupation should not be blank." InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        <asp:Label runat = "server" ID = "lblAnnualIncome">Annual Income:</asp:Label> 
                                    </td>
                                    <td>
                                       <asp:TextBox ID="txtFIncome" runat="server" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" CssClass="MidTxtNormalAdmission" MaxLength="20" />
                                        <asp:CustomValidator ID="cstAnnualIncome" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAnnualIncome" ErrorMessage=""></asp:CustomValidator>
                                    </td>
                                    <td>
                                       <asp:TextBox ID="txtMIncome" runat="server" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" CssClass="MidTxtNormalAdmission" MaxLength="20" />
                                        <asp:CustomValidator ID="CustomValidator5" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateMotherIncome" ErrorMessage="Mother's Annual Income should not be blank."></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Name of the Company/Organisation:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFCompany" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                        <asp:RequiredFieldValidator ID="reqValtxtFCompany" runat="server" ControlToValidate="txtFCompany"
                                            Display="None" Enabled="false" ErrorMessage="Father's Name of the Company/Organisation should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMCompany" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ClientValidationFunction="ValidateMotherCompany"
                                            CssClass="ClsMdtStar" Display="None" ErrorMessage="Mother's Name of the Company/Organisation should not be blank."></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trSPSParentOrgAddress" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt; height: 75px;">
                                        Company/Organisation Address:
                                    </td>
                                    <td style="height: 75px">
                                        <asp:TextBox ID="txtFOrgAddress" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"  ViewStateMode="Enabled"
                                            TextMode="MultiLine" Columns="20" Rows="4" /> 
                                        <asp:RequiredFieldValidator ID="reqValtxtFOrgAddress" runat="server" ControlToValidate="txtFOrgAddress"
                                            Display="None" Enabled="false" ErrorMessage="Father's Company/Organisation Address should not be blank."></asp:RequiredFieldValidator>                                       
                                    </td>
                                    <td style="height: 75px">
                                        <asp:TextBox ID="txtMOrgAddress" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                            TextMode="MultiLine" Columns="20" Rows="4" />   
                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ClientValidationFunction="ValidateMotherOfficeAddress"
                                            CssClass="ClsMdtStar" Display="None" ErrorMessage="Mother's Company/Organisation Address should not be blank."></asp:CustomValidator>                                     
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Designation/Type of Business:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFOccDetails" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"  ViewStateMode="Enabled"
                                            TextMode="MultiLine" Columns="20" Rows="4" />
                                        <asp:RequiredFieldValidator ID="reqValFOccDetails" runat="server" ControlToValidate="txtFOccDetails"
                                            Display="None" Enabled="false" ErrorMessage="Father's Designation/Type of Business should not be blank."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regFOccDetails" runat="server" ControlToValidate="txtFOccDetails"
                                            Display="None" ErrorMessage="Father Occupation Details should not exceed than 200 characters."
                                            ValidationExpression="^[\s\S]{0,200}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMOccDetails" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                            TextMode="MultiLine" Columns="20" Rows="4" />
                                        <asp:RegularExpressionValidator ID="regMOccDetails" runat="server" ControlToValidate="txtMOccDetails"
                                            Display="None" ErrorMessage="Mother Occupation Details should not exceed than 200 characters."
                                            ValidationExpression="^[\s\S]{0,200}$"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator8" runat="server" ClientValidationFunction="ValidateMotherDesignation"
                                            CssClass="ClsMdtStar" Display="None" ErrorMessage="Mother's Designation/Type of Business should not be blank."></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Office Contact No. :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFOffPhone" runat="server" CssClass="TxtNormalAdmission" MaxLength="20"
                                            onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" />
                                        <asp:RequiredFieldValidator ID="reqValFOffPhone" runat="server" ControlToValidate="txtFOffPhone"
                                            Display="None" Enabled="false" ErrorMessage="Father's Office Telephone No should not be blank."></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMOffPhone" runat="server" CssClass="TxtNormalAdmission" MaxLength="20"
                                            onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" />
                                        <asp:CustomValidator ID="CustomValidator9" runat="server" ClientValidationFunction="ValidateMotherOfficeTelNo"
                                            CssClass="ClsMdtStar" Display="None" ErrorMessage="Mother's Office Telephone No. should not be blank."></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        E-mail Address:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFEmail" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                            TextMode="SingleLine" Columns="20" Rows="4" />
                                        <asp:RequiredFieldValidator ID="reqValtxtFEmail" runat="server" ControlToValidate="txtFEmail"
                                            Display="None" Enabled="false" ErrorMessage="Father's E-mail Address should not be blank."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regvalFEmail" runat="server" ControlToValidate="txtFEmail"
                                            Display="None" ErrorMessage="Father Email Address should be in valid format(For Example :\&quot; john.smith@yahoo.com \&quot;)."
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMEmail" runat="server" CssClass="TxtNormalAdmission" MaxLength="200"
                                            TextMode="SingleLine" Columns="20" Rows="4" />
                                        <asp:RegularExpressionValidator ID="regvalMEmail" runat="server" ControlToValidate="txtMEmail"
                                            Display="None" ErrorMessage="Mother Email Address should be in valid format(For Example :\&quot; john.smith@yahoo.com \&quot;)."
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator10" runat="server" ClientValidationFunction="ValidateMotherEmail"
                                            CssClass="ClsMdtStar" Display="None" ErrorMessage="Mother's E-mail Address should not be blank."></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="reqValMotherEmailAddress" runat="server" ControlToValidate="txtMEmail"
                                            Display="None" Enabled="false" ErrorMessage="Mother's E-mail Address should not be blank."></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cmpValFMEmail" runat="server" ErrorMessage="Father's and Mother's Email Address should not be same." Display="None" Enabled="false" ControlToValidate="txtFEmail" ControlToCompare="txtMEmail" Operator="NotEqual"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr id="trFax" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Fax Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFFaxNo" runat="server" MaxLength="10" CssClass="TxtNormalAdmission"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMFaxNo" runat="server" MaxLength="10" CssClass="TxtNormalAdmission"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trSector" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Sector:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFSector" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMSector" runat="server" CssClass="MidTxtNormalAdmission" MaxLength="100" />
                                    </td>
                                </tr>
                                <tr id="trPTAAssociation" runat = "server">
                                    <td colspan="3" class="TxtNormal" style="height: 20px; font-size: 10pt">
                                        Areas in which you would like to participate in the Parent - Teacher Association:
                                    </td>
                                </tr>
                                <tr id="trPTAControls" runat ="server">
                                    <td colspan="3" class="TxtNormal" style="font-size: 10pt">
                                        <asp:CheckBoxList runat="server" ID="chklstEvents" RepeatColumns="3" RepeatDirection="Horizontal" ViewStateMode="Enabled"
                                            class="ClsLabel" />
                                    </td>
                                    <%--<td>
										<asp:CheckBox runat="server" ID="ChkNewsPublicity" Text="Newsletter and Publicity" />
									</td>
									<td>
										<asp:CheckBox runat="server" ID="chkActivities" Text="Co-curricular Activities" />
									</td>
								</tr>
								
								<tr>
									<td>
										<asp:CheckBox runat="server" ID="ChkComputer" Text="Computer / Software" />
									</td>
									<td>
										<asp:CheckBox runat="server" ID="ChkExcursion" Text="Excursions and Visits" />
									</td>
									<td>
										<asp:CheckBox runat="server" ID="ChkSport" Text="Sports" />
									</td>--%>
                                </tr>
                                <tr id="trAdmissionCoordinator" runat="server">
                                    <td class="TxtNormal" style="font-size: 10pt">
                                        Admission Co-ordinator:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAdmissionCoordinator" runat="server" MaxLength="50" Style="width: 200px;"
                                            onblur="formatName(this)" CssClass="TxtNormalAdmission" ViewStateMode="Enabled"></asp:TextBox>                                        
                                        <asp:CustomValidator ID="CustomValidator4" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateCoordinator" ErrorMessage=""></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trSNSSpace" runat="server" visible="true">
                                    <td colspan="4" style="height:10px;">
                                    </td>
                                </tr>
                                <tr id="trSNSBrotherDetails" runat="server" visible="false">
                                    <td class="TxtNormal" style="font-size: 10pt" colspan="5">
                                        <b>Details of Brothers and Sisters of the student:</b>
                                    </td>                                    
                                </tr>
                                <tr id="trSNSBrotherDetails1" runat="server" visible="false">
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td style="width:250px;" class="TxtNormal">
                                                    Name
                                                </td>
                                                <td style="width:50px;" class="TxtNormal">
                                                    Age
                                                </td>
                                                <td style="width:250px;" class="TxtNormal">
                                                    Name of the Institution
                                                </td>
                                                <td style="width:100px;" class="TxtNormal">
                                                    Standard
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:250px;">
                                                    <asp:TextBox ID="txtBName1" runat="server" CssClass="MidTxtNormalAdmission" Width="250px"/>
                                                </td>                                                
                                                <td style="width:50px;">
                                                    <asp:TextBox ID="txtBAge1" runat="server" CssClass="MidTxtNormalAdmission" Width="50px" MaxLength="2"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                                </td>                                                
                                                <td style="width:250px;">
                                                    <asp:TextBox ID="txtBInstitution1" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                                                </td>                                                
                                                <td style="width:100px;">
                                                    <asp:TextBox ID="txtBStandard1" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:250px;">
                                                    <asp:TextBox ID="txtBName2" runat="server" CssClass="MidTxtNormalAdmission" Width="250px"/>
                                                </td>                                                
                                                <td style="width:50px;">
                                                    <asp:TextBox ID="txtBAge2" runat="server" CssClass="MidTxtNormalAdmission" Width="50px" MaxLength="2"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                                </td>                                                
                                                <td style="width:250px;">
                                                    <asp:TextBox ID="txtBInstitution2" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                                                </td>                                                
                                                <td style="width:100px;">
                                                    <asp:TextBox ID="txtBStandard2" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trSNSStrimwiseSubjects" runat="server" visible="false">
                                    <td colspan="4">
                                        <table style="width:100%; text-align:left; margin:0px auto;">
                                            <tr>
                                                <td style="height:10pt;"></td>
                                            </tr>
                                            <tr>
                                               <td class="TxtNormal" style="font-size: 10pt" colspan="5">
                                                    <b>Stream Wise Subject Details.</b>
                                               </td>  
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:Label runat = "server" ID = "Label1">Select Stream :</asp:Label> 
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStream" runat="server" CssClass="TxtNormalAdmission" 
                                                       ViewStateMode="Enabled">
                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="SCIENCE" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="COMMERCE" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="ARTS" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="ABROAD EDUCATION EXAMINATION" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CustomValidator ID="cstStream" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="ValidateStream" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cstStreamDetails" Display="None" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="validateSubjectDetails" ErrorMessage=""></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height:20px;"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table style="text-align:center; margin:0px auto; width:80%;" align="center">                                                        
                                                        <tr id="trScienceStream" style="display:none;">
                                                            <td colspan="4">   
                                                                <table border="1" cellpadding="1" cellspacing="1">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label6"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:280pt;">
                                                                            <asp:Label runat = "server" ID = "Label7"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label8"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:200pt;">
                                                                            <asp:Label runat = "server" ID = "Label9"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria;">
                                                                            <asp:RadioButton ID="rdoStream_SciGroupOne" Text="1" runat="server" GroupName="Science"></asp:RadioButton>                                                                            
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
                                                                            <asp:RadioButton ID="rdoStream_SciGroupTwo" Text="2" runat="server" GroupName="Science"></asp:RadioButton>
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            English, Physics, Chemistry, Biology
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">
                                                                            <asp:RadioButton ID="rdoStream_SciGr2PhyEdu" Text="Physical education" runat="server" Font-Size="12" GroupName="GroupTwo"></asp:RadioButton> <br /><br />
                                                                            <asp:RadioButton ID="rdoStream_SciGr2CompSci" Text="Computer Science" runat="server" GroupName="GroupTwo"></asp:RadioButton>
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">  
                                                                            <asp:CheckBox ID="chkStream_SciGr2Neet" runat="server" CssClass="LblSml" Text="NEET" />  
                                                                            <asp:CheckBox ID="chkStream_SciGr2ExtraCO" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>   
                                                        <tr id="trCommerceStream" style="display:none;">
                                                            <td colspan="4">   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label2"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label3"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label4"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label5"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
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
                                                                            <asp:RadioButton ID="rdoStream_ComPhyEdu" Text="Physical education" runat="server" GroupName="Commerce"></asp:RadioButton>
                                                                         </td>
                                                                         <td class="TxtNormal" style="font-size: 12pt; font-family:Cambria; text-align:left;">    
                                                                            <asp:CheckBox ID="chkStream_ComCA" runat="server" CssClass="LblSml" Text="CA" />
                                                                            <asp:CheckBox ID="chkStream_ComExtraCo" runat="server" CssClass="LblSml" Text="EXTRACOACHING" />                                                                                                                                                                                                                                
                                                                         </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trArtsStream" style="display:none;">
                                                            <td colspan="4">   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label10"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label11"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label12"><b>OPTIONAL SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label13"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
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
                                                        <tr id="trAbroadEducation" style="display:none;">
                                                            <td colspan="4">   
                                                                <table border="1" cellpadding="0" cellspacing="0">
                                                                    <tr>                                            
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:133pt;">
                                                                            <asp:Label runat = "server" ID = "Label14"><b>GROUP</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                            <asp:Label runat = "server" ID = "Label15"><b>COMPULSORY SUBJECTS</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                            <asp:Label runat = "server" ID = "Label16"><b>OPTIONAL SUBJECTS (Select any one)</b></asp:Label> 
                                                                        </td>
                                                                        <td class="TxtNormal" style="font-size: 10pt;">
                                                                            <asp:Label runat = "server" ID = "Label17"><b>COMPETITIVE EXAMS COACHING</b></asp:Label> 
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
                                    <td colspan="4">
                                         <table style="width:100%; text-align:left; margin:0px auto;">
                                            <tr>
                                                <td style="height:10pt;"></td>
                                            </tr>
                                            <tr>
                                               <td class="TxtNormal" style="font-size: 10pt" colspan="5">
                                                    <b>GRADE IX SUBJECT COMBINATION</b>
                                               </td>  
                                            </tr>
                                            <tr>
                                                <td colspan="4">   
                                                    <table border="1" style="text-align:center; width:70%; margin:0px auto;" cellpadding="0" cellspacing="0">
                                                        <tr>                                            
                                                            <td class="TxtNormal" style="font-size: 10pt; width:100pt;">
                                                                <asp:Label runat = "server" ID = "Label18"></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:50pt;">
                                                                <asp:Label runat = "server" ID = "Label22"></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:300pt;">
                                                                <asp:Label runat = "server" ID = "Label19"><b>Compulsory Subjects</b></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                <asp:Label runat = "server" ID = "Label20"><b>Optional Subjects (Select any one)</b></asp:Label> 
                                                            </td>
                                                            <td class="TxtNormal" style="font-size: 10pt; width:250pt;">
                                                                <asp:Label runat = "server" ID = "Label21"><b>Optional Subjects</b></asp:Label> 
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
                            </table>
                        </td>
                    </tr> 
                    <tr id="trImportant" runat="server">
                        <td class="HeadTxtBWOPadding borderBtm" style="height: 25px" align="left" colspan="4">
                            Important:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" class="TxtNormal" style="font-size: 10pt">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trParentConsent" runat="server" visible="false">
                        <td colspan="4" align="left" class="TxtNormal">
                            <asp:CheckBox ID="chkParentConsentForm" runat="server" /><span style="font-weight:bold">Yes, I have read parent consent form, if Not then </span><a id="aParentConsent" runat="server" style="font-weight:bold" href="../DOWNLOADS/AdmissionForms/Parental Consent Form.pdf" target="_blank">Click here to download Parent Consent Form</a>
                        </td>
                    </tr>
                    <tr id="trAssureNotice" runat="server">
                        <td colspan="4" align="left" class="TextNormalB" style="padding-left:5px;">
                            We assure the school that the information provided by us above is true and accept
                            that if any of the information is found to be incorrect in any way the school has
                            right to cancel the admission.
                        </td>
                    </tr>
                    <tr id="trAccept" runat="server">
                        <td colspan="4" align="left" class="TxtNormal" style="height: 23px">
                            <asp:RadioButton ID="rdoAccept" runat="server" GroupName="rdoGroupAccept" CssClass="ClsLabel"
                                Text="I accept" Checked="true" ViewStateMode="Enabled"></asp:RadioButton>
                            <asp:CustomValidator ID="cstDOB" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" EnableClientScript="true" ClientValidationFunction="checkIAccept"
                                ErrorMessage="I accept should be selected."></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="trNoAccept" runat="server">
                        <td colspan="4" align="left" class="TxtNormal">
                            <asp:RadioButton ID="rdoNoAccept" Text="I do not accept" runat="server" CssClass="ClsLabel"
                                GroupName="rdoGroupAccept" Checked="True" ViewStateMode="Enabled"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="TxtNormal">
                            <asp:HiddenField ID="hidServerDate" runat="server" ViewStateMode="Enabled"/>
                            <asp:HiddenField ID="hidFParentID" runat="server" ViewStateMode="Enabled"/>
                            <asp:HiddenField ID="hidMParentID" runat="server" ViewStateMode="Enabled"/>
                            <asp:HiddenField ID="hidNewAcadamicYearID" runat="server" ViewStateMode="Enabled"/>                            
                            <asp:HiddenField ID="hidValidateAdissionCoordinator" runat="server" ViewStateMode="Enabled" Value="N"/>
                            <asp:HiddenField ID="hidShowParentConsentRestriction" runat="server" ViewStateMode="Enabled" Value="N" />
                            <asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled"/>
                            <asp:HiddenField ID="hidSNSSchoolId" runat="server" ViewStateMode="Enabled"/>
                            <asp:HiddenField ID="hidStandardName" runat="server" ViewStateMode="Enabled" Value=""/>
                            <asp:HiddenField ID="hidIsSubjectSectionApplicable" runat="server" ViewStateMode="Enabled" Value="N"/>
                            <asp:HiddenField ID="hidShowAnnualIncomeValidation" runat="server" ViewStateMode="Enabled" Value="N"/>
                            <asp:HiddenField ID="hidShow10thStdValidation" runat="Server" ViewStateMode="Enabled" Value="N"/>
                            <asp:HiddenField ID="hidShowMotherRelatedValidaions" runat="Server" ViewStateMode="Enabled" Value="N"/>
                            <asp:Button runat="server" ID="btnSubmit" Text="Next" CausesValidation="true" CssClass="ClsButton"
                                OnClick="btnSubmit_Click" Enabled="true" />
                            <asp:Button runat="server" ID="btnSiblingSubmit" Text="Submit And Fill Sibling Admission Form"
                                CausesValidation="true" CssClass="ClsButton" OnClick="btnSiblingSubmit_Click"
                                Enabled="true" Visible="false" Width="250px" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
        <br />
    </div>

    <script language="javascript" type="text/javascript">
        var rdoAccept = "<%=this.rdoAccept.ClientID %>";
    _clientchkAddSiblingDetails = "<%=this.chkAddSiblingDetails.ClientID %>";
    _clienttxtSiblingName = "<%=this.txtSiblingName.ClientID %>"
    _clientcmbStandard = "<%=this.cmbStandard.ClientID %>"
    _clientcmbDivision = "<%=this.cmbDivision.ClientID %>"
    _clienttxtAdmissionCoordinator = "<%=this.txtAdmissionCoordinator.ClientID %>"
    _clienthidShowParentConsentRestriction = "<%=this.hidShowParentConsentRestriction.ClientID %>"
    _clientchkParentConsentForm = '<%=this.chkParentConsentForm.ClientID %>'
    _clientcmbStream = "<%=this.cmbStream.ClientID %>"
    _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
    _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
    _clienthidShowAnnualIncomeValidation = "<%=this.hidShowAnnualIncomeValidation.ClientID %>"
    _clienthidIsSubjectSectionApplicable = "<%=this.hidIsSubjectSectionApplicable.ClientID %>"

        function HideSiblingDetails() {
                var ShowSiblingDetails = document.getElementById(_clientchkAddSiblingDetails);
                if (ShowSiblingDetails.checked) {
                    $('#trSiblingDetails').show();
                }
                else {
                    $('#' + _clientcmbStandard).val("0")
                    $('#' + _clientcmbDivision).val("0")
                    $('#' + _clienttxtSiblingName).val("")
                    $('#trSiblingDetails').hide()
                }
            }

            $(document).ready(function () {
                //$('#trSiblingDetails').hide()
                HideSiblingDetails();
            });


            function ValidateSiblingStandard(oSrc, args) {
                var ShowSiblingDetails = document.getElementById(_clientchkAddSiblingDetails).checked
                if(ShowSiblingDetails)
                {
                    if ($('#' + _clientcmbStandard).val() == 0) {
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateSiblingDivision(oSrc, args) {
                var ShowSiblingDetails = document.getElementById(_clientchkAddSiblingDetails).checked
                if (ShowSiblingDetails) {
                    if ($('#' + _clientcmbDivision).val() == 0) {
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateSiblingName(oSrc, args) {
                var ShowSiblingDetails = document.getElementById(_clientchkAddSiblingDetails).checked
                if (ShowSiblingDetails) {
                    if ($('#' + _clienttxtSiblingName).val().trim() == '') {
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function ValidateCoordinator(oSrc, args) {
                if ($('#' + '<%=this.hidValidateAdissionCoordinator.ClientID %>').val() == "1") {
                    if ($('#' + _clienttxtAdmissionCoordinator).val().trim() == "") {
                        oSrc.errormessage = "Admission Co-ordinator should not be blank.";
                        args.IsValid = false;
                        return true;
                    }
                }
                args.IsValid = true;
                return false;
            }

            SetField();
            function SetField() {
                if ($('#' + '<%=this.hidValidateAdissionCoordinator.ClientID %>').val() == "1")
                    $('#' + _clienttxtAdmissionCoordinator).css("backgroundColor", "#ffffa0")                
                else
                    $('#' + _clienttxtAdmissionCoordinator).css("backgroundColor", "white")
            }

            function ValidateStream(oSrc, args) {
                var StreamId = document.getElementById(_clientcmbStream).value;
                var SchoolId = document.getElementById(_clienthidSchoolId).value;
                var SNSSchoolId = document.getElementById(_clienthidSNSSchoolId).value;

                if (SchoolId == SNSSchoolId && StreamId == "0") {
                    oSrc.errormessage = "Stream Should be selected.";
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function ChangeStreamDetails(value) { 
                var StreamId = $(value).val();
                $('[id*=rdoStream_]').prop('checked', false);
                $('[id*=chkStream_]').prop('checked', false);

                if (StreamId == "1") {
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

            function ValidateAnnualIncome(oSrc, args) {
                _clienttxtFIncome = "<%=this.txtFIncome.ClientID %>"
                var AnnualIncome = document.getElementById(_clienttxtFIncome).value;
                var ShowIncomeValidation = document.getElementById(_clienthidShowAnnualIncomeValidation).value;
                
                if (ShowIncomeValidation == "Y") {
                    if (AnnualIncome == "") {
                        oSrc.errormessage = "Father Annual Income should not be blank.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (AnnualIncome == "0") {
                        oSrc.errormessage = "Father Annual Income should be grater than zero.";
                        args.IsValid = false;
                        return true;
                    }
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
                        oSrc.errormessage = "One group for science stream should be select.";
                        args.IsValid = false;
                        return true;
                    }
                    else {
                        if (GroupOneChecked == true) {
                            OptionalSubject1 = $("#<%= rdoStream_SciGr1PhyEdu.ClientID %>").is(":checked");
                            OptionalSubject2 = $("#<%= rdoStream_SciGr1CompSci.ClientID %>").is(":checked");

                            if (OptionalSubject1 == false && OptionalSubject2 == false) {
                                oSrc.errormessage = "Optional Subject should be select.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                        else if (GroupTwoChecked == true) {
                            OptionalSubject1 = $("#<%= rdoStream_SciGr2PhyEdu.ClientID %>").is(":checked");
                            OptionalSubject2 = $("#<%= rdoStream_SciGr2CompSci.ClientID %>").is(":checked");

                            if (OptionalSubject1 == false && OptionalSubject2 == false) {
                                oSrc.errormessage = "Optional Subject should be select.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                    }
                }
                else if (StreamId == 2) {
                    OptionalSubject1 = $("#<%= rdoStream_ComMaths.ClientID %>").is(":checked");
                    OptionalSubject2 = $("#<%= rdoStream_ComPhyEdu.ClientID %>").is(":checked");

                    if (OptionalSubject1 == false && OptionalSubject2 == false) {
                        oSrc.errormessage = "Optional Subject should be select.";
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
                        oSrc.errormessage = "Optional Subject should be select.";
                        args.IsValid = false;
                        return true;
                    }                    
                }
                else if (StreamId == 4) {
                    var True = $("#<%= rdoStream_AbrodEduNo.ClientID %>").is(":checked");
                    var False = $("#<%= rdoStream_AbrodEduYes.ClientID %>").is(":checked");

                    if (True == false && False == false) {
                        oSrc.errormessage = "Competitive exam coaching should be select.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }

            function ConfirmAction() {
                var isValid = ValidateControls()
                if (isValid) {
                    return confirm('After this action you will not be able to see these fields again. Do you want to continue to next step?')
                }
                else {
                    return false;
                }
            }

            _clientcmbMOccupationb = "<%=this.cmbMOccupation.ClientID %>"
            _clienthidShowMotherRelatedValidaions = "<%=this.hidShowMotherRelatedValidaions.ClientID %>"
            _clienttxtMIncome = "<%=this.txtMIncome.ClientID %>"
            _clienttxtMEmail = "<%=this.txtMEmail.ClientID %>"
            _clienttxtMCompany = "<%=this.txtMCompany.ClientID %>"
            _clienttxtMOrgAddress = "<%=this.txtMOrgAddress.ClientID %>"
            _clienttxtMOccDetails = "<%=this.txtMOccDetails.ClientID %>"
            _clienttxtMOffPhone = "<%=this.txtMOffPhone.ClientID %>"

            function ValidateMotherIncome(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMIncome)
                args.IsValid = isValid;
                return !isValid;
            }

            function ValidateMotherCompany(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMCompany)
                args.IsValid = isValid;
                return !isValid;
            }

            function ValidateMotherOfficeAddress(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMOrgAddress)
                args.IsValid = isValid;
                return !isValid;
            }

            function ValidateMotherDesignation(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMOccDetails)
                args.IsValid = isValid;
                return !isValid;
            }

            function ValidateMotherOfficeTelNo(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMOffPhone)
                args.IsValid = isValid;
                return !isValid;
            }

            function ValidateMotherEmail(Osrc, args) {                
                var isValid = IsValidField(_clienttxtMEmail)
                args.IsValid = isValid;
                return !isValid;
            }
            
            function IsValidField(ctrlId) {
                var nm = $("#" + _clientcmbMOccupationb + " option:selected").text();
                var validate = $('#' + _clienthidShowMotherRelatedValidaions).val()

                if (validate == 'Y') {
                    if (nm == 'House Wife') {
                        return true;
                    }
                    else {
                        if ($('#' + ctrlId).val().trim() == '') {
                            return false;
                        }
                        else
                            return true;
                    }
                }
                else
                    return true;
            }

            function SetMotherRelatedFields() {
                var validate = $('#' + _clienthidShowMotherRelatedValidaions).val()

                if (validate == 'Y') {
                    var nm = $("#" + _clientcmbMOccupationb + " option:selected").text();
                    if (nm == 'House Wife') {
                        $('#' + _clienttxtMIncome).css("backgroundColor", "white")
                        $('#' + _clienttxtMEmail).css("backgroundColor", "white")
                        $('#' + _clienttxtMCompany).css("backgroundColor", "white")
                        $('#' + _clienttxtMOrgAddress).css("backgroundColor", "white")
                        $('#' + _clienttxtMOccDetails).css("backgroundColor", "white")
                        $('#' + _clienttxtMOffPhone).css("backgroundColor", "white")
                    }
                    else {
                        $('#' + _clienttxtMIncome).css("backgroundColor", "#ffffa0")
                        $('#' + _clienttxtMEmail).css("backgroundColor", "#ffffa0")
                        $('#' + _clienttxtMCompany).css("backgroundColor", "#ffffa0")
                        $('#' + _clienttxtMOrgAddress).css("backgroundColor", "#ffffa0")
                        $('#' + _clienttxtMOccDetails).css("backgroundColor", "#ffffa0")
                        $('#' + _clienttxtMOffPhone).css("backgroundColor", "#ffffa0")
                    }
                }
            }

    </script>
    <script src="../Scripts/Admission/AdmissionFormParentDetails.js?version=1.0" type="text/javascript"></script>
</asp:Content>
