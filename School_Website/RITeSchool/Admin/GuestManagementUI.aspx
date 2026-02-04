<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GuestManagementUI.aspx.cs" Inherits="GuestManagementUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" style="width: 100%">
        <tr align="center">
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr>
            <td id="tdMessage" runat="server" align="center">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="height: 10px;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" />
                          <asp:CustomValidator ID="cstCategoryName" runat="server" ErrorMessage="" ClientValidationFunction="ValidateCategory"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstGuestName" runat="server" ErrorMessage="" ClientValidationFunction="CheckGuestName"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstDate" runat="server" ErrorMessage="" ClientValidationFunction="CheckDateIsNotEmpty"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="InTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateInTime"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstOutTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateOutTime"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstCompareDates" runat="server" ErrorMessage="" ClientValidationFunction="CompareTimes"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstMobileNumber" runat="server" ErrorMessage="" ClientValidationFunction="CheckMobileNumberIsEmpty"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstAadharCard" runat="server" ErrorMessage="" ClientValidationFunction="CheckAadharCardNo"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstPanCard" runat="server" ErrorMessage="" ClientValidationFunction="CheckPanCardNo"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstPurpose" runat="server" ErrorMessage="" ClientValidationFunction="PurposeOfVisit"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstOrganisation" runat="server" ErrorMessage="" ClientValidationFunction="CheckOrganisationIsEmpty"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstWhomeUMeet" runat="server" ErrorMessage="" ClientValidationFunction="WhomeUMeet"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstDesignation" runat="server" ErrorMessage="" ClientValidationFunction="Designation"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstGuestPhoto" runat="server" ErrorMessage="" ClientValidationFunction="GuestPhotoValidation"
                            Display="None"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" style="width: 64%; text-align: center">
                          <tr>
                             <td align="left" style="width: 180px;" class="ClsBorderLight">
                                    <asp:Label ID="lblCategoryType" runat="server" CssClass="ClsLabel" Text="Category Type" Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlCategoryType" runat="server" CssClass="LrgCombo" TabIndex="0">                                   
                                    </asp:DropDownList>                                    
                                    <span class="ClsMdtStar">* </span>
                                </td>
                          </tr>
                            <tr>
                          
                                <td align="left" style="width: 180px;" class="ClsBorderLight">
                                    <asp:Label ID="lblVisitorName" runat="server" CssClass="ClsLabel" Text="Name" Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" TabIndex="0">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtVisitorName" CssClass="LrgTxtBox" MaxLength="100" runat="server"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td class="ClsBorderlight" style="width: 180px">
                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="Date"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtVisitDate" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                                    <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtVisitDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblInTime" runat="server" CssClass="ClsLabel" Text="In Time"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtInTime" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblOutTime" runat="server" CssClass="ClsLabel" Text="Out Time"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtOutTime" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabel" Text="Mobile No."
                                        Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtMobileNo" MaxLength="10" runat="server" ViewStateMode="Enabled"
                                        onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                        onpaste="event.returnValue=false" CssClass="LrgTxtBox" Style="position: relative;
                                        top: 0px; left: 0px;"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblAadharCard" runat="server" CssClass="ClsLabel" Text="Aadhar Card No."
                                        Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAadharNumber" MaxLength="12" runat="server" ViewStateMode="Enabled"
                                        onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                        onpaste="event.returnValue=false" CssClass="LrgTxtBox" Style="position: relative;
                                        top: 0px; left: 0px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="PAN Card No." Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPanNo" runat="server" MaxLength="10" ViewStateMode="Enabled"
                                        CssClass="LrgTxtBox" Style="position: relative; top: 0px; left: 0px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblPurpose" runat="server" CssClass="ClsLabel" Text="Purpose Of Visit"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td colspan="3" align="left">
                                    <asp:TextBox ID="txtPurpose" CssClass="LrgTxtBox" MaxLength="250" Width="95%" Height="50px"
                                        runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblOrganiation" runat="server" CssClass="ClsLabel" Text="Parent / Company Name"
                                        Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td colspan="3" align="left">
                                    <asp:TextBox ID="txtOrganisation" Width="95%" MaxLength="100" CssClass="LrgTxtBox"
                                        runat="server"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text="Concern Person"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtName" CssClass="LrgTxtBox" MaxLength="50" runat="server" autocomplete="off"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Designation"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDesignation" CssClass="LrgTxtBox" MaxLength="50" runat="server"></asp:TextBox>
                                            <span class="ClsMdtStar">* </span>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtName" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderLight">
                                    <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Photo"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <img id="imgPhoto" visible="false" alt="image" runat="server" height="151" width="119" />
                                    <img id="ImgWebCam" title="<%$ Resources:LocalizedResources, CapturePhoto%>" runat="server"
                                        style="cursor: pointer;" src="../images/WebCam.png" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table align="center" style="width: 60%; text-align: center">
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                            CausesValidation="False" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                            OnClick="btnSave_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSearch" CssClass="ClsBtn" runat="server" Style="visibility: hidden;
                                            height: 10px;" Text="Search" CausesValidation="False" OnClick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        <tr align="center">
            <td align="center">
                <table style="width: 90%; text-align: center;">
                    <tr align="center">
                        <td align="center">
                            <table style="width: 43%; text-align: center;" align="center">
                            <tr>
                             <td align="center" style="width: 180px;" class="ClsBorderLight">
                                    <asp:Label ID="lblGuestCategoryType" runat="server" CssClass="ClsLabel" Text=" Guest CategoryType" Height="16px"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlGuestCategoryType" runat="server" CssClass="LrgCombo" 
                                        TabIndex="0" >                                   
                                    </asp:DropDownList>
                                    
                                    <span class="ClsMdtStar">* </span>
                                </td>
                          </tr>
                                <tr align="center">
                                    <td align="left" style="width: 50%;" class="ClsBorderLight">
                                        <asp:Label ID="lblGuestType" runat="server" CssClass="ClsLabel" Text="Guest Type"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlGuestType" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlGuestType_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="-- All --"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Now Present In School"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Left from School"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="left" class="ClsBorderLight">
                                        <asp:Label ID="lblSearchGuest" runat="server" CssClass="ClsLabel" Text="Name / Mobile No. / Aadhar Card No. / PAN Card No."></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtGuestName" CssClass="LrgTxtBox" runat="server" autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="2">
                                        <asp:Button ID="btnSearchGuest" CssClass="ClsBtn" CausesValidation="false" runat="server"
                                            Text="Search" OnClick="btnSearchGuest_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr id="trItemCount" runat="server">
                        <td align="center" style="width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstSchoolGuestDetails"
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
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlGuestType" EventName="SelectedIndexChanged" />
                                      <asp:AsyncPostBackTrigger ControlID="ddlGuestCategoryType" EventName="SelectedIndexChanged"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="text-align: center;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ListView ID="lstSchoolGuestDetails" runat="server" DataKeyNames="GuestId" OnItemCommand="lstSchoolGuestDetails_ItemCommand"
                                        OnItemDataBound="lstSchoolGuestDetails_ItemDataBound" OnItemDeleting="lstSchoolGuestDetails_ItemDeleting"
                                        OnItemEditing="lstSchoolGuestDetails_ItemEditing" OnSelectedIndexChanged="lstSchoolGuestDetails_SelectedIndexChanged"
                                        OnDataBound="lstSchoolGuestDetails_DataBound">
                                        <LayoutTemplate>
                                            <table width="100%" align="center" style="color: #333333" cellpadding="0" cellspacing="1"
                                                class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="clsLabelgrd"  width="100px" style="padding-left: 10px;">
                                                        <span><b>Category</b></span>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" style="padding-left: 10px;">
                                                        <span><b>Name</b></span>
                                                    </th>
                                                    <th align="center" width="110px" class="clsLabelgrd">
                                                        <span><b>Date</b></span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        <span><b>In Time</b></span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        <span><b>Out Time</b></span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="100px">
                                                        <span><b>Mobile No.</b></span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="150px">
                                                        <span><b>Aadhar Card No.</b></span>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="150px">
                                                        <span><b>PAN Card No.</b></span>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="180px" style="padding-left: 10px;">
                                                        <span><b>Concern Person</b></span>
                                                    </th>
                                                    <th width="40px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="40px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                    <th width="90px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="Label3" runat="server" Text="Print" ToolTip="Print"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="10" align="left">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstSchoolGuestDetails">
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
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblCategoryName" runat="server" CssClass="ClsLabel" Text='<%#Eval("CategoryName") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("GuestName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblInTime" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("InTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblOutTime" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("OutTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("MobileNum") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblAadharCardNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("AadharCardNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPanCardNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("PanCardNo") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblWhomeToMeet" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("WhomToMeet") %>'></asp:Label>
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
                                                <td align="center">
                                                    <asp:Button ID="btnPrint" CssClass="ClsBtn" runat="server" Text="Print" CausesValidation="false"
                                                        CommandName="EXPORT" ToolTip="Print" CommandArgument="<%# Container.DataItemIndex %>" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text='<%#Eval("CategoryName") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("GuestName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblInTime" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("InTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblOutTime" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("OutTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblMobileNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("MobileNum") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblAadharCardNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("AadharCardNo") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPanCardNo" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("PanCardNo") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="padding-left: 5px;">
                                                    <asp:Label ID="lblWhomeToMeet" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("WhomToMeet") %>'></asp:Label>
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
                                                <td align="center">
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="ClsBtn" Text="Print" CausesValidation="false"
                                                        CommandName="EXPORT" ToolTip="Print" CommandArgument="<%# Container.DataItemIndex %>" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlGuestType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlGuestCategoryType" EventName="SelectedIndexChanged"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearchGuest" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center;">
                        <td align="center" style="text-align: center;">
                              <asp:Button ID="btnExportAll" CssClass="ClsBtn" runat="server" Text="Export All"
                                CausesValidation="false" OnClick="btnExportAll_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hidGuestId" runat="server" Value="0" />
                                     <asp:ObjectDataSource TypeName="BusinessLogic.SchoolGuestDetailsBL" EnablePaging="true"
                                        ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="GetCount"
                                        EnableCaching="false">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter Name="asFilter" ControlID="txtGuestName" PropertyName="Text" />
                                            <asp:ControlParameter Name="asGuestType" ControlID="ddlGuestType" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter Name="aiCategoryType" ControlID="ddlGuestCategoryType" PropertyName="SelectedValue" Type="int32" />        
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="lstSchoolGuestDetails" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlGuestType" EventName="SelectedIndexChanged" />
                                      <asp:AsyncPostBackTrigger ControlID="ddlGuestCategoryType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearchGuest" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienttxtInTime = "<%=this.txtInTime.ClientID %>";
        _clienttxtOutTime = "<%=this.txtOutTime.ClientID %>"
        _clienttxtVisitorName = "<%=this.txtVisitorName.ClientID %>"
        _clienttxtVisitDate = "<%=this.txtVisitDate.ClientID %>"
        _clienttxtMobileNo = "<%=this.txtMobileNo.ClientID %>"
        _clienttxtAadharNumber = "<%=this.txtAadharNumber.ClientID %>"
        _clienttxtPanNo = "<%=this.txtPanNo.ClientID %>"
        _clienttxtPurpose = "<%=this.txtPurpose.ClientID %>"
        _clienttxtOrganisation = "<%=this.txtOrganisation.ClientID %>"
        _clienttxtName = "<%=this.txtName.ClientID %>"
        _clienttxtDesignation = "<%=this.txtDesignation.ClientID %>"
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _clientddlCategoryType = "<%=this.ddlCategoryType.ClientID %>"
        
        
         function ValidateInTime(oSrc, args) {
            var InTime = $('#' + _clienttxtInTime).val()
            if (InTime.trim() != "") {
                if (!isTimeValid(_clienttxtInTime)) {
                    oSrc.errormessage = "In Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            else {
                oSrc.errormessage = "In Time should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateOutTime(oSrc, args) {
            var OutTime = $('#' + _clienttxtOutTime).val()
            if (OutTime.trim() != "") {
                if (!isTimeValid(_clienttxtOutTime)) {
                    oSrc.errormessage = "Out Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function CheckGuestName(oSrc, args) {
            var GuestName = $('#' + _clienttxtName).val()
            if (GuestName.trim() == "") {
                oSrc.errormessage = "Name should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


        function ValidateCategory(oSrc, args) {
           {
                var Category = $get("<%=this.ddlCategoryType.ClientID %>").value;
                if (Category == 0) {
                    oSrc.errormessage = "Category Name should be selected.";
                    args.IsValid = false
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

       
        function CheckDateIsNotEmpty(oSrc, args) {
            var VisitDate = $('#' + _clienttxtVisitDate).val()
            if (VisitDate.trim() == "") {
                oSrc.errormessage = "Date should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckMobileNumberIsEmpty(oSrc, args) {            
            var MObileNo = $('#' + _clienttxtMobileNo).val()
            if (MObileNo.trim() == "") {
                oSrc.errormessage = "Mobile No. should not be blank."
                args.IsValid = false
                return true
            }
            else {
                if (MObileNo.trim().length < 10) {
                    oSrc.errormessage = "Mobile No. should not be less than 10 digit."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function PurposeOfVisit(oSrc, args) {
            var PurposeOfVisit = $('#' + _clienttxtPurpose).val()
            if (PurposeOfVisit.trim() == "") {
                oSrc.errormessage = "Purpose of visit should not be blank."
                args.IsValid = false
                return true
            }
            else if (PurposeOfVisit.trim().length > 250) {
                oSrc.errormessage = "Purpose of visit - Length should not be grater than 250 character."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckOrganisationIsEmpty(oSrc, args) {
            var OrganisationName = $('#' + _clienttxtOrganisation).val()
            if (OrganisationName.trim() == "") {
                oSrc.errormessage = "Organisation/Company name should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckAadharCardNo(oSrc, args) {
            var AadharCardNo = $('#' + _clienttxtAadharNumber).val()
            if (AadharCardNo.trim() != "") {
                if (AadharCardNo.trim().length < 12) {
                    oSrc.errormessage = "Aadhar Card No. should not be less than 12 digit."
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
            args.IsValid = true
            return false
        }

        function CheckPanCardNo(oSrc, args) {
            var PanCardNo = $('#' + _clienttxtPanNo).val()
            if (PanCardNo.trim() != "") {
                if (PanCardNo.trim().length < 10) {
                    oSrc.errormessage = "PAN Card No. should not be less than 10 digit."
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
            args.IsValid = true
            return false
        }

        function WhomeUMeet(oSrc, args) {
            var WhomeUMeet = $('#' + _clienttxtName).val()
            if (WhomeUMeet.trim() == "") {
                oSrc.errormessage = "Whome You Meet should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function Designation(oSrc, args) {
            var DesignationName = $('#' + _clienttxtDesignation).val()
            if (DesignationName.trim() == "") {
                oSrc.errormessage = "Designation should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function GuestPhotoValidation(oSrc, args) {
            var IsPhotoCaptured = $('#' + _clienthidIsPhotoCaptured).val()
            if (IsPhotoCaptured.trim() != "" && IsPhotoCaptured == "N") {
                oSrc.errormessage = "Photo should be captured."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CompareTimes(oSrc, args) {
            var InTime = $('#' + _clienttxtInTime).val()
            var OutTime = $('#' + _clienttxtOutTime).val()

            if (InTime.trim() != "" && OutTime.trim() != "") {
                if (InTime > OutTime) {
                    oSrc.errormessage = "Out time should be greater than In time.";
                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true
            return false
        }

        function isTimeValid(txtTimeId) {
            var timeStr = trimAll(document.getElementById(txtTimeId).value.toUpperCase());
            if (trimAll(timeStr) == '')
                return false;

            // Checks if time is in HH:MM 12 hour format.
            // The seconds are optional.
            var timePat = /^(\d{1,2}):(\d{1,2})?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            if (timeStr.length < 6)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            ampm = matchArray[4];

            if (ampm == "") {
                return false;
            }

            if (hour <= 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + minute + '0';
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(txtTimeId).value = str;
            return true;
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function OpenWebcamPopup(sQueryString) {
            window.open('../Common/WebcamNewPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400').focus();
            return true;
        }

        function UpdateHiddenField() {
            $get(_clienthidIsPhotoCaptured).value = "Y";
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 0);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }
    </script>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
