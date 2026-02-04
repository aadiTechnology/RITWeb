<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StudentAchievementPopup.aspx.cs" Inherits="StudentAchievementPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" style="width: 100%;">
            <tr>
                <td align="left" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <span class="MainTitleHead">Student Achievement/Punishment Details.</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 30px" valign="bottom">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                        Text="Mandatory Fields"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" />
                            <asp:RequiredFieldValidator ID="reqDescription" runat="server" ErrorMessage="Description should not be blank."
                                ControlToValidate="txtDescription" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="reqcmbNoteCategory" runat="server" Display="None" ControlToValidate="cmbNoteCategory"
                                            InitialValue="0" ErrorMessage="Note Category should be selected." ></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstValidateFileUpload" runat="server" ErrorMessage="" Display="None"
                                ClientValidationFunction="IsValidFile"></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentAchievement" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentAchievement" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="1" style="text-align: center">
                                <tr style="height: 10px;">
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="ClsBorderlight">
                                       <span class="ClsLabel">Note Category :</span>
                                    </td>
                                    <td align="left">
                                      <asp:DropDownList ID="cmbNoteCategory" runat="server" ViewStateMode="Enabled" CssClass="MidCombo"  OnSelectedIndexChanged="cmbNoteCategory_SelectedIndexChanged" AutoPostBack="true">
                                       </asp:DropDownList>
                                      <span class="ClsMdtStar">*</span>
                                   </td>
                                </tr>
                                <tr>
                                    <td style="width: 280px;" class="ClsBorderlight">
                                        <asp:Label ID="lblRegistrationNumber" runat="server" CssClass="ClsLabel" Text="Registration Number"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td class="ClsHilightBGB" style="text-align: left; width:283px;">
                                        <asp:Label ID="lblRegistration" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblStudentNa" runat="server" CssClass="ClsLabel" Text="Student Name"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td class="ClsHilightBGB" style="text-align: left; width:283px;">
                                        <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblAchievementDate" runat="server" CssClass="ClsLabel" Text="Date"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtAchievementDate" CssClass="MidTxtBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_AchievementDate" runat="server" Control="txtAchievementDate"
                                            Format="dd MMM yyyy" Culture="en" ShowWeekend="True" AutoPostBack="False" To-Today="true" />
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" valign="top">
                                        <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text="Description"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" colspan="1">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine"
                                            Height="72px" Width="280px"></asp:TextBox>
                                        <span class="ClsMdtStar">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <asp:Label ID="lblAttachFile" runat="server" CssClass="ClsLabel" Text="Attachment"
                                            Height="16px"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="FileUploadAchievement" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left" class="paddingL">
                                        <span class="LblSmlGray">
                                            <asp:Label ID="lblUploadType" runat="server" EnableViewState="False" Text="Supports only .JPG, .JPEG, .PNG, .BMP, .PDF file type."></asp:Label><br />
                                            <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="File size should not exceed 1MB."></asp:Label>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentAchievement" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="Cancel" CausesValidation="False"
                                OnClick="btnCancel_Click" />
                            <asp:HiddenField ID="hidAchievementStudentId" runat="server" />
                            <asp:HiddenField ID="hidAchievementId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidAttachment" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentAchievement" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="lstvwStudentAchievement" runat="server" DataKeyNames="AchievementId"
                                OnItemDeleting="lstvwStudentAchievement_ItemDeleting" OnItemEditing="lstvwStudentAchievement_ItemEditing"
                                OnSelectedIndexChanged="lstvwStudentAchievement_SelectedIndexChanged" OnItemCommand="lstvwStudentAchievement_ItemCommand"
                                OnItemDataBound="lstvwStudentAchievement_ItemDataBound">
                                <LayoutTemplate>
                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                            <th align="left" class="clsLabelgrd" width="110px">
                                                <span><b>Class</b></span>
                                            </th>
                                            <th align="center" width="80px" class="clsLabelgrd">
                                                <span><b>Date</b></span>
                                            </th>
                                            <th align="left" width="250px" class="clsLabelgrd">
                                                <span><b>Description</b></span>
                                            </th>
                                            <th width="10px" align="center" class="clsLabelgrd">
                                                <asp:Label ID="lblAttachment" runat="server" Text="Attachment"> </asp:Label>
                                            </th>
                                            <th width="35px" align="center" class="clsLabelgrd">
                                                <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                            </th>
                                            <th width="10px" align="center" class="clsLabelgrd">
                                                <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:Label ID="lblStudentClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentClass") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAchievementDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("AchievementDate") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("Description") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnAttach" runat="server" CausesValidation="false" ToolTip="Attachment"
                                                CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:Label ID="lblStudentClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentClass") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAchievementDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("AchievementDate") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                Text='<%#Eval("Description") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnAttach" runat="server" CausesValidation="false" ToolTip="Attachment"
                                                CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentAchievement" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="cmbNoteCategory" EventName="SelectedIndexChanged" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" OnClientClick="ClosePopup(); return false;" />
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientFileUploadAchievement = "<%=this.FileUploadAchievement.ClientID %>";
        _clientcstValidateFileUpload = "<%=this.cstValidateFileUpload.ClientID %>";

        function IsValidFile(oSrc, args) {
            var sFileName = document.getElementById(_clientFileUploadAchievement).value;
            if (sFileName != "") {

                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {

                    args.IsValid = true;
                    return false;
                }

                else {
                    oSrc.errormessage = "Invalid File Type.";
                    document.getElementById(_clientcstValidateFileUpload).errormessage = "Invalid file type.";
                    args.IsValid = false;
                    return true;
                }
            }
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ClosePopup() {
            window.close();
        }

        function OpenFile(fileName) {
            window.open('../DOWNLOADS/StudentAchievement/' + fileName);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
