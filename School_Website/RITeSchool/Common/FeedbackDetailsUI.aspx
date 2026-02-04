<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="FeedbackDetailsUI.aspx.cs" Inherits="FeedbackListUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Src="~/UserControls/FeedbackDetails.ascx" TagName="FeedbackDetails"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<style type="text/css">
        .ie6fixed
        {
            position: absolute !important;
            top: expression(0+((e=document.documentElement.scrollTop)?e:document.body.scrollTop)+'px !important');
            left: expression(0+((e=document.documentElement.scrollLeft)?e:document.body.scrollLeft)+'px !important');
            background: transparent !important;
            -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#7F000000,endColorstr=#7F000000);
            -ms-filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#b2FFFFFF,endColorstr=#b2FFFFFF); /*filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#7F000000,endColorstr=#7F000000);*/
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#99000000, endColorstr=#99000000);
            zoom: 1;
        }
    </style>
    <div class="MainBodyDiv" style="filter: alpha(opacity=40);">
        <table id="tblMain" width="90%">
            <tr>
                <td align="right" style="width: 23%; padding-right: 30px;" valign="top">
                    <span class="ClsMdtStar">* Mandatory Fields</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:RadioButton ID="rdbUserFeedback" runat="server" CssClass="ClsLbl" Checked="True"
                        GroupName="A" Text="Feedback from users" AutoPostBack="true" OnCheckedChanged="rdbUserFeedback_CheckedChanged" />
                    <asp:RadioButton ID="rdbOtherFeedback" runat="server" CssClass="ClsLbl" GroupName="A"
                        Text="Feedback from others" AutoPostBack="true" OnCheckedChanged="rdbOtherFeedback_CheckedChanged" />
                </td>
            </tr>
            <tr id="trUserName" runat="server">
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left" class="ClsBorderlight" style="width: 80px">
                                <asp:Label ID="lblUser" runat="server" Text="User Name :" CssClass="ClsLabel"></asp:Label>
                            </td>
                            <td style="width: 113px">
                                <asp:TextBox ID="txtuser" runat="server">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnUserSearch" CssClass="ClsBtn" runat="server" Text="Search" CausesValidation="false"
                                    OnClick="btnUserSearch_Click" />
                            </td>
                            <td>
                            </td>
                            <td align="right" class="LblNormal">
                                <asp:HyperLink ID="hlnkAddNew" runat="server" Text="Add new feedback" NavigateUrl="#">
                                </asp:HyperLink>
                            </td>
                            <td  style="width:23px"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <contenttemplate>
                    <asp:Label ID="lblDelete"  ForeColor="Blue" CssClass="ClsLabelUpdate" runat="server"  
                                Visible="False" EnableViewState="False"></asp:Label>
                            </contenttemplate>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="RowDataBound" />
                            <asp:AsyncPostBackTrigger ControlID="btnUserSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnDisplayToUser" EventName="Click" />
                       </triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" id="tblFeedbackUser" runat="server">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                        <tr runat="server" id="trCombo">
                            <td align="left">
                                <asp:UpdatePanel UpdateMode="Always" runat="server">
                                    <contenttemplate>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Panel ID="pnlUserFeedbackGrid" runat="server">
                                                        <table id="Table1" runat="server" width="100%">
                                                            <tr runat="server" id="trTotalRec" align="center" visible="false">
                                                                <td>
                                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">To</span>
                                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">Out Of</span>
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">Records</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" valign="top">
                                                                    <asp:GridView CssClass="GridBorder" ID="grdUsersFeedback" runat="server" AllowPaging="True"
                                                                        AutoGenerateColumns="False" OnRowCommand="grdUsersFeedback_RowCommand" AllowSorting="True"
                                                                        OnRowDataBound="grdUsersFeedback_RowDatabound" EmptyDataText="No Record Found."
                                                                        Width="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                        GridLines="None" DataKeyNames="Feedback_Id, Is_Selected" OnSorting="grdUsersFeedback_Sorting"
                                                                        OnRowCreated="grdUsersFeedback_RowCreated" OnPageIndexChanging="grdUsersFeedback_PageIndexChanging">
                                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                        </PagerStyle>
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <input id="chkSelectAll" runat="server" type="checkbox" onclick="CheckAllOrUncheckAll(this)" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ChkBoxSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Feedback_Date" HeaderText="Date" SortExpression="Feedback_Date"
                                                                                DataFormatString="{0:dd MMM yyyy}">
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                    Width="10%" />
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                    Wrap="False" Width="10%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField SortExpression="User_Name"  DataField="User_Name" HeaderText="User Name">
                                                                                <ItemStyle Width="12%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                                                <HeaderStyle Width="12%" HorizontalAlign="Left" CssClass="paddingLSML"   Wrap="False"  VerticalAlign="Middle" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Email_Address" HeaderText="Email">
                                                                                <ItemStyle Width="12%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                                                <HeaderStyle Width="12%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Feedback" HeaderText="Comments ">
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="55%"
                                                                                    Wrap="true" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                                                    Wrap="True" Width="55%" />
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField ButtonType="Image" CommandName="Edit_FeedbackDetails" HeaderText="Edit"
                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif">
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                            </asp:ButtonField>
                                                                            <asp:ButtonField ButtonType="Image" CommandName="Delete_FeedbackDetails" HeaderText="Delete"
                                                                                Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <PagerTemplate>
                                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                                            OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:GridView>
                                                                    <asp:ObjectDataSource TypeName="BusinessLogic.FeedbackDetailsBL" EnablePaging="true"
                                                                        ID="GrdDSobj" runat="server" SelectMethod="GetUserFeedbackDetails" SortParameterName="sortExpression"
                                                                        SelectCountMethod="GettUsersFeedbackCount" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                        <SelectParameters>
                                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                            <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                                                PropertyName="Value" />
                                                                            <asp:ControlParameter Name="asUserName" Type="String" ControlID="txtuser" DefaultValue=" "
                                                                                PropertyName="Text" />
                                                                        </SelectParameters>
                                                                    </asp:ObjectDataSource>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight " style="width: 2%; background-color: #ffffc4;">
                                                        <span class="LblNrmlB" style="font-weight: bold; height: 16px;">Note :</span>
                                                </td>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; width: 27%">
                                                        <span class="LblSmlV">"Selected feedback will be displayed to user at login page in 'Appreciations' OR 'Testimonials' menu."</span>
                                                </td>
                                                <td style="width:1%;"></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <table border="0" cellpadding="0" align="center">
                                                        <tbody>
                                                            <tr>
                                                                <td align="center" style="height: 20px">
                                                                    <asp:Button ID="btnDisplayToUser" runat="server" BorderStyle="Solid" BorderWidth="1px" disable-page="true"
                                                                        CausesValidation="false" CssClass="ClsBtnSml" Text="Save" Visible="True" OnClick="btnDisplayToUser_Click" />
                                                                </td>
                                                                <asp:HiddenField ID="hidMode" runat="server" />
                                                                <asp:HiddenField ID="hidFeedbackId" runat="server" />
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </contenttemplate>
                                    <triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnUserSearch" EventName="Click" />
                                    </triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="tdOther" runat="server">
                    <table width="100%">
                        <tr>
                            <td align="center">
                               <asp:UpdatePanel UpdateMode="Always" runat="server">
                                    <contenttemplate>
                                <asp:Label ID="lblError" runat="server" CssClass="ClsMdtStar" EnableViewState="false" />
                                <asp:Label ID="lblUpdateOther" EnableViewState="false" ForeColor="Blue" CssClass="ClsLabelUpdate"
                                    runat="server" Visible="false" Text=""></asp:Label>
                                        </contenttemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ValidationGroup="Upload" HeaderText="Please fix following error(s):"
                                    CssClass="clslbl" ID="ValidationSummary1" runat="server" />
                                <asp:CustomValidator ID="cstValLink" ClientValidationFunction="validateDuplicateLink"
                                    ValidationGroup="Upload" runat="server">
                                </asp:CustomValidator>
                                <asp:CustomValidator ID="cstValidateFile" Display="None" runat="server" ClientValidationFunction="ValidateFile"
                                    ErrorMessage="Invalid file format." CssClass="TxtNormal" ValidationGroup="Upload">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="550px" class="ClsBorderlight">
                                    <thead style="height: 20; background-color: #AAAAAA">
                                        <tr>
                                            <td colspan="3" style="font-size: 15px; color: White; background-color: #AAAAAA;
                                                font-weight: bold">
                                                Feedback from others
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody style="background-color: #F3F3F3">
                                        <tr>
                                            <td class="ClsBorderlight" style="width: 126px; background-color: #F3F3F3">
                                                <asp:Label ID="lblName" runat="server" Text="Link Name :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLinkName" runat="server" MaxLength="100">
                                                </asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqlinkName" runat="server" Display="None" ValidationGroup="Upload"
                                                    ErrorMessage="Link name should not be blank." ControlToValidate="txtLinkName">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight" style="width: 126px; background-color: #F3F3F3;">                                                
                                                <span >Select File to Upload :</span>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqFile" runat="server" Display="None" ErrorMessage="File to upload should be selected."
                                                    ValidationGroup="Upload" ControlToValidate="FileUpload1">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnUpload" runat="server" ValidationGroup="Upload" CssClass="ClsBtn"
                                    BorderStyle="Solid" BorderWidth="1px" Text="Upload" OnClick="btnUpload_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" BorderStyle="Solid" BorderWidth="1px"
                                    CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">

                                <asp:ListView ID="lstvwOtherFeedback" runat="server" DataKeyNames="LinkId,FilePath,IsSelected"
                                    OnItemCommand="lstvwOtherFeedback_ItemCommand" OnItemDataBound="lstvwOtherFeedback_ItemDataBound">
                                    
                                  <LayoutTemplate>
                                        <table id="Table2" align="center" width="650px" runat="server" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th width="50px">
                                                    <input type="checkbox" id="SelectAll" onclick="SelectAll(this)" runat="server" />
                                                </th>
                                                <th width="100px">
                                                    <asp:LinkButton ID="lnkDate" runat="server" CausesValidation="false" CommandArgument="InsertDate" CommandName="SortRow"
                                                        ForeColor="Black" Text="Date"></asp:LinkButton>
                                                </th>
                                                <th align="left" class="paddingL">
                                                    <asp:LinkButton ID="lnkName" CommandArgument="LinkName" CommandName="SortRow" ForeColor="Black" CausesValidation="false"
                                                        Text="Link" runat="server"></asp:LinkButton>
                                                </th>
                                                <th align="center" width="50px">
                                                    Edit
                                                </th>
                                                <th align="center" width="50px">
                                                    Delete
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                     </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("InsertDate") %>'></asp:Label>
                                            </td>
                                            <td class="paddingL" align="left">
                                                <asp:HyperLink NavigateUrl="#" ID="lnkName" Text='<%# Eval("LinkName") %>' runat="server">LinkButton</asp:HyperLink>
                                            </td>
                                            <td align="center">
                                                <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" CommandName="EditCommand"
                                                    runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                            </td>
                                            <td align="center">
                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                            </td>
                                        </tr>
                                     </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("InsertDate") %>'></asp:Label>
                                            </td>
                                            <td class="paddingL" align="left">
                                                <asp:HyperLink NavigateUrl="#" ID="lnkName" Text='<%# Eval("LinkName") %>' runat="server">LinkButton</asp:HyperLink>
                                            </td>
                                            <td align="center">
                                                <asp:ImageButton ID="imgBtnEdit" CausesValidation="false" CommandName="EditCommand"
                                                    runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                            </td>
                                            <td align="center">
                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <tr style="width: 550px;">
                                            <td align="center" class="LblNoRecord">
                                                No Record Found.
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" BorderStyle="Solid" CausesValidation="false" disable-page="true"
                                    BorderWidth="1px" Text="Save" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                        <asp:HiddenField ID="hidFilePath" runat="server" />
                        <asp:HiddenField ID="hidOtherMode" runat="server" />
                        <asp:HiddenField ID="hidLinkId" runat="server" />
                        <asp:HiddenField ID="hidSortexpressionOther" runat="server" />
                    </table>
                </td>
            </tr>
        </table>
        <div id="divPopup" runat="server" class="ie6fixed" style="width: 100%; display: none;
            background: rgba(0,0,0,.6) !important;left: 0; position: absolute;
            top: 0; z-index: 101;">
            <div id="Div5" runat="server" style="position: absolute; margin: 0px; padding: 0px;
                width: 650px; height: 510px; border-width: 0px; left: 5px; top: 0px; line-height: normal;
                border: solid 2px darkgreen; margin: -70px 60px 100px 60px; background-color: #FFFFBF;">
                <div style="background-color: Transparent; padding-top: 3px; height: 50px; background-image: url(../images/GridHeaderBG.gif);
                    background-repeat: repeat-x; color: Black; text-align: right;">
                    <div style="font-size: 12px; width: 500px; letter-spacing: 1px; padding-left: 8px;
                        font-weight: bold; color: Green; float: left; height: 10px" align="left">
                       Add New Feedback
                    </div>
                    <span style="cursor: hand" onclick="javascript:HidePopup2();">
                        <img id="btnClose" class="img-align-top" runat="server" alt="Hide Popup" style="padding-right: 7px;" src="../images/close_vista.gif" border="0" />
                    </span>
                </div>
                <div style="padding: 2px; background-color: #FFFFBF; text-align: left; vertical-align: top;
                    color: #333; overflow: auto; height: 450px; width: 620px;" id="Div1">
                    <asp:UpdatePanel ID="upnlUserControl" runat="server">
                        <contenttemplate>
                            <uc1:FeedbackDetails ID="FeedbackDetails1" runat="server" />
                        </contenttemplate>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdUsersFeedback" EventName="RowCommand" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">

        _clientlstvwOtherFeedback = "<%=this.lstvwOtherFeedback.ClientID %>"
        _clienttxtLinkName = "<%=this.txtLinkName.ClientID %>"
        _clientlblName = "<%=this.lblName.ClientID %>"
        _clientcstValLink = "<%=this.cstValLink.ClientID %>"
        _clientlblUpdateOther = "<%=this.lblUpdateOther.ClientID %>"
        _clienthidOtherMode = "<%=this.hidOtherMode.ClientID %>"
        _clientFileUpload1 = "<%=this.FileUpload1.ClientID %>"
        _clientcstValidateFile = "<%=this.cstValidateFile.ClientID%>"
        _clientlblDelete = "<%=this.lblDelete.ClientID %>"
        _clientucFeedbackDetails = "<%=this.FeedbackDetails1.ClientID %>"


        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this Feedback details?')) {
                bResult = false
            }
            return bResult
        }
        function validateDuplicateLink(oSrc, args) {

            if (document.getElementById(_clientlblUpdateOther)) {
                document.getElementById(_clientlblUpdateOther).innerText = "";
                document.getElementById(_clientlblUpdateOther).innerHTML = "";
            }
            if (document.getElementById(_clientlblDelete)) {
                document.getElementById(_clientlblDelete).innerText = "";
                document.getElementById(_clientlblDelete).innerHTML = "";
            }
            if (document.getElementById(_clienthidOtherMode).value == "New") {
                var sRowNo = "";
                var iRowNumber = 0;
                var txtTheme = (document.getElementById(_clienttxtLinkName).value).trim();
                var lblName = document.getElementById(_clientlstvwOtherFeedback + "_ctrl" + iRowNumber + "_lnkName");

                if (txtTheme != "") {
                    while (lblName) {
                        if (txtTheme.toLowerCase() == (lblName.innerHTML).toLowerCase()) {
                            if (sRowNo = "")
                                sRowNo = (iRowNumber + 1);
                            else
                                sRowNo += (iRowNumber + 1) + ", ";
                        }
                        iRowNumber += 1;
                        lblName = document.getElementById(_clientlstvwOtherFeedback + "_ctrl" + iRowNumber + "_lnkName")
                    }
                }
                if (sRowNo != "") {
                    sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                    oSrc.errormessage = "Link name should not be duplicated for row(s): " + sRowNo + ".";
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            return true;
        }

        var Page_IsValid = true;

        function SelectAll(chk) {
            $("#<%=lstvwOtherFeedback.ClientID %>_Table2 input[type=checkbox]").attr('checked', chk.checked);
        }
        function SelectedCountOther() {
        	Page_IsValid = true;
            var n = $('#<%=lstvwOtherFeedback.ClientID %>_Table2 input:checked').length;
            if (n == 0) {
            	alert("Atleast one feedback should be selected for display to user.")
            	Page_IsValid = false;
                return false;
            }
            return true;
        }
        function CheckAllOrUncheckAll(chk) {
            $('#<%=grdUsersFeedback.ClientID %> input:checkbox').attr('checked', chk.checked);
        }

        function SelectedCount() {
        	Page_IsValid = true;
            var n = $('#<%=grdUsersFeedback.ClientID %> input:checked').length;
            if (n == 0) {
            	alert("Atleast one feedback should be selected for display to user.")
            	Page_IsValid = false;
                return false;
            }
            return true;
        }
        function ValidateFile(aSrc, args) {


            var myImage = document.getElementById(_clientFileUpload1).value
            if (!CheckFileType(myImage)) {
                document.getElementById(_clientcstValidateFile).errormessage = "Invalid file format."
                document.getElementById(_clientFileUpload1).empty = "";
                args.IsValid = false
                return false;
            }
            args.IsValid = true
            return false
        }

        function CheckFileType(sFileName) {
            var bIsValid = true
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() != ".INK" && sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() != ".EXE") {
                    bIsValid = true
                }
                else {
                    bIsValid = false
                }
            }
            return bIsValid
        }
        function HidePopup2() {

            $get("<%=this.divPopup.ClientID %>").style.display = "none";
            document.getElementById(_clientucFeedbackDetails + '_txtName').value = "";
            document.getElementById(_clientucFeedbackDetails + '_txtEmail').value = "";
            document.getElementById(_clientucFeedbackDetails + '_txtContent').value = ""
            document.getElementById(_clientucFeedbackDetails + '_optlstFeedbackType_0').checked = true;
            document.getElementById(_clientucFeedbackDetails + '_optlstFeedbackType_1').checked = false
            document.getElementById(_clientucFeedbackDetails + '_optlstFeedbackType_2').checked = false;
            document.getElementById(_clientucFeedbackDetails + '_optlstFeedbackFor_0').checked = true;
            document.getElementById(_clientucFeedbackDetails + '_optlstFeedbackFor_1').checked = false;
            document.getElementById(_clientucFeedbackDetails + '_ValidationSummary1').innerHTML = "";
            document.getElementById(_clientucFeedbackDetails + '_ValidationSummary1').innerText = "";
            if (document.getElementById(_clientucFeedbackDetails + '_lblMessage') != null) {
                document.getElementById(_clientucFeedbackDetails + '_lblMessage').innerHTML = "";
                document.getElementById(_clientucFeedbackDetails + '_lblMessage').innerText = "";
            }            
            return false;
        }
        function ShowPopup2() {

            var cssstyle = $get("<%=this.Div5.ClientID %>").style;
            var width = 140;
            var height = 53;
            var left = parseInt((screen.width / 4) - (width / 3)) - 90;
            var top = parseInt((screen.height / 2) - (height / 2)) - 90;
            cssstyle.left = left + "px";
            cssstyle.top = top + "px";
            $get("<%=this.divPopup.ClientID %>").style.height = $(document).height();
            $get("<%=this.divPopup.ClientID %>").style.width = $(document).width();
            $get("<%=this.divPopup.ClientID %>").style.display = "";

            if (document.getElementById(_clientlblDelete)) {
                document.getElementById(_clientlblDelete).innerText = "";
                document.getElementById(_clientlblDelete).innerHTML = "";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
