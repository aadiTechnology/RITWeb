<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransportOverrideDetailsUI.aspx.cs" Inherits="TransportOverrideDetailsUI" %>

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
                            <td align="center">
                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                            CssClass="Clslabel" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwTransportOverrideDetails" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center;">
                            <asp:UpdatePanel ID="upnl3" runat="server">
                                <ContentTemplate>                                    
                                    <table align="center">                                   
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width:150px">
                                            <asp:Label ID="lblRouteNo" runat="server" CssClass="clsLabel" Text="Route No / Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtRouteNo" runat="server" CssClass="LrgCombo"></asp:TextBox>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="clsLabel" Text="Vehicle No : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="LrgCombo"></asp:TextBox>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblJourney" runat="server" CssClass="clsLabel" Text="Journey Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtJourney" runat="server" CssClass="LrgCombo"></asp:TextBox>
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width:150px">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Text="Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtOverrideName" runat="server" CssClass="LrgCombo"></asp:TextBox>
                                        </td>
                                        <td align="left" class="ClsBorderLight">
                                            <asp:Label ID="lblStudentName" runat="server" CssClass="clsLabel" Text="Student Reg No / Name : "></asp:Label>
                                        </td>
                                        <td colspan="3" align="left">
                                            <asp:TextBox ID="txtStudentName" runat="server" CssClass="LrgCombo" Width="100%"></asp:TextBox>
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
                            <td>
                                <table align="center">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnClear" runat="server" CssClass="ClsBtn" Text="Clear" 
                                                onclick="btnClear_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" width="100%">
                                            <tr>
                                                <td align="left" style="padding-left:15px;">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="ClsLblLgnd">Legend : </span>
                                                            </td>
                                                            <td style="border:1px solid black;padding-left:5px; padding-right:5px;">
                                                                <span style="color:Navy">Active Overrides</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTransportOverrideDetails">
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
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table align="center" width="98%">
                                                        <tr>
                                                            <td>
                                                                <asp:ListView ID="lstvwTransportOverrideDetails" runat="server" DataKeyNames="Id"
                                                                    OnDataBound="lstvwTransportOverrideDetails_DataBound" OnItemCommand="lstvwTransportOverrideDetails_ItemCommand"
                                                                    OnItemDataBound="lstvwTransportOverrideDetails_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" style="color: #333333" cellpadding="0" cellspacing="1"
                                                                            class="GridBorder">
                                                                            <tr runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL">
                                                                                    Name
                                                                                </th>
                                                                                <th align="center" class="paddingL">
                                                                                    Start Date
                                                                                </th>
                                                                                <th align="center" class="paddingL">
                                                                                    End Date
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Source Route
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Source Vehicle
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Source Journey
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Target Route
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Target Vehicle
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Target Journey
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Category
                                                                                </th>
                                                                                <th align="center">
                                                                                    Edit
                                                                                </th>
                                                                                <th align="center">
                                                                                    Delete
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="12">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTransportOverrideDetails"
                                                                                        PageSize="20">
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
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="tr1" runat="server" class='<%# Container.DisplayIndex % 2 == 0?"ClsGridRow":"ClsGridAltRow" %>'>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%#Eval("StartDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblSourceRoute" runat="server" Text='<%#Eval("SourceRoute") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblSourceVehicle" runat="server" Text='<%#Eval("SourceVehicle") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblSourceJourney" runat="server" Text='<%#Eval("SourceJourney") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblTargetRoute" runat="server" Text='<%#Eval("TargetRoute") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblTargetVehicle" runat="server" Text='<%#Eval("TargetVehicle") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblTargetJourney" runat="server" Text='<%#Eval("TargetJourney") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("Category") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateTransportOverrideDetails"
                                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center" class="paddingL">
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DeleteTransportOverrideDetails"
                                                                                    ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>                                                                    
                                                                    <EmptyDataTemplate>
                                                                        <div align="center" class="LblNoRecord">
                                                                            No Record Found.
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                                <asp:ObjectDataSource ID="objOverrideDetails" TypeName="BusinessLogic.TransportBL.TransportOverrideDetailsBL"
                                                                    EnablePaging="true" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                                    SelectCountMethod="GetCount" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="int32" />
                                                                        <asp:ControlParameter ControlID="txtRouteNo" Name="asRouteNo" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtRouteNo" Name="asRouteName" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtVehicleNo" Name="asVehicleNo" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtJourney" Name="asJourneyName" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtStudentName" Name="asStudentName" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtStudentName" Name="asStudentRegNo" Type="String" />
                                                                        <asp:ControlParameter ControlID="txtOverrideName" Name="asOverrideName" Type="String" />
                                                                        <asp:Parameter Name="SortExpression" Type="String" />
                                                                        <asp:Parameter Name="SortDirection" Type="String" />
                                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" PostBackUrl="OverrideDetailsUI.aspx" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwTransportOverrideDetails" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function ConfirmDelete() {

            return confirm('Are you sure you want to delete this record?')
        }

    </script>
</asp:Content>
