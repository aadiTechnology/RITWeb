<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="DatewiseStaffLeavesPopup.aspx.cs" Inherits="DatewiseStaffLeavesPopup" %>

<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">  
    <style>
            .TopPadding
            {
                padding-top:4px;
            }
    </style>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr id="trNoLeaveConfig" runat="server">
                <td valign="top" align="center">
                    <table width="50%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblNoLeaveConfigured" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="Yearwise leaves are not configured for any user." EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCloseTop" runat="server" Text="Close" CausesValidation="false"
                                    CssClass="ClsBtn" OnClick="btnClose_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trLeaveDetails" runat="server">
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td align="left" colspan="4">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                <tr>
                                                    <td align="center" class="MainTitleHead" style="height: 20px">
                                                        <span style="font-weight: bold">User Leaves</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>                        
                          <td>
                          <table align="right">
                             <tr>
                                 <td align="right" style="height: 25px" class="ClsGreenBG">
                                     <asp:LinkButton ID="lnkODDetails" runat="server" Text="On Duty (O.D) Details"
                                       CssClass="SubTitle"></asp:LinkButton>
                                 </td>
                             </tr>
                          </table>                              
                          </td>
                        </tr>
                        <tr id="trStudentCombo" runat="server">
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="center" colspan="5">
                                            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblSuccessMessage" runat="server" Font-Bold="True" ForeColor="Blue"
                                                        EnableViewState="False"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />                                                   
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" ForeColor="Red"
                                                        EnableViewState="False"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="td2" align="center">
                                            <table align="center" class="ClsBorderlight">
                                                <tr>
                                                    <td style="width:100px;">
                                                        <span class="ClsLabel" style="font-weight: bold">Name :</span>
                                                    </td>                                        
                                                    <td align="left">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" Width="200px" autocomplete="off"></asp:TextBox>
                                                    </td>                                        
                                                    <td align="left">
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn"
                                                            CausesValidation="false" onclick="btnSearch_Click" />                                    
                                                    </td>
                                                </tr>
                                              </table>
                                          </td>
                                           <td runat="server" id="td3" align="center">
                                            <asp:Button ID="btnPullBioData" runat="server" Text="Pull Biometric Data" CssClass="ClsBtn" CausesValidation="false" />
                                          </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>                                        
                                            <table class="ClsBorderlight">
                                                <tr>
                                                    <td runat="server" id="td1">
                                                        <span class="ClsLabel" style="font-weight: bold">Staff Group :</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbStaffGroup" runat="server" AutoPostBack="True" Width="200px"
                                                            Font-Size="Small" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="10px">
                                                    </td>
                                                    <td runat="server" id="tdlblStudent">
                                                        <span class="ClsLabel" style="font-weight: bold">User :</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbUsers" runat="server" AutoPostBack="True" Width="200px"
                                                                    OnSelectedIndexChanged="cmbUsers_SelectedIndexChanged" Font-Size="Small">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td class="ClsGreenBG">
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:HyperLink ID="lnkPartialLeave" runat="server" Text="Partial Leave" NavigateUrl="" CssClass="SubTitle"></asp:HyperLink>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>                                                    
                                                    <td align="center" class="ClsGreenBG">
                                                        <asp:LinkButton ID="lnkUserInfo" runat="server" CssClass="SubTitle">User Details</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        </Triggers>
                                        </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                           <td align="center">                           
                                <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                   Text="No record found." Visible="false" EnableViewState="False" Width="99%"></asp:Label>                               
                           </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" align="center" class="ClsBorderlight">
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLblLgnd">Leave Balance : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLeaveBalance" runat="server" CssClass="ClsLabel" EnableTheming="True"
                                                                    Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLblLgnd">Late Mark Leaves : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLateMarkLeaves" runat="server" CssClass="ClsLabel" BorderWidth="0px"
                                                                    Font-Bold="True" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLblLgnd">Required Minimum Balance : </span>
                                                </td>
                                                <td class="ClsBorderlight" colspan="2">
                                                    <table align="left">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblRequiredBalamce" runat="server" CssClass="ClsLabel" EnableTheming="True"
                                                                    Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLblLgnd">Used Leaves : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblUsedLeaves" runat="server" CssClass="ClsLabel" BorderWidth="0px"
                                                                    Font-Bold="True" Text="Used Leaves :"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLblLgnd">Partial Leaves : Date(Leave Type) </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPartialLeaves" runat="server" CssClass="ClsLabel" BorderWidth="0px"
                                                                    Font-Bold="True" Text="Partial Leaves"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" style="height: 27px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLegend" runat="server" CssClass="ClsLblLgnd" EnableTheming="True"
                                                                    Text="Legend : "></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblHalfLeaveColor" runat="server" BackColor="#009B9B" Height="20px"
                                                                    Text="H" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True"
                                                                    Width="20px" EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="Label3" runat="server" CssClass="ClsLblLgnd" EnableTheming="True"
                                                                    Text="H - Half Leave"></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lvlLateMarkColor" runat="server" BackColor="#01A5EC" Height="20px"
                                                                    Text=" &lt;img 
                                                                    src=&quot;../images/spacer.gif&quot; width=&quot;20px&quot; height=&quot;20px&quot;/&gt;"
                                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                                    EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="Label4" runat="server" CssClass="ClsLblLgnd" EnableTheming="True"
                                                                    Text="L - Late Mark"></asp:Label>
                                                            </td>
                                                            <td width="10px">
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="Label13" runat="server" CssClass="ClsLblLgnd" EnableTheming="True"
                                                                    Text="P - Partial Leave"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="ClsBorderlight" style="height: 27px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblLateMark" runat="server" CssClass="ClsLabel" EnableTheming="True"
                                                                    Text="Late Mark(s) : 0" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="vertical-align: middle;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lbl" runat="server" CssClass="ClsLabel" Font-Bold="True">
                                                                        Select leave for all days :
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:DropDownList ID="cmbAllLeaves" runat="server" CssClass="SmlCombo" Font-Size="Small">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:PostBackTrigger ControlID="LeaveCalendar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trcalendar" runat="server">
                            <td colspan="4" align="center" valign="top">
                                <asp:UpdatePanel runat="server" ID="updateCalender" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <Calender:EventCalendar ID="LeaveCalendar" runat="server" BackColor="White" BorderColor="Silver"
                                            CellPadding="0" DayNameFormat="Full" EventBackColorName="" EventDescriptionColumnName=""
                                            EventEndDateColumnName="" EventForeColorName="" EventHeaderColumnName="" EventStartDateColumnName=""
                                            Font-Names="Arial" Font-Size="8pt" ForeColor="Blue" Height="310px" NextPrevFormat="FullMonth"
                                            ShowDescriptionAsToolTip="True" ShowGridLines="True" Width="100%" OnVisibleMonthChanged="LeaveCalendar_VisibleMonthChanged"
                                            Font-Bold="False" SelectionMode="None">
                                            <SelectedDayStyle BackColor="#E7E7E7" Font-Bold="True" ForeColor="Black" BorderColor="LightSteelBlue"
                                                BorderStyle="Solid" BorderWidth="1px" />
                                            <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                            <WeekendDayStyle BackColor="White" Font-Bold="False" ForeColor="#CC0099" />
                                            <OtherMonthDayStyle ForeColor="#999999" Height="50px" />
                                            <NextPrevStyle Font-Size="8pt" HorizontalAlign="Left" ForeColor="Navy" />
                                            <DayHeaderStyle ForeColor="White" Height="25px" CssClass="DayHeader" />
                                            <TitleStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="25px" BorderStyle="None"
                                                CssClass="MonthHeader" />
                                            <DayStyle Height="46px" />
                                        </Calender:EventCalendar>
                                        <table id="tblNote" runat="server" align="center" width="100%">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Except leave days, remaining days will be considered as present days."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label7" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label8" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Leave balance is displayed including the leaves of selected month (i.e. Leave Balance = Used Leaves + Late Mark Leaves)."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note3 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label10" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="User needs to save leave(s) to reflect changes made in late mark configuration."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr2" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note4 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblDates" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To pay salary according to joining / resign date, you will have to click on Save button after setting joining / resign date."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trLateMarkNote" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label5" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note5 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblLateMarkNote" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trLeaveSortOrder" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label6" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note6 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblLeaveSortOrder" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trHoliday" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="lblHolidayHeader" runat="server" BorderWidth="0px" Font-Bold="True"
                                                        Text="Note7 :" CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblHoliday" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trWeekend" runat="server">
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="lblWeekendHeader" runat="server" BorderWidth="0px" Font-Bold="True"
                                                        Text="Note8 :" CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblWeekend" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSalaryMonthId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSalaryYear" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidLateMarkCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidLateMarkDays" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMaxLateMarkCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidConsideredLeaves" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidColorCodes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidLeaveIds" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIsSalaryDeducted" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidHolidayDates" runat="server" Value="" />
                                        <asp:HiddenField ID="hidEnclosedDates" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAttachedDates" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIsPreEnclosed" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidIsPostEnclosed" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidConfigIds" runat="server" Value="" />
                                        <asp:HiddenField ID="hidDeductHolidayLeaves" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidAttEncConfig" runat="server" Value="" />
                                        <asp:HiddenField ID="hidHolidays" runat="server" Value="" />
                                        <asp:HiddenField ID="hidUsedLeaveBkp" runat="server" Value="Y" />
                                        <asp:HiddenField ID="hidLateMarkBkp" runat="server" Value="Y" />
                                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidPartialLeaves" runat="server" Value="" />
                                        <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                                        <asp:HiddenField ID="hidStaffGroupsId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedUserId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidExcludedLeaves" runat="server" Value="" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAllowZeroBalance" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBioData" runat="server" Value="" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlHolidayLeaves" runat="server">
                                    <ContentTemplate>
                                        <table align="center">
                                            <tr>
                                                <td class="ClsBorderlight" runat="server" id="tdDeductHolidayLeaves">
                                                    <asp:CheckBox ID="chkDeductHolidayLeaves" runat="server" Text="Consider Holiday Leaves" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="ClsBtn"
                                                OnClick="btnClose_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidStaffGroup" runat="server" Value="0" />
                            <asp:HiddenField ID="hidHalfLeaves" runat="server" Value="0" />
                            <asp:HiddenField ID="hidLeave" runat="server" Value="0" />
                            <asp:HiddenField ID="hidDay" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hidClosePopup" runat="server" Value="0" OnValueChanged="hidClosePopup_ValueChanged" />
                    <asp:HiddenField ID="hidTest" runat="server" Value="-" />
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <asp:HiddenField ID="hidIsLeaveConfigured" runat="server" Value="N" />
        </table>         
       <div id="divPopup" style="
                    position: fixed; margin: 0px; padding: 0px; width: 400px;  height: 300px; border-width: 0px;display:none;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 200px 250px;
                    background-color: white;" >
                     <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 100%; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label44" runat="server" Text="User Details"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideUserInfo();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                         <table>
                    <tr>
                        <td align="left" style="width:120px;">
                            <span class="ClsLblLgnd">Name</span>                        
                        </td>
                        <td align="center" style="width:20px;">
                            <span style="font-weight:bold">:</span>
                        </td>
                        <td align="left" class="TopPadding">                        
                            <span id="spnName" runat="server" class="clsLabel" style="font-weight:bold;"></span>
                        </td>
                    </tr>                    
                    <tr>
                        <td align="left">
                            <span class="ClsLblLgnd">Joining Date</span>                        
                        </td>
                        <td align="center">
                            <span style="font-weight:bold">:</span>
                        </td>
                        <td align="left" class="TopPadding">                        
                            <span id="spnJoiningDate" runat="server" class="clsLabel" style="font-weight:bold;"></span>
                        </td>
                    </tr>
                        <tr>
                        <td align="left">
                            <span class="ClsLblLgnd">Permanent Date</span>                        
                        </td>
                        <td align="center">
                            <span style="font-weight:bold">:</span>
                        </td>
                        <td align="left" class="TopPadding">                        
                            <span id="spnPermanentDate" runat="server" class="clsLabel" style="font-weight:bold;"></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span class="ClsLblLgnd">Photo</span>                        
                        </td>
                        <td align="center">
                            <span style="font-weight:bold">:</span>
                        </td>
                        <td align="left">                        
                            <img id="imgPhoto" alt="-" runat="server" height="151" width="119" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnCloseDiv" runat="server" Text="Close" CssClass="ClsBtn" OnClientClick="HideUserInfo(); return false;" />
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                    </asp:UpdatePanel>
            </div>    
    </div>
    <script type="text/javascript" language="javascript">

        _clienthidHalfLeaves = "<%=this.hidHalfLeaves.ClientID %>";
        _clienthidLeave = "<%=this.hidLeave.ClientID %>";
        _clienthidDay = "<%=this.hidDay.ClientID %>";
        _clientcmbUsers = "<%=this.cmbUsers.ClientID %>";
        _clientcmbAllLeaves = "<%=this.cmbAllLeaves.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _clienthidMonthId = "<%=this.hidMonthId.ClientID %>";
        _clienthidYear = "<%=this.hidYear.ClientID %>";
        _clienthidSalaryMonthId = "<%=this.hidSalaryMonthId.ClientID %>";
        _clienthidSalaryYear = "<%=this.hidSalaryYear.ClientID %>";
        _clientLeaveCalendar = "<%=this.LeaveCalendar.ClientID %>";
        _clienthidClosePopup = "<%=this.hidClosePopup.ClientID %>";
        _clientlblSuccessMessage = "<%=this.lblSuccessMessage.ClientID %>";
        _clienthidLateMarkCount = "<%=this.hidLateMarkCount.ClientID %>";
        _clienthidLateMarkDays = "<%=this.hidLateMarkDays.ClientID %>";
        _clienthidMaxLateMarkCount = "<%=this.hidMaxLateMarkCount.ClientID %>";
        _clienthidConsideredLeaves = "<%=this.hidConsideredLeaves.ClientID %>";
        _clienthidLateMarkCount = "<%=this.hidLateMarkCount.ClientID %>";
        _clienthidColorCodes = "<%=this.hidColorCodes.ClientID %>";
        _clienthidHolidayDates = "<%=this.hidHolidayDates.ClientID %>";
        _clientchkDeductHolidayLeaves = "<%=this.chkDeductHolidayLeaves.ClientID %>";
        _clienthidIsSalaryDeducted = "<%=this.hidIsSalaryDeducted.ClientID %>";
        _clienthidDeductHolidayLeaves = "<%=this.hidDeductHolidayLeaves.ClientID %>";
        _clientcmbStaffGroup = "<%=this.cmbStaffGroup.ClientID %>";
        _clientlblLateMark = "<%=this.lblLateMark.ClientID %>";
        _clientlblUsedLeaves = "<%=this.lblUsedLeaves.ClientID %>";
        _clienthidHolidays = "<%=this.hidHolidays.ClientID %>";
        _clienthidUsedLeaveBkp = "<%=this.hidUsedLeaveBkp.ClientID %>";
        _clienthidLateMarkBkp = "<%=this.hidLateMarkBkp.ClientID %>";
        _clienthidPartialLeaves = "<%=this.hidPartialLeaves.ClientID %>";

        _clinetTest = "<%=this.hidTest.ClientID %>";
        _clienthidExcludedLeaves = "<%=this.hidExcludedLeaves.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            DisableControls(false);
            var postBackElement = sender._postBackSettings.sourceElement;

            if (postBackElement.id == _clientLeaveCalendar) {
                document.getElementById(_clientlblSuccessMessage).innerHTML = "";
                document.getElementById(_clientcmbAllLeaves).value = "0";
            }

            UpdateLateMark(1);
            UpdateUsedLeaves();
            DisableCalender();
        }
        function beginRequestHandler(sender, args) {
            DisableControls(true);
        }

        function DisableControls(action) {
            if (document.getElementById(_clientbtnSave) != null)
                document.getElementById(_clientbtnSave).disabled = action
            if (document.getElementById(_clientbtnClose) != null)
                document.getElementById(_clientbtnClose).disabled = action
            if (document.getElementById(_clientcmbUsers) != null)
                document.getElementById(_clientcmbUsers).disabled = action
            if (document.getElementById(_clientcmbStaffGroup) != null)
                document.getElementById(_clientcmbStaffGroup).disabled = action
        }

        DisableCalender();

        function DisableCalender() {
            if ($get("<%=this.hidIsLeaveConfigured.ClientID %>").value != "N") {
                var sIds = ''
                var checks = document.forms[0].elements;
                var boxLength = checks.length;
                var allChecked = document.getElementById(_clientcmbAllLeaves).value;
                var totalChecked = 0;
                var selectedMonthId = parseInt(document.getElementById(_clienthidMonthId).value);
                var selectedYear = parseInt(document.getElementById(_clienthidYear).value);
                var salMonthId = parseInt(document.getElementById(_clienthidSalaryMonthId).value);
                var salYear = parseInt(document.getElementById(_clienthidSalaryYear).value);

                var IsValid = (selectedYear < salYear || selectedYear > salYear) ? false : selectedMonthId >= salMonthId ? true : false;

                document.getElementById(_clientbtnSave).disabled = !IsValid;
                for (j = 0; j < boxLength; j++) {
                    if ((checks[j].type == 'select-one' || checks[j].type == 'checkbox') && checks[j].id != _clientcmbUsers && checks[j].id != _clientcmbStaffGroup) {
                        checks[j].disabled = !IsValid;
                    }

                    if (IsValid && checks[j].type == 'checkbox' && checks[j].id.substring(0, checks[j].id.indexOf('_')) == "chk" && checks[j].checked == false && checks[j - 1].value == "0") {
                        checks[j].disabled = true;
                    }

                    if (IsValid && checks[j].type == 'checkbox' && checks[j].id.substring(0, checks[j].id.indexOf('_')) == "chkLateMark" && checks[j - 1].checked == false && checks[j - 2].value != "0") {
                        checks[j].disabled = true;
                    }
                }
            }
        }

        function DisableCheckbox(cmbLeave, chk, lateMark, dayIndex) {
            var selectedLeave = cmbLeave.value == 0;
            chk.disabled = selectedLeave;
            lateMark.disabled = selectedLeave;
            var halfLeaveSpan = document.getElementById("halfLeavespan" + dayIndex);
            var lateMarkSpan = document.getElementById("lateMarkSpan" + dayIndex);
            if (selectedLeave) {
                halfLeaveSpan.style.backgroundColor = "transparent";
                lateMarkSpan.style.backgroundColor = "transparent";
            }

            cmbLeave.style.backgroundColor = cmbLeave.options[cmbLeave.selectedIndex].style.backgroundColor;
            if (selectedLeave) {
                chk.checked = false;
            }

            if (cmbLeave.value == 0) {
                lateMark.checked = false;
                lateMark.disabled = false;
            }
            else {
                if (chk.checked) {
                    lateMark.disabled = false;
                }
                else {
                    lateMark.checked = false;
                    lateMark.disabled = true;
                    lateMarkSpan.style.backgroundColor = 'Transparent';
                }
            }
        }

        function OnCheck(chk, lateMark) {
            if (chk.checked) {
                lateMark.disabled = false;
                lateMark.checked = false;
            }
            else {
                lateMark.disabled = true;
                lateMark.checked = false;
            }
        }

        UpdateLateMark(1);
        function UpdateLateMark(displayLateMark) {
            if ($get("<%=this.hidIsLeaveConfigured.ClientID %>").value != "N") {
                $get(_clienthidLateMarkCount).value = "0";

                var cmb = document.forms[0].elements;
                var boxLength = cmb.length;
                var lateMarkCount = 0;

                for (j = 1; j < boxLength; j++) {
                    if (cmb[j].type == 'checkbox' && cmb[j].id.substring(0, cmb[j].id.indexOf('_')) == "chkLateMark") {
                        {
                            if (cmb[j].checked)
                                lateMarkCount++;
                        }
                    }
                }
                $get(_clienthidLateMarkCount).value = lateMarkCount;
                if (displayLateMark != 0)
                    $get(_clientlblLateMark).innerHTML = "Late Mark(s) : " + lateMarkCount;
            }
        }
        UpdateUsedLeaves();
        function UpdateUsedLeaves() {
            if ($get("<%=this.hidIsLeaveConfigured.ClientID %>").value != "N") {
                var cmb = document.forms[0].elements;
                var boxLength = cmb.length;
                var leaveNames = new Array();
                var leaveValue = new Array();
                var leaveCount = new Array();

                var cmbLeave;
                for (j = 1; j < boxLength; j++) {
                    if (cmb[j].type == 'select-one' && cmb[j].id != _clientcmbUsers && cmb[j].id != _clientcmbAllLeaves && cmb[j].id != _clientcmbStaffGroup) {
                        cmbLeave = cmb[j];
                        break;
                    }
                }

                for (j = 0; j < cmbLeave.length; j++) {
                    if (cmbLeave[j].value != "0") {
                        leaveNames[j] = cmbLeave[j].text;
                        leaveValue[j] = cmbLeave[j].value;
                        leaveCount[j] = 0.0;
                    }
                }

                for (j = 1; j < boxLength; j++) {
                    if ((cmb[j].type == 'select-one' || cmb[j].type == 'checkbox') && cmb[j].id != _clientcmbUsers && cmb[j].id != _clientcmbAllLeaves && cmb[j].id != _clientcmbStaffGroup) {
                        if (cmb[j].value != 0) {
                            for (k = 1; k <= leaveNames.length; k++) {
                                if (leaveValue[k] == cmb[j].value) {
                                    if (cmb[j + 1].checked)
                                        leaveCount[k] = leaveCount[k] + 0.5;
                                    else
                                        leaveCount[k] = leaveCount[k] + 1.0;
                                }
                            }
                        }
                    }
                }

                var leaves = "";
                for (j = 0; j < cmbLeave.length; j++) {
                    if (cmbLeave[j].value != "0") {
                        if (leaveCount[j] != 0) {
                            if ((leaveCount[j] + "").indexOf(".") != -1)
                                leaves = leaves + ", " + leaveNames[j] + "(" + leaveCount[j] + ")";
                            else
                                leaves = leaves + ", " + leaveNames[j] + "(" + leaveCount[j] + ".0)";
                        }
                        else
                            leaves = leaves + ", " + leaveNames[j] + "(0.0)";
                    }
                }

                if (leaves.length > 2)
                    leaves = leaves.substring(2);
                $get(_clientlblUsedLeaves).innerHTML = leaves;
            }
        }

        function SelectAll() {
            var sIds = ''
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var allChecked = document.getElementById(_clientcmbAllLeaves).value;
            var totalChecked = 0;
            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'select-one' && checks[j].id != _clientcmbUsers && checks[j].id != _clientcmbStaffGroup) {
                    checks[j].value = allChecked;
                    checks[j].style.backgroundColor = checks[j].options[checks[j].selectedIndex].style.backgroundColor;
                }

                if (checks[j].type == 'checkbox') {
                    checks[j].checked = false;
                    if (checks[j].id.substring(0, checks[j].id.indexOf('_')) == "chk") {
                        if (allChecked == 0) {
                            checks[j].disabled = true;
                        }
                        else {
                            checks[j].disabled = false;
                        }

                        var day = checks[j].id.substring(checks[j].id.indexOf('_') + 1);
                        var lateMark = document.getElementById("lateMarkSpan" + day);
                        var halfLeavespan = document.getElementById("halfLeavespan" + day);
                        lateMark.style.backgroundColor = "transparent";
                        halfLeavespan.style.backgroundColor = "transparent";
                    }
                    else {
                        if (allChecked == 0) {
                            checks[j].disabled = false;
                        }
                        else
                            checks[j].disabled = true;
                    }
                }

                if (checks[j].type == 'span' && checks[j].id != _clientcmbUsers && checks[j].id != _clientcmbStaffGroup) {
                    checks[j].value = allChecked;
                    checks[j].style.backgroundColor = checks[j].options[checks[j].selectedIndex].style.backgroundColor;
                }
            }
        }

        function CalculateLeaves() {
            var sIds = ''
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var allChecked = false;
            var totalChecked = 0;
            var colorCode = '';
            $get(_clienthidDay).value = "";
            $get(_clienthidLeave).value = "";

            document.getElementById(_clientlblSuccessMessage).innerHTML = "";

            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'checkbox' && checks[j].id != 'chkAll' && checks[j].id.substring(0, checks[j].id.indexOf('_')) == "chk") {
                    if (checks[j].checked == true) {
                        totalChecked++;
                        sIds = sIds + checks[j].id.split('_')[1] + '$';
                    }
                }
            }

            var newVariable = "chkLateMark";
            var lateMarks = "";
            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'checkbox' && checks[j].id != 'chkAll' && checks[j].id.substring(0, checks[j].id.indexOf('_')) == newVariable) {
                    if (checks[j].checked == true) {
                        lateMarks = lateMarks + checks[j].id.split('_')[1] + '$';
                    }
                }
            }

            if (lateMarks.length > 0)
                lateMarks = lateMarks.substring(0, lateMarks.length - 1)
            $get(_clienthidLateMarkDays).value = lateMarks;

            sIds = sIds.substring(0, sIds.length - 1)
            $get(_clienthidHalfLeaves).value = sIds;
            sIds = ''
            isHolidayAttachedDate = false;
            allChecked = false;

            var holidayLeave = false;
            var holidayLateMark = false;

            var partialLeaves = document.getElementById(_clienthidPartialLeaves).value;
            var leaves = partialLeaves.split('$');

            var attachedLeaves = new Array();
            var kl = 0

            totalSelected = 0;
            var sLeaves = ''
            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'select-one' && checks[j].id != _clientcmbUsers && checks[j].id != _clientcmbAllLeaves && checks[j].id != _clientcmbStaffGroup) {
                    if (checks[j].value != 0) {
                        totalChecked++;
                        sIds = sIds + checks[j].id.split('_')[1] + '$';
                        sLeaves = sLeaves + checks[j].value + '$';
                        colorCode = colorCode + checks[j].options[checks[j].selectedIndex].style.backgroundColor + '$';

                        var isPartialLeave = false;
                        for (k = 0; k < leaves.length; k++) {
                            var partialLeave = leaves[k].split(',');
                            if (partialLeave[0] == checks[j].id.split('_')[1] && partialLeave[1] != "0")
                                isPartialLeave = true;
                        }

                        var isHalfLeave = document.getElementById('chk_' + checks[j].id.split('_')[1]);
                        if (isHalfLeave != null && (isHalfLeave.checked == false || (isHalfLeave.checked == true && isPartialLeave == true))) {
                            if (isHolidayAttachedDate == false) {
                                var dates = document.getElementById(_clienthidHolidayDates).value.split(',');
                                for (dt = 0; dt < dates.length; dt++) {
                                    if (dates[dt] == checks[j].id.split('_')[1]) {
                                        attachedLeaves[kl++] = dates[dt];                                        
                                        break;
                                    }
                                }
                            }
                        }

                        var holidays = document.getElementById(_clienthidHolidays).value.split('$');
                        for (dy = 0; dy < holidays.length; dy++) {
                            if (holidays[dy] == checks[j].id.split('_')[1]) {
                                holidayLeave = true;
                                break;
                            }
                        }
                    }
                }
                else if (checks[j].type == 'checkbox') {
                    var isLateMark = document.getElementById('chkLateMark_' + checks[j].id.split('_')[1]);
                    if (isLateMark != null && isLateMark.checked == true) {
                        var holidays = document.getElementById(_clienthidHolidays).value.split('$');
                        for (dy = 0; dy < holidays.length; dy++) {
                            if (holidays[dy] == checks[j].id.split('_')[1]) {
                                holidayLeave = true;
                                break;
                            }
                        }
                    }
                }
            }

            var sAttachedAndEnclosedDays = ''
			var isPreFound = false;
            
            var isAttached = false;
            var excludedLeaves = $get(_clienthidExcludedLeaves).value.split(',');
            if (attachedLeaves.length > 0) {
                var attLeaves = $get("<%=this.hidAttachedDates.ClientID %>").value;
                var lv = attLeaves.split(',');
                for (var k = 0; k < lv.length; k++) {
                    for (var j = 0; j < attachedLeaves.length; j++) {
                        if (lv[k] == attachedLeaves[j]) {

                            var isFound = false;
                            var tLeaves = sLeaves.substring(0, sLeaves.length - 1).split('$');
                            for (kt = 0; kt < excludedLeaves.length; kt++) {
                                if (tLeaves[j] == excludedLeaves[kt]) {
                                    isFound = true;
                                    break;
                                }
                            }

                            if (!isFound) {
                                isAttached = true;
                                sAttachedAndEnclosedDays = sAttachedAndEnclosedDays + "," + lv[k];
                                break;
                            }
                            else
                                break;
                        }
                    }
                }
            }

            var isEnclosed = false;
            var no = 0;
            if (attachedLeaves.length > 0) {
                var enclosedLeaves = $get("<%=this.hidEnclosedDates.ClientID %>").value;
                if (enclosedLeaves.match(',$') != null)
                    enclosedLeaves = enclosedLeaves + "#";

                var leavesArr = enclosedLeaves.split('$');

                var leaves = new Array();
                for (var k = 0; k < leavesArr.length; k++) {
                    if (leavesArr[k] != "")
                        leaves[no++] = leavesArr[k];
                }

                for (var k = 0; k < leaves.length; k++) {
                    var value = leaves[k].split(',');
                    for (var kk = 0; kk < attachedLeaves.length; kk++) {

                        var isFound = false;
                        var tLeaves = sLeaves.substring(0, sLeaves.length - 1).split('$');
                        for (kt = 0; kt < excludedLeaves.length; kt++) {
                            if (tLeaves[kk] == excludedLeaves[kt]) {
                                isFound = true;
                                break;
                            }
                        }

                        if (isFound)
                            continue;

                        if (value[0] == "#" && isPreFound == false) {
                            for (var jj = 0; jj < attachedLeaves.length; jj++) {
                                if (value[1] == attachedLeaves[jj]) {
                                    if ($get("<%=this.hidIsPreEnclosed.ClientID %>").value != "0") {
                                        isEnclosed = true;
                                        sAttachedAndEnclosedDays = sAttachedAndEnclosedDays + "," + value[1];
										isPreFound = true;
                                        //break;
                                    }
                                }
                            }
                        }
                        else {
                            if (value[0] == attachedLeaves[kk]) {
                                if (value[1] == "#") {
                                    for (var jj = 0; jj < attachedLeaves.length; jj++) {
                                        if (value[0] == attachedLeaves[jj]) {
                                            if ($get("<%=this.hidIsPostEnclosed.ClientID %>").value != "0") {
                                                isEnclosed = true;
                                                sAttachedAndEnclosedDays = sAttachedAndEnclosedDays + "," + value[0];
                                                //break;
                                            }
                                        }
                                    }
                                }
                                else {
                                    for (var kkk = 0; kkk < attachedLeaves.length; kkk++) {
                                        if (value[1] == attachedLeaves[kkk]) {
                                            isEnclosed = true;
                                            sAttachedAndEnclosedDays = sAttachedAndEnclosedDays + "," + value[0] + "," + value[1];
                                            //break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (sAttachedAndEnclosedDays.length > 0) {
                sAttachedAndEnclosedDays = sAttachedAndEnclosedDays.substring(1);
                $get("<%=this.hidAttEncConfig.ClientID %>").value = sAttachedAndEnclosedDays;
            }
            
            var leaveMessage = ''
            if (isEnclosed && isAttached)
                leaveMessage = " taken attached to holiday as well as enclosing holiday";
            else if (isAttached)
                leaveMessage = " taken attached to holiday"
            else if (isEnclosed)
                leaveMessage = " enclosing holiday";

            if ($get(_clientchkDeductHolidayLeaves) != null && $get(_clientchkDeductHolidayLeaves).checked == true && leaveMessage == "") {
                $get(_clientchkDeductHolidayLeaves).checked = false;
                document.getElementById(_clienthidDeductHolidayLeaves).value = "N";
            }
       
            if ((leaveMessage != "" && $get(_clientchkDeductHolidayLeaves) != null && $get(_clientchkDeductHolidayLeaves).checked == false) || (leaveMessage != "" && $get(_clientchkDeductHolidayLeaves) == null)) {
                if (confirm('Leaves(s) are' + leaveMessage + ', salary amount will be deducted as per Staff Holidays Leave Deduction configuration, do you want to deduct salary?')) {
                    if ($get(_clientchkDeductHolidayLeaves) != null)
                        $get(_clientchkDeductHolidayLeaves).checked = true;
                    document.getElementById(_clienthidDeductHolidayLeaves).value = "Y";
                }
                else
                    document.getElementById(_clienthidDeductHolidayLeaves).value = "N";
            }

            if ($get(_clientchkDeductHolidayLeaves) != null && $get(_clientchkDeductHolidayLeaves).checked == true)
                document.getElementById(_clienthidDeductHolidayLeaves).value = "Y";

            if (leaveMessage == "") {
                if ($get(_clientchkDeductHolidayLeaves) != null)
                    $get(_clientchkDeductHolidayLeaves).checked = false;
                document.getElementById(_clienthidDeductHolidayLeaves).value = "N";
            }

            if (document.getElementById(_clienthidDeductHolidayLeaves).value != "N") {
                if ((holidayLeave && isHolidayAttachedDate && $get(_clientchkDeductHolidayLeaves) != null && $get(_clientchkDeductHolidayLeaves).checked == true) || (holidayLeave && isHolidayAttachedDate && $get(_clientchkDeductHolidayLeaves) == null)) {
                    alert("If the Leave or Late Mark are set for the holiday then attached leave deduction is not allowed.");
                    if ($get(_clientchkDeductHolidayLeaves) != null)
                        $get(_clientchkDeductHolidayLeaves).checked = false;
                    document.getElementById(_clienthidDeductHolidayLeaves).value = "N";
                    return false;
                }
            }

            sIds = sIds.substring(0, sIds.length - 1)
            sLeaves = sLeaves.substring(0, sLeaves.length - 1)
            $get(_clienthidDay).value = sIds;
            $get(_clienthidLeave).value = sLeaves;
            $get(_clienthidColorCodes).value = colorCode;
            bResult = true;
            if (sIds == "") {
                bResult = confirm('Are you sure you want to delete all the leaves(if any exists)?')
            }

            if (bResult) {
                UpdateLateMark(0);
                var maxLateMarkCount = document.getElementById(_clienthidMaxLateMarkCount).value;
                var consideredLeaves = document.getElementById(_clienthidConsideredLeaves).value;
                var lateMarkCount = document.getElementById(_clienthidLateMarkCount).value;

                if (maxLateMarkCount != 0 && parseInt(lateMarkCount) >= parseInt(maxLateMarkCount)) {
                    bResult = confirm("Late mark count is greater than configured late mark count, so leave(s) will be deducted as per late mark configuration, do you want to continue?")
                    if (bResult)
                        $get(_clientlblLateMark).innerHTML = "Late Mark(s) : " + lateMarkCount;
                }
            }
            return bResult;
        }

        function isHolidayAttachedDate(day) {
            var dates = document.getElementById(_clienthidHolidayDates).value.split(',');
            for (j = 1; j < dates.length; j++) {
                if (dates[j] == day)
                    return true;
            }
            return false;
        }

        function CheckLateMark(lateMark, rowIndex) {
            var chk = document.getElementById("chkLateMark_" + rowIndex);
            if (chk.checked)
                lateMark.style.backgroundColor = "#01A5EC";
            else
                lateMark.style.backgroundColor = "transparent";

            var partialLEave = document.getElementById("lblpartialleave" + rowIndex);
            if (chk.checked) {
                if (partialLEave != null)
                    partialLEave.innerHTML = ''

                var partialLeaves = document.getElementById(_clienthidPartialLeaves).value;
                var leaves = partialLeaves.split('$');

                var pLeaves = ''

                var isPartialLeave = false;
                for (k = 0; k < leaves.length; k++) {
                    var partialLeave = leaves[k].split(',');
                    if (partialLeave[0] == rowIndex)
                        partialLeave[1] = "0";
                    pLeaves = pLeaves + '$' + partialLeave[0] + ',' + partialLeave[1];
                }

                document.getElementById(_clienthidPartialLeaves).value = pLeaves;
            }
        }

        function CheckHalfLeave(halfLeave, rowIndex) {
            var chk = document.getElementById("chk_" + rowIndex);
            var lateMark = document.getElementById("lateMarkSpan" + rowIndex);
            var partialLEave = document.getElementById("lblpartialleave" + rowIndex);
            if (chk.checked)
                halfLeave.style.backgroundColor = "#009B9B";
            else {
                halfLeave.style.backgroundColor = "transparent";
                lateMark.style.backgroundColor = "transparent";
                if (partialLEave != null)
                    partialLEave.innerHTML = ''

                var partialLeaves = document.getElementById(_clienthidPartialLeaves).value;
                var leaves = partialLeaves.split('$');

                var pLeaves = ''

                var isPartialLeave = false;
                for (k = 0; k < leaves.length; k++) {
                    var partialLeave = leaves[k].split(',');
                    if (partialLeave[0] == rowIndex)
                        partialLeave[1] = "0";
                    pLeaves = pLeaves + '$' + partialLeave[0] + ',' + partialLeave[1];
                }
                document.getElementById(_clienthidPartialLeaves).value = pLeaves;
            }
        }

        function Test2() {
            document.getElementById().value = "Test";
        }

        function ShowPopup() {
            var x, y, tt_ovr_
            var width = 250
            var height = 110
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            // Override the z-index of the topmost wz_dragdrop.js D&D item
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
            cssstyle.visibility = "visible";
            cssstyle.display = "block";
        }

        function OpenODDetailsPopup() {
            _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('ODDetailsPopup.aspx?' + sEncryptedString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650')
            return false;
        }

        function ShowInfo() {
            $('#divPopup').show(500);
            $('#divPopup').center();
        }

        function HideUserInfo() {
            $('#divPopup').hide(500);
        }

        function UpdateBioData() {
            var str = $('[id$=hidBioData]').val()
            window.open(str, '_blank', 'scrollbars=no,resizable=no,top=100,left=100,width=400,height=150')
        }

    </script>
    <script language="javascript" type="text/javascript">
       $(document).ready(function () {
           AutoSearch();
       });

       function AutoSearch() {
           _slienttxtUserName = '#<%=txtName.ClientID%>';
           var SchoolId = "<%=miSchoolId %>";
           var AcademicYearId = "<%=miAcademicYearId %>"
           BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 0);
       }

       function SearchSelectedValue(val) {
           txt = document.getElementById("<%=this.txtName.ClientID %>");
           bt = document.getElementById("<%=this.btnSearch.ClientID %>");
           SearchResult(txt, val, bt);
       }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
