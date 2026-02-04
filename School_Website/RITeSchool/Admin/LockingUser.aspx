<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="LockingUser.aspx.cs" Inherits="LockingUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        &nbsp;
        <table width="97%">
            <tr id="trPrecondition" runat="server" visible="false">
                <td>
                    <div runat="server" id="divErr">
                        
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">                
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabelNrml" EnableViewState="false" Font-Bold="true" ForeColor="Blue" Text=""></asp:Label>                    
                    </ContentTemplate>
                    <Triggers>   
                        <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />   
                        <asp:AsyncPostBackTrigger ControlID="grdUsers" EventName="PageIndexChanging" />
                        <asp:AsyncPostBackTrigger ControlID="grdUsers" EventName="RowCommand" />                        
                    </Triggers>
                    </asp:UpdatePanel>
                </td> 
            </tr>
            <tr>
                <td align="left" class="ClsBtmBorderGray">
                <table width="100%">
                <tr>
                  <td>
                    <asp:Panel ID="pnlLegend" runat="server" Width="100%">
                        <table id="LegendTable" runat="server">
                            <tr>
                                <td align="left" colspan="1">      
                                <asp:Label ID = "lblLegend" runat = "server" CssClass = "ClsLblLgnd" EnableViewState = "false" Font-Bold = "true" Text = "<%$ Resources:LocalizedResources, Legend%>" ></asp:Label>                        
                                 </td>
                                <td align="right" colspan="1">
                                    <asp:Image ID="img1" runat="server" ImageUrl="~/RITeSchool/images/Icon_UserUnlock.gif"
                                        Border="0" Width="20px" />
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID = "lblActivate" runat = "server" CssClass = "ClsTextNormal" EnableViewState = "false" Font-Bold = "true" Text = "<%$ Resources:LocalizedResources, Activate%>" ></asp:Label>                        
                                </td>
                                <td align="right" colspan="1">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/RITeSchool/images/Icon_UserLock.gif"
                                        Border="0" Width="20px" />
                                </td>
                                <td align="left" colspan="1">
                                <asp:Label ID = "lblDeactivate" runat = "server" CssClass = "ClsTextNormal" EnableViewState = "false" Font-Bold = "true" Text = "<%$ Resources:LocalizedResources, Deactivate%>" ></asp:Label>                              
                                </td>
                                <td align="right" colspan="1">
								<%if (!Settings.IsMiniSite) %>
								<%{ %>
                                    <asp:Image ID="ImageAvailable" runat="server" ImageUrl= "~/RITeSchool/images/IconGrid_Mail.jpg"
                                        Border="0" Width="20px" />
										<%} %>
                                </td>
                                <td align="left" colspan="1">
								<%if (!Settings.IsMiniSite) %>
								<%{ %>
                                    <asp:Label ID = "lblSmsmessage" runat = "server" CssClass = "ClsTextNormal" EnableViewState = "false" Font-Bold = "true" Text = "<%$ Resources:LocalizedResources, AvaliableForSMSMessage%>" ></asp:Label>                        
									<%} %>
                                </td>
                                <td align="right" colspan="1">
									<%if (!Settings.IsMiniSite) %>
								<%{ %>
                                    <asp:Image ID="ImageNotAvailable" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Mail.gif"
                                        Border="0" Width="20px" /><%} %>
                                </td>
                                <td align="left" colspan="1">
									<%if (!Settings.IsMiniSite) %>
								<%{ %>
                                     <asp:Label ID = "lblNotAvaliableForSMS" runat = "server" CssClass = "ClsTextNormal" EnableViewState = "false" Font-Bold = "true" Text = "<%$ Resources:LocalizedResources, NotAvaliableForSMSMessage%>" ></asp:Label>                        
                                    <%} %></td>
                            </tr>
                        </table>
                     </asp:Panel>
                    </td>
                    <td width="20%" align="right">
                             <span class="ClsMdtStar">*</span> <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                   </td>
                </tr>
               </table>
              </td>
            </tr>
            <tr  id="trCombo">
                <td align="left">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 25%">
                                <table width="100%">
                                    <tr id="trUserRole" runat="server">
                                        <td class="ClsBorderlight" colspan="1" style="width: 25%;">
                                            <asp:Label CssClass = "ClsLabel" ID="lblUserRole" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserRole%>"></asp:Label>
                                                     <span class="ClsLabel ">:</span>
                                         </td>
                                        <td colspan="1" style="width: 20%;">
                                            <asp:DropDownList ID="ddlUserRole" runat="server" AutoPostBack="true" Width="132px"
                                                OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                                            </asp:DropDownList><span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 15%">
                              <asp:UpdatePanel UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false" 
                                    ID="UpdatePanel4">
                                 <ContentTemplate>
                                    <asp:Panel ID="pnlForUserType" runat="server" Width="100%">
                                     <table id="Table2"  width="100%">
                                        <tr>
                                            <td class="ClsBorderlight" colspan="1" >
                                                <asp:Label CssClass = "ClsLabel" ID="Label1" runat="server" EnableViewState="False" Text="User Type"></asp:Label>
                                                <span class="ClsLabel ">:</span>
                                            </td>
                                            <td colspan="1" >
                                                <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" Width="132px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                                                        <asp:ListItem Value="2">--All--</asp:ListItem>
                                                        <asp:ListItem Selected="True" Value="0">Active</asp:ListItem>
                                                        <asp:ListItem Value="1">Deactive</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                      </table>
                                    </asp:Panel>
                                 </ContentTemplate>
                                 <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                              </asp:UpdatePanel>
                            </td>
                            <td style="width: 40%">
                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                    ID="uPnl">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlForStudent" runat="server" Visible="false" Width="100%">
                                            <table id="Table1"  width="100%">
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                       <asp:Label CssClass = "ClsLabel" ID="lblStandard" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Standard%>"></asp:Label>
                                                     <span class="ClsLabel ">:</span>
                                                        </td>
                                                    <td align="left" >
                                                        <asp:DropDownList ID="ddlStandard" runat="server" Width="132px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" class="ClsBorderlight">
                                                      <asp:Label CssClass = "ClsLabel" ID="lblDivision" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Division%>"></asp:Label>
                                                     <span class="ClsLabel ">:</span>
                                                        </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                            ID="UpdatePanel2">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlDivision" runat="server" Width="122px" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr > 
                         <td colspan="3">
                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
                        <ContentTemplate>
                                    
                                <table width="60%" align="center" id="tblSearch" runat="server">
                                    <tr>
                                           <td width = "30%" class ="ClsBorderlight">
                                                <asp:Label ID="lblSearch" runat="server" Text= "<%$ Resources:LocalizedResources, Name_RegNo_UserName%>" CssClass="ClsLabel"></asp:Label>
                                                <span class="ClsLabel">:</span>
                                            </td>
                                            <td style="width: 12%"  class="ClsBorderlight" align="center">
                                            <table>
                                                <tr>
                                                   <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength ="50" CssClass="LrgTxtBox" autocomplete="off"></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                    <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSearch" runat="server" Text= "<%$ Resources:LocalizedResources, Search%>" CssClass="ClsBtn" 
                                                            onclick="btnSearch_Click"></asp:Button>
                                                            
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click"/>
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                </table>
                                
                         </ContentTemplate>
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                         </Triggers>
                       </asp:UpdatePanel>
                                
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="True" UpdateMode="Conditional"
                        runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlUserGrid" runat="server">
                                <table  width="100%">
                                    <tr runat="server" id="trTotalRec" align="center">
                                        <td>
                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                            <asp:Label ID = "lblTO" runat = "server" CssClass = "LblNormal" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, To%>" ></asp:Label>
                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                             <asp:Label ID = "lblOutOf" runat = "server" CssClass = "LblNormal" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, OutOf%>" ></asp:Label>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                             <asp:Label ID = "lblRecords" runat = "server" CssClass = "LblNormal" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Records%>" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:GridView CssClass="GridBorder" ID="grdUsers" runat="server" AllowPaging="True"
                                                AutoGenerateColumns="False" OnRowCommand="grdUsers_RowCommand" AllowSorting="True"
                                                OnRowCreated="grdUsers_RowCreated" OnRowDataBound="grdUsers_RowDatabound" Width="100%"
                                                PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                DataKeyNames="User_Id,Is_Locked,Name,Mobile_Number,Deactivation_Reason,IsConsideredForMessage" OnSorting="grdUsers_Sorting" OnPageIndexChanging="grdUsers_PageIndexChanging">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="Roll_No" HeaderText= "<%$ Resources:LocalizedResources, RollNo%>" SortExpression="Roll_No">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="3%"/>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText= "<%$ Resources:LocalizedResources, Name%>" SortExpression="Name">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="15%" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="Mobile_Number" HeaderText= "<%$ Resources:LocalizedResources, MobileNumber%>" >
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="User_Login" HeaderText= "<%$ Resources:LocalizedResources, UserName%>"  SortExpression="User_Login">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="7%"/>
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText= "<%$ Resources:LocalizedResources, Activate_Deactivate%>" Text=""
                                                        ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" CommandName="LOCK">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="True" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, ChangePassword%>" Text="Change Password" ImageUrl="~/RITeSchool/images/Icon_ChngPass.gif">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" Width="7%" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, SendLoginSMS%>" Text="Send SMS" ImageUrl="~/RITeSchool/images/SMS Icon.png" CommandName = "SEND_SMS">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" Width="5%" />
                                                    </asp:ButtonField>  
                                                    <asp:ButtonField ButtonType="Image" HeaderText= "<%$ Resources:LocalizedResources, ActivateDeactivateSMSMessage%>" Text=""
														ImageUrl="~/RITeSchool/images/IconGrid_Mail.gif" CommandName="SMS"  >
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" Wrap="true" />
													</asp:ButtonField>
                                                    <asp:TemplateField HeaderText="Login" Visible="false">
														<ItemTemplate>
															<asp:Button ID="btnLogin" Text="Login" runat="server" CausesValidation="false" CommandName="LOGIN"
																CommandArgument="<%# Container.DataItemIndex %>" />
														</ItemTemplate>
														<ControlStyle CssClass="ClsBtnSml" />
														<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
														<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
													</asp:TemplateField>
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
                                                                <asp:Label ID="MessageLabel" Text= "<%$ Resources:LocalizedResources, SelectAPage%>" runat="server" CssClass="LblNrmlB" />
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
                                            <asp:ObjectDataSource TypeName="BusinessLogic.SchoolUserCollectionBL" EnablePaging="true"
                                                ID="GrdDSobj" runat="server" SelectMethod="GetUserDetails" SortParameterName="sortExpression"
                                                SelectCountMethod="GetCountUsers" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:ControlParameter Name="aiUserRoleId" Type="int32" ControlID="ddlUserRole" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter Name="aiUserTypeId" Type="int32" ControlID="ddlUserType"  PropertyName="SelectedValue" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="int32" />
                                                    <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                        PropertyName="Value" />
                                                    <asp:ControlParameter Name="asCriteria" Type="String" ControlID="txtSearch" PropertyName="Text" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdODStudent"
                                                runat="server" SelectMethod="GetAllCurrentStudents" SortParameterName="sortExpression"
                                                SelectCountMethod="CountCurrentStudentRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="string" />
                                                    <asp:ControlParameter ControlID="ddlUserType" Type="int32" PropertyName="SelectedValue"
                                                        Name="aiUserTypeId" />
                                                    <asp:ControlParameter ControlID="hidStandardId" Type="Int32" PropertyName="Value" DefaultValue="0"
                                                        Name="aiStandardId" />
                                                    <asp:ControlParameter ControlID="hidDivisionId" Type="Int32" PropertyName="Value" DefaultValue="0"
                                                        Name="aiDivisionId" />
                                                    <asp:ControlParameter ControlID="txtSearch" Type="String" PropertyName="Text" DefaultValue=""
                                                        Name="asName" />
                                                    <asp:Parameter Name="abIncludeUserName" DefaultValue="true" Type="Boolean" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidBackUrl" runat="server"/>
                            <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidFromUrl" runat="server" Value="" />
                            <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                             <asp:AsyncPostBackTrigger ControlID="ddlUserType" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 20px">
                    <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" OnClick="btnCancel_Click"
                         />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">

        //This function is used to display confirmation message.
        function ConfirmLocking(str)
        {      
               return window.confirm(str);  
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            var sElementId = "" + postBackElement.id;
            
            var postBackElement = sender._postBackSettings.sourceElement;
            if (sElementId.substring(sElementId.indexOf('btnLogin')) == 'btnLogin') {
                UserLogin()
            }

            AutoSearch();
        }
        //This function is used to open new window as a login widow for selected user.
        function UserLogin() {
            window.open('../Common/ControlPanel.aspx', '_blank');
            return true;
        }

        function ConfirmSmsMessage(msg) {
        	return confirm(msg);
        }
        
    </script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtSearch.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            _clientddlUserRole = '<%=ddlUserRole.ClientID%>';
            var _clientddlStandard = '<%=ddlStandard.ClientID%>';
            var _clientddlDivision = '<%=ddlDivision.ClientID%>';


            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _slienttxtUserName, _clientddlUserRole, 0, _clientddlStandard, _clientddlDivision, null);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtSearch.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function ShowAlert(msg) {
            alert(msg)
            return false;
        }

	</script>

</asp:Content>
