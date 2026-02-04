<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="FeedbackUI.aspx.cs" Inherits="FeedbackUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Src="~/UserControls/FeedbackDetails.ascx" TagName="FeedbackDetails"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%">
                        <tr>
                            <td align="left">
                                <table align="left" border="0" width="100%">
                                    <tr>
                                        <td style="height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px">
                                            <uc1:FeedbackDetails ID="FeedbackDetails1" runat="server" />
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
    <script language="javascript" type="text/javascript">
      
    </script>
</asp:Content>
