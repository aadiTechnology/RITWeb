    <%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SchoolwiseAttendanceDetails.aspx.cs" Inherits="SchoolwiseAttendanceDetails " ViewStateMode="Disabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="EventCalendar" Namespace="ExtendedControls" TagPrefix="Calender" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div style="padding-left: 10px;">
        <table border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr align="center" id="tdlbl" runat="server" viewstatemode="Enabled">
                <td style="padding-bottom: 5px" colspan="3" align="left">
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled" ShowMessageBox="true" ShowSummary="true"
                        CssClass="ClsLabel" />
                </td>
            </tr>
            <tr>
                <td align="right" colspan="3">
                    <span class="ClsMdtStar"><span class="ClsMdtStar">*</span>
                        <asp:Label ID="Label36" runat="server" CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"
                            EnableViewState="false"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr runat="server" viewstatemode="Enabled" id="trAttendenceDetails">
                <td align="left" valign="top" style="width: 60%;vertical-align:top;">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" style="width: 60%">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="center" class="ClsTextNormal">
                                                                <asp:Label ID="lblUpdateSuccess" runat="server" EnableViewState="false" Font-Bold="true"
                                                                    ForeColor="Blue"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="tdErrMsg" visible="false" enableviewstate="false">
                                                            <td align="center" runat="server" id="tdlblErrorMsg" class="ClsHilightBGB" style="height: 30px;">
                                                                <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True" ViewStateMode="Enabled"
                                                                    CssClass="ClsHilightErrorB"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAcademicYr" runat="server" visible="false" enableviewstate="false">
                                                            <td align="center" runat="server" id="td2" class="ClsHilightBGB" style="height: 30px;">
                                                                <asp:Label ID="lblAcademicYrErrorMsg" runat="server" Font-Bold="True" EnableViewState="false"
                                                                    CssClass="ClsHilightErrorB"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%">
                                <asp:UpdatePanel ID="upnlTotalStudents" runat="server" >
                                    <ContentTemplate>
                                        <table align="center" width="550px">
                                            <tr>
                                                <td class="AttendanceTD" align="left">
                                                    <span class="LblNrmlB" style="font-weight: bold">
                                                        <asp:Label ID="lblPresentStudents" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Presentstudents  %>"></asp:Label>
                                                        <span>/</span>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblTotalstudents" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Totalstudents  %>"></asp:Label>
                                                        <span id="Span2" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left" class="ClsHilightFeeL" style="width: 150px; padding-top: 11px" valign="middle">
                                                    <asp:LinkButton ID="lnkTotalStudents" runat="server" ViewStateMode="Enabled" Height="30px" ToolTip="<%$ Resources:LocalizedResources, AttendanceStatus %>">                                                                                                       
                                                    </asp:LinkButton>
                                                </td>
                                                <td class="AttendanceTD" style="width: 600px" align="left">
                                                    <span class="LblNrmlB" style="font-weight: bold">
                                                        <asp:Label ID="lblAttendanceMarkedClasses" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, AttendanceMarkedClasses  %>"></asp:Label>
                                                        <span>/</span>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblstudent" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, TotalClasses  %>"></asp:Label><span
                                                            id="Span4" class="colonPadding">:</span> </span>
                                                </td>
                                                <td class="ClsHilightFeeL" style="width: 150px; vertical-align: middle; padding-top: 10px">
                                                    <asp:LinkButton ID="lnkAttendanceStatus" runat="server" ViewStateMode="Enabled" Height="30px" ToolTip="<%$ Resources:LocalizedResources, AttendanceStatus %>">>                                                                                                       
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trMontwiseAttendanceNote" runat="server">
                            <td style="text-align:center; margin:0px auto;" align="center">                                 
                               <table align="center" style="text-align:center; margin:0px auto;">                                    
                                   <tr>
                                       <td align="left" class="ClsBorderlight " style="background-color: #ffffc4; padding: 3px;
                                           width: 17%">
                                           <span class="LblNrmlB" style="font-weight: bold; height: 16px;"><b>Note :</b></span>
                                       </td>
                                       <td align="left" class="ClsBorderlight" style="padding: 3px; width: 80%">
                                           <div id="div" style="font-family: Verdana; font-size: 8pt; border: 100%;">
                                               Attendance will not be marked for the future dates.
                                           </div>
                                       </td>
                                   </tr>                                   
                               </table>                                        
                            </td>
                        </tr>
                        <tr id="trStudentGrid" runat="server">
                            <td align="center" valign="top">
                                <asp:Panel ID="pnlFields" runat="server" ViewStateMode="Enabled" Width="100%">
                                    <table align="center" width="100%">
                                        <tr id="trButton" runat="server" viewstatemode="Enabled" >
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSaveUp" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save %>"
                                                            OnClick="btnSave_Click" disable-page="true" />
                                                        <asp:Button ID="btnMarkMonthwiseAttendance" runat="server" ViewStateMode="Enabled" 
                                                            CssClass="ClsBtn" Text="Mark Monthwise Attendance" disable-page="true" />
                                                        <asp:Button ID="btnDelete" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Delete %>"
                                                            Visible="false" OnClick="btnDelete_Click" />                                                        
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr align="center" id="trIndiviAttendance" runat="server" viewstatemode="Enabled">
                                            <td align="center">
                                                <table align="center">
                                                    <tr>
                                                        <td align="center" colspan="4">
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <span id="hlnkIndiviAttendance" style="cursor: pointer;" runat="server" viewstatemode="Enabled" class="ToprLinkHlilight LblNrmlB ClsPaddingGen">
                                                                        <u>
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, IndividualAttendance %>"></asp:Label></u>
                                                                    </span>&nbsp; <span id="hlnkIdentity" style="cursor: pointer;" runat="server" viewstatemode="Enabled" class="ToprLinkHlilight LblNrmlB ClsPaddingGen">
                                                                        <u>
                                                                            <asp:Label ID="lblIndividualAttendance" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MonthWiseAttendance %>"></asp:Label></u>
                                                                    </span>
                                                                    &nbsp; <span id="hlnkAbsentStudents" style="cursor: pointer;" visible="false" runat="server" viewstatemode="Enabled" class="ToprLinkHlilight LblNrmlB ClsPaddingGen">
                                                                        <u>
                                                                            <asp:Label ID="lblabsentStudents" runat="server" ViewStateMode="Enabled" Text="Absent Student Details"></asp:Label></u>
                                                                    </span>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%">
                                                        <td id="tdclsTeacher" class="ClsBorderlight" runat="server">
                                                            <asp:Label ID="lblTeacher" runat="server" CssClass="ClsLabel colonPadding" Text="<%$ Resources:LocalizedResources, ClassTeacher %>"
                                                                EnableViewState="False"></asp:Label>
                                                            <span id="Span3" class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td id="tddropdwn" runat="server" style="width: 50%">
                                                            <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                                OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" Width="100%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Date %>"
                                                                EnableViewState="false"></asp:Label>
                                                            <span id="Span1" class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="calTodaysDate" CssClass="SmlCombo" runat="server" ViewStateMode="Enabled" AutoPostBack="True"
                                                                        MaxLength="11"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="cAttendDate" runat="server" ViewStateMode="Enabled" Control="calTodaysDate" Format="dd MMM yyyy"
                                                                        ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg %>" Culture = "en"
                                                                        OnSelectionChanged="calTodaysDate_DateChanged" AutoPostBack="True" To-Today="true" />
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:RequiredFieldValidator ID="reqForEndDate" runat="server" ViewStateMode="Enabled" ControlToValidate="calTodaysDate"
                                                                        ErrorMessage="Date should not be blank." SetFocusOnError="True" Display="None">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </asp:RequiredFieldValidator>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" runat="server" viewstatemode="Enabled" id="trgrdStudent" style="width: 100%">
                                            <td align="center" valign="top">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <div id="DivgrdStudent" runat="server" style="width: 586px;">
                                                            <asp:GridView CssClass="GridBorder" AllowSorting="True" ID="grdStudentAttendanceManagement"
                                                                runat="server" ViewStateMode="Enabled" AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" DataKeyNames="Student_Id,Attendance_Date,SchoolWise_Attendance_Id,isApplicable,Joining_Date"
                                                                PageSize="2000" GridLines="None" ForeColor="#333333" Width="100%" OnSorting="grdAttendance_Sorting"
                                                                OnRowCreated="grdAttendance_RowCreated">
                                                                <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText %>"
                                                                    PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>"
                                                                    Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                <Columns>
                                                                    <asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources, RollNo %>">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="100px" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Width="50px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FullName" HeaderText="<%$ Resources:LocalizedResources, StudentName %>">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <input id="chkAttendance" runat="server" viewstatemode="Enabled" type="checkbox" onclick="CheckAllOrUncheckAllAttendance(document,_clientGridId,this,'chkPresentOrAbsent')">
                                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, IsPresent %>"></asp:Label>
                                                                            </input>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkPresentOrAbsent" Checked='<%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Is_Present")))%>'
                                                                                Enabled='<%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"isApplicable")))%>'
                                                                                runat="server" ViewStateMode="Enabled" onclick="MarkAbsentOrPresent(this);" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px" />
                                                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px"/>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                       <HeaderTemplate>
                                                                        <input id="ChkAllHalfDayPresent" type="checkbox" runat="server" viewstatemode="Enabled" style="margin-left: 2px" onclick="javascript:SelectheaderCheckboxes(this)" >
                                                                        <asp:Label ID="lblChkHalfDayAttendance" runat="server" ViewStateMode="Enabled" Text = "Is Half Day Present?"></asp:Label></input>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkBoxHalfDayPresent" runat="server" ViewStateMode="Enabled" Checked='<%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Is_HalfDayPresent")))%>'
                                                                                Enabled='<%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Is_Present")))%>' />
                                                                    </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                                                                         <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="170px"/>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerStyle CssClass="ClsNwGridPaging" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False"
                                                                    ForeColor="Black" Font-Names="Arial" Font-Size="Small" />
                                                                <RowStyle CssClass="ClsGridRow" />
                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                            </asp:GridView>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                           <td >
                                                <asp:CheckBox ID="chkboxSave" runat="server" Text=" Send Message to Absent Student(s)" CssClass="LblNrmlB" />
                                           </td>
                                        </tr>
                                        <tr align="center" id="trbtnCancel" runat="server" >
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" >
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSave" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save %>"
                                                            OnClick="btnSave_Click" disable-page="true" />
                                                        <asp:Button ID="btnMonthwise" runat="server" ViewStateMode="Enabled" 
                                                            CssClass="ClsBtn" Text="Mark Monthwise Attendance" disable-page="true" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding-top: 5px" valign="top">
                </td>
                <td valign="top" style="width: 42%;" class="td-vertical-align-top">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0" id="tblAttendenceCalender"
                        runat="server" >
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" >
                                    <ContentTemplate>
                                        <Calender:EventCalendar ID="AttendanceCalendar" runat="server" ViewStateMode="Enabled" BackColor="White"
                                            BorderColor="Silver" CellPadding="0" EventBackColorName="" EventDescriptionColumnName=""
                                            EventEndDateColumnName="" EventForeColorName="" EventHeaderColumnName="" EventStartDateColumnName=""
                                            Font-Names="Arial" Font-Size="8pt" ForeColor="Black" NextPrevFormat="ShortMonth"
                                            ShowDescriptionAsToolTip="True" ShowGridLines="True" Width="99%" OnVisibleMonthChanged="AttendanceCalendar_VisibleMonthChanged"
                                            Font-Bold="False" SelectionMode="Day" OnSelectionChanged="AttendanceCalendar_SelectionChanged">
                                            <SelectedDayStyle BackColor="#E7E7E7" Font-Bold="True" ForeColor="Black" BorderColor="LightSteelBlue"
                                                BorderStyle="Solid" BorderWidth="1px" Height="45px" Width="45px" />
                                            <SelectorStyle BackColor="#99CCCC" Height="45px" Width="45px" ForeColor="#336666" />
                                            <OtherMonthDayStyle ForeColor="#999999" Height="45px" Width="45px" />
                                            <NextPrevStyle Font-Size="8pt" ForeColor="Navy" />
                                            <DayHeaderStyle ForeColor="White" Height="25px" CssClass="DayHeader" />
                                            <TitleStyle Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="20px" BorderStyle="None"
                                                CssClass="MonthHeader" />
                                            <DayStyle Height="45px" Width="45px" />
                                        </Calender:EventCalendar>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" >
                                    <ContentTemplate>
                                        <table runat="server" viewstatemode="Enabled" id="tblPresentGrids" visible="false"
                                            width="100%">
                                            <tr>
                                                <td align="left" valign="top" class="ClsBorderlight">
                                                    <span class="lblGreenB">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, PresentStudents %>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:GridView CssClass="GridBorder" AllowSorting="false" ID="grdPresent" runat="server" ViewStateMode="Enabled"
                                                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" PageSize="2000" GridLines="None"
                                                        ForeColor="#333333" Width="100%">
                                                        <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText %>"
                                                            PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>"
                                                            Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField DataField="Boys" HeaderText="<%$ Resources:LocalizedResources, Boys %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Girls" HeaderText="<%$ Resources:LocalizedResources, Girls %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Total" HeaderText="<%$ Resources:LocalizedResources, Total %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="ClsBorderlight">
                                                    <span class="ClsHilightErrorB">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, AbsentStudents %>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:GridView CssClass="GridBorder" AllowSorting="false" ID="grdAbsent" runat="server" ViewStateMode="Enabled"
                                                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" PageSize="2000" GridLines="None"
                                                        ForeColor="#333333" Width="100%">
                                                        <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText %>"
                                                            PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>"
                                                            Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField DataField="Boys" HeaderText="<%$ Resources:LocalizedResources, Boys %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Girls" HeaderText="<%$ Resources:LocalizedResources, Girls %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Total" HeaderText="<%$ Resources:LocalizedResources, Total %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="ClsBorderlight">
                                                    <span class="lblBlkB">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, TotalStudents %>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:GridView CssClass="GridBorder" AllowSorting="false" ID="grdTotalPresent" runat="server" ViewStateMode="Enabled"
                                                        AutoGenerateColumns="False" CellPadding="0" CellSpacing="1" PageSize="2000" GridLines="None"
                                                        ForeColor="#333333" Width="100%">
                                                        <PagerSettings NextPageText="<%$ Resources:LocalizedResources, NextPageText %>" LastPageText="<%$ Resources:LocalizedResources, LastPageText %>"
                                                            PreviousPageText="<%$ Resources:LocalizedResources, PreviousPageText %>" FirstPageText="<%$ Resources:LocalizedResources, FirstPageText %>"
                                                            Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField DataField="Boys" HeaderText="<%$ Resources:LocalizedResources, Boys %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Girls" HeaderText="<%$ Resources:LocalizedResources, Girls %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Total" HeaderText="<%$ Resources:LocalizedResources, Total %>">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="80px" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Width="80px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="ClsBorderlight">
                                                    <span class="lblBlkB">
                                                        <asp:Label ID="Label8" runat="server" Text="Present Students Average of Selected Month (In %)"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:ListView ID="lstvwAverageDetails" runat="server" ViewStateMode="Enabled">
                                                    <LayoutTemplate>
                                                        <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblBoysHeader" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Boys %>" CssClass="clsLabel"></asp:Label>
                                                                </th>
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, Girls %>" CssClass="clsLabel"></asp:Label>
                                                                </th>
                                                                <th align="left" class="ClspaddingL">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Total %>" CssClass="clsLabel"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblTotalBoys" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("PresentBoys") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblGirls" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("PresentGirls") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblTotal" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("Total") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblTotalBoys" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("PresentBoys") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblGirls" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("PresentGirls") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblTotal" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text='<%#Eval("Total") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                               <asp:Label ID="lblNoRecFound" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" viewstatemode="Enabled" id="divErr">
                    </div>
                </td>
            </tr>
        </table>
         <div id="divConfirmation" runat="server" viewstatemode="Enabled" style="position: fixed;
                            display: none; margin: 0px; padding: 0px; width: 400px; height: 100px; border-width: 0px;
                            left: 500px; top: 400px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 100px 00px;
                            background-color: white; z-index: 499;">
                            <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; color: Black; width: 390px; text-align: right;">                               
                                <span style="cursor: hand" onclick="javascript:HideConfirmationPopup();">
                                    <img alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                                <div style="margin: 10px auto; text-align: center;" align="center">
            <div style="height:20px;"><span class="ClsLabel">Do you want to overwrite existing attendance?</span></div>
            <div style="margin:5px auto;width:320px;">
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ClsBtn" OnClientClick="SendNotification()" OnClick="btnYes_Click" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ClsBtn" OnClientClick="CancelNotification()" OnClick="btnNo_Click" />
                <asp:Button ID="btnCancelOp" runat="server" Text="Cancel" CssClass="ClsBtn" />
            </div>
            </div>
            </div>
        </div>
        <asp:HiddenField ID="hidSendNotification" runat="server" Value="Y" />
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
            <ContentTemplate>
                <asp:HiddenField ID="hidYearEndDate" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidStdDivId" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidYearStartDate" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidSortExpression" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidSortDirection" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidIsNonWorking" runat="server" viewstatemode="Enabled" Value="" />
                <asp:HiddenField ID="hidCanEdit" runat="server" viewstatemode="Enabled" Value="" />
                <asp:HiddenField ID="hidGridItemCount" runat="server" viewstatemode="Enabled" Value="0" />
                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" viewstatemode="Enabled" Value="False" />
                <asp:HiddenField ID="hidAllStudentsMarkedAsPresent" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidAllStudentsMarkedAsAbsent" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidAreYouSureYouWantToSaveTheAttendance" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidAreYouSureYouWantToDeleteAttendanceOfDate" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidDateShouldNotBeBlank" runat="server" viewstatemode="Enabled" />
                <asp:HiddenField ID="hidReturnValue" runat="server" viewstatemode="Enabled" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">

        _clientDate = "<%=this.calTodaysDate.ClientID %>";
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
        _clientcmbTeachers = "<%=this.cmbTeachers.ClientID %>";
        _clientGridId = "<%=this.grdStudentAttendanceManagement.ClientID %>";
        _clientSaveId = "<%=this.btnSave.ClientID %>";
        _clientSaveUpId = "<%=this.btnSaveUp.ClientID %>";
        _clienthidIsNonWorkingId = "<%=this.hidIsNonWorking.ClientID %>";
        _clientGridItemCount = "<%=this.hidGridItemCount.ClientID %>";
        _clientDeleteId = "<%=this.btnDelete.ClientID %>";




        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginReqHandler);
        prm.add_endRequest(EndReqHandler);

        var Page_IsValid = true;
        function IsAllStudentPresentOrAbsent(args) {
            Page_IsValid = true;
            var bResult;
            var bNonWorkingResult = true;
            if (args) {
                var str;
                str = window.location.href;
                var iIndex = str.lastIndexOf("/#");
                if (iIndex != -1)
                    str = str.substr(0, iIndex)

                str = str + "/#top";

                if (document.all && navigator.appVersion.indexOf('MSIE 7') == 0)
                    window.navigate(str);
            }
            if (document.getElementById(_clienthidIsNonWorkingId).value != "") {
                bNonWorkingResult = window.confirm(document.getElementById(_clienthidIsNonWorkingId).value + document.getElementById("<%=this.hidAreYouSureYouWantToSaveTheAttendance.ClientID %>").value);
                if (!bNonWorkingResult) {
                    Page_IsValid = false;
                    return bNonWorkingResult;
                }
            }
            var iLoopCounter;
            var grdStudentAttendance = document.getElementById(_clientGridId);
            if (grdStudentAttendance) {
                var iRowcount = grdStudentAttendance.rows.length;
                var LateJoining = 0;
                var ichkCount = 0;
                var ilength = 256;
                iRowcount = iRowcount + 1;
                var isApplicable = false;
                for (iLoopCounter = 2; iLoopCounter < iRowcount; iLoopCounter++) {
                    var chkSelectOrDeselect;
                    if (iLoopCounter < 10) {
                        chkSelectOrDeselect = document.getElementById(_clientGridId + "_ctl0" + iLoopCounter + "_chkPresentOrAbsent");

                    }
                    else {
                        chkSelectOrDeselect = document.getElementById(_clientGridId + "_ctl" + iLoopCounter + "_chkPresentOrAbsent");
                    }
                    if (chkSelectOrDeselect != null) {
                        if (chkSelectOrDeselect.checked == true) {
                            ichkCount++;
                        }
                        else {

                        }
                    } else {
                        LateJoining = LateJoining + 1;
                    }
                }

                if (document.getElementById(_clientDate).value == '') {
                    window.alert(document.getElementById("<%=this.hidDateShouldNotBeBlank.ClientID %>").value);
                }
                else if (ichkCount == iRowcount - LateJoining - 2)
                    bResult = window.confirm(document.getElementById("<%=this.hidAllStudentsMarkedAsPresent.ClientID %>").value);
                else if (ichkCount == 0)
                    bResult = window.confirm(document.getElementById("<%=this.hidAllStudentsMarkedAsAbsent.ClientID %>").value);
                if (!bResult)
                    Page_IsValid = false;

            }
            else
                bResult = true;

            return bResult;
        }

        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement && (postBackElement.id == _clientSaveId || postBackElement.id == _clientSaveUpId || postBackElement.id == _clientDeleteId)) {
                DisableButtons(true);
            }
        }

        function ConfirmDelete(sDate) {
            if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteAttendanceOfDate.ClientID %>").value + sDate))
                return false;
            return true;
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement && (postBackElement.id == _clientSaveId || postBackElement.id == _clientSaveUpId || postBackElement.id == _clientDeleteId))
                DisableButtons(false);

            var chk = document.getElementById(_clientGridId + "_ctl01_chkAttendance");
            if (chk != null) {
                    if ($("[id$=chkPresentOrAbsent]").length == $("[id$=chkPresentOrAbsent]:checked").length)
                        $("[id$=chkAttendance]").attr('checked', "checked");
                    else $("[id$=chkAttendance]").removeAttr("checked");
            }
        }

        document.ready(new function () {
            if ($("[id$=chkPresentOrAbsent]").length == $("[id$=chkPresentOrAbsent]:checked").length) {
                $("[id$=chkAttendance]").attr('checked', "checked");
            }
            else $("[id$=chkAttendance]").removeAttr("checked");
        })

        function DisableButtons(action) {
            var isPageValid = true;
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate();

            if (isPageValid) {
                if (document.getElementById(_clientSaveId) != null)
                    document.getElementById(_clientSaveId).disabled = action;
                if (document.getElementById(_clientSaveUpId) != null)
                    document.getElementById(_clientSaveUpId).disabled = action;
                if (document.getElementById(_clientDeleteId) != null)
                    document.getElementById(_clientDeleteId).disabled = action;
            }
        }
        function ShowIdentities(sQryStr) {
            _sClienthhlnkIdentity = "<%=this.hlnkIdentity.ClientID %>";
            if ((document.getElementById(_sClienthhlnkIdentity) == null) || (document.getElementById(_sClienthhlnkIdentity) == "") || (document.getElementById(_sClienthhlnkIdentity).disabled))
                return false;

            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=yes,top=0,left=0,width=1200,height=600').focus();
            return false;
        }

        //this is to select or unselect the datagrid check boxes
        function CheckAllOrUncheckAllAttendance(oDocument, grdid, obj, objlist, iPageCnt) {
            //this function decides whether to check or uncheck all         
            SelectAllCheckBoxOfGrid(obj);

            if (obj.checked) {
                $("#" + _clientGridId + " tr:even").removeClass('ClsGridAltRowHighLight');
                $("#" + _clientGridId + " tr:odd").removeClass('ClsGridRowHighLight');
                $("#" + _clientGridId + " tr:odd").addClass('ClsGridRow');
                $("#" + _clientGridId + " tr:even").addClass('ClsGridAltRow');
                $("#" + _clientGridId + " tr:nth(0)").removeClass('ClsGridAltRowHighLight');
            }

            else {
                $("#" + _clientGridId + " tr:even").removeClass('ClsGridRow');
                $("#" + _clientGridId + " tr:odd").removeClass('ClsGridAltRow');
                $("#" + _clientGridId + " tr:even").addClass('ClsGridAltRowHighLight');
                $("#" + _clientGridId + " tr:odd").addClass('ClsGridRowHighLight');
                $("#" + _clientGridId + " tr:nth(0)").removeClass('ClsGridAltRowHighLight');
            }

            $("#" + _clientGridId + " tr:nth(0)").removeClass('ClsGridAltRow');
        }

        function SelectAllCheckBoxOfGrid(obj) {
            //$($(sGridName) + 'input:checkbox').attr('checked', obj.checked);
            var gvcheck = document.getElementById(_clientGridId);
            var inputs;
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (obj.checked) {
                for (i = 2; i <= gvcheck.rows.length; i++) {
                    if (i < 10) {
                        inputs = document.getElementById(_clientGridId + "_ctl0" + i + "_chkPresentOrAbsent");
                        inputs1 = document.getElementById(_clientGridId + "_ctl0" + i + "_ChkBoxHalfDayPresent");
                    }
                    else {
                        inputs = document.getElementById(_clientGridId + "_ctl" + i + "_chkPresentOrAbsent");
                        inputs1 = document.getElementById(_clientGridId + "_ctl" + i + "_ChkBoxHalfDayPresent");
                    }

                    if (inputs != null)
                        inputs.checked = true;

                    if (inputs1 != null)
                        inputs1.disabled = false;
                }
            }
            else {
                chk = document.getElementById(_clientGridId + "_ctl0" + 1 + "_ChkAllHalfDayPresent");

                if(chk != null)
                    chk.checked = false;

                for (i = 2; i <= gvcheck.rows.length; i++) {
                    if (i < 10) {
                        inputs = document.getElementById(_clientGridId + "_ctl0" + i + "_chkPresentOrAbsent");
                        inputs1 = document.getElementById(_clientGridId + "_ctl0" + i + "_ChkBoxHalfDayPresent");
                    }
                    else {
                        inputs = document.getElementById(_clientGridId + "_ctl" + i + "_chkPresentOrAbsent");
                        inputs1 = document.getElementById(_clientGridId + "_ctl" + i + "_ChkBoxHalfDayPresent");
                    }

                    if (inputs != null)
                        inputs.checked = false;

                    if (inputs1 != null) {
                        inputs1.disabled = true;
                        inputs1.checked = false;
                    }
                }
            } 

        }

        function OpenPopUp(sQryStr) {
            window.open('../Teacher/AttendanceStatusUI.aspx?' + sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=630,location=no').focus();
            return false;
        }

        function MarkAbsentOrPresent(obj) {
            var rowIndex = $(obj).closest('tr')[0].rowIndex;
            RowIndexForEnableorDisable = rowIndex + 1;
            if(rowIndex < 10)
                inputs = document.getElementById(_clientGridId + "_ctl0" + RowIndexForEnableorDisable + "_ChkBoxHalfDayPresent");
            else
                inputs = document.getElementById(_clientGridId + "_ctl" + RowIndexForEnableorDisable + "_ChkBoxHalfDayPresent");
            if (obj.checked) {
                if (inputs != null)
                    inputs.disabled = false;

                if (parseInt(rowIndex) % 2 == 1) {
                    $(obj).closest('tr').removeClass('ClsGridRowHighLight');
                    $(obj).closest('tr').addClass('ClsGridRow');
                }
                else {
                    $(obj).closest('tr').removeClass('ClsGridAltRowHighLight');
                    $(obj).closest('tr').addClass('ClsGridAltRow');
                }
            }
            else {
                if (inputs != null) {
                    inputs.disabled = true;
                    inputs.checked = false;
                }
                if (parseInt(rowIndex) % 2 == 1) {
                    $(obj).closest('tr').removeClass('ClsGridAltRow');
                    $(obj).closest('tr').addClass('ClsGridRowHighLight');

                }
                else {
                    $(obj).closest('tr').removeClass('ClsGridRow');
                    $(obj).closest('tr').addClass('ClsGridAltRowHighLight');
                }
            }

             if ($("[id$=chkPresentOrAbsent]").length == $("[id$=chkPresentOrAbsent]:checked").length)
                $("[id$=chkAttendance]").attr('checked', "checked");
             else 
                $("[id$=chkAttendance]").removeAttr("checked");
        }

        function OpenPopupWindow(sQryStr) {
            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600').focus();
            return false;
        }

        function SelectheaderCheckboxes(headerchk) {            
            var gvcheck = document.getElementById(_clientGridId);
            var inputs;
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 2; i <= gvcheck.rows.length; i++) {
                    if (i < 10)
                        inputs = document.getElementById(_clientGridId + "_ctl0" + i + "_ChkBoxHalfDayPresent");
                    else
                        inputs = document.getElementById(_clientGridId + "_ctl" + i + "_ChkBoxHalfDayPresent");
                    if (inputs != null && inputs.disabled == false)
                        inputs.checked = true;
                }
            }
            else {
                for (i = 2; i <= gvcheck.rows.length; i++) {
                    if (i < 10)
                        inputs = document.getElementById(_clientGridId + "_ctl0" + i + "_ChkBoxHalfDayPresent");
                    else
                        inputs = document.getElementById(_clientGridId + "_ctl" + i + "_ChkBoxHalfDayPresent");

                    if (inputs != null)
                        inputs.checked = false;
                }
            }
        }        
        </script>
       <script language="javascript" type="text/javascript">

        _clientdivConfirmation = "<%=this.divConfirmation.ClientID %>";
        _clienthidSendNotification = "<%=this.hidSendNotification.ClientID %>"
        function OpenConfirmationPopup() {            
            $('#' + _clientdivConfirmation).fadeIn(700);
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divConfirmation.ClientID %>").style
            var width = 600
            var height = 120
            var left = parseInt((screen.width / 2) - (width / 2.3)) - 100
            var top = parseInt((screen.height / 2) - (height / 2)) - 70
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
        }

        function HideConfirmationPopup() {
            $('#' + _clientdivConfirmation).fadeOut(700);
        }

        function SendNotification() {
            $get(_clienthidSendNotification).value = true;
        }

        function CancelNotification() {
            $get(_clienthidSendNotification).value = false;
        }
    </script>
</asp:Content>
