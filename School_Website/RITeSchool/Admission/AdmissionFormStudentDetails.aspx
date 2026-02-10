<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmissionNew.master"
    AutoEventWireup="true" CodeFile="AdmissionFormStudentDetails.aspx.cs" Inherits="AdmissionFormStudentDetails"
    ViewStateMode="Enabled" Title="Admission process" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
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
            <b class="rtop"></b>
            <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0"
                width="100%">
                <tbody>
                    <tr>
                        <td class="HeadTxtBWOPadding borderBtm" align="left" colspan="2">
                            Admission Form
                            <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="false"
                                CssClass="ClsMdtStar" Font-Bold="false" ShowSummary="true" />
                            <asp:CustomValidator ID="cstBlackListStudent" runat="server" ErrorMessage="Error occurred while submitting form." Font-Bold="true" Display="None"
                             OnServerValidate="BlackListStudent_Validate"></asp:CustomValidator>  
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
                    <tr>
                        <td align="left" class="TxtNormal" style="width: 225px">
                            <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                Width="150px" />
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
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt; width: 250px;">
                                        For Academic Year:
                                    </td>
                                    <td align="left" style="width: 300px;">
                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="TxtBox" AutoPostBack="true"
                                            ViewStateMode="Enabled" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="cmp_valYear" runat="server" ControlToValidate="cmbYear"
                                            Display="None" ErrorMessage="Academic Year should be selected." Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt; width: 250px;">
                                        Admission sought for:&nbsp;
                                    </td>
                                    <td align="left">
                                        <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                <ContentTemplate>--%>
                                        <asp:DropDownList ID="cmbStd" runat="server" CssClass="TxtBox" Enabled="False" Visible="False"
                                            AutoPostBack="false" BackColor="#ffffa0" onchange="StandardOnChangeHandler(this);"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblStdName" runat="server" Font-Bold="true"></asp:Label>
                                        <asp:CompareValidator ID="cmp_valStdr" runat="server" ControlToValidate="cmbStd"
                                            Display="None" ErrorMessage="Admission sought for standard should be selected."
                                            Operator="NotEqual" ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:HiddenField ID="hidMinBdate" runat="server" />
                                        <asp:HiddenField ID="hidMaxBdate" runat="server" />
                                        <%--</ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr id="tr1">
                                    <td align="left" class="TxtNormal" valign="top" style="font-size: 10pt">
                                        Student's Name:&nbsp;
                                    </td>
                                    <td align="left" colspan="3">
                                        <table align="left" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSLastName" MaxLength="50" onblur="formatName(this)"
                                                        onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:CustomValidator ID="cstLastName" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ClientValidationFunction="ValidateStudentName" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqValStudLastName" runat="server" Enabled="false"
                                                        ErrorMessage="Student's Last Name should not be blank." CssClass="ClsMdtStar"
                                                        Display="None" ControlToValidate="txtSLastName"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtSName" MaxLength="50" BackColor="#ffffa0"
                                                        onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:RequiredFieldValidator ID="reqSName" runat="server" ErrorMessage="Student's First Name should not be blank."
                                                        Display="None" ControlToValidate="txtSName"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFahterName" MaxLength="50" onblur="formatName(this)"
                                                        onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:CustomValidator ID="cstValMiddleName" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ClientValidationFunction="ValidateMiddleName" ErrorMessage="" Enabled="false"></asp:CustomValidator>
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
                                                    (Middle Name)
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trAadharNameNote">
                                                <td colspan="3">
                                                  <span id="spnAadharNameNote" runat="server">(Student name as per Birth Certificate / Aadhar Card)</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Gender:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:RadioButton ID="rdoMale" Text="Male" runat="server" GroupName="rdoGroupSex"
                                            CssClass="ClsLabel" Checked="True" ViewStateMode="Enabled"></asp:RadioButton>
                                        <asp:RadioButton ID="rdoFemale" Text="Female" runat="server" GroupName="rdoGroupSex"
                                            CssClass="ClsLabel" ViewStateMode="Enabled"></asp:RadioButton>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Mother Tongue:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMotherTongue" MaxLength="20" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                        <asp:RequiredFieldValidator ID="reqValMotherTongue" runat="server" Enabled="false"
                                            ErrorMessage="Mother Tongue should not be blank." CssClass="ClsMdtStar" Display="None"
                                            ControlToValidate="txtMotherTongue"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trSPSBirthDetails" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Birth Taluka:
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSPSBirthTaluka" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Birth District:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtSPSBirthDistrict" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Nationality:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox runat="server" CssClass="TxtBox" ID="txtNationality" MaxLength="50" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                        <asp:RequiredFieldValidator ID="reqValNationality" runat="server" Enabled="false"
                                            ErrorMessage="Nationality should not be blank." CssClass="ClsMdtStar" Display="None"
                                            ControlToValidate="txtNationality"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Religion:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbReligion" runat="server" CssClass="TxtBox" ViewStateMode="Enabled">
                                        </asp:DropDownList>
                                        <asp:CustomValidator ID="cstReligion" runat="server" ClientValidationFunction="ValidateReligion"
                                            Display="None" ValidateEmptyText="true" ControlToValidate="cmbReligion"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Caste/Sub-caste:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtCasteAndSubcaste" runat="server" CssClass="TxtBox" MaxLength="50" />
                                        <asp:CustomValidator ID="cstCastAndSubcast" runat="server" ClientValidationFunction="ValidateCastAndSubcast"
                                            Display="None" ValidateEmptyText="true" ControlToValidate="txtCasteAndSubcaste"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="reqValCaste" runat="server" Enabled="false" ErrorMessage="Caste/Sub-caste should not be blank."
                                            CssClass="ClsMdtStar" Display="None" ControlToValidate="txtCasteAndSubcaste"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Category:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="TxtBox" ViewStateMode="Enabled"
                                            BackColor="#ffffa0">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Category should be selected."
                                            ControlToValidate="cmbCategory" ValueToCompare="0" Type="Integer" Operator="NotEqual"
                                            Display="None"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr id="trCasteCert" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                            <asp:Label ID="Label12" runat="server" Text="Caste Certificate (if applicable)"></asp:Label>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                            <asp:FileUpload ID="flUploadCastCert" runat="server" ViewStateMode="Inherit" />
                                            <asp:ImageButton ID="imgCastCert" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                            <asp:CustomValidator ID="cstValCasteCertFile" runat="server" ControlToValidate="FilUpImg" OnServerValidate="CasteCertFIle_ServerValidate"
                                                ClientValidationFunction="ValidateCasteCertFile" Display="None" ValidateEmptyText="True" Enabled="false"></asp:CustomValidator>
                                            <asp:HiddenField ID="hidCasteCertFileName" runat="server" Value="" />        
                                    </td>                                  
                                    <td colspan="2">
                                        <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 1 MB.)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Aadhar Card Number:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtAadharCardNo" runat="server" CssClass="TxtBox" MaxLength="12"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />
                                        <asp:CustomValidator ID="cstAadharCard" runat="server" ClientValidationFunction="ValidateAadharCard"
                                            Display="None" ValidateEmptyText="true" ControlToValidate="txtAadharCardNo"></asp:CustomValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Name as per Aadhar Card :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtNameOnAadharCard" runat="server" CssClass="ExLrgTxtBox" MaxLength="150" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                        <asp:RequiredFieldValidator ID="reqValNameAsPerAadhar" runat="server" ErrorMessage="Name on Aadhar Card should not be blank."
                                            ControlToValidate="txtNameOnAadharCard" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trAadharNote" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdAadharHeader" runat="server">
                                        <asp:Label ID="lblAadharCard" runat="server"></asp:Label>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdAadharData" runat="server">
                                        <asp:FileUpload ID="FilUpImg" runat="server" CssClass="LrgTxtBox" ViewStateMode="Inherit"
                                            Width="200px" />
                                        <asp:ImageButton ID="btnView1" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                            ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                        <asp:CustomValidator ID="cstValidateFileUpload" runat="server" ControlToValidate="FilUpImg"
                                            ClientValidationFunction="FileUploadValidation" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                    <td colspan="2" align="left">
                                        <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File
                                            size should not exceed 1MB.)&nbsp; &nbsp;&nbsp;</span>
                                    </td>                                    
                                </tr>                                
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdSaralNoH" runat="server" >
                                        Saral Number:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdSaralNoData" runat="server">
                                        <asp:TextBox ID="txtSaralNo" runat="server" CssClass="TxtBox" MaxLength="20" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Language Known :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtLanguageKnown" runat="server" CssClass="TxtBox" Width="250px" />
                                        <asp:RequiredFieldValidator ID="reqValLangKnown" runat="server" Display="None" Enabled="false"
                                            ErrorMessage="Language Known should not be blank." ControlToValidate="txtLanguageKnown"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Only Child:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:RadioButtonList ID="rdoOnlyChild" runat="server" RepeatColumns="4" CssClass="ClsLabel"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Minority :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:RadioButtonList ID="rdoMinority" runat="server" RepeatColumns="4" CssClass="ClsLabel"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Text="Yes" Value="1" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Second Language :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbSecondSLanguageSubjectId" runat="server" CssClass="TxtBox"
                                            ViewStateMode="Enabled" Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValSecondLanguage" runat="server" ErrorMessage="Second Language should be selected."
                                            Display="None" Enabled="false" ControlToValidate="cmbSecondSLanguageSubjectId"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Third Language :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbThirdLanguage" runat="server" CssClass="TxtBox" ViewStateMode="Enabled"
                                            Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValThirdLanguage" runat="server" ErrorMessage="Third Language should be selected."
                                            Display="None" Enabled="false" ControlToValidate="cmbThirdLanguage" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cstValThirdLanguage" runat="server" ErrorMessage="" Display="None"
                                            ClientValidationFunction="ValidateLanguages" Enabled="false"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Student Blood Group :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbStudentBloodGroup" runat="server" CssClass="TxtBox" ViewStateMode="Enabled"
                                            Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValBloodGrp" runat="server" ErrorMessage="Student Blood Group should be selected."
                                            Display="None" Enabled="false" ControlToValidate="cmbStudentBloodGroup" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdPrfBatchHeader" runat="server" visible="false">
                                        Preference Batch :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" id="tdPrfBatch" runat="server" visible="false">
                                        <asp:DropDownList ID="cmbPreferenceBatch" runat="server" CssClass="TxtBox" ViewStateMode="Enabled" Width="150px">
                                            <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Morning" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Afternoon" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqValPrfBatch" runat="server" ErrorMessage="Preference Batch should be selected."
                                            Display="None" Enabled="false" ControlToValidate="cmbPreferenceBatch" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>                                
                                <tr id="trStudentPhoto" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Student Photo :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:FileUpload ID="flStudentPhoto" runat="server" ViewStateMode="Enabled" />
                                        <asp:CustomValidator ID="cstValidateStudentPhoto" Display="None" runat="server" Enabled="false"
                                            ClientValidationFunction="ValidateStudentPhotoFile" ErrorMessage="InvalidFileFormat"
                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                    </td>
                                    <td colspan="2">
                                        <asp:Image ID="imgPhoto" runat="server" Height="151" Width="119" Visible="false" />
                                    </td>
                                </tr>
                                <tr id="trStudentPhotoNote" runat="server">                                   
                                    <td align="left" colspan="4">
                                        <span class="LblSmlGray">
                                            <asp:Label ID="Label10" runat="server" EnableViewState="False" Text="(Photo with plain white background)"></asp:Label><br />
                                            <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="(Max Height: 151px and Max Width: 112px)"></asp:Label><br />
                                            <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="(Image size should not exceed 1 mb. Supported file formats are JPG, JPEG, PNG, BMP)"></asp:Label>
                                        </span>
                                    </td>
                                </tr>                                
                                <tr id="trSPSPassport" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Passport No. :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtPassportNo" runat="server" CssClass="TxtBox" />
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Passport Expire On Date :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtDateOfExpiry" runat="server" CssClass="SmlTxtBox" />
                                        <rjs:PopCalendar ID="PopCalendar5" runat="server" Control="txtDateOfExpiry" Format="dd MMM yyyy"
                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth." />
                                    </td>
                                </tr>
                                <tr id="trSPSMarriageAnniversary" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Marriage Anniversary Date :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtMarriageAnniversary" runat="server" CssClass="SmlTxtBox" />
                                        <rjs:PopCalendar ID="PopCalendar6" runat="server" Control="txtMarriageAnniversary"
                                            Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                            To-Today="true" />
                                        <asp:CustomValidator ID="cstAnniversaryDate" Display="None" runat="server" ClientValidationFunction="ValidateAnniversaryDate"
                                            ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="txtMarriageAnniversary"
                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Family Income :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtFamilyIncome" runat="server" CssClass="SmlTxtBox" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px;">
                                    </td>
                                </tr>
                                <tr id="trSPSAdopted" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Is The Student An Adopted Child :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:CheckBox ID="chkIsAdoptedChild" runat="server" CssClass="ClsLabel" />
                                    </td>
                                </tr>
                                <tr id="trSPSResponsible" runat="server" visible="false">
                                    <%-- <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                If Yes Who Is Financially Responsible For The Child :
                                            </td>                        
                                            <td class="TxtNormal" align="left" style="font-size: 10pt"> 
                                                <asp:TextBox ID="txtFinancialResponsible" runat="server" MaxLength="50" Style="width: 200px;"
                                                            onblur="formatName(this)" CssClass="TxtNormalAdmission" ViewStateMode="Enabled"></asp:TextBox>
                                            </td>--%>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Financially Responsible :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:RadioButton ID="rdoFRFather" Text="Father" runat="server" CssClass="ClsLabel"
                                            GroupName="Financially" Checked="True"></asp:RadioButton>
                                        <asp:RadioButton ID="rdoFRMother" Text="Mother" runat="server" CssClass="ClsLabel"
                                            GroupName="Financially"></asp:RadioButton>
                                        <asp:RadioButton ID="rdoFRGuardian" Text="Guardian" runat="server" CssClass="ClsLabel"
                                            GroupName="Financially"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr id="trSPSLivingWith" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Student Is Living With :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <%--<asp:RadioButton ID="rdoBothParent" Text="Both Parents" runat="server" GroupName="rdoLiving"
                                                 CssClass="ClsLabel" ViewStateMode="Enabled"></asp:RadioButton>--%>
                                        <asp:RadioButton ID="rdoFather" Text="Father" runat="server" GroupName="rdoLiving"
                                            CssClass="ClsLabel" ViewStateMode="Enabled" Checked="true"></asp:RadioButton>
                                        <asp:RadioButton ID="rdoMother" Text="Mother" runat="server" GroupName="rdoLiving"
                                            CssClass="ClsLabel" ViewStateMode="Enabled"></asp:RadioButton>
                                        <asp:RadioButton ID="rdoLocalGuardian" Text="Guardian" runat="server" GroupName="rdoLiving"
                                            CssClass="ClsLabel" ViewStateMode="Enabled"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr id="EmergancyDetails" runat="server">
                                    <td colspan="4">
                                        <table align="left" style="text-align: left;">
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Emergency Contact No. :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmergancyContact" runat="server" CssClass="TxtBox" onblur="extractNumber(this,0,false);"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                </td>
                                                <td style="width: 5px;">
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Name Of the person to be Contacted :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPersonToContacted" runat="server" CssClass="TxtBox" Width="250px" />
                                                </td>
                                                <td style="width: 5px;">
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Relationship :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRelationship" runat="server" CssClass="TxtBox" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trPersonalMarks" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Personal Marks of Identification :
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtFirstPersonalMark" runat="server" MaxLength="50" Style="width: 200px;"
                                            onblur="formatName(this)" CssClass="TxtNormalAdmission" ViewStateMode="Enabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trFirstPersonalMarks" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtSecondPersonalMark" runat="server" MaxLength="50" Style="width: 200px;"
                                            onblur="formatName(this)" CssClass="TxtNormalAdmission" ViewStateMode="Enabled"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td class="TextNormalB borderBtm" style="height: 15px" align="left" colspan="4">
                                        Last School Details
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        School Name:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="3" style="font-size: 10pt">
                                        <asp:TextBox ID="txtSchoolName" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                            onkeypress="return AllowOnlyNameFormat(event)" Width="90%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Previous School Address:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="3" style="font-size: 10pt">
                                        <asp:TextBox ID="txtPreviousSchoolAddress" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                            Width="90%" />
                                    </td>
                                </tr>
                                <tr id="trPreSchoolSaralId" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt; width: 250px;" runat="server" id="tdLastSchoolStudSaralId"> 
                                        <asp:Label ID="lblLastSchoolStudSaralId" runat="server" Text="Previous School Saral Id :"></asp:Label>
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="1" rowspan="1" style="font-size: 10pt;
                                        width: 300px;" runat="server" id="tdLastSchoolStudSaralIddata">
                                        <asp:TextBox ID="txtPreviousSchoolSaralId" runat="server" CssClass="TxtBox" MaxLength="19"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />
                                        <asp:CustomValidator ID="cstPreviousSchoolSaral" runat="server" ClientValidationFunction="PreviousSchoolSaralValidation"
                                            Display="None"></asp:CustomValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt; width: 300px;">
                                        Previous School U-DISE No.:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt" >
                                        <asp:TextBox ID="txtPreviousSchoolUDISENo" runat="server" CssClass="LrgTxtBox" MaxLength="11"
                                            Width="80%" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />
                                        <asp:CustomValidator ID="cstSchoolUDISE" runat="server" ControlToValidate="txtPreviousSchoolUDISENo"
                                            ClientValidationFunction="ValidateSchoolUDISE" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trPreviousStandard" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Standard:&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt">
                                        <asp:TextBox ID="txtLastStd" runat="server" CssClass="TxtBox" MaxLength="50" />
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Last School Phone No with STD code:
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt">
                                        <asp:TextBox ID="txtLastSchoolPhone" runat="server" CssClass="TxtBox" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr id="trLastSchoolBoard" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        School Board Name:
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt;">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rdolstlastSchoolBoard" runat="server" RepeatColumns="4"
                                                        ViewStateMode="Enabled">
                                                        <asp:ListItem Text="ICSE" Value="ICSE" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="CBSE" Value="CBSE" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="SSC" Value="SSC" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="OTHERS" Value="OTHERS" Selected="False"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Is School out of Maharashtra?:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="1" rowspan="1" style="font-size: 10pt">
                                        <asp:CheckBox ID="chkIsSchoolFromOutOfState" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trRecognised" runat="server">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Recognised:
                                    </td>
                                    <td class="TxtNormal" align="left" colspan="1" style="font-size: 10pt">                                       
                                        <asp:RadioButtonList ID="rdolstIsRecognised" runat="server" RepeatColumns="4" CssClass="ClsLabel"
                                            ViewStateMode="Enabled">
                                            <asp:ListItem Text="YES" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="0" Selected="False"></asp:ListItem>
                                        </asp:RadioButtonList>                                       
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ClientValidationFunction="ValidateLastSchoolName"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ClientValidationFunction="ValidateLastSchoolAddress"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ClientValidationFunction="ValidateLastSchoolUDISE"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ClientValidationFunction="ValidateLastSchoolStandard"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator8" runat="server" ClientValidationFunction="ValidateLastSchoolBoard"
                                            Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValidate10thDetails" runat="server" ClientValidationFunction="Validate10thDetails"
                                            Display="None"></asp:CustomValidator>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        Permanent Education Number (PEN No.) :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="3">
                                        <asp:TextBox ID="txtPenNo" CssClass="TxtBox" runat="server" MaxLength="11" onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"/>
                                        <asp:RequiredFieldValidator ID="reqValPenNo" runat="server" ErrorMessage="Permanent Education Number should not be blank." Display="None" Enabled="false" ControlToValidate="txtPenNo"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        APAAR ID :&nbsp;
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt" colspan="3">
                                        <asp:TextBox ID="txtApaarId" CssClass="TxtBox" runat="server" MaxLength="12" onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"/> 
                                          <asp:RequiredFieldValidator ID="reqValAparId" runat="server" ErrorMessage="APAAR ID should not be blank." Display="None" Enabled="false" ControlToValidate="txtApaarId"></asp:RequiredFieldValidator>
                                       
                                    </td>
                                </tr>
                                <tr id="trSNS10thStdDetails" runat="server" visible="false">
                                    <td align="left" colspan="4">
                                        <table>
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt; width: 230px;">
                                                    Std. X Board :
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    <asp:TextBox ID="txt10Board" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                        Width="250pt" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Std. X Roll No. :
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    <asp:TextBox ID="txt10RollNo" runat="server" CssClass="LrgTxtBox" MaxLength="200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Std. X Exam :
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    <asp:TextBox ID="txt10Exam" runat="server" CssClass="LrgTxtBox" MaxLength="200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Std. X Year Of Passing :
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    <asp:TextBox ID="txt10PassingYear" runat="server" CssClass="LrgTxtBox" MaxLength="200" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    Basic / Standard Mathematics :
                                                </td>
                                                <td class="TxtNormal" align="left" style="font-size: 10pt">
                                                    <asp:TextBox ID="txt10thMaths" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                        Width="250pt" />
                                                </td>
                                            </tr>
                                        </table>
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
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="left" class="TextNormalB borderBtm" colspan="4" style="height: 15px">
                                        Parent's Details
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" valign="top" style="font-size: 10pt; width: 250px;">
                                        Father's / Guardian's Name:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="3" style="font-size: 10pt">
                                        <table cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFSurname" MaxLength="50" onblur="formatName(this)"
                                                        onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:CustomValidator ID="cstValidateFatherName" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ClientValidationFunction="ValidateFatherName" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqValFLastName" runat="server" Enabled="false" ErrorMessage="Father's / Guardian's Last Name should not be blank."
                                                        CssClass="ClsMdtStar" Display="None" ControlToValidate="txtFSurname"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtFName" MaxLength="50" BackColor="#ffffa0"
                                                        onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:RequiredFieldValidator ID="reqFName" runat="server" ErrorMessage="Father's First Name should not be blank."
                                                        Display="None" ControlToValidate="txtFName"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFFatherName" MaxLength="50"
                                                        onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                                    <asp:RequiredFieldValidator ID="reqValFatherFName" runat="server" ErrorMessage="Father's Middle Name should not be blank."
                                                        Display="None" ControlToValidate="txtFFatherName" Enabled="false"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtFAge" Width="50px" MaxLength="2"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <asp:RequiredFieldValidator ID="reqValtxtFAge" runat="server" ErrorMessage="Father's / Guardian's age should not be blank." Display="None" Enabled="false" ControlToValidate="txtFAge"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="compValFAge" runat="server" 
                                                        ErrorMessage="Father's / Guardian's age should be greater than zero." 
                                                        Display="None" Enabled="false" ControlToValidate="txtFAge" Operator="NotEqual" 
                                                        Type="Integer" ValueToCompare="0"></asp:CompareValidator>
                                                </td>
                                                <td style="padding-left:20px;" id="tdFatherAadharCard" runat="server" visible="false">
                                                    <asp:FileUpload ID="flUploadFatherAaadhar" runat="server" ViewStateMode="Enabled" />
                                                    <asp:ImageButton ID="imgFatherAadhar" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                        ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                    <asp:CustomValidator ID="cstValFatherAadharFile" runat="server" ControlToValidate="flUploadFatherAaadhar" Enabled="false"
                                                        ClientValidationFunction="ValidateFatherAadharFile" OnServerValidate="FatherAadharFile_ServerValidate" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                    <asp:HiddenField ID="hidFatherAadharCardFileName" runat="server" Value="" />
                                                    <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type with size upto 1 MB.)</span>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trFatherAadharName">
                                                <td colspan="3" align="center">
                                                    <span class="LblSmlGray" id="spnFatherAadharName" runat="server">----------------------------As per student Birth Certificate
                                                        / Aadhar Card----------------------------</span>
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
                                                    (Father's Name)
                                                </td>
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Age)
                                                </td>
                                                <td style="padding-left:20px;" id="tdAadharCardHeaderFather" runat="server" visible="false">
                                                    (Aadhar Card Copy)
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" valign="top" style="font-size: 10pt">
                                        Mother's Name:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="3" style="font-size: 10pt">
                                        <table cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMSurname" MaxLength="50" onblur="formatName(this)"
                                                        onkeypress="return AllowOnlyNameFormat(event)" ViewStateMode="Enabled" />
                                                    <asp:CustomValidator ID="cstValidateMotherName" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        ClientValidationFunction="ValidateMotherName" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqValmLastName" runat="server" Enabled="false" ErrorMessage="Mother's Last Name should not be blank."
                                                        CssClass="ClsMdtStar" Display="None" ControlToValidate="txtMSurname"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBoxMand" ID="txtMName" MaxLength="50" BackColor="#ffffa0"
                                                        onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" ViewStateMode="Enabled" />
                                                    <asp:RequiredFieldValidator ID="reqMName" runat="server" ErrorMessage="Mother's First Name should not be blank."
                                                        Display="None" ControlToValidate="txtMName"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMHName" MaxLength="50" onblur="formatName(this)"
                                                        onkeypress="return AllowOnlyNameFormat(event)" ViewStateMode="Enabled" />
                                                    <asp:RequiredFieldValidator ID="reqValMotherHName" runat="server" ErrorMessage="Mother's Middle Name should not be blank."
                                                        Display="None" ControlToValidate="txtMHName" Enabled="false"> </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="TxtNormal" style="font-size: 10pt">
                                                    <asp:TextBox runat="server" CssClass="TxtBox" ID="txtMAge" Width="50px" MaxLength="2"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <asp:RequiredFieldValidator ID="reqValMotherAge" runat="server" ErrorMessage="Mother's age should not be blank." Display="None" Enabled="false" ControlToValidate="txtMAge"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cmpValMAge" runat="server" 
                                                        ErrorMessage="Mother's age should be greater than zero." 
                                                        Display="None" Enabled="false" ControlToValidate="txtMAge" Operator="NotEqual" 
                                                        Type="Integer" ValueToCompare="0"></asp:CompareValidator>
                                                </td>
                                                <td style="padding-left:20px;" id="tdMotherAadharCard" runat="server" visible="false">
                                                    <asp:FileUpload ID="flUploadMotherAaadhar" runat="server" ViewStateMode="Enabled" />
                                                    <asp:ImageButton ID="imgMotherAadhar" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                        ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                    <asp:CustomValidator ID="cstValMotherAadharFile" runat="server" ControlToValidate="flUploadMotherAaadhar" OnServerValidate="MotherAadharFile_ServerValidate"
                                                        ClientValidationFunction="ValidateMotherAadharFile" Display="None" ValidateEmptyText="True" Enabled="false"></asp:CustomValidator>
                                                    <asp:HiddenField ID="hidMotherAadharCardFileName" runat="server" Value="" />
                                                    <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type with size upto 1 MB.)</span>
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
                                                <td class="TxtNormal" align="center" style="font-size: 10pt">
                                                    (Age)
                                                </td>
                                                <td style="padding-left:20px;" id="tdAadharCardHeaderMother" runat="server" visible="false">
                                                    (Aadhar Card Copy)
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trMotherAadharName">
                                                <td colspan="3" align="center">
                                                    <span class="LblSmlGray" id="spnMotherAadharName" runat="server">----------------------------As per student Birth Certificate
                                                        / Aadhar Card----------------------------</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr id="trResidenceTypeHeader" runat="server">
                                    <td class="TextNormalB borderBtm" style="height: 15px" align="left" colspan="4">
                                        Preference of Admission
                                    </td>
                                </tr>
                                <tr id="trResidenceType" runat="server">
                                    <td align="left" class="TxtNormal" rowspan="1" style="width:250px;">
                                        <asp:Label ID="lblResidenceType" runat="server" Text="Residence Type:"></asp:Label>
                                    </td>
                                    <td style="padding-top:5px;">
                                        <asp:DropDownList ID="cmbResidenceType" runat="server" CssClass="MidCombo" ViewStateMode="Enabled"
                                            AutoPostBack="false" Width="150px">
                                        </asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator9" runat="server" ClientValidationFunction="ValidateResidenceType"
                                            Display="None" ValidateEmptyText="true"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                    
                    <%-- <tr style="height:15px;">
                    <td>
                    </td>
                    </tr>--%>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td class="TextNormalB borderBtm" style="height: 15px" align="left" colspan="4">
                                        Address Details
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TxtNormal" align="left" rowspan="3" style="font-size: 10pt; width: 250px;">
                                        <asp:Label ID="lblAddress" runat="server" Text="Residential Address of Parents:"></asp:Label>
                                    </td>
                                    <td class="TxtNormal" align="left" rowspan="3" style="font-size: 10pt; width: 300px;">
                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="TxtBoxMand" MaxLength="300"
                                            TextMode="MultiLine" Columns="21" Rows="4" Width="200px" BackColor="#ffffa0"
                                            ViewStateMode="Enabled" />
                                        <asp:RequiredFieldValidator ID="reqAddress" runat="server" ErrorMessage="Address should not be blank."
                                            Display="None" ControlToValidate="txtAddress"> </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regAddress" runat="server" ControlToValidate="txtAddress"
                                            Display="None" ErrorMessage="Address should not exceed than 300 characters."
                                            ValidationExpression="^[\s\S]{0,300}$"></asp:RegularExpressionValidator>
                                    </td>
                                    <td id="tdCity" runat="server" class="TxtNormal" align="left" style="font-size: 10pt;
                                        width: 250px;">
                                        City:&nbsp;
                                    </td>
                                    <td id="tdtxtCity" runat="server" class="TxtNormal" align="left" style="font-size: 10pt;">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="TxtBoxMand" MaxLength="50" BackColor="#ffffa0"
                                            ViewStateMode="Enabled" />
                                        <asp:RequiredFieldValidator ID="reqCity" runat="server" ErrorMessage="City should not be blank."
                                            Display="None" ControlToValidate="txtCity"> </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" style="height: 22px; font-size: 10pt">
                                        State:
                                    </td>
                                    <td align="left" class="TxtNormal" style="height: 22px; font-size: 10pt" colspan="3">
                                        <asp:TextBox ID="txtState" runat="server" CssClass="TxtBoxMand" MaxLength="50" BackColor="#ffffa0" />
                                        <asp:RequiredFieldValidator ID="reqState" runat="server" ErrorMessage="State should not be blank."
                                            Display="None" ControlToValidate="txtState"> </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trPincode" runat="server">
                                    <td align="left" class="TxtNormal" style="font-size: 10pt">
                                        Pincode:
                                    </td>
                                    <td align="left" class="TxtNormal" style="font-size: 10pt" colspan="3">
                                        <asp:TextBox ID="txtPincode" runat="server" CssClass="TxtBoxMand" MaxLength="6" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" BackColor="#ffffa0" />
                                        <asp:RequiredFieldValidator ID="reqPinCode" runat="server" Display="None" ErrorMessage="Pincode should not be blank."
                                            ControlToValidate="txtPincode" CssClass="ClsMdtStar"></asp:RequiredFieldValidator><asp:CustomValidator
                                                Display="None" CssClass="ClsMdtStar" ErrorMessage="Pincode should be of 6 digits."
                                                ID="cst_PIN" runat="server" ClientValidationFunction="PinCodeValidation"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trSPSEmpty" runat="server" visible="false">
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trSPSPermanentAddress" runat="server" visible="false">
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:Label ID="Label8" runat="server" Text="Permanent Address :"></asp:Label>
                                    </td>
                                    <td class="TxtNormal" align="left" style="font-size: 10pt">
                                        <asp:TextBox ID="txtPermanentAddress" runat="server" CssClass="TxtBox" MaxLength="300"
                                            TextMode="MultiLine" Columns="21" Rows="4" Width="200px" ViewStateMode="Enabled" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Address should not be blank."
                                            Display="None" ControlToValidate="txtAddress"> </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAddress"
                                            Display="None" ErrorMessage="Address should not exceed than 300 characters."
                                            ValidationExpression="^[\s\S]{0,300}$"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                                        Phone (Residence):
                                    </td>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                                        <asp:TextBox ID="txtRPhone" runat="server" CssClass="TxtBox" MaxLength="20" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                        <asp:RequiredFieldValidator ID="reqValtxtRPhone" runat="server" ErrorMessage="Phone (Res) should not be blank."
                                            Display="None" Enabled="false" ControlToValidate="txtRPhone"></asp:RequiredFieldValidator>
                                    </td>
                                    <td id="tdMobileNo" runat="server" align="left" class="TxtNormal" style="font-size: 10pt">
                                        <span id="spnFatherMobileNo" runat="server">Mobile Number1:</span>
                                    </td>
                                    <td id="tdtxtMobileNo" runat="server" align="left" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="TxtBoxMand" MaxLength="10" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" BackColor="#ffffa0" />
                                        <asp:RequiredFieldValidator ID="reqMobileNo" runat="server" Display="None" ErrorMessage="Mobile number1 should not be blank."
                                            CssClass="ClsMdtStar" ControlToValidate="txtMobile"></asp:RequiredFieldValidator><asp:CustomValidator
                                                ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar" Visible="true"
                                                ErrorMessage="Mobile number1 should be of 10 digits." EnableClientScript="true"
                                                ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                                        Phone (Office):
                                    </td>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                                        <asp:TextBox ID="txtOPhone" runat="server" CssClass="TxtBox" MaxLength="20" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                    </td>
                                    <td id="tdMobileNo2" runat="server" align="left" class="TxtNormal" style="font-size: 10pt">
                                        <span id="spnMotherMobileNo" runat="server">Mobile Number2:</span>
                                    </td>
                                    <td id="tdtxtMobileNo2" runat="server" align="left" class="TxtNormal" style="font-size: 10pt">
                                        <asp:TextBox ID="txtMobile2" runat="server" CssClass="TxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                        <asp:RequiredFieldValidator ID="reqmobileno2" runat="server" Display="None" ErrorMessage="Mobile number2 should not be blank."
                                            CssClass="ClsMdtStar" ControlToValidate="txtMobile2"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cst_MobileNumber2" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="Mobile number2 should be of 10 digits." EnableClientScript="true"
                                            ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator10" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="Mother mobile number should not be blank." EnableClientScript="true"
                                            ClientValidationFunction="MobileNumber2Validation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="custValMobileNo2" Display="None" runat="server" CssClass="ClsMdtStar"
                                            Visible="true" ErrorMessage="Mobile Number 1 and 2 should not be same." EnableClientScript="true"
                                            Enabled="false" ClientValidationFunction="ValidateMobileNo2"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt">
                                        Email Address:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="1" rowspan="1" style="font-size: 10pt">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="TxtBoxMandMid" MaxLength="100"
                                            ViewStateMode="Enabled" />
                                        <asp:CustomValidator ID="cstValEmail" runat="server" ControlToValidate="txtEmail"
                                            ClientValidationFunction="EmailValidation" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                    <td align="left" class="TxtNormal" rowspan="1" style="font-size: 10pt; display: none">
                                        Is For Day Boarding?:
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="1" rowspan="1" style="font-size: 10pt;
                                        display: none">
                                        <asp:CheckBox ID="chkIsForDayBoarding" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trLivingLocation" runat="server">
                                    <td align="left" class="TxtNormal" style="font-size: 10pt">
                                        <span id="spnLocationHeader" runat="server">Living Location</span>:
                                    </td>
                                    <td align="left" class="TxtNormal" style="font-size: 10pt">
                                        <asp:DropDownList ID="cmbLivingLocation" runat="server" CssClass="TxtBoxMandMid"
                                            ViewStateMode="Enabled" Width="136px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtLivingLocation" Style="display: none;" AutoPostBack="false" runat="server"
                                            CssClass="TxtBox" MaxLength="50" Width="200px" />
                                        <asp:CustomValidator ID="cstLivingLocation" runat="server" ClientValidationFunction="ValidateLivingLocation"
                                            Display="None" ValidateEmptyText="true" ControlToValidate="cmbLivingLocation"></asp:CustomValidator>
                                    </td>
                                    <td align="left" class="TxtNormal">
                                        &nbsp;
                                    </td>
                                    <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- <% if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                                            {%>--%>
                    <tr>
                        <td colspan="4">
                            <cc1:CollapsablePanel ID="colpnlStudentAdditionalDetails" runat="server" TitleText="Additional Details"
                                TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                Collapsed="false" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0"
                                    width="100%">
                                    <tr id="trAdditional1" runat="server">
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt; width: 250px;
                                            height: 20px;">
                                            House Name/Plot no:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px;" colspan="1">
                                            <asp:TextBox ID="txtHouseNo" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px; width: 250px;"
                                            colspan="1">
                                            Land Mark:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt; height: 20px;">
                                            <asp:TextBox ID="txtLandmark" runat="server" CssClass="TxtBox" MaxLength="50" />
                                        </td>
                                    </tr>
                                    <tr id="trAdditional2" runat="server">
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Sub Area/ Lane:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txtSubArea" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            Main Area/Lane:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            <asp:TextBox ID="txtMainArea" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr id="trAdditional3" runat="server">
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Mother Office Address:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txtmOffcAddr" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            Father Office Address:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            <asp:TextBox ID="txtfoffcAddr" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr id="trAdditional4" runat="server">
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Taluka:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txttaluka" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            District:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            <asp:TextBox ID="txtDistrict" runat="server" CssClass="TxtBox" MaxLength="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 250px;">
                                            Date of Birth:&nbsp;
                                        </td>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 300px;">
                                            <asp:TextBox ID="txtCalDobPopup" CssClass="TxtBoxMand" runat="server" AutoPostBack="True"
                                                BackColor="#ffffa0"></asp:TextBox><rjs:PopCalendar ID="CalDobPopup" runat="server"
                                                    Control="txtCalDobPopup" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false"
                                                    To-Today="true" InvalidDateMessage="Please select valid date of birth." />
                                            <asp:Label ID="lblAge" runat="server" CssClass="LblI" Style="font-size: 10px;font-weight: bold;"></asp:Label>
                                            <asp:RequiredFieldValidator ID="reqDOB" runat="server" ErrorMessage="Date of Birth should not be blank."
                                                Display="None" ControlToValidate="txtCalDobPopup"> </asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cstDOB" Display="None" runat="server" CssClass="ClsMdtStar"
                                                ControlToValidate="txtCalDobPopup" Visible="true" EnableClientScript="true" ClientValidationFunction="checkDOB"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator11" Display="Static" runat="server" CssClass="ClsMdtStar"
                                                Visible="true" EnableClientScript="true" OnServerValidate="DOB_Validate"></asp:CustomValidator>
                                        </td>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt; width: 250px;">
                                            Place of Birth:&nbsp;
                                        </td>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt;">
                                            <asp:TextBox runat="server" CssClass="TxtBox" ID="txtBirthPlace" MaxLength="50"  onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateBirthPlace"
                                                Display="None"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Birth Taluka:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txtBirthTaluka" runat="server" CssClass="TxtBox" MaxLength="100" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)"/>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateBirthTaluka"
                                                Display="None"></asp:CustomValidator>
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            Birth District:
                                        </td>
                                        <td align="left" class="TxtNormal" style="font-size: 10pt">
                                            <asp:TextBox ID="txtBirthDistrict" runat="server" CssClass="TxtBox" MaxLength="100" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)" />
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateBirthDistrict"
                                                Display="None"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Birth State:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txtBirthState" runat="server" CssClass="TxtBox" MaxLength="100" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)"/>
                                            <asp:CustomValidator ID="cstValBirthState" runat="server" Enabled="false" ClientValidationFunction="ValidateBirthState"
                                                Display="None"></asp:CustomValidator>
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            Birth Country:
                                        </td>
                                        <td align="left" class="TxtNormal" colspan="1" style="font-size: 10pt">
                                            <asp:TextBox ID="txtBirthCountry" runat="server" CssClass="TxtBox" MaxLength="100" onblur="formatName(this)" onkeypress="return AllowOnlyNameFormat(event)"/>
                                            <asp:CustomValidator ID="custValBirthCountry" runat="server" Enabled="false" ClientValidationFunction="ValidateBirthCountry"
                                                Display="None"></asp:CustomValidator>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt" id="td1" runat="server">
                                            <asp:Label ID="lbl1" runat="server" Text="Birth Certificate"></asp:Label>
                                        </td>
                                        <td class="TxtNormal" align="left" style="font-size: 10pt" id="td2" runat="server">
                                            <asp:FileUpload ID="flUploadBirthCertificate" runat="server" CssClass="LrgTxtBox" Width="200px"
                                                ViewStateMode="Inherit" />
                                            <asp:ImageButton ID="btnViewBirthCert" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                            <asp:CustomValidator ID="cstValBirthCertificate" runat="server" ControlToValidate="flUploadBirthCertificate"
                                                Enabled="false" ClientValidationFunction="BirthCertValidation" Display="None"
                                                ValidateEmptyText="True"></asp:CustomValidator>
                                        </td>                                   
                                    </tr>
                                    <tr>                                        
                                        <td colspan="2" align="left">
                                            <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File
                                                size should not exceed 1MB.)&nbsp; &nbsp;&nbsp;</span>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>                                    
                                </table>
                            </cc1:CollapsablePanel>
                        </td>
                    </tr>
                    <%--    <%} %>--%>
                    <tr style="width: 100%;">
                        <td colspan="4" style="width: 100%">
                            <cc1:CollapsablePanel ID="colpnlHealthDetails" runat="server" TitleText="Student Health Details"
                                TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                Collapsed="false" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                <table align="center" class="paddingLR" cellspacing="1" cellpadding="1" border="0"
                                    width="100%" id="tblDSkHealthDetails" runat="server" visible="false">
                                    <tr style="height: 20px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtNormal" align="left" style="width: 200px;">
                                            <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Inoculation Given"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdoYes" runat="server" GroupName="HealthRatio" Text="Yes" />
                                            <asp:RadioButton ID="rdoNo" runat="server" GroupName="HealthRatio" Text="No" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtNormal" align="left" style="width: 200px;">
                                            <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Blood Group"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbBloodGroup" runat="server" CssClass="MidCombo" Width="150px">
                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                <asp:ListItem>O+</asp:ListItem>
                                                <asp:ListItem Value="A+">A+</asp:ListItem>
                                                <asp:ListItem>B+</asp:ListItem>
                                                <asp:ListItem>AB+</asp:ListItem>
                                                <asp:ListItem>O-</asp:ListItem>
                                                <asp:ListItem>A-</asp:ListItem>
                                                <asp:ListItem>B-</asp:ListItem>
                                                <asp:ListItem>AB-</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal" colspan="1">
                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Vaccination"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td style="font-weight: bold;">
                                                        <asp:Label class="TxtNormal" ID="Label2" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Doses"></asp:Label>
                                                    </td>
                                                    <td style="font-weight: bold;">
                                                        <asp:Label ID="Label3" class="TxtNormal" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Date"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal">
                                                        <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="i"></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="1" class="TxtNormal">
                                                        <asp:TextBox ID="txt1" runat="server" CssClass="SmlCombo" MaxLength="200"></asp:TextBox>
                                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txt1" Format="dd MMM yyyy"
                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                            To-Today="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal" colspan="1">
                                                        <asp:Label ID="Label7" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="ii"></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:TextBox ID="txtii" runat="server" CssClass="SmlCombo" MaxLength="200"></asp:TextBox>
                                                        <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtii" Format="dd MMM yyyy"
                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                            To-Today="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal">
                                                        <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="iii"></asp:Label>
                                                    </td>
                                                    <td align="left" class="TxtNormal">
                                                        <asp:TextBox ID="txtiii" runat="server" CssClass="SmlCombo" MaxLength="200"></asp:TextBox>
                                                        <rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtiii" Format="dd MMM yyyy"
                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                            To-Today="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal">
                                                        <asp:Label ID="Label11" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Booster"></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:TextBox ID="txtBooster" runat="server" CssClass="SmlCombo" MaxLength="200"></asp:TextBox>
                                                        <rjs:PopCalendar ID="PopCalendar4" runat="server" Control="txtBooster" Format="dd MMM yyyy"
                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                            To-Today="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal">
                                            <asp:Label ID="Label13" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Any Minor or Major Ailment"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAilment" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label15" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Allergies"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAllergies" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtNormal" align="left">
                                            <asp:Label ID="Label17" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Name Of The Family Doctor"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFamilyDoc" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal">
                                            <asp:Label ID="Label19" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Phone No."></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td class="TxtNormal">
                                            <table>
                                                <tr>
                                                    <td class="TxtNormal">
                                                        <asp:Label ID="Label21" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Clinic :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtClinic" runat="server" CssClass="SmlCombo" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                    <td class="TxtNormal">
                                                        <asp:Label ID="Label22" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Mobile :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDocMobile" runat="server" CssClass="SmlCombo" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="TxtNormal">
                                            <asp:Label ID="Label24" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="Contact Ph. No.(In Emergency)"></asp:Label>
                                            <span class="colonPadding">:</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoNoInEmergancy" runat="server" CssClass="LrgTxtBox" Width="300px"
                                                MaxLength="12"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblSNSHealthDetails" runat="server" align="left" visible="true" style="width: 100%;">
                                    <tr>
                                        <td style="height: 5pt;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 30%;" colspan="2">
                                                        <b>Vision :</b>
                                                    </td>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 30%;" colspan="2">
                                                        <b>Hearing : </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 17%; padding-left: 20pt;">
                                                        Any Consultation with doctor done :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkConsultation" runat="server" Text="Yes" />
                                                    </td>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 17%; padding-left: 20pt;">
                                                        Any difficulty observed :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkDifficulty" runat="server" Text="Yes" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 17%; padding-left: 20pt;">
                                                        Use of Spectacles/Corrective Lenses :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkSpectacles" runat="server" Text="Yes" />
                                                    </td>
                                                    <td class="TxtNormal" style="font-size: 10pt; width: 17%; padding-left: 20pt;">
                                                        Any Consultation with doctor done :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkHearinConclusion" runat="server" Text="Yes" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 5pt;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal" style="font-size: 10pt" colspan="2">
                                                        Any Medication taken for general well being :
                                                    </td>
                                                    <td class="TxtNormal" style="font-size: 10pt" colspan="2">
                                                        Any Allergy/any medical information that school should be aware of :
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="TxtNormal" align="left" rowspan="3" style="font-size: 10pt" colspan="2">
                                                        <asp:TextBox ID="txtMedication" runat="server" CssClass="TxtBox" MaxLength="300"
                                                            TextMode="MultiLine" Columns="21" Rows="4" Width="400px" ViewStateMode="Enabled" />
                                                    </td>
                                                    <td class="TxtNormal" align="left" rowspan="3" style="font-size: 10pt" colspan="2">
                                                        <asp:TextBox ID="txtSNSAllergy" runat="server" CssClass="TxtBox" MaxLength="300"
                                                            TextMode="MultiLine" Columns="21" Rows="4" Width="400px" ViewStateMode="Enabled" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5pt;">
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsablePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="TxtNormal" style="font-size: 10pt">
                            <asp:HiddenField ID="hidServerDt" runat="server" ViewStateMode="Enabled" />
                            <asp:HiddenField ID="hidStudentAdmisssionID" runat="server" ViewStateMode="Enabled" />
                            <asp:HiddenField ID="hidMinMaxDOBMap" runat="Server" ViewStateMode="Enabled" />
                            <asp:HiddenField ID="hidAmount" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidPPSSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidSNSSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidZLSPSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidOWSSchoolId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <%--<asp:HiddenField ID="hidHasFileUploaded" runat="Server" ViewStateMode="Enabled" Value="0"/>--%>
                            <asp:HiddenField ID="hidShowBirthValidations" runat="Server" ViewStateMode="Enabled"
                                Value="0" />
                            <asp:HiddenField ID="hidAadharCardScanCopy" runat="Server" ViewStateMode="Enabled"
                                Value="" />
                            <asp:HiddenField ID="hidBirthCertificateScanCopy" runat="Server" ViewStateMode="Enabled"
                                Value="" />
                            <asp:HiddenField ID="hidStudentPhoto" runat="Server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidIsEditMode" runat="Server" ViewStateMode="Enabled" Value="N" />
                            <asp:HiddenField ID="hidEnquiryId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidShowLastSchoolValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowLastStdValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowValidationForSchool" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowAdmissionCategoryValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowResidentTypeValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidFOccupationId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidMOccupationId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidFMobileNo" runat="Server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidMMobileNo" runat="Server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidAcademicYearId" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidCurrentDate" runat="Server" ViewStateMode="Enabled" Value="" />
                            <asp:HiddenField ID="hidSchoolIdBFS" runat="Server" ViewStateMode="Enabled" Value="0" />
                            <asp:HiddenField ID="hidShowUDISEValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowFullNameValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShow10thStdValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowAadharCardValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:HiddenField ID="hidShowreligionValidation" runat="Server" ViewStateMode="Enabled"
                                Value="N" />
                            <asp:Button runat="server" ID="btnSubmit" Text="Save & Next" CausesValidation="true"
                                CssClass="ClsBtn" Width="120px" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <b class="rbottom"></b>
        </div>
        <br />
    </div>
    <script language="javascript" type="text/javascript">
        _clienttxtPincode = "<%=this.txtPincode.ClientID %>"
        _clienttxtMobile2 = "<%=this.txtMobile2.ClientID %>"
        _clienttxtMobile = "<%=this.txtMobile.ClientID %>"
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>"
        _clienttxtCalDobPopup = "<%=this.txtCalDobPopup.ClientID %>"
        _clientcst_PIN = "<%=this.cst_PIN.ClientID %>"
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>"
        _clientcst_MobileNumber2 = "<%=this.cst_MobileNumber2.ClientID %>"
        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>"
        _clientcstDOB = "<%=this.cstDOB.ClientID %>"
        _clienthidServerDt = "<%=this.hidServerDt.ClientID %>"
        _clienthidMaxBdate = "<%=this.hidMaxBdate.ClientID %>"
        _clienthidMinBdate = "<%=this.hidMinBdate.ClientID %>"
        _clienttxtBirthDistrict = "<%=this.txtBirthDistrict.ClientID %>"
        _clienthidShowBirthValidations = "<%=this.hidShowBirthValidations.ClientID %>"
        _clienttxtBirthPlace = "<%=this.txtBirthPlace.ClientID %>"
        _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
        _clienthidPPSSchoolId = "<%=this.hidPPSSchoolId.ClientID %>"
        _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
        _clienthidOWSSchoolId = "<%=this.hidOWSSchoolId.ClientID %>"
        _clienthidIsEditMode = "<%=this.hidIsEditMode.ClientID %>"
        _clienthidZLSPSchoolId = "<%=this.hidZLSPSchoolId.ClientID %>"
        _clienthidSchoolIdBFS = "<%=this.hidSchoolIdBFS.ClientID %>"

        _clienttxtBirthTaluka = "<%=this.txtBirthTaluka.ClientID %>"
        _clienthidShowLastSchoolValidation = "<%=this.hidShowLastSchoolValidation.ClientID %>"
        _clienthidShowLastStdValidation = "<%=this.hidShowLastStdValidation.ClientID %>"
        _clientxtSchoolName = "<%=this.txtSchoolName.ClientID %>"
        _clienttxtPreviousSchoolAddress = "<%=this.txtPreviousSchoolAddress.ClientID %>"
        _clienttxtPreviousSchoolUDISENo = "<%=this.txtPreviousSchoolUDISENo.ClientID %>"
        _clienttxtLastStd = "<%=this.txtLastStd.ClientID %>"
        _clientrdolstlastSchoolBoard = "<%=this.rdolstlastSchoolBoard.ClientID %>"
        _clienthidShowValidationForSchool = "<%=this.hidShowValidationForSchool.ClientID %>"
        var minmaxDOBMap = eval($get('<%= this.hidMinMaxDOBMap.ClientID %>').value)[0];
        _clienthidShowResidentTypeValidation = "<%=this.hidShowResidentTypeValidation.ClientID %>"
        _clientcmbResidenceType = "<%=this.cmbResidenceType.ClientID %>"
        _clienttxtMarriageAnniversary = "<%=this.txtMarriageAnniversary.ClientID %>"
        _clientcstAnniversaryDate = "<%=this.cstAnniversaryDate.ClientID %>"
        _clienthidShowUDISEValidation = "<%=this.hidShowUDISEValidation.ClientID %>"
        _clienthidShowFullNameValidation = "<%=this.hidShowFullNameValidation.ClientID %>"
        _clientcstValidateFatherName = "<%=this.cstValidateFatherName.ClientID %>"
        _clientcstValidateMotherName = "<%=this.cstValidateMotherName.ClientID %>"
        _clienthidShow10thStdValidation = "<%=this.hidShow10thStdValidation.ClientID %>"

        _clienttxtPreviousSchoolSaralId = "<%=this.txtPreviousSchoolSaralId.ClientID %>"
        _clienttxtBirthState = "<%=this.txtBirthState.ClientID %>"
        _clienttxtBirthCountry = "<%=this.txtBirthCountry.ClientID %>"

        function ValidateBirthTaluka(oSrc, args) {
            if ($('#' + _clienthidShowBirthValidations).val() == "Y") {
                var tal = $('#' + _clienttxtBirthTaluka).val()
                if (tal.trim() == "") {
                    oSrc.errormessage = "Birth Taluka should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateBirthState(oSrc, args) {
            if ($('#' + _clienthidShowBirthValidations).val() == "Y") {
                var tal = $('#' + _clienttxtBirthState).val()
                if (tal.trim() == "") {
                    oSrc.errormessage = "Birth State should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateBirthCountry(oSrc, args) {
            if ($('#' + _clienthidShowBirthValidations).val() == "Y") {
                var tal = $('#' + _clienttxtBirthCountry).val()
                if (tal.trim() == "") {
                    oSrc.errormessage = "Birth Country should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateBirthDistrict(oSrc, args) {
            if ($('#' + _clienthidShowBirthValidations).val() == "Y") {
                var dist = $('#' + _clienttxtBirthDistrict).val()
                if (dist.trim() == "") {
                    oSrc.errormessage = "Birth District should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateBirthPlace(oSrc, args) {
            if ($('#' + _clienthidShowBirthValidations).val() == "Y" || $('#' + _clienthidShowValidationForSchool).val() == "Y") {
                var place = $('#' + _clienttxtBirthPlace).val()
                if (place.trim() == "") {
                    oSrc.errormessage = "Place of Birth should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function FileUploadValidation(oSrc, args) {
            if ($get("<%=this.FilUpImg.ClientID %>") != null) {
                if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidPPSSchoolId).val()) {

                    var fl = $get("<%=this.FilUpImg.ClientID %>").value;
                    if (fl == '') {
                        var adh = $get('<%=this.hidAadharCardScanCopy.ClientID %>').value
                        if (adh == '') {
                            oSrc.errormessage = "Please select file for Aadhar Card.";
                            args.IsValid = false;
                            return true;
                        }
                        else {
                            args.IsValid = true;
                            return false;
                        }
                    }
                    else if (fl != "") {
                        if (!(fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPG" ||
                      fl.substr(fl.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".BMP" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PDF" ||
                      fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PNG"
                    )) {
                            oSrc.errormessage = "Please select valid file type for Aadhar Card.";
                            args.IsValid = false;
                            return true;
                        }
                    }
                    //                else {
                    //                    var IsEditMode = $('#' + _clienthidIsEditMode).val();
                    //                    if (IsEditMode == "N") {
                    //                        oSrc.errormessage = "Please upload Birth Certificate.";
                    //                        args.IsValid = false;
                    //                        return true;
                    //                    }
                    //}
                }
            }
            args.IsValid = true
            return false
        }

        function BirthCertValidation(oSrc, args) {
            if ($get("<%=this.flUploadBirthCertificate.ClientID %>") != null) {

                var fl = $get("<%=this.flUploadBirthCertificate.ClientID %>").value;

                if (fl == '') {

                    var birthCert = $get('<%=this.hidBirthCertificateScanCopy.ClientID %>').value
                    if (birthCert == '') {
                        oSrc.errormessage = "Please select file for Birth Certificate.";
                        args.IsValid = false;
                        return true;
                    }
                    else {
                        args.IsValid = true;
                        return false;
                    }
                }
                else if (fl != "") {
                    if (!(fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".JPG" ||
                fl.substr(fl.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" ||
                fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".BMP" ||
                fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PDF" ||
                fl.substr(fl.lastIndexOf('.'), 4).toUpperCase() == ".PNG"
            )) {
                        oSrc.errormessage = "Please select valid file type for Birth Certificate.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            args.IsValid = true
            return false
        }




        SetField();
        function SetField() {
            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidPPSSchoolId).val()) {
                var fl = $get("<%=this.FilUpImg.ClientID %>")
                var CastAndSub = $get("<%=this.txtCasteAndSubcaste.ClientID %>")
                var S1 = $get("<%=this.cmbReligion.ClientID %>")
                //                fl.style.backgroundColor = "#ffffa0";
                CastAndSub.style.backgroundColor = "#ffffa0";
                S1.style.backgroundColor = "#ffffa0";
            }

            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidSchoolIdBFS).val()) {
                var aadharCard = $get("<%=this.txtAadharCardNo.ClientID %>")
                aadharCard.style.backgroundColor = "#ffffa0";
            }

            if ($('#' + _clienthidShowResidentTypeValidation).val() == "Y")
                $('#' + _clientcmbResidenceType).css("backgroundColor", "#ffffa0")
            else
                $('#' + _clientcmbResidenceType).css("backgroundColor", "white")

            if ($('#' + _clienthidSchoolId).val() != $('#' + _clienthidSNSSchoolId).val())
                $('#' + "<%=this.cmbLivingLocation.ClientID %>").css("backgroundColor", "#ffffa0")
            else
                $('#' + "<%=this.cmbLivingLocation.ClientID %>").css("backgroundColor", "white")

            var validateLastSchoolDetails = $('#' + _clienthidShowLastSchoolValidation).val()
            if (parseInt(validateLastSchoolDetails) == 1) {
                $('#' + _clientxtSchoolName).css("backgroundColor", "#ffffa0")
                $('#' + _clienttxtPreviousSchoolAddress).css("backgroundColor", "#ffffa0")
                $('#' + _clienttxtLastStd).css("backgroundColor", "#ffffa0")

                if ($('#' + _clienthidShowUDISEValidation).val() == 'Y') {
                    $('#' + _clienttxtPreviousSchoolUDISENo).css("backgroundColor", "#ffffa0")
                    $('#' + _clienttxtPreviousSchoolSaralId).css("backgroundColor", "#ffffa0")
                }
            }

            if ($('#' + _clienthidShowValidationForSchool).val() == "Y")
                $('#' + _clienttxtBirthPlace).css("backgroundColor", "#ffffa0")
        }        
    </script>
    <script type="text/javascript">
        function SetVisibilityOfLocationTxt(obj) {
            _clienttxtLivingLocation = "<%=this.txtLivingLocation.ClientID %>"
            _clienttxtLivingLocation = "<%=this.txtLivingLocation.ClientID %>"
            var tblTxtLiving = document.getElementById(_clienttxtLivingLocation);
            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidPPSSchoolId).val() && obj.value == 14) {
                tblTxtLiving.style.display = '';
            }
            else
                tblTxtLiving.style.display = 'none';
        }

        function ValidateReligion(oSrc, args) {
            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidPPSSchoolId).val() ||
             $('#' + '<%=this.hidShowreligionValidation.ClientID %>').val() == 'Y') {
                var Religion = $get("<%=this.cmbReligion.ClientID %>").value;
                if (Religion == 0) {
                    oSrc.errormessage = "Religion should be selected.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateCastAndSubcast(oSrc, args) {
            _clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidPPSSchoolId).val() || $('#' + _clienthidSchoolId).val() == $('#' + _clienthidSNSSchoolId).val()) {
                var CastAndSub = $get("<%=this.txtCasteAndSubcaste.ClientID %>").value;
                if (CastAndSub.trim() == "") {
                    oSrc.errormessage = "Caste/Sub-caste should not be blank.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateLivingLocation(oSrc, args) {
            if (($('#' + _clienthidSchoolId).val() != $('#' + _clienthidSNSSchoolId).val()) && ($('#' + _clienthidSchoolId).val() != $('#' + _clienthidZLSPSchoolId).val())) {
                var LivingLocation = $get("<%=this.cmbLivingLocation.ClientID %>").value;
                if (LivingLocation == 0) {
                    if ($('#' + '<%=this.hidShowAdmissionCategoryValidation.ClientID %>').val() == "Y")
                        oSrc.errormessage = "Admission Category should be selected.";
                    else
                        oSrc.errormessage = "Living Location should be selected.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateLastSchoolName(oSrc, args) {
            if (ShowLastSchoolValidation()) {
                if ($('#' + _clientxtSchoolName).val().trim() == "") {
                    oSrc.errormessage = "School Name of Last School Details should not be blank.";
                    args.IsValid = false;
                    return true
                }
            }
            args.IsValid = true;
            return false
        }

        function ShowLastSchoolValidation() {
            var validateLastSchoolDetails = $('#' + _clienthidShowLastSchoolValidation).val()
            if (parseInt(validateLastSchoolDetails) == 1)
                return true;
            else
                return false;
        }

        function ShowLastStdValidation() {
            if ($('#' + _clienthidShowLastStdValidation).val() == "Y")
                return true;
            else
                return false;
        }





        function ValidateLastSchoolAddress(oSrc, args) {
            if (ShowLastSchoolValidation() || ShowLastStdValidation()) {
                if ($('#' + _clienttxtPreviousSchoolAddress).val().trim() == "") {
                    oSrc.errormessage = "Previous School Address should not be blank.";
                    args.IsValid = false;
                    return true
                }
            }
            args.IsValid = true;
            return false
        }

        function ValidateLastSchoolUDISE(oSrc, args) {
            if (ShowLastSchoolValidation()) {
                if ($('#' + _clienttxtPreviousSchoolUDISENo).val().trim() == "") {
                    oSrc.errormessage = "Previous School U-DISE should not be blank.";
                    args.IsValid = false;
                    return true
                }
            }
            args.IsValid = true;
            return false
        }

        function ValidateLastSchoolStandard(oSrc, args) {
            if (ShowLastSchoolValidation() || ShowLastStdValidation()) {
                if ($('#' + _clienttxtLastStd).val().trim() == "") {
                    oSrc.errormessage = "Standard of Last School Details should not be blank.";
                    args.IsValid = false;
                    return true
                }
            }
            args.IsValid = true;
            return false
        }

        function ValidateLastSchoolBoard(oSrc, args) {
            if (ShowLastSchoolValidation()) {
                var list = document.getElementById(_clientrdolstlastSchoolBoard); //Client ID of the radiolist
                var inputs = list.getElementsByTagName("input");
                var selected = null;
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selected = inputs[i];
                        break;
                    }
                }

                if (selected == null) {
                    oSrc.errormessage = "School Board should be selected.";
                    args.IsValid = false;
                    return true
                }
            }
            args.IsValid = true;
            return false
        }

        function ValidateResidenceType(oSrc, args) {
            _clienthidShowResidentTypeValidation = "<%=this.hidShowResidentTypeValidation.ClientID %>"
            if ($('#' + _clienthidShowResidentTypeValidation).val() == "Y") {
                if ($('#' + _clientcmbResidenceType).val() == "0") {
                    oSrc.errormessage = "Preference should be selected.";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateAnniversaryDate(oSrc, args) {
            var StudentDOB = new Date(convertdate(document.getElementById(_clienttxtCalDobPopup).value));
            var AnniversaryDate = new Date(convertdate(document.getElementById(_clienttxtMarriageAnniversary).value));

            if (StudentDOB < AnniversaryDate) {
                document.getElementById(_clientcstAnniversaryDate).errormessage = "Marriage Anniversary date should be less than student DOB."
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        $('*[id*=CalDobPopup]').change(function () {
            SetDate();
        });

        function ValidateAadharCard(oSrc, args) {
            _clienttxtAadharCardNo = "<%=this.txtAadharCardNo.ClientID %>"
            _clientcstAadharCard = "<%=this.cstAadharCard.ClientID %>"

            _clienthidShowAadharCardValidation = "<%=this.hidShowAadharCardValidation.ClientID %>"

            var AadharCardNo = document.getElementById(_clienttxtAadharCardNo).value;

            if ($('#' + _clienthidShowAadharCardValidation).val() == 'Y') {
                if (AadharCardNo == "") {
                    document.getElementById(_clientcstAadharCard).errormessage = "Aadhar Card Number should not be blank."
                    args.IsValid = false;
                    return true;
                }
                else if (AadharCardNo != "" && AadharCardNo.length != 12) {
                    document.getElementById(_clientcstAadharCard).errormessage = "Aadhar Card Number should be of 12 digits."
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            else if (AadharCardNo != "" && AadharCardNo.length != 12) {
                document.getElementById(_clientcstAadharCard).errormessage = "Aadhar Card Number should be of 12 digits."
                args.IsValid = false;
                return true;
            }
        }

        function ValidateSchoolUDISE(oSrc, args) {
            _clientcstSchoolUDISE = "<%=this.cstSchoolUDISE.ClientID %>"
            if ($('#' + _clienthidSchoolId).val() == $('#' + _clienthidSchoolIdBFS).val()) {
                var SchoolUDISE = $get("<%=this.txtPreviousSchoolUDISENo.ClientID %>")
                if (SchoolUDISE.value == "") {
                    document.getElementById(_clientcstSchoolUDISE).errormessage = "Previous School U-DISE number should not be blank."
                    args.IsValid = false;
                    return true;
                }
                else if (SchoolUDISE.value.length != 11) {
                    document.getElementById(_clientcstSchoolUDISE).errormessage = "Previous School U-DISE number should be of 11 digit."
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }

        function MobileNumber2Validation(oSrc, args) {
            args.IsValid = true;
            if ($('#' + _clienthidShowResidentTypeValidation).val() == 'Y') {
                if ($('#' + _clienttxtMobile2).val().trim() == '') {
                    oSrc.errormessage = 'Mother mobile number should not be blank.'
                    args.IsValid = false;
                }
            }

            return !args.IsValid;
        }

        function SetDate() {
            //Finding Current Date
            _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID %>"
            var age;
            var curDate = new Date(document.getElementById(_clienthidCurrentDate).value);
            var curr_year = curDate.getFullYear();
            var curr_month = curDate.getMonth() + 1;
            var Curr_Date = curDate.getDate();
            //Finding DOB
            var dt1 = document.getElementById("<%=this.txtCalDobPopup.ClientID %>").value;
            if (dt1 != '') {
                var dob;
                if (document.all)
                    dob = new Date(dt1.replace('-', ' '));
                else
                    dob = new Date(convertdate(dt1));
                var dob_Year = dob.getFullYear();
                var dob_Month = dob.getMonth() + 1;
                var dob_Date = dob.getDate();
                //Calculate Year and Month For display age
                var Year = curr_year - dob_Year;
                var Month = curr_month - dob_Month;
                var DateCount = Curr_Date - dob_Date;
                if (DateCount < 0) {
                    Month = Month - 1;
                }
                if (Month < 0) {
                    Year = Year - 1;
                    Month = Math.abs(Month);
                    Month = 12 - Month;
                }

                if (Year < 0) {
                    age = "- Year(s) - Month(s)";
                }
                else {
                    age = Year.toString() + " Year(s) " + Month.toString() + " Month(s)";
                }

            }
            else {
                age = "- Year(s) - Month(s)";
            }
            var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

            $('#<%=lblAge.ClientID%>').html('<BR />'+age + " till " + curDate.getDate() + " " + month[curDate.getMonth()] + " " + curDate.getFullYear());
        }

        function OnBlur(test) {
            if ($('#' + _clienthidShowUDISEValidation).val() == "Y") {
                extractNumber(val, 0, false);
            }
        }

        function OnKeyUp(val) {
            if ($('#' + _clienthidShowUDISEValidation).val() == "Y") {
                extractNumber(val, 0, false);
            }
        }

        function OnKeyPress(val, event) {
            if ($('#' + _clienthidShowUDISEValidation).val() == "Y") {
                return blockNonNumbers(val, event, false, false);
            }
        }

        function ValidateStudentName(oSrc, args) {
            _clienttxtSLastName = "<%=this.txtSLastName.ClientID %>"
            _clienttxtFahterName = "<%=this.txtFahterName.ClientID %>"

            var StudentLastName = document.getElementById(_clienttxtSLastName).value;
            var StudentMIddleName = document.getElementById(_clienttxtFahterName).value;
            var ShowFullNameValidation = document.getElementById(_clienthidShowFullNameValidation).value;

            if (ShowFullNameValidation == "Y") {
                if (StudentLastName == "" && StudentMIddleName == "") {
                    oSrc.errormessage = "Students Last Name & Middle Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (StudentLastName == "" && StudentMIddleName != "") {
                    oSrc.errormessage = "Students Last Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (StudentLastName != "" && StudentMIddleName == "") {
                    oSrc.errormessage = "Students Middle Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateMiddleName(oSrc, args) {
            _clienttxtFatherName = "<%=this.txtFahterName.ClientID %>"
            var StudentMiddleName = document.getElementById(_clienttxtFatherName).value;

            if (StudentMiddleName == "") {
                oSrc.errormessage = "Student's Middle Name should not be blank.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFatherName(oSrc, args) {
            _clienttxtFSurname = "<%=this.txtFSurname.ClientID %>"
            _clienttxtFFatherName = "<%=this.txtFFatherName.ClientID %>"

            var FatherLastName = document.getElementById(_clienttxtFSurname).value;
            var FatherMiddleName = document.getElementById(_clienttxtFFatherName).value;
            var ShowFullNameValidation = document.getElementById(_clienthidShowFullNameValidation).value;

            if (ShowFullNameValidation == "Y") {
                if (FatherLastName == "" && FatherMiddleName == "") {
                    oSrc.errormessage = "Father Last Name & Father's Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (FatherLastName == "" && FatherMiddleName != "") {
                    oSrc.errormessage = "Father Last Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (FatherLastName != "" && FatherMiddleName == "") {
                    oSrc.errormessage = "Father Father's Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateMotherName(oSrc, args) {
            _clienttxtMSurname = "<%=this.txtMSurname.ClientID %>"
            _clienttxtMHName = "<%=this.txtMHName.ClientID %>"

            var MotherLastName = document.getElementById(_clienttxtMSurname).value;
            var MotherMiddleName = document.getElementById(_clienttxtMHName).value;
            var ShowFullNameValidation = document.getElementById(_clienthidShowFullNameValidation).value;

            if (ShowFullNameValidation == "Y") {
                if (MotherLastName == "" && MotherMiddleName == "") {
                    oSrc.errormessage = "Mother Last Name & Husband's Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (MotherLastName == "" && MotherMiddleName != "") {
                    oSrc.errormessage = "Mother Last Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (MotherLastName != "" && MotherMiddleName == "") {
                    oSrc.errormessage = "Mother Husband's Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function Validate10thDetails(oSrc, args) {
            var ShowValidation = document.getElementById(_clienthidShow10thStdValidation).value;
            if (ShowValidation == "Y") {
                _clienttxt10Board = "<%=this.txt10Board.ClientID %>";
                _clienttxt10RollNo = "<%=this.txt10RollNo.ClientID %>";
                _clienttxt10Exam = "<%=this.txt10Exam.ClientID %>";
                _clienttxt10PassingYear = "<%=this.txt10PassingYear.ClientID %>";
                _clienttxt10thMaths = "<%=this.txt10thMaths.ClientID %>";

                var BoardName = document.getElementById(_clienttxt10Board).value;
                var RollNo = document.getElementById(_clienttxt10RollNo).value;
                var Exam = document.getElementById(_clienttxt10Exam).value;
                var PasingYear = document.getElementById(_clienttxt10PassingYear).value;
                var Maths = document.getElementById(_clienttxt10thMaths).value;

                if (BoardName == "") {
                    oSrc.errormessage = "10th Std. Board name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (RollNo == "") {
                    oSrc.errormessage = "10th Std. Board Roll Number should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (Exam == "") {
                    oSrc.errormessage = "10th Std. Exam should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (PasingYear == "") {
                    oSrc.errormessage = "10th Std. Board passing year should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else if (Maths == "") {
                    oSrc.errormessage = "10th Std. Basic / Standard Mathematics should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function PreviousSchoolSaralValidation(oSrc, args) {
            if ($('#' + _clienthidShowLastSchoolValidation).val() == "1") {
                if ($("#<%= chkIsSchoolFromOutOfState.ClientID %>").is(":checked") == false) {
                    var PreSaralId = $get("<%=this.txtPreviousSchoolSaralId.ClientID %>").value;
                    if (PreSaralId == "") {
                        oSrc.errormessage = "Previous School Student Saral Id should not be blank.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            args.IsValid = true;
            return false;
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

        function ValidateStudentPhotoFile(oSrc, args) {
            var _clientFileUploadLogo = "<%=this.flStudentPhoto.ClientID %>"
            var _clienthidStudentPhoto = "<%=this.hidStudentPhoto.ClientID %>"

            if ($get(_clientFileUploadLogo) != null) {
                var fl = $get(_clientFileUploadLogo).value;
                var sFile = $get(_clienthidStudentPhoto).value

                if (fl == '' && sFile != "1") {
                    oSrc.errormessage = "Please select File for Student Photo.";
                    args.IsValid = false;
                    return true;
                }
                else if (fl != "") {
                    var file = $get("<%=this.flStudentPhoto.ClientID %>")
                    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP"
                                )) {
                        oSrc.errormessage = "Please select valid file type for Student photo.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (file.files[0].size >= 1048576) {
                        oSrc.errormessage = "Student Photo file size should not be more than 1 MB."
                        args.IsValid = false
                        return true
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateLanguages(src, args) {
            var secondLang = $get('<%=this.cmbSecondSLanguageSubjectId.ClientID %>')
            var thirdLang = $get('<%=this.cmbThirdLanguage.ClientID %>')

            if (secondLang != null && thirdLang != null && secondLang.value != "0" && thirdLang.value != "0") {

                var secLangText = secondLang.options[secondLang.selectedIndex].text;
                var trdLangText = thirdLang.options[thirdLang.selectedIndex].text;

                if (secLangText.search('Marathi II') == -1 && trdLangText.search('Marathi III') == -1) {
                    src.errormessage = 'Either second or third language should be Marathi.'
                    args.IsValid = false;
                    return true;
                }
                else if ((secLangText.search('Marathi') != -1 && trdLangText.search('Marathi') != -1) ||
                (secLangText.search('Hindi') != -1 && trdLangText.search('Hindi') != -1) ||
                (secLangText.search('Sanskrit') != -1 && trdLangText.search('Sanskrit') != -1)) {
                    src.errormessage = 'Second and Third Language should not be of same subject.'
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMobileNo2(src, args) {
            var mobile1 = $('#' + _clienttxtMobile).val();
            var mobile2 = $('#' + _clienttxtMobile2).val();
            if (mobile1 != '' && mobile2 != '' && mobile1 == mobile2) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateFatherAadharFile(sender, args) {
            var fileInput = document.getElementById('<%= flUploadFatherAaadhar.ClientID %>');
            var filePath = fileInput.value.toLowerCase();

            // Allowed extensions
            var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;

            if (filePath == '') {
                sender.errormessage = "Please select file for Father's Aadhar Card.";
                args.IsValid = false;
                return;
            }

            if (!allowedExtensions.test(filePath)) {
                sender.errormessage = "Please select valid file type for Father's Aadhar Card.";                                
                args.IsValid = false;
                return;
            }

            // Check size if file selected
            if (fileInput.files && fileInput.files[0]) {
                var fileSize = fileInput.files[0].size; // in bytes
                if (fileSize > 1048576) { // 1 MB
                    sender.errormessage = "Size of Father's Aadhar Card file should not be more than 1 mb.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        function ValidateMotherAadharFile(sender, args) {
            var fileInput = document.getElementById('<%= flUploadMotherAaadhar.ClientID %>');
            var filePath = fileInput.value.toLowerCase();

            // Allowed extensions
            var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;

            if (filePath == '') {
                sender.errormessage = "Please select file for Mother's Aadhar Card.";
                args.IsValid = false;
                return;
            }

            if (!allowedExtensions.test(filePath)) {
                sender.errormessage = "Please select valid file type for Mother's Aadhar Card.";
                args.IsValid = false;
                return;
            }

            // Check size if file selected
            if (fileInput.files && fileInput.files[0]) {
                var fileSize = fileInput.files[0].size; // in bytes
                if (fileSize > 1048576) { // 1 MB
                    sender.errormessage = "Size of Mother's Aadhar Card file should not be more than 1 mb.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        function ValidateCasteCertFile(sender, args) {
            var fileInput = document.getElementById('<%= flUploadCastCert.ClientID %>');
            var filePath = fileInput.value.toLowerCase();

            // Allowed extensions
            var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;

            if (filePath != '') {
                if (!allowedExtensions.test(filePath)) {
                    sender.errormessage = "Please select valid file type for Caste Certificate.";
                    args.IsValid = false;
                    return;
                }

                // Check size if file selected
                if (fileInput.files && fileInput.files[0]) {
                    var fileSize = fileInput.files[0].size; // in bytes
                    if (fileSize > 1048576) { // 1 MB
                        sender.errormessage = "Size of Caste Certificate file should not be more than 1 mb.";
                        args.IsValid = false;
                        return;
                    }
                }
            }

            args.IsValid = true;
        }

    </script>
    <script type="text/javascript" src="../Scripts/Admission/AdmissionFormStudentDetails.js?version=1.7"></script>
</asp:Content>
