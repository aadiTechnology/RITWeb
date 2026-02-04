<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master" CodeFile="MessageInbox.aspx.cs" Inherits="MessageInbox" ViewStateMode="Disabled"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <style>
        .PaddingLeft-23px{
            padding-left:23px;
        }
        
        .ClsLabel
        {
            font-family:Open Sans;
        }
        
    </style>

    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                        <tr>
                            <td align="left" valign="top">
                                <table style="width: 100%" cellspacing="1" cellpadding="0" border="0">
                                    <tbody>
                                        <tr>
                                            <td align="right" colspan="3">
                                                <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanel3" runat="server" ViewStateMode="Enabled">
                                                    <ContentTemplate>
                                                        <table cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgBtnInbox" EnableViewState="false" OnClick="imgBtnInbox_Click"
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconBtn_Inbox.gif" ToolTip="Message Inbox">
                                                                        </asp:ImageButton>
                                                                        &nbsp;<asp:ImageButton ID="imgBtnShowTrash" EnableViewState="false" OnClick="imgBtnShowTrash_Click"
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconBtn_ArchivedMsgs.gif" ToolTip="Trash Messages">
                                                                        </asp:ImageButton>
                                                                        &nbsp;<asp:ImageButton ID="imgBtnDraft" EnableViewState="false" 
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconBtn_Draft.gif" 
                                                                            ToolTip="Draft" onclick="imgBtnDraft_Click">
                                                                        </asp:ImageButton>
                                                                        &nbsp;<asp:ImageButton ID="imgBtnSentItems" EnableViewState="false" OnClick="imgBtnSentItems_Click"
                                                                            runat="server" ImageUrl="~/RITeSchool/images/IconBtn_SentMsgs.gif" ToolTip="Sent Messages">
                                                                        </asp:ImageButton>
                                                                    </td>
                                                                    <%--<td id="tdEmailSetting" runat="server"  style="width:150px" >
																	<div class="ToprLinkHlilight" style="height:20px">
																		<img alt="" src="../../images/newLink.gif" id="img1" runat="server"  />																		
																			<asp:HyperLink id="hlnkEmailSetting" CssClass="ClsHilightTextB" ToolTip="Email Setting" Text="E-mail Settings" runat="server"
																			Style="white-space: nowrap;cursor:pointer;
																			 font-size: 9pt; font-weight: bold;
																			text-decoration: underline; padding-top: 20px;" onclick="OpenSettingPopup(this);"></asp:HyperLink>
																			</div>
																	</td>--%>
                                                                    <%--<td style="width: 25%" align="right">
                                                                        <asp:Button ID="imgBtnNewMessage" EnableViewState="false" UseSubmitBehavior="false"
                                                                            OnClick="imgBtnComposeMessage_Click" runat="server" Text="Compose Message" CssClass="ClsBtnLrg"
                                                                            BorderWidth="1px" BorderStyle="Solid"></asp:Button>
                                                                    </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td id="tdEmailSetting" runat="server" align="right" colspan="2">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <img alt="" src="../../images/newLink.gif" id="img1" runat="server" />
                                                                                </td>
                                                                                <td class="ToprLinkHlilight" width="138px" style="text-align: right">
                                                                                    <asp:HyperLink ID="hlnkEmailSetting" CssClass="ClsHilightTextB" ToolTip="Email Setting"
                                                                                        Text="E-mail Settings" runat="server" Style="white-space: nowrap; cursor: pointer;
                                                                                        font-size: 9pt; font-weight: bold; text-align: right; text-decoration: underline;
                                                                                        padding-top: 20px; padding-right: 18px;" onclick="OpenSettingPopup(this);"></asp:HyperLink>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="imgBtnInbox" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="imgBtnShowTrash" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="imgBtnSentItems" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center" colspan="7" style="height: 18px">
                                                                    <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                                        ForeColor="Blue" Visible="False" EnableViewState="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSearchDetails" runat="server">
                                                                <td class="ClsBorderlight" align="left" style="width:100px">
                                                                    <asp:Label ID="lblAcademicYear" runat="server" CssClass="ClsLabel" Text="Select Academic Year"></asp:Label>
                                                                    <span class="ClsLabel colonPadding">:</span>
                                                                </td>
                                                                <td align="left" style="width:50px">
                                                                    <asp:DropDownList ID="cmbAcademicYear" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="ClsBorderlight" align="left" style="width:150px">
                                                                    <asp:Label ID="lblSearch" runat="server" CssClass="ClsLabel" Text="Name / Subject / Message Body "></asp:Label>
                                                                    <span class="ClsLabel colonPadding">:</span>
                                                                </td>
                                                                <td align="left" style="width:100px;">
                                                                    <asp:TextBox ID="txtSearch" CssClass="MidTxtBox " runat="server" MaxLength="50" Style="width:98%;"></asp:TextBox>                                                                    
                                                                </td>
                                                                <td class="ClsBorderlight" align="left" style="width:30px">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Date"></asp:Label>
                                                                    <span class="ClsLabel colonPadding">:</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbOperation" runat="server" CssClass="ExSmlCombo">
													                </asp:DropDownList>
                                                                    <asp:TextBox ID="txtSearchDate" CssClass="SmlTxtBox" runat="server"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="cal_SearchDate" runat="server" Control="txtSearchDate" Format="dd MMM yyyy"
                                                                        Culture="en" ShowWeekend="True" AutoPostBack="False" />
                                                                    <asp:Button ID="btnSearch" CssClass="ClsBtn remove-margin-top" runat="server" Text="Search"
                                                                        CausesValidation="false" OnClick="btnSearch_Click" />                                                                    
                                                                    &nbsp;</td>                                                                   
                                                                    <td style="width: 25%" align="right">
                                                                        <asp:Button ID="imgBtnNewMessage" EnableViewState="false" UseSubmitBehavior="false"
                                                                            OnClick="imgBtnComposeMessage_Click" runat="server" Text="Compose Message" CssClass="ClsBtnLrg"
                                                                            BorderWidth="1px" BorderStyle="Solid"></asp:Button>
                                                                    </td>                                                                  
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="7" runat="server" id="tdUnreadMessage" visible="False"
                                                                    class="ClsHilightBGB">
                                                                    <asp:Label ID="lblUnreadMessage" runat="server" Visible="False" Font-Size="10pt"
                                                                        Font-Names="Arial"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trTotalRec" align="center">
                                                                <td colspan="6">
                                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">To </span>
                                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">Out Of </span>
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                    <span class="LblNormal">Records</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100%" valign="top" align="left" colspan="7">
                                                                    <asp:GridView CssClass="GridBorder" ID="grdvwMessageInbox" runat="server" ForeColor="#333333"
                                                                        OnRowDataBound="grdvwMessageInbox_RowDataBound" OnPageIndexChanging="grdvwMessageInbox_PageIndexChanging"
                                                                        OnRowCreated="grdvwMessageInbox_RowCreated" AllowSorting="True" OnSorting="grdvwMessageInbox_Sorting"
                                                                        GridLines="None" CellSpacing="1" CellPadding="0" PageSize="20" AutoGenerateColumns="False"
                                                                        AllowPaging="True" Width="100%" DataKeyNames="Message_Details_Id,Message_Receiver_Details_Id,Read_Message_Flag,ItemType,Attatchment,RequestReadReceipt,IsReadRequestAccepted,ReadingDateTime,HasReadReceipt, Reply_Forward_Flag,Insert_Date"
                                                                        OnDataBound="grdvwMessageInbox_DataBound">
                                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Size="8pt">
                                                                        </PagerStyle>
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientIdGrid,this,'ChkBoxDelete', false)" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                                <HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField HeaderText="Flag" ButtonType="Image">
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="GridDate"
                                                                                    Wrap="False" />
                                                                            </asp:ButtonField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <span style="float: left">
                                                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/attachment.png" Visible='<%# (Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Attatchment")))%>' />
                                                                                        <asp:Label ID="lbl" runat="server" Height="18px" ReadOnly="True" Width="15px"
                                                                                            Visible='<%# (!Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Attatchment")))%>'>
                                                                                         <img src="../images/spacer.gif" width="18px" height="18px"/></asp:Label>
                                                                                    </span>
                                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"UserName")))%>'
                                                                                        CssClass="ClsLabel "> </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <HeaderStyle CssClass="ClspaddingL PaddingLeft-23px" />
                                                                            </asp:TemplateField>
                                                                           <asp:TemplateField Visible="false" HeaderText= "Cc">
                                                                                <ItemTemplate>
                                                                                     <asp:Label ID="lblCCUserName" runat="server" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"CcUserName")))%>'
                                                                                        CssClass="ClsLabel "> </asp:Label>
                                                                                  </ItemTemplate>
                                                                                <ItemStyle Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                <HeaderStyle CssClass="ClspaddingL PaddingLeft-23px"/>
                                                                            </asp:TemplateField>
                                                                            <asp:HyperLinkField NavigateUrl="#" DataTextField="Subject" HeaderText="Subject"
                                                                                SortExpression="Subject">
                                                                                <ItemStyle HorizontalAlign="Left" Width="40%" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                                    Wrap="False" />
                                                                            </asp:HyperLinkField>
                                                                            <asp:HyperLinkField Visible="false" DataTextField="Subject" HeaderText="Read Receipt Information"
                                                                                NavigateUrl="#">
                                                                                <ItemStyle HorizontalAlign="Center" Width="12%" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                                    Wrap="False" />
                                                                            </asp:HyperLinkField>
                                                                            <asp:BoundField DataField="Insert_Date" HeaderText="Received Date" SortExpression="Insert_Date">
                                                                                <ItemStyle HorizontalAlign="Left" Width="18%" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                                    Wrap="False" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                        <PagerTemplate>
                                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" AppendDataBoundItems="true"
                                                                                            CssClass="LblNormal" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged"
                                                                                            runat="server">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:GridView>
                                                                    <asp:ObjectDataSource TypeName="BusinessLogic.MessageDetailsCollectionBL" EnablePaging="true"
                                                                        ID="GrdDSobj" runat="server" SelectMethod="GetMessages" SortParameterName="sortExpression"
                                                                        SelectCountMethod="GetMsgCount" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="cmbAcademicYear" PropertyName="SelectedValue" Name="aiAcadYrId" />
                                                                            <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                            <asp:SessionParameter Name="aiUserRoleId" SessionField="S_USERLOGIN_ROLE_ID" Type="int32" />
                                                                            <asp:ControlParameter ControlID="hidQueryStrViewMode" Name="asMode" Type="String"
                                                                                PropertyName="Value" />
                                                                            <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String"
                                                                                PropertyName="Text" />
                                                                            <asp:ControlParameter ControlID="cmbOperation" PropertyName="SelectedValue" Name="asOperator" />
                                                                            <asp:ControlParameter ControlID="txtSearchDate" Name="dtDate" Type="String" PropertyName="Text" />
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
                                          <td align="center" style="width:100%;" colspan="6">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ListView ID="lstDraftMessageDetails" runat="server" ViewStateMode="Enabled" Visible="false" style="width:100%;"
                                                        DataKeyNames="DraftId" 
                                                        onitemdatabound="lstDraftMessageDetails_ItemDataBound" 
                                                        onitemcommand="lstDraftMessageDetails_ItemCommand" 
                                                        onitemdeleting="lstDraftMessageDetails_ItemDeleting" 
                                                        onitemediting="lstDraftMessageDetails_ItemEditing">                                                          
                                                                <LayoutTemplate>
                                                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                            <th align="left" class="clsLabelgrd" style="width:25%">
                                                                                <span><b>Subject</b></span>
                                                                            </th>
                                                                            <th align="left" class="clsLabelgrd">
                                                                                <span><b>Message Body</b></span>
                                                                            </th>
                                                                            <th align="center" class="clsLabelgrd" width="120px">
                                                                                <span><b>Draft Date</b></span>
                                                                            </th>
                                                                            <th align="center" class="clsLabelgrd" width="70px" style="padding-right: 5px;">
                                                                                <span><b>Delete</b></span>
                                                                            </th>                                                            
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                        <td align="left" style="padding-left:5px; width:20%;">
                                                                            <asp:HyperLink ID="hlnkSubject" NavigateUrl="#" runat="server"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:HyperLink ID="hlnkMessageBody" NavigateUrl="#" runat="server"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblDraftDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                Text='<%#Eval("DraftDate") %>'></asp:Label>
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
                                                                        <td align="left"  style="padding-left:5px; width:20%;">
                                                                            <asp:HyperLink ID="hlnkSubject" NavigateUrl="#" runat="server"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:HyperLink ID="hlnkMessageBody" NavigateUrl="#" runat="server"></asp:HyperLink>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblDraftDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                Text='<%#Eval("DraftDate") %>'></asp:Label>
                                                                        </td>                                                        
                                                                        <td align="center">
                                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                     <div class="LblNoRecord">
                                                                        <asp:Label ID="lblNoRecord" runat="server" Text="No record found."></asp:Label>
                                                                     </div>                                                                     
                                                                </EmptyDataTemplate>
                                                        </asp:ListView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lstDraftMessageDetails" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="imgBtnDraft" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="imgBtnInbox" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="imgBtnShowTrash" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="imgBtnSentItems" EventName="Click" />                                                    
                                                </Triggers>
                                            </asp:UpdatePanel>
                                          </td>
                                        </tr>                                       
                                        <tr>
                                            <td style="height: 15px" align="left" colspan="4">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" ViewStateMode="Enabled">
                                                    <ContentTemplate>
                                                        <table style="height: 100%" cellspacing="1" cellpadding="0" width="100%" align="center"
                                                            border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left" style="height: 30px">
                                                                        <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                                                            CssClass="ClsBtnSml" BorderWidth="1px" BorderStyle="Solid" Visible="True"></asp:Button>
                                                                    </td>
                                                                    <td style="height: 30px" align="left">
                                                                        <asp:Button ID="btnArchive" OnClick="btnArchive_Click" runat="server" Text="Delete"
                                                                            CssClass="ClsBtnSml" BorderWidth="1px" BorderStyle="Solid" Visible="True"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnUnread" runat="server" Text="Mark As Unread" CssClass="ClsBtnSml"
                                                                            BorderWidth="1px" BorderStyle="Solid" Visible="True" Width="109px" OnClick="btnUnread_Click">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                         <asp:Button ID="btnRead" runat="server" Text="Mark As Read"
                                                                            CssClass="ClsBtnSml" Visible="True" 
                                                                             Width="109px" onclick="btnRead_Click"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDeleteFromEveryOne" runat="server" 
                                                                            Text="Delete from Everyone" CssClass="ClsBtnSml" CausesValidation="false"
                                                                            BorderWidth="1px" BorderStyle="Solid" Width="109px" Visible="false" 
                                                                            onclick="btnDeleteFromEveryOne_Click">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td style="width: 90%; height: 30px" align="right">
                                                                        <asp:Button ID="imgBtnComposeMessage" UseSubmitBehavior="false" OnClick="imgBtnComposeMessage_Click"
                                                                            runat="server" Text="Compose Message" CssClass="ClsBtnLrg" BorderWidth="1px"
                                                                            BorderStyle="Solid"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 5px; width: 20%" align="left">
                                            </td>
                                            <td align="left">
                                            </td>
                                            <td style="padding-right: 10px; width: 18%" align="right">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%; height: 20px" align="left">
                                            </td>
                                            <td align="left">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ViewStateMode="Enabled">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidQueryStrViewMode" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidBackUrl" runat="server" />
                                                        <asp:HiddenField ID="HidSuperAdminId" runat="server" />
                                                        <asp:HiddenField ID="HidSuperAdminRoleId" runat="server" />
                                                        <asp:HiddenField ID="hidIsReadReceiptAccepted" runat="server" OnValueChanged="hidIsReadReceiptAccepted_ValueChanged" />
                                                        <asp:HiddenField ID="hidEmailShouldNotBlank" runat="server" />
                                                        <asp:HiddenField ID="hidEmailValidation" runat="server" />
                                                        <asp:HiddenField ID="hidEmailAddress" runat="server" />
                                                        <asp:HiddenField ID="hidCanReceiveMail" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="width: 23%; height: 20px" align="left">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
    <div id="divSetting" runat="server" style="visibility: hidden; display: none; position: absolute;
        margin: 0px; padding: 0px; width: 500px; height: 175px; border-width: 1px; left: 5px;
        top: 150px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
        background-color: white;">
        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
            background-repeat: repeat-x; color: Black; width: 500px; text-align: right">
            <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                E-mail Settings
            </div>
            <span style="cursor: hand" onclick="javascript:HidePopup();">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
            </span>
        </div>
        <div>
            <table>
                <tr>
                    <td colspan="2" align="left">
                        <asp:CheckBox ID="chkReceiveMail" runat="server" ViewStateMode="Enabled" Text=" Yes, I want to receive messages on below email address."
                            CssClass="ClsLabel" Style="padding-right: 1px;" />
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight" style="white-space: nowrap;">
                        <span class="ClsLabel">E-mail :</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailId" runat="server" ViewStateMode="Enabled" CssClass="LrgTxtBox" Width="200px" />
                    </td>
                </tr>
                <tr style="display: none; visibility: hidden;">
                    <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                        <span class="LblNrmlB" style="font-weight: bold">Note :</span>
                    </td>
                    <td align="left" class="ClsBorderlight">
                        <span class="LblSmlV" style="border-width: 0px;">Please contact school admin staff for
                            any addition/change to the email address.</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" valign="bottom" style="padding: 10px;">
                        <asp:Button ID="btnSavePopUp" runat="server" ViewStateMode="Enabled" Text="Save" CssClass="ClsBtnMid" CausesValidation="false"
                            OnClientClick="if(!EmailValidation()) return false;" Width="75px" OnClick="btnSavePopUp_Click" />
                        <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="Close" CssClass="ClsBtnMid" CausesValidation="false"
                            Width="75px" OnClientClick="javascript:HidePopup();return false;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        _clientIdGrid = "<%=this.grdvwMessageInbox.ClientID%>"
        _clienttxtEmailId = "<%=this.txtEmailId.ClientID %>";
        _clienthidEmailAddress = "<%=this.hidEmailAddress.ClientID %>";
        _clienthidCanReceiveMail = "<%=this.hidCanReceiveMail.ClientID %>"
        _clientchkReceiveMail = "<%=this.chkReceiveMail.ClientID %>"
        _clientDivSettings = "<%=this.divSetting.ClientID %>"
        _clienthidEmailShouldNotBlank = "<%=this.hidEmailShouldNotBlank.ClientID %>"
        _clienthidEmailValidation = "<%=this.hidEmailValidation.ClientID %>"
    </script>

    <script src="../Scripts/Common/MessageInbox.js?version=1.1" type="text/javascript"></script>
</asp:Content>

