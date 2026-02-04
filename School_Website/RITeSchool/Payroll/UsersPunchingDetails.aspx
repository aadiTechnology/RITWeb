<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UsersPunchingDetails.aspx.cs" Inherits="UsersPunchingDetails" ViewStateMode ="Disabled"%>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    
     <style type="text/css">
        .padding_Top_21 {
            padding-top:21px;
        }
      
    </style>

    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummary" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
             <tr>
                <td align="center" colspan="3">
                    <table>
                        <tr>
                            <td align="left" width="100px" class="ClsBorderlight">
                                <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="ClsLabel"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" ReadOnly="true" AutoPostBack="True" ViewStateMode="Enabled"
                                   />
                                <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Date should not be blank." OnSelectionChanged="cal_Date_SelectionChanged" 
                                    AutoPostBack="True"  ViewStateMode="Enabled"/>
                            </td>
                            <td>
                              &nbsp;&nbsp; <asp:Button runat="server" ID="btnrefresh" Text="Refresh" class="ClsBtn"  />
                            </td>
                        </tr>
                        <tr style="padding-left: 20px;">
                            <td align="center" style="padding-left: 10px; padding-right: 5px; padding-top:10px;" 
                                                    colspan="3">
                               <asp:CheckBox ID="chkGrpUser" runat="server"  Text="Show User Wise Data" AutoPostBack="true"/>
                            </td>
                        </tr>
                  </table>
                </td>
            </tr>
       </table>
       <div style=" width:100%">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
           <tr align="center">
             <td style=" padding-top:0px; vertical-align: top;" align="center">
                <div style="width: 700px;" align="center">
                   <table width="100%"  id="tblPunchedUsers">
                    <tr align="center" style="width:100%" >
                        <td align="center" id="tdpunched" style="font-weight:bold">
                           Checked-In Users
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center">
                          <asp:UpdatePanel ID="UpnlListViewPunchedUsers" runat="server">
                            <ContentTemplate>
                             <table align ="center" width="100%">  
                             <tr id="tr4" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwUsersPunched">
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
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records" />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                             <td align="center">
                            <div id="tdUsers" runat="server" align="center" colspan="1">
                             <asp:ListView ID="lstvwUsersPunched" runat="server" DataKeyNames="IndexNo"
                                 OnDataBound="lstvwUsersPunched_DataBound" OnSorting="lstvwUsersPunched_Sorting" >
                                <LayoutTemplate>
                                <table width="70%" runat="server" id="tblUsersPunched" style="color: #333333" cellpadding="0"
                                cellspacing="1" class="GridBorder">
                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                    <th align="center" width="200px">
                                    <asp:LinkButton ID="lnkEmpNo1" runat="server" 
																	ForeColor="Black"> Employee No.</asp:LinkButton>
                                    </th>
                                    <th align="center" style="padding-left: 10px;"  >
                                     <asp:LinkButton ID="lnkEmpName1" runat="server" CommandName="Sort" CommandArgument="UserName"
																	ForeColor="Black"> Employee Name</asp:LinkButton>
                                    </th>
                                    <th align="center" style="padding-left: 10px;">
                                     <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="EventDateTime"
																	ForeColor="Black"> Time</asp:LinkButton>
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                                 <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                    <td colspan="3">
                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUsersPunched" PageSize="20">
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
                            <tr id="Tr21" runat="server" class="ClsGridRow">
                                <td align="center">
                                    <asp:Label ID="lblEmpNo" runat="server" EnableViewState="false" Text='<%#Eval("Employee_No") %>'></asp:Label>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:Label ID="lblEmpName" runat="server" EnableViewState="false" Text='<%#Eval("UserName") %>'></asp:Label>
                                </td>
                                <td align="center" style="padding-left: 10px;">
                                    <asp:Label ID="lbltime" runat="server" EnableViewState="false" Text='<%#Eval("EventDateTime") %>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                <td align="center">
                                   <asp:Label ID="lblEmpNo" runat="server" EnableViewState="false" Text='<%#Eval("Employee_No") %>'></asp:Label>
                                </td>
                                <td style="padding-left: 10px;">
                                   <asp:Label ID="lblEmpName" runat="server" EnableViewState="false" Text='<%#Eval("UserName") %>'></asp:Label>
                                </td>
                                <td align="center" style="padding-left: 10px;">
                                   <asp:Label ID="lbltime" runat="server" EnableViewState="false" Text='<%#Eval("EventDateTime") %>'></asp:Label>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                          <EmptyDataTemplate>
						    <tr >
							    <td class="LblNoRecord" align="center" id="tdnorecord">
								    No record found.
							    </td>
						    </tr>
					    </EmptyDataTemplate>
                      </asp:ListView>
                       <asp:ObjectDataSource TypeName="BusinessLogic.UsersPunchingDetailsBL" EnablePaging="True"
                            ID="ObjDSConfigureUsersPunched" runat="server" SelectMethod="GetAllUsersPunched" SortParameterName="sortExpression" SelectCountMethod="CountTotalUsersPunched"
                            EnableCaching="False" >
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:ControlParameter ControlID="txtDate" PropertyName="Text" Name="asSelectedDate"
                                                        DefaultValue="" />
                                <asp:ControlParameter ControlID="chkGrpUser" PropertyName="Checked" Name="abChkGroupByUser" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                            </td>
                            </tr>     
                            </table>   
                           </ContentTemplate>
                         </asp:UpdatePanel>
                        </td>
                     </tr>
                 </table>
                 </div>
             </td>
            <td width="10px">
            </td>
             <td style=" padding-top:0px; vertical-align: top;" align="center">
                <div style="width: 700px;" align="center">
                    <table width="100%" align="center" id="tblnonPunchedUsers">
                       <tr align="center" style="width:100%" >
                            <td align="center" id="tdnonpunched" style="font-weight:bold">
                               Non Checked-In Users
                            </td>
                        </tr>
                       <tr align="center">
                         <td>
                        <asp:UpdatePanel ID="UpnlListViewNotPunchedUsers" runat="server">
                        <ContentTemplate>
                       <table width="100%" align="center" >  
                             <tr id="tr5" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount1" runat="server" PageSize="20" PagedControlID="lstvwNotPunchedUsers">
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
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records" />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                          </tr>
                          <tr>
                            <td align="center">
                              <div id="Div3" runat="server" colspan="1">
                                <asp:ListView ID="lstvwNotPunchedUsers" runat="server" DataKeyNames=""
                                     OnDataBound="lstvwNotPunchedUsers_DataBound" OnSorting="lstvwNotPunchedUsers_Sorting" >
                                    <LayoutTemplate>
                                      <table width="70%" runat="server" id="tblUsersPunched" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="200px">
                                        <asp:LinkButton ID="lnkEmpNo" runat="server" CommandName="Sort" CommandArgument="Employee_No"
																	ForeColor="Black"> Employee No.</asp:LinkButton>
                                        </th>
                                        <th align="center" style="padding-left: 10px;" >
                                         <asp:LinkButton ID="lnkEmpName" runat="server" CommandName="Sort" CommandArgument="UserName"
																	ForeColor="Black"> Employee Name </asp:LinkButton>
                                        
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                        <td colspan="2">
                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwNotPunchedUsers" PageSize="20">
                                                <Fields>
                                                    <asp:TemplatePagerField>
                                                        <PagerTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt1_SelectedIndexChanged">
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
                                <tr id="Tr21" runat="server" class="ClsGridRow">
                                    <td align="center">
                                        <asp:Label ID="lblEmpNo" runat="server" EnableViewState="false" Text='<%#Eval("Employee_No") %>'></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:Label ID="lblEmpName" runat="server" EnableViewState="false" Text='<%#Eval("UserName") %>'></asp:Label>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                       <asp:Label ID="lblEmpNo" runat="server" EnableViewState="false" Text='<%#Eval("Employee_No") %>'></asp:Label>
                                    </td>
                                    <td style="padding-left: 10px;">
                                       <asp:Label ID="lblEmpName" runat="server" EnableViewState="false" Text='<%#Eval("UserName") %>'></asp:Label>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
						    <tr>
							    <td class="LblNoRecord" align="center" id="tdnorecord1">
								    No record found.
							    </td>
						    </tr>
					    </EmptyDataTemplate>
                      </asp:ListView>
                        <asp:ObjectDataSource TypeName="BusinessLogic.UsersPunchingDetailsBL" EnablePaging="True"
                            ID="ObjDSConfigureUsersNotPunched" runat="server" SelectMethod="GetAllUsersNotPunched" SortParameterName="sortExpression" SelectCountMethod="CountTotalUsersNotPunched"
                            EnableCaching="False" >
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                           
                            <asp:ControlParameter ControlID="txtDate" PropertyName="Text" Name="asSelectedDate"
                                                        DefaultValue="" />
                            <asp:ControlParameter ControlID="chkGrpUser" PropertyName="Checked" Name="abChkGroupByUser" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        </div>
                         </td>
                        </tr>   
                        </table>     
                      </ContentTemplate>
                    </asp:UpdatePanel>
                         </td>
                       </tr>
                     </table>
                </div>
             </td>
           </tr>
            <tr>
                <td align="center" colspan="3" style=" padding:20Px;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                            <asp:HiddenField ID="HidNonPunchedSortDirection" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="HidNonPunchedSortExprsn" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="HidPunchedSortDirection" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="HidPunchedSortExprsn" runat="server"></asp:HiddenField>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </td>
            </tr>
        </table>
       </div>
    </div>
    
    <script src="../Scripts/Payroll/UsersPunchingDetails.js" type="text/javascript"></script>
</asp:Content>
