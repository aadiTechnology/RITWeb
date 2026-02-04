<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ImportStudentHealthDetailsUI.aspx.cs" Inherits="ImportStudentHealthDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center">
                    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                        <tr>
                            <td align="right" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" colspan="3">
                                            <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                            <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                                             <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                ControlToValidate="FUStudentHealth" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileType%>"></asp:CustomValidator>
                                            <%--</ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />                                                    
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                        <td>
                                            <div style="float: right">
                                                <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                                    ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download template for adding students health details by import."></asp:HyperLink>                                                
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Label ID="lblHead" runat="server" Text="<%$ Resources:LocalizedResources, FileUplpadSuccessfully%>"
                                                Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                        <tr align="center">
                            <td align="center">
                                <table border="0" cellpadding="0" cellspacing="3" align="center" style="text-align: center;
                                    margin: 0px auto;">
                                    <tr align="center" style="text-align: center; margin: 0px auto;">
                                        <td align="center" style="padding-left : 80px;">
                                            <asp:Label CssClass="ClsLabel" ID="lblSelectFile" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, SelectFile %>"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:FileUpload ID="FUStudentHealth" runat="server" />                                                                             
                                        </td>                                        
                                    </tr>
                                    <tr align="center" style="text-align: center; margin: 0px auto;">
                                        <td colspan="3" align="left" style="text-align: left; margin: 0px auto; padding-left : 85px;">
                                            <asp:Label CssClass="LblSmlGray" ID="lblNoteFileType" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, FileType %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="clspaddingsmallt" colspan="3">
                                            <asp:Button ID="imgbtnBack" Text="<%$ Resources:LocalizedResources, Back%>" runat="server"
                                                CssClass="ClsBtnSml" BorderStyle="Solid" PostBackUrl="~/RITeSchool/Common/ControlPanel.aspx"
                                                Visible="True" BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false" />
                                            <asp:Button ID="btnImportStudent" Text="<%$ Resources:LocalizedResources, ImportStudent%>"
                                                runat="server" CssClass="ClsBtnMid" BorderStyle="Solid" Visible="True" CausesValidation="true"
                                                BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnImportStudent_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-top: 2px solid #C0C0C0;">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr align="center" style="text-align: center; margin: 0px auto;">
                            <td align="center" style="text-align: center; margin: 0px auto;">
                                <table align="center">
                                    <tr>
                                        <td align="center" colspan="1" class="ClsOnlyBorderlght" style="width: 150px;">
                                            <asp:Label CssClass="ClsLabel" ID="lblStandard" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" CssClass="SmlCombo"
                                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                            <asp:Label CssClass="ClsLabel" ID="lblDivision" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:UpdatePanel ID="UPanelDivision" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="True" CssClass="SmlCombo"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                            <asp:Label CssClass="ClsLabel" ID="Label1" runat="server" EnableViewState="False"
                                                Text="Student Name/ Registration No."></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:TextBox ID="txtFilter" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" CausesValidation="false"
                                                OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr id="trItemCount" runat="server">
                                                <td align="center" style="width: 100%;">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentList"
                                                        Visible="true">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                                        Text="<%# Container.StartRowIndex + 1%>" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text=" To " />
                                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text=" Out Of " />
                                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                        Text="Records " />
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwStudentList" runat="server" DataKeyNames="StudentId" OnItemDataBound="lstvwStudentList_ItemDataBound"
                                                        OnDataBound="lstvwStudentList_DataBound">
                                                        <LayoutTemplate>
                                                            <table width="80%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder"
                                                                align="center">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="center" width="80px" class="clsLabelgrd">
                                                                        <span><b>Roll No.</b></span>
                                                                    </th>
                                                                    <th align="left" width="120px" class="clsLabelgrd">
                                                                        <span><b>Reg. No.</b></span>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd">
                                                                        <span><b>Student Name</b></span>
                                                                    </th>
                                                                    <th align="left" class="clsLabelgrd" width="120px">
                                                                        <span><b>Class Name</b></span>
                                                                    </th>
                                                                    <th align="center" class="clsLabelgrd" width="230px">
                                                                        <span><b>Father Aadhar Card No.</b></span>
                                                                    </th>
                                                                    <th align="center" class="clsLabelgrd" width="252px">
                                                                        <span><b>Mother Aadhar Card No.</b></span>
                                                                    </th>
                                                                    <th align="center" class="clsLabelgrd" width="150px">
                                                                        <span><b>Monthly Income</b></span>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                    <td colspan="7" align="left">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStudentList">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
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
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center" style="text-align: center; font-size: 9pt; font-family: Arial;">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="text-align: left; font-size: 9pt; font-family: Arial; padding-left: 5px;">
                                                                    <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("EnrolmentNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblClassName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblFatherAadhar" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("FatherAadharCardNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMotherAadhar" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("MotherAadharCardNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMonthlyIncome" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("FamilyMonthlyIncome") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center" style="text-align: center; font-size: 9pt; font-family: Arial;">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" style="text-align: left; font-size: 9pt; font-family: Arial; padding-left: 5px;">
                                                                    <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("EnrolmentNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblClassName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblFatherAadhar" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("FatherAadharCardNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMotherAadhar" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("MotherAadharCardNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMonthlyIncome" runat="server" Style="float: inherit; text-align: center;
                                                                        font-size: 9pt; font-family: Arial;" Text='<%#Eval("FamilyMonthlyIncome") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="80%" align="center" style="text-align: center; margin: 0px auto;">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.HealthDetailsBL" EnablePaging="true"
                                                        ID="lstvwDSobj" runat="server" SelectMethod="GetStudentDetailsForImport" SelectCountMethod="Count"
                                                        EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="cmbStandard" Type="String" PropertyName="SelectedValue"
                                                                Name="asStandardId" />
                                                            <asp:ControlParameter ControlID="cmbDivision" Type="String" PropertyName="SelectedValue"
                                                                Name="asDivisionId" />
                                                            <asp:ControlParameter ControlID="txtFilter" Type="String" PropertyName="Text" Name="asFilter" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hidValFileUpload" runat="server" />
                                                    <asp:HiddenField ID="hidValFileUploadType" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clientFUStudentHealth = "<%=this.FUStudentHealth.ClientID %>";
            _clientlblHead = "<%=this.lblHead.ClientID %>";
            _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
            function validateFile(source, args) {
                ClearLabel()
                var oFileName = document.getElementById(_clientFUStudentHealth).value
                var Extension = oFileName.toUpperCase().substring(oFileName.indexOf("."))
                var bIsValid = true
                if (oFileName != "") {
                    if (oFileName.toUpperCase().indexOf(".XLS") == -1 && oFileName.toUpperCase().indexOf(".XLSX") == -1) {
                        bIsValid = false
                        document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value;
                    }
                    else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                        bIsValid = false
                        document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value;
                    }
                }
                else {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUpload.ClientID %>").value;
                }
                args.IsValid = bIsValid
                return !bIsValid
            }

            function ClearLabel() {
                if (document.getElementById(_clientlblHead)) {
                    document.getElementById(_clientlblHead).innerText = ""
                    document.getElementById(_clientlblHead).innerHTML = ""
                }
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
