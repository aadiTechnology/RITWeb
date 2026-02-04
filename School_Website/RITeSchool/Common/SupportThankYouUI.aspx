<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SupportThankYouUI.aspx.cs" Inherits="SupportThankYouUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;
            height: 100%">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 97%; height: 100%">
                        <tr>
                            <td align="center" colspan="4" valign="top">
                                <div class="pageContainer">
                                    <table cellpadding="0" cellspacing="2" style="width: 62%; padding: 1px 1px 1px 1xp"
                                        class="ClsBorderlight">
                                        <tr>
                                            <td align="center" class="ClsThankYouBG">
                                                Thank you for your request.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TxtNormal" style="padding-left:2px">
                                                Your support request information has been sent to our support team. Our Support
                                                Team will be reviewing your issue(s) as soon as possible and contact you via the
                                                e-mail address you entered in the online request form.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <span class="TxtNormal"><strong>Best regards,</strong>&nbsp;&nbsp;<br>
                                                    Site Administrator.</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4" valign="top">
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
