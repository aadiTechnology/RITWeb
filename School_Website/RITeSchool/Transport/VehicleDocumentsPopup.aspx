<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="VehicleDocumentsPopup.aspx.cs" Inherits="VehicleDocumentsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                <tr>
                                                    <td align="center" class="MainTitleHead" style="height: 20px">
                                                        <span style="font-weight: bold">Vehicle Documents</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="float: right; vertical-align: top;">
                                                <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                    <asp:RequiredFieldValidator ID="RequiredDocumentName" runat="server" ErrorMessage="Document should be selected."
                                                        Display="None" ControlToValidate="ddlDocuments" InitialValue="0"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredStartDate" runat="server" ErrorMessage="Start date should not be blank."
                                                        Display="None" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredTitle" runat="server" ErrorMessage="Title should not be blank."
                                                        Display="None" ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
                                                        Display="None" ControlToValidate="txtDescription" ErrorMessage="Length of Description should not be more than 500."
                                                        CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="reqValEndDate" runat="server" ErrorMessage="End Date should not be blank." Enabled="false"
                                                        Display="None" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="Start Date should be less than end date." Display="None" ClientValidationFunction="ValidateEndDate"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="reqValPolicyNo" runat="server" ErrorMessage="Policy No. should not be blank."
                                                        Enabled="false" Display="None" ControlToValidate="txtPolicyNo"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="reqValAmount" runat="server" ErrorMessage="Amount should not be blank."
                                                        Enabled="false" Display="None" ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cstFileType" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFile"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="csFileSizeTotal" runat="server" ClientValidationFunction="ValidateFileSize"
                                                        CssClass="ClsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="DocumentDate_Validate"
                                                        CssClass="ClsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" OnServerValidate="DocumentTitle_Validate"
                                                        CssClass="ClsLabel" Display="None" ErrorMessage="Title should not be duplicate for selected document."></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator3" runat="server" OnServerValidate="DocumentInsuranceDetails_Validate"
                                                        CssClass="ClsLabel" Display="None" ErrorMessage="Policy Number should not be duplicate for selected document."></asp:CustomValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDocuments" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwDocuments" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td id="tdMessage" align="center">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" Height="20px"
                                                        Width="100%" Text="" EnableViewState="false"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDocuments" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwDocuments" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="Label1" runat="server" Text="Vehicle Number:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td class="ClsHilightBGB">
                                                                <asp:Label ID="lblVehicleNumber" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="lblDocuments" runat="server" Text="Document:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td id="Td1" align="left" runat="server">
                                                                <asp:DropDownList ID="ddlDocuments" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                                    Width="190px" OnSelectedIndexChanged="ddlDocuments_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trTimeSpan" runat="server">
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtStartDate" CssClass="SmlCombo" runat="server" AutoPostBack="true"></asp:TextBox>
                                                                <rjs:PopCalendar ID="CalStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                                    ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start date." />
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trEndDate" runat="server" visible="false">
                                                            <td align="left" class="ClsBorderLight">
                                                                <asp:Label ID="lblEndDate" runat="server" Text="End Date:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtEndDate" CssClass="SmlCombo" runat="server" AutoPostBack="true"></asp:TextBox>
                                                                <rjs:PopCalendar ID="CalEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                                    ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid End date." />
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPolicy" runat="server" visible="false">
                                                            <td align="left" class="ClsBorderlight">
                                                                <asp:Label ID="lblPolicyNo" runat="server" Text="Policy No:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtPolicyNo" runat="server" MaxLength="20"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAmount" runat="server" visible="false">
                                                            <td align="left" class="ClsBorderlight">
                                                                <asp:Label ID="lblAmount" runat="server" Text="Amount:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" onblur="extractNumber(this,2,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                    <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trTitle" runat="server">
                                                            <td align="left" class="ClsBorderlight">
                                                                <asp:Label ID="lblTitle" runat="server" Text="Title:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtTitle" runat="server" class="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trDescription" runat="server">
                                                            <td align="left" class="ClsBorderlight">
                                                                <asp:Label ID="lblDescription" runat="server" Text="Description:" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="ExLrgTxtBox"
                                                                    Height="50px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight" style="height: 28px">
                                                                <span class="ClsLabel">Upload Document :</span>
                                                            </td>
                                                            <td align="left" style="height: 28px">
                                                                <asp:FileUpload ID="flDocument" runat="server" />
                                                                <span class="ClsMdtStar">*</span>
                                                                <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="UpdateUploadedFile"
                                                                    ToolTip="Update" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <span class="LblSmlGray">(Attachment supports files of types - .BMP, .JPG, .JPEG, .PDF,
                                                                    .PNG upto 5 MB.)</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDocuments" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwDocuments" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="BtnSave_Click" />
                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" width="100%">
                                            <tr runat="server" id="trTotalRec" align="center">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwDocuments">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1 %>" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize %>" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount %>" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="tblnamelist" align="center" width="98%">
                                                        <tr>
                                                            <td valign="top" align="center">
                                                                <asp:ListView ID="lstvwDocuments" runat="server" OnItemDataBound="lstvwDocuments_ItemDataBound"
                                                                    OnDataBound="lstvwDocuments_DataBound" DataKeyNames="Id,FileName" OnItemCommand="lstvwDocuments_ItemCommand">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" id="tblStaffInfo" style="color: #333333" cellpadding="0"
                                                                            cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingL" width="150px">
                                                                                    Document Name
                                                                                </th>
                                                                                <th align="left" class="paddingL">
                                                                                    Title
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    Start Date
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    End Date
                                                                                </th>
                                                                                <th align="center" width="50px">
                                                                                    View
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
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="7">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwDocuments" PageSize="20">
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
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%#Eval("StartDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                    ToolTip="View" CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateVehicleDocumentDetails"
                                                                                    ToolTip="Update" ImageUrl="../images/IconGrid_Edit.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteVehicleDocumentDetails"
                                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("DocumentName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%#Eval("StartDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                    ToolTip="View" CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateVehicleDocumentDetails"
                                                                                    ToolTip="Update" ImageUrl="../images/IconGrid_Edit.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteVehicleDocumentDetails"
                                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <div class="LblNoRecord">
                                                                            No Record Found.
                                                                        </div>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.VehicleDocumentBL" EnablePaging="true"
                                                                    ID="objdsDocumentDetails" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                                    SelectCountMethod="GetCount" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="Int32" />
                                                                        <asp:ControlParameter ControlID="hidVehicleId" Name="aiVehicleId" Type="Int32" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="ddlDocuments" Name="aiDocumentId" Type="Int32" PropertyName="SelectedValue" />
                                                                        <asp:Parameter Name="SortExpression" Type="String" />
                                                                        <asp:Parameter Name="SortDirection" Type="String" />
                                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                <asp:HiddenField ID="hidVehicleId" runat="server" />
                                                                <asp:HiddenField ID="hidFileUpload" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                                    OnClientClick="CloseWindow();return false;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlDocuments" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwDocuments" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
            _clientddlDocuments = "<%=this.ddlDocuments.ClientID %>"
            _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
            _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
            _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?')
            }


            function ValidateFile(oSrc, args) {
                var fl = $get("<%=this.flDocument.ClientID %>").value;
                var uploadedFile = $get("<%=this.hidFileUpload.ClientID %>").value

                if (fl == "" && uploadedFile == "") {
                    oSrc.errormessage = "Please select file to upload.";
                    args.IsValid = false;
                    return true;
                }

                if (fl != "") {
                    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF"
                                )) {
                        oSrc.errormessage = "Please select valid file type.";
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            function ResetMessage() {
                if ($get("<%=this.lblMessage.ClientID %>") != null)
                    $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

            function CloseWindow() {
                window.close();
            }

            function OpenFile(file) {
                window.open(file, '_blank')
                return false;
            }

            function ValidateFileSize(oSrc, args) {
                var fl = $get("<%=this.flDocument.ClientID %>");
                if (fl.value != '' && fl.files[0].size >= 5242880) {
                    oSrc.errormessage = "File size should be less than 5 MB."
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }

            function ValidateEndDate(oSrc, args) {
                var dtStartDate = document.getElementById(_clienttxtStartDate).value;
                
                var dtEndDate = ''
                if (document.getElementById(_clienttxtEndDate) != null)
                    dtEndDate = document.getElementById(_clienttxtEndDate).value;

                if (dtStartDate != '' && dtEndDate != '' && dtStartDate != undefined && dtEndDate != undefined) {
                    var startDate;
                    if (document.all)
                        startDate = new Date(dtStartDate.replace('-', ' '));
                    else
                        startDate = new Date(convertdate(dtStartDate));

                    var endDate;
                    if (document.all)
                        endDate = new Date(dtEndDate.replace('-', ' '));
                    else
                        endDate = new Date(convertdate(dtEndDate));

                    if (startDate > endDate) {
                        args.IsValid = false
                        return true;
                    }
                    else {
                        args.IsValid = true;
                        return false;
                    }
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
            
        </script>
    </div>
</asp:Content>
