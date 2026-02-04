<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="TransportReadingAllocationUI.aspx.cs" Inherits="RITeSchool_Transport_TransportReadingAllocationUI"
    ViewStateMode="Enabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr id="trVehicleAllocationDetailsControl" runat="server">
            <td align="center">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table id="tblVehicleAllocationDetailsControl" width="100%" runat="server" visible="true"
                            align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                            vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    HeaderText="Please fix following error(s)" Visible="false" Height="20px" Width="100%"
                                                                    CssClass="ClsMdtStar"></asp:Label>
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
                                            <td class="ClsTextNormal" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                    CssClass="ClsLabel" Font-Bold="True" Visible="true"></asp:Label>
                                                <table id="tblVehicleDetails" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                    style="width: 50%;" align="center">
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left" style="width: 200px">
                                                            <span class="ClsLabel">Reading Date :</span>
                                                        </td>
                                                        <td style="width: 250px" align="left">
                                                            <asp:TextBox ID="txtReadingDate" class="MidTxtBox" runat="server" TabIndex="8"></asp:TextBox><span
                                                                class="ClsMdtStar"></span>
                                                            <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtReadingDate" Format="dd MMM yyyy"
                                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start Date."
                                                                To-Today="true" />
                                                            <span class="ClsMdtStar">*&nbsp;</span>
                                                            <asp:RequiredFieldValidator ID="reqReadingDateText" runat="server" ControlToValidate="txtReadingDate"
                                                                Display="None" ErrorMessage="Reading Date should not be blank"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="width: 200px">
                                                            <span class="ClsLabel">Litters :</span>
                                                        </td>
                                                        <td style="width: 250px" align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtLitters" class="MidTxtBox" MaxLength="6" runat="server" onkeyup="extractNumber(this,2,false);"
                                                                onkeypress="return blockNonNumbers (this, event, true, false);" OnTextChanged="txtLitters_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox><span class="ClsMdtStar"> <span class="ClsMdtStar">
                                                                    *</span><asp:RequiredFieldValidator ID="reqLittersText" runat="server" ControlToValidate="txtLitters"
                                                                        Display="None" ErrorMessage="Litters should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left" style="width: 111px">
                                                            <span class="ClsLabel">Vehicle Number :</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlVehicleNumber" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="ddlVehicleNumber_SelectedIndexChanged"
                                                                AutoPostBack="true" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            &nbsp;<span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqVehicleNumber" ControlToValidate="ddlVehicleNumber"
                                                                Display="None" InitialValue="0" runat="server" ErrorMessage="Please Select Vehicle Number"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel">Per Litter Cost :</span>
                                                        </td>
                                                        <td style="width: 240px" align="left">
                                                            <asp:TextBox runat="server" ID="txtPerLitterCost" CssClass="MidTxtBox" MaxLength="6"
                                                                AutoPostBack="true" OnTextChanged="txtPerLitterCost_TextChanged" onkeyup="extractNumber(this,2,false);"
                                                                onkeypress="return blockNonNumbers (this, event, true, false);"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqPerLitterCost" runat="server" ControlToValidate="txtPerLitterCost"
                                                                Display="None" ErrorMessage="Per Litter Cost should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left" style="width: 111px">
                                                            <span class="ClsLabel">Receipt No. :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtReceiptNumber" CssClass="MidTxtBox" runat="server" MaxLength="10">
                                                            </asp:TextBox>
                                                            <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ID="reqReceiptNumber"
                                                                runat="server" ControlToValidate="txtReceiptNumber" Display="None" ErrorMessage="Receipt Number should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="width: 111px">
                                                            <span class="ClsLabel">Total Cost:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtTotalCost" MaxLength="10" CssClass="MidTxtBox"
                                                                onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers (this, event, true, false);"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ID="reqTotalCostText"
                                                                runat="server" ControlToValidate="txtTotalCost" Display="None" ErrorMessage="Total Cost should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left" style="width: 111px">
                                                            <span class="ClsLabel">Reading From :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtReadingFrom" CssClass="MidTxtBox" runat="server" onkeyup="extractNumber(this,2,false);"
                                                                onkeypress="return blockNonNumbers (this, event, true, false);">
                                                            </asp:TextBox>
                                                            <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ID="reqReadingFromText"
                                                                runat="server" ControlToValidate="txtReadingFrom" Display="None" ErrorMessage="Reading From should not be blank."></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="width: 111px">
                                                            <span class="ClsLabel">Fuel Station Name :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox runat="server" ID="txtFuelStationName" CssClass="MidTxtBox"></asp:TextBox></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="left" style="width: 111px">
                                                            <span class="ClsLabel">Reading To :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtReadingTo" CssClass="MidTxtBox" runat="server" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);">
                                                            </asp:TextBox>
                                                            <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ID="reqReadingTo" runat="server"
                                                                ControlToValidate="txtReadingTo" Display="None" ErrorMessage="Reading To should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cstValReadingText" runat="server" ClientValidationFunction="IsValidReadings"
                                                                Display="None">
                                                            </asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="padding-top: 20PX !important">
                                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save%>" runat="server"
                                        CssClass="ClsBtn" CausesValidation="true" BorderWidth="1px" OnClick="btnSave_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                        CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;" align="center">
                                    <table style="width: 80%">
                                        <tr>
                                            <td>
                                                <hr style="border: 1px solid #C0C0C0" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="text-align: center; margin: 0px auto;">
                                <td colspan="2" align="center">
                                    <table align="center">
                                        <tr align="center" style="text-align: center; margin: 0px auto;width:150px;">
                                            <td class="ClsBorderlight" style="text-align: center; width: 50%; margin: 0px auto;">
                                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Reading Date "></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td style="width:140px;" align="left">
                                                <asp:CheckBox ID="ChkIncludeAllDates" runat="server" Text="Consider All Dates" CssClass="ClsLabel"
                                                    OnCheckedChanged="ChkIncludeAllDates_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td style="text-align: left; width:150px;" align="center">
                                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtSearchReadingDate" CssClass="SmlTxtBox" runat="server" ViewStateMode="Enabled"></asp:TextBox>
                                                        <rjs:PopCalendar ID="cal_ReadingDate" runat="server" Control="txtSearchReadingDate"
                                                            Format="dd MMM yyyy" ViewStateMode="Enabled" Culture="en" ShowWeekend="True"
                                                            AutoPostBack="False" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ChkIncludeAllDates" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <tr align="center" style="text-align: center; margin: 0px auto;">
                                            <td class="ClsBorderlight" style="text-align: right; margin: 0px auto;">
                                                <asp:Label ID="lblSearch" runat="server" CssClass="ClsLabel" Text="Vehicle No. / Receipt No. / Fuel Station Name "></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td colspan="2" align="left" style="padding-left:10px; width:200px;">
                                                <asp:TextBox ID="txtSearch" CssClass="LrgTxtBox"  runat="server" ViewStateMode="Enabled" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:5px;">
                                            </td>
                                        </tr>
                                        <tr align="center" style="text-align: center; margin: 0px auto;">
                                            <td style="text-align: center; margin: 0px auto;" colspan="3">
                                                <asp:Button ID="btnSearch" CssClass="ClsBtn remove-margin-top" runat="server" Text="Search"
                                                    ViewStateMode="Enabled" CausesValidation="false" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidVehicleReadingAllocationId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hidIncludeAllDates" Value="0" runat="server"></asp:HiddenField>
                                </td>
                            </tr>
                            <tr id="trDtPgCount" runat="server">
                                <td align="center" style="padding-top: 20px !important">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwVehicleReadingAllocationDetails">
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
                                    <table width="95%">
                                        <tr style="width: 90%">
                                            <td style="width: 100%">
                                                <div id="divContainer" class="GridBorder" runat="server" visible="true" style="width: 100%">
                                                    <asp:ListView ID="lstvwVehicleReadingAllocationDetails" runat="server" ViewStateMode="Enabled"
                                                        DataKeyNames="VehicleReadingAllocationId,VehicleId" DataSourceID="ObjDSVehicleReadingAllocationDetails"
                                                        OnItemCommand="lstvwVehicleReadingAllocationDetails_ItemCommand" OnItemDataBound="lstvwVehicleReadingAllocationDetails_ItemDataBound"
                                                        OnDataBound="lstvwVehicleReadingAllocationDetails_DataBound" OnSorting="lstvwVehicleReadingAllocationDetails_Sorting">
                                                        <LayoutTemplate>
                                                            <table width="100%" runat="server" id="tblVehicleStaffInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" width="" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkBtnVehicleNumber" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                                            CausesValidation="false" ForeColor="Black"> Vehicle No.</asp:LinkButton>
                                                                    </th>
                                                                    <th align="left" width="100px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnReadingDate" runat="server" CommandName="Sort" CommandArgument="ReadingDate"
                                                                            CausesValidation="false" ForeColor="Black"> Reading Date</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="100px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnReceiptNumber" runat="server" CommandName="Sort" CommandArgument="ReceiptNumber"
                                                                            CausesValidation="false" ForeColor="Black">Receipt No.</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="150px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnReadingFrom" runat="server" CommandName="Sort" CommandArgument="ReadingFrom"
                                                                            CausesValidation="false" ForeColor="Black">Reading From</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="100px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnReadingTo" runat="server" CommandName="Sort" CommandArgument="ReadingTo"
                                                                            CausesValidation="false" ForeColor="Black">Reading To</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="100px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnLitters" runat="server" CommandName="Sort" CommandArgument="Litters"
                                                                            CausesValidation="false" ForeColor="Black">Litters</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="150px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnPerLitterCost" runat="server" CommandName="Sort" CommandArgument="PerLitterCost"
                                                                            CausesValidation="false" ForeColor="Black">Per Litter Cost</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="100px" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkbtnTotalCost" runat="server" CommandName="Sort" CommandArgument="TotalCost"
                                                                            CausesValidation="false" ForeColor="Black"> Total Cost</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" width="" style="padding-left: 9px;">
                                                                        <asp:LinkButton ID="lnkFuelStationName" runat="server" CommandName="Sort" CommandArgument="FuelStationName"
                                                                            CausesValidation="false" ForeColor="Black">Fuel Station Name</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" class="clsLabelgrd" width="150px" style="font-size: 10pt;">
                                                                        <span>Average (Per Ltr)</span>
                                                                    </th>
                                                                    <th align="center" width="75px">
                                                                        Edit
                                                                    </th>
                                                                    <th align="center" width="75px">
                                                                        Delete
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="11">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVehicleReadingAllocationDetails"
                                                                            PageSize="20">
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
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblVehicleNumber" runat="server" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblReadingDate" runat="server" Text='<%# Eval("ReadingDate","{0:dd-MMM-yyyy}")  %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReceiptNumber" runat="server" Text='<%# Eval("ReceiptNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReadingFrom" runat="server" Text='<%# Eval("ReadingFrom") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReadingTo" runat="server" Text='<%# Eval("ReadingTo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLitters" runat="server" Text='<%# Eval("Litters") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblPerLitterCost" runat="server" Text='<%# Eval("PerLitterCost") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblTotalCost" runat="server" Text='<%# Eval("TotalCost") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblFuelStationName" runat="server" Text='<%# Eval("FuelStationName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAverage" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand"
                                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblVehicleNumber" runat="server" Text='<%# Eval("VehicleNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="left" class="paddingL">
                                                                    <asp:Label ID="lblReadingDate" runat="server" Text='<%# Eval("ReadingDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReceiptNumber" runat="server" Text='<%# Eval("ReceiptNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReadingFrom" runat="server" Text='<%# Eval("ReadingFrom") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblReadingTo" runat="server" Text='<%# Eval("ReadingTo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLitters" runat="server" Text='<%# Eval("Litters") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblPerLitterCost" runat="server" Text='<%# Eval("PerLitterCost") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblTotalCost" runat="server" Text='<%# Eval("TotalCost") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblFuelStationName" runat="server" Text='<%# Eval("FuelStationName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAverage" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="DeleteCommand" CausesValidation="false"
                                                                        runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
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
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ObjectDataSource TypeName="BusinessLogic.TransportBL.VehicleReadingAllocationBL"
                                        EnablePaging="false" ID="ObjDSVehicleReadingAllocationDetails" runat="server"
                                        SelectMethod="GetAllVehicleReadingAllocationDetails" SortParameterName="asSortExpression"
                                        EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter ControlID="txtSearch" PropertyName="Text" Name="asFilter" Type="String" />
                                            <asp:ControlParameter ControlID="txtSearchReadingDate" PropertyName="Text" Name="asFilterDate"
                                                Type="String" />
                                            <asp:ControlParameter ControlID="hidIncludeAllDates" PropertyName="Value" Name="asIncludeAllDates"
                                                Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidCurrentOperation" runat="server" />
                        <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" EnableViewState="true"></asp:HiddenField>
                        <asp:HiddenField ID="hidSortExpression" runat="server" EnableViewState="true"></asp:HiddenField>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:PostBackTrigger ControlID="btnCancel" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwVehicleReadingAllocationDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="ddlVehicleNumber" EventName="SelectedIndexChanged" />
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
    <script type="text/javascript" lang="javascript">
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _ClientCurrentOPeration = "<%=this.hidCurrentOperation.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _ClienttxtReadingFrom = "<%=this.txtReadingFrom.ClientID %>";
        _ClienttxtReceiptNumber = "<%=this.txtReceiptNumber.ClientID %>";
        _ClienttxtReadingDate = "<%=this.txtReadingDate.ClientID %>";
        _ClienttxtReadingTo = "<%=this.txtReadingTo.ClientID %>";
        _ClientddlVehicleNumber = "<%=this.ddlVehicleNumber.ClientID %>";

        function CalculateAmount() {
            var ltr = $('[id$=txtLitters]').val()
            var price = $('[id$=txtPerLitterCost]').val()

            if (ltr != '' && price != '')
                $('[id$=txtTotalCost]').val(parseFloat(ltr) * parseFloat(price))
            else
                $('[id$=txtTotalCost]').val(0)
        }

    </script>
    <script type="text/javascript" src="../Scripts/Transport/VehicleReadingAllocationDetails.js"></script>
</asp:Content>
