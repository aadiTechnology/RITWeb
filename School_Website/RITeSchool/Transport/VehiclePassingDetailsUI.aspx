<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VehiclePassingDetailsUI.aspx.cs" Inherits="VehiclePassingDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="right">
                <span class="ClsMdtStar">* Mandatory fields.</span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSum" runat="server" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Vehical No. should be selected."
                            Display="None" ControlToValidate="cmbVehicleNos" ValueToCompare="0" Operator="NotEqual"
                            Type="Integer"></asp:CompareValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Passing Date should not be blank."
                            Display="None" ControlToValidate="txtDate"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" ClientValidationFunction="ValidatePassingDate"
                            Display="None"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Expiry Date should not be blank."
                            Display="None" ControlToValidate="txtExpiryDate"></asp:RequiredFieldValidator>
                         <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateExpiryDate"
                            Display="None"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Value of 'Notify before days()' should not be blank."
                            Display="None" ControlToValidate="txtNotificationDays"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
                            Display="None" ControlToValidate="txtNote" ErrorMessage="Length of Note should not exceed 500 characters."
                            CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFileType"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFileSize"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" ClientValidationFunction="ValidateNextDate"
                                    Display="None"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPassingDetails" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td id="tdMessage" runat="server" colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 200px;" class="clsBorderLight">
                                    <span class="clsLabel">Vehicle No. : </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbVehicleNos" runat="server" CssClass="LrgCombo" onchange="Test()">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="clsBorderLight">
                                    <span class="clsLabel">Passing Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDate" CssClass="MidTxtBox" runat="server" onchange="SetExpiryDate(this)" />
                                    <rjs:PopCalendar ID="calPassingDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Passing date should not be blank."
                                        AutoPostBack="False" To-Today="true" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="clsBorderLight">
                                    <span class="clsLabel">Expiry Date : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtExpiryDate" CssClass="MidTxtBox" runat="server" />
                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtExpiryDate" Format="dd MMM yyyy"
                                        Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Passing date should not be blank."
                                        AutoPostBack="False" From-Today="true" />
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="clsBorderLight">
                                    <span class="clsLabel">Notify before day(s) : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNotificationDays" runat="server" CssClass="SmlTxtBox" MaxLength="2"
                                        Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                        onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="clsBorderLight">
                                    <span class="clsLabel">Note : </span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNote" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine"
                                        Height="100px" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="clsBorderLight">
                                    <span class="clsLabel">Document Image(s) : </span>
                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="flFile" runat="server" CssClass="LrgTxtBox" accept=".BMP,.JPG,.JPEG,.PNG"
                                        multiple="true" />
                                    <asp:ImageButton ID="imgView" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                        CausesValidation="false" Visible="false" />
                                </td>
                            </tr>
                            <tr>                              
                                <td align="left" colspan="2">
                                    <span class="LblSmlGray">(Supports files of types - .BMP, .JPG, .JPEG, .PNG with total
                                        size upto 10 MB.)</span>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPassingDetails" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" OnClientClick="if(!ResetLabel()) return false;" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                            OnClick="btnCancel_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPassingDetails" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr style="width: 80%" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="left" class="clsBorderLight">
                            <span class="ClsLabel">Vehicle No. </span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" CausesValidation="false"
                                OnClick="btnShow_Click" />
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
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
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
                            <tr runat="server" id="trTotalRec" align="center">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwPassingDetails">
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
                                    <asp:ListView ID="lstvwPassingDetails" runat="server" DataKeyNames="Id" OnItemDataBound="lstvwPassingDetails_ItemDataBound"
                                        OnDataBound="lstvwPassingDetails_DataBound" OnItemCommand="lstvwPassingDetails_ItemCommand"
                                        OnSorting="lstvwPassingDetails_Sorting">
                                        <LayoutTemplate>
                                            <table style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder" width="100%">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="clsLabelgrd">
                                                        <asp:LinkButton ID="lnkVehicleNo" runat="server" CommandName="Sort" CommandArgument="VehicleNumber"
                                                            CausesValidation="false" ForeColor="Black"> Vehicle No. </asp:LinkButton>
                                                    </th>
                                                    <th align="center" class="clsLabelgrd" width="150px">
                                                        <asp:LinkButton ID="lnkPassingDate" runat="server" CommandName="Sort" CommandArgument="PassingDate"
                                                            CausesValidation="false" ForeColor="Black" Text="Passing Date"></asp:LinkButton>
                                                    </th>
                                                    <th align="center" width="150px" class="clsLabelgrd">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="ExpiryDate"
                                                            CausesValidation="false" ForeColor="Black" Text="Expiry Date"></asp:LinkButton>
                                                    </th>
                                                    <th align="right" class="clsLabelgrd" width="150px">
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="NotificationDays"
                                                            CausesValidation="false" ForeColor="Black" Text="Notification Days"></asp:LinkButton>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <span>Image(s)</span>
                                                    </th>
                                                    <th width="50px" align="center" class="clsLabelgrd">
                                                        <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="50px" class="clsLabelgrd">
                                                        <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="7">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwPassingDetails"
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
                                                <td align="left">
                                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPassingDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblExpiryDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblNotificationDays" runat="server" CssClass="ClsLabelL" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("NotificationDays") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgImage" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                        CausesValidation="false" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                <td align="left">
                                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="ClsLabel" Text='<%#Eval("VehicleNumber") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblPassingDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblExpiryDate" runat="server" CssClass="ClsLabel" Style="float: inherit"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblNotificationDays" runat="server" CssClass="ClsLabelL" Style="float: inherit;
                                                        padding-right: 5px;" Text='<%#Eval("NotificationDays") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgImage" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                        CausesValidation="false" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.VehicleDetailsBL" EnablePaging="True"
                                        ID="objdsPassingDetails" runat="server" SelectMethod="GetAllVehiclePassingDetails"
                                        SortParameterName="asSortExpression" SelectCountMethod="GetVehiclePassingDetailsCount"
                                        EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                            <asp:ControlParameter Name="abShowOldRecord" ControlID="chkShowOldRecord" PropertyName="Checked"
                                                                Type="Boolean" />
                                            <asp:ControlParameter ControlID="hidSortExpression" Name="asSortExpression" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter ControlID="hidSortDirection" Name="asSortDirection" Type="String"
                                                PropertyName="Value" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwPassingDetails" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" Visible="false" CausesValidation="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidPassingDetailsId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidPassingPeriod" runat="server" Value="0" />
                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                        <asp:HiddenField ID="hidDates" runat="server" Value="" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwPassingDetails" EventName="ItemCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function ValidateFileType(oSrc, args) {
            var isFound = false
            var files = $('[id$=flFile]')[0].value;
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
            var obj = document.getElementById('<%=flFile.ClientID %>')
            var fileSize = GetFileSize(obj)

            if (fileSize >= 10485760) {
                oSrc.errormessage = "Document image's total file size should be less than 10 MB."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }


        function ValidatePassingDate(oSrc, args) {
            var dt = $('[id$=txtDate]').val()
            if ($('[id$=hidPassingDetailsId]').val() == "0" && dt != '') {
                var dates = $('[id$=hidDates]').val()
                var data = JSON.parse(dates)
                var vehicleId = $('[id$=cmbVehicleNos]').val()
                                
                var passingDate;
                if (document.all)
                    passingDate = new Date(dt.replace('-', ' '));
                else
                    passingDate = new Date(convertdate(dt));

                for (k in data) {

                    if (data[k].VehicleId == vehicleId) {
                        var psDate = new Date(data[k].PassingDate.replace('-', ' '));

                        if (passingDate <= psDate) {
                            oSrc.errormessage = "Passing Date should be greater than active record's passing date of same vehicle."
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateExpiryDate(oSrc, args) {
            var dt = $('[id$=txtExpiryDate]').val()
            if ($('[id$=hidPassingDetailsId]').val() == "0" && dt != '') {
                var dates = $('[id$=hidDates]').val()
                var data = JSON.parse(dates)
                var vehicleId = $('[id$=cmbVehicleNos]').val()
                               
                var expiryDate;
                if (document.all)
                    expiryDate = new Date(dt.replace('-', ' '));
                else
                    expiryDate = new Date(convertdate(dt));

                for (k in data) {

                    if (data[k].VehicleId == vehicleId) {
                        var expDate = new Date(data[k].ExpiryDate.replace('-', ' '));

                        if (expiryDate <= expDate) {
                            oSrc.errormessage = "Expiry Date should be greater than active record's expiry date of same vehicle."
                            args.IsValid = false
                            return true
                        }
                    }
                }
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

        function SetExpiryDate(obj) {

            var dt = $(obj).val()

            var passingDate;
            if (document.all)
                passingDate = new Date(dt.replace('-', ' '));
            else
                passingDate = new Date(convertdate(dt));

            var passingPeriodInMonth = parseInt($('[id$=hidPassingPeriod]').val())
            var finalDate = new Date(passingDate.setMonth(passingDate.getMonth() + passingPeriodInMonth))
            var day = finalDate.getDate();

            if (("" + day).length == 1)
                day = '0' + day

            var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            $('[id$=txtExpiryDate]').val(day + '-' + month[finalDate.getMonth()] + '-' + finalDate.getFullYear())
        }


        function OpenImagePopup(index) {
            var query = $('[id$=ctrl' + index + '_hidQueryString]').val()
            window.open("TransportOptionImagesPopup.aspx?" + query, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=600')
        }

        function OpenPopup() {
            var query = $('[id$=hidQueryString]').val()
            window.open("TransportOptionImagesPopup.aspx?" + query, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=600')
        }

        function Test() {
            //alert('test')
        }

        function ResetLabel() {
            $('[id$=lblMessage]').html('')
            
            if ($('[id$=hidPassingDetailsId]').val() == "0") {
                var validationResult = true;
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");

                if (validationResult) {
                    var dates = $('[id$=hidDates]').val()
                    var data = JSON.parse(dates)
                    var vehicleId = $('[id$=cmbVehicleNos]').val()

                    var dt = $('[id$=txtDate]').val()
                    var passingDate;
                    if (document.all)
                        passingDate = new Date(dt.replace('-', ' '));
                    else
                        passingDate = new Date(convertdate(dt));

                    for (k in data) {

                        if (data[k].VehicleId == vehicleId) {
                            var expDate = new Date(data[k].ExpiryDate.replace('-', ' '));

                            if (passingDate < expDate)
                                return confirm('Expiry date of currently active record of selected vehicle is ' + dt + ' and you are adding earlier Passing Date. Do you want to continue by marking this new record as Active?')
                        }
                    }
                }
            }
            return true;
        }

        function ValidateNextDate(oSrc, args) {
            var dt = $('[id$=txtDate]').val()
            var nextdt = $('[id$=txtExpiryDate]').val()

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
                oSrc.errormessage = "Expiry Date should be greater than Passing Date of same vehicle."
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
