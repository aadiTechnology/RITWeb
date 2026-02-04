<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransportOverrideConfigDetailsUI.aspx.cs" Inherits="TransportOverrideConfigDetailsUI" %>

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
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                            CssClass="Clslabel" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwTransportOverrideConfigDetails" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center;">
                                <table align="center">
                                    <tr>
                                        <td align="left" class="ClsBorderlight" style="width: 150px">
                                            <asp:Label ID="lblName" runat="server" CssClass="clsLabel" Text="Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="LrgCombo" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblRouteNo" runat="server" CssClass="clsLabel" Text="Route No / Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtRouteNo" runat="server" CssClass="LrgCombo"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblVehicleNo" runat="server" CssClass="clsLabel" Text="Vehicle No. : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="LrgCombo" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblJourney" runat="server" CssClass="clsLabel" Text="Journey Name : "></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtJourney" runat="server" CssClass="LrgCombo" MaxLength="100"></asp:TextBox>
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
                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table align="center" width="98%">
                                                        <tr runat="server" id="trTotalRec" align="center">
                                                            <td align="center">
                                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTransportOverrideConfigDetails">
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
                                                                <asp:ListView ID="lstvwTransportOverrideConfigDetails" runat="server" DataKeyNames="Id"
                                                                    OnDataBound="lstvwTransportOverrideConfigDetails_DataBound" OnItemCommand="lstvwTransportOverrideConfigDetails_ItemCommand"
                                                                    OnItemDataBound="lstvwTransportOverrideConfigDetails_ItemDataBound" 
                                                                    onsorting="lstvwTransportOverrideConfigDetails_Sorting">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" style="color: #333333" cellpadding="0" cellspacing="1"
                                                                            class="GridBorder" align="center">
                                                                            <tr runat="server" class="ClsGridHeader" id="trHeader">
                                                                                <th align="left" class="paddingL" width="300px">
                                                                                    <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                                     CausesValidation="false" ForeColor="Black" Text="Name"></asp:LinkButton>                                                                                    
                                                                                </th>
                                                                                <th align="center" class="paddingL" width="100px">
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="StartDate"
                                                                                     CausesValidation="false" ForeColor="Black" Text="Start Date"></asp:LinkButton>   
                                                                                </th>
                                                                                <th align="center" class="paddingL" width="100px">
                                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="EndDate"
                                                                                     CausesValidation="false" ForeColor="Black" Text="End Date"></asp:LinkButton>   
                                                                                </th>
                                                                                <th align="left" class="paddingL">                                                                                    
                                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="RouteName"
                                                                                     CausesValidation="false" ForeColor="Black" Text="Route"></asp:LinkButton>   
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="125px">
                                                                                    
                                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                                                     CausesValidation="false" ForeColor="Black" Text="Vehicle"></asp:LinkButton>   
                                                                                </th>
                                                                                <th align="left" class="paddingL" width="100px">
                                                                                    
                                                                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="TransportShiftName"
                                                                                     CausesValidation="false" ForeColor="Black" Text="Journey"></asp:LinkButton>   
                                                                                </th>
                                                                                <th align="left" style="width:200px;">
                                                                                   <span class="paddingL">Weekdays</span>
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="75px">
                                                                                    Copy
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="75px">
                                                                                    Edit
                                                                                </th>
                                                                                <th align="center" class="paddingLR" width="75px">
                                                                                    Delete
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="10">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTransportOverrideConfigDetails"
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
                                                                        <tr id="tr1" runat="server" class = '<%# Container.DisplayIndex % 2 == 0?"ClsGridRow":"ClsGridAltRow" %>'>
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
                                                                            <td align="center" class="paddingLR">
                                                                                <asp:Label ID="lblSourceVehicle" runat="server" Text='<%#Eval("SourceVehicle") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingLR">
                                                                                <asp:Label ID="lblSourceJourney" runat="server" Text='<%#Eval("SourceJourney") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblWeekdays" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td align="center" class="paddingLR">
                                                                                <asp:ImageButton ID="imgBtnCopy" runat="server" CausesValidation="false" CommandName="CopyCommand"
                                                                                    ToolTip="Copy" ImageUrl="../images/Icon_BookAdd.gif" />
                                                                            </td>
                                                                            <td align="center" class="paddingLR">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center" class="paddingLR">
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand"
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
                                                                <asp:ObjectDataSource ID="objOverrideConfigDetails" TypeName="BusinessLogic.TransportBL.TransportOverrideConfigDetailsBL"
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
                                                                        <asp:ControlParameter ControlID="txtName" Name="asName" Type="String" />
                                                                        <asp:ControlParameter ControlID="hidSortExpression" Name="SortExpression" Type="String" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidSortDirection" Name="SortDirection" Type="String" PropertyName="Value" />                                                                        
                                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <asp:HiddenField ID="hidId" runat="server" />
                                                                <asp:HiddenField ID="hidQueryString" runat="server" />
                                                                <asp:HiddenField ID="hidCategoryId" runat="server" />
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwTransportOverrideConfigDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="Back" CausesValidation="false" />
                                            <asp:Button ID="btnAdd" runat="server" CssClass="ClsBtn" Text="Add" 
                                                onclick="btnAdd_Click" />
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
    <script type="text/javascript">

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function OpenPopup(qst) {
            window.open('TransportConfigOverrideCopyPopup.aspx?' + qst, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=750,height=700')
        }

    </script>
</asp:Content>
