<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LedgerMasterUI.aspx.cs" Inherits="LedgerMasterUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <%--<asp:UpdatePanel ID="mainUpdatePanel" runat="server">
        <ContentTemplate>--%>
            <table id="tblMain" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                <tr>
                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                        <span class="ClsMdtStar">* Mandatory Fields</span>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="valsumErrorMessages" runat="server" CssClass="ClsLabel"
                            ShowSummary="true" />
                        <asp:Label ID="lblErrorMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
                            Style="width: 100%; text-align: center; margin: 8px 0;" ForeColor="Red" Visible="false" />
                        <asp:Label ID="lblUpateMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
                            Style="width: 100%; text-align: center; margin: 8px 0;" ForeColor="Blue" Font-Bold="true"
                            Visible="false" />
                        <asp:CustomValidator ID="cstLedgerNameValidator" runat="server" Display="None" ClientValidationFunction="ValidateLedgerName"
                            EnableClientScript="true" ErrorMessage="Ledger Name should not be blank." />
                        <asp:CustomValidator ID="cstGroupSelectValidator" runat="server" Display="None" ClientValidationFunction="ValidateGroupSelection"
                            EnableClientScript="true" ErrorMessage="Group should be selected." />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="0" cellspacing="2">
                            <tr>
                                <td class="ClsBorderlight" valign="middle">
                                    <span class="ClsLabel">Ledger Name :</span>
                                </td>
                                <td colspan="2" align="left" valign="top">
                                    <asp:TextBox ID="txtLedgerName" runat="server" CssClass="LrgTxtBox" MaxLength="100" />
                                    <span class="ClsMdtStar"  margin-left: 5px;">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight" valign="middle">
                                    <span class="ClsLabel">Group :</span>
                                </td>
                                <td colspan="2" align="left" valign="top">
                                    <asp:DropDownList ID="ddlGroupList" runat="server" CssClass="LrgCombo" AppendDataBoundItems="true"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged" />
                                    <span class="ClsMdtStar" margin-left: 5px;">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight" valign="middle">
                                    <span class="ClsLabel">Opening Balance :</span>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtOpeningBal" runat="server" CssClass="MidTxtBox" MaxLength="11"
                                        Width="135px" onblur="extractNumber(this,2,false);" onkeyup="extractNumber(this,2,false);"
                                        onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                        ondrop="event.returnValue=false" onChange="ValidateDecimalNumber()" />
                                    <asp:DropDownList ID="ddlDebitCredit" runat="server">
                                        <asp:ListItem Text="Credit" Value="0" />
                                        <asp:ListItem Text="Debit" Value="1" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderlight" valign="middle">
                                    <span class="ClsLabel">Budget :</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtBudget" runat="server" CssClass="LrgTxtBox" MaxLength="11" onblur="extractNumber(this,2,false);"
                                        onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" onChange="ValidateDecimalNumber()" />
                                </td>
                            </tr>
                            <tr runat="server" id="trPan" visible="False">
                                <td align="left" class="ClsBorderlight">
                                    <span class="ClsLabel">PAN No. :</span>
                                </td>
                                <td align="left" class="ClsMdtStar">
                                    <asp:TextBox ID="txtPanNo" runat="server" MaxLength="20" CssClass="LrgTxtBox" onkeypress="return PreventSpecialChars(event);"></asp:TextBox>
                                    <span style="color: red; white-space: nowrap" class="ClsMdtStar">*</span>
                                    <asp:CustomValidator ID="cstValPan" Display="None" runat="server" ClientValidationFunction="ValidatePan"
                                        ErrorMessage="" CssClass="TxtNormal"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr runat="server" id="trUpload" visible="False">
                                <td align="left" class="ClsBorderLight">
                                    <span class="ClsLabel" id="Span2" style="white-space: nowrap">Upload File :</span>
                                </td>
                                <td align="left" style="white-space: nowrap">
                                    <asp:FileUpload ID="UploadFile" runat="server" Style="white-space: nowrap" />
                                    <span style="color: red; white-space: nowrap" class="ClsMdtStar">*</span>
                                    <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateFile"
                                        ErrorMessage="Invalid file format. Only bitmap(*.bmp) is allowed." CssClass="TxtNormal"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr runat="server" id="trUploadNote" visible="False">
                                <td colspan="2" align="center">
                                    <span class="LblSmlGray">(Supports files of types - .BMP,.DOC,.DOCX,.JPG,.JPEG,.PDF,.XLS,.XLSX
                                        upto 1 MB)</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" OnClick="btnSave_Click"
                                        disable-page="true" OnClientClick="ClearMessages();" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="Cancel" CausesValidation="false"
                                        UseSubmitBehavior="false" OnClientClick="ResetControls(); return false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>--%>
  <%--  <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>--%>
        
    <table>
        <tr>
            <td align="center" id="tdlstvwLedgerList" runat="server">
                <asp:ObjectDataSource ID="objdsLedgerList" runat="server" TypeName="SchoolBusinessService.AccountLedgerClient"
                    SelectMethod="GetAllLedgers" SelectCountMethod="GetLedgerCount" EnablePaging="true">
                    <SelectParameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                        <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="sortExpression"
                            Type="String" />
                        <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="sortDirection"
                            Type="String" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ListView ID="lstvwLedgerDetails" runat="server" DataSourceID="objdsLedgerList"
                    DataKeyNames="Id,OriginalLedger,Group,IsDebit,IsSystemDefined,PanNo,FilePath,IsPanApplicable"
                    OnItemDataBound="lstvwLedgerDetails_ItemDataBound" OnDataBound="lstvwLedgerDetails_DataBound"
                    OnItemCommand="lstvwLedgerDetails_ItemCommand">
                    <LayoutTemplate>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwLedgerDetails"
                                        PageSize="20">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                        Text="<%# Container.StartRowIndex + 1%>" />
                                                    <asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="LblNormal"
                                                        Text=" To " />
                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                        Text=" Out Of " />
                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                        Text="Records" />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="3" cellspacing="1" class="GridBorder" width="800px">
                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                <th align="left" style="font-size: 9pt; width: 200px; white-space: nowrap;">
                                    <asp:LinkButton ID="lnkbtnName" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                        CommandArgument="LedgerMaster.Name" Text="Ledger Name" ForeColor="Black" />
                                </th>
                                <th align="left" style="font-size: 9pt; width: 200px; white-space: nowrap;">
                                    <asp:LinkButton ID="lnkbtnGroup" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                        CommandArgument="LedgerMaster.GroupName" Text="Group" ForeColor="Black" />
                                </th>
                                <th align="right" style="font-size: 9pt; width: 140px; white-space: nowrap;">
                                    <asp:LinkButton ID="lnkbtnOpeningBalance" runat="server" CausesValidation="false"
                                        CommandName="SORT_ROW" CommandArgument="LedgerMaster.OpeningBalance" Text="Opening Balance (Rs.)"
                                        ForeColor="Black" />
                                </th>
                                <th align="right" style="font-size: 9pt; width: 100px; white-space: nowrap;">
                                    <asp:LinkButton ID="lnkbtnBudget" runat="server" CausesValidation="false" CommandName="SORT_ROW"
                                        CommandArgument="LedgerMaster.Budget" Text="Budget (Rs.)" ForeColor="Black" />
                                </th>
                                <th style="font-size: 9pt; width: 50px;">
                                    Action
                                </th>
                                <th style="font-size: 9pt; width: 30px;">
                                    View
                                </th>
                            </tr>
                            <tr id="itemPlaceHolder" runat="server">
                            </tr>
                            <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                <td colspan="6">
                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwLedgerDetails"
                                        PageSize="20">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                        <tr id="trGridRow" runat="server" class="ClsGridRow">
                            <td>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group.Name") %>' />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblOpeningBal" runat="server" Style="padding-right: 3px;" Text='<%# GetOpeningBalText(Eval("OpeningBalance"), Eval("IsDebit")) %>' />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblBudget" runat="server" Text='<%# Utility.CommonUtility.FormatCurrency(Eval("Budget")) %>' />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="EDIT_ROW"
                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DELETE_ROW"
                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!WarnOnDelete()){return false;}" />
                            </td>
                            <td align="center">
                                <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                    CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div class="LblNoRecord" style="margin: 10px 0; width: 700px;">
                            No record found.</div>
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
        
      <%--  </ContentTemplate>
        <Triggers>
               <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <%-- HIDDEN FIELDS --%>
    <asp:HiddenField ID="hidLedgerId" runat="server" Value="0" />
    <asp:HiddenField ID="hidSortExpression" runat="server" />
    <asp:HiddenField ID="hidSortDirection" runat="server" />
    <asp:HiddenField ID="hidFilePath" runat="server" />
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Style="margin-top: 10px;"
                    Text="Back" CausesValidation="false" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>
    <%-- JAVASCRIPT --%>
    <script type="text/javascript">
        // IDs of controls on page.
        var _clientvalsumErrorMessages = '<%= this.valsumErrorMessages.ClientID %>';
        var _clientlblUpateMessage = '<%= this.lblUpateMessage.ClientID %>';
        var _clientlblErrorMessage = '<%= this.lblErrorMessage.ClientID %>'
        var _clientbtnSave = '<%= this.btnSave.ClientID %>';
        var _clienttxtLedgerName = '<%= this.txtLedgerName.ClientID %>';
        var _clientddlGroupList = '<%= this.ddlGroupList.ClientID %>';
        var _clienttxtOpeningBal = '<%= this.txtOpeningBal.ClientID %>';
        var _clientddlTransactionType = '<%= this.ddlDebitCredit.ClientID %>';
        var _clienttxtBudget = '<%=this.txtBudget.ClientID %>';
        var _clientUploadFile = '<%=this.UploadFile.ClientID %>';
        var _clientcstValidateLogo = '<%=this.cstValidateLogo.ClientID %>';
        var _clienttrUpload = '<%=this.trUpload.ClientID %>';
        var _clienthidFilePath = "<%=this.hidFilePath.ClientID %>";
        var _clienttrPan = "<%=this.trPan.ClientID %>";
        var _clienttrUploadNote = "<%=this.trUploadNote.ClientID %>";
        var _clienttxtPanNo = "<%=this.txtPanNo.ClientID %>";
        var _clienthidLedgerId = "<%=this.hidLedgerId.ClientID %>";

        // Register listeners for Postbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);


        /* -----------------------
        *	PAGE REQUEST HANDLERS
        * -----------------------
        */

        // This function is used to disable controls on the page when a postback occurs.
        function BeginRequestHandler() {
            ToggleControls(true);
        }

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            ToggleControls(false);
        }


        /* ----------------------
        *	VALIDATION FUNCTIONS
        * ----------------------
        */

        // Ledger Name Validation
        function ValidateLedgerName(src, args) {
            var txtLedgerName = $get(_clienttxtLedgerName);
            if (txtLedgerName && txtLedgerName.value.trim() == '') {
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        // Group Selection Validation
        function ValidateGroupSelection(src, args) {
            var ddlGroupList = $get(_clientddlGroupList);
            if (ddlGroupList && ddlGroupList.selectedIndex == 0) {
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }


        /* -----------------------
        *	MISC HELPER FUNCTIONS
        * -----------------------
        */

        function ToggleControls(state) {
            var _btnSave = $get(_clientbtnSave);

            if (_btnSave)
                _btnSave.disabled = state;
        }

        // This function warns the user when deleting a ledger
        function WarnOnDelete() {
            return confirm("Are you sure you want to delete this Ledger?");
        }

        function ClearMessages() {
            var valsumErrorMessages = $get(_clientvalsumErrorMessages);
            if (valsumErrorMessages)
                valsumErrorMessages.style.display = 'none';

            var lblUpdateMessage = $get(_clientlblUpateMessage);
            if (lblUpdateMessage)
                lblUpdateMessage.innerHTML = '';

            var lblErrorMessage = $get(_clientlblErrorMessage);
            if (lblErrorMessage)
                lblErrorMessage.innerHTML = '';
        }

        function ResetControls() {

            ClearMessages();

            var txtLedgerName = $get(_clienttxtLedgerName);
            if (txtLedgerName)
                txtLedgerName.value = '';

            var ddlGroupList = $get(_clientddlGroupList);
            if (ddlGroupList)
                ddlGroupList.selectedIndex = 0;

            var txtOpeningBal = $get(_clienttxtOpeningBal);
            if (txtOpeningBal)
                txtOpeningBal.value = '';

            var ddlTransactionType = $get(_clientddlTransactionType);
            if (ddlTransactionType)
                ddlTransactionType.selectedIndex = 0;

            var btnSave = $get(_clientbtnSave);
            if (btnSave)
                btnSave.value = 'Save';

            var txtBudget = $get(_clienttxtBudget)
            if (txtBudget)
                txtBudget.value = '';

            var txtPanNo = $get(_clienttxtPanNo);
            if (txtPanNo)
                txtPanNo.value = '';

            var trPan = $get(_clienttrPan);
            if (trPan)
                trPan.style.display = "none";

            var trUpload = $get(_clienttrUpload);
            if (trUpload)
                trUpload.style.display = "none";

            var trUploadNote = $get(_clienttrUploadNote);
            if (trUploadNote)
                trUploadNote.style.display = "none";

            var hidFilePath = $get(_clienthidFilePath);
            if (hidFilePath)
                hidFilePath.value = "";
                 
            var hidLedgerId = $get(_clienthidLedgerId);
            if (hidLedgerId)
                $get(_clienthidLedgerId).value = "";
        }

        function ValidateDecimalNumber() {
            if (document.getElementById(_clienttxtOpeningBal).value == ".")
                document.getElementById(_clienttxtOpeningBal).value = 0;
            if (document.getElementById(_clienttxtBudget).value == ".")
                document.getElementById(_clienttxtBudget).value = 0;

        }

        function PreventSpecialChars(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
        }

        function ValidatePan(aSrc, args) {
            var status = $get(_clienttrPan).style.display;
            if ($get(_clienttxtPanNo) != null && status != "none") {
                if ($get(_clienttxtPanNo).value == "") {
                    aSrc.errormessage = "PAN No. should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateFile(aSrc, args) {
            var myImage = new Image();
            myImage.src = $get(_clientUploadFile).value;
            var status = $get(_clienttrUpload).style.display;
            if ($get(_clienttrUpload) != null && status != "none") {
                if ($get(_clienthidFilePath).value == "" && $get(_clientUploadFile).value == '') {
                    $get(_clientcstValidateLogo).errormessage = "Upload File should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                if ($get(_clientUploadFile).value != '' && !CheckFileType(myImage.src)) {
                    $get(_clientcstValidateLogo).errormessage = "Invalid file format.";
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function CheckFileType(sFileName) {
            var bIsValid = true;
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".DOC"
            || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".DOCX" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG"
            || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF"
            || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".XLSX" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".XLS")
                    bIsValid = true;
                else
                    bIsValid = false;
            }
            return bIsValid;
        }

    </script>
</asp:Content>
