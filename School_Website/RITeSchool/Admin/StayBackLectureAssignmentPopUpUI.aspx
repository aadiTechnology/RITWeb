<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    CodeFile="StayBackLectureAssignmentPopUpUI.aspx.cs" Inherits="StayBackLectureAssignmentPopUpUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
        <ContentTemplate>
            <table width="100%" cellpadding="2" cellspacing="1">
                <tr>
                    <td align="left" rowspan="1" style="height: 5%">
                        <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                            <tr>
                                <td style="height: 20px">
                                    <asp:Label ID="lblIndentDetails" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                        Text="<%$ Resources:LocalizedResources, MsgStayBackLecturesClass %>" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="650px" cellpadding="2" cellspacing="1">
                            <tr>
                                <td colspan="4" align="center">
								<asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" ></asp:Label>
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr id="Tr2">
                                <td width="50px">
                                </td>
                                <td align="left" class="ClsBorderlight" width="150px">
                                    <span class="ClsLabel">
                                    <asp:Label ID="lblStandardText" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                    <span class="colonPadding"> :</span>
                                   </span>
                                </td>
                                <td align="left" class="ClsHilightBG" width="250px">
                                    <asp:Label ID="lblStandardName" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                </td>
                                <td width="50px">
                                </td>
                            </tr>
                            <tr id="Tr1">
                                <td width="50px">
                                </td>
                                <td align="left" class="ClsBorderlight" width="150px">
                                    <span class="ClsLabel">
                                     <asp:Label ID="lblDivisionText" runat="server" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                    <span class="colonPadding"> :</span>
                                    </span>
                                </td>
                                <td align="left" class="ClsHilightBG" width="250px">
                                    <asp:Label ID="lblDivisionName" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                </td>
                                <td width="50px">
                                </td>
                            </tr>
                            <tr id="Tr9">
                                <td width="50px">
                                </td>
                                <td align="left" class="ClsBorderlight"width="150px">
                                    <span class="ClsLabel">
                                    <asp:Label ID="lblWeekdayText" runat="server" Text="<%$ Resources:LocalizedResources, Weekday %>"></asp:Label>
                                    <span class="colonPadding"> :</span>
                                    </span>
                                </td>
                                <td align="left" class="ClsHilightBG" width="250px">
                                    <asp:Label ID="lblWeekDay" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                </td>
                                <td width="50px">
                                </td>
                            </tr>
                            <tr>
                                <td width="50px">
                                </td>
                                <td align="left" class="ClsBorderlight" width="150px">
                                    <span class="ClsLabel">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Lectures %>"></asp:Label>
                                    <span class="colonPadding"> :</span>
                                    </span>
                                </td>
                                <td align="left" class="ClsHilightBG"width="250px">
                                    <asp:Label ID="lblMaxLecturesNotAssigned" runat="server" Text="<%$ Resources:LocalizedResources, MsgMaximumLecturesNotAssigned %>"
                                        CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                    <asp:CheckBoxList ID="chklstLectures" runat="server" RepeatDirection="Horizontal"
                                        RepeatColumns="6">
                                    </asp:CheckBoxList>
                                </td>
                                <td width="50px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">                               
                                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                        ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" />
                                </td>
                                <td width="50px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
                                    <asp:HiddenField ID="hidWeekDay" runat="server" />
                                    <asp:HiddenField ID="hidWeekDayId" runat="server" />
                                    <asp:HiddenField ID="hidLectureType" runat="server"/>
                                    <asp:HiddenField ID="hidSuccessMessge" runat="server"/>
                                    <asp:HiddenField ID="hidStaybackLecture" Value="Stayback" runat="server" />
                                    <asp:HiddenField ID="hidAssemblyLecture" Value="Assembly" runat="server"/>
                                    <asp:HiddenField ID="hidMPTLecture" Value="MPT" runat="server"/>
                                    <asp:HiddenField ID="hidIndentMessage" runat="server"/>
                                    <asp:HiddenField ID="hidWeeklyTest" Value="WeeklyTest" runat="server"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        function CloseWindow() {
            window.opener.location = window.opener.location.pathname;
            window.close();
            window.opener.focus();
            return false;
        }

        function SetLabeles() {
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            lbl1.innerHTML = "";
        }
    </script>

</asp:Content>
