<%@ Page Title="" Language="C#" MasterPageFile="~/PPSMaster.master" AutoEventWireup="true"
    ValidateRequest="false" EnableEventValidation="false" CodeFile="School_Notices.aspx.cs"
    Inherits="School_Notices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="RITeSchool/Styles/Styles2.css" rel="stylesheet" type="text/css" />
    <table cellspacing="2" cellpadding="0" width="99%" border="0">
        <tbody>
            <tr align="center">
                <td align="center">
                    <div style="width: 99%" align="center">
                        <br />
                        <div id="nifty" align="center">
                            <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
                            </b></b>
                            <div style="width: 100%" align="center">
                                <table width="100%" align="center">
                                    <tr>
                                        <td class="HeadTxtB borderBtm " style="height: 25px" align="left" colspan="2">
                                            School Notices
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwNoticeList" PageSize="20">
                                                <fields>
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
                                                </fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ListView ID="lstvwNoticeList" runat="server" OnItemDataBound="lstvwNoticeList_ItemDataBound"
                                                OnDataBound="lstvwNoticeList_DataBound" 
                                                OnItemCommand="lstvwNoticeList_ItemCommand" DataKeyNames="EndDate">
                                                <layouttemplate>
                                                    <table id="Table2" align="center" width="100%" runat="server" class="GridBorder">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                         <th align="center" width="15%">
                                                            <asp:LinkButton runat="server" AutoPostBack="true"   style="color:Black;" CommandName="SortNotice" CommandArgument="StartDate">Date</asp:LinkButton>
                                                            </th>
                                                            <th  class="paddingL" align="left">
                                                            <asp:LinkButton runat="server" AutoPostBack="true"  style="color:Black;" CommandName="SortNotice" CommandArgument="NoticeName">Details</asp:LinkButton>
                                                            </th>
                                                           
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                        <tr id="trDataPager" class="ClsBorderPager">
                                                            <td colspan="2">
                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwNoticeList"
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
                                                </layouttemplate>
                                                <itemtemplate>
                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                       <td  align="center" >
                                                           <asp:Label ID="lblStartDate"  runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL" align="left" >
                                                            <asp:HyperLink ID="lnkName" style="color: #0000FF" NavigateUrl="#" runat="server" Text='<%# Eval("NoticeName") %>'></asp:HyperLink>
                                                        </td>
                                                      
                                                    </tr>
                                                </itemtemplate>
                                                <alternatingitemtemplate>
                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                     <td  align="center">
                                                           <asp:Label ID="lblStartDate"  runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL" align="left">
                                                            <asp:HyperLink ID="lnkName" style="color: #0000FF" runat="server" NavigateUrl="#" Text='<%# Eval("NoticeName") %>'></asp:HyperLink>
                                                        </td>
                                                        
                                                    </tr>
                                                </alternatingitemtemplate>
                                                <EmptyDataTemplate>
                                                        <tr >
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
                                            <asp:ObjectDataSource TypeName="BusinessLogic.NoticeDetailsBL" EnablePaging="True"
                                                ID="ObjDSNoticeDetails" runat="server" SelectMethod="GetNotices" SelectCountMethod="GetNoticesCount"
                                                EnableCaching="False">
                                                <selectparameters>
                                                    <asp:Parameter Type="String" Name="asDisplayLocation" DefaultValue="H" />
                                                    <asp:Parameter Type="Boolean" Name="abShowAllNotices"  DefaultValue="true" />
                                                    <asp:Parameter Name="MaximumRows" DefaultValue="20" Type="Int32" />
                                                    <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                    <asp:ControlParameter ControlID="hidSortExpression" 
                                                        DefaultValue="StartDate " Name="asSortExpression" 
                                                        PropertyName="Value" />
                                                         <asp:ControlParameter ControlID="hidSortDirection" 
                                                        DefaultValue=" desc " Name="asSortDirection" 
                                                        PropertyName="Value" />
                                                    <asp:ControlParameter ControlID="hidSchoolId" DefaultValue="" 
                                                        Name="aiSchoolId" PropertyName="Value" />
                                                        <asp:ControlParameter ControlID="hidUserRoleId"  DefaultValue="" 
                                                        Name="aiUserRoleId" PropertyName="Value" />

                                                    
                                                </selectparameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            <asp:HiddenField ID="hidSchoolId" runat="server" />
                                            <asp:HiddenField ID="hidUserRoleId" runat="server" Value="3" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <b class="rbottom"><b class="r4"></b><b class="r3"></b><b class="r2"></b><b class="r1">
                            </b></b>
                        </div>
                        <br />
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="DivTextNotice" runat="server" style="visibility: hidden; display: none;
        background-color: #FFFFFF; position: absolute; margin: 0px; padding: 0px; width: 550px;
        height: 300px; left: 0px; top: 0px; line-height: normal; width: auto; border: 2px solid #496C00;
        margin: -10px 10px 10px -150px; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
        <div id="InnerDivHeader" runat="server" style="background-image: url('RITeSchool/images/GridHeaderBG.gif');
            cursor: hand; background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;
            cursor: pointer;">
            <div runat="server" id="divNoticeName" style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left">
                Notice :</div>
            <span style="cursor: hand">
                <img id="Img1" alt="Hide Popup" style="vertical-align: top" runat="server" src="RITeSchool/images/close_vista.gif"
                    onclick="javascript:HideNoticePopup();" border="0" />
            </span>
        </div>
        <div style="padding: 10px; text-align: left; width: 550px; height: 255px; overflow: auto;"
            class="ClsLabel" id="divText" runat="server">
        </div>
    </div>
    <script type="text/javascript">
        _clientDivTextNotice = "<%=this.DivTextNotice.ClientID %>"
        _clientdivText = "<%=this.divText.ClientID %>"
        _clientdivNoticeName = "<%=this.divNoticeName.ClientID %>"

        function ShowNoticePopup(content, NoticeName) {
            var x, y, tt_ovr_
            var cssstyle = document.getElementById(_clientDivTextNotice).style;

            document.getElementById(_clientdivText).innerText = content;
            document.getElementById(_clientdivText).innerHTML = content;
            document.getElementById(_clientdivNoticeName).innerText = NoticeName;
            document.getElementById(_clientdivNoticeName).innerHTML = NoticeName;
            var width = 250
            var height = 180
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            cssstyle.visibility = "visible"


            cssstyle.display = "block"
        }
        function HideNoticePopup() {
            document.getElementById(_clientDivTextNotice).style.visibility = "hidden"
            document.getElementById(_clientDivTextNotice).style.display = "none"
            var cssstyleMain = document.getElementById(_clientDivTextNotice).style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
    </script>
</asp:Content>
