<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DayworkinghrsConfigurationUI.aspx.cs" Inherits="DayworkinghrsConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div>
            <style>
                .check-box
                {
                    font-weight: bold;
                }
            </style>
            <table width="50%">
                <tr>
                    <td id="tdMessage" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="80%">
                            <tr>
                                <td align="center">
                                    <table>
                                        <tr>
                                            <td align="center" class="ClsBorderlight">
                                                <asp:Label ID="lblStandard" runat="server" Text="Standard" CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel colonPadding"></span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbStandardId" runat="server" CssClass="SmlCombo" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbStandardId_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="LegendTable" runat="server" visible="false">
                                        <tr>
                                            <td align="left" colspan="1">
                                                <span class="ClsLblLgnd"><b>Legend:</b> </span>
                                            </td>
                                            <td align="left" valign="middle" class="LblNormal" style="border: 1px solid #000000;">
                                                <b>FH</b>
                                            </td>
                                            <td>
                                                <b>Full Hours</b>
                                            </td>
                                            <td align="left" valign="middle" class="LblNormal" style="border: 1px solid #000000;">
                                                <b>HH</b>
                                            </td>
                                            <td>
                                                <b>Half Hours </b>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table align="center" width="50%">
                                        <tr>
                                            <td align="center" style="width: 50%">
                                                <asp:ListView ID="lstvwWorkinghrs" runat="server" OnItemDataBound="lstvwWorkinghrs_ItemDataBound"
                                                    DataKeyNames="DivisionId">
                                                    <LayoutTemplate>
                                                        <table align="center" width="40%" runat="server" id="tblhrsInfo" style="color: #333333"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left">
                                                                    <asp:Label ID="lblDivisions" runat="server" Text="Divisions"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblMonday" runat="server" Text="Monday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblTuesday" runat="server" Text="Tuesday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblWednesday" runat="server" Text="Wednesday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblThursday" runat="server" Text="Thursday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblFriday" runat="server" Text="Friday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblSaturday" runat="server" Text="Saturday"></asp:Label>
                                                                </th>
                                                                <th align="center" class="ClsPaddingL" colspan="2">
                                                                    <asp:Label ID="lblSunday" runat="server" Text="Sunday"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="trheader2" class="ClsGridHeader">
                                                                <th align="left" style="padding-left: 5px;">
                                                                    <asp:Label ID="lblDivision1" runat="server" Width="50px"></asp:Label>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsMonday" runat="server" MaxLength = "5" placeholder="FH"
                                                                        Width="50px" onchange="ChangeAllFullhours('txtfullhrsMonday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsMonday" runat="server" MaxLength = "5" placeholder="HH" Width="50px" onchange="ChangeAllFullhours('txtHalfhrsMonday')"
                                                                        Style="text-align: right;" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsTuesday" MaxLength = "5" runat="server" placeholder="FH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtfullhrsTuesday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsTuesday" MaxLength = "5" runat="server" placeholder="HH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtHalfhrsTuesday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="left">
                                                                    <asp:TextBox ID="txtfullhrsWednesday" runat="server" MaxLength = "5" placeholder="FH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtfullhrsWednesday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsWednesday" runat="server" MaxLength = "5" placeholder="HH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtHalfhrsWednesday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsThursday" runat="server" MaxLength = "5" placeholder="FH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtfullhrsThursday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsThursday" runat="server" MaxLength = "5" placeholder="HH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtHalfhrsThursday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsFriday" runat="server" MaxLength = "5" placeholder="FH" Width="50px" onchange="ChangeAllFullhours('txtfullhrsFriday')"
                                                                        Style="text-align: right;" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsFriday" runat="server" MaxLength = "5" placeholder="HH" Width="50px" onchange="ChangeAllFullhours('txtHalfhrsFriday')"
                                                                        Style="text-align: right;" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsSaturday" runat="server" MaxLength = "5" placeholder="FH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtfullhrsSaturday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsSaturday" runat="server" MaxLength = "5" placeholder="HH" Width="50px"
                                                                        onchange="ChangeAllFullhours('txtHalfhrsSaturday')" Style="text-align: right;"
                                                                        onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th>
                                                                    <asp:TextBox ID="txtfullhrsSunday" runat="server" MaxLength = "5" placeholder="FH" Width="50px" onchange="ChangeAllFullhours('txtfullhrsSunday')"
                                                                        Style="text-align: right;" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                                <th align="right">
                                                                    <asp:TextBox ID="txtHalfhrsSunday" runat="server" MaxLength = "5" placeholder="HH" Width="50px" onchange="ChangeAllFullhours('txtHalfhrsSunday')"
                                                                        Style="text-align: right;" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                        onpaste="event.returnValue=false"></asp:TextBox>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td align="left" style="padding-left: 5px;">
                                                                <asp:Label ID="lblDivision1" runat="server" Text='<%#Eval("DivisionName") %>' Width="50px"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsMonday" runat="server" MaxLength = "5" Width="50px" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsMonday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsTuesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsTuesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsWednesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsWednesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsThursday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsThursday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsFriday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsFriday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsSaturday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsSaturday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsSunday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsSunday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr>
                                                            <td align="left" style="padding-left: 5px;">
                                                                <asp:Label ID="lblDivision1" runat="server" Text='<%#Eval("DivisionName") %>' Width="50px"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsMonday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsMonday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsTuesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsTuesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsWednesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsWednesday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsThursday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsThursday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsFriday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsFriday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsSaturday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsSaturday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtfullhrsSunday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtHalfhrsSunday" runat="server" Width="50px" MaxLength = "5" Style="text-align: right;"
                                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="650px">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No Records Found.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                    OnClick="btnSave_Click" />
                                                <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                                                    CssClass="ClsBtn" CausesValidation="false" PostBackUrl="~/RITeSchool/Admin/schoolconfigurationcontrolpanel.aspx" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">

        _ClientListValue = "<%=this.lstvwWorkinghrs.ClientID %>"

        function CheckValidations() {            
            var sVAl = CheckSaveTextbox();
            var sCheck = CheckValueTextbox();

            if (sVAl == false && sCheck == false) {
                alert("Value of Full Hours and Half Hours should not be blank for none of the division - weekday.");
                return false;
            }
            else if (sVAl == true && sCheck == false) {
                alert("Value of Half Hours  should not be greater than Full Hours.");
                return false;
            }
            else if (sVAl == false && sCheck == true) {
                alert("Value of Full Hours and Half Hours should not be blank for none of the division - weekday.");
                return false;
            }
            else
                return true;
        }

        //This function is used for the blank textbox validation.
        function CheckSaveTextbox() {
            var lbl
            var iRowCount = 0
            lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblDivision1")
            var bIsBlank = false;
            while (lbl != null) {

                if (GetTextValue("txtfullhrsMonday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsMonday", iRowCount) == "" ||
                GetTextValue("txtfullhrsTuesday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsTuesday", iRowCount) == "" ||

                GetTextValue("txtfullhrsWednesday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsWednesday", iRowCount) == "" ||
                GetTextValue("txtfullhrsThursday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsThursday", iRowCount) == "" ||

                GetTextValue("txtfullhrsFriday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsFriday", iRowCount) == "" ||
                GetTextValue("txtfullhrsSaturday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsSaturday", iRowCount) == "" ||

                GetTextValue("txtfullhrsSunday", iRowCount) == "" ||
                GetTextValue("txtHalfhrsSunday", iRowCount) == "") {
                    bIsBlank = true
                    break;
                }

                iRowCount++;
                lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblDivision1")
            }

            if (bIsBlank) {
                return false
            }

            return true;


        }

        function GetTextValue(txtName, iRowCount) {
            var txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
            return txt.value.trim()
        }


        //This function is used Validation for FH and HH values.
        function CheckValueTextbox() {
            var lbl
            var iRowCount = 0
            lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblDivision1")
            var bIsBlank = false;
            while (lbl != null) {

                var FH = GetTextValue("txtfullhrsMonday", iRowCount)
                var HH = GetTextValue("txtHalfhrsMonday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;

                }
                var FH = GetTextValue("txtfullhrsTuesday", iRowCount)
                var HH = GetTextValue("txtHalfhrsTuesday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;
                }

                var FH = GetTextValue("txtfullhrsWednesday", iRowCount)
                var HH = GetTextValue("txtHalfhrsWednesday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;
                }

                var FH = GetTextValue("txtfullhrsThursday", iRowCount)
                var HH = GetTextValue("txtHalfhrsThursday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;

                }
                var FH = GetTextValue("txtfullhrsFriday", iRowCount)
                var HH = GetTextValue("txtHalfhrsFriday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;
                }

                var FH = GetTextValue("txtfullhrsSaturday", iRowCount)
                var HH = GetTextValue("txtHalfhrsSaturday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;
                }


                var FH = GetTextValue("txtfullhrsSunday", iRowCount)
                var HH = GetTextValue("txtHalfhrsSunday", iRowCount)
                if (parseInt(HH) > parseInt(FH)) {
                    bIsBlank = true
                    break;
                }
                iRowCount++;
                lbl = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_lblDivision1")
            }

            if (bIsBlank) {
                return false
            }

            return true;
        }

        function GetTextValue(txtName, iRowCount) {
            var txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
            return txt.value.trim()
        }



        //This function is used to set the all textbox value as per the Header textbox.

        function ChangeAllFullhours(txtName) {
            var txtAll = document.getElementById(_ClientListValue + "_" + txtName);
            var txt
            var iRowCount = 0;
            if (iRowCount < 10)
                txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
            else
                txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)

            while (txt != null) {
                if (txt) {
                    var txtFullhrsmonday = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
                    txtFullhrsmonday.value = txtAll.value;

                }
                iRowCount = iRowCount + 1;
                if (iRowCount < 10)
                    txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
                else
                    txt = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_" + txtName)
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
