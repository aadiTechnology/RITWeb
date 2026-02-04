<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadUserDocumentsUI.aspx.cs" Inherits="UploadUserDocumentsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td id="tdMessage" runat="server" align="center">
                                <%--                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                    ValidationGroup="Document" />
                                <asp:CustomValidator ID="cstDocument" runat="server" ErrorMessage="" ClientValidationFunction="CheckDocumentIsUploaded"
                                    Display="None" ValidationGroup="Document"></asp:CustomValidator>
                                <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server">
                                <table cellpadding="0" width="25%" cellspacing="2">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:RadioButton ID="optDocumentWise" runat="server" AutoPostBack="True" GroupName="Notice"
                                                Text="DocumentWise File Upload" Checked="True" OnCheckedChanged="optDocumentWise_CheckedChanged">
                                            </asp:RadioButton>
                                            <asp:RadioButton ID="optUserWise" runat="server" GroupName="Notice" Text="UserWise File Upload"
                                                AutoPostBack="True" OnCheckedChanged="optUserWise_CheckedChanged"></asp:RadioButton>
                                        </td>
                                    </tr>
                                    <tr id="documenttype" runat="server">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Document Type :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:DropDownList ID="cmbDocumentType" runat="server" AutoPostBack="true" CssClass="MidCombo">
                                                <%-- <asp:ListItem Value="1" Text="Form No 16"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Aadhar Card"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="PAN No"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Photo"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Salary Document"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Personal Imformation"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">User Role :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:DropDownList ID="cmbUserRole" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trnewclass" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Class : </span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                           <asp:DropDownList ID="cmbnewclass" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                 OnSelectedIndexChanged="cmbnewclass_SelectedIndexChanged">
                                           </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trusername" runat="server">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">User Name :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="MidTxtBox" MaxLength="100"
                                                autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                       <tr id = "trleftStudent" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Include left students? </span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                             <asp:CheckBox ID="chkLeftStudent"  runat="server" AutoPostBack="true" OnCheckedChanged="chkLeftStudent_CheckedChanged" 
                                                     ViewStateMode="Enabled" />
                                                                           
                                        </td>
                                    </tr>
                                    <tr id="trsearch" runat="server">
                                        <td colspan="2" align="center" class="ClsBorderlight">
                                            &nbsp;<asp:Button ID="btnSearch" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                                Text="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                    <tr id="trClass" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Class : </span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                           <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <asp:DropDownList ID="cmbClass" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                        OnSelectedIndexChanged="cmbClass_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                              <%--  </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <tr id="truser" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">User Name :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                           <%-- <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <asp:DropDownList ID="cmbUser" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                        OnSelectedIndexChanged="cmbUser_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                              <%--  </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <tr id="trpanno" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Pan No. :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblpanNo" runat="server" Text="" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trEmpNo" runat="server" visible="false">
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">Employee No. :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="lblEmpNo" runat="server" Text="" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trnote" runat="server">
                                        <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 10px">
                                            <span class="LblSmlGray">Support PDF,PNG,JPEG,JPG,.DOCX,.Xlsx files types.</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="DocumentWiseLstvw" runat="server">
                            <td align="center">
                                <%--   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <table width="100%">
                                    <tr>
                                        <td valign="top" align="center">
                                          <%--  <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <table width="100%">
                                                        <tr id="trPhotoPager" runat="server">
                                                            <td align="center">
                                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="10" PagedControlID="lstvwUserDocuments">
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
                                                            <td align="center">
                                                                <asp:ListView ID="lstvwUserDocuments" runat="server" DataKeyNames="UserId, DocumentFilePath, DocumentId, DocumentTypeId, DocumentTypeName, RowNo"
                                                                    OnDataBound="lstvwUserDocuments_DataBound" OnItemDataBound="lstvwUserDocuments_ItemDataBound"
                                                                    OnItemCommand="lstvwUserDocuments_ItemCommand">
                                                                    <LayoutTemplate>
                                                                        <table width="80%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                            cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="center">
                                                                                    Sr. No.
                                                                                </th>
                                                                                <th class="paddingLR" align="left">
                                                                                    Name
                                                                                </th>
                                                                                <th class="paddingLR" align="left" id="thpanno" runat="server">
                                                                                    Pan No.
                                                                                </th>
                                                                                <th class="paddingLR" align="left" id="thEmpNo" runat="server">
                                                                                    Employee No.
                                                                                </th>
                                                                                <th class="paddingLR" align="left">
                                                                                    Document Browse
                                                                                </th>
                                                                                <th class="paddingLR" align="center">
                                                                                    View Document
                                                                                </th>
                                                                                <th class="paddingLR" align="center" style="width: 50px;">
                                                                                    Remove
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="7">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUserDocuments" PageSize="20">
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
                                                                            <td align="center" width="9%">
                                                                                <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' />
                                                                            </td>
                                                                            <td width="35%" align="left">
                                                                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="paddingLR" />
                                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("DocumentTypeName")%>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td align="left" width="15%" id="tdSelect" runat="server">
                                                                                <asp:Label ID="lblPanNo" runat="server" Text='<%# Eval("PanNo") %>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td align="left" width="15%" id="tdEmpNo" runat="server">
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("EmployeeNo") %>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td class="paddingLR" align="left" width="25%">
                                                                                <asp:FileUpload ID="FileUploadDoc" runat="server" />
                                                                                <asp:HiddenField ID="hidDocFile" runat="server" Value="" />
                                                                            </td>
                                                                            <td class="paddingLR" align="center" width="15%">
                                                                                <asp:ImageButton ID="imgView" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                    Visible="false" />
                                                                            </td>
                                                                            <td class="paddingLR" align="center" width="5%">
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Visible="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                            <td align="center" width="9%">
                                                                                <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' />
                                                                            </td>
                                                                            <td width="35%" align="left">
                                                                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="paddingLR" />
                                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("DocumentTypeName")%>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td align="left" width="15%" id="tdSelect" runat="server">
                                                                                <asp:Label ID="lblPanNo" runat="server" Text='<%# Eval("PanNo") %>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td align="left" width="15%" id="tdEmpNo" runat="server">
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("EmployeeNo") %>' CssClass="paddingLR" />
                                                                            </td>
                                                                            <td class="paddingLR" align="left" width="25%">
                                                                                <asp:FileUpload ID="FileUploadDoc" runat="server" />
                                                                                <asp:HiddenField ID="hidDocFile" runat="server" Value="" />
                                                                            </td>
                                                                            <td class="paddingLR" align="center" width="15%">
                                                                                <asp:ImageButton ID="imgView" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                    Visible="false" />
                                                                            </td>
                                                                            <td class="paddingLR" align="center" width="5%">
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Visible="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <table width="70%">
                                                                            <tr>
                                                                                <td class="LblNoRecord" align="center">
                                                                                    No record found.
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.UploadUserDocumentBL" EnablePaging="true"
                                                                    ID="lstvwDsObj" runat="server" SelectMethod="GetUserDetailsForDocumentUpload"
                                                                    SelectCountMethod="CountUserForDocumentUplaod" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="int32" />
                                                                        <asp:ControlParameter ControlID="cmbDocumentType" PropertyName="SelectedValue" Name="aiDocumentTypeId" />
                                                                        <asp:ControlParameter ControlID="cmbUserRole" PropertyName="SelectedValue" Name="aiUserRoleId" />
                                                                        <asp:ControlParameter ControlID="txtUserName" PropertyName="Text" Name="asUserName"
                                                                            DefaultValue="" />
                                                                        <asp:ControlParameter ControlID="cmbnewclass" PropertyName="SelectedValue" Name="aiStandardDivisionId" />
                                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                                        <asp:Parameter Name="sortDirection" Type="String" />
                                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                        <asp:ControlParameter ControlID="cmbUser" PropertyName="SelectedValue" Name="aiUser" />
                                                                          <asp:ControlParameter ControlID="chkLeftStudent" PropertyName="Checked" Name="asLeftStudent"
                                                                            DefaultValue="" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <asp:HiddenField ID="hidCount" runat="server" />
                                                                <asp:HiddenField ID="hidPageNo" runat="server" />
                                                                <asp:HiddenField ID="hidRowCnt" runat="server" />
                                                                <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidAcademicYearId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                               <%-- </ContentTemplate>
                                                <Triggers>                                                    
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUser" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwUserDocuments" EventName="ItemCommand" />
                                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                </table>
                                <%--  </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="1">
                                 <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <asp:Button ID="btnUpload" runat="server" Text="Save" CssClass="ClsBtn" ValidationGroup="Document"
                                    OnClick="btnUpload_Click" />
                               <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="cmbUser" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwUserDocuments" EventName="ItemCommand" />
                                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwUserDocuments.ClientID %>";
        _clienthidCountID = "<%=this.hidCount.ClientID %>";
        _clientcstDocument = "<%=this.cstDocument.ClientID %>"

        function ConfirmDelete() {

            return confirm('Are you sure you want to remove document of this User?');
        }

        function OpenDocument(FilePath) {
            window.open("../DOWNLOADS/User Documents/" + FilePath,'_new');
            return false;
        }

        function CheckDocumentIsUploaded(oSrc, args) {
            var iRowCount = document.getElementById(_clienthidCountID).value;
            document.getElementById(_clientcstDocument).errormessage = "";
            var sMessage = "";
            var iCnt = 0;
            var iSizeCount = 0;
            var TotalFileCount = 0;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i;
                var UploadFile = _clientListViewID + "_ctrl" + RowNumber + "_" + "FileUploadDoc";
                var PhotoCapturedStatus = _clientListViewID + "_ctrl" + RowNumber + "_hidDocUploadStatus";

                var fileName = document.getElementById(UploadFile).value;

                if (document.getElementById(UploadFile).value != "") {
                    TotalFileCount++;
                    if (!CheckFileType(fileName))//if file type is valid
                    {
                        if (iCnt == 0)
                            iCnt = (parseInt(RowNumber) + 1);
                        else
                            iCnt = iCnt + ", " + (parseInt(RowNumber) + 1);
                    }
                    else {
                        var maxFileSize = 1024 // 1MB -> 1 * 1024 – to check in KB – Kilo Bytes
                        var fileUpload = document.getElementById(UploadFile);
                        var size = parseFloat(fileUpload.files[0].size / 1024);

                        if (size > maxFileSize) {
                            if (iSizeCount == 0)
                                iSizeCount = (parseInt(RowNumber) + 1);
                            else
                                iSizeCount = iSizeCount + ", " + (parseInt(RowNumber) + 1);
                        }
                    }
                }
            }

            if (iCnt != 0 && iSizeCount == 0) {
                sMessage = "Invalid file format at row number(s): " + iCnt + ". ";
                document.getElementById(_clientcstDocument).errormessage += "Invalid file format at row number(s): " + iCnt + ". ";
            }
            else if (iCnt == 0 && iSizeCount != 0) {
                sMessage = "Uploaded file should not be grater than 1Mb for row number(s): " + iSizeCount + ". ";
                document.getElementById(_clientcstDocument).errormessage += "Uploaded file should not be grater than 1Mb for row number(s):  " + iSizeCount + ". ";
            }
            else if (iCnt != 0 && iSizeCount != 0) {
                sMessage = "Uploaded file should not be grater than 1Mb for row number(s): " + iSizeCount + ". " + " And Invalid file format for row number(s): " + iCnt + ". ";
                document.getElementById(_clientcstDocument).errormessage += "Uploaded file should not be grater than 1Mb for row number(s): " + iSizeCount + ". " + " And Invalid file format for row number(s): " + iCnt + ". ";
            }

            if (TotalFileCount == 0) {
                sMessage = "There is no photo to upload.";
                document.getElementById(_clientcstDocument).errormessage = "There is no document to upload.";
            }

            if (sMessage == "") {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }

        //This function is used to check file type.
        function CheckFileType(sFileName) {
            var bIsValid;
            if (sFileName != "") {

                var extension = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();

                if (extension == ".PDF" || extension == ".JPEG" || extension == ".JPG" || extension == ".PNG" || extension == ".DOCX" || extension == ".DOC" || extension == ".XLS" || extension == ".XLSX")
                    bIsValid = true;
                else
                    bIsValid = false;
            }
            else
                bIsValid = false;

            return bIsValid;
        }

        function ResetMessage() {
            $('#' + "<%=lblMessage.ClientID%>").html('')
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = $('#' + "<%=hidSchoolId.ClientID %>").val();
            var AcademicYearId = $('#' + "<%=hidAcademicYearId.ClientID %>").val();
            _clientddlUserRole = '<%=cmbUserRole.ClientID%>';

            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _slienttxtUserName, _clientddlUserRole, 1, null, null, null);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
