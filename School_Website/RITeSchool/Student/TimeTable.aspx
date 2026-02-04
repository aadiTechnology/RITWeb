<%@ Page Language="C#" MasterPageFile="../MasterPages/PopupMaster.master" AutoEventWireup="true"
    CodeFile="TimeTable.aspx.cs" Inherits="TimeTable" Title="Lecture Time Table" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div style="width: 90%" align="center">
        <table align="center" class="paddingLR" cellspacing="2" cellpadding="0" border="0"
            width="100%">
            <tbody>
                <tr>
                    <td style="height: 25px" align="left">
                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 20px" class="ClsGrayMainTitle">
                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                        <tr>
                                            <td align="center" class="MainTitleHead" style="height: 20px">                                              
                                                    <span id="tt" style="font-weight:bold">Time Table - Lecture Timings</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="TxtNormal padding10" align="left" valign="top">
                        <asp:Table align="center" border="0" CellPadding="3" CellSpacing="1" CssClass="TitleRBg"
                            Width="60%" runat="server" id="tblTimings">                           
                        </asp:Table>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TxtNormal padding10" valign="top">
                        <table align="center" border="0" cellpadding="3" cellspacing="1" class="TitleRBg"
                            width="60%">
                            <tr>
                                <td align="center" class="ColorBg" colspan="2">
                                    Stay Back Activity
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="dataBG TxtNormal paddingL" style="width: 45%">
                                    Thursday
                                </td>
                                <td align="left" class="dataBG TxtNormal paddingL" style="width: 50%">
                                    2:00 PM - 2:45 PM
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="dataBG TxtNormal paddingL">
                                    Friday
                                </td>
                                <td align="left" class="dataBG TxtNormal paddingL">
                                    2:00 PM - 2:45 PM
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnSml"/>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <script language="javascript" type="text/javascript" >
       
         function Closewindow()
         {
              window.close();
          }          
    </script>
</asp:Content>
