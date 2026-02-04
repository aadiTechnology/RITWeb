<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeacherAdditionalDetailsUI.aspx.cs"
    Inherits="TeacherAdditionalDetailsUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Src="~/UserControls/UserBasicDetails.ascx" TagName="UserBasicDetails"
    TagPrefix="UserBasicDetailsUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td align="right">
                    <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="True" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="Update1" runat="server">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valTeacherDetails" runat="server" ValidationGroup="Save" />
                            <asp:RequiredFieldValidator ID="reqTeacherType" runat="server" CssClass="LblErrorMsg"
                                ControlToValidate="cmbTeacherTypes" Display="None" ErrorMessage="Types Of Teacher should be selected."
                                ValidationGroup="Save" InitialValue="0"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trControls" runat="server">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="80%">
                                <tr>
                                    <td align="center" style="text-align: center; margin: 0px auto;">
                                        <table align="center" style="text-align: center; margin: 0px auto;">
                                            <tr>
                                                <td align="center" class="ClsBorderLight">
                                                    <span id="lblUserName" class="ClsLabel">Teacher Name :</span><span id="cstValEmail"
                                                        style="color: Red; display: none;"></span>
                                                </td>
                                                <td align="left" class="ClsHilightBGB">
                                                    <asp:Label ID="lblTeacherName" runat="server" Height="20px" Width="100%" Visible="true"
                                                        EnableViewState="true" BackColor="Transparent"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Types Of Teacher : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbTeacherTypes" runat="server" CssClass="LrgCombo">                                                        
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Total Days Of in Service Training in Last Academic Year(BRC)
                                                        : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTotalDaysBRC" runat="server" CssClass="LrgTxtBox"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Total Days Of in Service Training in Last Academic Year(CRC)
                                                        : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTotalDaysCRC" runat="server" CssClass="LrgTxtBox"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Total Days Of in Service Training in Last Academic Year(DIET)
                                                        : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTotalDaysDIET" runat="server" CssClass="LrgTxtBox"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Total Days Of in Service Training in Last Academic Year(Other) : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtOtherCount" runat="server" CssClass="LrgTxtBox"> </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Types Of Training : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbTypesOfTraining" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Highest Qualification(Academic) : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbAcademicQualification" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Highest Qualification(Professional) : </span>
                                                </td>
                                                 <td align="left">
                                                    <asp:DropDownList ID="cmbProfessionalQualification" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Classes Taught : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbClassesTaught" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Appointed For Subject : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAppointedSubject" runat="server" CssClass="LrgTxtBox" TextMode ="MultiLine" Height="40px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Main Subject Taught : </span>
                                                </td>
                                               <td align="left">
                                                    <asp:DropDownList ID="cmbMainSubject" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Additional Subject Taught : </span>
                                                </td>
                                               <td align="left">
                                                    <asp:DropDownList ID="cmbAdditionalSubjects" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">No.Of Working Days Spent On Non Teaching Assignments : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNonTeachingAssignment" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Maths/Science Studied Upto : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMathsScienceStudiedUpto" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">English Studied Upto : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEnglishStudiedUpto" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Social Studies Studied Upto : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSocialStudiedUpto" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Working In Present School Since(Year) : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPresentSchoolYear" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Trained in Use Of Computer/Teaching Through Computer : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoIsComputerTrainedYes" Text="Yes" GroupName="Computer" runat="server" />
                                                    <asp:RadioButton ID="rdoIsComputerTrainedNo" Text="No" GroupName="Computer" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Disability(If Any) : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbDisability" runat="server" CssClass="LrgCombo"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Trained To Teach CWSN : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:RadioButton ID="rdoIsCWSNTrainedYes" Text="Yes" GroupName="CWSN" runat="server" />
                                                    <asp:RadioButton ID="rdoIsCWSNTrainedNo" Text="No" GroupName="CWSN" runat="server" />
                                                </td>
                                            </tr>                                           
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                                                        ValidationGroup="Save" />
                                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClick="btnClear_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                        PostBackUrl="~/RITeSchool/Admin/TeacherInfoUI.aspx" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center" style="height: 23px">
                                                    <asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidAdditionalDetailsId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
