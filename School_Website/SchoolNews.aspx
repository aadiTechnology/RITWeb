<%@ Page Title="" Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true"
    CodeFile="SchoolNews.aspx.cs" Inherits="SchoolNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 95%" align="center">
        <div id="nifty" align="center">
            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
            </b></b>
            <table width="100%">
                <tr>
                    <td class="HeadTxtB borderBtm" height="25px" align="left">
                        <label id="lblheadr" runat="server">School News
                        </label>
                    </td>
                </tr>
                <%-- <tr id="trNoRecord" runat="server">
                    <td class="LblNoRecord" align="center">
                        No Record Found.
                    </td>
                </tr>--%>
                <tr id="trOther" runat="server">
                    <td align="center">
                        <table id="tblParameter" runat="server" width="95%">
                        </table>
                    </td>
                </tr>
            </table>
            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
            </b></b>
        </div>
    </div>
</asp:Content>
