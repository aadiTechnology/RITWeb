<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="ImportNewAdmissionsUI.aspx.cs" Inherits="ImportNewAdmissionsUI"
    Title="Untitled Page" %>

<asp:Content ID="CntImportItem" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                    <tr>
                        <td align="right">
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="valGrpUFile" />
                                        <asp:Label ID="lblUploadErrMsg" runat="server" Visible="False" CssClass="LblErrorMsg"
                                            EnableViewState="false"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="float: right; vertical-align: top">
                                            <span class="ClsMdtStar">* Mandatory Fields</span><br />
                                            <asp:HyperLink ID="hlnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                                ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download the template for adding new admitted student details by template."></asp:HyperLink>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblUploadMsg" runat="server" Text="Your file has been uploaded sucessfully."
                                            Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="0" cellspacing="3">
                                            <tr>
                                                <td class="ClsBorderLight" colspan="3" align="center" style="text-align:center; margin:0px auto;">
                                                    <asp:RadioButton ID="rdoManualAdmission" Text="Manual Admission" runat="server" GroupName="Admission" />
                                                    <asp:RadioButton ID="rdoOnlineAdmission" Text="Online Admission" runat="server" GroupName="Admission" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderLight">
                                                    <%--<asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                        Text="Standard :" EnableViewState="False" Width="105px"></asp:Label>--%>
                                                        <span class="ClsLabel" id="lblStandard" style="width:105px">Standard :</span>
                                                </td>
                                                <td style="padding-right: 15px;">
                                                    <asp:DropDownList ID="ddlStandard" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                        TabIndex="1" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                    <asp:CompareValidator ID="cmpStandard" runat="server" ControlToValidate="ddlStandard"
                                                        Display="None" ErrorMessage="Standard should be selected." Operator="NotEqual"
                                                        ValueToCompare="0" CssClass="ClsLabel" ValidationGroup="valGrpUFile"></asp:CompareValidator>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1">
                                                    <asp:CustomValidator ID="cstvalFileType" runat="server" ClientValidationFunction="validateFile"
                                                        ControlToValidate="fileUploadItems" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                        ErrorMessage="Invalid file type." ValidationGroup="valGrpUFile"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                    <%--<asp:Label ID="lblSelectFile" runat="server" CssClass="ClsLabel" Text="Select File : "
                                                        EnableViewState="False"></asp:Label>--%>
                                                        <span class="ClsLabel" id="lblSelectFile" >Select File :</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:FileUpload ID="fileUploadItems" runat="server" />
                                                    <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                </td>
                                                <td align="center" colspan="1">
                                                    <%--<asp:Label ID="lblFileType" runat="server" Text="  (Supports only .XLS/.XLSX files type)"
                                                        CssClass="LblSmlGray" EnableViewState="False"></asp:Label>--%>
                                                        <span class="LblSmlGray" id="lblFileType">(Supports only .XLS/.XLSX files type)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                    <%--<asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Send SMS: " EnableViewState="False"></asp:Label>--%>
                                                    <span class="ClsLabel" id="Span1" >Send SMS: </span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:CheckBox ID="chkSms" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsOnlyBorderlght" colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 50%;">
                                        <asp:Button ID="btnImportStudent" Text="Import New Admissions" runat="server" CssClass="ClsBtnMid"
                                            BorderStyle="Solid" Visible="True" CausesValidation="true" BorderWidth="1px"
                                            UseSubmitBehavior="false" ValidationGroup="valGrpUFile" Width="150px" OnClick="btnImportStudent_Click" />
                                        <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                            Visible="True" BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false"
                                            PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                            ID="uPnl">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="trItemCount" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails"
                                                                Visible="true">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <div id="divGridView" runat="server" style="width: 100%;">
                                                                <asp:ListView ID="lstvwStudentDetails" runat="server" OnDataBound="lstvwStudentDetails_DataBound"
                                                                    OnSorting="lstvwStudentDetails_Sorting" DataKeyNames="Student_Admission_Id">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                            cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="ClspaddingL" width="13%">
                                                                                    Is Confirmed
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL" width="14%">
                                                                                    <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="Sort" CommandArgument="Form_Number"
                                                                                        ForeColor="Black">Form No.</asp:LinkButton>
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL" width="18%">
                                                                                    <asp:LinkButton ID="lnkStandardName" runat="server" CommandName="Sort" CommandArgument="Standard_Name"
                                                                                        ForeColor="Black">Standard Name</asp:LinkButton>
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL" width="40%">
                                                                                    <asp:LinkButton ID="lnlStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
                                                                                        ForeColor="Black">Student Name</asp:LinkButton>
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                                <td colspan="4">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
                                                                                        <Fields>
                                                                                            <asp:TemplatePagerField>
                                                                                                <PagerTemplate>
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                                        <tr class="ClsGridRow">
                                                                            <td align="center">
                                                                                <asp:Image ID="imgConfirm" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                                                    Visible='<%#Convert.ToBoolean(Eval("IsConfirmed")) %>' />
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trItemWise" runat="server" class="ClsGridAltRow">
                                                                            <td align="center">
                                                                                <asp:Image ID="imgConfirm" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                                                    Visible='<%#Convert.ToBoolean(Eval("IsConfirmed")) %>' />
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
                                                                            </td>
                                                                            <td class="ClspaddingL">
                                                                                <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <tr>
                                                                            <td class="LblNoRecord" align="center">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentAdmissionsBL" EnablePaging="true"
                                                                ID="lstvwObjDS" runat="server" SelectMethod="GetAllNewStudentDetails" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountAllNewStudentDetails" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter Name="aiAcademicYearID" Type="Int16" ControlID="hidNextAcademiYearId" PropertyName="Value" DefaultValue="0" />
                                                                    <asp:ControlParameter Name="aiStandardID" Type="Int16" ControlID="ddlStandard" PropertyName="SelectedValue" DefaultValue="0" />                                                                                                                      
                                                                    <asp:Parameter Name="aiAdmissionType" DbType="Int32" DefaultValue="0" />
                                                                    <asp:Parameter DbType="String" DefaultValue=" " Name="asStudentName" />
                                                                    <asp:Parameter DbType="Int32" DefaultValue="0" Name="aiAdmissionStatusId" />    
                                                                    <asp:Parameter DbType="String" Name="sortExpression" DefaultValue="" />
                                                                    <asp:Parameter DbType="Int32" Name="startRowIndex" DefaultValue="0" />
                                                                    <asp:Parameter DbType="Int32" Name="maximumRows" DefaultValue="" />
                                                                    <asp:Parameter DbType="Boolean" Name="abIsAdmitted" DefaultValue="true" />
                                                                    <asp:Parameter DbType="Int32" Name="aiAdmissionForId" DefaultValue="0" />                                                                                                                                                                                                                                                                                                                                 
                                                              </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                            <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidNextAcademiYearId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidAdminID" runat="server" />
                                                            <asp:HiddenField ID="hidSchoolStartDate" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="DataBound" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                <asp:PostBackTrigger ControlID="btnImportStudent" />
                <asp:PostBackTrigger ControlID="btnBack" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript">

        _clientFileUploadClientId = "<%=this.fileUploadItems.ClientID%>";
        _clientCstvalFileTypeId = "<%=this.cstvalFileType.ClientID%>";
        _clientbtnImportStudent = "<%=this.btnImportStudent.ClientID%>";
        _clientBtnBack = "<%=this.btnBack.ClientID%>";
        _clientlblUploadMsg = "<%=this.lblUploadMsg.ClientID%>";
        _clientlblUploadErrMsg = "<%=this.lblUploadErrMsg.ClientID%>";
        _clientddlStandard = "<%=this.ddlStandard.ClientID%>";

        function ClearLabel() {
            if (document.getElementById(_clientlblUploadMsg)) {
                document.getElementById(_clientlblUploadMsg).innerText = "";
                document.getElementById(_clientlblUploadMsg).innerHTML = "";
            }
            if (document.getElementById(_clientlblUploadErrMsg)) {
                document.getElementById(_clientlblUploadErrMsg).innerText = "";
                document.getElementById(_clientlblUploadErrMsg).innerHTML = "";
            }
        }

        function validateFile(source, args) {
            ClearLabel();
            var oFileName = document.getElementById(_clientFileUploadClientId).value;
            var oCusVal = document.getElementById(_clientCstvalFileTypeId);

            var bIsValid = true;
            if (oFileName != "") {
                var sFileExtension = oFileName.substring(oFileName.indexOf('.'));
                sFileExtension = sFileExtension.toUpperCase();
                if (sFileExtension != ".XLS" && sFileExtension != ".XLSX") {
                    bIsValid = false;
                    oCusVal.errormessage = "File to upload should be in valid format.";
                }
            }
            else {
                bIsValid = false;
                oCusVal.errormessage = "File to upload should be selected.";
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportStudent)) {
                var isPageValid = true;

                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate();
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportStudent).disabled = true;
                    document.getElementById(_clientBtnBack).disabled = true;
                }
            }
            else if (ObjBtn == document.getElementById(_clientBtnBack)) {
                document.getElementById(_clientbtnImportStudent).disabled = true;
                document.getElementById(_clientBtnBack).disabled = true;
            }
        }
    </script>

</asp:Content>
