<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="InvestmentDocumentPopup.aspx.cs" Inherits="InvestmentDocumentPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upnl" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <span style="font-weight: bold">Documents</span>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                    <asp:ValidationSummary ID="valSum" runat="server" />
                                                </td>
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
                                    <td align="center">
                                        <table width="80%">
                                            <tr>
                                                <td id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table width="80%">
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="150px">
                                                    <asp:Label ID="lblName" runat="server" Text="User Name:" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                                <td class="ClsHilightBGB">
                                                    <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Document Name :</span>
                                                </td>
                                                <td class="ClsHilightBGB">
                                                    <asp:Label ID="lblInvestmentMethod" runat="server" CssClass="ClsLabel"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Upload Document :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:FileUpload ID="flDocument" runat="server" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <span class="LblSmlGray">(Attachment supports files of types - .BMP, .DOC, .DOCX, .JPG,
                                                        .JPEG, .PDF, .XLS, .XLSX upto 5 MB.)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="BtnSave" Text="Upload" runat="server" CssClass="ClsBtn" OnClick="BtnSave_Click" />
                                                    <asp:CustomValidator ID="cstFileType" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFile"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <input type="hidden" id="hidDocumentTypeId" runat="server" />
                                        <asp:ListView ID="lstvwDocuments" runat="server" OnItemDataBound="lstvwDocuments_ItemDataBound"
                                            DataKeyNames="Id" OnItemCommand="lstvwDocuments_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="80%" runat="server" id="tblStaffInfo" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingL">
                                                            File Name
                                                        </th>
                                                        <th align="center" width="100px">
                                                            View
                                                        </th>
                                                        <th align="center" width="108px">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ToolTip="View" CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ToolTip="View" CommandName="DOWNLOAD" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
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
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                            OnClick="btnClose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                     
                </td>
                
            </tr>
            <asp:HiddenField ID="hidBtnState" runat="server" />
        </table>
        <script type="text/javascript" language="javascript">
            function ConfirmDelete() {
                var bResult = true
                if (!window.confirm('Are you sure you want to delete this record?')) {
                    bResult = false
                }
                return bResult
            }

            function ValidateFile(oSrc, args) {
                var fl = $get("<%=this.flDocument.ClientID %>").value;

                if (fl == "") {
                    oSrc.errormessage = "Please select file to upload.";
                    args.IsValid = false;
                    return true;
                }

                if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOC" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOCX" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLS" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLSX" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF"
                    )) {
                    oSrc.errormessage = "Please select valid file type.";
                    args.IsValid = false;
                    return true;
                }

                args.IsValid = true;
                return false;
            }

            function ResetMessage() {
                if ($get("<%=this.lblMessage.ClientID %>") != null)
                    $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            }

            function CloseNewWindow(ItemCount) {                
                window.opener.UpdateFileUploadCount(ItemCount);
                window.close();
                window.opener.focus();
            }

            function CloseAppWindow(ItemCount) {
                window.opener.UpdateFileUploadCount(ItemCount);
                window.close();
                window.opener.focus();
            }

            function CloseWindow() {
                window.close();
                window.opener.focus();
                window.opener.FilterPANDetails();
            }


            function ClosePerformanceWindow(Count, ClientId) {
                window.opener.focus();
                window.opener.RefreshLinkButton(Count, ClientId);
                window.close();
            }
        </script>
    </div>
</asp:Content>
