<%@ Page Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true"
    CodeFile="SiteMap.aspx.cs" Inherits="SiteMap" EnableViewState="false" %>

<%@ OutputCache Duration="1800" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="nifty" align="center" style="height: 100%; width: 97%;">
        <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
        </b></b>
        <table class="paddingLR" cellspacing="2" cellpadding="0" border="0" width="100%">
            <tr>
                <td align="center" class="borderBtm">
                    <table border="0" cellspacing="3" cellpadding="4" style="width: 99%; height: 100%;">
                        <tr>
                            <td align="left" class="HeadTxtB">
                                Site Map
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 98%; height: 100%;">
                    <table border="0" cellspacing="3" cellpadding="2" style="width: 100%; height: 100%;">
                        <tr>
                            <td align="center" valign="top" style="width: 12%;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="dataBG TxtB paddingLR borderBtm" align="left">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/images/Bullet.GIF" /><a href="Home.aspx"
                                                class="navST">Home</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="dataBG TxtB paddingLR borderBtm" align="left">
                                            <asp:Image ID="Image5" runat="server" ImageUrl="~/images/Bullet.GIF" /><a href="SiteMap.aspx"
                                                class="navST">Site Map&nbsp;</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="dataBG TxtB paddingLR borderBtm" style="width: 10%" align="left">
                                            <asp:Image ID="Image6" runat="server" ImageUrl="~/images/Bullet.GIF" /><a href="Contactus.aspx"
                                                class="navST">Contact Us</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="dataBG TxtB paddingLR borderBtm" align="left" runat="server" visible="false">
                                            <asp:Image ID="Image10" runat="server" ImageUrl="~/images/Bullet.GIF" /><a href="Careers.aspx"
                                                class="navST">Careers&nbsp;</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table style="width: 100%; height: 100%;" border="0">
                                    <tr>
                                        <td style="width: 15%" align="center">
                                            <asp:Image ID="Image13" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a href="OurAim.aspx" class="navST">Our Aim</a>
                                        </td>
                                        <td style="width: 15%" align="center">
                                            <asp:Image ID="Image7" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a href="Admission.aspx" class="navST">Admission</a>
                                        </td>
                                        <td style="width: 5%" align="center" runat="server" visible="false">
                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a href="Activities.aspx" class="navST">Activities</a>
                                        </td>
                                        <td style="width: 5%" align="center" runat="server" visible="false">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a href="Infrastructure.aspx" class="navST">Infrastructure</a>
                                        </td>
                                        <td style="width: 15%" align="center">
                                            <asp:Image ID="Image9" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a href="Management.aspx" class="navST">Management</a>
                                        </td>
                                        <td style="width: 5%" align="center" runat="server" visible="false">
                                            <asp:Image ID="Image19" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a class="navST">Information</a>
                                        </td>
                                        <td style="width: 5%" runat="server" visible="false" align="center">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Bullet.GIF" />
                                            <a class="navST">Appreciations</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 8%" align="center">
                                            <img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />
                                        </td>
                                        <td style="width: 10%" align="center">
                                            <img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />
                                        </td>
                                        <td style="width: 5%" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 5%" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td style="width: 5%" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtDivBG borderBtm" align="center" style="width: 9%; height: 30px;">
                                           
                                             <a href="MissionStatement.aspx" class="navSub">Mission Statement</a>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" style="width: 10%;">
                                            <a href="AdmissionPolicies.aspx" class="navSub">Admission Policies</a>
                                        </td>
                                        <td class="TxtDivBG borderBtm" align="center" runat="server" visible="false" style="width: 8%">
                                            <a href="ShowImageGallery.aspx" class="navSub">Photo/Video Gallery</a>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" runat="server" visible="false" style="width: 5%">
                                            <a href="BusTimingsDetails.aspx" class="navSub">Bus Route</a>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" runat="server" visible="false" style="width: 5%" >
                                            <a href="FeedbackListUI.aspx?wlHZOyPUhfm5/wwtuLvzmg==q" class="navSub">Parents</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 8%; height: 11px;" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td style="width: 10%; height: 11px;" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td style="width: 5%; height: 11px;" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td style="height: 11px">
                                        </td>
                                        <td style="height: 11px">
                                        </td>
                                        <td style="width: 5%; height: 11px;" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td style="width: 5%; height: 11px;" align="center">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TxtDivBG borderBtm" align="center" runat="server" visible="false" style="width: 8%; height: 30px;">
                                             <a href="PrincipalMsg.aspx" class="navSub">Principal's Message</a>
                                        </td>
                                        <td class="TxtDivBG borderBtm" runat="server" visible="false" align="center" style="width: 10%" >
                                            <%--                                            <a href="Guidelines.aspx" class="navSub" style="width: 28%">Registration Guidelines</a>--%>
                                            <a href="Guidelines.aspx" class="navSub" style="width: 10%">Registration Guidelines</a>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <%--<a href="ShowImageGallery.aspx" class="navSub" style="width: 28%">Photo/Video Gallery</a>--%>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" runat="server" visible="false" style="width: 7%">
                                            <a href="School_Notices.aspx" class="navSub">School Notices</a>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" runat="server" visible="false" style="width: 5%">
                                            <a href="FeedbackListUI.aspx?HTM+HFqsE3QbEML4MpFILg==q" class="navSub">Others</a>
                                        </td>
                                        <%--<td class="TxtDivBG borderBtm" align="center">
                                            <a href="Entrance_Exam_Result_New.aspx" class="navSub" style="width: 28%">Entrance Test
                                                Result</a>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <%--<img src="images/VerticalArrowRed.GIF" alt="image" class="paddingL" />--%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center" class="TxtDivBG borderBtm" runat="server" visible="false" style="width: 7%">
                                            <a href="PublicHolidayList.aspx" class="navSub">Public Holidays</a>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
        </b></b>
    </div>
</asp:Content>
