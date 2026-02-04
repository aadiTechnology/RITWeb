<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentTransportHistoryUI.aspx.cs" Inherits="StudentTransportHistoryUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr>
                <td style="height: 25px;">
                </td>
            </tr>
            <tr>
                <td>
                    <table width="40%" align="left" style="text-align: center;">
                        <tr>
                            <td align="center" id="tdUserName" runat="server">
                                <table width="100%" cellpadding="1" cellspacing="1" align="center" style="text-align: center;">
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width: 120px; height: 20px; padding-left : 5px;">
                                            <asp:Label ID="lblType" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" EnableViewState="False"
                                                Font-Bold="True" Text="User Name : "></asp:Label>
                                        </td>
                                        <td class="ClsHilightPhotoBGB" style="text-align: left; padding-left: 10px;">
                                            <asp:Label ID="lblUserName" runat="server" EnableViewState="true" Font-Bold="True"
                                                Height="20px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>            
            <tr style="text-align: center; margin: 0px solid;" align="center">
                <td align="center" style="text-align: center; margin: 0px auto;">
                    <table width="100%" align="center" style="text-align: center; margin: 0px auto;">
                        <tr>
                            <td>
                                <asp:ListView ID="lstvwStudentHistory" runat="server" DataKeyNames="UserId" 
                                    onitemdatabound="lstvwStudentHistory_ItemDataBound">
                                    <LayoutTemplate>
                                        <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                <th align="center" class="clsLabelgrd" style="font-size: 10pt; width:45px;">
                                                    <span>Sr. No.</span>
                                                </th>                                                
                                                <th align="center" class="clsLabelgrd" style="width: 100px; font-size: 10pt;">
                                                    <span>Route</span>
                                                </th>
                                                <th align="center" class="clsLabelgrd" style="width: 120px; font-size: 10pt;">
                                                   <span>Stop</span>
                                                </th>
                                                <th align="center" class="clsLabelgrd" width="100px" style="font-size: 10pt;">
                                                    <span>Shift</span>
                                                </th>
                                                <th align="center" class="clsLabelgrd" width="110px" style="font-size: 10pt;">
                                                    <span>Vehicle Number</span>
                                                </th>
                                                <th align="center" class="clsLabelgrd" width="110px" style="font-size: 10pt;">
                                                    <span>Effective From Date</span>
                                                </th> 
                                                <th align="center" class="clsLabelgrd" width="110px" style="font-size: 10pt;">
                                                    <span>Effective To Date</span>
                                                </th> 
                                                <th align="center" class="clsLabelgrd" width="60px" style="font-size: 10pt;">
                                                    <span>Left Date</span>
                                                </th>                                                
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("SrNo") %>'></asp:Label>
                                            </td>                                            
                                            <td align="center">
                                                <asp:Label ID="lblRoute" runat="server" CssClass="clsLabelC" Text='<%#Eval("Route") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblStop" runat="server" CssClass="clsLabelC" Text='<%#Eval("Stop") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblShift" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("Shift") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblVehicleNumber" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                            </td>
                                             <td align="center">
                                                <asp:Label ID="lblEffectiveFromDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("EffectiveFromDate") %>'></asp:Label>
                                            </td>  
                                             <td align="center">
                                                <asp:Label ID="lblEffectiveToDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("EffectiveToDate") %>'></asp:Label>
                                            </td>      
                                            <td align="center">
                                                <asp:Label ID="lblLeftDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("LeftDate") %>'></asp:Label>
                                            </td>                                         
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:Label ID="lblSrNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("SrNo") %>'></asp:Label>
                                            </td>                                           
                                            <td align="center">
                                                <asp:Label ID="lblRoute" runat="server" CssClass="clsLabelC" Text='<%#Eval("Route") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblStop" runat="server" CssClass="clsLabelC" Text='<%#Eval("Stop") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblShift" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("Shift") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblVehicleNumber" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                            </td>   
                                            <td align="center">
                                                <asp:Label ID="lblEffectiveFromDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("EffectiveFromDate") %>'></asp:Label>
                                            </td>  
                                            <td align="center">
                                                <asp:Label ID="lblEffectiveToDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("EffectiveToDate") %>'></asp:Label>
                                            </td>  
                                            <td align="center">
                                                <asp:Label ID="lblLeftDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("LeftDate") %>'></asp:Label>
                                            </td>                                            
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table width="100%">
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
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr style="text-align: center; margin: 0px auto;" align="center">
                <td align="center" style="text-align: center; margin: 0px auto;">
                    <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" Text="Back" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
        <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
        <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
        <asp:HiddenField ID="hidSearchText" runat="server" Value="" />
        <asp:HiddenField ID="hidUserRoleId" runat="server" Value="0" />
        <asp:HiddenField ID="hidRouteId" runat="server" Value="0" />
        <asp:HiddenField ID="hidStopId" runat="server" Value="0" />
        <asp:HiddenField ID="hidShiftId" runat="server" Value="0" />
        <asp:HiddenField ID="hidIncludeNotAssociated" runat="server" Value="0" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
