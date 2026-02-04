<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RouteDetailsUI.aspx.cs" Inherits="RouteDetailsUI"
    Title="Untitled Page" %>
    
 <%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>
            <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                vertical-align: top">
                <tr>
                    <td>
                        <table id="tblRouteStopDetails" runat="server" align="center" border="0" cellpadding="0"
                            cellspacing="1" style="width: 100%; vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <!-- Data Insert Here -->
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Visible="false" Height="20px" Width="100%" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" class="ClsTextNormal" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" id="tdMessage" runat="server" colspan="2">
                                                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                                    Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <!-- User InfoTable ListView -->
                            <tr>
                                <td align="center">
                                    <table id="tblStop" runat="server" align="center">
                                        <tr align="center">
                                            <td class="ClsTextNormal" align="center">
                                                <table id="tblRouteDetails" runat="server" border="0" cellpadding="1" cellspacing="2">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Route No :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtRouteNo" runat="server"></asp:TextBox>
                                                            <span id="sRouteNo" runat="server" class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="ReqRouteNo" runat="server" ControlToValidate="txtRouteNo"
                                                                 Display="None" ErrorMessage="Route No should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblRouteName" runat="server" CssClass="ClsLabel" Text="Route Name :"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtRouteName" runat="server" MaxLength="100" CssClass="LrgTxtBox"
                                                                Width="186px"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqRouteNamer" runat="server" ControlToValidate="txtRouteName"
                                                                Display="None" ErrorMessage="Route Name should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Journey Type :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:DropDownList ID="ddlJourneyType" runat="server" CssClass="MidCombo" Width="50%">
                                                                 <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                                 <asp:ListItem Value="1" Text="Pickup"></asp:ListItem>
                                                                 <asp:ListItem Value="2" Text="Drop"></asp:ListItem>
                                                             </asp:DropDownList>
                                                             <span id="sJourneyType" runat="server" class="ClsMdtStar">*</span>
                                                             <asp:RequiredFieldValidator ID="ReqJourneyType" runat="server" ControlToValidate="ddlJourneyType"
                                                                 Display="None" ErrorMessage="Journey type should be selected." InitialValue="0"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Start Time :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="SmlTxtBox" placeholder="hh:mm am/pm"> </asp:TextBox>                                                            
                                                            <span id="sStarTime" runat="server" class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="ReqStartTime" runat="server" ControlToValidate="txtStartTime"
                                                                 Display="None" ErrorMessage="Start time should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cstStartTimeFormat" runat="server" ClientValidationFunction="ValidateStartTimeFormat"
                                                             SetFocusOnError="true" Display="None" ErrorMessage="Please select valid Start time format"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">End Time :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="SmlTxtBox" placeholder="hh:mm am/pm"> </asp:TextBox>
                                                            <span id="sEndTime" runat="server" class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="ReqEndTime" runat="server" ControlToValidate="txtEndTime"
                                                                 Display="None" ErrorMessage="End time should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cstEndTimeFormat" runat="server" ClientValidationFunction="ValidateEndTimeFormat"
                                                             SetFocusOnError="true" Display="None" ErrorMessage="Please select valid End time format" ></asp:CustomValidator>
                                                             <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateStartAndEndTime"
                                                             SetFocusOnError="true" Display="None" ErrorMessage="" ></asp:CustomValidator>
                                                         </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Add route map picture :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="fileUploadItems" runat="server" ToolTip="Only PDF,PNG and JPG files are allowed" />                                      
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <span class="lblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3 MB.)
                                                             </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 600px">
                                                <div id="divContainer" class="GridBorder" runat="server" visible="true" style="width: 75%;
                                                    height: 300px; overflow: scroll">
                                                    <asp:ListView ID="lstvwStops" runat="server" DataKeyNames="miStopId,miRouteStopId,miRouteShiftTimingDetailsId">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="10%">
                                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                    </th>
                                                                    <th align="left" width="80%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lnkBtnSortName" runat="server" CausesValidation="false" ForeColor="Black">Stop Name </asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td class="paddingL" align="left">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSave" runat="server">
                                <td align="center">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                       OnClientClick="if(!IsFileValid())return false;"  OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                </td>
                            </tr>
                            <tr id="trDataPager" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwRouteStopAsso">
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
                            <tr>
                                <td align="center">
                                    <table width="60%">
                                        <tr style="width: 100%">
                                            <td style="width: 100%">
                                                <asp:ListView ID="lstvwRouteStopAsso" runat="server" DataKeyNames="miRouteId" DataSourceID="ObjDSRouteStopDetails"
                                                    OnItemCommand="lstvwRouteStopAsso_ItemCommand" OnDataBound="lstvwRouteStopAsso_DataBound"
                                                    OnSorting="lstvwRouteStopAsso_Sorting" OnItemDataBound="lstvwRouteStopAsso_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tbllstvwRouteStopInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" width="25%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkBtnSortRouteName" runat="server" CommandName="Sort" CommandArgument="RouteName"
                                                                        CausesValidation="false" ForeColor="Black"> Route Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="40%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkBtnStops" runat="server" CommandName="Sort" CommandArgument="Stops"
                                                                        CausesValidation="false" ForeColor="Black"> Associated Stops</asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="15%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkBtnVehicleName" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                                        CausesValidation="false" ForeColor="Black"> Vehicle Number </asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    Edit
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    View
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="6">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwRouteStopAsso"
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
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblRouteName" runat="server" Text='<%# Eval("msRouteName") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblStops" runat="server" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblVehicleNo" runat="server" Text='<%# Eval("miVehicleNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" ToolTip="View"
                                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblRouteName" runat="server" Text='<%# Eval("msRouteName") %>'></asp:Label>
                                                            </td>
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblStops" runat="server" Text='<%# Eval("msStopName") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblVehicleNo" runat="server" Text='<%# Eval("miVehicleNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" ToolTip="View"
                                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                &nbsp;
                                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ObjectDataSource TypeName="BusinessLogic.RouteDetailsBL" EnablePaging="True"
                            ID="ObjDSRouteStopDetails" runat="server" SelectMethod="GetAllRouteStopAsso"
                            SortParameterName="sortExpression" SelectCountMethod="CountTotalRouteStopAsso"
                            EnableCaching="False">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                    Type="int32" />
                                <asp:Parameter Name="sortExpression" Type="String" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidRouteId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidFilePath" runat="server" />
                        <asp:CustomValidator ID="CstStop" runat="server" ClientValidationFunction="CheckAtListOne"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript" language="javascript">


        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientlstvwlstvwStops = "<%=this.lstvwStops.ClientID %>"
        _ClientChkAll = _clientlstvwlstvwStops + "_ChkSelectAll";
        _clientCstStop = "<%=this.CstStop.ClientID %>"
        _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>"; //upload    hidFilePath
        _ClientlblSuccess = "<%=this.lblSuccess.ClientID %>"; //upload
        _ClientlblError = "<%=this.lblError.ClientID %>"; //upload
                
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }


        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwlstvwStops + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwlstvwStops + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }


        function CheckAtListOne(oSrc, args) {
            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientlstvwlstvwStops + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwlstvwStops + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
                $get(_clientCstStop).errormessage = "At least one stop should be selected for route."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ResetUpdateLbl() {

            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlblErrorMsg) != null) {
                document.getElementById(_clientlblErrorMsg).style.display = "none"
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            }

        }
        //Upload file source start here

        function OpenWindow(sfilepath) {
            window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            return false;
        }
        
        //This function is used to validate is file uploaded by user or not
        function IsFileValid() {
            if (document.getElementById(_ClientlblSuccess)) {
                document.getElementById(_ClientlblSuccess).innerHTML = "";
                document.getElementById(_ClientlblSuccess).innerText = "";
            }

            if (document.getElementById(_ClientlblError)) {
                document.getElementById(_ClientlblError).innerHTML = "";
                document.getElementById(_ClientlblError).innerText = "";
            }

            var lblUFileNameval = "";
            var myImage = document.getElementById(_ClientfileUploadItems).value;
            var file = document.getElementById(_ClientfileUploadItems);

            if (myImage == "" || myImage == null) {
                return true;
            }
            else {
                var ext = myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase()

                if (ext == ".PDF" || ext == ".PNG" || ext == ".JPG" || ext == ".JPEG" || ext == ".BMP") {

                    if (file.value != '' && file.files[0].size >= 3072000) {
                        alert("File size should be less than 3 MB.");
                        return false
                    }
                    else
                        return true;
                }
                else {
                    alert('Invalid file type.');
                    return false;
                }
            }
        }

        function ValidateStartTimeFormat(osrc, args) {

            var tStartTime = document.getElementById(_ClienttxtStartTime)

            if (tStartTime.value != '') {
                if (!isTimeFormatValid(_ClienttxtStartTime)) {
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateEndTimeFormat(osrc, args) {

            var tEndTime = document.getElementById(_clienttxtEndTime)

            if (tEndTime.value != '') {
                if (!isTimeFormatValid(_clienttxtEndTime)) {
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateStartAndEndTime(osrc, args) {        
            var tStartTime = document.getElementById(_ClienttxtStartTime)
            var tEndTime = document.getElementById(_clienttxtEndTime)

            if (tStartTime.value != '' && tEndTime.value !='') {
                if (isTimeFormatValid(_ClienttxtStartTime) && isTimeFormatValid(_clienttxtEndTime)) {

                    var stStartDate = '01-Jan-2022'

                    var StartDt;
                    
                    if (document.all) 
                        StartDt = new Date(stStartDate.replace('-', ' ') + " " + tStartTime.value);
                    else
                        StartDt = new Date(convertdate(stStartDate) + " " + tStartTime.value);

                    var endDt;
                    if (document.all)
                        endDt = new Date(stStartDate.replace('-', ' ') + " " + tEndTime.value);
                    else
                        endDt = new Date(convertdate(stStartDate) + " " + tEndTime.value);

                    if (StartDt >= endDt) {
                        osrc.errormessage = 'End Time should be greater than Start Time.'
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            args.IsValid = true;
            return false;
        }


        function isTimeFormatValid(result) {            
            var timeStr = document.getElementById(result).value;
            timeStr = timeStr.toUpperCase();

            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }

    </script>
</asp:Content>
