<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    CodeFile="OnlineAdmissionDashBoardUI.aspx.cs" Inherits="OnlineAdmissionDashBoardUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <script type="text/javascript" language="javascript">
        var _clientTdCongrates = "<%= this.tdCongrates.ClientID %>";
        var _clientTrWaitingList = "<%= this.trWaitingList.ClientID %>";
        var _clienttrNotSelected = "<%= this.trNotSelected.ClientID %>";
        
        function blinkIt() {
            s = document.getElementById(_clientTdCongrates);
            s1 = document.getElementById(_clientTrWaitingList);
            s2 = document.getElementById(_clienttrNotSelected);
            if (s != null)
                s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
            
        }
    </script>

    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;
        height: 100%">
        <tr>
            <td id="MainDataTable" align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 97%; height: 100%">
                    <tr>
                        <td align="center" colspan="4" valign="top">
                            <table cellpadding="0" cellspacing="0" style="width: 100%; height: 100%" class="ClsBorderlight">
                                <tr>
                                    <td style="width: 100%" align="left" valign="top">
                                        <table style="width: 100%" align="left">
                                            <tr align="center">
                                                <td class="ClsGridRow">
                                                    <strong>Welcome to Online Admission Process.</strong>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trAdmissionConform" runat="server" visible="false">
                        <td align="center" valign="middle">
                            <table style="height: 100%; vertical-align: middle">
                                <tr>
                                    <td id="tdCongrates" runat="server" align="center" style="font-family: Tahoma; font-size: large;
                                        font-weight: bold; color: #990066">
                                        Congratulations!!!

                                        <script type="text/javascript" language="javascript">
                                            setInterval('blinkIt()', 600)
                                        </script>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-size: 10pt;">
                                        <strong>Your child is selected in admission lottary process.<br />
                                            Please submit all required documents along with fees at <%= ConfigurationManager.AppSettings["SchoolName"]%>, Pune for confirmation
                                            of your child's admission.</strong>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trInProcess" runat="server" visible="true" colspan="4" style="height: 100px">
                        <td align="center" style="font-size: 10pt;">
                            <strong>Your child's admission is in process.<br />
                                Admission seats will be allotted by a lottery system on
                                <asp:Label runat="server" ID="lblLotterydate" Font-Italic="False" Font-Underline="True"></asp:Label>
                                at a random basis as selected by our software.</strong>
                        </td>
                    </tr>
                    <tr id="trWaitingList" runat="server" visible="false" colspan="4" style="height: 100px">
                        <td align="center" style="font-size: medium; color: Black">
                            <strong>Your child is on the <%= ConfigurationManager.AppSettings["SchoolName"]%> admission waiting list. In case, if students from the
                                main list cancels the admission, your child will be given a chance for the admission.</strong>

                            <script type="text/javascript" language="javascript">
                                setInterval('blinkIt()', 600)
                            </script>

                        </td>
                    </tr>
                    <tr id="trNotSelected" runat="server" visible="false" colspan="4" style="height: 100px">
                        <td align="center" style="font-size: medium; color:Black">
                            <strong>We apologize that your child is not selected from the admission lottery system
                                generated by <%= ConfigurationManager.AppSettings["SchoolName"]%>.</strong>

                            <script type="text/javascript" language="javascript">
                                setInterval('blinkIt()', 600)
                            </script>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4" valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
