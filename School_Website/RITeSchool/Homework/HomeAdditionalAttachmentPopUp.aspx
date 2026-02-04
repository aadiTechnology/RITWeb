<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
 AutoEventWireup="true" CodeFile="HomeAdditionalAttachmentPopUp.aspx.cs" Inherits="HomeAdditionalAttachmentPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">

    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <span style="font-weight: bold">Documents</span>
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
                                        <table width="80%">
                                            <tr>
                                                <td id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <input type="hidden" id="hidHomeworkId" runat="server" />
                                        <asp:ListView ID="lstvwDocuments" runat="server" OnItemDataBound="lstvwDocuments_ItemDataBound"
                                            DataKeyNames="Id,HasLinkedHomework" OnItemCommand="lstvwDocuments_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="80%" runat="server" id="tblStaffInfo" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingL">
                                                            File Name
                                                        </th>
                                                        <th align="center" width="108px" id="thDelete" runat="server">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <%--<asp:Label ID="lblFileName" runat="server" Text='<%#Eval("AttachmentsName") %>'></asp:Label>--%>
                                                        <asp:ImageButton ID="imgFile" runat="server" Width="100px" Height="100px" CausesValidation="false" />
                                                        <asp:LinkButton ID="lnkFile" runat="server" CausesValidation="false" ></asp:LinkButton>
                                                    </td>
                                                    <td align="center" id="tdDelete" runat="server">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL">
                                                        <%--<asp:Label ID="lblFileName" runat="server" Text='<%#Eval("AttachmentsName") %>'></asp:Label>--%>
                                                        <asp:ImageButton ID="imgFile" runat="server" Width="100px" Height="100px" CausesValidation="false" />
                                                        <asp:LinkButton ID="lnkFile" runat="server" CausesValidation="false" ></asp:LinkButton>
                                                    </td>
                                                    <td align="center" id="tdDelete" runat="server">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <div class="LblNoRecord">
                                                    No Record Found.
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                       <asp:Button ID="btnClose" CssClass="ClsBtn" runat="server" Text="Close" CausesValidation="False"
                                                             OnClientClick="ClosePopup(); return false;" /><br />
                                        <asp:HiddenField ID="hidDeleteFromAll" runat="server" Value="N" />
                                    </td>
                                </tr>
                            </table>
                     
                </td>
                
            </tr>
            <asp:HiddenField ID="hidBtnState" runat="server" />
        </table>
        <script type="text/javascript" language="javascript">
            _clienthidDeleteFromAll = '<%=this.hidDeleteFromAll.ClientID %>'
            function ConfirmDelete(HasLinkedHomework) {
                var bResult = true
                $('#' + _clienthidDeleteFromAll).val('N')
                if (!window.confirm('Are you sure you want to delete this record?')) {
                    bResult = false
                }
                else {
                    if (HasLinkedHomework == 1) {
                        if (confirm('Do you want to delete this image from same homework of all other classes?\n\nClick on - \nOk Button - To delete from all classes.\nCancel Button - To delete from only this class.')) {
                            $('#' + _clienthidDeleteFromAll).val('Y')
                        }                        
                    }
                }
                return bResult
            }
            function ResetMessage() {
                if ($get("<%=this.lblMessage.ClientID %>") != null)
                    $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

            function CloseNewWindow(ItemCount) {
                window.opener.UpdateFileUploadCount(ItemCount);
                window.close();
                window.opener.focus();
            }

            function CloseAppWindow(ItemCount) {
                window.opener.UpdateFileUploadCount(ItemCount);
                window.close();
                window.opener.focus();
            }

            function CloseWindow() {
                window.close();
                window.opener.focus();
                window.opener.FilterPANDetails();
            }
            function ClosePopup() {
                window.close();
            }

            function ClosePerformanceWindow(Count, ClientId) {
                window.opener.focus();
                window.opener.RefreshLinkButton(Count, ClientId);
                window.close();
            }

            function OpenFile(fileName) {                 
                window.open(fileName)
            }

        </script>
    </div>

</asp:Content>

