<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="XseedThemesUI.aspx.cs" Inherits="XseedThemesUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 800px;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 900px;">
                            <tr>
                                <td style="width: 700px">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="700px">
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                            Width="12%" CssClass="ClsMdtStar" Height="16px"></asp:Label>
                                        <asp:RequiredFieldValidator CssClass="ClsLbl" ID="reqValStandard" runat="server"
                                            ControlToValidate="cmbStandard" InitialValue="0" Display="None" ValidationGroup="Save"
                                            ErrorMessage="Standard should be selected."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqValAssessment" runat="server" ControlToValidate="cmbAssessment"
                                            CssClass="ClsLabel" Display="None" ValidationGroup="Save" ErrorMessage="Assessment should be selected."
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqTheme" runat="server" ControlToValidate="txtTheme"
                                            CssClass="ClsLabel" Display="None" ErrorMessage="Theme should not be blank."
                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqSortOrder" runat="server" ControlToValidate="txtSortOrder"
                                            CssClass="ClsLabel" Display="None" ErrorMessage="Sort order should not be blank."
                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cstValTheme" runat="server" ControlToValidate="txtSortOrder"
                                            ClientValidationFunction="validateTheme" CssClass="ClsMdtStar" Display="None"
                                            EnableClientScript="true" ErrorMessage="Error msg" ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDuplicateTheme" runat="server" ControlToValidate="txtTheme"
                                            ClientValidationFunction="validateDuplicateTheme" CssClass="ClsMdtStar" Display="None"
                                            EnableClientScript="true" ErrorMessage="Error msg" ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstvalDuplicateSortOrder" runat="server" ClientValidationFunction="DuplicateSortOrder"
                                            SetFocusOnError="True" ValidationGroup="Save" Display="None"></asp:CustomValidator>
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" enableviewstate="false" style="padding-right: 10px;
                                    top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="ClsLabel" ShowSummary="true" Width="616px" />
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 111%;">
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 900px;">
                <asp:Label ID="lblUpdateSucess" runat="server" CssClass="ClsLabelUpdate" EnableViewState="False"
                    Font-Bold="True" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table runat="server" cellpadding="1" cellspacing="2" align="center">
                    <tr style="padding-left: 10px;">
                        <td align="left" class="ClsBorderlight" width="150px">
                            <span class="ClsLabel" style="padding-left: 10px;">Standard :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbStandard" class="MidCombo" runat="server" Height="22px"
                                Width="150px" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">&nbsp;*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel" style="padding-left: 10px;">Assessment :</span>
                        </td>
                        <td>
                           
                            <asp:DropDownList class="MidCombo" ID="cmbAssessment" runat="server" Height="22px"
                                Width="150px" AutoPostBack="True" OnSelectedIndexChanged="cmbAssessment_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">&nbsp;*</span>
                           
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel" style="padding-left: 10px;">Theme :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTheme" runat="server" MaxLength="100" CssClass="SmlTxtBox" AutoPostBack="false"
                                Height="50px" Width="320px" TextMode="MultiLine"></asp:TextBox>
                            <span class="ClsMdtStar">&nbsp;*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel" style="padding-left: 10px; height: 14px;">Sort Order :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSortOrder" runat="server" oonkeyup="extractNumber(this,0,false);"
                                MaxLength="3" CssClass="SmlTxtBox" AutoPostBack="false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                onpaste="event.returnValue=false" Height="20px" Width="150px"></asp:TextBox>
                            <span class="ClsMdtStar">&nbsp;*</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="width: 900px;">
            <td>
            </td>
        </tr>
        <tr style="width: 900px;" align="center">
            <td align="center" colspan="2" style="height: 42px">
                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                    ValidationGroup="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="tblThemes" align="center">
                    <tr>
                        <td>
                            <asp:ListView ID="lstvwThemeDetails" runat="server" DataKeyNames="StandardwiseAssessmentId,ThemeId,Is_Deleted,StandardId"
                                OnItemCommand="lstvwThemeDetails_ItemCommand" OnItemDataBound="lstvwThemeDetails_ItemDataBound"
                                OnSorting="lstvwThemeDetails_Sorting">
                                <LayoutTemplate>
                                    <table align="center" width="850px" runat="server" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th style="width: 50px;">
                                                <asp:Label ID="lblSrNo" runat="server" Text="Sr. No."></asp:Label>
                                            </th>
                                            <th align="center" width="100px">
                                                <asp:Label ID="lblStandard" runat="server" Text="Standard"></asp:Label>
                                            </th>
                                            <th align="left" width="200px" style="padding-left: 10px;">
                                              <asp:Label ID="lblAssessmentName" runat="server" Text="Assessment Name"></asp:Label>
                                           </th>
                                            <th align="left" style="padding-left: 10px;" width="300px">
                                                <asp:LinkButton ID="lnkBtnTheme" runat="server" CommandArgument="Theme" CommandName="Sort"
                                                    CausesValidation="false" ForeColor="Black">Theme</asp:LinkButton>
                                            </th>
                                            <th align="center" width="120px">
                                                <asp:LinkButton ID="lnkBtnSortOrder" runat="server" CommandArgument="SortOrder" CommandName="Sort"
                                                    CausesValidation="false" ForeColor="Black">Sort Order</asp:LinkButton>
                                            </th>
                                            <th align="center" width="50px">
                                                Edit
                                            </th>
                                            <th align="center" width="50px">
                                                Delete
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblAssessmentName" runat="server" Text='<%# Eval("AssessmentName") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblTheme" runat="server" Text='<%# Eval("Theme") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:Label ID="lblSrNo" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblStandard" runat="server" Text='<%# Eval("StandardName") %>'></asp:Label>
                                        </td>
                                        <td class="paddingL" align="left">
                                            <asp:Label ID="lblAssessmentName" runat="server" Text='<%# Eval("AssessmentName") %>'></asp:Label>
                                        </td>
                                        <td align="left" class="paddingL">
                                            <asp:Label ID="lblTheme" runat="server" Text='<%# Eval("Theme") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                ImageUrl="../images/IconGrid_Edit.GIF" />
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
                </table>
            </td>
        </tr>
        <tr id="trNoRecordFound" align="center" runat="server">
            <td class="LblNoRecord" style="width:800px" align="center">
                No record found.
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidMode" runat="server" />
                <asp:HiddenField ID="hidThemeId" runat="server" />
                <asp:HiddenField ID="hidRowNo" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidSortDirection" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _ClientcstvalDuplicateSortOrder = "<%=this.cstvalDuplicateSortOrder.ClientID %>"
        _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
        _clientlstvwThemeDetails = "<%=this.lstvwThemeDetails.ClientID %>"
        _clienttxtTheme = "<%=this.txtTheme.ClientID %>"
        _clienthidMode = "<%=this.hidMode.ClientID %>"
        _clientcstValTheme = "<%=this.cstValTheme.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientcstDuplicateTheme = "<%=this.cstDuplicateTheme.ClientID %>"

           function DuplicateSortOrder(oSrc, args) {
            var SortOrder = "";
            var sRowNo = "";
            var iRowNumber = 0;
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtSortOrder = (document.getElementById(_clienttxtSortOrder).value).trim();

            var lblUpdate = document.getElementById(_clientlblUpdateSucess);
            if (lblUpdate.innerText != "")
                document.getElementById(_clientlblUpdateSucess).innerText = "";

            SortOrder = document.getElementById(_clientlstvwThemeDetails + "_ctrl" + iRowNumber + "_lblSortOrder");
            if (txtSortOrder != "")
            while (SortOrder != null) {
                if (txtSortOrder == SortOrder.innerHTML && iRowNumber != (iRowNo - 1)) {
                    sRowNo += (iRowNumber + 1) + ", ";
                }
                iRowNumber += 1;
                SortOrder = document.getElementById(_clientlstvwThemeDetails + "_ctrl" + iRowNumber + "_lblSortOrder");
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = "Sort order should not be duplicated for row(s): " + sRowNo + ".";
                document.getElementById(_ClientcstvalDuplicateSortOrder).innerText = "Sort order should not be duplicated for row(s): " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function validateDuplicateTheme(oSrc, args) {       
            if (document.getElementById(_clientlblUpdateSucess).innerText != "")
                document.getElementById(_clientlblUpdateSucess).innerText = "";
            var sRowNo = "";
            var iRowNumber = 0;
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtTheme = (document.getElementById(_clienttxtTheme).value).trim();
            var lblTheme = document.getElementById(_clientlstvwThemeDetails + "_ctrl" + iRowNumber + "_lblTheme")
            if (txtTheme != "") {
                while (lblTheme) {
                    if (txtTheme.toLowerCase() == (lblTheme.innerHTML).toLowerCase() && iRowNumber != (iRowNo - 1)) {
                        if (sRowNo = "")
                            sRowNo = (iRowNumber + 1);
                        else
                            sRowNo += (iRowNumber + 1) + ", ";
                    }
                    iRowNumber += 1;
                    lblTheme = document.getElementById(_clientlstvwThemeDetails + "_ctrl" + iRowNumber + "_lblTheme")
                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = "Theme should not be duplicated for row(s): " + sRowNo + ".";
                document.getElementById(_clientcstDuplicateTheme).innerText = "Theme should not be duplicated for row(s): " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }   
        }
        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function validateTheme(source, args) {
            var txtTheme = document.getElementById(_clienttxtTheme).value;
            var bIsValid = true;
            if (txtTheme.trim() != "") {
                if (txtTheme.length > 100) {
                    bIsValid = false;
                    document.getElementById(_clientcstValTheme).errormessage =
                        "Theme should be of length less than 100.";
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
