<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AchievementDetailsUI.aspx.cs" Inherits="AchievementDetailsUI"
    ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsgText" runat="server" CssClass="ClsLabel"
                                ViewStateMode="Enabled" ShowSummary="true" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:PostBackTrigger ControlID="btnCancelText" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAchievements" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center" id="tdMessage" runat="server">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                            EnableViewState="false" CssClass="ClsLabelNrml" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:PostBackTrigger ControlID="btnCancelText" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwAchievements" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trTextControls" runat="server" align="center">
                <td align="center">
                    <table id="tblTextNoticeControls" width="100%" runat="server" visible="true">
                        <tr>
                            <td align="center">
                                <table style="width: 80%">
                                    <tr>
                                        <td class="paddingL" align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table runat="server" id="tblJobControls" style="height: 52px; width: 500px">
                                                        <tr>
                                                            <td align="right" style="width: 150px" class="ClsBorderlight paddingL">
                                                                <span class="ClsLabel">Title :</span>
                                                            </td>
                                                            <td align="left" style="width: 50%" colspan="2">
                                                                <asp:TextBox ID="txtTitle" class="ExLrgTxtBox" runat="server" MaxLength="50"></asp:TextBox>
                                                                <span class="ClsMdtStar">*&nbsp;</span>
                                                                <asp:RequiredFieldValidator ID="reqPostName" runat="server" ErrorMessage="Title should not be blank."
                                                                    ControlToValidate="txtTitle" Display="None"> </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ClsBorderlight paddingL" align="left">
                                                                <span class="ClsLabel">Description :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtDescription" class="ExLrgTxtBox" runat="server" TextMode="MultiLine"
                                                                    Columns="21" Rows="4" Width="330px" Height="150px">
                                                                </asp:TextBox>
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                                                    ClientValidationFunction="ValidateDescription"></asp:CustomValidator>
                                                            </td>
                                                            <td valign="top">
                                                                <span class="ClsMdtStar">*&nbsp;</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight" valign="top">
                                                                <span class="ClsLabel">Display On Home Page? :</span>
                                                            </td>
                                                            <td align="left" class="ClsBorderlight" valign="top">
                                                                <asp:CheckBox ID="chkDisplayOnHomepage" runat="server" CssClass="ClsLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <table id="tblFileUpload" runat="server" width="100%">
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight" width="150px">
                                                                            <span class="ClsLabel">Select Photo :</span>
                                                                        </td>
                                                                        <td align="left" width="200px">
                                                                            <asp:FileUpload ID="flImage1" runat="server" CssClass="LrgTxtBox" />
                                                                        </td>
                                                                        <td width="70px">
                                                                            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:ImageButton ID="btnView1" runat="server" CausesValidation="false" ToolTip="View"
                                                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" Width="16px" /><span
                                                                                            style="width: 10px"></span>
                                                                                    <asp:ImageButton ID="imgbtnDelete1" runat="server" CausesValidation="false" ToolTip="Remove"
                                                                                        ImageUrl="../images/IconGrid_Delete.GIF" Visible="false" EnableViewState="true"
                                                                                        OnClick="imgbtnDelete1_Click" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnDelete1" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td align="left" class="ClsPaddingL">
                                                                            <asp:Label ID="lblErrMsg1" runat="server" CssClass="LblErrorMsg" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <span class="ClsLabel">Select Photo :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:FileUpload ID="flImage2" runat="server" CssClass="LrgTxtBox" EnableTheming="True" />
                                                                        </td>
                                                                        <td width="70px">
                                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:ImageButton ID="btnView2" runat="server" CausesValidation="false" ToolTip="View"
                                                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" /><span style="width: 10px"></span>
                                                                                    <asp:ImageButton ID="imgbtnDelete2" runat="server" CausesValidation="false" ToolTip="Remove"
                                                                                        ImageUrl="../images/IconGrid_Delete.GIF" Visible="false" EnableViewState="true"
                                                                                        OnClick="imgbtnDelete2_Click" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnDelete2" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td align="left" class="ClsPaddingL">
                                                                            <asp:Label ID="lblErrMsg2" runat="server" CssClass="LblErrorMsg" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <span class="ClsLabel">Select Photo :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:FileUpload ID="flImage3" runat="server" CssClass="LrgTxtBox" />
                                                                        </td>
                                                                        <td width="70px">
                                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:ImageButton ID="btnView3" runat="server" CausesValidation="false" ToolTip="View"
                                                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" /><span style="width: 10px"></span>
                                                                                    <asp:ImageButton ID="imgbtnDelete3" runat="server" CausesValidation="false" ToolTip="Remove"
                                                                                        ImageUrl="../images/IconGrid_Delete.GIF" Visible="false" EnableViewState="true"
                                                                                        OnClick="imgbtnDelete3_Click" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnDelete3" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td align="left" class="ClsPaddingL">
                                                                            <asp:Label ID="lblErrMsg3" runat="server" CssClass="LblErrorMsg" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <span class="ClsLabel">Select Photo :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:FileUpload ID="flImage4" runat="server" CssClass="LrgTxtBox" />
                                                                        </td>
                                                                        <td width="70px">
                                                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:ImageButton ID="btnView4" runat="server" CausesValidation="false" ToolTip="View"
                                                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" /><span style="width: 10px"></span>
                                                                                    <asp:ImageButton ID="imgbtnDelete4" runat="server" CausesValidation="false" ToolTip="Remove"
                                                                                        ImageUrl="../images/IconGrid_Delete.GIF" Visible="false" EnableViewState="true"
                                                                                        OnClick="imgbtnDelete4_Click" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnDelete4" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td align="left" class="ClsPaddingL">
                                                                            <asp:Label ID="lblErrMsg4" runat="server" CssClass="LblErrorMsg" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <span class="ClsLabel">Select Photo :</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:FileUpload ID="flImage5" runat="server" CssClass="LrgTxtBox" />
                                                                        </td>
                                                                        <td width="70px">
                                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:ImageButton ID="btnView5" runat="server" CausesValidation="false" ToolTip="View"
                                                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" /><span style="width: 10px"></span>
                                                                                    <asp:ImageButton ID="imgbtnDelete5" runat="server" CausesValidation="false" ToolTip="Remove"
                                                                                        ImageUrl="../images/IconGrid_Delete.GIF" Visible="false" EnableViewState="true"
                                                                                        OnClick="imgbtnDelete5_Click" />
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="imgbtnDelete5" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td align="left" class="ClsPaddingL">
                                                                            <asp:Label ID="lblErrMsg5" runat="server" CssClass="LblErrorMsg" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:PostBackTrigger ControlID="btnCancelText" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwAchievements" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button runat="server" Text="Save" class="ClsBtn" ID="btnSave" disable-page="true"
                                            ViewStateMode="Enabled" OnClick="btnSave_Click" />
                                        <asp:Button runat="server" Text="Cancel" class="ClsBtn" ID="btnCancelText" CausesValidation="False"
                                            OnClick="btnCancelText_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:PostBackTrigger ControlID="btnCancelText" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwAchievements" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trLink" runat="server">
                            <td>
                                <table id="tblLstvwAchievement" align="center" width="100%" runat="server">
                                    <tr>
                                        <td align="center" style="width: 100%">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table align="center" width="100%">
                                                        <tr id="trPager" runat="server" width="100%">
                                                            <td align="center">
                                                                <asp:ListView ID="lstvwAchievements" runat="server" DataKeyNames="Id" ViewStateMode="Enabled"
                                                                    OnItemCommand="lstvwAchievements_ItemCommand" OnItemDataBound="lstvwAchievements_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                            class="GridBorder" width="75%">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL">
                                                                                    Title
                                                                                </th>
                                                                                <th align="center" class="paddingL" style="width: 200px;">
                                                                                    Display On Home Page?
                                                                                </th>
                                                                                <th style="width: 100px">
                                                                                    <asp:Label ID="Label2" runat="server" Text="Image Count"></asp:Label>
                                                                                </th>
                                                                                <th align="center" style="width: 50px;">
                                                                                    Edit
                                                                                </th>
                                                                                <th align="center" style="width: 50px;">
                                                                                    Delete
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblAchievementitle" runat="server" Text='<%# Eval("AchievementTitle") %>'>
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Image ID="imgHomePage" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPhotoCount" runat="server" Text='<%#Eval("PhotoCount") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateCommand" CausesValidation="false"
                                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="RemoveCommand" CausesValidation="false"
                                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblAchievementitle" runat="server" Text='<%# Eval("AchievementTitle") %>'>
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Image ID="imgHomePage" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblPhotoCount" runat="server" Text='<%#Eval("PhotoCount") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateCommand" CausesValidation="false"
                                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="RemoveCommand" CausesValidation="false"
                                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td align="center" class="LblNoRecord">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwAchievements" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script>
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function OpenPopup(path) {
            window.open(path)
        }

        function ValidateDescription(oSrc, args) {

            var desc = $('#' + "<%=this.txtDescription.ClientID %>").val()

            if (desc.trim() == "") {
                oSrc.errormessage = "Description should not be blank.";
                args.IsValid = false
                return true
            }
            else if (desc.length > 1000) {
                oSrc.errormessage = "Description length should not be greater than 1000.";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false;
        }

        function ClearLabels() {
            $('#' + "<%=this.lblMessage.ClientID %>").html("")
        }

    </script>
</asp:Content>
