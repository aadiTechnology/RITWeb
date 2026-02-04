<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentTransportDetailsUI.aspx.cs" Inherits="StudentTransportDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table width="80%" >
                        <tr style="height: 5px;">
                            <td align = "right"  >
                             <asp:LinkButton ID="lnkbtnRoute" runat="server" Text="View Route Map" CssClass="SubTitle" CausesValidation="false" Visible="false"></asp:LinkButton> &nbsp &nbsp &nbsp
                                <div id="divGPSTracking" runat="server" class="ClsGreenBG" style="width: 100px; float: right; height: 16px;" >
                                    <asp:HyperLink ID = "hlnkGPSTrancking" runat="server" CssClass="SubTitle" style="cursor:pointer; " onclick="openGPSTrackingLink()" Text = "<u>GPS Tracking</u>"
                                    ForeColor="Blue" />                                    
                                </div>                               
                             </td>                                        
                         </tr>
                            <tr>
                                <td align="left" class="HilightBGGray">
                                    <table>
                                    <tr>
                                       <td align="left" style="background-color:#e7e7e7;color:#906;font-size:9pt;font-weight:700;font-family:Verdana;height:20px;">
                                        <span><strong>Select Route Type :</strong></span>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdoTransportTypes" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" DataTextField="Type" DataValueField="Id" style="font-size:9pt;font-weight:bold"
                                            onselectedindexchanged="rdoTransportTypes_SelectedIndexChanged">
                                        <asp:ListItem Text="Pick Up" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Drop" Value="2" ></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    </tr>
                                   </table>
                                </td>                            
                            </tr>
                            <tr id="trTransportDetails" runat="server" width="80%">
                                <td class="HilightBGGray" width="80%" colspan="4" >
                                    <span><strong>Transport Staff Details :</strong></span>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td valign="top">
                                    <table id="tblStudentTransportDetails" runat="server" border="1" cellpadding="1"
                                        cellspacing="4" align="left" visible="true" 
                                        style=" vertical-align:top; border-style: solid; width:100%; height: 7px;">
                                    </table>
                                </td>
                            </tr>
                            <tr id="trVehicleDetails" runat="server">
                                <td class="HilightBGGray" width="50%" colspan="4">
                                    <span><strong>Vehicle Details :</strong> </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="1" cellpadding="1" cellspacing="4" id="tblVehicleDetails" visible="true"
                                        runat="server" style="border-style: solid; width: 100%;" align="center">
                                        <tr>
                                            <td align="left"  colspan="1" style="width: 8%" class="ClsBorderlight paddingL ">
                                                <span class="ClsLabel"  > Route :</span>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight paddingL " style="width: 20%; height: 26px;">
                                                <asp:Label ID="lblVehicleRoute" runat="server" CssClass="ClsLabel" ></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" width="11%" class="ClsBorderlight paddingL">
                                                <span class="ClsLabel" >Vehicle Type :</span>
                                            </td>
                                            <td align="left" colspan="1" width="12%" class="ClsBorderlight paddingL ">
                                                <asp:Label ID="lblVehicle" runat="server" CssClass="ClsLabel" ></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight paddingL" style="width: 15%;">
                                                <span class="ClsLabel">Vehicle Number :</span>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight paddingL " style="width: 12%;">
                                                <asp:Label ID="lblVehicleNumber" runat="server" CssClass="ClsLabel" ></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight paddingL" style="width: 12%;" id="tdVehicleContactNoHeader" runat="server" visible="false">
                                                <span class="ClsLabel">Contact Number :</span>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight paddingL " style="width: 15%;" id="tdVehicleContactNo" runat="server" visible="false">
                                                <asp:Label ID="lblVehicleContactNo" runat="server" CssClass="ClsLabel" ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                              <tr id="trShiftdet" runat="server">
                                <td class="HilightBGGray" width="100%" colspan="4">
                                    <span><strong>Shift Details :</strong> </span>
                                </td>
                            </tr>
                            <tr id="trShift" runat="server" width="100%">
                                <td align="center" >
                                    <table border="1" cellpadding="1" width="100%" cellspacing="4" id="tblShift" visible="true" runat="server"
                                        style="border-style: solid;" align="center">
                                        <tr>
                                            <td align="center" colspan="1" class="ClsBorderlight paddingL" style="width: 11%; height: 26px">
                                                <span class="ClsLabel">Shift :</span>
                                            </td>
                                            <td align="center" colspan="1" class="ClsBorderlight paddingL " style="width: 12%;">
                                                <asp:Label ID="lblShift" runat="server" CssClass="ClsLabel" ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                  <table align="center" width="80%" class="LblNoRecord" id="tblErrorMessage" runat="server">
                                        <tr runat="server">
                                            <td align="center">
                                                <asp:Label ID="lblError" runat="server" CssClass=" ClsConfigText" EnableViewState="False" 
                                                    Text="No records found." ForeColor = "Blue"  ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   
                                </td>
                            </tr>
                            <tr id="trStops" runat="server">
                                <td>
                                    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 65%; 
                                        vertical-align: top" id="tblList" runat="server">
                                        <tr id="trPagerTransportStaff" runat="server">
                                            <td align="center">
                                             <table id="tblLegend" runat="server" align="left">
                                        <tr>
                                            <td align="left" valign="middle" colspan="1" class="ClsLblLgnd">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px"  Font-Bold="True"
                                                    Text="Legend : " EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                &nbsp;<asp:Label ID="txtUserStop" runat="server" BackColor="#FFCCCC" Height="20px"
                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                    EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="My Stop" CssClass="ClsTextNormal"
                                                    EnableViewState="false"></asp:Label>
                                            </td>                                            
                                            <td>
                                            </td>                                                
                                                <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                <asp:Label ID="Label7" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Applicable Details"
                                                    ForeColor="Maroon" Font-Bold="True" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                    Width="110px" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwStudentTransportDetails" runat="server" DataKeyNames="StopName,StopNumber"
                                                    OnItemDataBound="lstvwStudentTransportDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" style="padding-left: 9px;">
                                                                    Stop No.
                                                                </th>
                                                                <th align="left" class="paddingL">
                                                                    Stop Name
                                                                </th>
                                                                <th align="center" style="padding-left: 9px;">
                                                                    Pickup Time
                                                                </th>
                                                                <th align="center" style="padding-left: 9px;">
                                                                    Drop Time
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="5">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trTimingDetails" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblStopNumber" runat="server" Text='<%# Eval("StopNumber") %>'></asp:Label>
                                                            </td>
                                                            <td class="paddingL" >
                                                                <asp:Label ID="lblStopName" runat="server" Text='<%# Eval("StopName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" >
                                                                <asp:Label ID="lblPickUpTime" runat="server" Text='<%# Eval("PickupTime") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" >
                                                                <asp:Label ID="DropTime" runat="server" Text='<%# Eval("DropTime") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                     <EmptyDataTemplate>
                                                        <table style="width: 80%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No record found.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>                                                    
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:HiddenField ID="hidStopName" runat="server" />
                                                 <asp:HiddenField ID="hidGPSTrackingUrl" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr id="trTracking" runat="server" visible="false">
                                <td class="HilightBGGray" width="100%" colspan="4">
                                    <span><strong>Bus Status :</strong> </span>
                                </td>
                            </tr>
                            <tr id="trMap" runat="server">
                                <td align="center">
                                    <div style="border:2px solid gray;" id="divPickupVehicleURL" runat="server" visible="false">
                                    </div>
                                </td>
                            </tr>
                            <tr id="trMapMessage" runat="server">
                                <td align="left">
                                    <asp:Label ID="lblMapMessage" runat="server" CssClass="ClsLabel" Font-Bold="true" ><strong>The bus is in parking. You will be able to track the bus when the trip starts.</strong></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _ClienthidStopName = "<%=this.hidStopName.ClientID %>"

        //This function is used to open GPS Tracking Demo video
        function openGPSTrackingLink(_ClienthidGPSTrackingLink) {
            window.open(_ClienthidGPSTrackingLink, '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1100,height=750')
        }

        function OpenWindow(sfilepath) {
            window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            return false;
        }
    </script>
</asp:Content>
