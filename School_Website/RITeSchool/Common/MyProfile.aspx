<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="MyProfile.aspx.cs" Inherits="MyProfile" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%; vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <td colspan="4" align="center">
                        <table id="tblStudInfo" border="0" cellpadding="1" cellspacing="2" align="center">
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblAddress" class="ClsLabel">Address :</span><span id="cstAddress" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblStudentAddress" runat="server" ViewStateMode="Enabled" TextMode="StudentAddress" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblPhoneNo" class="ClsLabel">Residence Phone No :</span><span id="cstPhoneNo" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblStudentPhoneNo" runat="server" ViewStateMode="Enabled" TextMode="StudentPhoneNo" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lbReligion" class="ClsLabel">Religion :</span><span id="cstReligion" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblReligion" runat="server" ViewStateMode="Enabled" TextMode="Religion" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblCaste" class="ClsLabel">Caste & Sub-Caste : </span><span id="cstCaste&SubCaste" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblCasteSubCaste" runat="server" ViewStateMode="Enabled" TextMode="Caste & Sub-Caste" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblCategory" class="ClsLabel">Category : </span><span id="cstCategory" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblSub" runat="server" ViewStateMode="Enabled" TextMode="Caste & Sub-Caste" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblUDISE" class="ClsLabel">UDISEnumber : </span><span id="cstUDISE" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblUDISENO" runat="server" ViewStateMode="Enabled" TextMode="UDISE NO" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblBirth" class="ClsLabel">Place of Birth : </span><span id="cstBirth" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblBirthPlace" runat="server" ViewStateMode="Enabled" TextMode="Birth Place" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblNation" class="ClsLabel">Nationality : </span><span id="cstNationality" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblNationality" runat="server" ViewStateMode="Enabled" TextMode="Nationality" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblMother" class="ClsLabel">Mother Tongue :</span><span id="cstMother" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblMotherTongue" runat="server" ViewStateMode="Enabled" TextMode="Mother Tongue" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblBloogGroup" class="ClsLabel">Blood Group :</span><span id="cstBG" style="color: Red; display: none;"></span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblBG" runat="server" ViewStateMode="Enabled" TextMode="Blood Group" MaxLength="20"
                                        CssClass="ClsLabelPadding"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <span id="lblFamily" class="ClsLabel">Family Photo :</span><span id="cstFamily" style="color: Red; display: none;"></span>
                                </td>
                                <td>
                                    <asp:Image ID="imgFamilyPhoto" class="ClsBorderLight" ImageUrl="~/RITeSchool/images/Student_BlankPh.jpg" runat="server"
                                        Height="100px" Width="150px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        function OpenFamilyPhoto(FamilyPhotoPath) {            
            window.open(FamilyPhotoPath, '_new', 'scrollbars=yes,resizable=yes,top=100,left=100,width=800,height=600');
            return false;
        }

    </script>
</asp:Content>
