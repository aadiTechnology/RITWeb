<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="ImportBookUI.aspx.cs" Inherits="ImportBookUI" %>

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
                                        <td align="left" colspan="4">
                                            <asp:ValidationSummary ID="valErrorMsg" runat="server" CssClass="ClsLabel" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <asp:Label ID="lblHead" runat="server" Text="Your file has been imported sucessfully."
                                                Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div style="float: right">
                                    <asp:HyperLink ID="lnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                        ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download the template for adding book by template."></asp:HyperLink>
                                    <br />
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                    <tr>
                                        <td align="center" colspan="6">
                                            <table border="0" cellpadding="0" cellspacing="3">
                                                <tr>
                                                    <td align="left" colspan="1">
                                                        <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                            ControlToValidate="fileUploadBooks" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="Invalid file type."></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                        <%--<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Select File : " EnableViewState="False"></asp:Label>--%>
                                                        <span class="ClsLabel">Select File : </span>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                        <asp:FileUpload ID="fileUploadBooks" runat="server" />
                                                        <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                    </td>
                                                    <td align="center" colspan="1">
                                                        <%--<asp:Label ID="Label3" runat="server" 
                                                            Text="  (Supports only .XLS/.XLSX files type)" CssClass="LblSmlGray" 
                                                            EnableViewState="False"></asp:Label>--%>
                                                        <span class="LblSmlGray">(Supports only .XLS/.XLSX files type)</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td align="left"  colspan="1">
                                                        <%--<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Select File : " EnableViewState="False"></asp:Label>--%>
                                                        <span class="ClsLabel"> </span>
                                                    </td>
                                                    <td align="left"  colspan="1">
                                                        <asp:Button ID="btnImportBook" Text="Import Books" runat="server" CssClass="ClsBtnMid"
                                                            BorderStyle="Solid" OnClick="btnImportBook_Click" Visible="True" CausesValidation="true"
                                                            BorderWidth="1px" UseSubmitBehavior="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top" style="width: 100%">
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <table align="center" cellpadding="2" width="100%">
                                                        <div style="float: none" id="divlbl" runat="server">
                                                            <table>
                                                                <tr runat="server" id="trTotalRec" align="center">
                                                                    <td>
                                                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">To</span>
                                                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">Out Of</span>
                                                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">Records</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:GridView ID="grdvwImportBooks" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    CssClass="GridBorder" ForeColor="#333333" EmptyDataText="No Record Found" DataSourceID="GrdDSobj"
                                                                    AllowPaging="True" CellPadding="0" CellSpacing="1" DataKeyNames="Book_Id" OnRowDataBound="grdvwImportBooks_RowDataBound"
                                                                    GridLines="None" OnPageIndexChanging="grdvwImportBooks_PageIndexChanging" PageSize="20"
                                                                    OnRowCreated="grdvwImportBooks_RowCreated" OnSorting="grdvwImportBooks_Sorting"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Book Title" DataField="Book_Title" SortExpression="Book_Title"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Author" DataField="Author_Name" SortExpression="Author_Name"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Category" DataField="Category_Name" SortExpression="Category_Name"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Available" DataField="Available_Books" 
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Total" DataField="Total_Book_Quantity" HtmlEncode="False"
                                                                           >
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Edition" DataField="BookEdition" 
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField HeaderText="Book Year" DataField="BookYear" 
                                                                            HtmlEncode="False">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <PagerTemplate>
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center"
                                                                        VerticalAlign="Middle" />
                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                </asp:GridView>
                                                                <asp:ObjectDataSource ID="GrdDSobj" runat="server" TypeName="BusinessLogic.BookBL"
                                                                    EnablePaging="true" SelectMethod="GetImportBookList" SortParameterName="sortExpression"
                                                                    SelectCountMethod="CountImportRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="asBookName" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="aiMediaType" PropertyName="Value"
                                                                            DefaultValue="2" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="aiMainCategoryId" PropertyName="Value"
                                                                            DefaultValue="0" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="asAuthorName" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="asPublisher" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="asDescription" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidTempBookDetails" Name="asAccessionNumber" PropertyName="Value" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                <asp:HiddenField ID="hidTempBookDetails" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="imgbtnBack" Text="Back" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                    OnClick="imgbtnBack_Click" Visible="True" BorderWidth="1px" CausesValidation="false"
                                    UseSubmitBehavior="false" />
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
        _clientFileUploadClientId = "<%=this.fileUploadBooks.ClientID%>"
        _clientCustomValId = "<%=this.CstValFileType.ClientID%>"
        _clientbtnImportBook = "<%=this.btnImportBook.ClientID%>"
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
            var oCusVal = document.getElementById(_clientCustomValId)
            var bIsValid = true
            if (oFileName != "") {
                var sFileExtension = oFileName.substring(oFileName.lastIndexOf('.'))
                sFileExtension = sFileExtension.toUpperCase()
                if (sFileExtension != ".XLS" && sFileExtension != ".XLSX") {
                    bIsValid = false
                    oCusVal.errormessage = "File to be imported should be in valid format."
                }
            }
            else {
                bIsValid = false
                oCusVal.errormessage = "File to import should be selected."
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportBook)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportBook).disabled = true
                    document.getElementById(_clientimgbtnBack).disabled = true
                }
            }
            else if (ObjBtn == document.getElementById(_clientimgbtnBack)) {
                document.getElementById(_clientbtnImportBook).disabled = true
                document.getElementById(_clientimgbtnBack).disabled = true
            }
        }
    </script>

</asp:Content>
