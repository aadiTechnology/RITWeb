<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateMenuFilesUI.aspx.cs"
    Inherits="UpdateMenuFilesUI" MasterPageFile="../MasterPages/MasterPage.master"
    ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td style="width: 1097px">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td style="width: 800px">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="800px">
                                        <asp:Label ID="lblErrorMsg" runat="server" Style="text-align: left" EnableViewState="false"
                                            ForeColor="Red" Width="100%" CssClass="ClsMdtStar" />
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Update"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td align="center">
                                    <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="3">
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="False"
                                                    CssClass="ClsLabel" Font-Bold="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 150px" class="ClsBorderLight">
                                                <span class="ClsLabel">Menu/Sub Menu Name :</span>
                                            </td>
                                            <td style="width: 330px">
                                                 <asp:TextBox ID="txtTopSearch" runat="server" MaxLength="100" CssClass="LrgTxtBox" />
                                                 <asp:Button ID="btnTopSearch" runat="server" CssClass="ClsBtn" Text="Search" 
                                                     ValidationGroup="Update" disable-page="true" onclick="btnTopSearch_Click" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 100px" class="ClsBorderLight">
                                                <span class="ClsLabel">Menu Name :</span>
                                            </td>
                                            <td style="width: 330px">
                                                <asp:DropDownList ID="ddlMenus" runat="server" CssClass="LrgTxtBox" Style="width: 320px;"
                                                    OnSelectedIndexChanged="ddlMenus_SelectedIndexChanged" ViewStateMode="Enabled" />
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CompareValidator ID="valRequiredMenu" runat="server" ControlToValidate="ddlMenus"
                                                    ValueToCompare="0" Operator="NotEqual" Display="None" ValidationGroup="Update"
                                                    ErrorMessage="Menu Name should be selected." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 100px" class="ClsBorderLight">
                                                <span class="ClsLabel">Link Name :</span>
                                            </td>
                                            <td style="width: 330px">
                                                <asp:TextBox ID="txtLinkName" runat="server" MaxLength="400" CssClass="LrgTxtBox"
                                                    Width="320px" />
                                                <span class="ClsMdtStar">* </span>
                                                <asp:RequiredFieldValidator ID="valRequiredName" runat="server" ControlToValidate="txtLinkName"
                                                    Display="None" ValidationGroup="Update" ErrorMessage="Link Name should not be empty." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 100px" class="ClsBorderLight">
                                                <span class="ClsLabel">Select One :</span>
                                            </td>
                                            <td style="width: 330px">
                                                <asp:RadioButton ID="optFilePath" runat="server" GroupName="ShowFileURL" Text="File Path" />
                                                <asp:RadioButton ID="optURL" runat="server" GroupName="ShowFileURL" Text=" URL" />
                                            </td>
                                        </tr>
                                        <tr id="tblPathDetails" class="path">
                                            <td align="left" style="width: 100px" class="ClsBorderLight">
                                                <span class="ClsLabel">File Path :</span>
                                            </td>
                                            <td style="width: 330px">
                                                <asp:FileUpload ID="fileUploadItems" runat="server" CssClass="LrgTxtBox" Style="width: auto !important;" />
                                                <span class="ClsMdtStar" id="spanUploadFile" runat="server">* </span>
                                                <asp:CustomValidator ID="CstValFileUpload" runat="server" ValidationGroup="Update"
                                                    CssClass="LblErrorMsg" EnableClientScript="true" SetFocusOnError="True" ClientValidationFunction="ValidateUploadedFile" />
                                            </td>
                                        </tr>
                                        <tr class="path">
                                            <td colspan="2">
                                                <span class="ClsBorderLight LblSmlGray" style="display: inline-block; padding: 2px;">
                                                    Supported file types - .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .pps and .ppsx
                                                    (upto 5mb in size). </span>
                                            </td>
                                        </tr>
                                        <tr id="tblURLDetails" class="url" style="display:none;">
                                            <td align="left" class="ClsBorderLight">
                                                <span class="ClsLabel">URL :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFileURL" runat="server" MaxLength="400" CssClass="ExLrgTxtBox" Width="320px" />
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstValidateFileurl" runat="server" Display="None" ClientValidationFunction="ValidateFileURL"
                                                    ValidationGroup="Update"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" ValidationGroup="Update"
                                                    OnClientClick="ClearValidation()" OnClick="btnSave_Click" disable-page="true" />
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                    Text="Cancel" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:PostBackTrigger ControlID="btnCancel" />
                        <asp:PostBackTrigger ControlID="btnSearch" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwMenuFilesDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwMenuFilesDetails" EventName="DataBound" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr style="width: 90%; background-color: Black" align="center" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td class="ClsBorderlight" align="left" style="width: 250px">
                            <asp:Label ID="lblSearchMenu" runat="server" class="ClsLabel" Text="Search Parent Menu/Sub Menu/Link Name :"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="false" MaxLength="300" Width="300px"
                                CssClass="LrgTxtBox"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Search%>"
                                CausesValidation="false" OnClick="btnSearch_Click" ViewStateMode="Enabled" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" width="90%">
                            <tr id="trPager" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwMenuFilesDetails">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" EnableViewState="false" Text="<%# Container.StartRowIndex + 1 %>"
                                                        CssClass="LblNrmlB" />
                                                    <span class="LblNormal">To</span>
                                                    <asp:Label ID="TotalPagesLabel" runat="server" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize %>"
                                                        CssClass="LblNrmlB" />
                                                    <span class="LblNormal">Out Of </span>
                                                    <asp:Label ID="TotalItemsLabel" runat="server" Text="<%# Container.TotalRowCount %>"
                                                        CssClass="LblNrmlB" />
                                                    <span class="LblNormal">Records </span>
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwMenuFilesDetails" runat="server" DataSourceID="ObjDSMenuFilesDetails"
                                        DataKeyNames="Id, Path" OnDataBound="lstvwMenuFilesDetails_DataBound" OnItemCommand="lstvwMenuFilesDetails_ItemCommand"
                                        OnItemDataBound="lstvwMenuFilesDetails_ItemDataBound" ViewStateMode="Enabled">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblMenuFilesDetails" style="color: #333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader" style="font-size: 9pt;">
                                                    <th align="left" style="padding-left: 7px; width: 180px;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="SortRow" CommandArgument="ParentMenu"
                                                            CausesValidation="false" ForeColor="Black" Text="Parent Menu" />
                                                    </th>
                                                    <th align="left" style="padding-left: 7px; width: 325px;">
                                                        <asp:LinkButton ID="lnBtnSubMenuName" runat="server" CommandName="SortRow" CommandArgument="ConfigureSubMenuName"
                                                            CausesValidation="false" ForeColor="Black" Text="Sub Menu Name" />
                                                    </th>
                                                    <th align="left" style="padding-left: 7px; width: 225px;">
                                                        <asp:LinkButton ID="lnkBtnMenuName" runat="server" CommandName="SortRow" CommandArgument="ConfigureMenuName"
                                                            CausesValidation="false" ForeColor="Black" Text="Menu Name" />
                                                    </th>
                                                    <th align="left" style="padding-left: 7px; width: 275px;">
                                                        <asp:LinkButton ID="lnkChildSubMenuName" runat="server" CommandName="SortRow" CommandArgument="ChildMenuName"
                                                            CausesValidation="false" ForeColor="Black" Text="Child Sub Menu Name" />
                                                    </th>
                                                    <th align="left" style="padding-left: 7px; width: 350px;">
                                                        <asp:LinkButton ID="lnkBtnLinkName" runat="server" CommandName="SortRow" CommandArgument="LinkName"
                                                            CausesValidation="false" ForeColor="Black" Text="Link Name" />
                                                    </th>
                                                    <th align="center" style="padding-left: 3px; width: 100px;">
                                                        File Extension
                                                    </th>
                                                    <th align="center" style="padding-left: 3px; width: 110px;">
                                                        <asp:LinkButton ID="lnkBtnAddedOn" runat="server" CommandName="SortRow" CommandArgument="InsertDate"
                                                            CausesValidation="false" ForeColor="Black" Text="Added On" />
                                                    </th>
                                                    <th align="center" style="width: 60px;">
                                                        Edit
                                                    </th>
                                                    <th align="center" style="width: 60px;">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="9">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwMenuFilesDetails"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left">
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
                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex % 2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label1" EnableViewState="false" runat="server" Text='<%# Eval("Menu.ParentMenu.Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblSubMenuName" EnableViewState="false" runat="server" Text='<%# Eval("Menu.SubMenu.Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL" id="tdMenuName" runat="server">
                                                    <asp:Label ID="lblMenuName" EnableViewState="false" runat="server" Text='<%# Eval("Menu.Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblChildMenuName" EnableViewState="false" runat="server" Text='<%# Eval("Menu.ChildMenu.Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblLinkName" runat="server" EnableViewState="false" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblExtension" EnableViewState="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblAddedOn" EnableViewState="false" runat="server" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateFile"
                                                        ImageUrl="../images/IconGrid_Edit.gif" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteFile"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
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
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ObjectDataSource ID="ObjDSMenuFilesDetails" runat="server" TypeName="BusinessLogic.MenuFileBL"
                                        EnablePaging="True" SelectMethod="GetAll" SelectCountMethod="GetCount" EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="sortExpression"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="sortDirection"
                                                Type="String" />
                                            <asp:ControlParameter Name="asSearchText" ControlID="txtSearch" PropertyName="text" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidFileName" runat="server" />
                                    <asp:HiddenField ID="hidNewFileName" runat="server" />
                                    <asp:HiddenField ID="hidFilePath" runat="server" />
                                    <asp:HiddenField ID="hidFileType" runat="server" />
                                    <asp:HiddenField ID="hidFileURL" runat="server" />
                                    <asp:HiddenField ID="hidMenuFileId" runat="server" />
                                    <asp:HiddenField ID="hidMode" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" CssClass="ClsBtn"
                                CausesValidation="false" ViewStateMode="Enabled" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" lang="javascript">
        _clientFileUploadClientId = '<%= this.fileUploadItems.ClientID%>';
        _clienthidNewFileName = '<%= this.hidNewFileName.ClientID %>';
        _clientlblUpdateSucess = '<%= this.lblUpdateSucess.ClientID %>';
        _clientlblErrorMsg = '<%= this.lblErrorMsg.ClientID %>';
        _clientvalSumErrorMsg = '<%= this.valSumErrorMsg.ClientID %>';
        _clienthidoldFileName = '<%= this.hidFileName.ClientID %>';
        _clientoptURL = "<%=this.optURL.ClientID %>"
        _clientoptFilePath = "<%=this.optFilePath.ClientID %>"
        _clienttxtFileURL = "<%=this.txtFileURL.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {        
            ShowFileURL();
        }
        
        function ValidateFileURL(oSrc, args) {
            var FileURL = document.getElementById(_clienttxtFileURL).value;
            if ($get(_clientoptURL).checked) {
                if (FileURL == "") {
                    oSrc.errormessage = "File URL should not be blank."
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }
            args.IsValid = true;
            return false;
        }



        function ShowFileURL() {
        
        if ($get(_clientoptURL).checked) {
            $(".path").hide();
            $(".url").show(100);
            }
            else {
                $(".url").hide();
                $(".path").show(100);
            }
        }

    </script>
    <script src="../Scripts/Admin/UpdateMenuFilesUI.js" type="text/javascript"></script>
</asp:Content>
