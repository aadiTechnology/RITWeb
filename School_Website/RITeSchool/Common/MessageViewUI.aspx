<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="MessageViewUI.aspx.cs" Inherits="MessageViewUI" ViewStateMode="Disabled" %>

<%--<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" id="tblInbox" runat="server" viewstatemode="Enabled" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 95%; height: 100%">
                        <tr>
                            <td align="left" colspan="4" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td align="left" width="150px">
                                        </td>
                                        <td align="right" width="690px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">From :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblFromUserName" runat="server" ViewStateMode="Enabled" CssClass="ClsLblRslt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel" id="spReceivedDate" runat="server" viewstatemode="Enabled">Received Date :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblReceivedDate" runat="server" ViewStateMode="Enabled" CssClass="ClsLblRslt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">To :</span>&nbsp;
                                        </td>
                                        <td align="left" style="" class="ClsBorderlight">
                                            <asp:Label ID="lblToUserName" runat="server" ViewStateMode="Enabled" CssClass="ClsLblRslt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Cc :</span>&nbsp;
                                        </td>
                                        <td align="left" style="" class="ClsBorderlight">
                                            <asp:Label ID="lblCcUserName" runat="server" ViewStateMode="Enabled" CssClass="ClsLblRslt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Subject :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblSubject" runat="server" ViewStateMode="Enabled" CssClass="ClsLblRslt"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tdAttachment" runat="server" viewstatemode="Enabled" visible="false">
                                        <td align="left" style="" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Attachment1 :</span><br />
                                        </td>
                                        <td align="left" style="" valign="top">
                                            <asp:HyperLink ID="lnkAttachment" runat="server" ViewStateMode="Enabled" CssClass="CursorHand ClsLblRslt"
                                                Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr id="tdAttachment1" runat="server" viewstatemode="Enabled" visible="false">
                                        <td align="left" style="" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Attachment2 :</span><br />
                                        </td>
                                        <td align="left" style="" valign="top">
                                            <asp:HyperLink ID="lnkAttachment1" runat="server" ViewStateMode="Enabled" CssClass="CursorHand ClsLblRslt"
                                                Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr id="tdAttachment2" runat="server" viewstatemode="Enabled" visible="false">
                                        <td align="left" style="" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Attachment3 :</span><br />
                                        </td>
                                        <td align="left" style="" valign="top">
                                            <asp:HyperLink ID="lnkAttachment2" runat="server" ViewStateMode="Enabled" CssClass="CursorHand ClsLblRslt"
                                                Target="_blank" ToolTip="Click to download the file." Font-Size="Smaller" Font-Underline="true"></asp:HyperLink>
                                        </td>
                                    </tr>

                                    <tr>
                                     <td align="left" style="" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Attachments :</span><br />
                                        </td>
                                        <td align="left" style="" valign="top">
                                          <asp:Panel ID="pnl" runat="server" style="height:auto">
                                          
                                          </asp:Panel>
                                        </td>
                                    
                                    </tr>
                                    <tr>
                                        <td align="left" style="" class="ClsBorderlight" valign="top">
                                            <span class="ClsLabel">Message :</span><br />
                                        </td>
                                        <td align="left" style="" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <%--<CKEditor:CKEditorControl ID="FCKMessageBody" ReadOnly="true" Toolbar="" BasePath="../ckeditor/" Visible="false"
                                                Width="100%" runat="server" ViewStateMode="Enabled" Height="350px" ToolbarCanCollapse="False"></CKEditor:CKEditorControl>--%>
                                            <div id="divData" runat="server" style="overflow:auto;border-style:solid;border-color:Gray;border-width:1px;padding:5px;background-color:lightGray;width:100%;height:400px;background-color:#FCFCFC">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <table align="center" border="0" cellpadding="0" cellspacing="0" height="100%" width="100%">
                                                <tr>
                                                    <td align="left" style="width: 30%">
                                                        <asp:Button ID="btnBack" UseSubmitBehavior="false" OnClick="btnBack_Click" runat="server"
                                                            ViewStateMode="Enabled" Text="Back" CssClass="ClsBtnSml" BorderWidth="1px" BorderStyle="Solid" Visible="True">
                                                        </asp:Button>
                                                        <asp:Button ID="btnGoToInbox" Text="Go To Inbox" runat="server" ViewStateMode="Enabled" OnClick="btnGoToInbox_Click"
                                                            CssClass="ClsBtnLrg" UseSubmitBehavior="false" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnForward" Text="Forward" runat="server" ViewStateMode="Enabled" OnClick="btnForward_Click"
                                                            CssClass="ClsBtnLrg" />
                                                        <asp:Button ID="btnReply" Text="Reply" runat="server" ViewStateMode="Enabled" OnClick="btnReply_Click" CssClass="ClsBtnLrg" />
                                                        <asp:Button ID="btnReplyToAll" Text="Reply To All" runat="server" ViewStateMode="Enabled" OnClick="btnReplyToAll_Click"
                                                            CssClass="ClsBtnLrg" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="">
                                            <asp:HiddenField ID="HidBackUrl" runat="server" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidShowRequestMessage" runat="server" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidDraftMessage" runat="server" Value = "0" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidAccYearId" runat="server" Value = "0" ViewStateMode="Enabled" />
                                            <asp:HiddenField ID="hidRestrictCopy" runat="server" Value = "0" ViewStateMode="Enabled" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
    <div id="divSetting" runat="server" viewstatemode="Enabled" align="center" style="visibility: hidden; display: none;
        position: absolute; margin: 0px; padding: 0px; width: 30%; border-width: 1px;
        left: 10px; top: 150px; line-height: normal; border: solid 2px darkgreen; background-color: white;">
        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
            background-repeat: repeat-x; color: Black; width: 100%; text-align: right">
            <div style="font-size: 12px; width: 50%; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                Read Receipt Confirmation
            </div>
        </div>
        <div>
            <table>
                <tr>
                    <td style="horrizontal-align: left;">
                        <asp:Label ID="lblsiblingNote" runat="server" ViewStateMode="Enabled" Text="The sender of this message has requested  'Read Receipt'. Do you want to send it?"></asp:Label>
                    </td>
                </tr>
            </table>
            <div>
                <table>
                    <tr>
                        <td colspan="2" align="center" valign="bottom" style="padding: 10px;">
                            <asp:Button ID="btnSavePopUp" runat="server" ViewStateMode="Enabled" Text="Yes" CssClass="ClsBtnMid" CausesValidation="true"
                                Width="75px" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="No" CssClass="ClsBtnMid"
                                CausesValidation="true" Width="75px" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        _clientDivSettings = "<%=this.divSetting.ClientID %>"
        _clienthidShowRequestMessage = "<%=this.hidShowRequestMessage.ClientID %>"
        _clienttblInbox = "<%=this.tblInbox.ClientID %>"

        $(document).ready(function () {
            $(this).on("copy cut", function (e) {
                if ($('[id$=hidRestrictCopy]').val() == '1') {
                    alert('Cut/Copy is disabled.')
                    e.preventDefault();
                }
            });
        });
    </script>
    <script src="../Scripts/Common/MessageView.js" type="text/javascript"></script>
</asp:Content>
