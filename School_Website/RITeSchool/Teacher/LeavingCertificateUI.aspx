<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeavingCertificateUI.aspx.cs"
    EnableEventValidation="false" Inherits="LeavingCertificateUI" MasterPageFile="../MasterPages/PopupMaster.master" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table cellpadding="0" cellspacing="1" width="90%">
        <tr>
            <td align="right" colspan="2" style="float: right">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatory" runat="server" ForeColor="Red" EnableViewState="False"
                    CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:ValidationSummary ValidationGroup="Save" ID="valSumErrorMsg" runat="server"
                    CssClass="ClsLabel" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
                <asp:ValidationSummary ValidationGroup="Search" ID="ValidationSummary1" runat="server"
                    CssClass="ClsLabel" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
            </td>
            <td style="float: right;" align="right">
                <asp:CustomValidator ID="cstReg" runat="server" ClientValidationFunction="validateReg"
                    CssClass="ClsMdtStar" ValidationGroup="Search" Display="None" EnableClientScript="true"
                    ErrorMessage="<%$ Resources:LocalizedResources, NameOrRegNumberBlank%>" Visible="true"></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <table width="90%">
        <tr>
            <td>
                <table style="width: 100%; vertical-align: top" cellspacing="1" cellpadding="0" border="0">
                    <tr>
                        <td style="background-color: white;">
                            <table width="100%">
                                <tr id="trlblError" runat="server">
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
                                            <ContentTemplate>
                                                <asp:Label ID="lblErr" CssClass="ClsLabel" ForeColor="red" runat="server"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                            <ContentTemplate>
                                                <table id="tblSearchInput" runat="server">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblNameRegNo" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, NameOrReg%>"></asp:Label>
                                                            <span class="ClsLabel colonpadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" autocomplete="off"></asp:TextBox>
                                                            <span style="color: #ff0000">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSearch" ValidationGroup="Search" CssClass="ClsBtn" runat="server"
                                                                Text="<%$ Resources:LocalizedResources, Search%>" OnClick="btnSearch_Click" Width="100px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="left" valign="top">
                                        <table id="tblReport" runat="server">
                                            <tr>
                                                <td align="left" style="vertical-align: middle">
                                                    <asp:DropDownList ID="DDLFormatType" runat="server" CssClass="MidCombo" Width="100px">
                                                        <asp:ListItem>PDF</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" colspan="1" width="20%" class="ClsBorderlight">
                                                    <asp:Label ID="Label2" runat="server" class="ClsLabel" Text="Print Date"></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="calPrintdateTop" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                        MaxLength="11"></asp:TextBox>
                                                    <rjs:PopCalendar ID="PopCalendar2" runat="server" Control="calPrintdateTop" Format="dd MMM yyyy"
                                                        ShowWeekend="True" ShowErrorMessage="false" />                                                    
                                                </td>
                                                <td id="tdDisplayInmarathi" runat="server" visible="false" align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label ID="lblChkDisplayInMarathi" runat="server" class="ClsLabel" Text="Display in Marathi"></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td id="tdSSNDisplayInmarathi" runat="server" visible="false">
                                                    <asp:CheckBox ID="chkDisplayMarathi" runat="server" EnableViewState="false"/>
                                                </td>
                                                <td align="left" style="vertical-align: top; width: 100px;">
                                                    <asp:Button ID="btnReport" runat="server" Text="<%$ Resources:LocalizedResources, PrintLC%>"
                                                        CssClass="ClsBtn" OnClick="btnReport_Click" Width="99px" CausesValidation="true"
                                                        ValidationGroup="Save" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width: 100%; vertical-align: top" cellspacing="1" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr id="Tr5">
                                                                <td align="center">
                                                                    <asp:DataPager ID="DataPgCnt" runat="server" PageSize="5" PagedControlID="lstVwStudent">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To%>"
                                                                                        EnableViewState="false" />
                                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>"
                                                                                        EnableViewState="false" />
                                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>"
                                                                                        EnableViewState="false" />
                                                                                    <br />
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <div>
                                                                        <asp:ListView ID="lstVwStudent" runat="server" DataKeyNames="Enrolment_Number,YearWise_Student_Id,SchoolWise_Student_Id"
                                                                            OnItemCommand="lstVwStudent_ItemCommand" OnDataBound="lstVwStudent_DataBound"
                                                                            OnItemDataBound="lstVwStudent_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                cellspacing="1">
                                                                                                <tr id="Tr1" runat="server" class="ClsGridHeader">
                                                                                                    <th id="Th2" runat="server" align="left" class="ClspaddingL">
                                                                                                        <asp:Label ID="lblRegNo" runat="server" Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                                                    </th>
                                                                                                    <th id="Th3" runat="server" align="left" class="ClspaddingL">
                                                                                                        <asp:Label ID="lblClass" runat="server" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                                    </th>
                                                                                                    <th id="Th1" runat="server" align="left" class="ClspaddingL">
                                                                                                        <asp:Label ID="lblRollNo" runat="server" Text="<%$ Resources:LocalizedResources,RollNo%>"></asp:Label>
                                                                                                    </th>
                                                                                                    <th id="Th4" runat="server" align="left" class="ClspaddingL">
                                                                                                        <asp:Label ID="lblStudentName" runat="server" Text="<%$ Resources:LocalizedResources,StudentName%>"></asp:Label>
                                                                                                    </th>
                                                                                                    <th id="Th5" runat="server">
                                                                                                        <asp:Label ID="lblDOB" runat="server" Text="<%$ Resources:LocalizedResources,DateOfBirth%>"></asp:Label>
                                                                                                    </th>
                                                                                                    <th id="Th6" runat="server">
                                                                                                        <asp:Label ID="lblSelectStudent" runat="server" Text="<%$ Resources:LocalizedResources,SelectStudent%>"></asp:Label>
                                                                                                    </th>
                                                                                                </tr>
                                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                                </tr>
                                                                                            </table>
                                                                                            <table width="100%" runat="server" id="tblDataPager" style="color: #333333" cellpadding="0"
                                                                                                cellspacing="1">
                                                                                                <tr class="ClsGridAltRow">
                                                                                                    <td>
                                                                                                        <asp:DataPager ID="DataPgCnt1" runat="server" PageSize="5" PagedControlID="lstVwStudent">
                                                                                                            <Fields>
                                                                                                                <asp:TemplatePagerField>
                                                                                                                    <PagerTemplate>
                                                                                                                        <table width="100%">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources,SelectPage%>"
                                                                                                                                        runat="server" CssClass="LblNrmlB" />
                                                                                                                                    <span class="colonPadding">:</span>
                                                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                                                                    </asp:DropDownList>
                                                                                                                                </td>
                                                                                                                                <td align="right">
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
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Enrolment_Number") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("StandardDivision") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblRoll_No" runat="server" Text='<%# Eval("Roll_No") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblStudent_Name" runat="server" Text='<%# Eval("Name") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Convert.ToDateTime(Eval("DOB")).ToString("dd-MMM-yyyy")%>'
                                                                                            CssClass="LblNormal" />
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                            CommandName="Select" CommandArgument='<%# Eval("Enrolment_Number") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Enrolment_Number") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("StandardDivision") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblRoll_No" runat="server" Text='<%# Eval("Roll_No") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="lblStudent_Name" runat="server" Text='<%# Eval("Name") %>' CssClass="ClspaddingL" />
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Convert.ToDateTime(Eval("DOB")).ToString("dd-MMM-yyyy") %>'
                                                                                            CssClass="LblNormal" />
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                            CommandName="Select" CommandArgument='<%# Eval("Enrolment_Number") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="lstDSobj"
                                                                        runat="server" SelectMethod="GetAllLeaveStudents" SortParameterName="sortExpression"
                                                                        SelectCountMethod="CountRowsOfLeaveStaudent" EnableCaching="false">
                                                                        <SelectParameters>
                                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                Type="string" />
                                                                            <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asName" />
                                                                        </SelectParameters>
                                                                    </asp:ObjectDataSource>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: white;" id="MainDataTable" runat="server" align="center"
                                                valign="top" visible="false">
                                                <asp:UpdatePanel runat="server" ID="UPnl">
                                                    <ContentTemplate>
                                                        <!-- Data Insert Here -->
                                                        <table runat="server" id="tblLC" style="width: 95%;" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="center" colspan="4" id="tdMessage" runat="server">
                                                                        <asp:Label ID="lblUpdateMessage" runat="server" ForeColor="Blue" Width="100%" CssClass="ClsLabel"
                                                                            EnableViewState="false" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="4">
                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="height: 20px; padding-top: 10px;" colspan="1">
                                                                                        <table id="tblHead" runat="server" class="ClsGrayMainTitle" style="padding-right: 5px"
                                                                                            cellspacing="0" cellpadding="0" border="0">
                                                                                            <tr>
                                                                                                <td style="height: 20px" class="MainTitleHead">
                                                                                                    <asp:Label ID="lblLeavingCertificateDetails" runat="server" class="MainTitleHead"
                                                                                                        Style="font-weight: bold" Text="<%$ Resources:LocalizedResources,LeavingCertificateDetails%>"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                        <asp:Label ID="lblStandard" Font-Bold="True" Text="" BorderWidth="0px" runat="server"
                                                                            EnableViewState="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr id="LCSerialNo" runat="server">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight" id="tdlblPrintCount" runat="server">
                                                                        <asp:Label ID="lblSerialNo" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,SerialNumber%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight" id="tdlPrintCount" runat="server">
                                                                        <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="MidTxtBox" MaxLength="15"
                                                                            Enabled="false"></asp:TextBox>
                                                                    </td>   
                                                                    <%-- <td align="right" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="LC Print No."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="right" colspan="1" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtLCPrint" runat="server" CssClass="MidTxtBox" MaxLength="15"
                                                                            Enabled="false"></asp:TextBox>
                                                                    </td>   --%>                                                                 
                                                                </tr>--%>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblRegistrationNumber" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,RegistrationNumber%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtRegNumber" runat="server" CssClass="MidTxtBox" MaxLength="15"
                                                                            Enabled="false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblUDISENo" runat="server" class="ClsLabel" Text="Student UDISE Number"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtUDISENO" runat="server" CssClass="MidTxtBox" MaxLength="50"
                                                                            Enabled="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label3" runat="server" class="ClsLabel" Text="Registration Number For LC."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtLCRegNumber" runat="server" CssClass="MidTxtBox" MaxLength="15"
                                                                            Enabled="false"></asp:TextBox>
                                                                        <%--<span style="color: #ff0000" class="ClsMdtStar">*</span>--%>
                                                                        <%--<asp:RequiredFieldValidator ID="reqLCRegistrationNo" runat="server" ControlToValidate="txtLCRegNumber"
                                                                            Display="None" ErrorMessage="Registration number for LC should not be blank."
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblStudentName" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,StudentName%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtStudentName" runat="server" CssClass="LrgTxtBox" MaxLength="250" Width="400px"></asp:TextBox>
                                                                        <span style="color: #ff0000" class="ClsMdtStar">*</span>
                                                                        <asp:RequiredFieldValidator ID="reqValTxtStudentName" runat="server" ControlToValidate="txtStudentName"
                                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources,StudentNameShouldNotBeBlalnk%>"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                                        <asp:Label ID="Label21" runat="server" class="ClsLabel" Text="Father Name"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblparentGurdianName" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,ParentGuardianName%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtParentName" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblMotherName" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,MotherName%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtMotherName" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" colspan="1" style="padding-left: 5px; width: 25%;" class="ClsBorderlight"
                                                                        valign="middle">
                                                                        <asp:Label ID="lblNationality" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Nationality%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtNationality" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblReligionCast" runat="server" class="ClsLabel" Text="Religion"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtReligion" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                    
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label7" runat="server" class="ClsLabel" Text="Cast"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtCaste" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblCategory" runat="server" class="ClsLabel" Text="Category"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="MidTxtBox" Enabled="false"></asp:TextBox>                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblDateOfAddmission" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,AdmissionDateOnLC%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="28%" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtDateofAdmission" runat="server" CssClass="MidTxtBox" MaxLength="11"
                                                                            ValidationGroup="Save" CausesValidation="true"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="cDateofAdmission" runat="server" Control="txtDateofAdmission"
                                                                            Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,PleaseSelectValidDateOfAdmission%>"
                                                                            To-Today="true" Culture="en" />
                                                                        <span style="color: #ff0000" class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="csttxtDateofAdmission" runat="server" ClientValidationFunction="validateAdmissionDate"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                                                                            Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblDOB" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,DateOfBirth%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                                        <asp:TextBox ID="txtDOB" runat="server" CausesValidation="true" CssClass="MidTxtBox"
                                                                            MaxLength="11" ValidationGroup="Save"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="cDateOfBirth" runat="server" Control="txtDOB" Format="dd MMM yyyy"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,PleaseSelectValidBirthDate%>"
                                                                            To-Today="true" Culture="en" />
                                                                        <span style="color: #ff0000" class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="cstvalDOB" runat="server" ClientValidationFunction="validateBirthDate"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                                                                            Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblBirthPlace" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,BirthPlace%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1">
                                                                        <asp:TextBox ID="txtBirthPlace" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>  
                                                                   <tr id="trDOBText" runat="server" >
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label20" runat="server" class="ClsLabel" Text="Date of Birth (In Words)"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" class="ClsBorderlight">
                                                                          <asp:TextBox ID="txtDOBWords" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                                            Width="350px"></asp:TextBox>
                                                                       
                                                                    </td>
                                                                  </tr>                                                   
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblTql" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,BirthTal%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtBirthTaluka" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblDist" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,BirthDist%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtBirthDistrict" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>                                                               
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblState" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,State%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtState" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblCountry" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Country%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>                                                                 
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblMotherTongue" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,MotherTongue%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtMotherTongue" runat="server" CssClass="MidTxtBox" MaxLength="20"
                                                                            Width="145px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblAadharCard" runat="server" class="ClsLabel" 
                                                                            Text="Aadhar Card No." Width="117px"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtAadharCardNo" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trStudentNOSVP" runat="server" visible="false">
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label9" runat="server" class="ClsLabel" Text="Student No."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtStudentNo" runat="server" CssClass="MidTxtBox" MaxLength="20"
                                                                            Width="145px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="4">
                                                                        <asp:Label ID="lblLastSchoolAttended" runat="server" class="ClsLblLgnd" Style="font-weight: bold"
                                                                            Text="<%$ Resources:LocalizedResources,LastSchoolAttendedDetails%>"> </asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblSchoolName" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,SchoolName%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="txtLstSchoolName" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                                            Width="350px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RegularExpressionValidator ID="regLastSchoolDetails" runat="server" ControlToValidate="txtLastSchoolDetails"
                                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources,LastSchoolAddressShouldOfLengthLessThan%>"
                                                                            ValidationExpression="^[\s\S]{0,150}$" ValidationGroup="Save">
                                                                        </asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblAddress" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Address%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="txtLastSchoolDetails" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                                            Width="94%"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                        <asp:CustomValidator ID="cstValLastschoolAddress" runat="server" ClientValidationFunction="validateLastSchoolAddress"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Error msg"
                                                                            ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblStandard1" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Standard%>"> </asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" rowspan="1" valign="top" width="25%">
                                                                        <asp:TextBox ID="txtLstStandardDivName" runat="server" CssClass="MidTxtBox" MaxLength="15"
                                                                            Width="100%"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" style="width: 23%">
                                                                    </td>
                                                                    <td align="left" width="25%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="4">
                                                                        <asp:Label ID="lblStudentAcademicDetail" runat="server" class="ClsLblLgnd" Style="font-weight: bold"
                                                                            Text="<%$ Resources:LocalizedResources,StudentAcademicDetails%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblProgressRemark" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,ProgressRemark%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" rowspan="1" valign="top">
                                                                        <asp:TextBox ID="txtProgress" runat="server" CssClass="MidTxtBox" Height="32px" TextMode="MultiLine"
                                                                            Width="350px"></asp:TextBox>
                                                                        <asp:CustomValidator ID="cstValAcademicProgress" runat="server" ClientValidationFunction="validateAcademicProgress"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Error msg"
                                                                            ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblCurrentClass" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,CurrentClass%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="txtCurrentStandard" runat="server" CssClass="MidTxtBox" MaxLength="22"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label4" runat="server" class="ClsLabel" Text="Admission Standard"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="txtAdmissionStandard" runat="server" CssClass="MidTxtBox" MaxLength="22"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="30%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblYearSinceStuding" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,YearSinceWhenStudyingInSchool%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                        <asp:TextBox ID="txtYearOfLeaving" CssClass="MidTxtBox" runat="server" />
                                                                    </td>
                                                                    <td style="width: 23%">
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblSubjectStudied" runat="server" class="clsLabel" Text="Subject Studied"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                        <asp:TextBox ID="txtSubjectStudied" CssClass="ExLrgTxtBox" Width="350px" runat="server" />
                                                                    </td>
                                                                    <td style="width: 23%">
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblDateOfLeavigSchool" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,DateOfLeavingSchool%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="calDateOfLeaving" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                                            MaxLength="11"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="cDateOfLeaving" runat="server" Control="calDateOfLeaving" Format="dd MMM yyyy"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,PleaseSelectValidDateOfAdmission%>"
                                                                            Culture="en" />
                                                                        &nbsp;<span style="color: #ff0000" class="ClsMdtStar">*</span>
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                        <asp:CustomValidator ID="cstDOL" runat="server" ClientValidationFunction="validateDateOfLeaving"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ValidationGroup="Save"
                                                                            Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblReasonForLeavingSchool" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,ReasonForLeavingSchool%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3" width="50%">
                                                                        <asp:TextBox ID="txtReason" runat="server" CssClass="MidTxtBox" Height="32px" TextMode="MultiLine"
                                                                            Width="350px"></asp:TextBox>
                                                                        <asp:CustomValidator ID="cstValReason" runat="server" ClientValidationFunction="validateReasonOfLeaving"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Error msg"
                                                                            ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblConduct" runat="server" class="ClsLabel" 
                                                                            Text="" Width="46px"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:TextBox ID="txtConduct" Width="350px" runat="server" CssClass="MidTxtBox" MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblPromotion" runat="server" class="ClsLabel" Text=""></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:TextBox ID="txtPromotion" Width="350px" runat="server" CssClass="MidTxtBox"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label5" runat="server" class="ClsLabel" Text="Exam Status"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:RadioButton ID="rdoPass" runat="server" Text="Pass" 
                                                                            GroupName="ExamStatus" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoFail" runat="server" Text="Fail" 
                                                                            GroupName="ExamStatus" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoExamStatusNA" runat="server" Text="N/A" 
                                                                            GroupName="ExamStatus" CssClass = "ClsLabel" Visible="false" />
                                                                        <asp:RadioButton ID="rdoExamStatusBlank" runat="server" Text="Keep It Blank" 
                                                                            GroupName="ExamStatus" CssClass = "ClsLabel" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="All sum due to school have been settled"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
<%--                                                                    <td align="left" colspan="3">
                                                                        <asp:CheckBox ID="chkAllDueSettled" runat="server" />
                                                                    </td>--%>
                                                                       <td align="left" colspan="3">
                                                                        <asp:RadioButton ID="rdoYes" runat="server" Text="Yes" 
                                                                            GroupName="SchoolSettled" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoNo" runat="server" Text="No" 
                                                                            GroupName="SchoolSettled" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoNotApplicable" runat="server" Text="N/A" 
                                                                            GroupName="SchoolSettled" CssClass = "ClsLabel" />
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="lblRemarks" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Remarks%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="MidTxtBox" Height="44px" TextMode="MultiLine"
                                                                            Width="400px"></asp:TextBox>
                                                                        <asp:CustomValidator ID="cstValRemarks" runat="server" ClientValidationFunction="validateRemarks"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Error msg"
                                                                            ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPramotedToNext" runat="server" style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label10" runat="server" class="ClsLabel" Text="Is Promoted to Next Standard"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:CheckBox ID="chkIsPromotedToNext" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPramotedToNextPPSH" runat="server" style="width: 100%">
                                                                     <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label12" runat="server" class="ClsLabel" Text="Is Promoted to Next Standard"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                        <asp:RadioButton ID="rdoPramotedYes" runat="server" Text="Yes" 
                                                                            GroupName="PramotedToNext" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoPramotedNo" runat="server" Text="No" 
                                                                            GroupName="PramotedToNext" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoPromotedNA" runat="server" Text="N/A" 
                                                                            GroupName="PramotedToNext" CssClass = "ClsLabel" />
                                                                        <asp:RadioButton ID="rdoPromotedBlank" runat="server" Text="Keep It Blank" 
                                                                            GroupName="PramotedToNext" CssClass = "ClsLabel" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label11" runat="server" class="ClsLabel" Text="Date Of Application"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" colspan="3">
                                                                         <asp:TextBox ID="txtApplicationDate" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                                            MaxLength="11"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="calDateOfApplication" runat="server" Control="txtApplicationDate" Format="dd MMM yyyy"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,PleaseSelectValidDateOfAdmission%>"
                                                                            Culture="en" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label13" runat="server" class="ClsLabel" Text="Book No."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtBookNo" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                                            MaxLength="11"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label14" runat="server" class="ClsLabel" Text="SL. No."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtSLNo" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                                            MaxLength="11"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label15" runat="server" class="ClsLabel" Text="School/Board Annual examination last taken with result"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtLastExamTaken" CssClass="ExLrgCombo" runat="server" ValidationGroup="Save" Width="400px"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label16" runat="server" class="ClsLabel" Text="Whether failed, if so once/twice in the same class"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtWhetherFailed" CssClass="ExLrgCombo" runat="server" ValidationGroup="Save" Width="400px"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label17" runat="server" class="ClsLabel" Text="Extra curricular activities"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtExtraCurricular" CssClass="ExLrgCombo" runat="server" ValidationGroup="Save" Width="400px"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label18" runat="server" class="ClsLabel" Text="Academic Performance"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtAcademicPerformance" CssClass="ExLrgCombo"  runat="server" ValidationGroup="Save" MaxLength="500" Width="400px"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="ClsBorderlight">
                                                                        <asp:Label ID="Label19" runat="server" class="ClsLabel" Text="StudentUIDNo"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td colspan="3" align="left">
                                                                         <asp:TextBox ID="txtStudentUIDNo" CssClass="ExLrgCombo"  runat="server" ValidationGroup="Save" MaxLength="20" Width="400px"></asp:TextBox>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="clsBorderLight">
                                                                        <asp:Label ID="lblDocuments" runat="server" class="ClsLabel" Text="Document in support of D.O.B."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                       <asp:TextBox ID="txtDocument" CssClass="ExLrgCombo"  runat="server" ValidationGroup="Save" MaxLength="100" Width="400px"></asp:TextBox> 
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="clsBorderLight">
                                                                        <asp:Label ID="lblSchoolDues" runat="server" class="ClsLabel" Text="School dues/ paid (If any)"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                       <asp:TextBox ID="txtSchoolDues" CssClass="LrgCombo"  runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="1" align="left" width="25%" class="clsBorderLight">
                                                                        <asp:Label ID="Label22" runat="server" class="ClsLabel" Text="PEN No."></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                       <asp:TextBox ID="txtPenNo" CssClass="LrgCombo"  runat="server" ValidationGroup="Save" MaxLength="50"></asp:TextBox> 
                                                                    </td>
                                                                </tr>
                                                                 <tr>
                                                                    <td colspan="1" align="left" width="25%" class="clsBorderLight">
                                                                        <asp:Label ID="Label23" runat="server" class="ClsLabel" Text="APAAR ID"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                       <asp:TextBox ID="txtApaarId" CssClass="LrgCombo"  runat="server" ValidationGroup="Save" MaxLength="12"></asp:TextBox> 
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 100%">
                                                                    <td align="left" colspan="1" width="25%">
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                    </td>
                                                                    <td align="left" colspan="1" style="width: 23%">
                                                                    </td>
                                                                    <td align="left" colspan="1" width="25%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="4">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources,Save%>"
                                                                            ValidationGroup="Save" OnClick="btnSave_Click" CausesValidation="True" disable-page="true" />
                                                                        <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources,Close%>"
                                                                            OnClick="btnCancel_Click" CausesValidation="False" UseSubmitBehavior="false"
                                                                            OnClientClick="refreshParent" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="4">
                                                                        <asp:HiddenField ID="hidStandardId" runat="server" />
                                                                        <asp:HiddenField ID="hidDivisionId" runat="server" />
                                                                        <asp:HiddenField ID="hidStudentId" runat="server" />
                                                                        <asp:HiddenField ID="HidBackUrl" runat="server" />
                                                                        <asp:HiddenField ID="hidLcDetailId" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="hidServerDate" runat="server" />                                                                        
                                                                        <asp:HiddenField ID="hidDateOfBirthInText" runat="server" />
                                                                        <asp:HiddenField ID="hidFormat" runat="server" />
                                                                        <asp:HiddenField ID="hidIsTeacher" runat="server" />
                                                                        <asp:HiddenField runat="server" ID="hidClassName" />
                                                                        <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                                                        <asp:HiddenField ID="hidLastAttendedSchoolStandardShouldBeBetween" runat="server" />
                                                                        <asp:HiddenField ID="hidDateOfLeavingShouldBeSelected" runat="server" />
                                                                        <asp:HiddenField ID="hidDateOfLeavingShouldNotBeFutureDate" runat="server" />
                                                                        <asp:HiddenField ID="hidDateOfLeavingShouldBeGreaterThanDateOfAdmission" runat="server" />
                                                                        <asp:HiddenField ID="hidDateOfAdmissionShouldBeSelected" runat="server" />
                                                                        <asp:HiddenField ID="hidDateOfAdmissionShouldNotBeFutureDate" runat="server" />
                                                                        <asp:HiddenField ID="hidBirthDateShouldBeSelected" runat="server" />
                                                                        <asp:HiddenField ID="hidBirthDateShouldNotBeFutureDate" runat="server" />
                                                                        <asp:HiddenField ID="hidProgressRemarkOfStudentShouldBeOfLengthLessThan" runat="server" />
                                                                        <asp:HiddenField ID="hidLastSchoolAttendedAddressDetailsOfStudentShould" runat="server" />
                                                                        <asp:HiddenField ID="hidReasonForLeavingSchoolShouldBeOfLength" runat="server" />
                                                                        <asp:HiddenField ID="hidRemarksShouldBeOfLengthLessThan" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="trReportBottom" runat="server">
                        <td align="center">
                            <table>
                                <tr>
                                    <td align="right" class="ClsBorderlight">
                                        <asp:Label ID="lblSelectDisplay" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,SelectDisplayType%>"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" style="padding-left: 5px;">
                                        <asp:DropDownList ID="DDLFormatType2" runat="server" CssClass="MidCombo">
                                            <asp:ListItem>PDF</asp:ListItem>
                                            <asp:ListItem>MS Word</asp:ListItem>
                                            <asp:ListItem>Excel</asp:ListItem>
                                        </asp:DropDownList>
                                        <td align="left" colspan="1" width="15%" class="ClsBorderlight">
                                            <asp:Label ID="Label1" runat="server" class="ClsLabel" Text="Print Date"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="calPrintdate" CssClass="SmlCombo" runat="server" ValidationGroup="Save"
                                                MaxLength="11" ></asp:TextBox>
                                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="calPrintdate" Format="dd MMM yyyy"
                                                ShowWeekend="True" ShowErrorMessage="false"  />                                            
                                        </td>
                                        <td align="left" runat="server" id="tdSSNMarathi" colspan="1" class="ClsBorderlight" visible="false">
                                            <asp:Label ID="Label8" runat="server" class="ClsLabel" Text="Display in Marathi"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td> 
                                        <td align="left">
                                            <asp:CheckBox ID="chkSSNMarathi" runat="server" Visible="false"/>                                                
                                            <asp:Button ID="btnReport2" Text="<%$ Resources:LocalizedResources, PrintLC%>" runat="server"
                                                CssClass="ClsBtn" OnClick="btnReport_Click" Width="99px" ValidationGroup="Save" />
                                        </td>                                       
                                        <asp:HiddenField ID="hidRegNo" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidSearch" runat="server" />
    <script language="javascript" type="text/javascript">

        _clientAcademicProgress = "<%=this.txtProgress.ClientID%>";
        _clienttxtLastSchoolDetails = "<%=this.txtLastSchoolDetails.ClientID%>";

        _clientAcademicProgressCustomValId = "<%=this.cstValAcademicProgress.ClientID%>";
        _clientcstValLastschoolAddressCustomValId = "<%=this.cstValLastschoolAddress.ClientID%>";
        _clientReason = "<%=this.txtReason.ClientID%>";
        _clientReasonCustomValId = "<%=this.cstValReason.ClientID%>";
        _clientRemarks = "<%=this.txtRemarks.ClientID%>";
        _clientRemarksCustomValId = "<%=this.cstValRemarks.ClientID%>";


        _clientLastSchool = "<%=this.txtLastSchoolDetails.ClientID%>";
        _clientLblDOA = "<%=this.txtDateofAdmission.ClientID%>";
        _clientDOL = "<%=this.calDateOfLeaving.ClientID%>";
        _clientcstValDOL = "<%=this.cstDOL.ClientID%>";
        _clientlblDOB = "<%=this.txtDOB.ClientID%>";
        _clientbtnSave = "<%=this.btnSave.ClientID%>";
        _clientbtnCancel = "<%=this.btnCancel.ClientID%>";
        _clienttxtYear = "<%=this.txtYearOfLeaving.ClientID %>"

        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clienthidLcDetailId = "<%=this.hidLcDetailId.ClientID %>";
        _clienttxtStandard = "<%=this.txtLstStandardDivName.ClientID%>";

        _clienttxtregNo = "<%=this.txtRegNo.ClientID%>";

        _clientvalSumErrorMsgID = "<%=this.valSumErrorMsg.ClientID%>";
        _clientbtnReportID = "<%=this.btnReport.ClientID%>";
        _clientddlrpt = "<%=this.DDLFormatType.ClientID%>";
        _clientbtnReportID2 = "<%=this.btnReport2.ClientID%>";
        _clientddlrpt2 = "<%=this.DDLFormatType2.ClientID%>";
        _clientlblErrID = "<%=this.lblErr.ClientID%>";
        _clientbtnSearchID = "<%=this.btnSearch.ClientID%>";
        _clientlstVwStudentID = "<%=this.lstVwStudent.ClientID%>";
        _clientMainDataTable = "<%=this.MainDataTable.ClientID%>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID%>";
        _clientcsttxtDateofAdmission = "<%=this.csttxtDateofAdmission.ClientID %>";
        _clientcstvalDOB = "<%=this.cstvalDOB.ClientID %>";
        _clienthidFormat = "<%=this.hidFormat.ClientID %>";
        _clientDDLFormatType = "<%=this.DDLFormatType.ClientID %>";
        _clientDDLFormatType2 = "<%=this.DDLFormatType2.ClientID %>";
        _clienthidIsTeacher = "<%=this.hidIsTeacher.ClientID %>";

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginReqHandler);
        prm.add_endRequest(EndReqHandler);

        Onload();
        //This function is used to disable "Save & Print LC" button.
        function Onload() {
            if (document.getElementById(_clientbtnReportID) != null)
                document.getElementById(_clientbtnReportID).disabled = true;
        }

        //This function is used to refresh parent screen.
        function refreshParent() {
            if (document.getElementById(_clienthidIsTeacher).value == "True")
                window.opener.location.reload(true);
            window.close();
            window.opener.focus();
        }

        function BeginReqHandler(sender, args) {

            if (document.getElementById(_clienttxtregNo) != null)
                document.getElementById(_clienttxtregNo).disabled = true;
            if (document.getElementById(_clientbtnSearchID) != null)
                document.getElementById(_clientbtnSearchID).disabled = true;

            ToggleReportControls(true);
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientbtnSearchID) {
                ClearUpdateMessage();
                if (trimAll(document.getElementById(_clienttxtregNo).value) == ''
                    || document.getElementById(_clientlblErrID).innerHTML != ""
                    ) {
                    ToggleReportControls(true);
                    return true;
                }
                else {
                    var postBackElement = sender._postBackSettings.sourceElement;
                    if (postBackElement.id == _clientbtnReportID || postBackElement.id == _clientbtnSave
                        || postBackElement.id == _clientbtnCancel || postBackElement.id == _clientbtnSearchID) {

                        document.getElementById(_clientlblErrID).innerHTML == "";
                        if (document.getElementById(_clientMainDataTable) != null && document.getElementById(_clientMainDataTable).style.display != "none" && ($get(_clienthidLcDetailId) && $get(_clienthidLcDetailId).value != "0")) {
                            ToggleReportControls(false);
                        }

                    }

                    return false;
                }
            }
            else if ((postBackElement.id.match(_clientlstVwStudentID) && ($get(_clienthidLcDetailId) && $get(_clienthidLcDetailId).value != "0")) || postBackElement.id == _clientbtnSave) {
                if (document.getElementById(_clientbtnSearchID) != null)
                    document.getElementById(_clientbtnSearchID).disabled = false;
                ToggleReportControls(false);
            }
        }

        $(document).ready(function () {
            $('#<%= btnReport2.ClientID %>').click(function () {

                $get(_clienthidFormat).value = $get(_clientDDLFormatType2).value;
            });
        });

        $(document).ready(function () {
            $('#<%= btnReport.ClientID %>').click(function () {

                $get(_clienthidFormat).value = $get(_clientDDLFormatType).value;
            });
        });

        function ClearUpdateMessage() {
            var lblUpdateMessage = $get(_clientlblUpdateMessage);
            if (lblUpdateMessage != null) {
                lblUpdateMessage.innerHTML = '';
                lblUpdateMessage.style.display = 'none';
            }
        }

        function ToggleReportControls(show) {
            var btnReport = $get(_clientbtnReportID);
            var ddlRpt = $get(_clientddlrpt);
            var btnReport2 = $get(_clientbtnReportID2);
            var ddlRpt2 = $get(_clientddlrpt2);

            if (btnReport != null)
                btnReport.disabled = show;
            if (ddlRpt != null)
                ddlRpt.disabled = show;
            if (btnReport2 != null)
                btnReport2.disabled = show;
            if (ddlRpt2 != null)
                ddlRpt2.disabled = show;
        }

        //This function is used to validate registration number.
        function validateReg(oSrc, args) {
            if (trimAll(document.getElementById(_clienttxtregNo).value) == '') {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        //This function is used to validate last school standard.
        function validateStandard(oSrc, args) {
            var txtStandard = document.getElementById(_clienttxtStandard).value;
            var bIsValid = true;
            if (!(parseInt(txtStandard) >= 1 && parseInt(txtStandard) <= 10)) {
                bIsValid = false;
                document.getElementById(_clientLastSchoolStdVal).errormessage = document.getElementById("<%=hidLastAttendedSchoolStandardShouldBeBetween.ClientID%>").value;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }
        //This function is used to disable buttons.
        function DisableButtons(ObjBtn, ValGroup) {
            if (ObjBtn == document.getElementById(_clientbtnSave)) {
                var isPageValid = true;

                if (typeof (Page_ClientValidate) == 'function')
                    isPageValid = Page_ClientValidate(ValGroup);
                if (isPageValid) {
                    document.getElementById(_clientbtnSave).disabled = true;
                    document.getElementById(_clientbtnCancel).disabled = true;
                }
            }
            else if (ObjBtn == document.getElementById(_clientbtnCancel)) {
                document.getElementById(_clientbtnSave).disabled = true;
                document.getElementById(_clientbtnCancel).disabled = true;
            }
        }
        function getCalDateStr(sId) {
            var dt = document.getElementById(sId).value;
            var sInputDate;
            if (window.navigator.appName == "Microsoft Internet Explorer") {
                sInputDate = new Date(dt.replace(/-/g, ' '));
            }
            else
                sInputDate = new Date(dt.replace(/-/g, '/'));
            return sInputDate;
        }
        //This function is used to validate leaving date.
        function validateDateOfLeaving(source, args) {
            var txtDOL = trimAll(document.getElementById(_clientDOL).value);
            var bIsValid = true;
            var serverDate = document.getElementById(_clientServerDate).value;
            if (txtDOL.trim() == "") {
                bIsValid = false;
                document.getElementById(_clientcstValDOL).errormessage = document.getElementById("<%=hidDateOfLeavingShouldBeSelected.ClientID%>").value;
            }
            if (txtDOL.trim() != "") {
                var oDOL, oCurrDate, oDOA;
                var dtDate1 = getCalDateStr(_clientLblDOA);
                var dtDate = new Date(dtDate1);
                var today = new Date(serverDate);
                var dtDOL = getCalDateStr(_clientDOL);
                //                if (today < dtDOL) {
                //                    bIsValid = false;
                //                    document.getElementById(_clientcstValDOL).errormessage = document.getElementById("<%=hidDateOfLeavingShouldNotBeFutureDate.ClientID%>").value;
                //                }
                if (dtDOL < dtDate) {
                    bIsValid = false;
                    document.getElementById(_clientcstValDOL).errormessage = document.getElementById("<%=hidDateOfLeavingShouldBeGreaterThanDateOfAdmission.ClientID%>").value;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        //This function is used to validate admission date.
        function validateAdmissionDate(source, args) {
            var txtDOA = trimAll(document.getElementById(_clientLblDOA).value);
            var bIsValid = true;
            var serverDate = document.getElementById(_clientServerDate).value;
            if (txtDOA.trim() == "") {
                bIsValid = false;
                document.getElementById(_clientcsttxtDateofAdmission).errormessage = document.getElementById("<%=hidDateOfAdmissionShouldBeSelected.ClientID%>").value;
            }
            if (txtDOA.trim() != "") {
                var dtDOL = getCalDateStr(_clientLblDOA);
                var today = new Date(serverDate);
                if (today < dtDOL) {
                    bIsValid = false;
                    document.getElementById(_clientcsttxtDateofAdmission).errormessage = document.getElementById("<%=hidDateOfAdmissionShouldNotBeFutureDate.ClientID%>").value;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function validateBirthDate(source, args) {
            var txtDOA = trimAll(document.getElementById(_clientlblDOB).value);
            var bIsValid = true;
            var serverDate = document.getElementById(_clientServerDate).value;
            if (txtDOA.trim() == "") {
                bIsValid = false;
                document.getElementById(_clientcstvalDOB).errormessage = document.getElementById("<%=hidBirthDateShouldBeSelected.ClientID%>").value;
            }
            if (txtDOA.trim() != "") {
                var dtDOL = getCalDateStr(_clientlblDOB);
                var today = new Date(serverDate);
                if (today < dtDOL) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalDOB).errormessage = document.getElementById("<%=hidBirthDateShouldNotBeFutureDate.ClientID%>").value;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        //This function is used to validate academic progress.
        function validateAcademicProgress(source, args) {
            var txtAProgress = document.getElementById(_clientAcademicProgress).value;
            var txtLastSchoolDetails = document.getElementById(_clienttxtLastSchoolDetails).value;
            var bIsValid = true;
            if (txtAProgress.trim() != "") {
                if (txtAProgress.length > 200) {
                    bIsValid = false;
                    document.getElementById(_clientAcademicProgressCustomValId).errormessage = document.getElementById("<%=hidProgressRemarkOfStudentShouldBeOfLengthLessThan.ClientID%>").value;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }
        function validateLastSchoolAddress(source, args) {
            var txtLastSchoolDetails = document.getElementById(_clienttxtLastSchoolDetails).value;
            var bIsValid = true;
            if (txtLastSchoolDetails.trim() != "") {
                if (txtLastSchoolDetails.length > 200) {
                    bIsValid = false;
                    document.getElementById(_clientcstValLastschoolAddressCustomValId).errormessage = document.getElementById("<%=hidLastSchoolAttendedAddressDetailsOfStudentShould.ClientID%>").value;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        //This function is used to validate reason of leaving.
        function validateReasonOfLeaving(source, args) {
            var txtReason1 = document.getElementById(_clientReason).value;
            var bIsValid = true;
            if (txtReason1.trim() != "") {
                if (txtReason1.length > 200) {
                    bIsValid = false;
                    document.getElementById(_clientReasonCustomValId).errormessage = document.getElementById("<%=hidReasonForLeavingSchoolShouldBeOfLength.ClientID%>").value;
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        //This function is used to validate remark.
        function validateRemarks(source, args) {
            var txtremarks = document.getElementById(_clientRemarks).value;
            var bIsValid = true;
            if (txtremarks.trim() != "") {
                if (txtremarks.length > 200) {
                    bIsValid = false;
                    document.getElementById(_clientRemarksCustomValId).errormessage = document.getElementById("<%=hidRemarksShouldBeOfLengthLessThan.ClientID%>").value;
                }
            }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function fnover(varname, doc) {
            var objTXT = document.getElementById(varname)
            hidClassName = objTXT.className;
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
        }

        function fnout(varname, doc) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = '';
            objTXT.style.borderColor = '';
            objTXT.className = hidClassName;
            objTXT.style.backgroundImage = '';
        }

        //This function is used to set admission year.
        function SetAdmissionYear() {
            document.getElementById(_clienttxtMNameId).value = document.getElementById(_clienttxtMotherNameId).value;
            return false;
        }

        function ValidateControls() {
            var bResult = false;
            if (typeof (Page_ClientValidate) == "function") {
                bResult = Page_ClientValidate("Save");
            }
            return bResult;
        }

        function CrearMessage() {
            $get("<%=this.lblUpdateMessage.ClientID %>").innerHTML = "";
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });
        function AutoSearch() {
            var SchoolId = "<%=miSchoolId %>";
            _clienttxtRegNumber = '#<%=txtRegNo.ClientID%>';
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventforOnlyLeftStudent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtRegNo.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }
    </script>
</asp:Content>
