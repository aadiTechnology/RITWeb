<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VendorConfigurationUI.aspx.cs" Inherits="VendorConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <%--<style type="text/css">
        .Modal
        {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }
        
        /* Modal Content */
        .Modal-content
        {
            background-color: #fefefe;
            margin: auto;
            padding: 20px;
            border: 1px solid #888;
            width: 80%;
        }
    </style>--%>
    <table id="tblData" width="100%" align="center">
        <tr align="center">
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divErrorPopUp" runat="server" viewstatemode="Enabled" style="position: fixed;
                            display: none; margin: 0px; padding: 0px; width: 800px; height: 100px; border-width: 0px;
                            left: 0px; top: 50px; line-height: normal; border: solid 2px darkgreen; margin: 10px 0px 0px 0px;
                            background-color: white; z-index: 499;">
                            <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; color: Black; width: 795px; text-align: right;">
                                <table>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:ValidationSummary ID="valSumError" runat="server" CssClass="ClsMdtStar" ShowMessageBox="false"
                                                ShowSummary="true" />                                            
                                            <asp:RequiredFieldValidator ID="reqCompanyName" runat="server" Display="None" ErrorMessage="Company Name should not be blank."
                                                ControlToValidate="txtCompanyName"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="reqVendorAddress" runat="server" Display="None" ErrorMessage="Vendor Address should not be blank."
                                                ControlToValidate="txtAddress"></asp:RequiredFieldValidator>        
                                            <asp:RequiredFieldValidator ID="reqGSTNo" runat="server" Display="None" ErrorMessage="GST No. should not be blank."
                                                ControlToValidate="txtGSTNo"></asp:RequiredFieldValidator>                                     
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwVendorDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSalutation" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" id="tdMessage" runat="server" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwVendorDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSalutation" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td width="100%" align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                      <%--  <div id="myModal" class="Modal">
                            <div class="Modal-content">--%>
                           
                        <div id="divPopup" runat="server" viewstatemode="Enabled" style="position: fixed;
                            display: none; margin: 0px; padding: 0px; width: 800px; height: 500px; border-width: 0px;
                            left: 500px; top: 400px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 100px 00px;
                            background-color: white; z-index: 499;">
                            <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; color: Black; width: 790px; text-align: right;">                               
                                <span style="cursor: hand" onclick="javascript:HidePopup();">
                                    <img alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                                <div style="margin: 10px auto; text-align: center;" align="center">
                                    <table>
                                        <tr id="trMandatory" runat="server">
                                            <td align="right">
                                                <span class="ClsMdtStar">*</span>
                                                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="center" width="100%">
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <span style="font-weight:bold;">Vendor Details</span>
                                                        </td>
                                                    </tr>
                                                    <tr align="center" id="trVendorNo" runat="server" visible="false">
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" Text="Vendor Number"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" style="text-align: left;" colspan="3">                                                            
                                                            <asp:TextBox ID="txtVendorNo" runat="server" Enabled="false" CssClass="SmlCombo"></asp:TextBox>                                                            
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="left" class="ClsBorderlight" style="width:180px;">
                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Company Name"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="ExLrgTxtBox" Width="100%"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <span class="ClsMdtStar">* </span>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblReportingRole" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" style="text-align: left;" colspan="3">
                                                            <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="MidTxtBox" placeholder="First Name"></asp:TextBox>    
                                                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" placeholder="Middle Name"></asp:TextBox>   
                                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="MidTxtBox" placeholder="Last Name"></asp:TextBox>                                                              
                                                        </td>
                                                         <td align="left">
                                                        </td>
                                                    </tr>                                       
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Company Address"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="ExLrgTxtBox" Width="100%"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                            <span class="ClsMdtStar">* </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Pin Code"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPinCode" MaxLength="6" runat="server" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);"
                                                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="width:175px;">
                                                            <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Phone No."></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPhoneNo" MaxLength="11" runat="server" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);"
                                                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>                                                            
                                                        </td>
                                                         <td align="left">
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label10" runat="server" CssClass="ClsLabel" Text="Mobile No."></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtMobileNo" MaxLength="10" runat="server" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);"
                                                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>                                                            
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="Fax."></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtFax" runat="server" MaxLength="11" CssClass="LrgTxtBox"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                       
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label12" runat="server" CssClass="ClsLabel" Text="PAN No."></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPanNo" runat="server" onkeypress="return PreventSpecialChars(event);" MaxLength="20" CssClass="LrgTxtBox"></asp:TextBox>
                                                        </td>
                                                         <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label11" runat="server" CssClass="ClsLabel" Text="GST No."></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtGSTNo" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                            <span class="ClsMdtStar">* </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                         <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Email"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                            <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                                                ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr> 
                                                    <tr style="height:10px;">
                                                        <td align="left" colspan="4">
                                                        </td>
                                                    </tr>        
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <span style="font-weight:bold;">Bank Details</span>
                                                        </td>
                                                    </tr>     
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label7" runat="server" CssClass="ClsLabel" Text="Account Holder Name"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAccountHolder" runat="server" CssClass="LrgTxtBox" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                         <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text="Account Number"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="50" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);"
                                                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                        </td>
                                                    </tr> 
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label17" runat="server" CssClass="ClsLabel" Text="Bank Name"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbBank" runat="server" CssClass="LrgCombo">
                                                            </asp:DropDownList>
                                                        </td>
                                                         <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label18" runat="server" CssClass="ClsLabel" Text="Branch"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtBranchName" runat="server" MaxLength="100" CssClass="LrgTxtBox"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                        </td>
                                                    </tr>     
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label19" runat="server" CssClass="ClsLabel" Text="IFSC Code"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                          <asp:TextBox ID="txtIFSCCode" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                                                        </td>
                                                         <td align="left">
                                                        </td>
                                                        <td align="left">
                                                        </td>
                                                         <td align="left">
                                                        </td>
                                                    </tr>                                         
                                                    <tr>
                                                        <td style="height: 5px;">
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td colspan="4" align="center">
                                                            <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                                BorderWidth="1px" CommandName="Save" OnClick="btnSave_Click"></asp:Button>
                                                            <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                                                Text="<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px" OnClientClick="HidePopup(); return false;">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                     <%--   </div>
                        </div>--%>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwVendorDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbSalutation" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
            </td>
        </tr>
        <tr align="center" style="width: 100%;">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trItemCount" runat="server">
                                <td align="center" style="width: 100%;">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwVendorDetails"
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
                            <tr align="center" style="text-align: center;">
                                <td align="center" style="text-align: center;">
                                    <asp:ListView ID="lstvwVendorDetails" runat="server" DataKeyNames="VendorId" OnDataBound="lstvwVendorDetails_DataBound"
                                        OnItemCommand="lstvwVendorDetails_ItemCommand" OnItemDataBound="lstvwVendorDetails_ItemDataBound"
                                        OnItemDeleting="lstvwVendorDetails_ItemDeleting" OnItemEditing="lstvwVendorDetails_ItemEditing"
                                        OnSorting="lstvwVendorDetails_Sorting">
                                        <LayoutTemplate>
                                            <table width="60%" style="color: #333333" align="center" cellpadding="0" cellspacing="1"
                                                class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="center" class="paddingL" style="width:100px; font-size: 10pt;">
                                                        <asp:LinkButton ID="lnkName" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="FirstName, MiddleName, LastName" CommandName="SortRow">Vendor No.</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 300px; font-size: 10pt;">
                                                        <asp:Label ID="lblCompanyName" runat="server" CssClass="clsLabelC" Text="Company Name"></asp:Label>                                                    
                                                    </th>
                                                    <th align="center" style="width: 150px; font-size: 10pt;">
                                                        <asp:Label ID="lblPhoneNo" runat="server" CssClass="clsLabelC" Text="Phone No."></asp:Label>                                                       
                                                    </th>
                                                      <th align="center" style="width: 150px; font-size: 10pt;">
                                                        <asp:Label ID="Label13" runat="server" CssClass="clsLabelC" Text="Mobile No."></asp:Label>                                                       
                                                    </th>
                                                      <th align="left" style="width: 150px; font-size: 10pt;">
                                                        <asp:Label ID="Label14" runat="server" CssClass="clsLabelC" Text="GST No."></asp:Label>                                                       
                                                    </th>
                                                    <th width="60px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="60px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="7" align="left">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwVendorDetails">
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
                                                <td align="center" style="padding-left: 5px;">
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("VendorNo") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblLeaveDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("PhNumber") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label15" runat="server" CssClass="clsLabelC" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                                </td>
                                                 <td align="left">
                                                    <asp:Label ID="Label16" runat="server" CssClass="clsLabelC" Text='<%#Eval("GSTNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="center" style="padding-left: 5px;">
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("VendorNo") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblLeaveDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("CompanyName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("PhNumber") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label15" runat="server" CssClass="clsLabelC" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                                </td>
                                                 <td align="left">
                                                    <asp:Label ID="Label16" runat="server" CssClass="clsLabelC" Text='<%#Eval("GSTNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table align="center" width="60%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidVendorId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidEmailValidation" runat="server" />
                                    <asp:ObjectDataSource TypeName="BusinessLogic.VendorDetailsBL" EnablePaging="true"
                                        ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                        EnableCaching="false">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwVendorDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button CssClass="ClsBtn" ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>"
                    BorderWidth="1px" OnClientClick="ShowPopup(); return false;" CausesValidation="false">
                </asp:Button>
                <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>"
                    CssClass="ClsBtn" CausesValidation="false" TabIndex="13" />
            </td>
        </tr>       
    </table>
    <script language="javascript" type="text/javascript">
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _clientcstValEmail = "<%=this.cstValEmail.ClientID %>"
        _clientdivPopup = "<%=this.divPopup.ClientID %>";
        _clientdivErrorPopUp = "<%=this.divErrorPopUp.ClientID %>"
        _clienthidVendorId = "<%=this.hidVendorId.ClientID %>"
        _clienttdMessage = "<%=this.tdMessage.ClientID %>"
        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clientcmbSalutation = "<%=this.cmbSalutation.ClientID %>"
        _clienttxtFirstName = "<%=this.txtFirstName.ClientID %>"
        _clienttxtMiddleName = "<%=this.txtMiddleName.ClientID %>"
        _clienttxtLastName = "<%=this.txtLastName.ClientID %>"
        _clienttxtCompanyName = "<%=this.txtCompanyName.ClientID %>"
        _clienttxtAddress = "<%=this.txtAddress.ClientID %>"
        _clienttxtPinCode = "<%=this.txtPinCode.ClientID %>"
        _clienttxtPhoneNo = "<%=this.txtPhoneNo.ClientID %>"
        _clienttxtFax = "<%=this.txtFax.ClientID %>"
        _clienttxtEmail = "<%=this.txtEmail.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clienttxtPanNo = "<%=this.txtPanNo.ClientID %>"
        _clienttxtGSTNo = "<%=this.txtGSTNo.ClientID %>"
        _clienttxtMobileNo = "<%=this.txtMobileNo.ClientID %>"
        _clienttxtVendorNo = "<%=this.txtVendorNo.ClientID %>"
        _clienttrVendorNo = "<%=this.trVendorNo.ClientID %>"
        _clienttxtAccountHolder = "<%=this.txtAccountHolder.ClientID %>"
        _clienttxtAccountNumber = "<%=this.txtAccountNumber.ClientID %>"
        _clientcmbBank = "<%=this.cmbBank.ClientID %>"
        _clienttxtBranchName = "<%=this.txtBranchName.ClientID %>"
        _clienttxtIFSCCode = "<%=this.txtIFSCCode.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function EmailValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtEmailId).value;
            if (sEmail != "") {
                sEmail = stripLeadingTrailingBlanks(sEmail);
                // If email is not blank then validate for valid email address.
                if (!isEmail(sEmail)) {
                    document.getElementById(_clientcstValEmail).errormessage = document.getElementById("<%=this.hidEmailValidation.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        //var modal = document.getElementById('myModal');
        function ShowPopup() {            
            $('#' + _clientlblMessage).html("")
            $('#' + _clientdivPopup).fadeIn(700);            
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divPopup.ClientID %>").style
            var width = 600
            var height = 120
            var left = parseInt((screen.width / 2) - (width / 2.3))-100
            var top = parseInt((screen.height / 2) - (height / 2)) - 70
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            
           // modal.style.display = "block";
        }

        function HidePopup() {            
           // modal.style.display = "none";
            $('#' + _clientdivPopup).fadeOut(700);
            $('#' + _clientdivErrorPopUp).fadeOut(700);
            ClearControls();
            return false;
        }

        function ClearControls() {
            $('#' + _clienttrVendorNo).hide();
            document.getElementById(_clienthidVendorId).value = 0;
            document.getElementById(_clientcmbSalutation).value = 1;
            document.getElementById(_clienttxtFirstName).value = "";
            document.getElementById(_clienttxtMiddleName).value = "";
            document.getElementById(_clienttxtLastName).value = "";
            document.getElementById(_clienttxtCompanyName).value = "";
            document.getElementById(_clienttxtAddress).value = "";
            document.getElementById(_clienttxtPinCode).value = "";
            document.getElementById(_clienttxtPhoneNo).value = "";
            document.getElementById(_clienttxtFax).value = "";
            document.getElementById(_clienttxtEmail).value = "";            
            document.getElementById(_clienttxtMobileNo).value = "";
            document.getElementById(_clienttxtGSTNo).value = "";
            document.getElementById(_clienttxtPanNo).value = "";

            $('#' + _clienttxtAccountHolder).val("")
            $('#' + _clienttxtAccountNumber).val("")
            $('#' + _clienttxtBranchName).val("")
            $('#' + _clienttxtIFSCCode).val("")
            $('#' + _clientcmbBank).val(0)

            document.getElementById(_clientbtnSave).value = "SAVE";
            $('#' + _clientlblMessage).html("")
        }


        function ShowValidationPopup() {
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate();
            }
            if (validationResult == false) {
                //$('#' + _clientdivErrorPopUp).fadeIn(1000);
                $('#' + _clientdivErrorPopUp).show(500);
                var x, y, tt_ovr_
                var cssstyle = $get("<%=this.divErrorPopUp.ClientID %>").style
                var width = 600
                var height = 120
                var left = parseInt((screen.width / 2) - (width / 2.3))-100
                var top = parseInt((screen.height / 6) - (height / 6))-30
                cssstyle.left = left + "px"
                cssstyle.top = top + "px"
            }
            else {
                $('#' + _clientdivPopup).fadeOut(700);
            }
        }

        function PreventSpecialChars(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
        }    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
