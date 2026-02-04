<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LibraryVendorUI.aspx.cs" Inherits="LibraryVendorUI" ViewStateMode = "Disabled"%>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td style="width: 77%">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                             Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                        <asp:Label ID="lblCheckDependency" Visible="true" Style="text-align: left" runat="server" ForeColor="Red"
                                             Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%"
                                        Visible="true" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <table runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 45%;
                                        margin-left: 19px;">
                                        <tr>
                                            <td align="left" style="width: 35%" class="ClsBorderLight">
                                                <span class="ClsLabel">Name :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVendorName" runat="server" MaxLength="100" CssClass="MidTxtBox"
                                                    Width="200px"></asp:TextBox><span class="ClsMdtStar">&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtVendorName"
                                                    Display="None" ValidationGroup="Save" ErrorMessage="Name should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="width: 35%" class="ClsBorderLight">
                                                <span class="ClsLabel">Address :</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddress" CssClass="MidTxtBox" runat="server" MaxLength="200"
                                                    TextMode="MultiLine" Height="54px" />
                                                <asp:RegularExpressionValidator ID="regvalTxtAddress" runat="server" Display="None"
                                                    ControlToValidate="txtAddress" ValidationGroup="Save" ErrorMessage="Address should not be greater than 200 characters."
                                                    ValidationExpression="^[\s\S]{0,200}$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight" style="width: 35%">
                                                <span class="ClsLabel">Contact No.:</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 31%">
                                                <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                                *<asp:RequiredFieldValidator ID="reqvalMobileNo" runat="server" ControlToValidate="txtMobileNo"
                                                    Display="None" ValidationGroup="Save" ErrorMessage="Contact number should not be blank."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ClientValidationFunction="MobileNumberValidation" Visible="true" ValidationGroup="Save"
                                                    ErrorMessage="Mobile number should be of greater than equal to 1 and less than equal to 15 digits." EnableClientScript="true">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl2" runat="server">
                    <ContentTemplate>
                        <table id="tblVendername" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 80%; margin-left: 19px;">
                            <tr id="trPagerTransportStaff" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwLibraryVendor">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr >
                                <td align="center">
                                    <table >
                                        <tr align="center" >
                                            <td align="center" style="width: 800px">
                                                <asp:ListView ID="lstvwLibraryVendor" DataKeyNames="VendorId,VendorName" runat="server"
                                                    OnItemCommand="lstvwLibraryVendor_ItemCommand" OnItemDataBound="lstvwLibraryVendor_ItemDataBound" ViewStateMode = "Enabled"
                                                    OnSorting="lstvwLibraryVendor_Sorting" DataSourceID="ObjDSLibraryVendor" OnDataBound="lstvwLibraryVendor_DataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" width="25%" style="padding-left: 7px;">
                                                                    <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Vendor_Name"
                                                                        CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="30%" style="padding-left: 7px;">
                                                                    <asp:LinkButton ID="lnkBtnAddress" runat="server" CommandName="Sort" CommandArgument="Address"
                                                                        CausesValidation="false" ForeColor="Black"> Address</asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="25%" style="padding-left: 7px;">
                                                                    <asp:LinkButton ID="lnkBtnMobileNo" runat="server" CommandName="Sort" CommandArgument="MobileNumber"
                                                                        CausesValidation="false" ForeColor="Black"> Mobile No</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="125px">
                                                                    Edit
                                                                </th>
                                                                <th align="center" width="125px">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwLibraryVendor"
                                                                        PageSize="20">
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
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateVendor"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveVendor"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                                            </td>
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateVendor"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveVendor" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.LibraryVendorBL" EnablePaging="True"
                                            ID="ObjDSLibraryVendor" runat="server" SelectMethod="GetLibraryVendorDetailsBL"
                                            SortParameterName="sortExpression" SelectCountMethod="CountTotalLibraryVendorBL"
                                            EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidMode" runat="server" />
                                        <asp:HiddenField ID="hidServerDate" runat="server" />
                                        <asp:HiddenField ID="hidVendorId" runat="server" />
                                        <asp:HiddenField ID="hidVendorName" runat="server" />
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" lang ="javascript">
        _clienttxtMobile = "<%=this.txtMobileNo.ClientID %>"
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>"
        _clientlbl_CheckDependency = "<%=this.lblCheckDependency.ClientID %>"
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbl_ErrorMessage = "<%=this.lblErrorMsg.ClientID %>"
    </script>
    <script src="../Scripts/LibrarianManagement/LibraryVendorUI.js" type="text/javascript"></script>
</asp:Content>
