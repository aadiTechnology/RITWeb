<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" 
CodeFile="TransportCapacityDetailsUI.aspx.cs" Inherits="TransportCapacityDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
    <div class="MainBodyDiv">
    <style>
        .container {
            width: 400px;
            border: 2px solid #1f8f6d;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            height:auto;
            top:100px;
            left:100px;
            position:fixed;
            background-color:White;
        }

        .title-bar {
            background-color: #1f8f6d;
            color: white;           
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            padding:5px;
        }

        .content {            
            background-color: white;
            text-align:center;            
        }
    </style>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
        <tr>
            <td>
                <table width="70%" align="center">
                    <tr>
                        <td>
                            <asp:ListView ID="lstvwTransportCapacity" OnItemDataBound="lstvwTransportCapacity_ItemDataBound" DataKeyNames="VehicleNumber" runat="server">
                                <LayoutTemplate>
                                    <table width="100%" runat="server" style="color: #333333" cellpadding="0"
                                     cellspacing="1" class="GridBorder">
                                        <tr id="Tr2" runat="server" class="ClsGridHeader">
                                            <th>
                                            </th>
                                            <th>
                                            </th>
                                            <th>
                                            </th>
                                            <th>
                                            </th>
                                            <th colspan="3">
                                                Pickup Journey Student Count
                                            </th>
                                            <th colspan="3">
                                                Drop Journey  Student Count
                                            </th>
                                         </tr>
                                        <tr runat="server" class="ClsGridHeader">
                                            <th align="center" width="40px">
                                                Sr. No.
                                            </th>
                                            <th align="center" class="paddingL" width="200px">
                                                Route
                                            </th>
                                            <th align="center" class="paddingL" width="100px">
                                                Vehicle Number
                                            </th>
                                            <th align="center" class="paddingL" width="30px">
                                                Capacity
                                            </th>
                                            <th align="center" class="paddingL" width="40px">
                                                A
                                            </th>
                                            <th align="center" class="paddingL" width="40px">
                                                B
                                            </th>
                                            <th align="center" class="paddingL" width="40px">
                                                C
                                            </th>
                                            
                                            <th align="center" class="paddingL" width="40px">
                                                A
                                            </th>
                                            <th align="center" class="paddingL" width="40px">
                                                B
                                            </th>
                                            <th align="center" class="paddingL" width="40px">
                                                C
                                            </th>
                                         </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                          </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="tr1" runat="server" class="ClsGridRow">
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblRoute" runat="server" Text='<%#Eval("RouteName") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%#Eval("VehicleCapacity") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkPickupCountA" runat="server" Text='<%#Eval("PickUpCount_A") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkPickupCountB" runat="server" Text='<%#Eval("PickUpCount_B") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkPickupCountC" runat="server" Text='<%#Eval("PickUpCount_C") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkDropCountA" runat="server" Text='<%#Eval("DropCount_A") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                           <asp:LinkButton ID="lnkDropCountB" runat="server" Text='<%#Eval("DropCount_B") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkDropCountC" runat="server" Text='<%#Eval("DropCount_C") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="tr1" runat="server" class="ClsGridAltRow">
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblRoute" runat="server" Text='<%#Eval("RouteName") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:Label ID="lblCapacity" runat="server" Text='<%#Eval("VehicleCapacity") %>'></asp:Label>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkPickupCountA" runat="server" Text='<%#Eval("PickUpCount_A") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkPickupCountB" runat="server" Text='<%#Eval("PickUpCount_B") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                           <asp:LinkButton ID="lnkPickupCountC" runat="server" Text='<%#Eval("PickUpCount_C") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkDropCountA" runat="server" Text='<%#Eval("DropCount_A") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkDropCountB" runat="server" Text='<%#Eval("DropCount_B") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td align="center" class="paddingL">
                                            <asp:LinkButton ID="lnkDropCountC" runat="server" Text='<%#Eval("DropCount_C") %>' CausesValidation="false"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <div align="center" class="LblNoRecord">
                                         No Record Found.
                                    </div>
                                </EmptyDataTemplate>
                             </asp:ListView>
                             <tr>
                                <td>
                                    <asp:HiddenField ID="hidStd" runat="server" Value="" />
                                </td>
                             </tr>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div id="divStandardContainer" style="display:none;" class="container">
            <div class="title-bar">Standardwise Count</div>
            <div style="magin:10px auto;padding-top:10px;">
                <span id="spnVehicleNumber" style="margin-right:20px;font-weight:bold;"></span>
                <span id="spnJourney" style="font-weight:bold;"></span>                
            </div>
            <div id="divStandards" class="content">            
            </div>
            <div style="float:right;">
                <a style="float:right;padding-right:10px;" href="#" onclick="CloseDiv();return false;">Close</a>
            </div>
       </div>
</div>
<script language="javascript" type="text/javascript">

 function FillStandardwiseCount(obj,VehicleNumber, TypeId, Category) {
           
            $('#divStandardContainer').fadeIn(500)
            $('#divStandardContainer').css({"left":((window.screen.width/2)-200)+'px'})            
            
            $('#spnVehicleNumber').html("Vehicle Number : "+VehicleNumber)
            $('#spnJourney').html("Journey : "+ (TypeId==1?"PICKUP ":"DROP ") + Category)
           
            var Standard = $('[id$=hidStd]').val()
            var remarkData = JSON.parse(Standard)
            var filteredData = remarkData.filter(rmk => rmk.VehicleNumber == VehicleNumber && rmk.JourneyTypeId == TypeId &&  rmk.JourneyName == Category);

            var sContent = ''
            var cls = 'ClsGridRow'
            for(var k=0; k< filteredData.length; k++)
            {
                if(k%2==0)
                    cls = 'ClsGridRow'
                else
                    cls = 'ClsGridAltRow'

                sContent += '<tr class="'+cls+'"><td><label class="link">'+filteredData[k].StandardName+'</label></td><td><label class="link">'+filteredData[k].Count+'</label></td></tr>'
            }

             $('#divStandards').html('<table class="GridBorder" style="margin:10px auto;"><tr class="ClsGridHeader"><th style="width:100px;padding:10px;">Standard</th><th style="width:150px;padding:10px;">Student Count</th></tr>'+ sContent +'</table>')

             $('[id$=_lnkPickupCountA]').attr('style','font-weight:regular;color:#428bca');
             $('[id$=_lnkPickupCountB]').attr('style','font-weight:regular;color:#428bca');
             $('[id$=_lnkPickupCountC]').attr('style','font-weight:regular;color:#428bca');
             $('[id$=_lnkDropCountA]').attr('style','font-weight:regular;color:#428bca');
             $('[id$=_lnkDropCountB]').attr('style','font-weight:regular;color:#428bca');
             $('[id$=_lnkDropCountC]').attr('style','font-weight:regular;color:#428bca');
             
             $(obj).css("font-weight","bold");
             $(obj).css("color","maroon");
    }

    function CloseDiv()
        {
            $('#divStandardContainer').hide()
        }


</script>
</asp:Content>

