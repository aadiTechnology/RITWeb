<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransportNotificationDetailsUI.aspx.cs" Inherits="TransportNotificationDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBody">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <div style="float: right; vertical-align: top;">
                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields</span>
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
                                                <asp:AsyncPostBackTrigger ControlID="ddlRoute" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                            </Triggers>
                                           </asp:UpdatePanel>
                                        </td>
                                        <td align="right" style="width:25%">
                                            <asp:Button ID="btnLoadData" runat="server" CausesValidation="false"
                                                Text="Load Current Notification Details" CssClass="ClsBtn" 
                                                onclick="btnLoadData_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>       
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center;">
                                <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" style="width: 150px">
                                                    <asp:Label ID="lblRoute" runat="server" CssClass="ClsLabel" Text="Route :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled"
                                                        OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <%-- <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="ReqRoute" runat="server" ControlToValidate="ddlRoute"
                                                ErrorMessage="Route should be selected." Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblJourney" runat="server" CssClass="ClsLabel" Text="Journey :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlJourney" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRoute" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <%-- <span class="ClsMdtStar">*</span>
                                            <asp:RequiredFieldValidator ID="ReqJourney" runat="server" ControlToValidate="ddlJourney"
                                                ErrorMessage="Journey should be selected." Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="clsLabel" Text="Vehicle :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <%--<asp:DropDownList ID="ddlVehicleNo" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                            </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="LrgTxtBox" MaxLength="15"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblStartDate" runat="server" CssClass="ClsLabel" Text="Start Date :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                        ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid date." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate"
                                                        ErrorMessage="Start Date should not be blank." Display="None"></asp:RequiredFieldValidator>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="ClsLabel" Text="End Date :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                        ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid date." />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate"
                                                        ErrorMessage="End Date should not be blank." Display="None"></asp:RequiredFieldValidator>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblType" runat="server" CssClass="ClsLabel" Text="Type :"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlTypes" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text="Student Name :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>                                            
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" class="ClsBtn" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" class="ClsBtn" CausesValidation="false"
                                    OnClick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="90%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblTotalCount" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwNotificationDetails" runat="server" OnItemDataBound="lstvwNotificationDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="100%" runat="server" style="color: #333333" cellpadding="0" cellspacing="1"
                                                                class="GridBorder">
                                                                <tr runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="50px">
                                                                        Sr. No.
                                                                    </th>
                                                                    <th align="left" class="paddingL" style="width: 200px">
                                                                        Student Name
                                                                    </th>
                                                                    <th align="left" width="50px" class="paddingLR">
                                                                        Standard
                                                                    </th>
                                                                    <th align="left" width="50px" class="paddingLR">
                                                                        Division
                                                                    </th>
                                                                    <%--<th align="center" class="paddingL" width="100px">
                                                                        Vehicle No.
                                                                    </th>--%>
                                                                    <th align="center" width="120px">
                                                                        Sent Date/Time
                                                                    </th>
                                                                    <th align="left" width="600px" class="paddingL">
                                                                        Message
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="tr1" runat="server" class="ClsGridRow">
                                                                <td align="center" class="paddingLR">
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblStudentName" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLR">
                                                                    <asp:Label ID="lblStd" runat="server" Text='<%#Eval("Standard_Name") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLR">
                                                                    <asp:Label ID="lblDiv" runat="server" Text='<%#Eval("Division_Name") %>'></asp:Label>
                                                                </td>
                                                                <%--<td align="center" class="paddingL">
                                                                    <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                                                </td>--%>
                                                                <td align="center" class="paddingL">
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("CreateDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblMsg" runat="server" Text='<%#Eval("MessageString") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="tr1" runat="server" class="ClsGridAltRow">
                                                                <td align="center" class="paddingLR">
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblStudentName" runat="server" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLR">
                                                                    <asp:Label ID="lblStd" runat="server" Text='<%#Eval("Standard_Name") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingLR">
                                                                    <asp:Label ID="lblDiv" runat="server" Text='<%#Eval("Division_Name") %>'></asp:Label>
                                                                </td>
                                                                <%--<td align="center" class="paddingL">
                                                                    <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                                                </td>--%>
                                                                <td align="center" class="paddingL">
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("CreateDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblMsg" runat="server" Text='<%#Eval("MessageString") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div align="center" class="LblNoRecord">
                                                                No Record Found.
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
