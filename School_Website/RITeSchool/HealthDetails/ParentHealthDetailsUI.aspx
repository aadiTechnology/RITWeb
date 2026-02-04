<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ParentHealthDetailsUI.aspx.cs" Inherits="ParentHealthDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" width="100%" cellpadding="0">
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td align="left">
                            <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <div style="float: right; vertical-align: top;">
                                            <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="ValErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Blue"
                                Width="100%" Height="20px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr>
                                    <td class="txtNormal">
                                        <asp:RequiredFieldValidator ID="ReqFName" runat="server" ErrorMessage="Father Name should not be blank."
                                            Display="None" ControlToValidate="txtFName"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMName" runat="server" ErrorMessage="Mother Name should not be blank."
                                            Display="None" ControlToValidate="txtMName"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqFDOB" runat="server" ErrorMessage="Father DOB should not be blank."
                                            Display="None" ControlToValidate="txtFDOB"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMDOB" runat="server" ErrorMessage="Mother DOB should not be blank."
                                            Display="None" ControlToValidate="txtMDOB"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqFAadhar" runat="server" ErrorMessage="Father Aadhar no should not be blank."
                                            Display="None" ControlToValidate="txtFAadharCardNo"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMAadhar" runat="server" ErrorMessage="Mother Aadhar no should not be blank."
                                            Display="None" ControlToValidate="txtMAadharCardNo"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqFBloodGroup" runat="server" ErrorMessage="Father Blood Group should be selected."
                                            Display="None" ControlToValidate="ddlFBloodGroup" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMBloodGroup" runat="server" ErrorMessage="Mother Blood Group should be selected."
                                            Display="None" ControlToValidate="ddlMBloodGroup" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqFHeight" runat="server" ErrorMessage="Father Height should not be blank."
                                            Display="None" ControlToValidate="txtFHeight"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMHeight" runat="server" ErrorMessage="Mother Height should not be blank."
                                            Display="None" ControlToValidate="txtMHeight"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqFWeight" runat="server" ErrorMessage="Father Weight should not be blank."
                                            Display="None" ControlToValidate="txtFWeight"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="ReqMWeight" runat="server" ErrorMessage="Mother Weight should not be blank."
                                            Display="None" ControlToValidate="txtMWeight"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="width: 150px;">
                                                    <asp:Label ID="lblLabel1" runat="server" Text="Student Name :" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td class="ClsHilightBGB" colspan="3">
                                                    <asp:Label ID="lblStudentName" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                                        Width="50px" />
                                                </td>
                                                <td align="left">
                                                    <strong>FATHER</strong>
                                                </td>
                                                <td style="width: 50px;">
                                                </td>
                                                <td align="left">
                                                    <strong>MOTHER</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderLight">
                                                    <span class="clsLabel">Full Name : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFName" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMName" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderLight">
                                                    <span class="clsLabel">Date Of Birth : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFDOB" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                                    <rjs:PopCalendar ID="FDOB" runat="server" Control="txtFDOB" Format="dd MMM yyyy"
                                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                        To-Today="true" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMDOB" runat="server" CssClass="MidTxtBox" ReadOnly="true"></asp:TextBox>
                                                    <rjs:PopCalendar ID="MDOB" runat="server" Control="txtMDOB" Format="dd MMM yyyy"
                                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                        To-Today="true" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderLight">
                                                    <span class="clsLabel">Aadhar Card Number : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFAadharCardNo" runat="server" CssClass="LrgTxtBox" MaxLength="15"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMAadharCardNo" runat="server" CssClass="LrgTxtBox" MaxLength="15"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="clsBorderLight">
                                                    <span class="clsLabel">Blood Group : </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFBloodGroup" runat="server" CssClass="SmlCombo" ViewStateMode="Enabled">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem>O+</asp:ListItem>
                                                        <asp:ListItem>A+</asp:ListItem>
                                                        <asp:ListItem>B+</asp:ListItem>
                                                        <asp:ListItem>AB+</asp:ListItem>
                                                        <asp:ListItem>O-</asp:ListItem>
                                                        <asp:ListItem>A-</asp:ListItem>
                                                        <asp:ListItem>B-</asp:ListItem>
                                                        <asp:ListItem>AB-</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMBloodGroup" runat="server" CssClass="SmlCombo" ViewStateMode="Enabled">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem>O+</asp:ListItem>
                                                        <asp:ListItem>A+</asp:ListItem>
                                                        <asp:ListItem>B+</asp:ListItem>
                                                        <asp:ListItem>AB+</asp:ListItem>
                                                        <asp:ListItem>O-</asp:ListItem>
                                                        <asp:ListItem>A-</asp:ListItem>
                                                        <asp:ListItem>B-</asp:ListItem>
                                                        <asp:ListItem>AB-</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="clsBorderLight">
                                                    <span class="clsLabel">Height : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFHeight" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMHeight" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="clsBorderLight">
                                                    <span class="clsLabel">Weight : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFWeight" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,1,false);"
                                                        onkeyup="extractNumber(this,1,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMWeight" runat="server" CssClass="SmlTxtBox" onblur="extractNumber(this,1,false);"
                                                        onkeyup="extractNumber(this,1,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" Enabled="false" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidId" runat="server" />
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
