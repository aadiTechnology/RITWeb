<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VehicleServicingDetailsUI.aspx.cs" Inherits="VehicleServicingDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trMandetory" runat="server">
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" />
                            <asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div runat="server" id="divErr">
                                <asp:RequiredFieldValidator ID="reqVehicleNumber" runat="server" ErrorMessage="Vehicle Number should be selected."
                                    ControlToValidate="cmbVehical" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="reqNotificationDays" runat="server" ErrorMessage="Value of 'Notify before day(s)' should not be blank."
                                    ControlToValidate="txtNotificationDays" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="reqServicingDate" runat="server" ErrorMessage="Servicing Date should not be blank."
                                    ControlToValidate="txtServicingDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="reqNextServicingDate" runat="server" ErrorMessage="Next Servicing Date should not be blank."
                                    ControlToValidate="txtNextServicingDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFileType"
                                    Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFileSize"
                                    Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" ClientValidationFunction="ValidateServicingDate"
                                    Display="None"></asp:CustomValidator>
                                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateNextServicingDate"
                                    Display="None"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
                                    Display="None" ControlToValidate="txtNote" ErrorMessage="Length of Note should not exceed 500 characters."
                                    CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>         
                                 <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" ClientValidationFunction="ValidateNextDate"
                                    Display="None"></asp:CustomValidator>                       
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center" style="text-align: center; margin: 0px auto;">
                <td align="center" style="text-align: center; margin: 0px auto;">
                    <table width="100%" align="center" style="text-align: center; margin: 0px auto;">
                        <tr id="trFilters" runat="server" align="center" style="text-align: center; margin: 0px auto;">
                            <td width="100%" align="center" style="text-align: center; margin: 0px auto;">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" cellpadding="1" cellspacing="2" width="40%" style="text-align: center;
                                            margin: 0px auto;">
                                            <tr align="center" style="text-align: center;">
                                                <td valign="middle" class="ClsBorderlight" align="center" style="margin: 0px auto;
                                                    text-align: center; width: 235px;">
                                                    <span class="ClsLabel">Vehicle Number :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbVehical" runat="server" CssClass="LrgCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Servicing Date :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtServicingDate" CssClass="SmlTxtBox" runat="server" onchange="SetExpiryDate(this)" />
                                                    <rjs:PopCalendar ID="calServicingDate" runat="server" Control="txtServicingDate" To-Today="true"
                                                        Format="dd MMM yyyy" Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Next Servicing Date :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNextServicingDate" CssClass="SmlTxtBox" runat="server" />
                                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtNextServicingDate"
                                                        Format="dd MMM yyyy" Culture="en" ShowWeekend="True" ShowErrorMessage="false" From-Today="true"
                                                        InvalidDateMessage="Expiry date should not be blank." AutoPostBack="False" />
                                                    <span class="ClsMdtStar">* </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Notify before day(s) :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNotificationDays" runat="server" CssClass="SmlTxtBox" MaxLength="2"
                                                    Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Note :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="LrgTxtBox" MaxLength="500" TextMode="MultiLine"
                                                        Height="50px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Document Images :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:FileUpload ID="fuDocumentPhoto" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG"
                                                        multiple="true" />
                                                    <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" ToolTip="View"
                                                        ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                </td>
                                            </tr>
                                             <tr>                                               
                                                <td align="left" colspan="2">
                                                    <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG with total
                                                        size upto 10 MB.)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                        OnClick="btnSave_Click" OnClientClick="if(!ResetLabel()) return false;" />
                                                    <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                                        CausesValidation="False" OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr style="color: #C0C0C0" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;" align="center">
                                <table align="center" width="35%" style="text-align: center; margin: 0px auto;">
                                    <tr align="center" style="text-align: center;">
                                        <td class="ClsBorderlight" align="center" style="width: 100px;">
                                            <asp:Label ID="lblNameSearch" runat="server" CssClass="ClsLabel" Text="Vehicle No."></asp:Label>
                                            <span class="ClsLabel colonPadding">:
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtSearch" CssClass="LrgTxtBox" runat="server" TabIndex="11"></asp:TextBox>
                                            <asp:Button ID="btnSearch" CssClass="ClsBtn" runat="server" Text="Search" TabIndex="12"
                                                CausesValidation="false" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                    <tr align="center" style="text-align: center;">
                                        <td class="ClsBorderlight" align="center" style="width: 150px;">
                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Show Old Records?"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="2">
                                            <asp:CheckBox ID="chkShowOldRecord" runat="server" AutoPostBack="True" 
                                                oncheckedchanged="chkShowOldRecord_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="80%">
                                            <tr>
                                                <td align="left">
                                                    <table id="LegendTable" runat="server">
                                                        <tr>
                                                            <td align="left" width="55px" valign="middle">
                                                                <span class="ClsLblLgnd">Legend : </span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" style="padding-right: 5px;padding-left:5px;"
                                                                    TabIndex="3" ForeColor="Maroon" Font-Bold="true" ReadOnly="True" Text="Active Notifications"></asp:Label>
                                                            </td>  
                                                            <td align="left">
                                                                <asp:Label ID="Label2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" style="padding-right: 5px;padding-left:5px;"
                                                                    TabIndex="3" ForeColor="Navy" Font-Bold="true" ReadOnly="True" Text="Old Records"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trItemCount" runat="server">
                                                <td align="center" style="width: 100%;">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwVehicleServicingDetails"
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
                                            <tr>
                                                <td>
                                                    <asp:ListView ID="lstvwVehicleServicingDetails" runat="server" DataKeyNames="VehicalId,VehicleServicingId,IsFileExists,IsLocked"
                                                        OnDataBound="lstvwVehicleServicingDetails_DataBound" OnItemCommand="lstvwVehicleServicingDetails_ItemCommand"
                                                        OnItemDataBound="lstvwVehicleServicingDetails_ItemDataBound" OnSorting="lstvwVehicleServicingDetails_Sorting">
                                                        <LayoutTemplate>
                                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                    <th align="left" class="paddingL" style="font-size: 10pt;">
                                                                        <span>Vehicle Number</span>
                                                                    </th>
                                                                    <th align="center" class="paddingL" style="width: 150px; font-size: 10pt;">
                                                                        <asp:LinkButton ID="lnkServicingDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            CommandArgument="Date" CommandName="SortRow">Servicing Date</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" class="paddingL" style="width: 180px; font-size: 10pt;">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" ForeColor="Black"
                                                                            CommandArgument="Date" CommandName="SortRow">Next Servicing Date</asp:LinkButton>
                                                                    </th>
                                                                    <th align="center" class="clsLabelgrd" width="140px" style="font-size: 10pt;">
                                                                        <span>Notification Days</span>
                                                                    </th>
                                                                    <th width="100px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                                        <asp:Label ID="lblView" runat="server" Text="View Image(s)" ToolTip="<%$ Resources:LocalizedResources, View%>"> </asp:Label>
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
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwVehicleServicingDetails">
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
                                                                <td align="left">
                                                                    <asp:Label ID="lblVehicleNumber" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("VehicalNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblServicingDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("ServicingDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblNextServicingDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("NextServicingDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblNotificationDays" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("NotificationDays") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdView" runat="server" viewstatemode="Enabled">
                                                                    <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                                        CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                                        ToolTip="View" />
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
                                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblVehicleNumber" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("VehicalNumber") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblServicingDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("ServicingDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblNextServicingDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("NextServicingDate") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblNotificationDays" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                        Text='<%#Eval("NotificationDays") %>'></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdView" runat="server" viewstatemode="Enabled">
                                                                    <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                                        CommandName="VIEW" CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                                        ToolTip="View" />
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
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        No record found.
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidNotificationDays" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidServicingId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidDates" runat="server" Value="" />
                                                    <asp:ObjectDataSource EnablePaging="true" TypeName="BusinessLogic.TransportBL.VehicleServicingDetailsBL"
                                                        ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                                        EnableCaching="false" SortParameterName="asSortExpression">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:ControlParameter Name="abShowOldRecord" ControlID="chkShowOldRecord" PropertyName="Checked"
                                                                Type="Boolean" />
                                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value"
                                                                Type="String" />
                                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value"
                                                                Type="String" />
                                                            <asp:ControlParameter Name="asFilter" ControlID="txtSearch" PropertyName="Text" Type="String" />
                                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lstvwVehicleServicingDetails" EventName="ItemCommand" />
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
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
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        _clientfuDocumentPhoto = "<%=this.fuDocumentPhoto.ClientID %>"
        _clienttxtServicingDate = "<%=this.txtServicingDate.ClientID %>"
        _clienthidNotificationDays = "<%=this.hidNotificationDays %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function SetExpiryDate(obj) {
            var dt = $(obj).val()

            var PUCDate;
            if (document.all)
                PUCDate = new Date(dt.replace('-', ' '));
            else
                PUCDate = new Date(convertdate(dt));

            var PUCPeriodInMonth = parseInt($('[id$=hidNotificationDays]').val())
            var finalDate = new Date(PUCDate.setMonth(PUCDate.getMonth() + PUCPeriodInMonth))
            var day = finalDate.getDate();

            if (("" + day).length == 1)
                day = '0' + day

            var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            $('[id$=txtNextServicingDate]').val(day + '-' + month[finalDate.getMonth()] + '-' + finalDate.getFullYear())
        }

        function ValidateFileType(oSrc, args) {
            var isFound = false
            var files = $('[id$=fuDocumentPhoto]')[0].value;
            if (files.trim() != '') {
                var fileList = files.split(',')
                for (var k = 0; k < fileList.length; k++) {
                    var file = fileList[k].trim()

                    var extension = file.substr(file.lastIndexOf('.')).toUpperCase()
                    if (extension != ".BMP" && extension != ".JPG" && extension != ".JPEG" && extension != ".PNG") {
                        //  numbers = numbers + ',' + num
                        isFound = true
                        break;
                    }
                }
            }

            if (isFound) {
                oSrc.errormessage = "Document image type should be in only BMP, .JPG, .JPEG and .PNG format.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFileSize(oSrc, args) {
            var obj = document.getElementById('<%=fuDocumentPhoto.ClientID %>')
            var fileSize = GetFileSize(obj)

            if (fileSize >= 10485760) {
                oSrc.errormessage = "Document image's total file size should be less than 10 MB."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

        function GetFileSize(obj) {
            var size = 0;
            for (var k = 0; k < obj.files.length; k++) {
                size += obj.files[k].size;
            }
            return size;
        }

        function OpenPhotoPopup(QueryString) {
            window.open('TransportOptionImagesPopup.aspx?' + QueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=600')
        }

        function ResetLabel() {
            $('[id$=lblMessage]').html('')

            if ($('[id$=hidServicingId]').val() == "0") {
                var validationResult = true;
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");

                if (validationResult) {
                    var dates = $('[id$=hidDates]').val()
                    var data = JSON.parse(dates)
                    var vehicleId = $('[id$=cmbVehical]').val()

                    var dt = $('[id$=txtServicingDate]').val()
                    var passingDate;
                    if (document.all)
                        passingDate = new Date(dt.replace('-', ' '));
                    else
                        passingDate = new Date(convertdate(dt));

                    for (k in data) {

                        if (data[k].VehicleId == vehicleId) {
                            var expDate = new Date(data[k].ExpiryDate.replace('-', ' '));

                            if (passingDate < expDate)
                                return confirm('Next Servicing date of currently active record of selected vehicle is ' + dt + ' and you are adding earlier Passing Date. Do you want to continue by marking this new record as Active?')
                        }
                    }
                }
            }
            return true;
        }

        function ValidateServicingDate(oSrc, args) {            
            var dt = $('[id$=txtServicingDate]').val()
            if ($('[id$=hidServicingId]').val() == "0" && dt != '') {
                var dates = $('[id$=hidDates]').val()
                var data = JSON.parse(dates)
                var vehicleId = $('[id$=cmbVehical]').val()

                var passingDate;
                if (document.all)
                    passingDate = new Date(dt.replace('-', ' '));
                else
                    passingDate = new Date(convertdate(dt));

                for (k in data) {

                    if (data[k].VehicleId == vehicleId) {
                        var psDate = new Date(data[k].PassingDate.replace('-', ' '));

                        if (passingDate <= psDate) {
                            oSrc.errormessage = "Servicing Date should be greater than active record's servicing date of same vehicle."
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateNextServicingDate(oSrc, args) {
            var dt = $('[id$=txtNextServicingDate]').val()
            if ($('[id$=hidServicingId]').val() == "0" && dt != '') {
                var dates = $('[id$=hidDates]').val()
                var data = JSON.parse(dates)
                var vehicleId = $('[id$=cmbVehical]').val()
               
                var expiryDate;
                if (document.all)
                    expiryDate = new Date(dt.replace('-', ' '));
                else
                    expiryDate = new Date(convertdate(dt));
              
                for (k in data) {

                    if (data[k].VehicleId == vehicleId) {
                        var expDate = new Date(data[k].ExpiryDate.replace('-', ' '));

                        if (expiryDate <= expDate) {
                            oSrc.errormessage = "Next Servicing Date should be greater than active record's Next Servicing date of same vehicle."
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }

            args.IsValid = true;
            return false;
        }


        function ValidateNextDate(oSrc, args) {
            var dt = $('[id$=txtServicingDate]').val()
            var nextdt = $('[id$=txtNextServicingDate]').val()

            var expiryDate;
            if (document.all)
                expiryDate = new Date(nextdt.replace('-', ' '));
            else
                expiryDate = new Date(convertdate(nextdt));

            var currentDate;
            if (document.all)
                currentDate = new Date(dt.replace('-', ' '));
            else
                currentDate = new Date(convertdate(dt));

            if (expiryDate <= currentDate) {
                oSrc.errormessage = "Next Servicing Date should be greater than Servicing Date of same vehicle."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
