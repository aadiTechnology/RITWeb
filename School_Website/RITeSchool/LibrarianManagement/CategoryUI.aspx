<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="CategoryUI.aspx.cs" Inherits="CategoryUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" width="100%">
        <tr>
            <td>
                <table id="Table1"  border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorMessage" runat="server" Visible="False" EnableViewState="false"
                                        CssClass="LblErrorMsg"></asp:Label>
                                    <div style="float: left; width: 100%; text-align: center">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" EnableViewState="false"
                                            Font-Bold="True" ForeColor="Blue" Visible="False"></asp:Label></div>
                                    <div style="float: right; vertical-align: top" class="LblErrorMsg">
                                        * Mandatory Fields
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valsumMainCategory" runat="server" CssClass="LblErrorMsg"
                                ValidationGroup="GrpValMainCat" />
                            <asp:ValidationSummary ID="valsumSubCategory" runat="server" CssClass="LblErrorMsg"
                                ValidationGroup="GrpValSubCat" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;" align="left" class="ClsBorderlight" valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="left" class="ClsBorderlight" style="width: 100%; height: 120px;" valign="top">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table2" align="left" runat="server" border="0" cellpadding="2" cellspacing="2"
                                                    style="width: 100%;">
                                                    <tr>
                                                        <td align="center" class="ClsBorderLight" style="width: 32%;">
                                                            <span class="ClsLabel">Media Type :</span> 
                                                        </td>
                                                        <td align="left">
                                                            <asp:RadioButton ID="optPrintable" runat="server" Text="Printable" GroupName="GrpMediaType"
                                                                AutoPostBack="True" OnCheckedChanged="optPrintable_CheckedChanged" CssClass="ClsLabel"
                                                                EnableViewState="True" TabIndex="1" />
                                                            <asp:RadioButton ID="optNonPrintable" runat="server" Text="Non Printable" GroupName="GrpMediaType"
                                                                AutoPostBack="True" OnCheckedChanged="optPrintable_CheckedChanged" CssClass="ClsLabel" ViewStateMode = "Enabled"
                                                                TabIndex="1" EnableViewState="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" class="ClsBorderLight">
                                                                <span class="ClsLabel">Category :</span> 
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbMainCategory" runat="server" CssClass="LrgTxtBox" TabIndex="2">
                                                            </asp:DropDownList>
                                                            <span style="color: #ff0000; font-size: 9pt">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight">
                                                            <asp:Label ID="lblCategory" runat="server" Text="Category Name :" Font-Bold="False"
                                                                CssClass="ClsLabel" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td colspan="1">
                                                            <asp:TextBox ID="txtCategory" runat="server" MaxLength="100" EnableViewState="False"
                                                                CssClass="LrgTxtBox" TabIndex="2"></asp:TextBox>
                                                            <span style="font-size: 9pt; color: #ff0000">*</span>
                                                            <asp:CustomValidator ID="cstValMainCategory" runat="server" ClientValidationFunction="IsValidateMainCategory"
                                                                ErrorMessage="Category name should not be blank." CssClass="LblErrorMsg" Display="None"
                                                                SetFocusOnError="True" ValidationGroup="GrpValSubCat"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cstValCategory" runat="server" ClientValidationFunction="IsValidateCategory"
                                                                ErrorMessage="Sub category name should not be blank." CssClass="LblErrorMsg"
                                                                Display="None" SetFocusOnError="True" ValidationGroup="GrpValSubCat"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true"
                                                                ValidationGroup="GrpValSubCat" TabIndex="3" />
                                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="ClsBtn" OnClick="btnDelete_Click"
                                                                ValidationGroup="GrpValMainCat" TabIndex="4" />
                                                            <asp:Button ID="btnNew" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                                                CausesValidation="False" TabIndex="5" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="tvwCategory" EventName="SelectedNodeChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="optPrintable" EventName="CheckedChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="optNonPrintable" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%;" class="ClsBorderlight" valign="top" rowspan="3">
                                        <asp:Panel ID="pnlTree" runat="server"  ScrollBars="Vertical" CssClass="allborder"
                                            HorizontalAlign="Left">
                                            <asp:UpdatePanel ID="UPanelTreeView" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <asp:TreeView runat="Server" ID="tvwCategory" Width="100%" Height= "100%" OnSelectedNodeChanged="tvwCategory_SelectedNodeChanged" ViewStateMode = "Enabled"
                                                        Font-Names="Verdana" Font-Size="8pt" SelectedNodeStyle-Font-Bold="true" SelectedNodeStyle-ForeColor="darkgreen"
                                                        SelectedNodeStyle-Font-Underline="true" ForeColor="Black" ShowLines="True" TabIndex="6">
                                                        <SelectedNodeStyle Font-Bold="True" ForeColor="DodgerBlue" Font-Underline="True" />
                                                    </asp:TreeView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hidCategoryId" runat="server" />
                                                <asp:HiddenField ID="hidCategoryName" runat="server" />
                                                <asp:HiddenField ID="hidSubCategoryId" runat="server" />
                                                <asp:HiddenField ID="hidIsSubCategory" runat="server" />
                                                <asp:HiddenField ID="hidIsConfig" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            &nbsp;&nbsp;
                            <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                                OnClick="btnBack_Click" CausesValidation="False" TabIndex="7" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script lang ="javascript" type="text/javascript">
        _clientTextboxid = "<%=this.txtCategory.ClientID %>"
        _clientValMainCat = "<%=this.valsumMainCategory.ClientID %>"
        _clientValSubCat = "<%=this.valsumSubCategory.ClientID %>"
        _clienthidCategoryId = "<%=this.hidCategoryId.ClientID %>"
        _clienthidCategoryName = "<%=this.hidCategoryName.ClientID %>"
        _clienthidSubCategoryId = "<%=this.hidSubCategoryId.ClientID %>"
        _clienthidIsSubCategory = "<%=this.hidIsSubCategory.ClientID %>"
        _clientlblHeader = "<%=this.lblMessage.ClientID %>"
        _clientlblError = "<%=this.lblErrorMessage.ClientID %>"
        _clientlblCategory = "<%=this.lblCategory.ClientID %>"
        _clientMainCategory = "<%=this.cmbMainCategory.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnDelete = "<%=this.btnDelete.ClientID %>"
        _clientbtnNew = "<%=this.btnNew.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
    </script>
    <script src="../Scripts/LibrarianManagement/CategoryUI.js" type="text/javascript"></script>
</asp:Content>
