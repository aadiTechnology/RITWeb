<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserAttendanceInBusDetailsUI.aspx.cs" Inherits="UserAttendanceInBusDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <div style="float: right; vertical-align: top;">
                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td style="width:25%">
                                        </td>
                                        <td align="center">
                                           <asp:UpdatePanel ID="upnl20" runat="server" UpdateMode="Conditional">
                                           <ContentTemplate>                                           
                                                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"  ForeColor = "Blue"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnLoadData" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlVehicleNumber" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />                                                
                                            </Triggers>
                                           </asp:UpdatePanel>
                                        </td>
                                        <td align="right" style="width:25%">
                                            <asp:Button ID="btnLoadData" runat="server" CausesValidation="false"
                                                Text="Load Current Notification Details" CssClass="ClsBtn" 
                                                onclick="btnLoadData_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trBusAttendance" runat="server" visible="false">
                                        <td colspan="3" align="right">
                                            <asp:Button ID="btnLoadAttendance" runat="server" CausesValidation="false" 
                                                Text="Load Attendance" CssClass="ClsBtn" onclick="btnLoadAttendance_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>       
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center;">
                                <table align="center">                                    
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblDate" runat="server" Text="Date :" CssClass="ClsLabel"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtDate" CssClass="SmlCombo" runat="server" AutoPostBack="true"></asp:TextBox>
                                            <rjs:PopCalendar ID="calDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                                ShowWeekend="true" ShowErrorMessage="false" />
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="reqDate" runat="server" Display="None" ControlToValidate="txtDate"
                                                ErrorMessage="Date Should not be blank."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblVehicleNo" runat="server" Text="Vehicle Number :" CssClass="ClsLabel"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlVehicleNumber" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="reqVehicleNo" runat="server" Display="None" ControlToValidate="ddlVehicleNumber"
                                                InitialValue="0" ErrorMessage="Vehicle No. should be selected."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblJourney" runat="server" Text="Journey :" CssClass="ClsLabel"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnl1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlJourney" runat="server" CssClass="LrgCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlVehicleNumber" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="reJourney" runat="server" Display="None" ControlToValidate="ddlJourney"
                                                InitialValue="0" ErrorMessage="Journey should be selected."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnShow" runat="server" CssClass="ClsBtn" Text="Show" OnClick="btnShow_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" width="100%">
                                    <%--<tr runat="server" id="trTotalRec" align="center">
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="100" PagedControlID="lstvwUserAttendanceDetails">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1 %>" />
                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                            <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize %>" />
                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                            <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount %>" />
                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                            <br />
                                                        </PagerTemplate>
                                                    </asp:TemplatePagerField>
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            <table align="center" width="98%">
                                                <tr id="trLegend" runat="server" visible="false">
                                                    <td align="left">
                                                        <table>
                                                            <tr>
                                                               <td width="70px">
                                                                    <span class="ClsLblLgnd">Legend : </span>
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <span class="ClsLabel" style="color: Maroon; float: inherit; white-space: nowrap;font-weight:bold;padding-left:5px;padding-right:5px;">Vehicle Change</span>
                                                                </td>
                                                                <td style="width:5px;">
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <span class="ClsLabel" style="color: Maroon; float: inherit; white-space: nowrap;font-weight:bold;padding-left:5px;padding-right:5px;">N/A - Not available for this vehicle</span>
                                                                </td>
                                                                <td style="width:5px;">
                                                                </td>
                                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                                    <span class="ClsLabel" style="color: Navy; float: inherit; white-space: nowrap;font-weight:bold;padding-left:5px;padding-right:5px;">Journey Change</span>
                                                                </td>                                                                
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListView ID="lstvwUserAttendanceDetails" runat="server" DataKeyName="VehicleId"
                                                            OnDataBound="lstvwUserAttendanceDetails_DataBound" 
                                                            OnItemCommand="lstvwUserAttendanceDetails_ItemCommand" 
                                                            onitemdatabound="lstvwUserAttendanceDetails_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" style="color: #333333" cellpadding="0" cellspacing="1"
                                                                    class="GridBorder">
                                                                    <tr class="ClsGridHeader">                                                                    
                                                                        <th align="center" class="paddingL" colspan="6">                                                                            
                                                                        </th>
                                                                        <th align="center" class="paddingL" colspan="3">
                                                                            Notification Status                                                                            
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" class="ClsGridHeader">
                                                                        <th align="left" class="paddingL" width="50px">
                                                                            Sr.No.
                                                                        </th>
                                                                        <th align="left" class="paddingL" width="230px">
                                                                            Student Name
                                                                        </th>
                                                                        <th align="left" class="paddingL" width="80px">
                                                                            Class
                                                                        </th>                                                                        
                                                                        <th align="left" class="paddingL" width="250px">
                                                                            Route
                                                                        </th>                                                                        
                                                                        <th align="center" class="paddingL" width="50px">
                                                                            Time
                                                                        </th>
                                                                        <th align="center" class="paddingL" width="50px">
                                                                            Location
                                                                        </th>
                                                                        <th align="center" class="paddingL" width="50px">
                                                                            On-Boarding
                                                                        </th>
                                                                         <th align="center" class="paddingL" width="50px">
                                                                            Geofencing
                                                                        </th>
                                                                         <th align="center" class="paddingL" width="50px">
                                                                            Off-Boarding
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                   <%-- <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                        <td colspan="9">
                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUserAttendanceDetails"
                                                                                PageSize="100">
                                                                                <Fields>
                                                                                    <asp:TemplatePagerField>
                                                                                        <PagerTemplate>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td align="right" class="LblNormal">
                                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </PagerTemplate>
                                                                                    </asp:TemplatePagerField>
                                                                                </Fields>
                                                                            </asp:DataPager>
                                                                        </td>
                                                                    </tr>--%>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="tr1" runat="server" class="ClsGridRow">
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblStdName" runat="server" Text='<%#Eval("Standard_Name") %>'></asp:Label>
                                                                    </td>                                                                    
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("RouteName") %>'></asp:Label>
                                                                    </td>                                                                    
                                                                    <td align="center">
                                                                        <asp:Label ID="lblTime1" runat="server" Text='<%#Eval("PunchingDateTime") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="LnkBtnLocation" runat="server" Text="Location" CausesValidation="false"></asp:LinkButton>
                                                                    </td>
                                                                   <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblOnBoard" runat="server" Text='<%#Eval("IsOnBoardingNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblGeo" runat="server" Text='<%#Eval("IsGeofenceNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblOffBoard" runat="server" Text='<%#Eval("IsOffBoardingNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="tr1" runat="server" class="ClsGridAltRow">
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblStudentName" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblStdName" runat="server" Text='<%#Eval("Standard_Name") %>'></asp:Label>
                                                                    </td>                                                                    
                                                                    <td align="left" class="paddingL">
                                                                        <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("RouteName") %>'></asp:Label>
                                                                    </td>                                                                    
                                                                    <td align="center">
                                                                        <asp:Label ID="lblTime1" runat="server" Text='<%#Eval("PunchingDateTime") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="LnkBtnLocation" runat="server" Text="Location" CausesValidation="false"></asp:LinkButton>
                                                                    </td>
                                                                    <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblOnBoard" runat="server" Text='<%#Eval("IsOnBoardingNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblGeo" runat="server" Text='<%#Eval("IsGeofenceNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                    <td align="center" class="paddingL">
                                                                        <asp:Label ID="lblOffBoard" runat="server" Text='<%#Eval("IsOffBoardingNotificationSent") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                            <EmptyDataTemplate>
                                                                <div align="center" class="LblNoRecord">
                                                                    No Record Found.
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:ListView>
                                                        <%--<asp:ObjectDataSource ID="objUserAttendance" TypeName="BusinessLogic.TransportBL.UserAttendanceInBusBL"
                                                            EnablePaging="true" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                            SelectCountMethod="GetCount" EnableCaching="false">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                    Type="int32" />
                                                                <asp:ControlParameter ControlID="txtDate" Name="adDate" Type="DateTime" />
                                                                <asp:ControlParameter ControlID="ddlVehicleNumber" Name="aiVehicleId" Type="Int32"
                                                                    PropertyName="SelectedValue" />
                                                                <asp:ControlParameter ControlID="ddlJourney" Name="aiJourneyId" Type="Int32" PropertyName="SelectedValue" />
                                                                <asp:Parameter Name="SortExpression" Type="String" />
                                                                <asp:Parameter Name="SortDirection" Type="String" />
                                                                <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>--%>
                                                        <asp:HiddenField ID="hidVehicleId" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>                                                                                        
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript">

            function OpenLocation(sURL) {            
                window.open(sURL);
            }

        </script>
    </div>
</asp:Content>
