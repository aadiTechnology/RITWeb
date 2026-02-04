<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="Shifts.aspx.cs" Inherits="RITeSchool_Payroll_Shifts" EnableViewState="false"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
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
                                    <!--Insert Data Here-->
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                               <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label></asp:Panel>
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="ClsTextNormal" align="center">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </table>
                        <!--Shift Configuration starts here-->
                        <table id="tblShiftname" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel">Shift Name :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtShiftName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqShiftName" runat="server" ControlToValidate="txtShiftName"
                                        Display="None" ErrorMessage="Shift Name should not be blank."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <table id="tblShiftStartTime" runat="server" border="0" cellpadding="1" cellspacing="2"
                        style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel"> Start Time :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtShiftStartTime" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqShiftStartTime" runat="server" ControlToValidate="txtShiftStartTime"
                                        Display="None" ErrorMessage="Shift start time should not be blank."></asp:RequiredFieldValidator>
                                     <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid start time in 24 hr format."
                                            ClientValidationFunction="IsValidStartTime">
                                     </asp:CustomValidator>
                                     <asp:CustomValidator ID="cstInvalidShiftStartTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" 
                                            ClientValidationFunction="IsValidStartDayTime" ControlToValidate="txtShiftStartTime">
                                     </asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                        <table id="tblShiftEndTime" runat="server" border="0" cellpadding="1" cellspacing="2"
                        style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel"> End Time :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtShiftEndTime" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqShiftEndTime" runat="server" ControlToValidate="txtShiftEndTime"
                                        Display="None" ErrorMessage="Shift end time should not be blank."></asp:RequiredFieldValidator>
                                     <asp:CustomValidator ID="cstInvalidEndTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid End time in 24 hr format."
                                            ClientValidationFunction="IsValidEndTime">
                                     </asp:CustomValidator>
                                     <asp:CustomValidator ID="cstInvalidStartEndTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" 
                                            ClientValidationFunction="IsValidStartEndTime" ControlToValidate="txtShiftEndTime">
                                     </asp:CustomValidator>
                                     <asp:CustomValidator ID="cstInvalidShiftEndTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" 
                                            ClientValidationFunction="IsValidEndDayTime" ControlToValidate="txtShiftEndTime">
                                     </asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                        <table id="tblHalfDayTime" runat="server" border="0" cellpadding="1" cellspacing="2"
                        style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel"> Half Day Time :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtHalfDayTime" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqHalfDayTime" runat="server" ControlToValidate="txtHalfDayTime"
                                        Display="None" ErrorMessage="Half day time should not be blank."></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cstInvalidHalfDayTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid Half day time in 24 hr format."
                                            ClientValidationFunction="IsValidHalfDayTime">
                                     </asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                        <table id="tblLateMarkTime" runat="server" border="0" cellpadding="1" cellspacing="2"
                        style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel"> Late Mark Time :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtLateMarkTime" runat="server" MaxLength="5" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqLateMarkTime" runat="server" ControlToValidate="txtLateMarkTime"
                                        Display="None" ErrorMessage="Late mark time should not be blank."></asp:RequiredFieldValidator>
                                     <asp:CustomValidator ID="cstInvalidLateMarkTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid Late mark time in 24 hr format."
                                            ClientValidationFunction="IsValidLateMarkTime">
                                     </asp:CustomValidator>
                                      <asp:CustomValidator ID="clsInvalidLateMarkHalfDayTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" 
                                            ClientValidationFunction="IsValidLateMarkHalfDayTime" ControlToValidate="txtLateMarkTime">
                                     </asp:CustomValidator>
                                      <asp:CustomValidator ID="cstInvalidLateMarkAndStartTime" CssClass="LblErrorMsg" runat="server"
                                            SetFocusOnError="True" Display="None" 
                                            ClientValidationFunction="IsValidStartTimeAndLateMarkTime" ControlToValidate="txtLateMarkTime">
                                     </asp:CustomValidator>
                                </td>
                            </tr>
                             <tr>
                                <td style="width: 15%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel"> Is Default Shift?: </span>
                                </td>
                                <td align="left"  style="width: 31%; margin-left: 40px;">
                                    <asp:CheckBox ID="chkIsDefault" runat="server" Text="" CssClass="ClsLabel" />
                                </td>
                            </tr>
                        </table>
                        <table id="tblSaveShiftdetails" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 41%">
                                    &nbsp
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="true" disable-page="true" onclick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" 
                                        onclick="btnCancel_Click"  />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpnlListView" runat="server">
                    <ContentTemplate>
                        <table>                       
                            <tr id="tr1" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwConfigureShift">
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
                        </table>
                        <table id="tblShiftList" align="center" width="46%">
                            <tr align="center" style="width: 100%">
                                <td align="center" style="width: 800">
                                    <asp:ListView ID="lstvwConfigureShift" runat="server" DataKeyNames="ShiftId, ShiftName, IsDefault"
                                        ondatabound="lstvwConfigureShift_DataBound" OnSorting="lstvwConfigureShift_Sorting"
                                        onitemcommand="lstvwConfigureShift_ItemCommand" onitemdatabound="lstvwConfigureShift_ItemDataBound" DataSourceID ="ObjDSConfigureShift"
                                         >
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblShiftInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" width="30%" style="padding-left: 9px;">
                                                        <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                            CausesValidation="false" ForeColor="Black"> Shift Name </asp:LinkButton>
                                                    </th>
                                                     <th align="left" style="padding-left: 9px;">
                                                         Shift Start Time
                                                    </th>
                                                    <th align="left" style="padding-left: 9px;">
                                                         Shift End Time
                                                    </th>
                                                    <th align="left" style="padding-left: 9px;">
                                                        Half Day Time
                                                    </th>
                                                    <th align="left" style="padding-left: 9px;">
                                                        Late Mark Time
                                                    </th>
                                                    <th align="center">
                                                        Edit
                                                    </th>
                                                    <th align="center">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="7">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfigureShift"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" >
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
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" ShiftName") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblShiftStartTime" runat="server" Text='<%# Eval(" ShiftStartTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblShiftEndTime" runat="server" Text='<%# Eval(" ShiftEndTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblHalfDayTime" runat="server" Text='<%# Eval(" HalfDayTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblLateMarkTime" runat="server" Text='<%# Eval(" LateMarkTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESHIFT"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVESHIFT"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" align="left">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" ShiftName") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblShiftStartTime" runat="server" Text='<%# Eval(" ShiftStartTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblShiftEndTime" runat="server" Text='<%# Eval(" ShiftEndTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblHalfDayTime" runat="server" Text='<%# Eval(" HalfDayTime") %>'></asp:Label>
                                                </td>
                                                 <td align="left" class="paddingL">
                                                    <asp:Label ID="lblLateMarkTime" runat="server" Text='<%# Eval(" LateMarkTime") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESHIFT" 
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="REMOVESHIFT" CausesValidation="false"
                                                        runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
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
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource TypeName="BusinessLogic.ShiftDetailsBL" EnablePaging="True"
                            ID="ObjDSConfigureShift" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                            EnableCaching="False" SelectCountMethod="CountTotalShiftRecords">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                    Type="int32" />
                                <asp:Parameter Name="sortExpression" Type="String" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidShiftId" runat="server" />
                        <asp:HiddenField ID="hidShiftName" runat="server" />
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
    <table>
            <tr>
                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                         <span class="LblNrmlB">Note :</span>
                </td>
                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                    <span class="LblSmlV"> If any members associated with shift which is not the default shift, and if you want to delete this shift then the members which are associated with this shift are updated and assigned to the default shift.</span>
                </td>
             </tr>
    </table>

<script type="text/javascript" language="javascript">
    _clientIsDefault = "<%=this.chkIsDefault.ClientID %>"
    _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
    _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
    _ClienttxtShiftStartTime = "<%=this.txtShiftStartTime.ClientID %>"
    _ClienttxtShiftEndTime = "<%=this.txtShiftEndTime.ClientID %>"
    _ClienttxtHalfDayTime = "<%=this.txtHalfDayTime.ClientID %>"
    _ClienttxtLateMarkTime = "<%=this.txtLateMarkTime.ClientID %>"

</script>
<script type="text/javascript" src="../Scripts/Payroll/ShiftDetails.js"></script>
</asp:Content>