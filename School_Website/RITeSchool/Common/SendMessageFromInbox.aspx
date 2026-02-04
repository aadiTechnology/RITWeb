<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SendMessageFromInbox.aspx.cs" Inherits="SendMessageFromInbox" Culture="auto" ViewStateMode="Disabled" ValidateRequest="false"
    meta:resourcekey="PageResource1" UICulture="auto" %>
    <%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <script type="text/javascript" src="../ckeditor_Full/ckeditor.js?version=1.0"></script>
    <div id="divPopup"  style="display:none;">
        <span style="font-weight:bold;color:Black;font-size:medium;font-family:Rockwell"><i class="fa fa-spinner fa-spin progress-spinner"></i>&nbsp;&nbsp;We are saving current message as Draft. Please wait..</span>
    </div>
    <table align="center" width="100%" style="height: 100%" border="0" cellspacing="0"
        cellpadding="0">
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center">
                            <!--MainDataTable Starts Here -->
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                                <tr>
                                    <td style="background-color: white;" id="MainDataTable" align="center">
                                        <!-- Data Insert Here -->
                                        <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                            <tr>
                                                <td colspan="1" align="left" class="TdDataEntryControl" width="70%">
                                                    <div style="float: right" class="LblErrorMsg">
                                                        * Mandatory Fields</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1">
                                                    <asp:ValidationSummary ID="valSum_SendMessage" runat="server" ValidationGroup="valGroupSend"
                                                        CssClass="LblErrorMsg" meta:resourcekey="valSum_SendMessageResource1" />
                                                    <asp:Label ID="lblErr" runat="server" EnableViewState="False" CssClass="ClsLabel"
                                                        ForeColor="Red" meta:resourcekey="lblErrResource1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="center">
                                                    <table align="center">
                                                        <tr align="center">
                                                            <td align="center">
                                                                <asp:UpdatePanel ID="updateLable" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:Label ID="lblSaveMsg" runat="server" Text="" CssClass="ClsLabel" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnDraft" EventName="Click" />
                                                                    </Triggers>
                                                                 </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="2" width="100%">
                                                        <tr>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight" width="150px">
                                                                <span class="ClsLabel ">From :</span>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ClsBorderlight">
                                                                <asp:Label ID="lblFrom" runat="server" CssClass="ClsHilightText" BorderStyle="None"
                                                                    ReadOnly="True" Width="346px" meta:resourcekey="txtFromResource1"></asp:Label>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr class="Height10">
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdForSelectingOption" runat="server">
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                &nbsp;
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ClsBorderlight">
                                                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                                                    <ContentTemplate>
                                                                        <asp:CheckBox ID="chkAdmin" Text="Include Admin" runat="server" meta:resourcekey="chkAdminResource1"  ViewStateMode="Enabled"/>
                                                                        <asp:CheckBox ID="chkPrincipal" Text="Include Principal" runat="server" meta:resourcekey="chkPrincipalResource1"  ViewStateMode="Enabled"/>
                                                                        <asp:CheckBox ID="chkSuperAdmin" Text="Include Software Coordinator" runat="server"
                                                                            meta:resourcekey="chkSuperAdminResource1"   ViewStateMode="Enabled"/>
                                                                        <asp:LinkButton ID="lnkTeacherGroups" runat="server" Visible="true" CssClass="SMSLblSMlBlue"
                                                                            Style="vertical-align: middle !important; padding-left: 10px" CausesValidation="False" OnClientClick="TeacherGroup(); return false;">Contact Group(s)</asp:LinkButton>
                                                                        <br />
                                                                        <asp:RadioButton ID="optTeachers" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="UserType" Text="Teachers" Width="97px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optTeachersResource1" />
                                                                        <asp:RadioButton ID="optStudents" AutoPostBack="false" runat="server" CssClass="ClsLabel" 
                                                                            Font-Bold="False" GroupName="UserType" Text="Students" Width="97px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optStudentsResource1" />
                                                                        <asp:RadioButton ID="optSupervisor" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="UserType" Text="Supervisor" Width="102px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optSupervisorResource1" />
                                                                             <asp:RadioButton ID="optParentTeacherAssociation" AutoPostBack="false" runat="server" CssClass="ClsLabel" Visible="false"
                                                                            Font-Bold="False" GroupName="UserType" Text="PTA" Width="90px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optParentTeacherAssociation1"   />
                                                                         
                                                                        <asp:RadioButton ID="optAll" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="UserType" Text="Entire School" Width="115px"  ViewStateMode="Enabled"
                                                                            meta:resourcekey="optAllResource1" />
                                                                        <asp:HiddenField ID="hidQry" runat="server" />
                                                                    </ContentTemplate>                                                                   
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <span class="ClsMdtStar">*</span><br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TdDataEntryHeader ClsBorderlight">
                                                               <span class="clsLabel">Search by Name :</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox" autocomplete="off"></asp:TextBox>
                                                                <a href="#" onclick="ClearText()" class="ClsLabelNrml"><u>Clear</u></a>                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class=" ClsBorderlight">
                                                                <table>
                                                                    <tr>
                                                                        <td class="ClsTextNormal">
                                                                            To...
                                                                        </td>
                                                                        <td>
                                                                            <a href="JavaScript:ToUserId()" runat="server" id="HlnkAddBook">
                                                                                <img border="none" src="../images/AddressBook.gif" />
                                                                            </a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl">
                                                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel1">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtToUserId" runat="server" CssClass="ClsReadOnly" MaxLength="200"
                                                                            ReadOnly="True" TextMode="MultiLine" Width="100%" Height="44px" meta:resourcekey="txtToUserIdResource1"  ViewStateMode="Enabled"></asp:TextBox>
                                                                        <span style="color: #ff0099"></span>
                                                                    </ContentTemplate>
                                                                    <%--<Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnDraft" EventName="Click" />
                                                                    </Triggers>--%>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                                <span class="ClsMdtStar">*</span><br />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                &nbsp;
                                                            </td>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <asp:Label ID="lblNote" runat="server" CssClass="LblGrayMsg" Text='Click on <img border="none" src="../images/AddressBook.gif" /> for selecting message recipients.'
                                                                     meta:resourcekey="lblNoteResource1"></asp:Label>
                                                                <asp:CustomValidator ID="cstValidate_reqToUserId" runat="server" EnableClientScript="true"
                                                                    ClientValidationFunction="Validate_reqToUserId" CssClass="ClsLabel" Display="None"
                                                                    ErrorMessage="" ValidationGroup="valGroupSend" meta:resourcekey="CstValFileTypeResource1"></asp:CustomValidator>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr class="Height10">
                                                            <td>
                                                            </td>
                                                        </tr>
                                                          <tr id="Tr1" runat="server">
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                &nbsp;
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ClsBorderlight">
                                                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel6">
                                                                    <ContentTemplate>
                                                                        <asp:CheckBox ID="chkAdminCC" Text="Include Admin" runat="server" meta:resourcekey="chkAdminResource1"  ViewStateMode="Enabled"/>
                                                                        <asp:CheckBox ID="chkPrincipleCC" Text="Include Principal" runat="server" meta:resourcekey="chkPrincipalResource1"  ViewStateMode="Enabled"/>
                                                                        <asp:CheckBox ID="chkSuperAdminCC" Text="Include Software Coordinator" runat="server"  ViewStateMode="Enabled"
                                                                            meta:resourcekey="chkSuperAdminResource1" />
                                                                        <asp:LinkButton ID="lnkTeacherGroupsCC" runat="server" Visible="true" CssClass="SMSLblSMlBlue"
                                                                            Style="vertical-align: middle !important; padding-left: 10px" CausesValidation="False" OnClientClick="TeacherGroupCc(); return false;">Contact Group(s)</asp:LinkButton>
                                                                        <br />
                                                                        <asp:RadioButton ID="optCCTeachers" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="CCUserType" Text="Teachers" Width="97px"  ViewStateMode="Enabled"
                                                                            meta:resourcekey="optTeachersResource1" />
                                                                        <asp:RadioButton ID="optCCStudents" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="CCUserType" Text="Students" Width="97px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optStudentsResource1" />
                                                                        <asp:RadioButton ID="optCCSupervisor" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="CCUserType" Text="Supervisor" Width="102px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optSupervisorResource1" />

                                                                             <asp:RadioButton ID="optCCParentTeacherAssociation" AutoPostBack="false" runat="server" CssClass="ClsLabel" Visible="false"
                                                                            Font-Bold="False" GroupName="CCUserType" Text="PTA" Width="90px"   ViewStateMode="Enabled"
                                                                            meta:resourcekey="optParentTeacherAssociation1" />

                                                                        <asp:RadioButton ID="optCCAll" AutoPostBack="false" runat="server" CssClass="ClsLabel"
                                                                            Font-Bold="False" GroupName="CCUserType" Text="Entire School" Width="115px"  ViewStateMode="Enabled"
                                                                            meta:resourcekey="optAllResource1" />
                                                                        <asp:HiddenField ID="hidQryCC" runat="server" />
                                                                    </ContentTemplate>                                                                   
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TdDataEntryHeader ClsBorderlight">
                                                               <span class="clsLabel">Search by Name :</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtSearchCC" runat="server" CssClass="LrgTxtBox" autocomplete="off"></asp:TextBox>
                                                                <a href="#" onclick="ClearTextCC()" class="ClsLabelNrml"><u>Clear</u></a>                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class=" ClsBorderlight">
                                                                <table>
                                                                    <tr>
                                                                        <td class="ClsTextNormal">
                                                                            Cc..
                                                                        </td>
                                                                        <td>
                                                                            <a href="JavaScript:CCUserId()" runat="server" id="HlnkAddBookCC">
                                                                                <img border="none" src="../images/AddressBook.gif" />
                                                                            </a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ">
                                                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtCCUserId" runat="server" CssClass="ClsReadOnly" MaxLength="200"
                                                                            ReadOnly="True" TextMode="MultiLine" Width="100%" Height="44px" meta:resourcekey="txtToUserIdResource1"  ViewStateMode="Enabled"></asp:TextBox>
                                                                        <span style="color: #ff0099"></span>
                                                                    </ContentTemplate>
                                                                    <%--<Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnDraft" EventName="Click" />
                                                                    </Triggers>--%>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                &nbsp;
                                                            </td>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <asp:Label ID="Label1" runat="server" CssClass="LblGrayMsg" Text='Click on <img border="none" src="../images/AddressBook.gif" /> for selecting message recipients.'
                                                                     meta:resourcekey="lblNoteResource1"></asp:Label>
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" EnableClientScript="true"
                                                                    ClientValidationFunction="Validate_reqToUserId" CssClass="ClsLabel" Display="None"
                                                                    ErrorMessage="" ValidationGroup="valGroupSend" meta:resourcekey="CstValFileTypeResource1"></asp:CustomValidator>
                                                            </td>
                                                            <td align="left" colspan="1">
                                                            </td>
                                                        </tr>
                                                        <tr class="Height10">
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <span class="ClsLabel ">Subject :</span>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ">
                                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="ExLrgTxtBox" MaxLength="200"
                                                                    Width="100%" meta:resourcekey="txtSubjectResource1"></asp:TextBox><span style="color: #ff0099"></span><span
                                                                        style="color: #ff0099"><asp:RequiredFieldValidator ID="reqValSubject" runat="server"
                                                                            ControlToValidate="txtSubject" ErrorMessage="Subject should not be blank." ValidationGroup="valGroupSend"
                                                                            SetFocusOnError="True" Display="None" CssClass="LblErrorMsg" meta:resourcekey="reqValSubjectResource1"></asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="cstRec" Display="None" runat="server" ClientValidationFunction="ValidateContentText"
                                                                            ErrorMessage="Message body should not be blank." CssClass="LblErrorMsg" ValidationGroup="valGroupSend"
                                                                            meta:resourcekey="cstRecResource1"></asp:CustomValidator>
                                                                    </span>
                                                            </td> 
                                                            <td align="left" colspan="1">
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trfileupload" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <span class="ClsLabel ">Attachment1 :</span>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ">
                                                                <asp:FileUpload ID="File_attatchment" runat="server" CssClass="ExLrgTxtBox" TabIndex="5" accept=".XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSX, .PNG" multiple="true" 
                                                                    meta:resourcekey="File_attatchmentResource1" ViewStateMode="Enabled" />
                                                                <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Invalid file type." ValidationGroup="valGroupSend"
                                                                    meta:resourcekey="CstValFileTypeResource1" ></asp:CustomValidator>
                                                                <span class="LblSmlGray">(Support only .XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSX, .PNG files
                                                                    types.)</span>
                                                            </td>
                                                        </tr>                                                        
                                                        <tr id="tdAttachment" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" style="" class="ClsBorderlight" valign="top">
                                                                <span class="ClsLabel ">Attachment1 :</span><br />
                                                            </td>
                                                            <td align="left" style="" valign="top">
                                                                <asp:HyperLink ID="lnkAttachment" runat="server" CssClass="CursorHand ClsLblRslt"
                                                                    Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true"
                                                                    meta:resourcekey="lnkAttachmentResource1" ViewStateMode="Enabled" >[lnkAttachment]</asp:HyperLink>
                                                                <asp:Button ID="btnChanegAttachment" runat="server" CssClass="ClsBtnMid" Text="Remove Attachment"
                                                                    Height="23px" Width="144px" OnClientClick="GetRecieptlist()" OnClick="btnChanegAttachment_Click" />
                                                            </td>
                                                        </tr>
                                                         <tr id="trfileupload1" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <span class="ClsLabel ">Attachment2 :</span>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ">
                                                                <asp:FileUpload ID="File_attatchment1" runat="server" CssClass="ExLrgTxtBox" TabIndex="6" accept=".XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSXG, .PNG " multiple="true" 
                                                                    meta:resourcekey="File_attatchmentResource1" ViewStateMode="Enabled" />
                                                                <asp:CustomValidator ID="CstValFileType1" runat="server" ClientValidationFunction="validateFile1"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Invalid file type." ValidationGroup="valGroupSend"
                                                                    meta:resourcekey="CstValFileTypeResource1"></asp:CustomValidator>
                                                                <span class="LblSmlGray">(Support only .XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSX, .PNG files
                                                                    types.)</span>
                                                            </td>
                                                             </tr>
                                                       <tr id="tdAttachment1" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" style="" class="ClsBorderlight" valign="top">
                                                                <span class="ClsLabel ">Attachment2 :</span><br />
                                                            </td>
                                                            <td align="left" style="" valign="top">
                                                                <asp:HyperLink ID="lnkAttachment1" runat="server" CssClass="CursorHand ClsLblRslt"
                                                                    Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true" ViewStateMode="Enabled"
                                                                    meta:resourcekey="lnkAttachmentResource1">[lnkAttachment1]</asp:HyperLink>
                                                                <asp:Button ID="btnChanegAttachment1" runat="server" CssClass="ClsBtnMid" Text="Remove Attachment"
                                                                    Height="23px" Width="144px" OnClientClick="GetRecieptlist1()" OnClick="btnChanegAttachment1_Click" />
                                                            </td>
                                                        </tr>
                                                            <tr id="trfileupload2" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight">
                                                                <span class="ClsLabel ">Attachment3 :</span>
                                                            </td>
                                                            <td align="left" class="TdDataEntryControl ">
                                                                <asp:FileUpload ID="File_attatchment2" runat="server" CssClass="ExLrgTxtBox" TabIndex="7" accept=".XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSX, .PNG" multiple="true" 
                                                                    meta:resourcekey="File_attatchmentResource1" ViewStateMode="Enabled" />
                                                                <asp:CustomValidator ID="CstValFileType2" runat="server" ClientValidationFunction="validateFile2"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Invalid file type." ValidationGroup="valGroupSend"
                                                                    meta:resourcekey="CstValFileTypeResource1"></asp:CustomValidator>
                                                                <span class="LblSmlGray">(Support only .XLS, .XLSX, .DOC, .DOCX, .PDF, .JPG, .JPEG, .PPT, .PPTX, .PPS, .PPSX, .PNG files
                                                                    types.)</span>
                                                            </td>
                                                        </tr>                   
                                                       <tr id="tdAttachment2" runat="server" ViewStateMode="Enabled" >
                                                            <td align="left" style="" class="ClsBorderlight" valign="top">
                                                                <span class="ClsLabel ">Attachment3 :</span><br />
                                                            </td>
                                                            <td align="left" style="" valign="top">
                                                                <asp:HyperLink ID="lnkAttachment2" runat="server" CssClass="CursorHand ClsLblRslt"
                                                                    Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true" ViewStateMode="Enabled"
                                                                    meta:resourcekey="lnkAttachmentResource1">[lnkAttachment2]</asp:HyperLink>
                                                                <asp:Button ID="btnChanegAttachment2" runat="server" CssClass="ClsBtnMid" Text="Remove Attachment"
                                                                    Height="23px" Width="144px" OnClientClick="GetRecieptlist2()" OnClick="btnChanegAttachment2_Click" />
                                                            </td>
                                                        </tr>
                                                           <tr id="trAttachments" runat="server" visible="false">
                                                                <td align="left" style="" class="ClsBorderlight" valign="top">
                                                                    <span class="ClsLabel">Attachments :</span><br />
                                                                </td>
                                                                <td align="left" style="" valign="top">
                                                                  <table ID="pnl" runat="server" style="height:auto" ViewStateMode="Enabled">
                                                                  </table>
                                                                </td>
                                    
                                                           </tr>
                                                        <tr>
                                                             <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                                    CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; font-weight:bold;">
                                                                <asp:Label ID="Label4" runat="server" BorderWidth="0px" Text="Total file size should be less than 50 MB."
                                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Request Read Receipt? :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkReadReceipt" runat="server" ViewStateMode="Enabled"/>
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                            <td class="ClsBorderlight">
                                                                <span class="ClsLabel">Schedule Message at :</span>
                                                            </td>
                                                            <td>
                                                           
                                                               <%-- <asp:CheckBox ID="chkScheduleMessages" runat="server" ViewStateMode="Enabled" OnCheckedChanged="chkScheduleMessages_CheckedChanged"/>--%>
                                                            <asp:CheckBox ID="chkScheduleMessages" runat="server" ViewStateMode="Enabled" />
                                                                 <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" TabIndex="1" Width="90px" disabled ViewStateMode="Enabled">
                                                                          </asp:TextBox>
                                                                  <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtDate" Format="dd MMM yyyy" 
                                                                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="date should not be blank." /> 
                                                           <asp:TextBox ID="txtStartTime" runat="server" CssClass="MidTxtBox" Width="70px" MaxLength="8" disabled ViewStateMode="Enabled"></asp:TextBox>                                                           
                                                           <span class="LblSmlGray" style="padding-left:5px">e.g. 07:00 AM. You can schedule message for next 7 days. For scheduled message, recipients wont get notification on mobile.</span>  
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="IsValidTimeRange"
                                                                                CssClass="LblErrorMsg" Display="None" ErrorMessage="" 
                                                                                ValidationGroup="valGroupSend"></asp:CustomValidator>
                                                           <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server" ValidationGroup="valGroupSend"
                                                                                SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid time e.g. 10:00 AM."
                                                                                ClientValidationFunction="IsValidStartTime"> </asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <div style="float: left">
                                                                    <span class="ClsLabel">Message :</span>
                                                                </div>
                                                                <div style="float: right">
                                                                    <asp:Button ID="btnSendMessageUp" Text="Send Message" runat="server" OnClick="imgBtnSendMessage_Click"
                                                                        ValidationGroup="valGroupSend" CssClass="ClsBtnMid" disable-page="true" 
                                                                        meta:resourcekey="btnSendMessageResource1" UseSubmitBehavior="False" /></div>
                                                                        <asp:CustomValidator ID="cvDuplicateFile" runat="server" ClientValidationFunction="CheckDuplicateFile"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="File should not be Duplicate." ValidationGroup="valGroupSend"
                                                                    meta:resourcekey="CstValFileTypeResource1"></asp:CustomValidator>
                                                                     <asp:CustomValidator ID="csFileSizeTotal" runat="server" ClientValidationFunction="ValidateFileSize"
                                                                    CssClass="ClsLabel" Display="None" ErrorMessage="" ValidationGroup="valGroupSend"></asp:CustomValidator>
                                                            </td>
                                                            <td align="right" style="padding-right: 30px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="TdDataEntryHeader ClsBorderlight " colspan="2">                                                                
                                                                <textarea id="edtr1"></textarea>
                                                                <div id="divData" runat="server" enableviewstate="true" style="overflow:auto;border-style:solid;border-color:Gray;border-width:1px;padding:5px;background-color:lightGray;width:100%;height:400px;background-color:#FCFCFC" visible="false"></div>
                                                                <asp:HiddenField ID="hidData" runat="server" Value="" />
                                                                <asp:HiddenField ID="hidOldData" runat="server" Value="" />
                                                                <input type="hidden" id ="hidData1" runat="server" />
                                                            </td>
                                                            <td align="left" colspan="1" valign="top">
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNote" runat="server" visible="false">
                                                            <td align="left" class="ClsBorderlight " style="width: 11%; background-color: #ffffc4;">
                                                                <span class="LblNrmlB"><b>Note :</b></span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                                <span class="LblSmlV">Address all your messages regarding your child to the Class Teacher
                                                                    and only if necessary to the Principal. In case of urgency, please call the school
                                                                    office.</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <div style="float: left">
                                                                    <asp:Button ID="btnSendMessage" Text="Send Message" runat="server" OnClick="imgBtnSendMessage_Click"
                                                                        ValidationGroup="valGroupSend" CssClass="ClsBtnMid" disable-page="true" 
                                                                        meta:resourcekey="btnSendMessageResource1" UseSubmitBehavior="False" />
                                                                </div>
                                                                <div style="float: left">
                                                                    <asp:Button ID="btnDraft" Text="Save As Draft" runat="server" CssClass="ClsBtnMid" 
                                                                        onclick="btnDraft_Click" CausesValidation="false" ValidationGroup="valGroupSend" />
                                                                </div>
                                                                <div style="float: right">
                                                                    <asp:Button ID="imgBtnGoToInbox" Text="Go To Inbox" runat="server" OnClick="imgBtnGoToInbox_Click"
                                                                        CssClass="ClsBtnMid" meta:resourcekey="imgBtnGoToInboxResource1" /></div>
                                                            </td>
                                                            <td align="right" style="padding-right: 30px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTimer" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Timer ID="timer" runat="server" Interval="300000" Enabled="true" 
                                                                ontick="timer_Tick">
                                                            </asp:Timer>
                                                            <asp:HiddenField ID="hidTimerStart" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="HidReplyUserNames" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReplyUserNamesCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidUserNames" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidUserNamesCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReplyUserID" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReplyUserIDCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidUserId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidUserIdCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserID" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserIDCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminUserNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipalUserID" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipalUserIDCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipleName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidPrincipleNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminReplyName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAdminReplyNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTeacherId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTeacherIdCC" runat="server" ViewStateMode="Enabled"/>

                                                             <asp:HiddenField ID="HidPTAId" runat="server" ViewStateMode="Enabled"/><%--/////////////////////--%>
                                                            <asp:HiddenField ID="HidPTAIdCC" runat="server" ViewStateMode="Enabled"/><%--/////////////////////--%>

                                                            <asp:HiddenField ID="HidStdDivId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStdDivIdCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStudentId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStudentIdCC" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidTeacherName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTeacherNameCC" runat="server" ViewStateMode="Enabled"/>


                                                             <asp:HiddenField ID="HidPTAName" runat="server" ViewStateMode="Enabled"/><%--/////////////////////--%>
                                                              <asp:HiddenField ID="HidPTANameCC" runat="server" ViewStateMode="Enabled"/><%--/////////////////////--%>

                                                            <asp:HiddenField ID="HidStdDivName" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidStdDivNameCC" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidStudentName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidStudentNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidUserType" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidUserTypeCC" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidSupervisorId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSupervisorIdCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSupervisorName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSupervisorNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSuperAdminUserId" runat="server" ViewStateMode="Enabled" />
                                                            <asp:HiddenField ID="HidSuperAdminName" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSuperAdminUserIdCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidSuperAdminNameCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidId" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidTO" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidCC" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAttachment" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAttachment1" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidAttachment2" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReciepents" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReciepents1" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="HidReciepents2" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidUserGroupId" Value="0"  ViewStateMode="Enabled"/>
                                                            <asp:HiddenField runat="server" ID="hidUserGroupIdCC" Value="0" ViewStateMode="Enabled" />
                                                            <asp:HiddenField runat="server" ID="hidUserGroupName" Value="" ViewStateMode="Enabled"/>   
                                                            <asp:HiddenField runat="server" ID="hidUserGroupNameCC" Value="" ViewStateMode="Enabled"/>   
                                                            <asp:HiddenField ID="hidViewAllStudents" runat="server" ViewStateMode="Enabled"/>
                                                            <asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                            <asp:HiddenField ID="hidAcademicYearId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                            <asp:HiddenField ID="hidLoginUserId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                            <asp:HiddenField ID="hidShowOnlyCoordinators" runat="server" ViewStateMode="Enabled" Value="0" />
                                                            <asp:HiddenField ID="hidDeleteedIds" runat="server" ViewStateMode="Enabled" Value="" />
                                                            <asp:HiddenField ID="hidRestrictCopy" runat="server" ViewStateMode="Enabled" Value="" />
                                                            <asp:HiddenField ID="hidIsPTAMember" runat="server" ViewStateMode="Enabled" Value="N" />                                                            
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                     <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
                                                        <ContentTemplate>
                                                            <asp:HiddenField runat="server" ID="hidDraftId" Value="0" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID ="timer" EventName="Tick" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- Data Insert End Here -->
                                    </td>
                                </tr>
                            </table>
                            <!--MainDataTable End Here -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientHiddenId = "<%=this.HidReplyUserNames.ClientID %>"
        _clienttxtToUserId = "<%=this.txtToUserId.ClientID %>"
        _clientHidReplyUserID = "<%=this.HidReplyUserID.ClientID %>"
        _clienthidUserId = "<%=this.hidUserId.ClientID %>"
        _clientHidUserNames = "<%=this.HidUserNames.ClientID %>"
        _clienthidUserHasFullAccess = "<%=this.hidUserHasFullAccess.ClientID %>"
        _clientHidAdminUserID = "<%=this.HidAdminUserID.ClientID %>"
        _clientHidPrincipleUserID = "<%=this.HidPrincipalUserID.ClientID %>"
        _clientHidStdDivId = "<%=this.HidStdDivId.ClientID %>"
        _clientHidTeacherId = "<%=this.HidTeacherId.ClientID %>"
        _clientHidStudentId = "<%=this.HidStudentId.ClientID %>"
        _clientHidSupervisorId = "<%=this.HidSupervisorId.ClientID %>"
        _clientHidUserType = "<%=this.HidUserType.ClientID %>"
        _clientHidStdDivName = "<%=this.HidStdDivName.ClientID %>"
        _clientHidTeacherName = "<%=this.HidTeacherName.ClientID %>"
        _clientHidStudentName = "<%=this.HidStudentName.ClientID %>"
        _clientHidAdminUserName = "<%=this.HidAdminUserName.ClientID %>"
        _clientHidPrincipleName = "<%=this.HidPrincipleName.ClientID %>"
        _clientHidAdminReplyName = "<%=this.HidAdminReplyName.ClientID %>"
        _clientHidSupervisorName = "<%=this.HidSupervisorName.ClientID %>"
        _clientoptTeachers = "<%=this.optTeachers.ClientID %>"
        _clientoptStudents = "<%=this.optStudents.ClientID %>"
        _clientoptSupervisor = "<%=this.optSupervisor.ClientID %>"
        _clientoptAll = "<%=this.optAll.ClientID %>"
        _clientChkAdmin = "<%=this.chkAdmin.ClientID %>"
        _clientchkPrincipal = "<%=this.chkPrincipal.ClientID %>"
        _clientButton1 = "<%=this.btnSendMessage.ClientID %>"
        _clienthidQry = "<%=this.hidQry.ClientID %>"

        _clientFileUploadClientId = "<%=this.File_attatchment.ClientID%>"
        _clientFileUploadClientId1 = "<%=this.File_attatchment1.ClientID%>"
        _clientFileUploadClientId2 = "<%=this.File_attatchment2.ClientID%>"
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        _clientCustomValId1 = "<%=this.CstValFileType1.ClientID%>"
        _clientCustomValId2 = "<%=this.CstValFileType2.ClientID%>"
        _clientcstValidate_reqToUserId = "<%=this.cstValidate_reqToUserId.ClientID%>"
        _clientlblErr = "<%=this.lblErr.ClientID %>"
        _clientHlnkAddBook = "<%=this.HlnkAddBook.ClientID %>"
        _clientchkSuperAdmin = "<%=this.chkSuperAdmin.ClientID %>"
        _clientHidSuperAdminUserId = "<%=this.HidSuperAdminUserId.ClientID %>"
        _clientHidSuperAdminName = "<%=this.HidSuperAdminName.ClientID %>"

        _clientHidReciepents = "<%=this.HidReciepents.ClientID %>"
        _clientHidReciepents1 = "<%=this.HidReciepents1.ClientID %>"
        _clientHidReciepents2 = "<%=this.HidReciepents2.ClientID %>"
        _clienttxtToUserId = "<%=this.txtToUserId.ClientID %>"
        _clientlnkTeacherGroups = "<%=this.lnkTeacherGroups.ClientID %>"
        _clientlnkTeacherGroupsCC = "<%=this.lnkTeacherGroupsCC.ClientID %>"
        _clientHidReplyUserIDCC = "<%=this.HidReplyUserIDCC.ClientID %>"
        _clienttxtCCUserId = "<%=this.txtCCUserId.ClientID %>"
        _clientChkAdminCC = "<%=this.chkAdminCC.ClientID %>"
        _clientchkPrincipalCC = "<%=this.chkPrincipleCC.ClientID %>"
        _clientchkSuperAdminCC = "<%=this.chkSuperAdminCC.ClientID %>"
        _clientHlnkAddBookCC = "<%=this.HlnkAddBookCC.ClientID %>"
        _clienthidQryCC = "<%=this.hidQryCC.ClientID %>"
        _clientoptCCTeachers = "<%=this.optCCTeachers.ClientID %>"
        _clientoptCCStudents = "<%=this.optCCStudents.ClientID %>"
        _clientoptCCSupervisor = "<%=this.optCCSupervisor.ClientID %>"
        _clientoptCCAll = "<%=this.optCCAll.ClientID %>"
        _clienthidUserIdCC = "<%=this.hidUserIdCC.ClientID %>"
        _clientHidUserNamesCC = "<%=this.HidUserNamesCC.ClientID %>"
        _clientHidAdminUserIDCC = "<%=this.HidAdminUserIDCC.ClientID %>"
        _clientHidPrincipleUserIDCC = "<%=this.HidPrincipalUserIDCC.ClientID %>"
        _clientHidStdDivIdCC = "<%=this.HidStdDivIdCC.ClientID %>"
        _clientHidTeacherIdCC = "<%=this.HidTeacherIdCC.ClientID %>"
        _clientHidStudentIdCC = "<%=this.HidStudentIdCC.ClientID %>"
        _clientHidSupervisorIdCC = "<%=this.HidSupervisorIdCC.ClientID %>"
        _clientHidUserTypeCC = "<%=this.HidUserTypeCC.ClientID %>"
        _clientHidStdDivNameCC = "<%=this.HidStdDivNameCC.ClientID %>"
        _clientHidTeacherNameCC = "<%=this.HidTeacherNameCC.ClientID %>"
        _clientHidStudentNameCC = "<%=this.HidStudentNameCC.ClientID %>"
        _clientHidAdminUserNameCC = "<%=this.HidAdminUserNameCC.ClientID %>"
        _clientHidPrincipleNameCC = "<%=this.HidPrincipleNameCC.ClientID %>"
        _clientHidAdminReplyNameCC = "<%=this.HidAdminReplyNameCC.ClientID %>"
        _clientHidSupervisorNameCC = "<%=this.HidSupervisorNameCC.ClientID %>"
        _clienthidUserGroupName = "<%=this.hidUserGroupName.ClientID %>";
        _clienthidUserGroupId = "<%=this.hidUserGroupId.ClientID %>";
        _clienthidUserGroupNameCC = "<%=this.hidUserGroupNameCC.ClientID %>";
        _clienthidUserGroupIdCC = "<%=this.hidUserGroupIdCC.ClientID %>";
        _clientcvDuplicateFile = "<%=this.cvDuplicateFile.ClientID %>";
        _lnkAttachment = "<%=this.lnkAttachment.ClientID %>";
        _lnkAttachment1 = "<%=this.lnkAttachment1.ClientID %>";
        _lnkAttachment2 = "<%=this.lnkAttachment2.ClientID %>";
        _tdAttachment = "<%=this.tdAttachment.ClientID %>";
        _tdAttachment1 = "<%=this.tdAttachment1.ClientID %>";
        _tdAttachment2 = "<%=this.tdAttachment2.ClientID %>";
        _clientTimer = "<%=this.timer.ClientID %>"
        _clientbtnDraft = "<%=this.btnDraft.ClientID %>"
        _clienthidData = "<%=this.hidData.ClientID %>"



        _clienttxtDate = '<%=this.txtDate.ClientID %>'
        _clienttxtStartTime = '<%=this.txtStartTime.ClientID %>'
        _clientchkScheduleMessages = '<%=this.chkScheduleMessages.ClientID %>'


        _clientHidPTAId = "<%=this.HidPTAId.ClientID %>"
        _clientHidPTAIdCC = "<%=this.HidPTAIdCC.ClientID %>"
        _clientHidPTAName = "<%=this.HidPTAName.ClientID %>"
        _clientHidPTANameCC = "<%=this.HidPTANameCC.ClientID %>"
        _clientoptCCParentTeacherAssociation = "<%=this.optCCParentTeacherAssociation.ClientID %>"
        _clientoptParentTeacherAssociation = "<%=this.optParentTeacherAssociation.ClientID %>"

        _clienthidIsPTAMember = "<%=this.hidIsPTAMember.ClientID %>"

    </script>
    <script src="../Scripts/Common/SendMessageFromInbox.js?version=2.1" type="text/javascript">
    </script>

    <script type="text/javascript" src="../ckeditor_full/ckeditor.js?version=1.0"></script>
    
    <script type="text/javascript">
        CKEDITOR.replace("edtr1");
       
        ShowMsg();
        function ShowMsg() {
            var s = document.getElementById(_clienthidData).value
            $('#edtr1').val(s)
            $('[id$=divData]').val(s)

            $('#edtr1').on('cut copy', function (e) {            
                e.preventDefault();
                alert("Cut/Copy action is disabled.");
            });
        }

    </script>

    <script type="text/javascript">

        function ScheduleMessage() {
          
            var chkSchedule = $get(_clientchkScheduleMessages);
            if (chkSchedule != null && chkSchedule.checked) {
                $get(_clienttxtDate).disabled = false;
                $get(_clienttxtStartTime).disabled = false;
              
            }

            else {
                if ($get(_clienttxtDate) != null) {
                    $get(_clienttxtDate).disabled = true;
                    $get(_clienttxtDate).value = '';
                }

                if ($get(_clienttxtStartTime) != null) {
                    $get(_clienttxtStartTime).disabled = true;
                }
            }
        }
        
        function IsValidTimeRange(src, args) {

            var bIsValid = true;

            var chkSchedule = $get(_clientchkScheduleMessages);
            if (chkSchedule != null && chkSchedule.checked) {

                var bIsValid = true;

                var StartDt = "";
                var sStrtDate = document.getElementById(_clienttxtDate).value
                var sStrtTime = document.getElementById(_clienttxtStartTime).value
                if (sStrtTime == "")
                    sStrtTime = "00:00 AM";
                    
                    var serverDate = $('[id$=hidServerFullDate]').val()

                    var currentdate;
                    if (document.all)
                        currentdate = new Date(serverDate.replace('-', ' '));
                    else {
                        currentdate = new Date(serverDate.replace('-', ' ').replace(/-/g, ' '));
                    }

                var hours, minutes;
                //minutes = (currentdate.getMinutes() + 15);
                minutes = currentdate.getMinutes();
                hours = currentdate.getHours()

//                if (parseInt(minutes) >= 60) {
//                    hours = parseInt(hours) + 1;
//                    minutes = parseInt(minutes) - 60;
//                }

                var datetime = new Date((currentdate.getMonth() + 1) + "/" + currentdate.getDate()
                                    + "/" + currentdate.getFullYear() + " " +
                                    +hours + ":"
                                    + minutes + ":" + currentdate.getSeconds());

                //var date = new Date();
                var result = currentdate.setTime(currentdate.getTime() + (7 * 24 * 60 * 60 * 1000));

                if (sStrtDate == "") {
                    src.errormessage = "Schedule Date should not be blank.";
                    bIsValid = false;
                }
                else if (sStrtDate != "" && !validateDate(sStrtDate)) {
                    src.errormessage = "Schedule Date should be in valid format.";
                    bIsValid = false;

                }
                else if (GetConvertedDate(sStrtDate, sStrtTime) <= datetime) {
                    src.errormessage = "Message schedule time should be in future.";
                    bIsValid = false;
                }
                else if (GetConvertedDate(sStrtDate, sStrtTime) >= new Date(result)) {
                    src.errormessage = "Message schedule date & time should be less than 7 days from now.";
                    bIsValid = false;
                }

            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function validateDate(txtDueDate) {
            var isValid = true;
            if (document.all) {
                if (isNaN(new Date(convertdate(txtDueDate).replace(/-/g, ' '))))
                    isValid = false;
            }
            else {
                if (isNaN(new Date(convertdate(txtDueDate).replace('-', ' '))))
                    isValid = false;
            }
            return isValid;
        }
        function GetConvertedDate(sStrtDate, sStartTime) {
            StartDt = new Date(sStrtDate.replace('-', ' ').replace(/-/g, ' ') + " " + sStartTime);
            return StartDt;
        }

        function IsValidStartTime(oSrc, args) {
        
            if ($get(_clientchkScheduleMessages).checked) {
                if (document.getElementById(_clienttxtStartTime)) {
                    if (document.getElementById(_clienttxtStartTime).value != '') {
                        if (!isTimeValid(_clienttxtStartTime)) {
                            args.IsValid = false;
                            return true;
                        }
                        else if (isTimeValid(_clienttxtStartTime)) {
                            var time = $get(_clienttxtStartTime).value.trim();
                            if (time.toLowerCase() == "00:00 pm") {
                                args.IsValid = false;
                                return true;
                            }
                            else if (time.toLowerCase() == "00:00 am") {
                                args.IsValid = false;
                                return true;
                            }
                        }

                        args.IsValid = true;
                        return false;
                    }
                    else if (document.getElementById(_clienttxtStartTime).value == '') {
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            args.IsValid = true;
            return false;
        }

        function isTimeValid(result) {

            var timeStr = document.getElementById(result).value;
            timeStr = timeStr.toUpperCase();
            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            if (ampm == null)
                return false;
            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }






        function ValidateFileSize(oSrc, args) {
            var FileUpload1 = document.getElementById('<%=File_attatchment.ClientID %>')
            var FileUpload2 = document.getElementById('<%=File_attatchment1.ClientID %>')
            var FileUpload3 = document.getElementById('<%=File_attatchment2.ClientID %>')

            var File1Size = 0;
            var File2Size = 0;
            var File3Size = 0;
            var TotalFileSize = 0;

            if (FileUpload1.value != "") {
                File1Size = FileUpload1.files[0].size;
            }
            if (FileUpload2.value != "") {
                File2Size = FileUpload2.files[0].size;
            }
            if (FileUpload3.value != "") {
                File3Size = FileUpload3.files[0].size;
            }

            TotalFileSize = File1Size + File2Size + File3Size;

            if (TotalFileSize >= 52428800) {
                oSrc.errormessage = "Total file size should be less than 50 MB."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }

        }



        function SendCKEditorMessage() {            
            var data = CKEDITOR.instances.edtr1.getData() + $('[id$=hidOldData]').val()
            $('#' + _clienthidData).val(data)
        }
        function ValidateContentText(source, args) {
            var msg = CKEDITOR.instances.edtr1.getData();
            msg = trimAll(msg.replace(/&nbsp;/g, "").replace(/<p>/g, "").replace(/<\/p>/g, ""))
            if (msg == "") {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


       
    </script>

     <script language="javascript" type="text/javascript">
         _slienttxtUserName = '#<%=txtSearch.ClientID%>';
         _slienttxtSearchCC = '#<%=txtSearchCC.ClientID%>';
         _clientSchoolId = '<%=this.hidSchoolId.ClientID %>'
         _clientAcademicYearId = '<%=this.hidAcademicYearId.ClientID %>'
         _clientLoginUserId = '<%=this.hidLoginUserId.ClientID %>'
         _clienthidShowOnlyCoordinators = '<%=this.hidShowOnlyCoordinators.ClientID %>'
         
         $(document).ready(function () {
             AutoSearch();

             $(this).on("copy cut", function (e) {
                 if ($('[id$=hidRestrictCopy]').val() == '1') {
                     alert('Cut/Copy is disabled.')
                     e.preventDefault();
                 }
             }); 
         });


          



         function AutoSearch() {         
             var SchoolId = $('#' + _clientSchoolId).val()
             var userId = $('#' + _clientLoginUserId).val()
             var AcademicYearId = $('#' + _clientAcademicYearId).val()

             BindAutoCompleteEventForMessageCenter(SchoolId, AcademicYearId, _slienttxtUserName, 0, userId, _clienthidShowOnlyCoordinators);
             BindAutoCompleteEventForMessageCenter(SchoolId, AcademicYearId, _slienttxtSearchCC, 0, userId, _clienthidShowOnlyCoordinators);
         }

         function SearchSelectedValue(val) {
             
             if ($(_slienttxtUserName).is(":focus")) {
                 var bFound = false
                 var s = $('[id*=txtToUserId]').val()
                 var ss = s.split(',')
                 for (var k = 0; k < ss.length; k++) {
                     if (ss[k] == val) {
                         bFound = true;
                         break;
                     }
                 }

                 if (bFound)
                     alert('This user is already present in list.')
                 else {
                     var schoolId = $('#' + _clientSchoolId).val()
                     var academicYearId = $('#' + _clientAcademicYearId).val()

                     $.ajax({
                         type: "POST",
                         data: '{"asUserName": "' + val + '","asSchoolId":"' + schoolId + '","asAcademicYearId":"' + academicYearId + '"}',
                         url: "SendMessageFromInbox.aspx/GetUserDetails",
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (msg) {
                             var ids = ''
                             var names = ''

                             if (msg.d.UserRoleId != null && msg.d.UserRoleId != 0) {
                                 if (msg.d.UserRoleId == 2) {
                                     $get(_clientoptTeachers).checked = true;

                                     if (msg.d.IsPrincipal) {
                                         if ($get(_clientchkPrincipal) != null)
                                             $get(_clientchkPrincipal).checked = true;
                                     }

                                     ids = document.getElementById(_clientHidTeacherId).value
                                     names = document.getElementById(_clientHidTeacherName).value

                                 }
                                 else if (msg.d.UserRoleId == 6) {
                                     $get(_clientoptSupervisor).checked = true;
                                     ids = document.getElementById(_clientHidSupervisorId).value
                                     names = document.getElementById(_clientHidSupervisorName).value
                                 }
                                 else if (msg.d.UserRoleId == 3) {
                                     $get(_clientoptStudents).checked = true;
                                     ids = document.getElementById(_clientHidStudentId).value
                                     names = document.getElementById(_clientHidStudentName).value
                                 }
                                 else if (msg.d.UserRoleId == 1 && msg.d.IsAdmin == true) {
                                     if ($get(_clientChkAdmin) != null)
                                         $get(_clientChkAdmin).checked = true;

                                     var sAdmin = document.getElementById(_clientHidAdminUserName).value
                                     var sUsers = document.getElementById(_clienttxtToUserId).value

                                     if (sUsers != "")
                                         document.getElementById(_clienttxtToUserId).value = sUsers + ', ' + sAdmin
                                     else
                                         document.getElementById(_clienttxtToUserId).value = sAdmin

                                     document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
                                 }
                                 else if (msg.d.UserRoleId == 1 && msg.d.IsSWCoordinator == true) {
                                     if ($get(_clientchkSuperAdmin) != null)
                                         $get(_clientchkSuperAdmin).checked = true;

                                     var sSuperAdmin = document.getElementById(_clientHidSuperAdminName).value
                                     var sUsers = document.getElementById(_clienttxtToUserId).value

                                     if (sUsers != "")
                                         document.getElementById(_clienttxtToUserId).value = sUsers + ', ' + sSuperAdmin
                                     else
                                         document.getElementById(_clienttxtToUserId).value = sSuperAdmin

                                     document.getElementById(_clientHidUserNames).value = document.getElementById(_clienttxtToUserId).value
                                 }

                                 if (msg.d.UserRoleId != 1) {
                                    
                                     var finalIds = ids + ";" + msg.d.UserId
                                     var finalNames = names + "," + val

                                     if (finalIds.startsWith(";"))
                                         finalIds = finalIds.substring(1)

                                     if (finalNames.startsWith(","))
                                         finalNames = finalNames.substring(1)

                                     SetToUserId(finalNames, finalIds, 'Y')
                                 }
                             }
                         },
                         error: function (msg) {
                             alert(msg);
                         }
                     });
                 }
             }
             else {
                 var bFoundCC = false
                 var s = $('[id*=txtCCUserId]').val()
                 var ss = s.split(',')
                 for (var k = 0; k < ss.length; k++) {
                     if (ss[k] == val) {
                         bFoundCC = true;
                         break;
                     }
                 }

                 if (bFoundCC == true) {
                     alert('This user is already present in list.')
                 }
                 else {
                     var schoolId = $('#' + _clientSchoolId).val()
                     var academicYearId = $('#' + _clientAcademicYearId).val()

                     $.ajax({
                         type: "POST",
                         data: '{"asUserName": "' + val + '","asSchoolId":"' + schoolId + '","asAcademicYearId":"' + academicYearId + '"}',
                         url: "SendMessageFromInbox.aspx/GetUserDetails",
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (msg) {
                             var ids = ''
                             var names = ''
                             if (msg.d.UserRoleId != null && msg.d.UserRoleId != 0) {
                                 if (msg.d.UserRoleId == 2) {
                                     $get(_clientoptCCTeachers).checked = true;

                                     if (msg.d.IsPrincipal) {
                                         if ($get(_clientchkPrincipalCC) != null)
                                             $get(_clientchkPrincipalCC).checked = true;
                                     }

                                     ids = document.getElementById(_clientHidTeacherIdCC).value
                                     names = document.getElementById(_clientHidTeacherNameCC).value
                                 }
                                 else if (msg.d.UserRoleId == 6) {
                                     $get(_clientoptCCSupervisor).checked = true;
                                     ids = document.getElementById(_clientHidSupervisorIdCC).value
                                     names = document.getElementById(_clientHidSupervisorNameCC).value
                                 }
                                 else if (msg.d.UserRoleId == 3) {
                                     $get(_clientoptCCStudents).checked = true;
                                     ids = document.getElementById(_clientHidStudentIdCC).value
                                     names = document.getElementById(_clientHidStudentNameCC).value
                                 }
                                 else if (msg.d.UserRoleId == 1 && msg.d.IsAdmin == true) {
                                     if ($get(_clientChkAdminCC) != null)
                                         $get(_clientChkAdminCC).checked = true;

                                     var sAdmin = document.getElementById(_clientHidAdminUserName).value
                                     var sUsers = document.getElementById(_clienttxtCCUserId).value

                                     if (sUsers != "")
                                         document.getElementById(_clienttxtCCUserId).value = sUsers + ', ' + sAdmin
                                     else
                                         document.getElementById(_clienttxtCCUserId).value = sAdmin

                                     document.getElementById(_clientHidUserNamesCC).value = document.getElementById(_clienttxtCCUserId).value
                                 }
                                 else if (msg.d.UserRoleId == 1 && msg.d.IsSWCoordinator == true) {
                                     if ($get(_clientchkSuperAdminCC) != null)
                                         $get(_clientchkSuperAdminCC).checked = true;

                                     var sSuperAdmin = document.getElementById(_clientHidSuperAdminName).value
                                     var sUsers = document.getElementById(_clienttxtCCUserId).value

                                     if (sUsers != "")
                                         document.getElementById(_clienttxtCCUserId).value = sUsers + ', ' + sSuperAdmin
                                     else
                                         document.getElementById(_clienttxtCCUserId).value = sSuperAdmin

                                     document.getElementById(_clientHidUserNamesCC).value = document.getElementById(_clienttxtCCUserId).value
                                 }

                                 if (msg.d.UserRoleId != 1) {                                     
                                     var finalIds = ids + ";" + msg.d.UserId
                                     var finalNames = names + "," + val

                                     if (finalIds.startsWith(";"))
                                         finalIds = finalIds.substring(1)

                                     if (finalNames.startsWith(","))
                                         finalNames = finalNames.substring(1)

                                     SetCcUserId(finalNames, finalIds, 'Y')
                                 }
                             }
                         },
                         error: function (msg) {
                             alert(msg);
                         }
                     });
                 }
             }             
         }

        function ClearText() {
            $(_slienttxtUserName).val('')
            $(_slienttxtUserName).focus()
        }

        function ClearTextCC() {
            $(_slienttxtSearchCC).val('')
            $(_slienttxtSearchCC).focus()
        }

         function ShowAlert(msg) {
             alert(msg)
             return false;
         }

         function HideAttachment(index) {             
             $('[id$=hyper_' + index + ']').hide();
             $('[id$=img_' + index + ']').hide();
             
             var sData = $('[id$=hidDeleteedIds]').val()
             if (sData == '')
                 $('[id$=hidDeleteedIds]').val(index)
            else
                $('[id$=hidDeleteedIds]').val(sData+','+index)
         }




        
	</script>
</asp:Content>
