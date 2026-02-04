<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="ImportStudentUI.aspx.cs" Inherits="ImportStudentUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                        <tr>
                            <td align="right" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" colspan="3">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                      <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                                                </ContentTemplate>
                                                <Triggers>
                                                   <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                   <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                   <asp:AsyncPostBackTrigger ControlID="grdvwAllStudents" EventName="RowDataBound" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <div style="float: right">
                                                <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                                    ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip= "<%$ Resources:LocalizedResources, ToolTipDownloadTemplate%>"></asp:HyperLink>
                                                <br />
                                                <span class="ClsMdtStar">* </span>
                                                 <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:UpdatePanel ID="upnlMsg" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblHead" runat="server" Text= "<%$ Resources:LocalizedResources, FileUplpadSuccessfully%>"
                                                        Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                                </ContentTemplate>
                                               
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td align="center" colspan="6">
                                            <table border="0" cellpadding="0" cellspacing="3">
                                                <tr id="tr1">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                       <asp:Label ID="lblNote1" runat="server" Font-Bold= "true" CssClass = "LblNrmlB" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Note1 %>"></asp:Label><span> : </span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                                       <asp:Label ID="lblNoteForImportEntries" runat="server" CssClass = "LblSmlV" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteImportEntries %>"></asp:Label>
                                                            <br />
                                                            1.<asp:Label ID="lblNoteForDefaultEntries" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteDefaultFeeEntries %>"></asp:Label>
                                                            <br />
                                                            2. <asp:Label ID="lblCautionMoney" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, CautionMoney %>"></asp:Label>
                                                            <br />
                                                            3. <asp:Label ID="lblNoteAttadance" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForAttendance %>"></asp:Label>
                                                            <br />
                                                            4. <asp:Label ID="lblProgressCard" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForProgressCard %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr2">
                                                    <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                        <asp:Label ID="lblNote2" runat="server" Font-Bold= "true" CssClass = "LblNrmlB" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Note2 %>"></asp:Label><span> : </span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px; width: 78%">
                                                         <asp:Label CssClass = "LblSmlV" ID="lblPRnNumber" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteAutoGeneratedPRNNumber %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                                       <asp:Label CssClass = "ClsLabel" ID="lblStandard" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                                            CssClass="SmlCombo">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar" style="color: #ff0000">* </span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        &nbsp;<span class="ClsMdtStar" style="color: #ff0000"></span>
                                                        <asp:CompareValidator ID="cmp_Standards" runat="server" ControlToValidate="cmbStandard"
                                                            CssClass="ClsLabel" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValStandardselected%>"
                                                            Operator="NotEqual" ValueToCompare="0" Visible="True"></asp:CompareValidator>
                                                        <asp:CompareValidator ID="cmp_Division" runat="server" ControlToValidate="cmbDivision"
                                                            CssClass="ClsLabel" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValDivisionSelected%>"
                                                            Operator="NotEqual" ValueToCompare="0" Visible="True"></asp:CompareValidator>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                        <asp:Label CssClass = "ClsLabel" ID="lblDivision" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:UpdatePanel ID="UPanelDivision" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged"
                                                                    CssClass="SmlCombo">
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                            ControlToValidate="fileUploadStudents" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage= "<%$ Resources:LocalizedResources, InvalidFileType%>" ></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                        <asp:Label CssClass = "ClsLabel" ID="lblSelectFile" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectFile %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:FileUpload ID="fileUploadStudents" runat="server" />
                                                        <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span> 
                                                          <asp:Label CssClass = "LblSmlGray" ID="lblNoteFileType" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, FileType %>"></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                    </td>
                                                </tr>
                                                	<%if (!Settings.IsMiniSite) %>
													<%{ %>
                                                <tr>
                                                    <td align="left" class="ClsOnlyBorderlght" colspan="2">
                                                        <asp:CheckBox ID="chkSendSMS" runat="server" Text= "<%$ Resources:LocalizedResources, SendSMS%>"
                                                            CssClass="Lbl10pt" />
                                                    </td>
                                                    <td align="center" colspan="1">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                	<%} %>
                                                <tr>
                                                    <td align="center" class="clspaddingsmallt" colspan="3">
                                                        <asp:Button ID="imgbtnBack" Text= "<%$ Resources:LocalizedResources, Back%>" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                                            PostBackUrl="~/RITeSchool/Admin/AllStudentsUI.aspx" Visible="True" BorderWidth="1px"
                                                            CausesValidation="false" UseSubmitBehavior="false" />
                                                        <asp:Button ID="btnImportStudent" Text= "<%$ Resources:LocalizedResources, ImportStudent%>" runat="server" CssClass="ClsBtnMid"
                                                            BorderStyle="Solid" OnClick="btnImportStudent_Click" Visible="True" CausesValidation="true"
                                                            BorderWidth="1px" UseSubmitBehavior="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="6" valign="top" style="width: 100%">
                                            <asp:UpdatePanel ID="UPanelGrid" ChildrenAsTriggers="True" UpdateMode="Conditional"
                                                runat="server">
                                                <ContentTemplate>
                                                    <div style="float: none" id="divlbl" runat="server" visible="false">
                                                        <table>
                                                            <tr runat="server" id="trTotalRec" align="center">
                                                                <td>
                                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                   <asp:Label ID="lblTo" Text= "<%$ Resources:LocalizedResources, To%>" runat="server" CssClass="LblNormal" />
                                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                  <asp:Label ID="lblOutOf" Text= "<%$ Resources:LocalizedResources, OutOf%>" runat="server" CssClass="LblNormal" />
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" Text= "<%$ Resources:LocalizedResources, Records%>" runat="server" CssClass="LblNormal" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <asp:GridView CssClass="GridBorder" ID="grdvwAllStudents" runat="server" AllowPaging="True"
                                                        AutoGenerateColumns="False" AllowSorting="True" Width="100%" PageSize="20" CellPadding="0"
                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" OnPageIndexChanging="grdvwAllStudents_PageIndexChanging"
                                                        EmptyDataText=  "<%$ Resources:LocalizedResources, NoStudentAvailable %>" OnRowCreated="grdvwAllStudents_RowCreated"
                                                        EnableViewState="True" OnRowDataBound="grdvwAllStudents_RowDataBound">
                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="Enrolment_Number" HeaderText= "<%$ Resources:LocalizedResources, RegNo%>" SortExpression="Enrolment_Number">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                    Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Roll_No" HeaderText= "<%$ Resources:LocalizedResources, RollNo%>" SortExpression="Roll_No">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText= "<%$ Resources:LocalizedResources, StudentName%>" SortExpression="First_Name">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DOB" HeaderText= "<%$ Resources:LocalizedResources, DateOfBirth%>" SortExpression="DOB">
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Parent_Name" HeaderText= "<%$ Resources:LocalizedResources, ParentName%>" SortExpression="Parent_Name">
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="ClspaddingL" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <PagerTemplate>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                        <asp:Label ID="MessageLabel" Text= "<%$ Resources:LocalizedResources, SelectAPage%>" runat="server" CssClass="LblNrmlB" />
                                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                            OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
                                                        runat="server" SelectMethod="GetAllCurrentStudents" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountCurrentStudentRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="cmbStandard" Type="Int32" PropertyName="SelectedValue"
                                                                Name="aiStandardId" />
                                                            <asp:ControlParameter ControlID="cmbDivision" Type="Int32" PropertyName="SelectedValue"
                                                                Name="aiDivisionId" />
                                                            <asp:Parameter Name="asName" DefaultValue="" Type="String" />
                                                            <asp:Parameter Name="abIncludeUserName" DefaultValue="false" Type="Boolean" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID = "hidValFileUpload" runat = "server" />
                                                    <asp:HiddenField ID = "hidValFileUploadType" runat = "server" />
                                                    <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td align="left" style="width: 2px;">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientStandard = "<%=this.cmbStandard.ClientID%>"
        _clientDivision = "<%=this.cmbDivision.ClientID%>"
        _clientFileUploadClientId = "<%=this.fileUploadStudents.ClientID%>"
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        _clientbtnImportStudent = "<%=this.btnImportStudent.ClientID%>"
        _clientimgbtnBack = "<%=this.imgbtnBack.ClientID%>"
        _clientlblHead = "<%=this.lblHead.ClientID%>"
        function ClearLabel() {
            if (document.getElementById(_clientlblHead)) {
                document.getElementById(_clientlblHead).innerText = ""
                document.getElementById(_clientlblHead).innerHTML = ""
            }
        }
        function validateFile(source, args) {
            ClearLabel()
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var Extension = oFileName.toUpperCase().substring(oFileName.indexOf("."))
            var bIsValid = true
            if (oFileName != "") {
                if (oFileName.toUpperCase().indexOf(".XLS") == -1 && oFileName.toUpperCase().indexOf(".XLSX") == -1) {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage =
document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value;
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage =
document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value;
                }
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage =
 document.getElementById("<%=this.hidValFileUpload.ClientID %>").value;
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportStudent)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function')
                    isPageValid = Page_ClientValidate()
                if (isPageValid) {
                    document.getElementById(_clientbtnImportStudent).disabled = true
                    document.getElementById(_clientimgbtnBack).disabled = true
                    document.getElementById(_clientStandard).disabled = true
                    document.getElementById(_clientDivision).disabled = true
                }
            }
            else if (ObjBtn == document.getElementById(_clientimgbtnBack)) {
                document.getElementById(_clientbtnImportStudent).disabled = true
                document.getElementById(_clientimgbtnBack).disabled = true
                document.getElementById(_clientStandard).disabled = true
                document.getElementById(_clientDivision).disabled = true
            }
        }
    </script>
</asp:Content>
