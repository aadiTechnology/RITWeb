<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master"
    ValidateRequest="false" EnableEventValidation="false" AutoEventWireup="true"
    CodeFile="UploadNoticesUI.aspx.cs" Inherits="UploadNoticesUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="right">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate>
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                 <asp:ValidationSummary ID="valSumErrorMsgText" runat="server" CssClass="ClsLabel"
                    ShowSummary="true" ValidationGroup="TextNotice" />
                 <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="DuplicateTextValue" ValidationGroup="TextNotice"
                                                    Display="None">
                                                </asp:CustomValidator>
                 </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr style="width: 100%;">
            <td align="center">
                <table>
                 <tr>
            <td class="TxtNormal" align="center" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <contenttemplate>
                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                    </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
                    <tr class="LblNormal">
                        <td>                            
                            <span>Notice Display Type :</span>
                        </td>
                        <td>
                            <asp:RadioButton ID="optLink" runat="server" AutoPostBack="True" GroupName="Notice"
                                Text="File" OnCheckedChanged="optLink_CheckedChanged" Checked="True">
                            </asp:RadioButton>
                            <asp:RadioButton ID="optText" runat="server" GroupName="Notice" Text="Text" OnCheckedChanged="optText_CheckedChanged"
                                AutoPostBack="True">
                            </asp:RadioButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>       
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                                      <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" 
                               Width="100%" CssClass="ClsMdtStar" EnableViewState="False"></asp:Label>
                                </contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr runat="server">
            <td align="center">
                <table id="tblLinkNoticeControls" width="80%" runat="server">
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <contenttemplate>
                                    <table id="Table1" runat="server" width="100%">
                                        <tr>
                                            <td class="TxtNormal" colspan="4">
                                                <asp:RequiredFieldValidator ID="ReqLinkName" runat="server" ErrorMessage="Link name should not be blank."
                                                    Display="None" ControlToValidate="txtLinkName">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstStartEndDateValidation" runat="server" SetFocusOnError="True"
                                                    Display="None" ClientValidationFunction="IsStartEndDateValid">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="reqEndDate" runat="server" ErrorMessage="End date should not be blank."
                                                    Display="None" ControlToValidate="txtCalEndDtPopup">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstLinkNameValidation" runat="server" ClientValidationFunction="DuplicateValue"
                                                    Display="None">
                                                </asp:CustomValidator>                                               
                                                <asp:CustomValidator ID="cstFileNameValidation" ControlToValidate="fileUploadItems"
                                                    runat="server" ClientValidationFunction="IsFileUploaded" Display="None" ValidateEmptyText="True">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="cstFileNameValidationforUpdate" ControlToValidate="fileUploadItems"
                                                    runat="server" ClientValidationFunction="IsValidFile" Display="None" ValidateEmptyText="True">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" ControlToValidate="fileUploadItems"
                                                    runat="server" ClientValidationFunction="IsValidImageFile" Display="None" ValidateEmptyText="True">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="CstVCheckUpdate" runat="server" ClientValidationFunction="IsUpdateRunning"
                                                    ControlToValidate="ddlDisplayLocation" Display="None" SetFocusOnError="True"
                                                    ValidationGroup="CheckUpdate">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="ReqSortOrder" runat="server" ErrorMessage="Sort order should not be blank."
                                                    Display="None" ControlToValidate="txtSortOrder">
                                                </asp:RequiredFieldValidator>
                                                 <asp:CustomValidator ID="cstRoleValidate" runat="server"  ClientValidationFunction="CheckBoxListRoles"
                                        ErrorMessage="At least one user role should be selected." Display="None"  ></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server"  ClientValidationFunction="ValidateClasses"
                                        ErrorMessage="At least one class should be selected." Display="None"  ></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width:25%;" >
                                                <span class="ClsLabel">Link Name :</span>
                                            </td>
                                            <td class="TxtNormal" style="width:25%;" align="left">
                                                <asp:TextBox ID="txtLinkName" runat="server" MaxLength="50" CssClass="LrgTxtBox" >
                                                </asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL" align="left" style="width:87px;">
                                                <span class="ClsLabel">Display Location :</span>
                                            </td>
                                            <td style="width:25%;" align="left">
                                                <asp:DropDownList ID="cmbDisplayLocation" runat="server" CssClass="MidCombo" onselectedindexchanged="cmbDisplayLocation_SelectedIndexChanged" AutoPostBack ="true"
                                                    >
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">Start Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCalStartDtPopup" CssClass="MidTxtBox" runat="server" AutoPostBack="True">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="CalStartDtPopup" runat="server" Control="txtCalStartDtPopup"
                                                    Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start Date." />
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL" style="width: 87px">
                                                <span class="ClsLabel">Start Time :</span>
                                            </td>
                                            <td class="TxtNormal" style="width: 300px" align="left">
                                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="MidTxtBox" MaxLength="8">12:00 AM</asp:TextBox><span
                                                    class="LblSmlGray">&nbsp;e.g. 10:00 AM</span>
                                                <asp:CustomValidator ID="cstInvalidStartTime" CssClass="LblErrorMsg" runat="server"
                                                    SetFocusOnError="True" Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM."
                                                    ClientValidationFunction="IsValidStartTime">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="cstTimeRangeValidation" runat="server" SetFocusOnError="True"
                                                    Display="None" ClientValidationFunction="IsValidTimeRange" ControlToValidate="txtEndTime">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">End Date :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtCalEndDtPopup" CssClass="MidTxtBox" runat="server" AutoPostBack="True">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="CalEndDtPopup" runat="server" Control="txtCalEndDtPopup" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid End Date." />
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL" style="width: 87px">
                                                <span class="ClsLabel">End Time :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="MidTxtBox" MaxLength="8">11:59 PM</asp:TextBox><span
                                                    class="LblSmlGray">&nbsp;e.g. 04:00 PM</span>
                                                <asp:CustomValidator ID="cstInvalidEndTime" CssClass="LblErrorMsg" runat="server"
                                                    SetFocusOnError="True" ErrorMessage="Please enter valid end time e.g. 10:00 AM."
                                                    Display="None" ClientValidationFunction="IsValidEndTime">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Notice File :</span>
                                            </td>
                                            <td align="left" >
                                                <asp:FileUpload ID="fileUploadItems" runat="server" ToolTip="Only PDF,JPEG,BMP,JPG,PNG files are allowed" Width="200px" />
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Image File :</span>
                                            </td>
                                            <td align="left" >
                                                <asp:FileUpload ID="FilUplodNotice" runat="server" ToolTip="Only image files are allowed" Width="170px" /> 
                                                <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false" EnableViewState = "false"  /> 
                                                <asp:ImageButton ID="imgbtnDelete" runat="server"  CausesValidation="false" 
                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                    OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                    onclick="imgbtnDelete_Click" EnableViewState = "false"  />                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="paddingL" colspan="2">
                                                <span class="LblSmlGray">(Supports only .PDF, .PNG,.JPEG, .JPG, .BMP file type. File size should not exceed
                                                    10MB.)&nbsp; &nbsp;&nbsp;</span>
                                            </td>
                                            <td align="left" colspan="2">
                                            <span class="LblSmlGray ClsLabel">(Supports only ".JPG", ".JPEG", ".PNG", ".BMP" file type. File size should not exceed
                                                    10MB.)&nbsp; &nbsp;&nbsp;</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 87px">
                                                <span class="ClsLabel">Description :</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" valign="top">
                                                <asp:TextBox ID="txtDescription" CssClass="ExLrgTxtBox" runat="server" AutoPostBack="false"
                                                    TextMode="MultiLine" MaxLength="600" Height="50px"></asp:TextBox>   
                                            </td>                                            
                                             <td class="ClsBorderlight paddingL" align="left" style="width: 87px">
                                                <span class="ClsLabel">Sort Order :</span>
                                            </td>
                                            <td style="width: 220px" align="left">
                                                <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="3" CssClass="MidTxtBox"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                </asp:TextBox>
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                            </td>
                                        </tr>                                      
                                     <tr>
                                       <td class="ClsBorderlight" align="right">
                                                          <asp:Label ID="Label6" runat="server" Text="Applicable to :" CssClass="ClsLblLgnd" style="padding-left:190px;white-space:nowrap;"
                                                                        EnableViewState="False" ></asp:Label><br>
                                                                 <asp:CheckBox ID="chkAll" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" style="white-space:nowrap; padding-right:08px"
                                                                    onclick="CheckAllUncheckAlls()" /> 
                                                        </td>
                                                        <td align="left" valign="top" class="ClsBorderlight">
                                                                  <asp:CheckBoxList ID="chkListRoles" runat="server" CellPadding="0" CellSpacing="0"
                                                                CssClass="ClsBorderLight" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" onclick="ShowClasses()" >
                                                            </asp:CheckBoxList>
                                                                </td>
                                                                <td style="width: 87px">
                                                <span id ="spnMandatoryUserRoleFile" class="ClsMdtStar" runat = "server">*</span>
                                            </td>
                                       </tr>
                                        <tr id="trClassDivisions" runat="server" style="display: none;">
                                            <td align="right" class="ClsBorderlight" valign="top">
                                            <span class="LblRht"> :</span>
                                                <asp:Label ID="Label2" runat="server" Text="Associated Class(es)" CssClass="LblRht"
                                                    EnableViewState="False"></asp:Label><br />
                                                
                                                <asp:CheckBox ID="chkLstClasses" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" />
                                            </td>
                                            <td class="ClsBorderlight" valign="top" align="left"> 
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>                                   
                                                    <asp:ListView ID="lstvwStandardDivisions" runat="server" DataKeyNames="StandardId" 
                                                                OnItemDataBound="lstvwStandardDivisions_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table align="right" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                        cellpadding="0" cellspacing="1" class="GridBorder">                                                        
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" style="padding-left: 5px">
                                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'/>                                                            
                                                                        </td>
                                                                        <td align="left" style="padding-left: 5px">                                                        
                                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                                            </asp:CheckBoxList>
                                                                        </td>                                
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height:10px">
                                                                        <td align="left" style="padding-left: 5px">
                                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'   />                                                           
                                                                        </td>
                                                                        <td align="left" style="padding-left: 5px">                                                            
                                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoRecord" runat="server" Text= "<%$ Resources:LocalizedResources, NoRecordsFound%>" 
                                                        EnableViewState="False"></asp:Label>       
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnUpdate" runat="server" CssClass="ClsBtn" OnClick="btnUpdate_Click" disable-page="true"
                                                    Text="Save" />
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="ClsBtn"
                                                    OnClick="btnCancel_Click" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </contenttemplate>
                                <triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lstvwNoticeDetails" EventName="ItemCommand" />
                                </triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr id="trTextNoticeControls" runat="server">
            <td align="center">              
                        <table id="tblTextNoticeControls" width="100%" runat="server" visible="true">
                            <tr>
                                <td align="center">
                                  <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <contenttemplate>


                                    <table  style="width: 80%">
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 200px">
                                                <span class="ClsLabel">Notice Name :</span>
                                            </td>
                                            <td style="width: 250px" align="left">
                                                <asp:TextBox ID="txtNoticeName" class="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox><span class="ClsMdtStar">&nbsp;*&nbsp;</span>
                                                <asp:RequiredFieldValidator ID="reqNoticName" runat="server" ErrorMessage="Notice name should not be blank."
                                                    ValidationGroup="TextNotice" ControlToValidate="txtNoticeName" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL" style="width: 200px">
                                                <span class="ClsLabel">Display Location :</span>
                                            </td>
                                            <td class="TxtNormal" style="width: 350px" align="left">
                                                <asp:DropDownList ID="cmbDisplayLocationTextNotice" class="MidCombo" 
                                                    runat="server" 
                                                    onselectedindexchanged="cmbDisplayLocationTextNotice_SelectedIndexChanged" AutoPostBack = "true">
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">Start Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtStartDateTextNotice" CssClass="MidTxtBox" runat="server" AutoPostBack="True">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="calStartDtText" runat="server" Control="txtStartDateTextNotice" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start Date." />
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                                <asp:CustomValidator ID="cstValStartDateText" runat="server" ClientValidationFunction="IsStartEndDateValidForText"
                                                    Display="None" ValidationGroup="TextNotice">
                                                </asp:CustomValidator>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL">
                                                <span class="ClsLabel">Start Time :</span>
                                            </td>
                                            <td class="TxtNormal" style="width: 240px" align="left">
                                                <asp:TextBox runat="server" ID="txtStartTimeTextNotice" CssClass="MidTxtBox" MaxLength="8">12:00 AM</asp:TextBox><span
                                                    class="LblSmlGray">&nbsp;e.g. 10:00 AM</span>
                                                <asp:CustomValidator CssClass="LblErrorMsg" runat="server" SetFocusOnError="True"
                                                    ID="cstValTime" Display="None" ErrorMessage="Please enter valid start time e.g. 10:00 AM."
                                                    ValidationGroup="TextNotice" ClientValidationFunction="IsValidStartTimeText">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator runat="server" SetFocusOnError="True" Display="None" ClientValidationFunction="IsValidTimeRangeText"
                                                    ValidationGroup="TextNotice" ID="cstValStartEndTime" 
                                                    ControlToValidate="txtEndTimeTextNotice"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">End Date :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox ID="txtEndDateTextNotice" CssClass="MidTxtBox" runat="server" AutoPostBack="True">
                                                </asp:TextBox>
                                                <rjs:PopCalendar ID="calEndDtText" runat="server" Format="dd MMM yyyy" ShowWeekend="True"
                                                    ShowErrorMessage="false" InvalidDateMessage="Please select valid end date." Control="txtEndDateTextNotice" />
                                                <asp:RequiredFieldValidator ID="reqEndDateText" runat="server" ErrorMessage="End date should not be blank."
                                                    ValidationGroup="TextNotice" Display="None" ControlToValidate="txtEndDateTextNotice">
                                                </asp:RequiredFieldValidator>
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight paddingL" style="width: 111px">
                                                <span class="ClsLabel">End Time :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox runat="server" ID="txtEndTimeTextNotice" CssClass="MidTxtBox" MaxLength="8">11:59 PM</asp:TextBox><span
                                                    class="LblSmlGray">&nbsp;e.g. 04:00 PM</span>
                                                <asp:CustomValidator CssClass="LblErrorMsg" runat="server" SetFocusOnError="True"
                                                    ID="cstValEndTimeText" ErrorMessage="Please enter valid end time e.g. 10:00 AM."
                                                    ValidationGroup="TextNotice" Display="None" ClientValidationFunction="IsValidEndTimeText">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">Sort Order :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox runat="server" ID="txtSortOrderTextNotice" CssClass="MidTxtBox" MaxLength="3"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                </asp:TextBox> <span class="ClsMdtStar">*&nbsp;</span>
                                                <asp:RequiredFieldValidator ID="reqValSortOrderText" runat="server" ValidationGroup="TextNotice"
                                                    ErrorMessage="Sort order should not be blank." ControlToValidate="txtSortOrderTextNotice"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">Image File Path :</span>
                                            </td>
                                            <td align="left" >
                                                <asp:FileUpload ID="FilTextNoticeUpload" runat="server" ToolTip="Only image files are allowed" Width="170px" /> 
                                                <asp:ImageButton ID="btnTxtView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false" EnableViewState = "false"  /> 
                                                <asp:ImageButton ID="imgbtnTxtDelete" runat="server"  CausesValidation="false" 
                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                    OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                    onclick="imgbtnTxtDelete_Click" EnableViewState = "false"  />                                                
                                            </td>                                                                                        
                                        </tr>
                                        <tr>
                                       <td class="ClsBorderlight" align="right" style="height: 54px">
                                                          <asp:Label ID="Label1" runat="server" Text="Applicable to :" CssClass="ClsLblLgnd" style="padding-left:190px;white-space:nowrap;"
                                                                        EnableViewState="False" ></asp:Label><br>
                                                                 <asp:CheckBox ID="chkAllText" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" style="white-space:nowrap; padding-right:08px"
                                                                    onclick="CheckAllUncheckAllsText()" /> 
                                                        </td>
                                                        <td align="left" valign="top" class="ClsBorderlight" 
                                                style="height: 54px">
                                                                  <asp:CheckBoxList ID="chkListRolesText" runat="server" CellPadding="0" CellSpacing="0"
                                                                CssClass="ClsBorderLight" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" onclick="ShowClassesText()">
                                                            </asp:CheckBoxList>
                                                            <span id = "spnMandatoryUserRoleText" class="ClsMdtStar" runat="server">*</span>
                                                            <asp:CustomValidator ID="cstRoleTextValidate" runat="server"  ClientValidationFunction="CheckBoxListRolesText"
                                                    ErrorMessage="At least one user role should be selected." ValidationGroup="TextNotice" Display="None"  ></asp:CustomValidator>
                                                                </td>
                                                                <td class="ClsBorderlight paddingL" align="left" style="width: 87px">
                                                                    <span class="ClsLabel">Description :</span>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="txtTextNoticeDescription" CssClass="ExLrgTxtBox" runat="server" AutoPostBack="false"
                                                                         TextMode="MultiLine" MaxLength="300" Height="70px"></asp:TextBox>   
                                                                </td> 
                                                                
                                       </tr>
                                       <tr id="trClassDivisionsText" style="display:none">
                                            <td align="left" class="ClsBorderlight" valign="top">
                                            <span class="LblRht"> :</span>
                                                <asp:Label ID="Label3" runat="server" Text="Associated Class(es)" CssClass="LblRht"
                                                    EnableViewState="False"></asp:Label><br />
                                               
                                                <asp:CheckBox ID="chkLstClassesText" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll %>" />
                                            </td>
                                            <td class="ClsBorderlight" valign="top" align="left"> 
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>                                   
                                                    <asp:ListView ID="lstvwStandardDivisionsText" runat="server" DataKeyNames="StandardId" 
                                                                OnItemDataBound="lstvwStandardDivisionsText_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table align="right" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                        cellpadding="0" cellspacing="1" class="GridBorder">                                                        
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" style="padding-left: 5px">
                                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'/>                                                            
                                                                        </td>
                                                                        <td align="left" style="padding-left: 5px">                                                        
                                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                                            </asp:CheckBoxList>
                                                                        </td>                                
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height:10px">
                                                                        <td align="left" style="padding-left: 5px">
                                                                            <asp:CheckBox ID="chkStandard" runat="server" Text='<%# Eval("StandardName") %>'   />                                                           
                                                                        </td>
                                                                        <td align="left" style="padding-left: 5px">                                                            
                                                                            <asp:CheckBoxList ID="chkStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                                                CssClass="ClsLabel" RepeatColumns="6">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="LblNoRecord" align="center">
                                                                            <asp:Label ID="lblNoRecord" runat="server" Text= "<%$ Resources:LocalizedResources, NoRecordsFound%>" 
                                                        EnableViewState="False"></asp:Label>       
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:CustomValidator ID="CustomValidator4" runat="server"  ClientValidationFunction="ValidateTextClasses" ValidationGroup="TextNotice"
                                                                ErrorMessage="At least one class should be selected." Display="None"  ></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="4" >
                                                <CKEditor:CKEditorControl ID="FCKNoticeContent" Toolbar="Bold|Italic|Underline|Strike|-|Subscript|Superscript NumberedList|BulletedList|-|Outdent|Indent / Styles|Format|Font|FontSize|TextColor|BGColor" BasePath="../ckeditor/" Width="99%" ReadOnly="false" runat="server" Height="250px">
                                                </CKEditor:CKEditorControl>
                                              
                                            </td>
                                        </tr>
                                    </table>
                                     </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwNoticeDetails" EventName="ItemCommand" />      
                        <asp:PostBackTrigger ControlID="btnSaveText"   />
                        <asp:AsyncPostBackTrigger ControlID = "btnCancelText" EventName="Click" /> 
                    </triggers>
                </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                         <contenttemplate>
                                            <asp:Button runat="server" Text="Save" class="ClsBtn" ID="btnSaveText" OnClick="btnSaveText_Click" disable-page="true"
                                                ValidationGroup="TextNotice" />
                                            <asp:Button runat="server" Text="Cancel" class="ClsBtn" ID="btnCancelText" OnClick="btnCancelText_Click"
                                                CausesValidation="False" />
                                        </contenttemplate>
                                        <triggers>
                                             <asp:AsyncPostBackTrigger ControlID="lstvwNoticeDetails" EventName="ItemCommand" />
                                        </triggers>
                                   </asp:UpdatePanel>
                                </td>
                            </tr>
                            
                        </table>                   
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" runat="server" style="width: 70%">
                    <tr>
                        <td class="ClsBorderlight">
                            <span class="ClsLabel paddingL">Display Location :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDisplayLocation" runat="server" CssClass="MidCombo" ValidationGroup="CheckUpdate"
                                OnSelectedIndexChanged="ddlDisplayLocation_SelectedIndexChanged" CausesValidation="True"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RadioButton class="ClsLabel" ID="optAllNotices" runat="server" GroupName="Filter"
                                Text="Show All Notices" OnCheckedChanged="optNotices_CheckedChanged" AutoPostBack="True" />
                            <asp:RadioButton class="ClsLabel" ID="optActiveNotices" runat="server" GroupName="Filter"
                                Text="Show Active Notices" OnCheckedChanged="optNotices_CheckedChanged" AutoPostBack="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <td align="left" colspan="1" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                        <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                            CssClass="LblNrmlB" EnableViewState="False" Height="16px" Width="46px"></asp:Label>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                        
                            <span class="LblSmlV" style="width: 100%;border-width=0px;" >Select the notices from the list to be displayed on School web site under School Notices.</span>
                    </td>
                </table>
            </td>
        </tr>
        <tr id="trlstvwTextNotice" runat="server" visible="false">
            <td>
                <asp:HiddenField ID="hidModeText" runat="server">
                </asp:HiddenField>
                <asp:HiddenField ID="hidNoticeIdText" runat="server">
                </asp:HiddenField>
                <asp:HiddenField ID="hidNoticeImage" runat="server">
                </asp:HiddenField>
            </td>
        </tr>
        <tr id="trLink" runat="server">
            <td>
                <table id="tblLstvwLinkNotic" align="center" width="100%" runat="server">
                    <tr>
                        <td align="center" style="width: 100%">
                            <asp:UpdatePanel ID="upnlLstvwLinkNotice" runat="server" UpdateMode="Conditional">
                                <contenttemplate>
                                    <table align="center" width="100%">
                                        <tr id="trDtPgCount" runat="server" visible="true">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwNoticeDetails" PageSize="20">
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
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwNoticeDetails" runat="server" DataKeyNames="NoticeId,DisplayLocation,SortOrder,IsSelected, NoticeDescription, NoticeImage"
                                                    OnDataBound="lstvwNoticeDetails_DataBound" OnItemCommand="lstvwNoticeDetails_ItemCommand"
                                                    OnItemDataBound="lstvwNoticeDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                            class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                              
                                                                <th align="left" class="paddingLSML" width="15%;">
                                                                    <asp:LinkButton ID="LinkBtnLinkName" runat="server" CausesValidation="false" CommandArgument="NoticeName"
                                                                        CommandName="SortRow" ForeColor="Black">Link 
                                                        Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="paddingLSML" width="15%">
                                                                    <asp:LinkButton ID="LinkBtnDisplayLocation" runat="server" CausesValidation="false"
                                                                        CommandArgument="DisplayLocation" CommandName="SortRow" ForeColor="Black">Display Location</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="18%">
                                                                    <asp:LinkButton ID="LinkBtnStartDt" runat="server" CausesValidation="false" CommandArgument="StartDate"
                                                                        CommandName="SortRow" ForeColor="Black">Start 
                                                        Date and Time</asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="18%">
                                                                    <asp:LinkButton ID="LinkBtnEndDt" runat="server" CausesValidation="false" CommandArgument="EndDate"
                                                                        CommandName="SortRow" ForeColor="Black">End Date and Time</asp:LinkButton>
                                                                </th>
                                                                <th align="center" class="paddingL" style="width: 12%;">
                                                                    <asp:LinkButton ID="LinkBtnSortOrder" runat="server" CausesValidation="false" CommandArgument="outSortOrder"
                                                                        CommandName="SortRow" ForeColor="Black">Sort 
                                                        Order</asp:LinkButton>
                                                                </th>
                                                                <th id="thFileName" runat="server" class="paddingL" align="left" style=" width:15%;">
                                                                    <asp:LinkButton ID="LinkBtnFileName" runat="server" CausesValidation="false" CommandArgument="FileName"
                                                                        CommandName="SortRow" ForeColor="Black">File Name</asp:LinkButton>
                                                                </th>
                                                                  <th style="width:4%">
                                                                    <asp:Label runat="server" Text="Select"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 5%;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 4%;">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="9">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwNoticeDetails"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
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
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                           
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("NoticeName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDisplayLocation" runat="server" Text='<%# Eval("DisplayLocation") %>'> </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblStartDt" runat="server" Text='<%# Eval("StartDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblEndDt" runat="server" Text='<%# Eval("EndDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="tdFileName" runat="server" align="left" class="paddingL">
                                                                <asp:HyperLink ID="lnkBtnFileName" runat="server" CausesValidation="false" Text='<%# Eval("FileName") %>'></asp:HyperLink>
                                                            </td>
                                                             <td align="center" >
                                                                <asp:CheckBox ID="chkSelect" runat="server" tooltip="Select notice to display under School Notices"></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateNotice" CausesValidation="false"
                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteNotice" CausesValidation="false"
                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                          
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("NoticeName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDisplayLocation" runat="server" Text='<%# Eval("DisplayLocation") %>'> </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblStartDt" runat="server" Text='<%# Eval("StartDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblEndDt" runat="server" Text='<%# Eval("EndDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="tdFileName" runat="server" align="left" class="paddingL">
                                                                <asp:HyperLink ID="lnkBtnFileName" runat="server" CausesValidation="false" Text='<%# Eval("FileName") %>'></asp:HyperLink>

                                                            </td>
                                                              <td>
                                                                <asp:CheckBox ID="chkSelect" runat="server" tooltip="Select notice to display under School Notices"></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateNotice"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteNotice"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
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
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidNoticeId" runat="server" />
                                                <asp:HiddenField ID="hidFileDisplayLocation" runat="server" />
                                                <asp:HiddenField ID="hidFileDisplayLocationText" runat="server" />
                                                <asp:HiddenField ID="hidSortOrder" runat="server" />
                                                <asp:HiddenField ID="hidSortOrderText" runat="server" />
                                                <asp:HiddenField ID="hidFileName" runat="server" />
                                                <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidCurrentOperation" runat="server" />
                                                <asp:HiddenField ID="hidMode" runat="server" />
                                                <asp:HiddenField ID="hidNoticeImageId" runat="server" />
                                                <asp:HiddenField ID="hidStandardDivIds" runat="server" Value="" />
                                            </td>
                                        </tr>
                                    </table>
                                </contenttemplate>
                                <triggers>
                                    <asp:PostBackTrigger ControlID="btnUpdate" />
                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwNoticeDetails" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlDisplayLocation" EventName="SelectedIndexChanged" />
                                    <asp:PostBackTrigger ControlID="btnSaveText"   />                                   
                                </triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ObjectDataSource TypeName="BusinessLogic.NoticeDetailsBL" EnablePaging="True"
                    ID="ObjDSNoticeDetails" runat="server" SelectMethod="GetAll" SelectCountMethod="GetCount"
                    EnableCaching="False">
                    <selectparameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                        <asp:ControlParameter ControlID="ddlDisplayLocation" PropertyName="SelectedValue"
                            Type="String" Name="asDisplayLocation" DefaultValue="B" />
                        <asp:ControlParameter ControlID="optAllNotices" PropertyName="Checked" Type="Boolean"
                            Name="abShowAllNotices" />
                        <asp:ControlParameter ControlID="optText" PropertyName="Checked" Type="Boolean" Name="abText" />
                        <asp:ControlParameter ControlID="hidSortExpression" Name="asSortExpression" Type="String"
                            PropertyName="Value" />
                        <asp:ControlParameter ControlID="hidSortDirection" Name="asSortDirection" Type="String"
                            PropertyName="Value" />
                        <asp:Parameter Name="MaximumRows" DefaultValue="20" Type="Int32" />
                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                    </selectparameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr id="trSave" runat="server">
            <td align="center">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSaveSelected" runat="server" Text="Save" CssClass="ClsBtn" CausesValidation="false" disable-page="true"
                                OnClick="btnSaveSelected_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _ClienttxtStartDate = "<%=this.txtCalStartDtPopup.ClientID %>";
        _ClienttxtEndDate = "<%=this.txtCalEndDtPopup.ClientID %>";
        _ClientcstStartEndDateValidation = "<%=this.cstStartEndDateValidation.ClientID%>";
        _ClienttxtLinkName = "<%=this.txtLinkName.ClientID %>";
        _ClienttxtNoticeName = "<%=this.txtNoticeName.ClientID %>";
        _ClienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>";
        _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>";
        _clientFilUplodNotice = "<%=this.FilUplodNotice.ClientID %>";
        _ClientcstFileNameValidation = "<%=this.cstFileNameValidation.ClientID %>";
        _ClientCurrentOPeration = "<%=this.hidCurrentOperation.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clientlstvwNoticeDetailsId = "<%=this.lstvwNoticeDetails.ClientID %>"
        _ClientcstLinkNameValidation = "<%=this.cstLinkNameValidation.ClientID %>"
        _ClienttxtStartTime = "<%=this.txtStartTime.ClientID %>"
        _ClienttxtEndTime = "<%=this.txtEndTime.ClientID %>"
        _ClientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _ClientcstTimeRangeValidation = "<%=this.cstTimeRangeValidation.ClientID %>"
        _ClentoptAllNotices = "<%=this.optAllNotices.ClientID %>"
        _ClientoptActiveNotices = "<%=this.optActiveNotices.ClientID %>"
        _ClientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _ClienttxtStartDateTextNotice = "<%=this.txtStartDateTextNotice.ClientID %>"
        _ClienttxtEndDateTextNotice = "<%=this.txtEndDateTextNotice.ClientID %>"
        _ClientcstValStartDateText = "<%=this.cstValStartDateText.ClientID %>"
        _ClienttxtStartTimeTextNotice = "<%=this.txtStartTimeTextNotice.ClientID %>"
        _ClienttxtEndTimeTextNotice = "<%=this.txtEndTimeTextNotice.ClientID %>"
        _ClientcstValStartEndTime = "<%=this.cstValStartEndTime.ClientID %>"
        _ClientDtPgCount = "<%=this.DtPgCount.ClientID %>"
        _ClientcstFileNameValidationforUpdate = "<%=this.cstFileNameValidationforUpdate.ClientID %>";
        _clientchkAll = "<%=this.chkAll.ClientID %>";
        _clientchkListRoles = "<%=this.chkListRoles.ClientID %>";
        _clientchkAllText = "<%=this.chkAllText.ClientID %>";
        _clientchkListRolesText = "<%=this.chkListRolesText.ClientID %>";
        _ClientcmbDisplayLocation = "<%=this.cmbDisplayLocation.ClientID %>";
        _ClientcmbDisplayLocationTextNotice = "<%=this.cmbDisplayLocationTextNotice.ClientID %>";
        _clientchkLstClasses = "<%=this.chkLstClasses.ClientID %>";
        _clientchkLstClassesText = "<%=this.chkLstClassesText.ClientID %>";
        _clienttrClassDivisions = "<%=this.trClassDivisions.ClientID%>"       
        _clientlstvwStandardDivisions = "<%=this.lstvwStandardDivisions.ClientID %>"
        _clientlstvwStandardDivisionsText = "<%=this.lstvwStandardDivisionsText.ClientID %>"

        function IsValidTimeRange(oSrc, args) {

            var StartDt = ""; var EndDt = "";
            var sStrtDate = document.getElementById(_ClienttxtStartDate).value
            var sEndDate = document.getElementById(_ClienttxtEndDate).value

            var sStrtTime = document.getElementById(_ClienttxtStartTime).value
            var sEndTime = document.getElementById(_ClienttxtEndTime).value

            if (sStrtDate != "" && sEndDate != "") {
                if (document.all) {
                    StartDt = new Date(sStrtDate.replace('-', ' '));
                    EndDt = new Date(sEndDate.replace('-', ' '));
                }
                else {
                    StartDt = new Date(convertdate(sStrtDate));
                    EndDt = new Date(convertdate(sEndDate));
                }

                if (convertvaliddate(sStrtDate) == convertvaliddate(sEndDate)) {
                    if (new Date(convertdate(sStrtDate + " " + sStrtTime)) > new Date(convertdate(sEndDate + " " + sEndTime))) {
                        oSrc.errormessage = "End time should be greater than start time.";
                        args.IsValid = false;
                        return true;
                    }
                }

            }
        }
        function IsValidTimeRangeText(oSrc, args) {

            var StartDt = ""; var EndDt = "";
            var sStrtDate = document.getElementById(_ClienttxtStartDateTextNotice).value
            var sEndDate = document.getElementById(_ClienttxtEndDateTextNotice).value

            var sStrtTime = document.getElementById(_ClienttxtStartTimeTextNotice).value
            var sEndTime = document.getElementById(_ClienttxtEndTimeTextNotice).value

            if (sStrtDate != "" && sEndDate != "") {
                if (document.all) {
                    StartDt = new Date(sStrtDate.replace('-', ' '));
                    EndDt = new Date(sEndDate.replace('-', ' '));
                }
                else {
                    StartDt = new Date(convertdate(sStrtDate));
                    EndDt = new Date(convertdate(sEndDate));
                }


                if (convertvaliddate(sStrtDate) == convertvaliddate(sEndDate)) {
                    if (new Date(convertdate(sStrtDate + " " + sStrtTime)) > new Date(convertdate(sEndDate + " " + sEndTime))) {
                        oSrc.errormessage = "End time should be greater than start time.";
                        document.getElementById(_ClientcstValStartEndTime).errormessage = "End time should be greater than start time.";
                        args.IsValid = false;
                        return true;
                    }
                }

            }
        }
        function PositiveSortOrder(oSrc, args) {
            var isortOrder = document.getElementById(_ClienttxtSortOrder).value
        }

        function IsFileUploaded(oSrc, args) {
            if (document.getElementById(_ClientlblErrorMsg)) {
                document.getElementById(_ClientlblErrorMsg).innerText = "";
                document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            }

            if (document.getElementById(_ClientlblUpdateSucess)) {
                document.getElementById(_ClientlblUpdateSucess).innerHTML = "";
                document.getElementById(_ClientlblUpdateSucess).innerText = "";
            }

            
            var myImage = document.getElementById(_ClientfileUploadItems).value;
            
            if (myImage == "") {
                oSrc.errormessage = "";
                document.getElementById(_ClientcstFileNameValidation).errormessage = "Notice File to be uploaded should be selected.";
                args.IsValid = false;
                return false;
            }

        }

        function IsValidFile(oSrc, args) {
            var sFileName = document.getElementById(_ClientfileUploadItems).value;
            if (sFileName != "") {
                var extension = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();
                if (extension == ".PDF" || extension == ".JPEG" || extension == ".JPG" || extension == ".PNG" || extension == ".BMP") {
                
                    args.IsValid = true;
                    return false;
                }
                else {
                    oSrc.errormessage = "Invalid file type for Notice File."
                    args.IsValid = false;
                    return true;
                }
            }

        }

        function IsValidImageFile(oSrc, args) {
            var sFileName = document.getElementById(_clientFilUplodNotice).value;
            if (sFileName != "") {
                var extension = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();
                if (extension == ".JPG" || extension == ".JPEG" || extension == ".PNG" || extension == ".BMP") {
                    args.IsValid = true;
                    return false;
                }
                else {
                    oSrc.errormessage = "Invalid file type for Image File.";
                    args.IsValid = false;
                    return true;
                }
            }

        }

        function DuplicateValue(oSrc, args) {
        
            var sRowNo = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtLinkName = document.getElementById(_ClienttxtLinkName).value
            var txtCalStartDtPopup = document.getElementById(_ClienttxtStartDate).value
            var txtCalEndDtPopup = document.getElementById(_ClienttxtEndDate).value
            var txtStartTime = document.getElementById(_ClienttxtStartTime).value
            var txtEndTime = document.getElementById(_ClienttxtEndTime).value
            var sStartDateTime = txtCalStartDtPopup + " " + txtStartTime;
            var sEndDateTime = txtCalEndDtPopup + " " + txtEndTime;

            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblLink = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblLinkName").innerHTML;
                lblStartDt = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblStartDt").innerHTML;
                lblEndDt = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblEndDt").innerHTML;
                if ((txtLinkName.trim()).toLowerCase() == lblLink.toLowerCase() && iRowNumber != (iRowNo - 1) && (sStartDateTime = lblStartDt) && (sEndDateTime == lblEndDt)) {
                    sRowNo += (iRowNumber + 1) + ", " ;

                } 
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = "Link name should not be duplicated for row(s): " + sRowNo + ".";
                document.getElementById(_ClientcstLinkNameValidation).innerText = "Link name should not be duplicated for row(s): " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            

            else {
                args.IsValid = true
                return false
            }
        }

        function DuplicateTextValue(oSrc, args) {
            var sRowNo = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtNoticeName = document.getElementById(_ClienttxtNoticeName).value
            var txtCalStartDtPopup = document.getElementById(_ClienttxtStartDateTextNotice).value
            var txtCalEndDtPopup = document.getElementById(_ClienttxtEndDateTextNotice).value
            var txtStartTime = document.getElementById(_ClienttxtStartTimeTextNotice).value
            var txtEndTime = document.getElementById(_ClienttxtEndTimeTextNotice).value
            var sStartDateTime = txtCalStartDtPopup + " " + txtStartTime;
            var sEndDateTime = txtCalEndDtPopup + " " + txtEndTime;
            

            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblLink = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblLinkName").innerHTML;
                lblStartDt = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblStartDt").innerHTML;
                lblEndDt = document.getElementById(_clientlstvwNoticeDetailsId + "_ctrl" + iRowNumber + "_lblEndDt").innerHTML;
                if ((txtNoticeName.trim()).toLowerCase() == lblLink.toLowerCase() && iRowNumber != (iRowNo - 1) && (sStartDateTime = lblStartDt) && (sEndDateTime == lblEndDt)) {
                    sRowNo += (iRowNumber + 1) + ", ";

                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = "Link name should not be duplicated for row(s): " + sRowNo + ".";
                document.getElementById(_ClientcstLinkNameValidation).innerText = "Link name should not be duplicated for row(s): " + sRowNo + ".";
                args.IsValid = false
                return true
            }


            else {
                args.IsValid = true
                return false
            }
        }
        
        function IsStartEndDateValid(oSrc, args) {

            if (document.getElementById(_ClientlblErrorMsg)) {
                document.getElementById(_ClientlblErrorMsg).innerText = "";
                document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            }
            if (document.getElementById(_ClientlblUpdateSucess)) {
                document.getElementById(_ClientlblUpdateSucess).innerHTML = "";
                document.getElementById(_ClientlblUpdateSucess).innerText = "";
            }
            var sStrtDate = (document.getElementById(_ClienttxtStartDate).value)
            var sEndDate = document.getElementById(_ClienttxtEndDate).value
            if (sStrtDate == "") {
                oSrc.errormessage = "Start date can not be blank.";
                document.getElementById(_ClientcstStartEndDateValidation).errormessage = "Start date should not be blank.";
                args.IsValid = false;
                return false;
            }


            if (sStrtDate != "" && sEndDate != "") {
                var StartDt = new Date(convertdate((document.getElementById(_ClienttxtStartDate).value)))
                var EndDt = new Date(convertdate(document.getElementById(_ClienttxtEndDate).value))

                if (StartDt > EndDt) {
                    oSrc.errormessage = "End date should be greater than start date.";
                    document.getElementById(_ClientcstStartEndDateValidation).errormessage = "End date should be greater than start date.";
                    args.IsValid = false;
                    return false;
                }
            }
        }
        function IsStartEndDateValidForText(oSrc, args) {
            if (document.getElementById(_ClientlblErrorMsg)) {
                document.getElementById(_ClientlblErrorMsg).innerText = "";
                document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            }
            if (document.getElementById(_ClientlblUpdateSucess)) {
                document.getElementById(_ClientlblUpdateSucess).innerHTML = "";
                document.getElementById(_ClientlblUpdateSucess).innerText = "";
            }
            var sStrtDate = (document.getElementById(_ClienttxtStartDateTextNotice).value)
            var sEndDate = document.getElementById(_ClienttxtEndDateTextNotice).value
            if (sStrtDate == "") {
                oSrc.errormessage = "Start date can not be blank.";
                document.getElementById(_ClientcstValStartDateText).errormessage = "Start date should not be blank.";
                args.IsValid = false;
                return false;
            }


            if (sStrtDate != "" && sEndDate != "") {
                var StartDt = new Date(convertdate((document.getElementById(_ClienttxtStartDateTextNotice).value)))
                var EndDt = new Date(convertdate(document.getElementById(_ClienttxtEndDateTextNotice).value))

                if (StartDt > EndDt) {
                    oSrc.errormessage = "End date should be greater than start date.";
                    document.getElementById(_ClientcstValStartDateText).errormessage = "End date should be greater than start date.";
                    args.IsValid = false;
                    return false;
                }
            }
        }

        function IfAddUpdate() {
            var sCurrentOperation
            var bResult = true
            sCurrentOperation = document.getElementById(_ClientCurrentOPeration).value
            if (sCurrentOperation != "AddUpdate") {
                bResult = false
            }
            return bResult
        }

        function SetUpdateOperation() {
            document.getElementById(_ClientCurrentOPeration).value = "AddUpdate"
            return true
        }

        function ConfirmDelete() {

            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }


        function ConfirmActiveDelete() {
            var bResult = true
            if (IfAddUpdate()) {
                AlertUpdateOperation()
                bResult = false
                return bResult
            }
            if (!window.confirm('This is an active notice. Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function IsUpdateRunning(oSrc, args) {

            if (IfAddUpdate()) {
                AlertUpdateOperation()
                args.IsValid = false;
                return false;
            }
            else {
                __doPostBack('ddlDisplayLocation', '');
                return true
            }
        }

        function isTimeValid(result) {

            var timeStr = document.getElementById(result).value;
            timeStr = timeStr.toUpperCase();
            if (trimAll(timeStr) == '')
                return false;

            var timePat = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s)(AM|am|PM|pm)?$/;
            var matchArray = timeStr.match(timePat);

            if (matchArray == null)
                return false;

            hour = matchArray[1];
            minute = matchArray[2];
            second = matchArray[4];
            ampm = matchArray[6];

            if (second == "") { second = null; }
            if (ampm == "") { ampm = null; }

            if (hour < 0 || hour > 12)
                return false;

            if (minute < 0 || minute > 59)
                return false;

            if (second != null && (second < 0 || second > 59))
                return false;

            var str;
            if (hour.length == 1)
                str = '0' + hour;
            else
                str = hour;
            if (minute.length == 1)
                str = str + ':' + '0' + minute;
            else
                str = str + ':' + minute;

            str = str + ' ' + ampm.toUpperCase();

            document.getElementById(result).value = str;
            return true;
        }

        function IsValidStartTime(oSrc, args) {
            if (document.getElementById(_ClienttxtStartTimeTextNotice)) {
                if (document.getElementById(_ClienttxtStartTimeTextNotice).value != '') {
                    if (!isTimeValid(_ClienttxtStartTimeTextNotice)) {
                        args.IsValid = false;
                        return true;
                    }
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = true;
            return false;
        }

        function IsValidEndTimeText(oSrc, args) {
            if (document.getElementById(_ClienttxtEndTimeTextNotice).value) {
                if (document.getElementById(_ClienttxtEndTimeTextNotice).value != '') {
                    if (!isTimeValid(_ClienttxtEndTimeTextNotice)) {
                        args.IsValid = false;
                        return true;
                    }
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = true;
            return false;
        }
        function IsValidStartTimeText(oSrc, args) {
            if (document.getElementById(_ClienttxtStartTimeTextNotice)) {
                if (document.getElementById(_ClienttxtStartTimeTextNotice).value != '') {
                    if (!isTimeValid(_ClienttxtStartTimeTextNotice)) {
                        args.IsValid = false;
                        return true;
                    }
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = true;
            return false;
        }

        function IsValidEndTime(oSrc, args) {
            if (document.getElementById(_ClienttxtEndTime).value) {
                if (document.getElementById(_ClienttxtEndTime).value != '') {
                    if (!isTimeValid(_ClienttxtEndTime)) {
                        args.IsValid = false;
                        return true;
                    }
                    args.IsValid = true;
                    return false;
                }
            }
            args.IsValid = true;
            return false;
        }
        function SelectAll(chk, flag) {

            if (flag == 0)
                $("#<%=lstvwNoticeDetails.ClientID %>_tblNoticeDetails input[type=checkbox]").attr('checked', chk.checked);
        }

        var Page_IsValid = true;
        function SelectedCount(flag) {
            
            Page_IsValid = true;
            if (flag == 0) {
                var n = $('#<%=lstvwNoticeDetails.ClientID %>_tblNoticeDetails input:checked').length;
            }
            return true;
        }


        function CheckAllUncheckAlls() {            
            var checkAll;
            if (document.getElementById(_clientchkAll) != null)
                checkAll = document.getElementById(_clientchkAll).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientchkListRoles + "_" + iRowCount)
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientchkListRoles + "_" + iRowCount);
            }
            ShowClasses();            
        }

        function CheckAllUncheckAllsText() {
            var checkAll;    
            if (document.getElementById(_clientchkAllText) != null)
                checkAll = document.getElementById(_clientchkAllText).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientchkListRolesText + "_" + iRowCount)
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientchkListRolesText + "_" + iRowCount);
            }
            ShowClassesText();
         }

        function CheckBoxListRoles(source, args) {
            
            var j = 0
            var checks = document.forms[0].elements
            var boxLength = checks.length
            for (i = 0; i < boxLength; i++) {
                if ((checks[i].type == 'checkbox' && checks[i].id.match("chkListRoles_") != null) || (checks[i].type == 'checkbox' && checks[i].id.match("chkListRoles_") != null)) {
                    if (checks[i].checked == true) {
                        j++
                    }
                }
            }

            var DisplayLocation = $get(_ClientcmbDisplayLocation).value;
            
            if (j > 0) {
                
                    args.IsValid = true
                    return false
                }

                else {
                    if (DisplayLocation == 'H') {
                        args.IsValid = true
                        return false
                    }
                    else {
                        args.IsValid = false
                        return true
                    }
            }
            }

            function CheckBoxListRolesText(source, args) {
                var j = 0
                var checks = document.forms[0].elements
                var boxLength = checks.length
                for (i = 0; i < boxLength; i++) {
                    if ((checks[i].type == 'checkbox' && checks[i].id.match("chkListRolesText_") != null) || (checks[i].type == 'checkbox' && checks[i].id.match("chkListRolesText_") != null)) {
                        if (checks[i].checked == true) {
                            j++
                        }
                    }
                }

                var DisplayLocation = $get(_ClientcmbDisplayLocationTextNotice).value;

                if (j > 0) {

                    args.IsValid = true
                    return false
                }

                else {
                    if (DisplayLocation == 'H') {
                        args.IsValid = true
                        return false
                    }
                    else {
                        args.IsValid = false
                        return true
                    }
                }
            }

            function CheckOrUncheckAllCheckBox() {
                        if (document.getElementById(_clientchkLstClasses).checked) {
                        $('[id$=chkStandard]').attr('checked', 'checked')
                        $('[id*=chkStandardDivLst]').attr('checked', 'checked')
                    }
                    else {
                            $('[id$=chkStandard').removeAttr('checked')
                            $('[id*=chkStandardDivLst]').removeAttr('checked')
                         }
            }

            function CheckAll(obj, index) {
            var id = 'ctrl' + index + '_chkStandardDivLst_'
            if (obj.checked) {
                $('[id*=' + id + ']').attr('checked', 'checked')
            }
            else {
                $('[id*=' + id + ']').removeAttr('checked')
            }
            CheckAllDependancy();            
        }

        function CheckAllCheck(index) {
            var classId = 'ctrl' + index + '_chkStandardDivLst_'
            var stdId = 'ctrl' + index + '_chkStandard'

            if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                $('[id$=' + stdId + ']').attr('checked', 'checked')
            else
                $('[id$=' + stdId + ']').removeAttr('checked')
            CheckAllDependancy();
        }

        function CheckAllDependancy() {
            var CheckAll = document.getElementById(_clientchkLstClasses).value;
            var v1 = 0;
            var listView = document.getElementById('<%= lstvwStandardDivisions.FindControl("tblStaffInfo").ClientID %>');
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox") {
                        if (!inputs[j].checked) {
                            v1 = 1;
                            break;
                        }
                    }
                    if (v1 == 1)
                        break;
                }
            }
            if (v1 == 1)
                    document.getElementById(_clientchkLstClasses).checked = false;
            else
                    document.getElementById(_clientchkLstClasses).checked = true;
        }

        function CheckOrUncheckAllCheckBoxForText() {
            if (document.getElementById(_clientchkLstClassesText).checked) {
                $('[id$=chkStandard]').attr('checked', 'checked')
                $('[id*=chkStandardDivLst]').attr('checked', 'checked')
            }
            else {
                $('[id$=chkStandard').removeAttr('checked')
                $('[id*=chkStandardDivLst]').removeAttr('checked')
            }
        }

        function CheckAllForText(obj, index) {
            var id = 'ctrl' + index + '_chkStandardDivLst_'
            if (obj.checked) {
                $('[id*=' + id + ']').attr('checked', 'checked')
            }
            else {
                $('[id*=' + id + ']').removeAttr('checked')
            }
            CheckAllDependancyForText();
        }

        function CheckAllCheckForText(index) {
            var classId = 'ctrl' + index + '_chkStandardDivLst_'
            var stdId = 'ctrl' + index + '_chkStandard'

            if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                $('[id$=' + stdId + ']').attr('checked', 'checked')
            else
                $('[id$=' + stdId + ']').removeAttr('checked')

            CheckAllDependancyForText();
        }

        function CheckAllDependancyForText() {
            var CheckAll = document.getElementById(_clientchkLstClassesText).value;
            var v1 = 0;

            var listView = document.getElementById('<%= lstvwStandardDivisionsText.FindControl("tblStaffInfo").ClientID %>');

            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox") {
                        if (!inputs[j].checked) {
                            v1 = 1;
                            break;
                        }
                    }
                    if (v1 == 1)
                        break;
                }
            }
            if (v1 == 1)
                document.getElementById(_clientchkLstClassesText).checked = false;
                
            else
                document.getElementById(_clientchkLstClassesText).checked = true;
                
        }


        function ShowClasses() {
            var trClasses = document.getElementById(_clienttrClassDivisions);
            if (document.getElementById(_clientchkListRoles + "_2").checked) {
                trClasses.style.display = "table-row";
            }
            else {
                trClasses.style.display = "none";
            }
        }

        function ShowClassesText() {
            if ($('#' + _clientchkListRolesText + '_2').attr('checked') == 'checked') {
                $('#trClassDivisionsText').show();
            }
            else
            {
                $('#trClassDivisionsText').hide();
            }
        }

            //This function is used to open popun on click on link annual planner.
        function OpenWindow(sfilepath) {
            window.open(sfilepath);
            return false;
        }

            //This function is used take confirmation about delete.
        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete Event Image?')
        }

        function ValidateClasses(oSrc, args) {
            if ($('[id$=chkListRoles_2]:checked').length == 1) {
                if ($('[Id*=_chkStandardDivLst_]:checked').length == 0) {
                    args.IsValid = false;
                    return true
                }
            }

            args.IsValid = true;
            return false
        }

        function ValidateTextClasses(oSrc, args) {            
            if ($('[id$=chkListRolesText_2]:checked').length == 1) {
                if ($('[Id*=_chkStandardDivLst_]:checked').length == 0) {
                    args.IsValid = false;
                    return true
                }
            }

            args.IsValid = true;
            return false
        }
    </script>
</asp:Content>
