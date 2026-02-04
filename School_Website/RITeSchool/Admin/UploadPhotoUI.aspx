<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="UploadPhotoUI.aspx.cs" Inherits="ManageGalleryUI" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/themes/redmond/jquery-ui.css"
        rel="stylesheet" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
    <script src="../Scripts/jquery.youtubepopup.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("a.youtube").YouTubePopup({ autoplay: 0, modal: false });
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .LabelWidth
        {
            width:120px;
        }
        
        .LabelWidth1
        {
            width:120px;
        }
</style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 99%;">
            <tr>
                <td style="padding: 5px;">
                </td>
            </tr>
            <tr>
                <td style="background-color: white;" id="MainDataTable">
                    <cc1:CollapsablePanel ID="colpnlPhotoGallery" runat="server" TitleText="Photo Gallery"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <!-- Data Insert Here -->
                        <asp:UpdatePanel ID="UPnlPhotoGallery" runat="server">
                            <ContentTemplate>
                                <table width="100%" align="center">
                                    <tr>
                                        <td align="left">
                                            <table border="0" align="left" cellpadding="0" cellspacing="2" width="100%">
                                                <tr>
                                                    <td align="right" style="width: 23%; padding-right: 10px;">
                                                        <span class="ClsMdtStar">*&nbsp; Mandatory Fields</span>
                                                    </td>
                                                </tr>
                                                 
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:ValidationSummary ID="valSummaryPhotoDetails" runat="server" ValidationGroup="valGrpDetails"
                                                            CssClass="LblErrorMsg" />
                                                        <asp:ValidationSummary ID="valSummaryPhotoUpdate" runat="server" ValidationGroup="valGrpDetailsUpdate"
                                                            CssClass="LblErrorMsg" />
                                                        <asp:RequiredFieldValidator ID="reqGalleryName" runat="server" CssClass="ClsMdtStar"
                                                            ValidationGroup="valGrpDetailsUpdate" ControlToValidate="txtGalleryName" Display="None"
                                                            ErrorMessage="Gallery Name should not be blank."></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cst_GImages" runat="server" ClientValidationFunction="ImagesValidation"
                                                            ValidationGroup="valGrpDetailsUpdate" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                                                            ErrorMessage="At least one image should be selected." Visible="true"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="reg_GalleryName" runat="server" CssClass="ClsMdtStar" ClientValidationFunction="CheckForSplCharacters"
                                                            Display="None" ErrorMessage="Special characters like \ / : * ? < > | are not allowed in Gallery Name."
                                                            ValidationGroup="valGrpDetailsUpdate"></asp:CustomValidator>
                                                        <asp:CustomValidator Display="None" ID="cstStandard" runat="server" ClientValidationFunction="ClearErrorLabel"
                                                            CssClass="ClsMdtStar" ErrorMessage="At least one section should be associated for photo gallery."
                                                            ValidationGroup="valGrpDetailsUpdate"> </asp:CustomValidator>
                                                          <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateClasses"
                                                            ValidationGroup="valGrpDetailsUpdate" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                                                            ErrorMessage="" Visible="true"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr id="trDuplicatePhotoGallery" enableviewstate="false" runat="server" visible="false">
                                                    <td valign="middle" width="90%">
                                                        <asp:Label ID="lblDuplicatePhotoGallery" runat="server" CssClass="LblErrorMsg" Text="Gallery name already exists."
                                                            Visible="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trUpdate" enableviewstate="false" runat="server" visible="false">
                                                    <td valign="middle" width="90%" align="center">
                                                        <asp:Label ID="lblUpdate" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                                            Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <table align="center" border="0" cellpadding="0" cellspacing="1" width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <span class="ClsLblLgnd" style="font-weight: bold">Photo Gallery Details :</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" width="12%">
                                                                    <span class="ClsLabel">Gallery Name :</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtGalleryName" runat="server" MaxLength="50" CssClass="LrgTxtBox"
                                                                        Width="289px"></asp:TextBox>
                                                                    <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" width="15%">
                                                                    <span class="ClsLabel">Add More Photos :</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkAddMore" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                           <tr id="trAssociatedClass" runat="server" visible="true">
                                                                <td align="right" class="ClsBorderlight" valign="middle">
                                                                    <span class="LblRht colonPadding"></span>
                                                                    <asp:Label ID="lblAssociatedClass" runat="server" Text="Applicable to all staff members and selected Class(es) :" 
                                                                        CssClass="LblRht" EnableViewState="False"></asp:Label><br />
                                                                    
                                                                    <asp:CheckBox ID="chkAllDivs" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll%>"
                                                                        TabIndex="7" Style="padding-right: 5px" />
                                                                </td>
                                                                
                                                                
                                                                <td align="left">
                                                                    <asp:ListView ID="lstvwStandardDivisions" runat="server" DataKeyNames="StandardId"
                                                                        OnItemDataBound="lstvwStandardDivisions_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table align="left" width="auto" runat="server" id="tblStaffInfo" style="color: #333333;"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                             
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                                                                          <ItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                        CssClass="ClsLabel" RepeatColumns="6">
                                                                                    </asp:CheckBoxList>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height: 10px">
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                        CssClass="ClsLabel" RepeatColumns="6">
                                                                                    </asp:CheckBoxList>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table width="50%">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" align="center">
                                                                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"
                                                                                            EnableViewState="False"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                     <span class="ClsMdtStar" style="color: Red">*</span>
                                                                   
                                                                </td>
                                                              
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2" valign="bottom">
                                                                    <span class="ClsLblLgnd" style="font-weight: bold; width: 150px">Photo Gallery Details
                                                                        :</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2" valign="top">
                                                                    <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG with total size upto 10 MB.
                                                                       )</span>
                                                                    <asp:Label ID="lblFileMdtNotice" runat="server" CssClass="LblSmlGray">&nbsp; At least one file must be selected.</asp:Label>
                                                                    <p class="LblSmlGray" style="font-size:12px;padding-top:10px;"><b>You can select multiple files of a folder in any file upload control.</b></p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2">
                                                                    <table id="tblFileUpload" runat="server" width="100%">
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth1">
                                                                                <span class="ClsLabel">1. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage1" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment1" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg1" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth1">
                                                                                <span class="ClsLabel">2. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="flImage2" runat="server" CssClass="LrgTxtBox" EnableTheming="True" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtComment2" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg2" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth1">
                                                                                <span class="ClsLabel">3. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="flImage3" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtComment3" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg3" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth1">
                                                                                <span class="ClsLabel">4. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="flImage4" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtComment4" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg4" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth1">
                                                                                <span class="ClsLabel">5. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="flImage5" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtComment5" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg5" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table id="tblMoreFileUpload_1" width="100%" style="display:none;">
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth">
                                                                                <span class="ClsLabel">6. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage6" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment6" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg6" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth">
                                                                                <span class="ClsLabel">7. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage7" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment7" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg7" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth">
                                                                                <span class="ClsLabel">8. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage8" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment8" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg8" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth">
                                                                                <span class="ClsLabel">9. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage9" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment9" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg9" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">10. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage10" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment10" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg10" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>                                                                                                                                          
                                                                    </table>
                                                                    <table id="tblMoreFileUpload_2" width="100%" style="display:none;">
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">11. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage11" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment11" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg11" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">12. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage12" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment12" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg12" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">13. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage13" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment13" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg13" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">14. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage14" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment14" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg14" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">15. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage15" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment15" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg15" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>                                                                          
                                                                    </table>
                                                                    <table id="tblMoreFileUpload_3" width="100%" style="display:none;">
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">16. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage16" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true"/>
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment16" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg16" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">17. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage17" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment17" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg17" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">18. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage18" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment18" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg18" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">19. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage19" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment19" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg19" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight LabelWidth" >
                                                                                <span class="ClsLabel">20. Select Photo :</span>
                                                                            </td>
                                                                            <td align="left" width="200px">
                                                                                <asp:FileUpload ID="flImage20" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG" multiple="true" />
                                                                            </td>
                                                                            <td width="70px" align="right" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Comment :</span>
                                                                            </td>
                                                                            <td width="205px">
                                                                                <asp:TextBox ID="txtComment20" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                                                                    Width="100%"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" class="ClsPaddingL">
                                                                                <asp:Label ID="lblErrMsg20" runat="server" CssClass="LblErrorMsg" />
                                                                            </td>
                                                                        </tr>                                                                            
                                                                    </table>
                                                                    <table width="100%" id="tblLinkAddMore">
                                                                         <tr>
                                                                            <td align="left" width="90px">                                                                               
                                                                            </td>
                                                                            <td width="200px">                                                                               
                                                                            </td>
                                                                            <td width="70px">                                                                                
                                                                            </td>
                                                                            <td align="right" width="215px">                                                                               
                                                                                <asp:LinkButton ID="lnkDisplayMore" OnClientClick="DisplayControls();return false;" runat="server"><b>Add More Photos</b></asp:LinkButton>
                                                                            </td>
                                                                            <td>                                                                                
                                                                            </td>
                                                                        </tr>                                                                        
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" valign="top" style="white-space: nowrap;">
                                                                    <asp:Label ID="Label6" runat="server" Text="Associated Section(s):" CssClass="ClsLblLgnd"
                                                                        Style="padding-left: 100px; white-space: nowrap;" EnableViewState="False"></asp:Label><br />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"
                                                                        Style="white-space: nowrap;" onclick="CheckAllUncheckAlls()" />
                                                                </td>
                                                                <td class="ClsBorderlight" valign="top" align="left">
                                                                    <asp:CheckBoxList ID="chkSectionList" runat="server" RepeatDirection="Horizontal"
                                                                        CssClass="ClsLabel" RepeatColumns="1" onchange="UpdateDeleteCount(this);">
                                                                    </asp:CheckBoxList>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <div align="center" runat="server" style="width: 80%;">
                                                                        <asp:Button ID="btnPhotoAdd" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                                            OnClick="btnPhotoAdd_Click" Text="Add" UseSubmitBehavior="False" ValidationGroup="valGrpDetailsUpdate"
                                                                            OnClientClick="ClearErrorLabels()" />
                                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateFileType"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="" ValidationGroup="valGrpDetailsUpdate"></asp:CustomValidator>
                                                                            <asp:CustomValidator ID="csFileSizeTotal" runat="server" ClientValidationFunction="ValidateFileSize"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="" ValidationGroup="valGrpDetailsUpdate"></asp:CustomValidator>
                                                                        <asp:Button ID="btnPhotoCancel" runat="server" BorderStyle="Solid" CausesValidation="false"
                                                                            CssClass="ClsBtnMid" Text="Cancel" OnClick="btnPhotoCancel_Click" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <span class="ClsLblLgnd" style="font-weight: bold">Existing image galleries :</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPhotoGalleryRowCount" runat="server">
                                                                            <td align="left">
                                                                                <div style="width: 80%; text-align: center">
                                                                                    <asp:Label ID="lblFirstIndex" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">to</span>
                                                                                    <asp:Label ID="lblLastIndex" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">out of</span>
                                                                                    <asp:Label ID="lblTotalPhotos" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">records</span>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:GridView ID="grdPhotoGallery" runat="server" AllowPaging="True" AllowSorting="True"
                                                                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" CssClass="GridBorder"
                                                                                    DataSourceID="ObjectDSPhotoGallery" EmptyDataText="No photo gallery available."
                                                                                    ForeColor="#333333" GridLines="None" OnRowCommand="grdPhotoGallery_RowCommand"
                                                                                    OnRowDataBound="grdPhotoGallery_RowDataBound" PageSize="20" Width="85%" OnRowCreated="grdPhotoGallery_RowCreated"
                                                                                    OnPageIndexChanging="grdPhotoGallery_PageIndexChanging" OnSorting="grdPhotoGallery_Sorting">
                                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Gallery_Name" HeaderText="Gallery Name" SortExpression="Gallery_Name">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Classes" HeaderText="Class Name" SortExpression="Classes">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Update_Date" HeaderText="Last Updated Date" SortExpression="Update_Date"
                                                                                            HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="18%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="18%" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="View Images">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnViewImageGallery" class="youtube" OnClientClick="" runat="server"
                                                                                                    CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>" CommandName="VIEW_PHOTO_GALLERY"
                                                                                                    ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Slide Show">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnSlideShow" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                    CommandName="SLIDE_SHOW" ImageUrl="~/RITeSchool/images/GridIcon_Slideshow.gif" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Download">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                    CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/download_transparent.png" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Height="20px" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:ButtonField ButtonType="Image" CommandName="EDITROW" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                                            Text="Edit">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:TemplateField HeaderText="Delete">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnDeleteImageGallery" runat="server" CausesValidation="false"
                                                                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="DELETEROW" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <PagerTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="left" class="ClsBorderPager" valign="middle" width="70%">
                                                                                                    <span class="LblNrmlB">Select a page:</span>
                                                                                                    <asp:DropDownList ID="PhotoGalleryPageDDList" runat="server" AutoPostBack="true"
                                                                                                        CssClass="LblNormal" OnSelectedIndexChanged="PhotoGalleryPageDDList_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right" class="ClsBorderPager" valign="middle" width="30%">
                                                                                                    <asp:Label ID="PhotoGalleryCurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </PagerTemplate>
                                                                                </asp:GridView>
                                                                                <asp:HiddenField ID="hidOrgGalleryName" runat="server" />
                                                                                <asp:HiddenField ID="hidPhotoGallerySortExpression" runat="server" />
                                                                                <asp:HiddenField ID="hidPhotoGallerySortDirection" runat="server" />
                                                                                <asp:HiddenField ID="hidHolidayId" runat="server" />
                                                                                <asp:HiddenField ID="hidChkLstCnt" runat="server" />
                                                                                 <asp:HiddenField ID="hidPhotoGalleryPerClasswise" runat="server" />
                                                                                 <asp:HiddenField ID="hidShowPhotoUploadCount" runat="server" Value="1" />
                                                                                 <asp:HiddenField ID="hidShowVideoUploadCount" runat="server" Value="1" />
                                                                                <asp:ObjectDataSource ID="ObjectDSPhotoGallery" runat="server" EnablePaging="True"
                                                                                    OnSelected="ObjectDSPhotoGallery_Selected" SelectCountMethod="CountPhotoGalleries"
                                                                                    SelectMethod="GetPhotoGalleryDetails" TypeName="BusinessLogic.ImageGalleryCollectionBL">
                                                                                    <SelectParameters>
                                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="S_SESSION_SCHOOL_ID" Type="Int32" />
                                                                                        <asp:SessionParameter Name="aiAccYrId" SessionField="S_SESSION_CURRENT_ACADEMIC_YEAR_ID"
                                                                                            Type="Int32" />
                                                                                        <asp:ControlParameter Name="sortExp" ControlID="hidPhotoGallerySortExpression" Type="String"
                                                                                            PropertyName="Value" DefaultValue="" />
                                                                                    </SelectParameters>
                                                                                </asp:ObjectDataSource>
                                                                            </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdPhotoGallery" EventName="Sorting" />
                                <asp:AsyncPostBackTrigger ControlID="btnPhotoCancel" EventName="Click" />
                                <asp:PostBackTrigger ControlID="btnPhotoAdd" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <!-- Data Insert End Here -->
                    </cc1:CollapsablePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <cc1:CollapsablePanel ID="colpnlVideoGallery" runat="server" TitleText="Video Gallery"
                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                        <table style="width: 100%;">
                            <tr id="tr1" enableviewstate="false" runat="server">
                                <td valign="middle" width="90%" align="center">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblVideoMessage" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                                Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdVideoGallery" EventName="RowCommand" />
                                            <asp:AsyncPostBackTrigger ControlID="grdVideoGallery" EventName="Sorting" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVideoCancel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVideoAdd" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UPanelVideoGallery" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" align="center" cellpadding="0" cellspacing="2" style="width: 100%;">
                                                <tr>
                                                    <td align="right" style="width: 23%; padding-right: 10px;">
                                                        <span style="color: red;" class="ClsMdtStar">*&nbsp; Mandatory Fields</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary runat="server" ID="valSummaryVideo" ValidationGroup="OnVideoAddClick"
                                                            CssClass="LblErrorMsg" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table align="left" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                                            <tr id="trDuplicateVideoGalleryname" enableviewstate="false" runat="server" visible="false">
                                                                <td>
                                                                    <asp:Label ID="lblDuplicatevideo" runat="server" CssClass="LblErrorMsg" Text="Video name already exists."
                                                                        Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 25px">
                                                                    <span class="ClsLblLgnd" style="font-weight: bold; width: 200px">Video Gallery Details
                                                                        :</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <table align="left" width="80%">
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight">
                                                                                <span class="ClsLabel">Video Name :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtVideoName" runat="server" Width="40%" CssClass="LrgTxtBox" MaxLength="50"></asp:TextBox>
                                                                                <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trStartDate" runat="server" visible="false">
                                                                             <td align="left" class="ClsBorderlight">
                                                                                  <span class="ClsLabel">Start Date :</span>
                                                                              </td>
                                                                              <td align="left">
                                                                                  <asp:TextBox ID="txtStartDate" CssClass="ExSmlTxtBox" Width="90px" runat="server"></asp:TextBox>
                                                                                  <rjs:PopCalendar ID="cal_StartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                                      Culture="en" ShowWeekend="True" AutoPostBack="False" />                                                                                            
                                                                                <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                                              </td>
                                                                        </tr>
                                                                        <tr id="trEndDate" runat="server" visible="false">           
                                                                               <td align="left" class="ClsBorderlight">
                                                                                   <span class="ClsLabel">End Date :</span>
                                                                               </td>
                                                                               <td align="left">
                                                                                   <asp:TextBox ID="txtEndDate" CssClass="ExSmlTxtBox" Width="90px" runat="server"></asp:TextBox>
                                                                                   <rjs:PopCalendar ID="cal_EndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                                                       Culture="en" ShowWeekend="True" AutoPostBack="False" />  
                                                                                     <span class="ClsMdtStar" style="color: #ff0000">*</span>                                                                                          
                                                                               </td>                                                                                    
                                                                        </tr> 
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight" valign="top" style="white-space: nowrap;">
                                                                                <asp:Label ID="Label1" runat="server" Text="Associated User Role(s):" CssClass="ClsLblLgnd"
                                                                                    Style="padding-left: 100px; white-space: nowrap;" EnableViewState="False"></asp:Label><br />
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                                                                                <asp:CheckBox ID="chkAllForVideo" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>"
                                                                                    Style="white-space: nowrap;" onclick="CheckAllForVideoGallery()" />
                                                                            </td>
                                                                            <td class="ClsBorderlight" valign="top" align="left">
                                                                                <asp:CheckBoxList ID="chkUserRoleLst" runat="server" RepeatDirection="Horizontal"
                                                                                    CssClass="ClsLabel" RepeatColumns="2" Width="50%">
                                                                                </asp:CheckBoxList>
                                                                                &nbsp;
                                                                            </td>                                                                            
                                                                        </tr> 
                                                                        <tr id="tr2" runat="server" visible="true">
                                                                            <td align="right" class="ClsBorderlight" valign="middle">
                                                                                <span class="LblRht colonPadding"></span>
                                                                                <asp:Label ID="Label2" runat="server" Text="Applicable to selected Class(es) :" 
                                                                                    CssClass="LblRht" EnableViewState="False"></asp:Label><br />
                                                                    
                                                                                <asp:CheckBox ID="chkAllDivForVdo" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll%>"
                                                                                    TabIndex="7" Style="padding-right: 5px" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ListView ID="lstvwVideoStandardDivision" runat="server" 
                                                                                    DataKeyNames="StandardId" 
                                                                                    onitemdatabound="lstvwVideoStandardDivision_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table align="left" width="auto" runat="server" id="tblStaffInfo" style="color: #333333;"
                                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                             
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                                                                                      <ItemTemplate>
                                                                                        <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left" style="padding-left: 5px">
                                                                                                <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                                            </td>
                                                                                            <td align="left" style="padding-left: 5px">
                                                                                                <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                                    CssClass="ClsLabel" RepeatColumns="6">
                                                                                                </asp:CheckBoxList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height: 10px">
                                                                                            <td align="left" style="padding-left: 5px">
                                                                                                <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                                                                            </td>
                                                                                            <td align="left" style="padding-left: 5px">
                                                                                                <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                                    CssClass="ClsLabel" RepeatColumns="6">
                                                                                                </asp:CheckBoxList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                    <EmptyDataTemplate>
                                                                                        <table width="50%">
                                                                                            <tr>
                                                                                                <td class="LblNoRecord" align="center">
                                                                                                    <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"
                                                                                                        EnableViewState="False"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:ListView>
                                                                                 <span class="ClsMdtStar" style="color: Red">*</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trSubjectDetails" runat="server">
                                                                            <td style="width: 225px;" class="ClsBorderlight">
                                                                                <asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Text="Subjects" Height="16px"></asp:Label>
                                                                                <span class="ClsLabel colonPadding">:</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="cmbSubject" runat="server" CssClass="MidCombo">
                                                                                </asp:DropDownList>                                                                               
                                                                            </td>
                                                                        </tr>  
                                                                         <tr id="tr4" runat="server">
                                                                            <td style="width: 225px;" class="ClsBorderlight">
                                                                                <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Url Source" Height="16px"></asp:Label>
                                                                                <span class="ClsLabel colonPadding">:</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlUrlSource" runat="server" CssClass="MidCombo">
                                                                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                               <asp:ListItem Value="1">YouTube</asp:ListItem>
                                                                                  <asp:ListItem Value="2">RITeSchool</asp:ListItem>
                                                                                 <%-- <asp:ListItem Value="MediaService"> Media Service </asp:ListItem>--%>
                                                                                </asp:DropDownList> 
                                                                                 <span class="ClsMdtStar" style="color: Red">*</span>      
                                                                                <asp:LinkButton ID="lnkRITeSchoolVideo" runat="server" OnClientClick="OpenWebsite(); return false;"
                                                                                    >Click here to upload video's on RITeSchool source</asp:LinkButton>                                                                     
                                                                            </td>
                                                                        </tr>                                                        
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <table width="100%" id="tblMoreVideoUpload" runat="server">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                                                            <span class="ClsLabel">1. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left" style="width: 250px;">
                                                                                            <asp:TextBox ID="txtVideoUrl1" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment1" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">2. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl2" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment2" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">3. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl3" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment3" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">4. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl4" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment4" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">5. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl5" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment5" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table width="100%" id="tblMoreVideoUpload_1" style="display:none;">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                                                            <span class="ClsLabel">6. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left" style="width: 250px;">
                                                                                            <asp:TextBox ID="txtVideoUrl6" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment6" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">7. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl7" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment7" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">8. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl8" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment8" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">9. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl9" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment9" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">10. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl10" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment10" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table width="100%" id="tblMoreVideoUpload_2" style="display:none;">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                                                            <span class="ClsLabel">11. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left" style="width: 250px;">
                                                                                            <asp:TextBox ID="txtVideoUrl11" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment11" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">12. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl12" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment12" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">13. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl13" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment13" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">14. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl14" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment14" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">15. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl15" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment15" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table width="100%" id="tblMoreVideoUpload_3" style="display:none;">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                                                            <span class="ClsLabel">16. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left" style="width: 250px;">
                                                                                            <asp:TextBox ID="txtVideoUrl16" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment16" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">17. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl17" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment17" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">18. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl18" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment18" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">19. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl19" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment19" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">20. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl20" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment20" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table width="100%" id="tblMoreVideoUpload_4" style="display:none;">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                                                            <span class="ClsLabel">21. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left" style="width: 250px;">
                                                                                            <asp:TextBox ID="txtVideoUrl21" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment21" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">22. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl22" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment22" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">23. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl23" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment23" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">24. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl24" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                            
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment24" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight">
                                                                                            <span class="ClsLabel">25. Video Url :</span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVideoUrl25" runat="server" CssClass="LrgTxtBox" MaxLength="3000"
                                                                                                Width="250px"></asp:TextBox>                                                                                           
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="width: 70px;">
                                                                                            <span class="ClsLabel">Title : </span>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtVidoComment25" runat="server" CssClass="LrgTxtBox" MaxLength="100"
                                                                                                Width="300px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr> 
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <table width="100%" id="tblLinkAddMoreVideos">
                                                                                     <tr>
                                                                                        <td align="left" width="90px">                                                                               
                                                                                        </td>
                                                                                        <td width="200px">                                                                               
                                                                                        </td>
                                                                                        <td width="70px">                                                                                
                                                                                        </td>
                                                                                        <td align="right" width="215px">                                                                               
                                                                                            <asp:LinkButton ID="lnkAddMoveVideos" OnClientClick="DisplayVideoControls();return false;" runat="server"><b>Add More Videos</b></asp:LinkButton>
                                                                                        </td>
                                                                                        <td>                                                                                
                                                                                        </td>
                                                                                    </tr>                                                                        
                                                                                </table>
                                                                            </td>
                                                                        </tr>                                                                   
                                                                        <tr id="trAddMoreVideos" runat="server" visible="false">
                                                                            <td align="left" class="ClsBorderlight" width="15%">
                                                                                <span class="ClsLabel">Add More Videos :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CheckBox ID="chkAddMoreVideos" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="tr3" runat="server">
                                                                            <td align="left" class="ClsBorderlight" width="15%">
                                                                                <span class="ClsLabel">Show On External Website? :</span>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CheckBox ID="chkShowOnExternal" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight " style="background-color: #ffffc4; padding: 3px;
                                                                                width: 17%">
                                                                                <span class="LblNrmlB" style="font-weight: bold; height: 16px;">Note 1 :</span>
                                                                            </td>
                                                                            <td align="left" class="ClsBorderlight" style="padding: 3px; width: 80%">
                                                                                <div id="div" style="font-family: Verdana; font-size: 8pt; border: 100%;">
                                                                                    Video should be from www.youtube.com, Url Example: http://www.youtube.com/v/bAUT_Pux73w.
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight " style="background-color: #ffffc4; padding: 3px;
                                                                                width: 17%">
                                                                                <span class="LblNrmlB" style="font-weight: bold; height: 16px;">Note 2 :</span>
                                                                            </td>
                                                                            <td align="left" class="ClsBorderlight" style="padding: 3px; width: 80%">
                                                                                <div id="div1" style="font-family: Verdana; font-size: 8pt; border: 100%;">
                                                                                    When you edit any gallery then change done in gallery name, dates, user roles and classes will be applicable for all subjects of respective gallery.
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>                                                                     
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="height: 19px;" valign="top">
                                                                    <table width="80%">
                                                                        <tr>
                                                                            <td width="30%" align="right">
                                                                                <asp:Button ID="btnVideoAdd" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                                                    OnClick="btnVideoAdd_Click" Text="Add" UseSubmitBehavior="false" ValidationGroup="OnVideoAddClick"
                                                                                    OnClientClick="ClearErrorLabels()" />
                                                                            </td>
                                                                            <td width="30%">
                                                                                <asp:Button ID="btnVideoCancel" runat="server" BorderStyle="Solid" CssClass="ClsBtnMid"
                                                                                    OnClick="btnVideoCancel_Click" Text="Cancel" CausesValidation="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <span class="ClsLblLgnd" style="font-weight: bold">Existing Videos List :</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trTotalRec" runat="server">
                                                                            <td align="left">
                                                                                <div style="width: 80%; text-align: center">
                                                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">to</span>
                                                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">out of</span>
                                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                                    <span class="LblNormal">records</span>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:GridView ID="grdVideoGallery" runat="server" AllowPaging="True" AllowSorting="True"
                                                                                    AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" CssClass="GridBorder"
                                                                                      DataKeyNames="Video_Id,Video_Url,SubjectId,Subject_Name,UrlSourceId,URLSource" EmptyDataText="No video available." ForeColor="#333333"
                                                                                    GridLines="None" OnPageIndexChanging="grdVideoGallery_PageIndexChanging" OnRowCommand="grdVideoGallery_RowCommand"
                                                                                    OnRowCreated="grdVideoGallery_RowCreated" OnRowDataBound="grdVideoGallery_RowDataBound"
                                                                                    PageSize="20" Width="90%" DataSourceID="ObjectDSVideoGallery" OnSorting="grdVideoGallery_Sorting">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Video_Name" HeaderText="Video Name" SortExpression="Video_Name">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="500px" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                                Width="500px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Subject_Name" HeaderText="Subject Name">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="500px" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                                Width="500px" />
                                                                                        </asp:BoundField>                                                                                        
                                                                                        <asp:BoundField DataField="Update_Date" HeaderText="Last Updated Date" SortExpression="Update_Date"
                                                                                            HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="250px" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="250px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate"
                                                                                            HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate"
                                                                                            HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="URLSource" HeaderText="URL Source">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="150px" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                                Width="150px" />
                                                                                        </asp:BoundField>  
                                                                                        <asp:ButtonField ButtonType="Image" CommandName="ADDSUBJECT" HeaderText="Add For Another Subject" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                                            Text="Edit">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="300px" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="300px" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:TemplateField HeaderText="View">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnViewVideoGallery" class="youtube" OnClientClick="" runat="server"
                                                                                                    CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>" CommandName="VIEW_PHOTO_GALLERY"
                                                                                                    ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:ButtonField ButtonType="Image" CommandName="EDITROW" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                                            Text="Edit">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:TemplateField HeaderText="Delete">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btnDeleteVideo" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                    CommandName="DELETEROW" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" HorizontalAlign="Center" CssClass="LblNoRecord" />
                                                                                    <PagerTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="left" class="ClsBorderPager" valign="middle" width="70%">
                                                                                                    <span class="LblNrmlB">Select a page:</span>
                                                                                                    <asp:DropDownList ID="PageDropDownList" runat="server" AutoPostBack="true" CssClass="LblNormal"
                                                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td align="right" class="ClsBorderPager" valign="middle" width="30%">
                                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </PagerTemplate>
                                                                                </asp:GridView>
                                                                                <asp:HiddenField ID="hidVedioId" runat="server" Value="0" />
                                                                                <asp:HiddenField ID="hidOldSubjectId" runat="server" Value="0" />
                                                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                                <asp:HiddenField ID="hidIsAdded" runat="server" />
                                                                                <asp:HiddenField ID="hidAddMoreSubjects" runat="server" Value="0" />
                                                                                <asp:HiddenField ID="hidVideoWebsiteURL" runat="server" Value="" />
                                                                                <asp:ObjectDataSource ID="ObjectDSVideoGallery" runat="server" EnablePaging="True"
                                                                                    OnSelected="ObjectDSVideoGallery_Selected" SelectCountMethod="CountFromVedioList"
                                                                                    SelectMethod="GetVideoGalleryDetails" TypeName="BusinessLogic.VideoGalleryCollectionBL">
                                                                                    <SelectParameters>
                                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="S_SESSION_SCHOOL_ID" Type="Int32" />
                                                                                        <asp:SessionParameter Name="aiAccYrId" SessionField="S_SESSION_CURRENT_ACADEMIC_YEAR_ID"
                                                                                            Type="Int32" />
                                                                                        <asp:ControlParameter Name="sortExp" ControlID="hidSortExpression" Type="String"
                                                                                            PropertyName="Value" DefaultValue="" />
                                                                                    </SelectParameters>
                                                                                </asp:ObjectDataSource>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="grdVideoGallery" EventName="Sorting" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVideoCancel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnVideoAdd" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </cc1:CollapsablePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <%--Video gallery Validations--%>
                    <asp:RequiredFieldValidator ID="reqVideoName0" runat="server" ControlToValidate="txtVideoName"
                        CssClass="LblErrorMsg" ErrorMessage="Video name should not be blank." ValidationGroup="OnVideoAddClick"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="reqVideoStartDate" runat="server" ControlToValidate="txtStartDate"
                        CssClass="LblErrorMsg" ErrorMessage="Start Date should not be blank." ValidationGroup="OnVideoAddClick"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="reqVideoEndDate" runat="server" ControlToValidate="txtEndDate"
                        CssClass="LblErrorMsg" ErrorMessage="End Date should not be blank." ValidationGroup="OnVideoAddClick"
                        Display="None"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ClientValidationFunction="ValidateUserRoleForVideo"
                        ValidationGroup="OnVideoAddClick" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                        ErrorMessage="" Visible="true"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateClassesForVideo"
                        ValidationGroup="OnVideoAddClick" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                        ErrorMessage="" Visible="true"></asp:CustomValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" Display="None"
                        ErrorMessage="URL Source should be selected." 
                        ControlToValidate="ddlUrlSource" ValidationGroup="OnVideoAddClick" 
                        ValueToCompare="0" Operator="NotEqual"></asp:CompareValidator>
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ClientValidationFunction="ValidateVideoTitle"
                        ValidationGroup="OnVideoAddClick" CssClass="ClsMdtStar" Display="None" EnableClientScript="true"
                        ErrorMessage="" Visible="true"></asp:CustomValidator>                    
                    <%--<asp:RequiredFieldValidator ID="reqVideoUrl" runat="server" ControlToValidate="txtVideoUrl"
                        CssClass="LblErrorMsg" ErrorMessage="Video url should not be blank." ValidationGroup="OnVideoAddClick"
                        Display="None"></asp:RequiredFieldValidator>--%>
                    <%--<asp:RegularExpressionValidator ID="regVideoUrl" runat="server" ControlToValidate="txtVideoUrl"
                        CssClass="LblErrorMsg" ErrorMessage="Video url should be in correct format."
                        ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" ValidationGroup="OnVideoAddClick"
                        Display="None"></asp:RegularExpressionValidator>--%>
                    <%--Video gallery Validations compleated--%>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        var blanks = " \t\n\r";  // Ek whitespace chars
        _ClientGridId = "<%=this.grdPhotoGallery.ClientID %>";
        _clienttxtGalleryName = "<%=this.txtGalleryName.ClientID %>";
        _clienttrDuplicateVideoGalleryname = "<%=this.trDuplicateVideoGalleryname.ClientID %>";
        _clientflImage1 = "<%=this.flImage1.ClientID %>";
        _clientflImage2 = "<%=this.flImage2.ClientID %>";
        _clientflImage3 = "<%=this.flImage3.ClientID %>";
        _clientflImage4 = "<%=this.flImage4.ClientID %>";
        _clientflImage5 = "<%=this.flImage5.ClientID %>";
        _clientflImage6 = "<%=this.flImage6.ClientID %>";
        _clientflImage7 = "<%=this.flImage7.ClientID %>";
        _clientflImage8 = "<%=this.flImage8.ClientID %>";
        _clientflImage9 = "<%=this.flImage9.ClientID %>";
        _clientflImage10 = "<%=this.flImage10.ClientID %>";
        _clientflImage11 = "<%=this.flImage11.ClientID %>";
        _clientflImage12 = "<%=this.flImage12.ClientID %>";
        _clientflImage13 = "<%=this.flImage13.ClientID %>";
        _clientflImage14 = "<%=this.flImage14.ClientID %>";
        _clientflImage15 = "<%=this.flImage15.ClientID %>";
        _clientflImage16 = "<%=this.flImage16.ClientID %>";
        _clientflImage17 = "<%=this.flImage17.ClientID %>";
        _clientflImage18 = "<%=this.flImage18.ClientID %>";
        _clientflImage19 = "<%=this.flImage19.ClientID %>";
        _clientflImage20 = "<%=this.flImage20.ClientID %>";

        _ClientVideoValSum = "<%=this.valSummaryVideo.ClientID %>";
        _ClientImageDuplicate = "<%=this.trDuplicatePhotoGallery.ClientID %>";
        _ClientImageUpdate = "<%=this.trUpdate.ClientID %>";
        _clientPhotoAdd = "<%=this.btnPhotoAdd.ClientID %>";
        _clientcst_GImages = "<%=this.cst_GImages.ClientID %>";
        _clientPhotoCancel = "<%=this.btnPhotoCancel.ClientID%>";
        _clientVideoAdd = "<%=this.btnVideoAdd.ClientID%>";
        _clientVideoCancel = "<%=this.btnVideoCancel.ClientID%>";
        _clienthidIsAdded = "<%=this.hidIsAdded.ClientID%>";
        _clienthidChkLstCnt = "<%=this.hidChkLstCnt.ClientID %>"
        _clientlstvwStandardDivisions = "<%=this.lstvwStandardDivisions.ClientID %>"
        _clientchkSelectAll = "<%=this.chkSelectAll.ClientID %>"
        _clientchkAllForVideo ="<%=this.chkAllForVideo.ClientID %>"
        _clientchkStandardLst = "<%=this.chkSectionList.ClientID %>";
        _clientchkUserRoleLst = "<%=this.chkUserRoleLst.ClientID %>"
        _clientchkAllDivs = "<%=this.chkAllDivs.ClientID %>"
        _clientchkAllDivForVdo = "<%=this.chkAllDivForVdo.ClientID %>"
        _clienthidPhotoGalleryPerClasswise = "<%=this.hidPhotoGalleryPerClasswise.ClientID %>"
               
            function fnover(varname) {
                var objTXT = document.getElementById(varname)
                objTXT.style.borderWidth = "1";
                objTXT.style.borderColor = "maroon";
                objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
            }

            function fnout(varname) {
                var objTXT = document.getElementById(varname)
                objTXT.style.borderWidth = "1";
                objTXT.style.borderColor = "#a3c07b";
                objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
            }

            //This function is used to check whrther at least one image is selected to upload or not.
            function ImagesValidation(oSrc, args) {
                ClearErrorLabels();
                var buttonText = document.getElementById(_clientPhotoAdd).value;
                if (buttonText != null && buttonText == "Add") {
                    if (document.getElementById(_clientflImage1).value == '' &&
					document.getElementById(_clientflImage2).value == '' &&
					document.getElementById(_clientflImage3).value == '' &&
					document.getElementById(_clientflImage4).value == '' &&
					document.getElementById(_clientflImage5).value == '' &&
                    document.getElementById(_clientflImage6).value == '' &&
                    document.getElementById(_clientflImage7).value == '' &&
                    document.getElementById(_clientflImage8).value == '' &&
                    document.getElementById(_clientflImage9).value == '' &&
                    document.getElementById(_clientflImage10).value == '' &&
                    document.getElementById(_clientflImage11).value == '' &&
                    document.getElementById(_clientflImage12).value == '' &&
                    document.getElementById(_clientflImage13).value == '' &&
                    document.getElementById(_clientflImage14).value == '' &&
                    document.getElementById(_clientflImage15).value == '' &&
                    document.getElementById(_clientflImage16).value == '' &&
                    document.getElementById(_clientflImage17).value == '' &&
                    document.getElementById(_clientflImage18).value == '' &&
                    document.getElementById(_clientflImage19).value == '' &&
                    document.getElementById(_clientflImage20).value == '') {
                        args.IsValid = false;
                        return true;
                    }
                    else {
                        if (!IsValidFilePath(document.getElementById(_clientflImage1).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage2).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage3).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage4).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage5).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage6).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage7).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage8).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage9).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage10).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage11).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage12).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage13).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage14).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage15).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage16).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage17).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage18).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage19).value)
                        || !IsValidFilePath(document.getElementById(_clientflImage20).value)) {
                            $get(_clientcst_GImages).errormessage = "File path should be valid";
                            args.IsValid = false;
                            return true;
                        }
                        else {
                            args.IsValid = true;
                            return false;
                        }
                    }
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }

            function IsValidFilePath(val) {
                var retVal = true;
                return retVal;
            }

            //This function is used to clear error labels
            function ClearErrorLabels() {
                if ($get("<%=this.lblErrMsg1.ClientID %>") != null)
                    $get("<%=this.lblErrMsg1.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg2.ClientID %>") != null)
                    $get("<%=this.lblErrMsg2.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg3.ClientID %>") != null)
                    $get("<%=this.lblErrMsg3.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg4.ClientID %>") != null)
                    $get("<%=this.lblErrMsg4.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg5.ClientID %>") != null)
                    $get("<%=this.lblErrMsg5.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg6.ClientID %>") != null)
                    $get("<%=this.lblErrMsg6.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg7.ClientID %>") != null)
                    $get("<%=this.lblErrMsg7.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg8.ClientID %>") != null)
                    $get("<%=this.lblErrMsg8.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg9.ClientID %>") != null)
                    $get("<%=this.lblErrMsg9.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg10.ClientID %>") != null)
                    $get("<%=this.lblErrMsg10.ClientID %>").style.display = "none";

                if ($get("<%=this.lblErrMsg11.ClientID %>") != null)
                    $get("<%=this.lblErrMsg11.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg12.ClientID %>") != null)
                    $get("<%=this.lblErrMsg12.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg13.ClientID %>") != null)
                    $get("<%=this.lblErrMsg13.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg14.ClientID %>") != null)
                    $get("<%=this.lblErrMsg14.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg15.ClientID %>") != null)
                    $get("<%=this.lblErrMsg15.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg16.ClientID %>") != null)
                    $get("<%=this.lblErrMsg16.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg17.ClientID %>") != null)
                    $get("<%=this.lblErrMsg17.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg18.ClientID %>") != null)
                    $get("<%=this.lblErrMsg18.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg19.ClientID %>") != null)
                    $get("<%=this.lblErrMsg19.ClientID %>").style.display = "none";
                if ($get("<%=this.lblErrMsg20.ClientID %>") != null)
                    $get("<%=this.lblErrMsg20.ClientID %>").style.display = "none";
                
                if ($get("<%=this.lblDuplicatevideo.ClientID %>") != null)
                    $get("<%=this.lblDuplicatevideo.ClientID %>").style.display = "none";
                if ($get("<%=this.lblDuplicatePhotoGallery.ClientID %>") != null)
                    $get("<%=this.lblDuplicatePhotoGallery.ClientID %>").innerHTML = "";

            }
            //This function is used to validate gallery name.
            function GalleryNameValidationUpdate(oSrc, args) {
                var sGName = document.getElementById(_clienttxtGalleryName).value;
                sGName = stripLeadingTrailingBlanks(sGName);

                if (isEmpty(sGName)) {
                    ClearErrorLabels();
                    document.getElementById(_clienttxtGalleryName).errormessage = "Gallery Name should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }

            }


            //This function is used to check for special character in gallery name.
            function CheckForSplCharacters(oSrc, args) {
                var sGName = document.getElementById(_clienttxtGalleryName).value;
                sGName = stripLeadingTrailingBlanks(sGName);

                if (sGName.indexOf('|') >= 0 || sGName.indexOf(':') >= 0 || sGName.indexOf('?') >= 0 ||
            sGName.indexOf('>') >= 0 || sGName.indexOf('<') >= 0 || sGName.indexOf('\\') >= 0 ||
            sGName.indexOf('/') >= 0 || sGName.indexOf('"') >= 0 || sGName.indexOf('*') >= 0) {
                    args.IsValid = false;
                    return true;
                }
                else {
                    return false;
                }
            }

            //This function is used to clear video gallery validation summary.
            function ClearVideoValSum() {
                if (document.getElementById(_ClientVideoValSum) != null)
                    document.getElementById(_ClientVideoValSum).style.display = "none";

                if (document.getElementById(_clienttrDuplicateVideoGalleryname) != null)
                    document.getElementById(_clienttrDuplicateVideoGalleryname).style.display = "none";
                return true;
            }

            //This function is used to clear duplicate message.
            function ClearDuplicationMessage() {

                if (document.getElementById(_ClientImageDuplicate) != null)
                    document.getElementById(_ClientImageDuplicate).style.display = "none";
                return true;
            }

            //This function is used to clear duplicate message.
            function ClearUpdateMessage() {

                if (document.getElementById(_ClientImageUpdate) != null)
                    document.getElementById(_ClientImageUpdate).style.display = "none";
                return true;
            }
            //This function is used to confirmation of delete image
            function ConfirmDelete() {
                var bResult = true;
                {
                    if (!window.confirm("Are you sure you want to delete this image?"))
                    { bResult = false; }
                }
                return bResult;
            }
            //This function is used to display confirmation message to delete photo gallery.
            function ConfirmPhotoGalleryDelete() {
                var bResult = true;
                {
                    if (!window.confirm("Are you sure you want to delete this photo gallery?"))
                    { bResult = false; }
                }
                return bResult;
            }
            //This function is used to display confirmation message to delete video gallery.
            function ConfirmVideoDelete() {
                var bResult = true;
                {
                    if (!window.confirm("Are you sure you want to delete this video?"))
                    { bResult = false; }
                }
                return bResult;
            }
            //This function is used to display slide show of photo gallery.
            function ShowGallery(galName) {

                if (galName != null || galName != '')
                    window.open('../Gallery/ImageGallery.aspx?' + galName + '', '_blank', 'scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=20,width=950,height=700');

                return false;
            }
            //This function is used to display video gallery.
            function ShowVideoGallery(_VideoId) {
                alert('Hi..' + _VideoId);


                return false;
            }




            ////This function is used to display photos of photo gallery.
            function ShowPhotos(sEncryptedString) {
                window.open('UploadPhotoViewUI.aspx?' + sEncryptedString + '', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=10,left=10,width=1000,height=500,resizable=no');
                return false;
            }

            ////This function is used to display photos of photo gallery.
            function ShowVideos(sEncryptedString) {
                window.open('UploadVideoViewUI.aspx?' + sEncryptedString + '', '_self', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=10,left=10,width=1000,height=500,resizable=no');
                return false;
            }
            //This function is used to disable buttons.
            function DisableButtons(isVideoGallery) {
                ClearUpdateMessage();
                var isPageValid = true;
                if (isPageValid) {
                    if (document.getElementById(_clientPhotoCancel) != null)
                        document.getElementById(_clientPhotoCancel).disable = true;
                    if (document.getElementById(_clientPhotoAdd) != null)
                        document.getElementById(_clientPhotoAdd).disable = true;
                    if (document.getElementById(_clientVideoCancel) != null)
                        document.getElementById(_clientVideoCancel).disable = true;
                    if (document.getElementById(_clientVideoAdd) != null)
                        document.getElementById(_clientVideoAdd).disable = true;
                }
            }
            //This function is used for confirmation of adding more photoes
            function ConfirmUpdate() {
                var isPageValid = true;
                if (typeof (Page_ClientValidate) == 'function')
                    isPageValid = Page_ClientValidate("valGrpDetailsUpdate");
                if (isPageValid) {
                    DisableButtons();
                    if (!window.confirm('Do you want to add the more photos in the same gallery?')) {
                        document.getElementById(_clienthidIsAdded).value = "N";
                    }
                    else
                        document.getElementById(_clienthidIsAdded).value = "Y";
                }
            }

            function UrlOpenPopup() {
                window.open('../images/VideoGalleryNote.png', '')
            }


            function CheckAllUncheckAlls() {
                var checkAll;
                if (document.getElementById(_clientchkSelectAll) != null)
                    checkAll = document.getElementById(_clientchkSelectAll).checked

                var iRowCount = 0
                var chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount)
                while (chk != null) {
                    chk.checked = checkAll
                    iRowCount = iRowCount + 1;
                    chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount);
                }
            }

            function CheckAllForVideoGallery() {                
                var checkAll;
                if (document.getElementById(_clientchkAllForVideo) != null)
                    checkAll = document.getElementById(_clientchkAllForVideo).checked

                var iRowCount = 0
                var chk = document.getElementById(_clientchkUserRoleLst + "_" + iRowCount)
                while (chk != null) {
                    chk.checked = checkAll
                    iRowCount = iRowCount + 1;
                    chk = document.getElementById(_clientchkUserRoleLst + "_" + iRowCount);
                }
            }


            function CheckOrUncheckAllCheckBox() {
                var iCount = document.getElementById(_clienthidChkLstCnt).value
                var chkAll = document.getElementById(_clientchkAll).checked;
                for (i = 0; i < iCount; i++) {
                    document.getElementById(_clientchkStandardLst + "_" + i).checked = chkAll
                }
            }


            function ClearErrorLabel(oSrc, args) {
                var flag = 0
                var iRowCount = 0;
                var chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount)
                while (chk != null) {
                    if (chk.checked) {
                        flag = 1;
                        break;
                    }
                    else {
                        iRowCount = iRowCount + 1;
                        chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount);
                    }
                }

                if (flag == 0) {
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function UpdateDeleteCount(aCheckBox) {

                var iRowCount = 0;
                var flag = 1

                var chkAll = document.getElementById(_clientchkAll)
                var chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount)
                while (chk != null) {
                    if (!chk.checked) {
                        flag = 0
                        break;
                    }
                    else {
                        iRowCount = iRowCount + 1;
                        chk = document.getElementById(_clientchkStandardLst + "_" + iRowCount);
                    }
                }
                chkAll.checked = (flag == 1);
            }


            function ValidateClasses(oSrc, args) {
                var isFound = false

                if ($('#' + _clienthidPhotoGalleryPerClasswise).val() == "1") {
                    if ($('[id*=chkStandardDivLst]:checked').length == 0) {
                        oSrc.errormessage = "At least one class should be selected.";
                        args.IsValid = false
                        return true
                    }
                }
                args.IsValid = true
                return false
            }

            function ValidateClassesForVideo(oSrc, args) {
                if ($('[id*=chkvideoStandardDivLst]:checked').length == 0) {
                    oSrc.errormessage = "At least one Class should be selected for Video Gallery.";
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }

            function ValidateUserRoleForVideo(oSrc, args) {
                if ($('[id*=chkUserRoleLst]:checked').length == 0) {
                    oSrc.errormessage = "At least one User Role should be selected for Video Gallery.";
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }

            function ValidateVideoTitle(oSrc, args) {            
            if ($('[id$=hidVedioId]').val() == '0' || $('[id$=hidAddMoreSubjects]').val() == '1') {
                    var index = 1;
                    var isRecordFound = false
                    var message = '';
                    $('[id*=txtVideoUrl]').each(function () {
                        if ($(this).val() != '') {
                            isRecordFound = true;
                            var id = $(this)[0].id;
                            id = id.replace('ctl00_MainBody_txtVideoUrl', '')
                            if ($('[id$=txtVidoComment' + id + ']').val().trim() == '')
                                message = message + ', ' + index
                        }

                        index++
                    })

                    if (message.length > 0) {
                        message = message.substring(2);
                        oSrc.errormessage = "Video title should not be blank if you set video URL for row(s) : " + message;
                        args.IsValid = false
                        return true
                    }
                    else if (!isRecordFound) {
                        oSrc.errormessage = "At least one Video URL should be set";
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true
                return false
            }

            function CheckAll(obj, index) {
                var id = 'ctrl'+index+'_chkStandardDivLst_'
                if (obj.checked) {
                    $('[id*=' + id + ']').attr('checked', 'checked')
                }
                else {
                    $('[id*=' + id + ']').removeAttr('checked')
                }

                CheckMain();
            }

            function CheckMain() {
                if ($('[id$=chkStandard]').length == $('[id$=chkStandard]:checked').length)
                    $('[id$=chkAllDivs]').attr('checked', 'checked')
                else
                    $('[id$=chkAllDivs]').removeAttr('checked')
            }

            function CheckAllForVideo(obj, index) {
                var id = 'ctrl' + index + '_chkvideoStandardDivLst_'               
                $('[id*=' + id + ']').prop('checked', obj.checked)
                CheckMainForVideo();
            }

            function CheckMainForVideo() {            
                if ($('[id$=chkVdoStandard]').length == $('[id$=chkVdoStandard]:checked').length)
                    $('[id$=chkAllDivForVdo]').prop('checked', true)
                else
                    $('[id$=chkAllDivForVdo]').prop('checked', false)

            }            

            function SelectAllDivisions(obj) {
                if (obj.checked) {
                    $('[id$=chkStandard]').attr('checked', 'checked')
                    $('[id*=chkStandardDivLst]').attr('checked', 'checked')
                }
                else {
                    $('[id$=chkStandard').removeAttr('checked')
                    $('[id*=chkStandardDivLst]').removeAttr('checked')
                }
            }

            function SelectAllDivisionsForVideo(obj) {
                $('[id$=chkVdoStandard]').prop('checked', obj.checked)
                $('[id*=chkvideoStandardDivLst]').prop('checked', obj.checked)

            }

            function CheckStd(index) {            
                var classId = 'ctrl' + index + '_chkStandardDivLst_'
                var stdId = 'ctrl' + index + '_chkStandard'

                if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                    $('[id$=' + stdId + ']').attr('checked', 'checked')
                else
                    $('[id$=' + stdId + ']').removeAttr('checked')

                CheckMain();
            }

            function CheckStdForVideo(index) {
                var classId = 'ctrl' + index + '_chkvideoStandardDivLst_'
                var stdId = 'ctrl' + index + '_chkVdoStandard'

                if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                    $('[id$=' + stdId + ']').prop('checked', true)
                else
                    $('[id$=' + stdId + ']').prop('checked', false)

                CheckMainForVideo();
            }
            
                     
            </script>
          <script type="text/javascript">

              function DisplayControls() {              
                  _clienthidShowPhotoUploadCount = "<%=this.hidShowPhotoUploadCount.ClientID %>"
                  var sValue =parseInt($('#' + _clienthidShowPhotoUploadCount).val());
                  $("#tblMoreFileUpload_" + sValue).show();                  
                  $('#' + _clienthidShowPhotoUploadCount).val(sValue + 1);

                  if (sValue == 3) {
                      $("#tblLinkAddMore").hide();
                  }
              }

              function DisplayVideoControls() {
                  _clienthidShowVideoUploadCount = "<%=this.hidShowVideoUploadCount.ClientID %>"
                  var sValue = parseInt($('#' + _clienthidShowVideoUploadCount).val());

                  $("#tblMoreVideoUpload_" + sValue).show();
                  $('#' + _clienthidShowVideoUploadCount).val(sValue + 1);

                  if (sValue == 4) {
                      $("#tblLinkAddMoreVideos").hide();
                  }
              }

              function ValidateFileType(oSrc, args) {              
                  var numbers = ''
                  $('[id*=_flImage]').each(function () {

                      var files = $(this)[0].value;

                      if (files.trim() != '') {

                          var id = $(this)[0].id
                          var num = id.substr(id.indexOf('flImage')).replace('flImage', '')

                          var fileList = files.split(',')
                          for (var k = 0; k < fileList.length; k++) {
                              var file = fileList[k].trim()

                              var extension = file.substr(file.lastIndexOf('.')).toUpperCase()
                              if (extension != ".BMP" && extension != ".JPG" && extension != ".JPEG" && extension != ".PNG") {
                                  numbers = numbers + ',' + num
                                  break;
                              }
                          }
                      }
                  })

                  if (numbers.length > 0) {
                      numbers = numbers.substring(1)
                      oSrc.errormessage = "File type should be in only BMP, .JPG, .JPEG and .PNG format for file upload control : " + numbers;
                      args.IsValid = false;
                      return true;
                  }

                  args.IsValid = true;
                  return false;
              }

              function ValidateFileSize(oSrc, args) {
                  
                  var FileUpload1 = document.getElementById('<%=flImage1.ClientID %>')
                  var FileUpload2 = document.getElementById('<%=flImage2.ClientID %>')
                  var FileUpload3 = document.getElementById('<%=flImage3.ClientID %>')
                  var FileUpload4 = document.getElementById('<%=flImage4.ClientID %>')
                  var FileUpload5 = document.getElementById('<%=flImage5.ClientID %>')

                  var FileUpload6 = document.getElementById('<%=flImage6.ClientID %>')
                  var FileUpload7 = document.getElementById('<%=flImage7.ClientID %>')
                  var FileUpload8 = document.getElementById('<%=flImage8.ClientID %>')
                  var FileUpload9 = document.getElementById('<%=flImage9.ClientID %>')
                  var FileUpload10 = document.getElementById('<%=flImage10.ClientID %>')

                  var FileUpload11 = document.getElementById('<%=flImage11.ClientID %>')
                  var FileUpload12 = document.getElementById('<%=flImage12.ClientID %>')
                  var FileUpload13 = document.getElementById('<%=flImage13.ClientID %>')
                  var FileUpload14 = document.getElementById('<%=flImage14.ClientID %>')
                  var FileUpload15 = document.getElementById('<%=flImage15.ClientID %>')

                  var FileUpload16 = document.getElementById('<%=flImage16.ClientID %>')
                  var FileUpload17 = document.getElementById('<%=flImage17.ClientID %>')
                  var FileUpload18 = document.getElementById('<%=flImage18.ClientID %>')
                  var FileUpload19 = document.getElementById('<%=flImage19.ClientID %>')
                  var FileUpload20 = document.getElementById('<%=flImage20.ClientID %>')


                  var File1Size = 0;
                  var File2Size = 0;
                  var File3Size = 0;
                  var File4Size = 0;
                  var File5Size = 0;

                  var File6Size = 0;
                  var File7Size = 0;
                  var File8Size = 0;
                  var File9Size = 0;
                  var File10Size = 0;

                  var File11Size = 0;
                  var File12Size = 0;
                  var File13Size = 0;
                  var File14Size = 0;
                  var File15Size = 0;

                  var File16Size = 0;
                  var File17Size = 0;
                  var File18Size = 0;
                  var File19Size = 0;
                  var File20Size = 0;

                  var TotalFileSize = 0;

                  if (FileUpload1.value != "") {
                      File1Size = GetFileSize(FileUpload1);
                  }
                  if (FileUpload2.value != "") {
                      File2Size = GetFileSize(FileUpload2);
                  }
                  if (FileUpload3.value != "") {
                      File3Size = GetFileSize(FileUpload3);
                  }
                  if (FileUpload4.value != "") {
                      File4Size = GetFileSize(FileUpload4);
                  }
                  if (FileUpload5.value != "") {
                      File5Size = GetFileSize(FileUpload5);
                  }
                  if (FileUpload6.value != "") {
                      File6Size = GetFileSize(FileUpload6);
                  }
                  if (FileUpload7.value != "") {
                      File7Size = GetFileSize(FileUpload7);
                  }
                  if (FileUpload8.value != "") {
                      File8Size = GetFileSize(FileUpload8);
                  }
                  if (FileUpload9.value != "") {
                      File9Size = GetFileSize(FileUpload9);
                  }
                  if (FileUpload10.value != "") {
                      File10Size = GetFileSize(FileUpload10);
                  }
                  if (FileUpload11.value != "") {
                      File11Size = GetFileSize(FileUpload11);
                  }
                  if (FileUpload12.value != "") {
                      File12Size = GetFileSize(FileUpload12);
                  }
                  if (FileUpload13.value != "") {
                      File13Size = GetFileSize(FileUpload13);
                  }
                  if (FileUpload14.value != "") {
                      File14Size = GetFileSize(FileUpload14);
                  }
                  if (FileUpload15.value != "") {
                      File15Size = GetFileSize(FileUpload15);
                  }
                  if (FileUpload16.value != "") {
                      File16Size = GetFileSize(FileUpload16);
                  }
                  if (FileUpload17.value != "") {
                      File17Size = GetFileSize(FileUpload17);
                  }
                  if (FileUpload18.value != "") {
                      File18Size = GetFileSize(FileUpload18);
                  }
                  if (FileUpload19.value != "") {
                      File19Size = GetFileSize(FileUpload19);
                  }
                  if (FileUpload20.value != "") {
                      File20Size = GetFileSize(FileUpload20);
                  }

                  TotalFileSize = File1Size + File2Size + File3Size + File4Size + File5Size + File6Size + File7Size + File8Size + File9Size + File10Size + File11Size + File12Size + File13Size + File14Size + File15Size + File16Size + File17Size + File18Size + File19Size + File20Size;

                  if (TotalFileSize >= 10485760) {
                      oSrc.errormessage = "Total file size should be less than 10 MB."
                      args.IsValid = false
                      return true
                  }
                  else {
                      args.IsValid = true
                      return false
                  }

              }

              function GetFileSize(obj) {
                var size = 0;
                for (var k = 0; k < obj.files.length; k++)
                {
                   size+= obj.files[k].size;
                }
                return size;
            }

            function OpenWebsite() {
                window.open($('[id$=hidVideoWebsiteURL]').val(), '_new');
            }


    </script>
</asp:Content>
