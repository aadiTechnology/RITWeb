<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VehicleDetailsUI.aspx.cs" Inherits="VehicleDetailsUI"
    Title="Vehicle Details" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                            vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <!-- Data Insert Here -->
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Visible="false" Height="20px" Width="100%" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="ClsTextNormal" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                <!-- User InfoTable starts here -->
                                                <table id="tblVehicleDetails" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                    style="width: 50%;" align="center">
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 100px">
                                                            <span class="ClsLabel">Vehicle Type :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar" style="width: 100px">
                                                            <asp:TextBox ID="txtVehicleType" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                TabIndex="1"></asp:TextBox>
                                                            *<asp:RequiredFieldValidator ID="reqVehicleType" runat="server" ControlToValidate="txtVehicleType"
                                                                Display="None" ErrorMessage="Vehicle Type should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight" style="width: 100px">
                                                            <span class="ClsLabel">Manufacturer Name :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar" style="width: 100px">
                                                            <asp:TextBox ID="txtManufacturer" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                                                TabIndex="2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Vehicle Number :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtVehicleNumber" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                Width="160px" TabIndex="3"></asp:TextBox>
                                                            *&nbsp;
                                                            <asp:RequiredFieldValidator ID="reqVehicleNumber" runat="server" ControlToValidate="txtVehicleNumber"
                                                                Display="None" ErrorMessage="Vehicle Number should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Capacity :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtCapacity" CssClass="MidTxtBox" runat="server" MaxLength="3" onblur="extractNumber(this,0,false);"
                                                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="4" />
                                                            *<asp:RequiredFieldValidator ID="reqVehicleCapacity" runat="server" ControlToValidate="txtCapacity"
                                                                Display="None" ErrorMessage="Capacity should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cmpValCpacity" runat="server" ControlToValidate="txtCapacity"
                                                                ValueToCompare="0" Operator="GreaterThan" Type="Integer" Display="None" ErrorMessage="Capacity should be greater than zero."></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Engine Number :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtEngineNo" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                Width="160px" TabIndex="5"></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Purchase / Hire :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:DropDownList ID="ddlPurchaseorhire" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                                Width="150px" OnSelectedIndexChanged="ddlPurchaseorhire_SelectedIndexChanged"
                                                                TabIndex="6">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel">Chassis Number :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtChassisNo" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                Width="160px" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel">Purchase / Hire Date :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtCalPopup" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                                                                TabIndex="8"></asp:TextBox>
                                                            <rjs:PopCalendar ID="CalPopup" runat="server" Control="txtCalPopup" Format="dd MMM yyyy"
                                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                                To-Today="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel">Fuel Type :</span>
                                                        </td>
                                                        <td align="left" colspan="1" style="width: 25%">
                                                            <asp:RadioButton ID="rbtPetrol" Text="Petrol / Gas" runat="server" GroupName="rdoGroupFuel"
                                                                CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                                                            <asp:RadioButton ID="rbtDiesel" Text="Diesel" runat="server" GroupName="rdoGroupFuel"
                                                                CssClass="ClsLabel clsLabel"></asp:RadioButton>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel"> Official Mobile No. :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtOffcialMobNo" runat="server" CssClass="MidTxtBox" MaxLength="10"
                                                            onblur="exactNumber(this,0,false);" onkeyup="exactNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);"
                                                            ondrop="event.returnValue=false"></asp:TextBox>
                                                            <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                                Visible="true" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel">Tracking URL :</span>
                                                        </td>
                                                        <td align="left" colspan="1" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtTrackingURL" runat="server" MaxLength="500" CssClass="MidTxtBox"
                                                                TabIndex="9"></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="clsLabel"> Attendant's RFID :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtRFID" runat="server" CssClass="MidTxtBox" MaxLength="100"
                                                             onblur="exactNumber(this,0,false);" onkeyup="exactNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);"
                                                            ondrop="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <!-- User InfoTable ListView -->
                            <tr>
                                <td>
                                    <div runat="server" id="divErr">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="tblStaff" runat="server" align="center" width="50%">
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 600px">
                                                <div id="divContainer" class="GridBorder" runat="server" visible="true" style="width: 85%;
                                                    height: 300px; overflow: scroll">
                                                    <asp:ListView ID="lstvwTransportStaff" runat="server" DataKeyNames="TransportStaffId,VehicleStaffId"
                                                        OnItemDataBound="lstvwTransportStaff_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" width="10%">
                                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                    </th>
                                                                    <th align="left" width="80%" style="padding-left: 9px;">
                                                                        <asp:Label ID="lnkBtnSortName" runat="server" CausesValidation="false" ForeColor="Black"> Name </asp:Label>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="center">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td class="paddingL" align="left">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSave" runat="server">
                                <td align="center">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                        disable-page="true" CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                </td>
                            </tr>
                            <tr align="center" style="text-align: center; margin: 0px auto;">
                                <td align="center" style="text-align: center;">
                                    <table align="center">
                                        <tr>
                                            <td class="ClsBorderLight" align="left">
                                                <span class="ClsLabel">Vehicle Number : </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnSearch" runat="server" Text="Search" class="ClsBtn"
                                                    CausesValidation="false" OnClick="BtnSearch_Click"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDataPager" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwVehicleStaffAsso">
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
                            <tr>
                                <td align="center">
                                    <table width="80%">
                                        <tr style="width: 90%">
                                            <td style="width: 100%">
                                                <asp:ListView ID="lstvwVehicleStaffAsso" runat="server" DataKeyNames="VehicleId, VehicleNumber"
                                                    DataSourceID="ObjDSVehicleStaffDetails" OnItemCommand="lstvwVehicleStaffAsso_ItemCommand"
                                                    OnItemDataBound="lstvwVehicleStaffAsso_ItemDataBound" OnDataBound="lstvwVehicleStaffAsso_DataBound"
                                                    OnSorting="lstvwVehicleStaffAsso_Sorting">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tblVehicleStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" width="15%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkBtnSortVehicleName" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                                        CausesValidation="false" ForeColor="Black"> Vehicle Number </asp:LinkButton>
                                                                </th>
                                                                <th align="left" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkBtnStaff" runat="server" CommandName="Sort" CommandArgument="StaffMembers"
                                                                        CausesValidation="false" ForeColor="Black"> Associated Staff</asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="10%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkbtnVehicleType" runat="server" CommandName="Sort" CommandArgument="VehicleType"
                                                                        CausesValidation="false" ForeColor="Black"> Vehicle Type</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="10%" style="padding-left: 9px;">
                                                                    <asp:LinkButton ID="lnkbtnVehicleCapacity" runat="server" CommandName="Sort" CommandArgument="VehicleCapacity"
                                                                        CausesValidation="false" ForeColor="Black"> Vehicle Capacity</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="7%">
                                                                    Edit
                                                                </th>
                                                                <th align="center" width="7%">
                                                                    Delete
                                                                </th>
                                                                <th align="center" width="150px">
                                                                    <asp:Label ID="lblUploadDocuments" runat="server" Text="Upload Document" />
                                                                </th>
                                                                <th align="center" width="100px" id="thSync" runat="server">
                                                                    Sync
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="9">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVehicleStaffAsso"
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
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("StaffMembers") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("VehicleType") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("VehicleCapacity") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                            <td id="tdlnkupload" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkUploadDocument" runat="server" Text="Upload"
                                                                 CausesValidation="false" ToolTip="Click to upload documents."></asp:LinkButton>
                                                            </td>
                                                            <td id="tdSync" runat="server" align="center">
                                                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Sync"
                                                                 CausesValidation="false" ToolTip="Click to Sync vehicle." CommandName="SYNC"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                                            </td>
                                                            <td class="paddingL" align="left">
                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("StaffMembers") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("VehicleType") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("VehicleCapacity") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                            <td id="tdlnkupload" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkUploadDocument" runat="server" Text="Upload"
                                                                 CausesValidation="false" ToolTip="Click to upload documents."></asp:LinkButton>
                                                            </td>
                                                            <td id="tdSync" runat="server" align="center">
                                                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Sync"
                                                                 CausesValidation="false" ToolTip="Click to Sync vehicle." CommandName="SYNC"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                           <td Align="Center">
                                               <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                  <ContentTemplate>
                                                      <asp:Button ID="btnExport" CssClass="ClsBtn" runat="server" Text="Export" OnClick="btnExport_Click"
                                                           CausesValidation="false" />
                                                          &nbsp;
                                                      <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="False" UseSubmitBehavior="false" />
                                                 </ContentTemplate>
                                                 <Triggers>                          
                                                   <asp:PostBackTrigger ControlID ="btnExport" />
                                                </Triggers>
                                             </asp:UpdatePanel>
                                         </td>
                                     </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ObjectDataSource TypeName="BusinessLogic.VehicleDetailsBL" EnablePaging="True"
                                        ID="ObjDSVehicleStaffDetails" runat="server" SelectMethod="GetAllVehicleStaffAsso"
                                        SortParameterName="sortExpression" SelectCountMethod="CountTotalVehicleStaffAsso"
                                        EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                            <asp:Parameter Name="sortExpression" Type="String" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidVehicleId" runat="server" Value="0" />
                                    <asp:CustomValidator ID="CstStaff" runat="server" ClientValidationFunction="CheckAtListOne"
                                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">


        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlstvwTransportStaff = "<%=this.lstvwTransportStaff.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _ClientChkAll = _clientlstvwTransportStaff + "_ChkSelectAll";
        _clientCstStaff = "<%=this.CstStaff.ClientID %>"

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }


        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwTransportStaff + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwTransportStaff + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function CheckAtListOne(oSrc, args) {
            var chk;
            var iRowCount = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientlstvwTransportStaff + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwTransportStaff + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
                $get(_clientCstStaff).errormessage = "At least one staff member should be selected for vehicle."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlblErrorMsg) != null) {
                document.getElementById(_clientlblErrorMsg).style.display = "none"
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            }
        }

        function OpenPopup(querystring) {
            window.open('../Transport/VehicleDocumentsPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=700').focus();
            return false;
        }

        _sClienttxtMobilePhoneNumberId = "<%=this.txtOffcialMobNo.ClientID %>";
        function MobileNumberValidation(oSrc, args) {            
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
           
           if (sMobileNumber.length < 10 && sMobileNumber.length != 0) {
                oSrc.errormessage = 'Official Mobile No. length should be 10 digit.';
                args.IsValid = false;
                return true;
             }
           else if (sMobileNumber.substring(0, 1) == '0') {
               oSrc.errormessage = 'Official Mobile No. should not start with 0.';
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }
    </script>
</asp:Content>