<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ODDetailsPopup.aspx.cs" Inherits="ODLeaveDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%">
        <tr>
            <td valign="top">
                <table width="100%">
                    <tr>
                        <td align="left" style="height: 20px; width: 99%;" class="ClsGrayMainTitle">
                            <span style="font-weight: bold">On Duty (O.D) Details.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" style="padding-right: 30px" valign="bottom">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                    Text="Mandatory Fields"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" />
                        <asp:RequiredFieldValidator ID="reqCmbStaffGroup" runat="server" Display="None" ErrorMessage="Staff group should be selected."
                            ControlToValidate="cmbStaffGroup" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqStaffMember" runat="server" Display="None" ErrorMessage="Name should be selected."
                            ControlToValidate="cmbUserName" InitialValue="0"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqStartTime" runat="server" Display="None" ErrorMessage="Start Time should not be blank."
                            ControlToValidate="txtStartTime"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstStartTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateStartTime"
                            Display="None"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqEndTime" runat="server" Display="None" ErrorMessage="End Time should not be blank."
                            ControlToValidate="txtEndTime"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstEndTime" runat="server" ErrorMessage="" ClientValidationFunction="ValidateEndTime"
                            Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="CompareDates"
                            Display="None"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="reqLocation" runat="server" Display="None" ErrorMessage="Location should not be blank."
                            ControlToValidate="txtLocation"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstDamageDescriptionValidator" runat="server" Display="None"
                            ClientValidationFunction="ValidateDescription"></asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td id="tdMessage" runat="server" align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" Style="text-align: center;" runat="server" Text="" EnableViewState="false"
                            CssClass="LblNormal"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="left">
            <td id="tdErrorMsg" runat="server" align="left">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorMsg" Style="text-align: left; float: left;" ForeColor="Red"
                            runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table align="center" width="95%">
                    <tr>
                        <td align="center" class="ClsHilightBGB">
                            <span style="font-weight: bold">Please make sure you have already assign O.D as leave
                                on Leave Assignment screen.</span>
       
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
         	<td runat="server" id="td2" align="center">
                   <table align="center" width="38%" class="ClsBorderlight">
                        <tr>
                           <td style="width:100px;">
                              <span class="ClsLabel" style="font-weight: bold">Name :</span>
                           </td>                                        
                           <td align="left">
                           <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" Width="200px" autocomplete="off"></asp:TextBox>
                           </td>                                        
                           <td align="left">
                           <asp:Button ID="btnSearchOD" runat="server" Text="Search" CssClass="ClsBtn"
                              CausesValidation="false" onclick="btnSearchOD_Click" />                                    
                           </td>
                           </tr>
                    </table>
              </td>
           </tr>
           <tr>
               <td align="center">                           
                  <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                     Text="No record found." Visible="false" EnableViewState="False" Width="99%"></asp:Label>                               
               </td>
           </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td class="ClsBorderlight" style="width: 110px;">
                                    <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel" Text="Staff Group"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbStaffGroup" runat="server" CssClass="LrgCombo" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged"
                                            AutoPostBack="true" TabIndex="1" Width="218px">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearchOD" EventName="Click" />                                        
                                    </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                                <td class="ClsBorderlight" style="width: 110px;">
                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cmbUserName" runat="server" CssClass="LrgCombo" TabIndex="2"
                                            Width="219px" OnSelectedIndexChanged="cmbUserName_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">* </span>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbUserName" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearchOD" EventName="Click" />                                        
                                    </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="Start Date / Time"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtStartDate" CssClass="SmlTxtBox" runat="server" 
                                        ReadOnly = "true" TabIndex="3"></asp:TextBox>
                                    <rjs:PopCalendar ID="cal_FormOpenDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" />  
                                    <asp:TextBox ID="txtStartTime" CssClass="SmlTxtBox" runat="server" TabIndex="4" 
                                        Width="60px"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>                                 
                                </td>
                                <td>
                                </td>
                                  <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="End Date / Time"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEndDate" CssClass="SmlTxtBox" runat="server" 
                                        ReadOnly="true" TabIndex="5"></asp:TextBox>
                                    <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" AutoPostBack="False" />  
                                     <asp:TextBox ID="txtEndTime" CssClass="SmlTxtBox" runat="server" TabIndex="6" Width="60px"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>                               
                                </td>                          
                            </tr>                            
                            <tr>
                                 <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Text="Location"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" colspan="4">
                                    <asp:TextBox ID="txtLocation" CssClass="LrgTxtBox" runat="server" TabIndex="7" 
                                        Width="96.5%"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>                               
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text="Description"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" colspan="4">
                                    <asp:TextBox ID="txtDescription" CssClass="ExLrgTxtBox" runat="server" TextMode="MultiLine"
                                        Height="73px" Width="97.6%" TabIndex="8" MaxLength="500"></asp:TextBox>
                                    <span class="ClsMdtStar">* </span>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" CssClass="ClsBtn" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="ClearMessages()"
                            TabIndex="9" />
                        <asp:Button ID="btnCancel" CssClass="ClsBtn" runat="server" Text="Cancel" CausesValidation="False"
                            OnClick="btnCancel_Click" TabIndex="10" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
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
            <td style="height: 5px;">
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" align="center">
                <table width="60%">
                    <tr>
                        <td class="ClsBorderlight" align="center" style="width: 150px;">
                            <asp:Label ID="lblNameSearch" runat="server" CssClass="ClsLabel" Text="Name"></asp:Label>
                            <span class="ClsLabel colonPadding">:                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSearch" CssClass="ExLrgTxtBox" runat="server" TabIndex="11"></asp:TextBox>
                            <asp:Button ID="btnSearch" CssClass="ClsBtn" runat="server" Text="Search" OnClick="btnSearch_Click"
                                TabIndex="12" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: ">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trItemCount" runat="server">
                                <td align="center" style="width: 100%;">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwODDetails"
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
                                    <asp:ListView ID="lstvwODDetails" runat="server" DataKeyNames="ODId,UserId" OnItemCommand="lstvwODDetails_ItemCommand"
                                        OnItemDataBound="lstvwODDetails_ItemDataBound" OnItemEditing="lstvwODDetails_ItemEditing" OnItemDeleting="lstvwODDetails_ItemDeleting" OnDataBound="lstvwODDetails_DataBound"
                                        OnSorting="lstvwODDetails_Sorting">
                                        <LayoutTemplate>
                                            <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                    <th align="left" class="paddingL" style="width: 160px; font-size: 10pt;">
                                                        <asp:LinkButton ID="lnkName" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="UserName" CommandName="SortRow">Name</asp:LinkButton>
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 130px; font-size: 10pt;">
                                                        <asp:LinkButton ID="lnkbtnDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="Date" CommandName="SortRow">Start Date / Time</asp:LinkButton>
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 130px; font-size: 10pt;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="EndDate" CommandName="SortRow">End Date / Time</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="clsLabelgrd" width="140px" style="font-size: 10pt;">
                                                        <span><b>Location</b></span>
                                                    </th>                                                                                          
                                                    <th width="40px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                    </th>
                                                    <th width="40px" align="center" class="clsLabelgrd" style="font-size: 10pt;">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="7" align="left">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwODDetails">
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
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("UserName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblLeaveDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Location") %>'></asp:Label>
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
                                                <td align="left">
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("UserName") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblLeaveDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("Date") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelC" Text='<%#Eval("EndDate") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                        Text='<%#Eval("Location") %>'></asp:Label>
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
                                    <asp:HiddenField ID="hidODId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                    <asp:HiddenField ID="hidCmbValue" runat="server" />
                                    <asp:HiddenField ID="hidYearId" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidQueryString" runat="server" Value ="" />
                                    <asp:HiddenField ID="hidStaffGroup" runat="server" Value="0" />
                                    <asp:ObjectDataSource TypeName="BusinessLogic.ODDetailsBL" EnablePaging="true" ID="lstvwDSobj"
                                        runat="server" SelectMethod="GetAllODDetails" SelectCountMethod="Count" EnableCaching="false">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="aiStaffGroupId" ControlID="hidCmbValue" PropertyName="Value" />
                                            <asp:ControlParameter Name="aiUserId" ControlID="hidUserId" PropertyName="Value" />
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                            <asp:ControlParameter Name="asFilter" ControlID="txtSearch" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwODDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>"
                    CssClass="ClsBtn" OnClientClick="ClosePopup(); return false;" 
                    CausesValidation="false" TabIndex="13" />
            </td>
        </tr>        
    </table>
    <script language="javascript" type="text/javascript">
        _clienttxtStartTime = "<%=this.txtStartTime.ClientID %>";
        _clienttxtEndTime = "<%=this.txtEndTime.ClientID %>";
        _clienttxtDescription = "<%=this.txtDescription.ClientID %>";
        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"

 <%--       var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);


        function BeginRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
        }

        function EndRequestHandler(Sender, args) {
            var postBackElement = Sender._postBackSettings.sourceElement;
            AutoSearch();
        }

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 0);
        }--%>


        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ValidateStartTime(oSrc, args) {
            var StartTime = $('#' + _clienttxtStartTime).val()
            if (StartTime.trim() != "") {
                if (!isTimeValid(_clienttxtStartTime)) {
                    oSrc.errormessage = "Start Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateEndTime(oSrc, args) {
            var EndTime = $('#' + _clienttxtEndTime).val()
            if (EndTime.trim() != "") {
                if (!isTimeValid(_clienttxtEndTime)) {
                    oSrc.errormessage = "End Time should be in HH:MM AM/PM format (e.g 10:00 AM)."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function CompareDates(oSrc, args) {
            var stDate = $('#' + _clienttxtStartDate).val()
            var edDate = $('#' + _clienttxtEndDate).val() 

            var startDate;
            if (document.all)
                startDate = new Date(stDate.replace('-', ' ') + " " + $('#' + _clienttxtStartTime).val());
            else
                startDate = new Date(convertdate(stDate) + " " + $('#' + _clienttxtStartTime).val());

            var endDate;
            if (document.all)
                endDate = new Date(edDate.replace('-', ' ') + " " + $('#' + _clienttxtEndTime).val());
            else
                endDate = new Date(convertdate(edDate) +" " + $('#' + _clienttxtEndTime).val());

            if (startDate >= endDate) {
                oSrc.errormessage = "End Date / Time should be greater than Start Date / Time.";
                args.IsValid = false
                return true
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

        function ClosePopup() {
            window.close();
        }

        function ValidateDescription(oSrc, args) {
            var Description = $get(_clienttxtDescription).value.trim();
            if (Description == "") {
                oSrc.errormessage = "Description should not be blank."
                args.IsValid = false;
                return true;
            }
            if (Description.length > 500) {
                oSrc.errormessage = "Description length should not be greater than 500 character(s)."
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ClearMessages() {
            $('#' + '<%=this.lblMessage.ClientID %>').html("")
            $('#' + '<%=this.lblErrorMsg.ClientID %>').html("")
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
            bt = document.getElementById("<%=this.btnSearchOD.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
