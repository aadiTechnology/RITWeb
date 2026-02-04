<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SupervisorUserListUI.aspx.cs" Inherits="SupervisorListUI" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td style="height: 20px">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top">
                                <asp:ValidationSummary ID="ValSummaryErrMsg" CssClass="LblErrorMsg" runat="server"
                                    ShowMessageBox="False" ShowSummary="True" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Label ID="lblHead" runat="server" Text= "<%$ Resources:LocalizedResources, FileUplpadSuccessfully%>"
                        Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td align="left">
                    <table id="LegendTable" runat="server">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, Legend%>" EnableViewState="false"></asp:Label>
                            </td>
                            <td align="left" style="padding-right: 3px">
                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                    BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text="<%$ Resources:LocalizedResources, DeactivatedUser%>"
                                    EnableViewState="false"></asp:Label>
                            </td>
                            <td align="right" style="width: 5px">
                            </td>
                        </tr>
                    </table>
                    <div style="float: right">
                        <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                            ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip= "<%$ Resources:LocalizedResources, ToolTipDownloadTemlate %>"></asp:HyperLink>
                        <br />
                       <span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
            <td height="30px">
                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" CssClass="LblErrorMsg" Visible="false"
                 EnableViewState="false"></asp:Label>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                        <tr>
                            <td align="center" colspan="6">
                                <table border="0" cellpadding="0" cellspacing="3" width="600px">
                                    <tr>
                                        <td align="left" class="ClsOnlyBorderlght" style="width: 100px; height: 68px;">
                                      <asp:Label CssClass = "ClsLabel" ID="lblSelectFile" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectFile%>"></asp:Label>
                                      <span class="colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 68px">
                                            <asp:FileUpload ID="fileUploadAdminStaff" runat="server" />
                                            <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                ControlToValidate="fileUploadAdminStaff" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                ErrorMessage="Invalid file type."></asp:CustomValidator>
                                            <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span> 
                                            <asp:Label CssClass = "LblSmlGray" ID="lblFileType" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, FileType%>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btnImportAdminStaff" Text= "<%$ Resources:LocalizedResources,Import %>" runat="server" CssClass="ClsBtn"
                                                BorderStyle="Solid" OnClick="btnImportAdminStaff_Click" Visible="True" CausesValidation="true"
                                                BorderWidth="1px" />
                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                                CausesValidation="False"  />
                                        </td>
                                    </tr>
                                </table>
                                <table align="center">
                                    <tr>
                                        <td style="height:10px;"></td>
                                    </tr>
                                                                     <tr align="center">
                                                                        <td align="left" class="ClsBorderlight">
                                                                        <asp:Label ID="Label1" runat="server" class="ClsLabel" Text="Name"></asp:Label>
                                                                         <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtName" TabIndex="1" runat="server" MaxLength="50" CssClass="MidTxtBox" autocomplete="off"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" TabIndex="2" CssClass="ClsBtnMid remove-margin-top"
                                                                                OnClick="btnSearch_Click" CausesValidation="false"/>
                                                                        </td>
                                                                    </tr>
                                                                   
                                                              
                                    <tr>
                                        <td align="center" style="width:150px;" class="ClsBorderlight">
                                                    <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="User Type"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                         <td align="center">                                                      
                                                <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" Width="132px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">                                                        
                                                </asp:DropDownList>                                             
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
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID = "grdSupervisors" EventName ="RowCommand"  />
                        </Triggers> 
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                        ID="uPnl">
                        <ContentTemplate><br>                        
                            <div id="divGridView" runat="server" style="width: 80%;">
                             <table>
                                                                    <tr runat="server" id="trTotalRec" align="center">
                                                                        <td >
                                                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />                                                                            
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To %>"
                                                                EnableViewState="false" />
                                                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf %>"
                                                                EnableViewState="false" />
                                                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records %>"
                                                                EnableViewState="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                <asp:GridView CssClass="GridBorder" ID="grdSupervisors" runat="server" Width="100%"
                                    AutoGenerateColumns="False" PageSize="20" DataKeyNames="ID,Supervisor_Id,Is_Locked,PhotoFilePath,BinaryPhotoImage"
                                    AllowSorting="True" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" 
                                    OnRowCreated="grdSupervisors_RowCreated" OnSorting="grdSupervisors_Sorting" OnRowDataBound="grdSupervisors_RowDataBound"
                                    EmptyDataText="No record available." EmptyDataRowStyle-HorizontalAlign="Center"
                                    OnRowCommand="grdSupervisors_RowCommand" OnPageIndexChanging="grdSupervisors_PageIndexChanging"  AllowPaging="true">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                        Font-Size="Small"></PagerStyle>
                                   
                                    <Columns>
                                        <asp:HyperLinkField  HeaderText="<%$ Resources:LocalizedResources, UserName %>"  SortExpression="Supervisor_First_Name" DataTextField="NameWithoutDesignation">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                       </asp:HyperLinkField>
                                        <asp:BoundField DataField="Designation" HeaderText="<%$ Resources:LocalizedResources, Designation  %>" SortExpression="Designation_Id">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Email_Address" HeaderText="<%$ Resources:LocalizedResources, Email%>" SortExpression="Email_Address">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Photo %>">
                                            <ItemTemplate>
                                                <asp:Image ID="imgPhotoUpload" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.GIF">
                                            <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Middle" />
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" CommandName="DELETE_SUPERVISOR" HeaderText = "<%$ Resources:LocalizedResources, Delete%>"
                                            Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                    
                                     <PagerTemplate >
                                                                        <table width="100%" cellpadding="0" cellspacing="0" >
                                                                            <tr>
                                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">                                                                                                
                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage %>" runat="server" CssClass="LblNrmlB" />
                                                                                    <span id="Span6" class="LblNrmlB colonPadding">:</span>
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
                                <asp:ObjectDataSource TypeName="BusinessLogic.SchoolWiseSupervisorMasterCollectionBL" EnablePaging="true"
                                                                    ID="GrdDSobj" runat="server" SelectMethod="FetchSchoolWiseSupervisorMasterDetails"  SortParameterName="asSortExpression"
                                                                    SelectCountMethod="CountSupervisor" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="string" />
                                                                        <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" Type="String" />
                                                                      <%-- <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" Type="String" />--%>
                                                                        <asp:ControlParameter Name="aiUserType" ControlID="ddlUserType" propertyname="SelectedValue" Type="int32" />
                                                                         <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                           <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                            <asp:ControlParameter Name="asFilter" ControlID="hidFilter" Type="String"  />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidIsConfig" runat="server" />

                                <asp:HiddenField ID="hidValFileUpload" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidValFileUploadType" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidDeleteAdminStaff" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                   <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                         
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 20px; padding-top: 5px">
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnBack_Click" CausesValidation="False" />
                    <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnAdd_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientGridId = "<%=this.grdSupervisors.ClientID %>"
        _clientFileUploadClientId = "<%=this.fileUploadAdminStaff.ClientID%>"
        _clientbtnImportAdminStaff = "<%=this.btnImportAdminStaff.ClientID%>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID%>"
        _clientlblHead = "<%=this.lblHead.ClientID%>"
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"

        function ConfirmAction(sActionName) {
            ClearLabel();
            var bResult = true
            if (!confirm((document.getElementById("<%=this.hidDeleteAdminStaff.ClientID %>").value))) {
                bResult = false
            }
            return bResult
        }

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
                    document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value
                }
                else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLS" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".XLSX") {
                    bIsValid = false
                    document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUploadType.ClientID %>").value
                }
            }
            else {
                bIsValid = false
                document.getElementById(_clientCustomValId).errormessage = document.getElementById("<%=this.hidValFileUpload.ClientID %>").value
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportAdminStaff)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportAdminStaff).disabled = true
                    document.getElementById(_clientbtnCancel).disabled = true
                }
            }
            else if (ObjBtn == document.getElementById(_clientbtnCancel)) {
                document.getElementById(_clientbtnImportAdminStaff).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
        }
    </script>

</asp:Content>
