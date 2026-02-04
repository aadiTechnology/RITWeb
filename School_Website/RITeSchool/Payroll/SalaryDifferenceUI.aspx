<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SalaryDifferenceUI.aspx.cs" Inherits="SalaryDifferenceUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnlSalaryDetails" runat="server">
                        <ContentTemplate>
                            <table width="100%" align="center">
                                <tr>
                                    <td>
                                        <div style="float: right">
                                            <span class="ClsMdtStar">* Mandatory Fields </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Blue" Text=""
                                            Visible="false" CssClass="ClsLabelNrml" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table align="center" width="100%">
                                            <tr>
                                                <td align="center" class="ClsHilightBGB">
                                                    <span style="font-weight: bold">Select month and year to display salary difference with
                                                        respective to selected base month's and year's configuration.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trComboboxes" runat="server">
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <span class="ClsLabel">Year : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbMonthToCompare_SelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td align="right">
                                                    <span class="ClsLabel">Month : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonths" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbMonthToCompare_SelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <span class="ClsLabel">Base Year : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbYearToCompare" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbMonthToCompare_SelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td align="right">
                                                    <span class="ClsLabel">Base Month : </span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonthToCompare" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbMonthToCompare_SelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="ClsLabel">Name : </span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSearch" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtnMid remove-margin-top" OnClick="btnShow_Click" />
                                                </td>
                                            </tr>
                                            <tr style="height:40px;">
                                                <td colspan="4" align="center">
                                                    <asp:UpdatePanel ID="upnlLinks" runat="server">
                                                        <ContentTemplate>
                                                            <table id="tblLinks" runat="server" visible="false">
                                                                <tr>
                                                                    <td align="right" style="height: 25px" class="ClsGreenBG">
                                                                        <asp:LinkButton ID="lnkConfig" runat="server" Text="Earning / Deduction For Difference"
                                                                            CssClass="SubTitle"></asp:LinkButton>
                                                                    </td>
                                                                    <td align="right" style="height: 25px" class="ClsGreenBG">
                                                                        <asp:LinkButton ID="lnkDetails" runat="server" Text="Details of Saved / Paid Difference"
                                                                            CssClass="SubTitle"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbMonthToCompare" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="cmbYearToCompare" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="cmbMonths" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="cmbYear" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;">
                                    </td>
                                </tr>
                                <tr id="trLegend" runat="server" visible="false">
                                    <td align="center">
                                        <table id="tblLegent" runat="server">
                                            <tr>
                                                <td width="60px">
                                                    <span class="ClsLblLgnd">Legend : </span>
                                                </td>
                                                <td align="left">
                                                    <table>
                                                        <tr>
                                                            <td width="20px">
                                                                <asp:Label ID="Label2" runat="server" CssClass="ClsGridRow" Height="20px" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                            </td>
                                                            <td width="200px">
                                                                <asp:Label ID="Label3" runat="server" CssClass="ClsLblLgnd" Text="Selected Month's Configuration"></asp:Label>
                                                            </td>
                                                            <td width="20px">
                                                                <asp:Label ID="Label5" runat="server" BackColor="Wheat" Height="20px" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                            </td>
                                                            <td width="230px">
                                                                <asp:Label ID="Label6" runat="server" CssClass="ClsLblLgnd" Text="Selected Base Month's Configuration"></asp:Label>
                                                            </td>
                                                            <td width="20px">
                                                                <asp:Label ID="lblDefaultNoticeColor" runat="server" BackColor="SkyBlue" Height="20px"
                                                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                                    EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" CssClass="ClsLblLgnd" Text="Salary Difference"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdMonthListMessage" runat="server" align="left" colspan="3" visible="false">
                                                    <asp:Label ID="lblMonthList" runat="server" CssClass="ClsLblLgnd" Text=""></asp:Label>
                                                    <asp:LinkButton ID="lnkmonthList" runat="server" Text="" CssClass="ClsLabel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdCurrentSalaryMonthName" runat="server" align="left" colspan="3">
                                                    <asp:Label ID="lblCurrentSalaryMonth" runat="server" CssClass="ClsLblLgnd" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <table id="tblPageDetails" runat="server" width="180px" cellpadding="0" cellspacing="0"
                                            visible="false" style="vertical-align: top;" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblStartIndex" Text="1" runat="server" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">to</span>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">out of</span>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblTotalRecords" runat="server" CssClass="LblNrmlB" />
                                                </td>
                                                <td align="center">
                                                    <span class="LblNormal">records</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trGrid">
                                    <td align="center" visible="true" runat="server" id="tdGrid">
                                        <div id="divContainer" runat="server" class="GridBorder" visible="false" style="width: 800px;
                                            color: #D6E1B7; height: 540px; overflow: scroll">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdSalaryDifference" Width="100%" runat="server" DataKeyNames="UserId"
                                                            PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                            CssClass="GridBorder" OnRowDataBound="grdSalaryDifference_RowDataBound" OnRowCommand="grdSalaryDifference_RowCommand"
                                                            OnRowDeleting="grdSalaryDifference_RowDeleting" AllowPaging="True" PagerSettings-Position="Bottom"
                                                            EnableModelValidation="True">
                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                FirstPageText="First" Mode="NumericFirstLast"></PagerSettings>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Save">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnSaveSalary" runat="server" CausesValidation="false" CommandName="SAVE"
                                                                            CommandArgument="<%# Container.DataItemIndex %>" CssClass="ClsBtn" Text="Save"
                                                                            ToolTip="Save" disable-page="true" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnDeleteSalaryDifference" runat="server" CausesValidation="false"
                                                                            CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" CssClass="ClsBtn"
                                                                            Text="Delete" ToolTip="Delete" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                            </PagerStyle>
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                            <EmptyDataTemplate>
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                            <PagerTemplate>
                                                            </PagerTemplate>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table id="tblPager" runat="server" width="100%" cellpadding="0" cellspacing="0"
                                                            style="vertical-align: top;">
                                                            <tr>
                                                                <td align="left" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                    <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                        OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="100px" align="right" class="ClsBorderPager" valign="middle">
                                                                    <asp:Label ID="lblCurrentPage" runat="server" CssClass="LblNormal" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Label ID="lblNoRecordMessage" runat="server" Text="No record found." CssClass="LblNoRecord"
                                            Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id="tblNote" runat="server" visible="false">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                    <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note1 : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span style="border-width: 0px" class="LblSmlV">User can delete salary difference of
                                                        selected month only when it is saved and not paid.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                    <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note2 : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span style="border-width: 0px" class="LblSmlV">Delete All button will delete salary
                                                        difference of all the users.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                    <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note3 : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span style="border-width: 0px" class="LblSmlV">Save button will save salary difference
                                                        of all the users that are available on screen.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                    <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note4 : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span style="border-width: 0px" class="LblSmlV">For calculating salary difference of
                                                        any staff, staff should be present for at least a day in selected base month and
                                                        year.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                    <span style="border-width: 0px; font-weight: bold" class="LblNrmlB">Note5 : </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                    <span style="border-width: 0px" class="LblSmlV">Salary difference row (marked as blue)
                                                        shows only the unsaved salary difference amount for each of the earning and deduction.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" Enabled="false" disable-page="true"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnSaveAll" runat="server" Text="Save All" CssClass="ClsBtn" 
                                            Enabled="false" disable-page="true" onclick="btnSaveAll_Click" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete All" CssClass="ClsBtn" Enabled="false"
                                            CausesValidation="false" UseSubmitBehavior="false" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" Enabled="false"
                                            CausesValidation="false" UseSubmitBehavior="false" OnClick="btnExport_Click" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                        <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidMonthList" runat="server" Value="" />
                                        <asp:HiddenField ID="hidColumnIndexes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCurrentMonth" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidLateMarkLeaveIndex" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedMonth" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedYear" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedBaseYear" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedBaseMonth" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsReadyToPay" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidSalaryDifferenceCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSelectedPageIndex" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
        _clientbtnShow = "<%=this.btnShow.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnExport = "<%=this.btnExport.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>";
        _clientbtnDelete = "<%=this.btnDelete.ClientID %>";
        _clienthidSelectedMonth = "<%=this.hidSelectedMonth.ClientID %>"
        _clienthidSelectedYear = "<%=this.hidSelectedYear.ClientID %>";
        _clientcmbMonths = "<%=this.cmbMonths.ClientID %>";
        _clientcmbYear = "<%=this.cmbYear.ClientID %>";
        _clienthidIsReadyToPay = "<%=this.hidIsReadyToPay.ClientID %>";
        _clienthidSalaryDifferenceCount = "<%=this.hidSalaryDifferenceCount.ClientID %>";

        _clientlnkConfig = "<%=this.lnkConfig.ClientID %>";
        _clientlnkDetails = "<%=this.lnkDetails.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);


        function BeginRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            //DisableElements(true, postBackElement);

            showtooltip();
        }

        function EndRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            //DisableElements(false, postBackElement);
            AutoSearch();
        }

        function DisableElements(action, postBackElement) {
            if (document.getElementById(_clientbtnShow) != null)
                document.getElementById(_clientbtnShow).disabled = action;

            if (!(action == false && (postBackElement.id == _clientbtnSave ||
                    (postBackElement.id == _clientbtnShow && document.getElementById(_clienthidRowCount).value == 0)))) {
                if (document.getElementById(_clientbtnSave) != null && document.getElementById(_clienthidSalaryDifferenceCount).value != "0")
                    document.getElementById(_clientbtnSave).disabled = action;

                if (document.getElementById(_clientbtnExport) != null)
                    document.getElementById(_clientbtnExport).disabled = action;

                var isReadyToPay = document.getElementById(_clienthidIsReadyToPay).value;
                if (document.getElementById(_clientbtnDelete) != null && isReadyToPay == "Y")
                    document.getElementById(_clientbtnDelete).disabled = action;
            }
        }

        function OpenPaidSalaryDifferencePopup() {
            var queryString = document.getElementById(_clienthidQueryString).value;
            window.open('PaidSalaryDifferenceUI.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top = 50,left=50,width=800,height=600')
        }

        //HideButtons();

        function HideButtons() {
            btnSave = document.getElementById(_clientbtnSave);
            btnDelete = document.getElementById(_clientbtnDelete);
            btnExport = document.getElementById(_clientbtnExport);
            lnkConfig = document.getElementById(_clientlnkConfig);
            lnkDetails = document.getElementById(_clientlnkDetails);


            selectedMonth = document.getElementById(_clienthidSelectedMonth).value;
            selectedYear = document.getElementById(_clienthidSelectedYear).value;

            currentMonth = document.getElementById(_clientcmbMonths).value;
            currentYear = document.getElementById(_clientcmbYear).value;

            var isReadyToPay = document.getElementById(_clienthidIsReadyToPay).value;

            var disableControl = selectedMonth == currentMonth && selectedYear == currentYear;

            if (btnSave != null)
                btnSave.disabled = !disableControl;
            if (isReadyToPay == "Y") {
                if (btnDelete != null)
                    btnDelete.disabled = !disableControl;
            }
            if (btnExport != null)
                btnExport.disabled = !disableControl;

            if (lnkConfig != null)
                lnkConfig.disabled = !disableControl;

            if (lnkDetails != null)
                lnkDetails.disabled = !disableControl;
        }

        function openPopup(obj, queryString) {
            if (obj.disabled) {
                alert('Please show salary difference first.');
                return false;
            }
            window.open('SalaryDifferenceConfigPopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top = 50,left=50,width=800,height=600')
            return false;
        }

        function openDetailsPopup(obj, queryString) {
            if (obj.disabled) {
                alert('Please show salary difference first.');
                return false;
            }

            window.open('SavedSalaryDifferencePopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top = 50,left=50,width=800,height=600')
            return false;
        }

    </script>
    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-blink.js"></script>
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/Validate2.js"></script>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>
    <style type="text/css">
        .class2
        {
            border: 1;
        }
    </style>
    <script type="text/javascript">

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        width: 3,
                        radius: 5
                    },
                    tip: 'topRight',
                    width: 200
                },

                position: { adjust: { x: -210, y: 0} }
            });
        }

        showtooltip();        
    </script>

    <script type="text/javascript" src="../RITAutoCompleteService/RITAutoSuggest.js?version=1.4"></script>
	<script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>	
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"            
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 1);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnShow.ClientID %>");
            SearchResult(txt, val, bt);
        }

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
