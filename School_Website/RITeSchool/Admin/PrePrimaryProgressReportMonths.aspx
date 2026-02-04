<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PrePrimaryProgressReportMonths.aspx.cs" Inherits="PrePrimaryProgressReportMonths" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%; vertical-align: top">
		<tr>
			<td>
				<div id="divErr" runat="server">
					<table class="LblNoRecord" width="100%"  cellpadding="0" cellspacing="0">
						<tr>
							<td class="ClsConfigText">
								Please configure Pre Primary standards for School :
							</td>
						</tr>
						<tr>
							<td>
								<a class="ClsConfigLink" href="StandardsList.aspx">Standards</a>
							</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
        <tr id="trRow1" runat="server">
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                            vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <!--Insert Data Here-->
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 86%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label></asp:Panel>
                                                        </td>
                                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 86%">
                                                            <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                                                <asp:Label ID="lblCheckDependency" Style="text-align: left" runat="server" ForeColor="Red"
                                                                     Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 86%">
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                                                Width="703px"  />
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
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%"
                                        Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" class="ClsBorderLight">
                                                <span class="ClsLabel">Standard :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="margin-left: 35px; width: 122px;">
                                                <asp:DropDownList ID="ddlStandard" runat="server" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged"
                                                    AutoPostBack="true" CssClass="SmlCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                            </td>
                                            <td align="left">
                                                <div id="divSortOrder" runat="server" align="center" class="ToprLinkHlilight" style="width: 135px;
                                                    height: 18px;" visible="false">
                                                    <asp:HyperLink ID="hlnkSortOrder" runat="server" CssClass="ClsHilightTextB" NavigateUrl="SortMonthsPopup.aspx"
                                                        Target="_blank">Month Sort Order</asp:HyperLink>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trRow2" runat="server" align="center">
            <td align="center">
                <asp:UpdatePanel runat="server" ID="upnl2">
                    <ContentTemplate>
                        <table id="tblMonthList" runat="server" style="width: 700px">
                            <tr align="center" >
                                <td align="center" style="width: 700px">
                                    <asp:ListView ID="lstvwConfigureMonth" runat="server" DataKeyNames="MonthId,Month,MonthAbbreviation,PrePrimaryProgressReportMonthId,CommentAbbreviation"
                                        OnItemDataBound="lstvwConfigureMonth_ItemDataBound">
                                        <LayoutTemplate>
                                            <table align="center" width="700px" runat="server" id="tblStopInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" width="50px" style="padding-left: 9px;">
                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()"></asp:CheckBox>
                                                    </th>
                                                    <th align="center" width="100px" >
                                                         Month
                                                    </th>
                                                    <th align="center" style="width:100px;">
                                                        Abbreviation
                                                    </th>
                                                    <th align="center" style="width:200px;">
                                                        Is Comment Applicable?
                                                    </th>
                                                    <th align="center" style=" width: 150px">
                                                        Comment Header
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                <td align="left"  width="50px" class="paddingL">
                                                    <asp:CheckBox ID="ChkSelect" runat="server"></asp:CheckBox>
                                                </td>
                                                <td align="left" style=" width:80px;" class="paddingL" >
                                                    <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                                </td>
                                                <td align="center" >
                                                    <asp:TextBox ID="txtAbbreviation" runat="server" Text='<%# Eval("MonthAbbreviation") %>'
                                                        MaxLength="3" />
                                                </td>
                                                <td align="center" >
                                                    <asp:CheckBox ID="chkComment" runat="server"></asp:CheckBox>
                                                </td>
                                                <td align="center" style=" padding-left: 9px;" >
                                                    <asp:TextBox ID="txtComment" runat="server" Text='<%# Eval("CommentAbbreviation") %>' MaxLength="100"  />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridRow">
                                                <td align="left" width="50px" class="paddingL">
                                                    <asp:CheckBox ID="ChkSelect" runat="server"></asp:CheckBox>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblMonth" style="" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                                </td>
                                                <td align="center" >
                                                    <asp:TextBox ID="txtAbbreviation" runat="server" Text='<%# Eval("MonthAbbreviation") %>'
                                                        MaxLength="3" />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkComment" runat="server"></asp:CheckBox>
                                                </td>
                                                <td align="center" style="padding-left: 9px;">
                                                    <asp:TextBox ID="txtComment" runat="server" Text='<%# Eval("CommentAbbreviation") %>' MaxLength="100"  />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trButton" align="center" runat="server">
            <td align="center">
                
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" BorderWidth="1px"
                            CausesValidation="true" UseSubmitBehavior="false" OnClick="btnSave_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                            CausesValidation="False" OnClick="btnBack_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CstMonth" runat="server" ClientValidationFunction="CheckAtListOne"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                <asp:CustomValidator ID="cstMonthAbbr" runat="server" ClientValidationFunction="ValidateMonths"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                <asp:CustomValidator ID="cstDuplicate" runat="server" ClientValidationFunction="DuplicateValue"
                    SetFocusOnError="True" Display="None" ErrorMessage="You have enetered duplicate values for abbreviation."></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientlstvwMonths = "<%=this.lstvwConfigureMonth.ClientID %>"
        _ClientChkAll = _clientlstvwMonths + "_ChkSelectAll";
        _clientCstMonth = "<%=this.CstMonth.ClientID %>"
        _clientcstMonthAbbr = "<%=this.cstMonthAbbr.ClientID %>"

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function CloseWindow() {
            window.close()
        }

        function ValidateMonths(aSrc, args) {
            var chk
            var chk1
            var sMessage = false
            var sMessage1 = false 
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            chk1 = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_chkComment")
            while (chk != null) {
                if (chk.checked == true) {
                    txtAbbreviation = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_txtAbbreviation")
                    if (txtAbbreviation.value.trim() == "")
                        sMessage = true
                    if (chk1.checked == true) {
                        txtComment = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_txtComment")
                        if (txtComment.value.trim() == "")
                            sMessage1 = true
                    }          
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
                chk1 = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_chkComment")
            }
            if (sMessage == true) {
                $get(_clientcstMonthAbbr).errormessage = "Selected months abbreviation should not be blank."
                args.IsValid = false
                return true
            }
            else if (sMessage1 == true) {
                $get(_clientcstMonthAbbr).errormessage = "Selected comment header should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function DuplicateValue(oSrc, args) {
            if (DuplicateText(document, _clientlstvwMonths, "_ChkSelect", "_txtAbbreviation")) {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }


        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }


        function CheckAtListOne(oSrc, args) {
            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
                $get(_clientCstMonth).errormessage = "At least one month should be selected."
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
    </script>

</asp:Content>
