<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="UpdateStudentDetailsInBulkUI.aspx.cs" Inherits="UpdateStudentDetailsInBulkUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <span class="ClsMdtStar">* Mandatory Field</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSum" runat="server" HeaderText="Please correct following errors."
                                            ValidationGroup="SHOW" />
                                        <asp:RequiredFieldValidator ID="reqcmbCategory" runat="server" Display="None" ControlToValidate="cmbCategory"
                                            InitialValue="0" ErrorMessage="Category should be selected." ValidationGroup="SHOW"></asp:RequiredFieldValidator>
                                        <asp:ValidationSummary ID="valSaveSummary" runat="server" HeaderText="Please correct the following errors."
                                            ValidationGroup="SAVE" />
                                        <asp:CustomValidator ID="cvAtLeastOneNewValue" runat="server" ClientValidationFunction="ValidateAtLeastOneNewValue"
                                            ErrorMessage="At least one Value is required." Display="None" ValidationGroup="SAVE" />
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateBlankData"
                                            ErrorMessage="New Value should be set for each selected student." Display="None"
                                            ValidationGroup="SAVE" />
                                        <asp:CustomValidator ID="cvStudentData" runat="server" ClientValidationFunction="validateAllRecordsBeforeSubmit"
                                            Display="None" ErrorMessage="Student data is invalid." ValidationGroup="SAVE"
                                            EnableClientScript="true" ForeColor="Red" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdMessage" runat="server" align="center">
                                <asp:UpdatePanel ID="upnlSuccessMsg" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblmessage" runat="server" Text="" EnableViewState="false" ForeColor="Blue"
                                            Font-Bold="True" CssClass="LblNormal"></asp:Label><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table width="70%">
                                            <tr>
                                                <td class="ClsBorderlight" style="padding-left: 5px">
                                                    <span class="ClsLabel">Category :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td class="ClsBorderlight" style="padding-left: 5px">
                                                    <span class="ClsLabel">Standard :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStandard" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">Division :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbDivision" CssClass="LrgCombo" runat="server" EnableViewState="true">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="Tr1" runat="Server">
                            <td class="HilightBGGray" align="center" colspan="4">
                                <asp:Label CssClass="ClsHilightText" ID="lblSelectStanardDivision" runat="server"
                                    EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectStanardDivision%>"></asp:Label>
                                <img src="../images/ArrowBlueDblRev.gif" />
                                <asp:Label CssClass="ClsHilightTextB" ID="lblAnd" runat="server" EnableViewState="False"
                                    Text="<%$ Resources:LocalizedResources, And%>"></asp:Label>
                                <img src="../images/ArrowBlueDblNw.gif" />
                                <asp:Label CssClass="ClsHilightTextB" ID="lblStudentRegNo" runat="server" EnableViewState="False"
                                    Text="<%$ Resources:LocalizedResources, SelectstudentNameRegNo%>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="25px">
                                                    <asp:RadioButton ID="optMain" runat="server" GroupName="Search" TabIndex="1" />
                                                </td>
                                                <td class="ClsBorderlight" align="left" style="padding-left: 5px" width="200px">
                                                    <asp:Label CssClass="clsLabel" ID="lblStudentNameRegNo" runat="server" EnableViewState="False"
                                                        Text="Student Name / Reg. No."></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="Center" class="ClsBorderlight" width="80px">
                                                    <asp:Label ID="lblLike" runat="server" Style="font-weight: bold" Text="<%$ Resources:LocalizedResources, Like%>"></asp:Label>
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtRegNumber" TabIndex="2" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                        AutoPostBack="false" autocomplete="off"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="25px">
                                                    <asp:RadioButton ID="optExact" runat="server" GroupName="Search" TabIndex="1" />
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="padding-left: 5px" width="100px">
                                                    <asp:Label CssClass="clsLabel" ID="lblRegNo" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td width="100px">
                                                    <asp:DropDownList ID="cmbOperation" disabled runat="server" Width="90px" CssClass="SmlCombo"
                                                        TabIndex="2" Height="19px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="80px">
                                                    <asp:DropDownList ID="cmbPrefix" runat="server" TabIndex="3" CssClass="SmlCombo"
                                                        Style="width: 80px;" disabled>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtReg" runat="server" CssClass="MidTxtBox" AutoPostBack="false"
                                                        disabled onblur="extractNumber(this,0,false);" CausesValidation="true" TabIndex="4"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="chkIsStudBlankRegNo" runat="server" Text="Show only students having blank value"
                                                        TabIndex="12" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnShow" CssClass="ClsBtn" runat="server" Text="Show" OnClick="btnShow_Click"
                                            ValidationGroup="SHOW" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel11" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table width="75%">
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
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
                                                    <asp:ListView ID="lstvwStudentDetails" runat="server" DataKeyNames="YearWise_Student_Id"
                                                        OnItemDataBound="lstvwStudentDetails_ItemDataBound" OnDataBound="lstvwStudentDetails_DataBound"
                                                        OnSorting="lstvwStudentDetails_Sorting">
                                                        <LayoutTemplate>
                                                            <table id="tblStudentList" width="100%" style="color: #333333" cellpadding="0" cellspacing="1"
                                                                class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th width="50px" align="center">
                                                                        <input id="chkAll" type="checkbox" onclick="CheckAll(this);" />
                                                                    </th>
                                                                    <th width="100px" align="left" class="clsLabelgrd">
                                                                        <asp:Label ID="Label1" runat="server" Text="Class"></asp:Label>
                                                                    </th>
                                                                    <th width="100px" align="left" class="clsLabelgrd">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Enrolment_Number"
                                                                            CausesValidation="false" ForeColor="Black"> Registration No. </asp:LinkButton>
                                                                    </th>
                                                                    <th width="60px" align="Center" class="clsLabelgrd">
                                                                        <asp:LinkButton ID="lnkUserName" runat="server" CommandName="Sort" CommandArgument="Roll_No"
                                                                            CausesValidation="false" ForeColor="Black"> Roll No. </asp:LinkButton>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd" width="250px">
                                                                        <asp:Label ID="lblStudentName" runat="server" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd" width="200px">
                                                                        <asp:Label ID="lblNewValue" runat="server" Text="Existing / New Value"></asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="6">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentDetails"
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
                                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex % 2 == 0?"ClsGridAltRow":"ClsGridRow" %>'>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </td>
                                                                <td align="Center">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                </td>
                                                                <td align="Center">
                                                                    <asp:Label ID="lblEnrollmentno" runat="server" CssClass="ClsLabel" Text='<%#Eval("EnrollmentNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" CssClass="centerText" Text='<%#Eval("RollNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtNewValue" runat="server" Width="98%" Text='<%#Eval("ExistingValue") %>'
                                                                        MaxLength="20" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"></asp:TextBox>
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
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.UpdateStudentDetailsInBulkBL" EnablePaging="True"
                                                        ID="objdsStudentList" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                                        SelectCountMethod="GetCount" EnableCaching="False">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="int32" />
                                                            <asp:ControlParameter ControlID="cmbStandard" Name="aiStandardId" PropertyName="SelectedValue" />
                                                            <asp:ControlParameter ControlID="cmbDivision" Name="aiDivisionId" PropertyName="SelectedValue" />
                                                            <asp:ControlParameter ControlID="cmbCategory" Name="aiCategoryId" PropertyName="SelectedValue" />
                                                            <asp:ControlParameter ControlID="txtRegNumber" Name="asEnrolmentNumber" Type="String"
                                                                PropertyName="Text" />
                                                            <asp:ControlParameter ControlID="chkIsStudBlankRegNo" Name="abIsStudBlankRegNo" PropertyName="Checked" />
                                                            <asp:ControlParameter ControlID="txtReg" Name="asRegNo" Type="String" PropertyName="Text" />
                                                            <asp:ControlParameter ControlID="optExact" Name="abIsExact" PropertyName="Checked" />
                                                            <asp:ControlParameter ControlID="cmbOperation" Name="asOperator" PropertyName="SelectedValue" />
                                                            <asp:ControlParameter ControlID="cmbPrefix" Name="asPrefix" PropertyName="SelectedValue" />
                                                            <asp:ControlParameter ControlID="hidIsResetCall" Name="asIsResetCall" PropertyName="Value" />
                                                            <asp:ControlParameter ControlID="hidSortExpression" Name="sortExpression" Type="String"
                                                                PropertyName="Value" />
                                                            <asp:ControlParameter ControlID="hidSortDirection" Name="sortDirection" Type="String"
                                                                PropertyName="Value" />
                                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                    <asp:HiddenField ID="hidIsResetCall" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" Text="Back" PostBackUrl="~/RITeSchool/Admin/AllStudentsUI.aspx"
                                            Visible="false"/>
                                        <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" OnClick="btnSave_Click"
                                            Visible="false" ValidationGroup="SAVE" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
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
        var _clientlstvwStudentDetails = "<%= lstvwStudentDetails.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnShow = "<%=this.btnShow.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientbtnSave) {
                window.scrollTo({ top: 0, behavior: 'smooth' });
            }
            else if (postBackElement.id == _clientbtnShow && $('#' + _clientbtnShow).val() == 'SHOW') {
                if ($('#' + '<%=this.optMain.ClientID %>').is(':checked')) {
                    DisableFields(1);
                }
                else {
                    DisableFields(2);
                }
            }
        }

        function ValidateAtLeastOneNewValue(source, args) {
            var hasValue = false;
            if ($('[id$=_chkSelect]:checked').length > 0) {
                hasValue = true;
            }

            if (!hasValue) {
                source.errormessage = "At least one student should be selected.";
                args.IsValid = false;
                return true;
            } else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateBlankData(src, args) {
            var hasErrors = false
            var invalidRollNos = [];

            $('#tblStudentList tr.ClsGridRow, #tblStudentList tr.ClsGridAltRow').each(function () {
                var row = $(this);
                var chk = row.find('input[id$=_chkSelect]')
                var newValueInput = row.find('input[id$=_txtNewValue]');
                var value = newValueInput.val() ? newValueInput.val().trim() : '';
                var rollNo = row.find('[id$=_lblRollNo]').text().trim();

                if (chk.prop('checked') && value == '') {
                    invalidRollNos.push(rollNo);
                    hasErrors = true;
                }
            });

            if (invalidRollNos.length > 0) {
                src.errormessage = "New value should not be blank for students with Roll No. : " + invalidRollNos.join(", ") + '.'
                args.IsValid = false;
                return true;
            } else {
                args.IsValid = true;
                return false;
            }
        }

        function validateAllRecordsBeforeSubmit(source, args) {
            var category = $('#<%= cmbCategory.ClientID %>').val();
            var hasErrors = false;

            var penNoInvalidRollNos = [];
            var apaarIdInvalidRollNos = [];
            var saralIdInvalidRollNos = [];

            $('#tblStudentList tr.ClsGridRow, #tblStudentList tr.ClsGridAltRow').each(function () {
                var row = $(this);
                var chk = row.find('input[id$=_chkSelect]')
                var newValueInput = row.find('input[id$=_txtNewValue]');
                var value = newValueInput.val() ? newValueInput.val().trim() : '';
                var rollNo = row.find('[id$=_lblRollNo]').text().trim();

                if (chk.prop('checked')) {
                    if (category === "2" && value.length > 20) {
                        penNoInvalidRollNos.push(rollNo);
                        hasErrors = true;
                    } else if (category === "3" && !/^\d{12}$/.test(value)) {
                        apaarIdInvalidRollNos.push(rollNo);
                        hasErrors = true;
                    } else if (category === "1" && value.length > 20) {
                        saralIdInvalidRollNos.push(rollNo);
                        hasErrors = true;
                    }
                }
            });

            var errorMessages = [];

            if (penNoInvalidRollNos.length > 0) {
                errorMessages.push("Length of PEN No. should not be greater than 20 for students with Roll No. : " + penNoInvalidRollNos.join(", ") + '.');
            }
            if (apaarIdInvalidRollNos.length > 0) {
                errorMessages.push("Length of APAAR ID must be 12 digits for students with Roll No. : " + apaarIdInvalidRollNos.join(", ") + '.');
            }
            if (saralIdInvalidRollNos.length > 0) {
                errorMessages.push("Length of Saral Id should not be greater than 20 for students with Roll No. : " + saralIdInvalidRollNos.join(", ") + '.');
            }

            if (hasErrors) {
                source.errormessage = errorMessages.join("<br/>");
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        }

        function SetField(obj, txt) {
            if (obj.checked) {
                $(txt).prop('disabled', false)
            }
            else {
                $(txt).prop('disabled', true)
            }

            if ($('[id$=_chkSelect]').length == $('[id$=_chkSelect]:checked').length)
                $('#chkAll').prop('checked', true);
            else
                $('#chkAll').prop('checked', false);
        }

        function CheckAll(obj) {
            if (obj.checked) {
                $('[id$=_chkSelect]').prop('checked', true)
                $('[id$=_txtNewValue]').prop('disabled', false)
            }
            else {
                $('[id$=_chkSelect]').prop('checked', false)
                $('[id$=_txtNewValue]').prop('disabled', true)
            }
        }

        function DisableFields(id) {
            var opr = $('#' + '<%=this.cmbOperation.ClientID %>')
            var prefix = $('#' + '<%=this.cmbPrefix.ClientID %>')
            var regNo = $('#' + '<%=this.txtReg.ClientID %>')
            var enrlNo = $('#' + '<%=this.txtRegNumber.ClientID %>')

            if (id == 1) {
                opr.val('0')
                prefix.val('0')
                regNo.val('')
                opr.prop('disabled', true)
                prefix.prop('disabled', true)
                regNo.prop('disabled', true)
                enrlNo.prop('disabled', false)
            }
            else {
                opr.prop('disabled', false)
                prefix.prop('disabled', false)
                regNo.prop('disabled', false)

                enrlNo.val('');
                enrlNo.prop('disabled', true)
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
