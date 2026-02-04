<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TravelerTransportDetailsUI.aspx.cs" Inherits="TravelerTransportDetailsUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="MainBodyDiv">
                <table width="98%" align="center">
                    <tr>
                        <td align="center" valign="top">
                            <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trMandetory" runat="server">
                        <td align="right" style="color: #ff3333" valign="top">
                            <span class="ClsMdtStar">* Mandatory Fields </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="Save" runat="server"
                                CssClass="lblNormal" />
                            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CustomValidator ID="cstvalListViewValidations" runat="server" ClientValidationFunction="ListViewValidations"
                                SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="You have entered duplicate value for selected Assessment sort order."></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="100%" align="center">
                                <tr id="trFilters" runat="server">
                                    <td width="100%">
                                        <table align="center" cellpadding="1" cellspacing="2" width="85%">
                                            <tr>                                               
                                                <td valign="middle" class="ClsBorderlight" style="width: 17%">
                                                    <span class="ClsLabel">User Role :</span>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="cmbUserRole" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td id="lblStandard" runat="server" align="left" visible="false" class="ClsBorderlight">
                                                    <asp:Label ID="Label1" Width="110px" runat="server" Text="Standard : " CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td align="left" style="width:100px">
                                                    <asp:DropDownList ID="ddlStandard" runat="server" Visible="false" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="lblDivision" style="width:80px" runat="server" align="center" class="ClsBorderlight" visible="false">
                                                    <span class="ClsLabel">Division :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlDivision" runat="server" Visible="false" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>                                               
                                                <td class="ClsBorderlight" valign="middle" style="width: 10%">
                                                    <%--<span class="ClsLabel">Route :</span>--%>
                                                    <asp:Label ID="lblRoute" CssClass="ClsLabel" runat="server" Text="Route :"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" style="width: 160px">
                                                    <asp:DropDownList ID="cmbRoute" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbRoute_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblcmbRouteMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                        Text="*" Width="14px" Visible="False"></asp:Label>
                                                </td>
                                                <td class="ClsBorderlight" style="width:120px" id="tdlblSearch" runat="server"> 
                                                    <asp:Label ID="lblSearch" Width="110px" runat="server" Text="Name / Reg. No. : " CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td id="tdSearch" runat="server" colspan="3" style="width:280px">
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="LrgTxtBox" autocomplete="off"></asp:TextBox>                                                    
                                                
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click">
                                                    </asp:Button>
                                                </td>
                                            </tr>                                            
                                            <tr>                                               
                                                <td class="ClsBorderlight" valign="middle" style="width: 10%">
                                                    <span class="ClsLabel">Stop :</span>
                                                </td>
                                                <td align="left" valign="top" style="width: 160px">
                                                    <asp:DropDownList ID="cmbStop" runat="server" CssClass="MidCombo" AutoPostBack="True" onselectedindexchanged="cmbStop_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblcmbStopMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                        Text="*" Width="14px" Visible="False"></asp:Label>
                                                </td>
                                            </tr>                                            
                                            <tr>                                                
                                                <td class="ClsBorderlight" valign="middle" style="width: 10%">
                                                    <span id="spnShiftHeader" runat="server" class="ClsLabel">Shift :</span>
                                                </td>
                                                <td align="left" valign="top" style="width: 160px">
                                                    <asp:DropDownList ID="cmbShift" runat="server" CssClass="MidCombo" 
                                                        AutoPostBack="True" onselectedindexchanged="cmbShift_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblShiftMandMark" runat="server" CssClass="ClsMdtStar" Height="14px"
                                                        Text="*" Width="14px" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>                                                
                                                <td class="ClsBorderlight" valign="top" colspan="3">
                                                    <asp:CheckBox AutoPostBack="True" ID="chkIncludeAll" Text="Show Travelers who have not associated to transport." OnClick="ResetOthers()"
                                                        runat="server" Visible="true" Checked="true" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr id="trNote" runat="server">
                                            <td align="left" colspan="4">
                                                <span class="LblNrmlB ClsBorderlight" style="background-color: #ffffc4;">
                                                    Note :</span> <span class="ClsBorderlight" style="font-family: Verdana; font-size: 8pt;
                                                        border: 100%;">All the Transport details are shown in the format of pickup details / drop details.</span>
                                            </td>
                                        </tr>                                           
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="50" PagedControlID="lstvwTravelersDetails">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr id="trlstvwTransport" runat="server">
                                    <td align="center">
                                        <table id="tblTransportDetails" runat="server" align="center" width="98%">
                                            <tr align="center" style="width: 100%">
                                                <td align="center" style="width: 100%">                                                   
                                                    <asp:ListView ID="lstvwTravelersDetails" runat="server" DataSourceID="ObjDSTravelersDetails"
                                                        OnDataBound="lstvwTravelersDetails_DataBound" OnItemDataBound="lstvwTravelersDetails_ItemDataBound"
                                                        DataKeyNames="TravelerTransportId,UserId,IsHistoryExists,ClassName">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblTravlerInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="50px" style="padding-left: 9px;">
                                                                        <asp:Label ID="lblSrNo" runat="server" CausesValidation="false" ForeColor="Black">Sr. No. </asp:Label>
                                                                    </th>
                                                                    <th align="left" width="18%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lnkSortName" runat="server" CausesValidation="false" ForeColor="Black">Travelers Name </asp:Label>
                                                                    </th>
                                                                    <th align="left" width="18%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lblAddress" runat="server" CausesValidation="false" ForeColor="Black">Address </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="150px" style="padding-left: 9px;">
                                                                        <asp:Label ID="lblMobile1" runat="server" CausesValidation="false" ForeColor="Black">Mobile Number1 </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="150px"  style="padding-left: 9px;" id="thMobieNo" runat="server">
                                                                        <asp:Label ID="lblMobile2" runat="server" CausesValidation="false" ForeColor="Black">Mobile Number2 </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="9%">
                                                                        <asp:Label ID="lblRoute" runat="server" CausesValidation="false" ForeColor="Black">Route</asp:Label>
                                                                    </th>
                                                                    <th align="center" width="9%">
                                                                        <asp:Label ID="lblStop" runat="server" CausesValidation="false" ForeColor="Black">Stop </asp:Label>
                                                                    </th>
                                                                    <th align="center" width="8%" id="thShift" runat='server'>
                                                                        <asp:Label ID="lblShift" runat="server" CausesValidation="false" ForeColor="Black">Shift</asp:Label>
                                                                    </th>
                                                                    <th align="center" width="9%">
                                                                        <asp:Label ID="lblVehicle" runat="server" CausesValidation="false" ForeColor="Black">Vehicle Number</asp:Label>
                                                                    </th>
                                                                     <th align="center" width="75px">
                                                                        <asp:Label ID="Label2" runat="server" CausesValidation="false" ForeColor="Black">Assign</asp:Label>
                                                                    </th>
                                                                    <th align="center" width="100px">
                                                                        <asp:Label ID="Label3" runat="server" CausesValidation="false" ForeColor="Black">Mark As Left</asp:Label>
                                                                    </th>
                                                                    <th align="center">
                                                                        <asp:Label ID="Label4" runat="server" CausesValidation="false" ForeColor="Black">History</asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="12">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTravelersDetails"
                                                                            PageSize="50">
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
                                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblRowNo" runat="Server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                </td>
                                                                 <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                                </td>                                                                
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblMobile1" runat="server" Text='<%# Eval("MobileNumber1") %>'></asp:Label>
                                                                </td>                                                                
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblMobile2" runat="server" Text='<%# Eval("MobileNumber2") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblRouteName" runat="server" Text='<%# Eval("RouteName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblStopName" runat="server" Text='<%# Eval("StopName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblShiftName" runat="server" Text='<%# Eval("ShiftName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">                                                                    
                                                                    <asp:Label ID="lblVehicleNumber" runat="server" Text='<%# Eval("VehicleName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                 <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Assign" ToolTip="Click here to assign transport details."
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />                                                                
                                                                </td>
                                                                <td align="center">
                                                                     <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                </td>
                                                                 <td align="center" id="tdView" runat="server" viewstatemode="Enabled">
                                                                    <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                                        CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                                        ToolTip="View" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>                                                       
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No record found.
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.TravelerTransportDetailsBL" EnablePaging="True"
                                            ID="ObjDSTravelersDetails" runat="server" SelectMethod="GetTravelersDetails"
                                            SortParameterName="sortExpression" SelectCountMethod="CountTravelersDetails"
                                            EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="cmbUserRole" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiUserRoleId" />
                                                <asp:ControlParameter ControlID="cmbRoute" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiRouteId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="cmbStop" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiStopId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="cmbShift" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiTransportShiftId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="ddlStandard" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiStandardId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="ddlDivision" PropertyName="SelectedValue" Type="Int32"
                                                    Name="aiDivisionId" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="chkIncludeAll" PropertyName="Checked" Type="Int32"
                                                    Name="aiIncludeNotAssociated" />
                                                <asp:ControlParameter Name="asCriteria" Type="String" ControlID="txtSearch" PropertyName="Text" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />                                                
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidRowCount" runat="server" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" />
                                        <asp:HiddenField ID="hidStdId" runat="server" />
                                        <asp:HiddenField ID="hidDivId" runat="server" />
                                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSms" Text="Send Sms/Message" CssClass="ClsBtn" runat="server" TabIndex="9" />
                                        <asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" CausesValidation="false" onclick="btnExport_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidPageNo" runat="server" />
                <asp:HiddenField ID="hidSetQueryString" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
 

    <script language="javascript" type="text/javascript">
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";
        _clientlstvwTravelersDetails = "<%=this.lstvwTravelersDetails.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clientcstvalListViewValidations = "<%=this.cstvalListViewValidations.ClientID %>"        
        _clientbtnSms = "<%=this.btnSms.ClientID %>"
        _clientbtnSearch = "<%=this.btnSearch.ClientID %>"
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        _clientcmbRoute = "<%=this.cmbRoute.ClientID %>"
        _clientcmbStop = "<%=this.cmbStop.ClientID %>"
        _clientcmbShift = "<%=this.cmbShift.ClientID %>"
        _clientchkIncludeAll = "<%=this.chkIncludeAll.ClientID %>"
        _clienthidUserId = "<%=this.hidUserId.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)

        function EndReqHandler(sender, args) {
            DisableControls(true)
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnSms) {
                var sQuerryString = document.getElementById(_clienthidQueryString).value
                window.open('../Transport/TravelerSmsPopup.aspx?' + sQuerryString + '\',\'_blank\',\'left=0, top=0, height=500, width=670, resizable= no, scrollbars= yes').focus();
            }
            AutoSearch();
        }

        function beginRequestHandler(sender, args) {
            DisableControls(false)
        }

        function ResetOthers() {
            if ($get(_clientchkIncludeAll) != null && $get(_clientchkIncludeAll).checked) {
                if ($get(_clientcmbRoute) != null && !$get(_clientcmbRoute).disabled)
                    $get(_clientcmbRoute).value = "0"

                if ($get(_clientcmbStop) != null && !$get(_clientcmbStop).disabled)
                    $get(_clientcmbStop).value = "0"

                if ($get(_clientcmbShift) != null && !$get(_clientcmbShift).disabled)
                    $get(_clientcmbShift).value = "0"
            }        
        }

        function DisableControls(flag) {
            if (document.getElementById(_clientbtnSms) != null)
                document.getElementById(_clientbtnSms).disabled = !flag;

            if (document.getElementById(_clientbtnSearch) != null)
                document.getElementById(_clientbtnSearch).disabled = !flag;
        }

        //This function is used to display message when page index will be changed.
        function MessageAboutUpload(oCmb) {
            var bIsValid;
            if (window.confirm('If you change the page then selected transport details from current page will get lost. Do you want to continue?'))
                bIsValid = true;
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false;
            }
            return bIsValid;
        }

        function ListViewValidations(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var sMsg = "";
            var iRowNo = "";

            for (var RowNumber = 0; RowNumber < iRowCount; RowNumber++) {
                var cmbRouteName = document.getElementById(_clientlstvwTravelersDetails + "_ctrl" + RowNumber + "_cmbRouteName").value;
                if (cmbRouteName != 0) {
                    var cmbStopName = document.getElementById(_clientlstvwTravelersDetails + "_ctrl" + RowNumber + "_cmbStopName").value;
                    var cmbShiftName = document.getElementById(_clientlstvwTravelersDetails + "_ctrl" + RowNumber + "_cmbShiftName").value;
                    var cmbVehicle = document.getElementById(_clientlstvwTravelersDetails + "_ctrl" + RowNumber + "_cmbVehicle").value;
                    if (cmbStopName == 0 || cmbShiftName == 0 || cmbVehicle == 0)
                        iRowNo = iRowNo + "," + (RowNumber + 1);
                }
            }
            if (iRowNo != "") {
                iRowNo = iRowNo.substring(1, iRowNo.length);
                oSrc.errormessage = "Stop, shift and vehicle number should be selected for row(s) : " + iRowNo + ".";
                document.getElementById(_clientcstvalListViewValidations).innerText = "Stop, shift and vehicle number should be selected for row(s): " + iRowNo + ".";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function OpenSendSMSPopup(sQueryString) {
            
            window.open('TravelerSmsPopup.aspx?' +
                sQueryString, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600').focus();
            return false;
        }

        function OpenRouteStopAssignment() {
            var SelectedQry = "";
            window.open("../Transport/RouteStopAssignmentPopup.aspx?" + SelectedQry, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
        }

        function OpenPopup(sQueryString) {
            window.open(sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
            return false;
        }

        function OpenLeftPopup() {
            var QryString = document.getElementById(_clienthidQueryString).value;            
            window.open('MarkLeftForTransportPopup.aspx?' + QryString, '_new', 'scrollbars=yes,resizable=no,top=100,left=100,width=500,height=400')
        } 
    </script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            _clientddlUserRole = '<%=cmbUserRole.ClientID%>';
            var _clientddlStandard = '<%=ddlStandard.ClientID%>';
            var _clientddlDivision = '<%=ddlDivision.ClientID%>';

            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _slienttxtUserName, _clientddlUserRole, 0, _clientddlStandard, _clientddlDivision, null);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
