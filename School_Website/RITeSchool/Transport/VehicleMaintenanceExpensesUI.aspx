<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VehicleMaintenanceExpensesUI.aspx.cs" Inherits="VehicleMaintenanceExpensesUI"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="98%">
        <tr>
            <td align="right">
                <div style="float: right;" class="LblErrorMsg" id="lblMandatoryMark" runat="server" viewstatemode="Enabled" >
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>*
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="left">

                         <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave"/>
                                <asp:PostBackTrigger ControlID="btnCancel" />
                                <asp:AsyncPostBackTrigger ControlID="lstvwPartsUsed" EventName="ItemCommand" />
                                <asp:AsyncPostBackTrigger ControlID="lstvwVehicleMaintenanceDetails" EventName="ItemCommand" />
                            </Triggers>
                         </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:Label ID="lblUpdateSuccess" runat="server" ForeColor="Blue" Width="100%" 
                                    CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave"/>
                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwPartsUsed" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwVehicleMaintenanceDetails" EventName="ItemCommand" />
                                </Triggers>
                             </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td align="center">
                <table id="tblVehicleControls" width="80%" runat="server" style="padding-bottom: 15px !important">
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" runat="server" width="98%">
                                        <tr>
                                            <td class="TxtNormal" colspan="4">
                                                <asp:CustomValidator ID="cstMaintenanceBillDateValidation" runat="server" SetFocusOnError="True"
                                                    Display="None" ClientValidationFunction="IsMaintenanceBillDateValid">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="cstVehicleNoValidation" runat="server" 
                                                    Display="None" ClientValidationFunction="IsVehicleNoSelected">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server" ClientValidationFunction="ValidateExpiryDate"
                                                     ErrorMessage="" CssClass="LblErrorMsg">
                                                 </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="ReqBillNo" runat="server" ErrorMessage="Bill Number should not be blank."
                                                    Display="None" ControlToValidate="txtBillNo">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstBillDateValidation" runat="server" SetFocusOnError="True"
                                                    Display="None" ClientValidationFunction="IsBillDateValid">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="ReqWorkshopName" runat="server" ErrorMessage="Workshop Name should not be blank."
                                                    Display="None" ControlToValidate="txtWorkshopName">
                                                </asp:RequiredFieldValidator>
                                                <%--asp:RequiredFieldValidator ID="ReqTotalAmount" runat="server" ErrorMessage="Total Amount should not be blank."
                                                    Display="None" ControlToValidate="txtTotalAmount">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:CustomValidator ID="cstTotalAmount" runat="server" SetFocusOnError="true" Display="None"
                                                  ClientValidationFunction="ValidateTotalAmount"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="ReqMaintenanceType" runat="server" ErrorMessage="Maintenance Type should be selected."
                                                    Display="None" ControlToValidate="ddlMaintenanceType" SetFocusOnError="true" InitialValue="0">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstValidateBillFile" Display="None" runat="server" ClientValidationFunction="ValidateFile"
                                                     ErrorMessage="InvalidFileFormat" CssClass="LblErrorMsg">
                                                 </asp:CustomValidator>                                                 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL width-111" align="left">
                                                <span class="ClsLabel">Maintenance Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtMaintenanceDate" CssClass="MidTxtBox" runat="server">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="cMaintenanceDate" runat="server" Control="txtMaintenanceDate"
                                                    Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Maintenance Date." />
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL width-111" align="left">
                                                <span class="ClsLabel">Bill Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtBillDate" CssClass="MidTxtBox" runat="server">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="cBillDate" runat="server" Control="txtBillDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Bill Date." />
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL width-111" align="left">
                                                <span class="ClsLabel">Vehicle No. :</span>
                                            </td>
                                            <td style="width: 25%;" align="left">
                                                <asp:DropDownList ID="ddlVehicleNo" runat="server" CssClass="MidCombo" ViewStateMode= "Enabled">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL width-111" align="left">
                                                <span class="ClsLabel">Expiry Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtExpiryDate" CssClass="MidTxtBox" runat="server">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="cExpiryDate" runat="server" Control="txtExpiryDate" Format="dd MMM yyyy"
                                                 ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Bill Date." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Meter Reading :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtMeterReading" CssClass="MidTxtBox" runat="server" onkeypress="return blockNonNumbers (this, event, true, false);">
                                                </asp:TextBox>
                                            </td>
                                             <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Workshop Name :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtWorkshopName" CssClass="MidTxtBox" runat="server" MaxLength="300">
                                                </asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Bill No. :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtBillNo" CssClass="MidTxtBox" runat="server" MaxLength = "50">
                                                </asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Work Details :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtWorkDetails" CssClass="MidTxtBox" runat="server" MaxLength="1000">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>                                            
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 87px;">
                                                <span class="ClsLabel">Labour Charges:</span>
                                            </td>
                                            <td  align="left">
                                                <asp:TextBox ID="txtLabour" CssClass="MidTxtBox" runat="server" AutoPostBack="true" onkeyup="extractNumber(this,2,false);"
                                                    OnTextChanged = "txtLabour_TextChanged" MaxLength = "7" onkeypress="return blockNonNumbers (this, event, true, false);">
                                                </asp:TextBox>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL">
                                                <span class="ClsLabel">Total Amount :</span>
                                            </td>
                                            <td style="width: 25%;" class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="MidTxtBox" Enabled="false" MaxLength = "15"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                         <tr>  
                                            <td align="left" class="ClsBorderlight paddingL">
                                                <span class="ClsLabel">Maintenance Type :</span>
                                            </td>
                                            <td class="txtNormal" align="left">
                                                <asp:DropDownList ID="ddlMaintenanceType" runat="server" CssClass="MidCombo" ViewStateMode= "Enabled">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Upload Bill :</span>
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FlBill" runat="server" />
                                                <asp:ImageButton ID="btnFile" runat="server" CausesValidation="false" CommandName="UpdateVehicleMaintenance"
                                                 ToolTip="Update" Visible="false" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                <asp:ImageButton ID="DeleteIcon" runat="server" CommandName="DeleteVehicleMaintenance"
                                                  CausesValidation="false" ToolTip="Delete" Visible="false" ImageUrl="../images/IconGrid_Delete.GIF" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                            <td colspan="2">
                                                <span class="LblSmlGray">(Attachment supports files of types - .BMP, .JPG, .JPEG, .PNG, .PDF upto 1 MB.)</span>
                                            </td>
                                        </tr>
                                        <tr id="Tr2" runat = "server">
                                            <td colspan="4" align="left" style = "padding:10px 0 10px 0">
                                                <asp:ListView runat ="server" ID = "lstvwPartsUsed" OnItemCommand = "lstvwPartsUsed_ItemCommand" ViewStateMode = "Enabled">
                                                    <LayoutTemplate>
                                                        <table id="tblPartsUsed" runat="server" align="center" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" width="50%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="paddingLSML" width="1%">
                                                                    <asp:LinkButton ID="LnkBtnPartsUsed" runat="server" CausesValidation="false"
                                                                        CommandArgument="MaintenancePartsUsed" CommandName="SortRow" ForeColor="Black">Parts Used</asp:LinkButton>
                                                                </th>
                                                                <th id="Th1" width="5%" align="left" runat="server">
                                                                    <asp:LinkButton ID="LnkBtnQty" runat="server" CausesValidation="false" CommandArgument="Qty"
                                                                        CommandName="SortRow" ForeColor="Black">Quantity</asp:LinkButton>
                                                                </th>
                                                                <th id="thRates" runat="server" class="paddingL" align="left" style="width: 5%;">
                                                                    <asp:LinkButton ID="LnkBtnRate" runat="server" CausesValidation="false" CommandArgument="Rates"
                                                                        CommandName="SortRow" ForeColor="Black">Rate</asp:LinkButton>
                                                                </th>
                                                                <th id="thAmounts" runat="server" class="paddingL" align="left" style="width: 5%;">
                                                                    <asp:LinkButton ID="LnkBtnAmount" runat="server" CausesValidation="false" CommandArgument="Amt"
                                                                        CommandName="SortRow" ForeColor="Black">Amount</asp:LinkButton>
                                                                </th>
                                                                <th align="center" style="width: 4%;">
                                                                    Add
                                                                </th>
                                                                <th align="center" style="width: 5%;">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplates" runat="server" class="ClsGridRow">
                                                            <td>
                                                                <asp:TextBox ID="txtPartsUsed" runat="server" >
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass = "width-100" AutoPostBack = "true" onkeyup="extractNumber(this,2,false);"
                                                                    OnTextChanged = "txtQuantity_TextChanged" MaxLength = "7" onkeypress="return blockNonNumbers (this, event, true, false);">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRate" runat="server" CssClass = "width-100" AutoPostBack = "true" onkeyup="extractNumber(this,2,false);"
                                                                    OnTextChanged = "txtRate_TextChanged" MaxLength = "7" onkeypress="return blockNonNumbers (this, event, true, false);">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAmounts" runat="server" CssClass = "width-100" AutoPostBack = "true" onkeyup="extractNumber(this,2,false);"
                                                                    OnTextChanged = "txtAmounts_TextChanged" MaxLength = "7" onkeypress="return blockNonNumbers (this, event, true, false);">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="AddParts" 
                                                                    CausesValidation="false" ToolTip="Add" ImageUrl="~/RITeSchool/images/Add_Grace.png" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteParts"
                                                                    CausesValidation="false" ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button runat="server" Text= "<%$ Resources:LocalizedResources, Save %>" class="ClsBtn" ID="btnSave" OnClick="btnSave_Click" disable-page="true" />
                                                <asp:Button runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" class="ClsBtn" ID="btnCancel" OnClick="btnCancel_Click" CausesValidation="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwPartsUsed" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwVehicleMaintenanceDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr id="trLink" runat="server">
            <td>
                <table id="tblLstvwLinkVehicle" align="center" width="90%" runat="server">
                    <tr>
                        <td>
                            <hr style="border: thin solid #C0C0C0" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table>
                                <tr>
                                    <td align="left" class="clsBorderLight">
                                        <span class="clsLabel">Vehicle No. / Workshop Name : </span>                                        
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td align="left" class="clsBorderLight">
                                        <span class="clsLabel">Maintenance Type : </span>                                        
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbMaintenanceType" runat="server" CssClass="LrgCombo">
                                        </asp:DropDownList>
                                    </td>                                    
                                </tr>                               
                                <tr>
                                    <td align="left" class="clsBorderLight">
                                        <span class="clsLabel">Start Date : </span>                                        
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtStartDate" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start Date." />
                                        <a onclick="ClearDate(1);" href="#" style="font-size:smaller;"><u>Clear</u></a>
                                    </td>
                                    <td align="left" class="clsBorderLight">
                                        <span class="clsLabel">End Date : </span>                                        
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtEndDate" CssClass="MidTxtBox" runat="server" ReadOnly="true"></asp:TextBox>
                                        <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtEndDate" Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid End Date." />
                                        <a onclick="ClearDate(2);" href="#" style="font-size:smaller;"><u>Clear</u></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Button ID="btnSearch" runat="server" Text="Show" CssClass="ClsBtn" 
                                            CausesValidation="false" onclick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class = "width-99-percentage">
                            <asp:UpdatePanel ID="upnlLstvwLinkVehicle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center" width="100%">
                                        <tr id="trDtPgCount" runat="server" visible="true">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwVehicleMaintenanceDetails"
                                                    PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB"
                                                                    Text="<%# Container.StartRowIndex + 1%>" />
                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal"
                                                                    Text=" To " />
                                                                <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal"
                                                                    Text=" Out Of " />
                                                                <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal"
                                                                    Text="Records " />
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwVehicleMaintenanceDetails" runat="server" DataKeyNames="VehicleMaintenanceExpensesId, VehicleId, WorkDetails, MaintenanceTypeId, ExpiryDate, BillFileName" ViewStateMode= "Enabled"
                                                    OnItemCommand="lstvwVehicleMaintenanceDetails_ItemCommand" OnItemDataBound="lstvwVehicleMaintenanceDetails_ItemDataBound"
                                                    OnDataBound="lstvwVehicleMaintenanceDetails_DataBound" OnSorting="lstvwVehicleMaintenanceDetails_Sorting">
                                                    <LayoutTemplate>
                                                        <table id="tblVehicleMaintenanceDetails" runat="server" align="center" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" class="paddingL" width="4%">
                                                                    <asp:LinkButton ID="LinkBtnMaintenanceDate" runat="server" CausesValidation="false" SortExpression = "MaintenanceDate"
                                                                        CommandArgument="MaintenanceDate" CommandName="Sort" ForeColor="Black">Maintenance Date</asp:LinkButton>
                                                                </th>
                                                                <th align="center" class="paddingL" width="5%">
                                                                    <asp:LinkButton ID="LinkBtnMaintenanceType" runat="server" CausesValidation="false" CommandArgument="MaintenanceType"
                                                                        CommandName="Sort" ForeColor="Black">Maintenance Type</asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="5%" class="paddingL"> 
                                                                    <asp:LinkButton ID="LinkBtnVehicleType" runat="server" CausesValidation="false" CommandArgument="VehicleNumber"
                                                                        CommandName="Sort" ForeColor="Black">Vehicle Number</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="5%" >
                                                                    <asp:LinkButton ID="LinkBtnMeterReading" runat="server" CausesValidation="false" CommandName="Sort"
                                                                        CommandArgument="MeterReading" ForeColor="Black">Meter Reading</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="paddingL" style="width: 4%;">
                                                                    <asp:LinkButton ID="LinkBtnBillNo" runat="server" CausesValidation="false" CommandArgument="BillNumber"
                                                                        CommandName="Sort" ForeColor="Black">Bill No.</asp:LinkButton>
                                                                </th>
                                                                <th id="thBillDate" runat="server" class="paddingL" align="center" style="width: 5%;">
                                                                    <asp:LinkButton ID="LinkBtnBillDate" runat="server" CausesValidation="false" CommandArgument="BillDate"
                                                                        CommandName="Sort" ForeColor="Black">Bill Date</asp:LinkButton>
                                                                </th>
                                                                <th id="thWorkshopName" runat="server" class="paddingL" align="left" style="width: 10%;">
                                                                    <asp:LinkButton ID="LinkBtnWorkshopName" runat="server" CausesValidation="false"
                                                                        CommandArgument="WorkshopName" CommandName="Sort" ForeColor="Black">Workshop Name</asp:LinkButton>
                                                                </th>
                                                                <th id="thLabour" runat="server" class="paddingL" align="center" style="width: 5%;">
                                                                    <asp:LinkButton ID="LinkBtnLabour" runat="server" CausesValidation="false" CommandArgument="Labour" CommandName="Sort"
                                                                        ForeColor="Black">Labour Charges</asp:LinkButton>
                                                                </th>
                                                                <th id="thTotalAmount" runat="server" class="paddingL" align="center" style="width: 5%;">
                                                                    <asp:LinkButton ID="LinkBtnTotalAmount" runat="server" CausesValidation="false" CommandArgument="TotalAmount"
                                                                        CommandName="Sort" ForeColor="Black">Total Amount</asp:LinkButton>
                                                                </th>
                                                                <th align="center" style="width: 2%;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 3%;">
                                                                    Delete
                                                                </th>
                                                                 <th align="center" style="width: 10%;">
                                                                    Download Uploaded Bill
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="15">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVehicleMaintenanceDetails"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                             <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                <asp:DropDownList ID="ddlCnt" ViewStateMode = "Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged" AutoPostBack = "true">
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
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblMaintenanceDt" runat="server" Text='<%# Eval("MaintenanceDate", "{0:dd MMM yyyy}") %>'> </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblMaintenanceType" runat="server" Text='<%# Eval("MaintenanceType") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblVehicleType" runat="server" Text='<%# Eval("VehicleNumber") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblMeterReading" runat="server" Text='<%# Eval("MeterReading") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblBillNo" runat="server" Text='<%# Eval("BillNumber") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="tdBillDt" runat="server" align="center" class="paddingL">
                                                                <asp:Label ID="lblBillDt" runat="server" Text='<%# Eval("BillDate", "{0:dd MMM yyyy}") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblWorkshopName" runat="server" Text='<%# Eval("WorkshopName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblLabour" runat="server" Text='<%# Eval("Labour") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("TotalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateVehicleMaintenance"
                                                                    CausesValidation="false" ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteVehicleMaintenance"
                                                                    CausesValidation="false" ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnView" runat="server" ToolTip="Download" Visible="false" CausesValidation="false"
                                                                CommandName="Bill" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblMaintenanceDt" runat="server" Text='<%# Eval("MaintenanceDate", "{0:dd MMM yyyy}") %>'> </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblMaintenanceType" runat="server" Text='<%# Eval("MaintenanceType") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblVehicleType" runat="server" Text='<%# Eval("VehicleNumber") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblMeterReading" runat="server" Text='<%# Eval("MeterReading") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblBillNo" runat="server" Text='<%# Eval("BillNumber") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="tdBillDt" runat="server" align="center" class="paddingL">
                                                                <asp:Label ID="lblBillDt" runat="server" Text='<%# Eval("BillDate", "{0:dd MMM yyyy}") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblWorkshopName" runat="server" Text='<%# Eval("WorkshopName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblLabour" runat="server" Text='<%# Eval("Labour") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingLR">
                                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("TotalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateVehicleMaintenance"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteVehicleMaintenance"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnView" runat="server" Visible="false" CausesValidation="false" ToolTip="Download"
                                                                CommandName="Bill" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record found.
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidVehicleMaintenanceExpensesId" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidFileUpload" runat="server" />
                                               
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwVehicleMaintenanceDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" Visible="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ObjectDataSource TypeName="BusinessLogic.VehicleMaintenanceExpensesBL" EnablePaging="false"
                    ID="ObjDSVehicleDetails" runat="server" SelectMethod="GetAllVehicleExpensesDetails" SortParameterName="aiSortExpression"
                     EnableCaching="False">
                    <SelectParameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />

                        <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                        <asp:ControlParameter ControlID="cmbMaintenanceType" Name="aiMaintenanceTypeId" Type="Int32" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="txtStartDate" Name="asStartDate" Type="String" PropertyName="Text" />
                        <asp:ControlParameter ControlID="txtEndDate" Name="asEndDate" Type="String" PropertyName="Text" />

                        <asp:ControlParameter Name="aiSortExpression" ControlID="hidSortExpression" Type="String" PropertyName="Value"/>
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        _ClientlblUpdateSuccess = "<%=this.lblUpdateSuccess.ClientID %>"
        _ClienttxtMaintenanceDate = "<%=this.txtMaintenanceDate.ClientID %>";
        _ClienttxtBillDate = "<%=this.txtBillDate.ClientID %>";
        _ClientcstMaintenanceBillDateValidation = "<%=this.cstMaintenanceBillDateValidation.ClientID%>";
        _ClientcstBillDateValidation = "<%= this.cstBillDateValidation.ClientID%>";
        _ClienttxtVehicleNo = "<%= this.ddlVehicleNo.ClientID%>";
        _ClientcstVehicleNoValidation = "<%= this.cstVehicleNoValidation.ClientID%>";
         _clientFlBill = "<%=this.FlBill.ClientID%>";
         _clienthidFileUpload = "<%=this.hidFileUpload.ClientID %>"
         _clienttxtExpiryDate = "<%=this.txtExpiryDate.ClientID %>"

         _clienttxtTotalAmount = "<%=this.txtTotalAmount.ClientID %>"

         _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
         _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"

        function ValidateTotalAmount(oSrc, args) {
            var iTotalAmount = document.getElementById(_clienttxtTotalAmount).value;

            if (iTotalAmount == "") {
                oSrc.errormessage = "Total Amount should not be blank."
                args.IsValid = false
                return true;
            }
            
            else if(iTotalAmount == "0") {
                oSrc.errormessage = "Total Amount should not be 0."
                args.IsValid = false
                return true;
            }
        }

        function ValidateFile(oSrc, args) {
            if ($get("<%=this.FlBill.ClientID %>") != null) {
                var fl = $get("<%=this.FlBill.ClientID %>").value;

                    if (fl != "") {
                    var file = $get("<%=this.FlBill.ClientID %>")
                    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG"

                        )) {
                        oSrc.errormessage = "Please select valid file type to upload Bill.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (file.files[0].size >= 1024000) {
                        oSrc.errormessage = "File size should not be more than 1 MB."
                        args.IsValid = false
                        return true
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function OpenFile(file) {
            window.open(file, '_blank')
            return false;
        }

        function Confirm() {
            document.getElementById(_clienthidFileUpload).value = "";
            alert('Please save changes to complete delete action.');
            return false;
        }

        function ValidateExpiryDate(source, argmts) {
            var expdate = $('#' + _clienttxtExpiryDate).val()
            
            if (expdate != '') {
                var dtDate;
                if (document.all)
                    dtDate = new Date(expdate.replace('-', ' '));
                else
                    dtDate = new Date(convertdate(expdate));
                
                var mntDate = $('#' + _ClienttxtMaintenanceDate).val()
                var dtmnDate;
                if (document.all)
                    dtmnDate = new Date(mntDate.replace('-', ' '));
                else
                    dtmnDate = new Date(convertdate(mntDate));

                if (dtDate <= dtmnDate) {                
                    source.errormessage = 'Expiry Date should be greater than Maintenance Date.'
                    argmts.IsValid = false
                    return true
                }
            }

            argmts.IsValid = true
            return false;
        }

        function ClearDate(id) {
            if (id == 1)
                $('#' + _clienttxtStartDate).val('')
            else
                $('#' + _clienttxtEndDate).val('')
        }
     </script>
    <script type="text/javascript" src="../Scripts/Transport/VehicleMaintenanceExpenses.js"></script>
</asp:Content>
