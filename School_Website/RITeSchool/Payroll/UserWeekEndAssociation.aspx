<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserWeekEndAssociation.aspx.cs" Inherits="RITeSchool_Payroll_UserWeekEndAssociation"  ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="server">
    <div class="MainBodyDiv" runat="server">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="800px" >
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="800px">
                        <tr>
                            <td align="right" colspan="2" style="color: #ff3333" valign="top">
                                <span class="ClsMdtStar">*
                                    <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 800px;">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trListview">
                <td align="center">
                    <table id="tblWeekendAssociation" border="0" cellpadding="1" cellspacing="1" runat="server"
                        width="600px">
                        <tr align="center">
                            <td align="center">
                                <asp:UpdatePanel ID="upnlWeekendAsso" runat="server" ChildrenAsTriggers="true" ViewStateMode="Enabled">
                                    <ContentTemplate>
                                        <table>
                                             <tr align="center">
                                                <td colspan ="3">
                                                    <asp:Label id="lblmessage" runat="server" Font-Bold="True" Height="20px" style="color: #ff3333" ></asp:Label>
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                    colspan="1">
                                                   <span id="Span1" class="ClsLabel">Staff Group :</span> 
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStaffGroup" runat="server" CausesValidation="false" CssClass="SmlCombo"
                                                        AutoPostBack="true" Height="22px" Width="200px" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged" ViewStateMode="Enabled">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnshow" Text="Show" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="true" disable-page="true" onclick="btnshow_Click"  />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderlight" style="padding-left: 10px; padding-right: 5px;"
                                                    colspan="1">
                                                    <span class="ClsLabel">Name :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="ExLrgTxtBox" Width="200px"
                                                        autocomplete="off"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="false" ValidationGroup="Search" OnClick="btnSearch_Click"/> 
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                                </td>
                                            </tr>
                                            <td colspan="3" align="center">
                                                <asp:Label ID ="lblError" runat="server" style="color: #ff3333" ></asp:Label>
                                            </td></tr>
                                            
                                            <tr align="center">
                                              <td align="center" colspan="3">
                                                <asp:UpdatePanel ID="UpnlListView" runat="server">
                                                 <ContentTemplate>
                                                 <table>                       
                                                    <tr id="tr1" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStaffGroupUsers">
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
                                                    </table>
                                                    <div id="Div1" style="overflow: auto;">
                                                        <asp:ListView ID="lstvwStaffGroupUsers" runat="server" DataKeyNames="UserId,UserName" 
                                                        OnItemDataBound="lstvwStaffGroupUsers_ItemDataBound"  
                                                             OnDataBound="lstvwStaffGroupUsers_DataBound" >
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder">
                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                        <th align="left" style="padding-left: 10px;">
                                                                            User Name
                                                                        </th>
                                                                        <th align="center">
                                                                             <asp:label ID="ChkSelectAll" runat="server" Text="Weekends Applicable" />
                                                                        </th>
                                                                    </tr>
                                                                    <tr id="itemPlaceholder" runat="server">
                                                                    </tr>
                                                                      <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                        <td colspan="7">
                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStaffGroupUsers" PageSize="20">
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
                                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                                    <td class="paddingL">
                                                                        <asp:Label ID="lblUserName" runat="server" MaxLength="100" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                    </td>
                                                                     <td align="center">
                                                                        <asp:CheckBoxList ID="ckhWeekendsList" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                                    <td class="paddingL">
                                                                        <asp:Label ID="lblUserName" runat="server" MaxLength="100" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                    </td>
                                                                     <td align="center">
                                                                        <asp:CheckBoxList ID="ckhWeekendsList" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:ListView>
                                                      </div>
                                                         <asp:ObjectDataSource TypeName="BusinessLogic.UserWeekEndAssociationBL" EnablePaging="True"
                                                            ID="ObjDSConfigureWeekend" runat="server" SelectMethod="GetAllUsersDetails" SortParameterName="sortExpression" SelectCountMethod="CountTotalUsers"
                                                            EnableCaching="False" >
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                 <asp:ControlParameter Name="aiStaffGroupId" Type="Int16" ControlID="cmbStaffGroup"
												            	PropertyName="SelectedValue" DefaultValue="0" />   
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                     </ContentTemplate>
                                                     <Triggers>
							                                <asp:AsyncPostBackTrigger ControlID="lstvwStaffGroupUsers" EventName="ItemCommand" />
                                                            <asp:AsyncPostBackTrigger ControlID="lstvwStaffGroupUsers" EventName="DataBound" />
						                             </Triggers>
                                                </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Button runat="server" ID="btnSaveWeekends" Text="Save" class="ClsBtn" Visible= "false" OnClick="btnSaveWeekends_Click"/>
                                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" 
                                                     UseSubmitBehavior="false"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    </td>
                    </tr>
              </table>
     </div>
    <script type="text/javascript" language="javascript" >
        _slienttxtUserName = '#<%=txtUserName.ClientID%>';
        _clientbtnSearch = "<%=this.btnSearch.ClientID %>";
        _clientlstvwStaffGroupUsers = "<%=this.lstvwStaffGroupUsers.ClientID %>";
    </script>
    <script src="../Scripts/Payroll/UserWeekendAssociation.js" type="text/javascript"></script>
</asp:Content>
