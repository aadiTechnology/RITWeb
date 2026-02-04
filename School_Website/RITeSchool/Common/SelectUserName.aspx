<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    CodeFile="SelectUserName.aspx.cs" Inherits="SelectUserName" ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
        <tr>
            <td style="background-color: white;" id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table style="width: 100%;" cellpadding="0" cellspacing="2" id="TABLE1" onclick="return TABLE1_onclick()">
                    <tr>
                        <td align="left" colspan="3">
                            <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                                padding-right: 5px;">
                                <tr>
                                    <td style="height: 20px">
                                        <asp:Label ID="lblSelectUser" runat="server" Font-Bold="True" Text="Select User To Send Message"
                                            EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 5px">
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trSPSType" runat="server" visible = "false">
                        <td colspan="3" align="right">
                            <table align="center">
                                <tr align="center" style="text-align:center;">
                                     <td class="ClsBorderlight">
                                         <span class="ClsLabel">Type :</span>
                                     </td>
                                     <td align="left">
                                         <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" 
                                             ViewStateMode="Enabled" onselectedindexchanged="cmbType_SelectedIndexChanged">
                                            <asp:ListItem Value = "0" Text="-- All --"></asp:ListItem>
                                            <asp:ListItem Value = "1" Text="Boarding"></asp:ListItem>
                                            <asp:ListItem Value = "2" Text="DayBoarding"></asp:ListItem>
                                         </asp:DropDownList>
                                     </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server" ViewStateMode="Enabled"
                                ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table id="tblForStudentDiv" runat="server" align="center" cellpadding="0" cellspacing="2">                                        
                                        <tr id="trClassDetails" runat="server">
                                            <td class="ClsBorderlight" id="tdrdoStdDiv" visible="false">
                                                <asp:RadioButton ID="rdoStdDiv" GroupName="StudentListFilter" Checked="true" runat="server" ViewStateMode="Enabled"
                                                    AutoPostBack="True" OnCheckedChanged="rdoStdDiv_CheckedChanged" />
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">Class :</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDListStdDiv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDListStdDiv_SelectedIndexChanged" ViewStateMode="Enabled">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trRegFilter" runat="server" visible="false">
                                            <td class="ClsBorderlight">
                                                <asp:RadioButton ID="rdoStudentReg" GroupName="StudentListFilter" runat="server" 
                                                    AutoPostBack="True" OnCheckedChanged="rdoStudentReg_CheckedChanged" />
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="1">
                                                <span class="ClsLabel">Name / Reg. No. : </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" Enabled="False" autocomplete="off" ></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" OnClick="btnSearch_Click"  />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Button Text="Ok" ID="imgBtnOKUp" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" />
                            <asp:Button Text="Close" ID="btnCloseUp" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Always" runat="server" ID="uPnl">
                                <ContentTemplate>
                                    <asp:Label ID="lblNote" runat="server" Text="" CssClass="clsLabel" Visible="false"></asp:Label>
                                    <asp:GridView CssClass="GridBorder" ID="grdvwSelectUser" Width="100%" runat="server"
                                        AutoGenerateColumns="False" CellPadding="0" GridLines="None" CellSpacing="1"
                                        AllowSorting="True" OnSorting="grdvwSelectUser_Sorting" OnRowDataBound="grdvwSelectUser_RowDataBound"
                                        OnRowCreated="grdvwSelectUser_RowCreated" BackColor="White" ForeColor="#333333"
                                        DataKeyNames="ID,Name,OriginalName" ViewStateMode="Enabled">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientGridId,this,'ChkBoxSelect')" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkBoxSelect" runat="server" />
                                                    <asp:HiddenField ID="HidStudentCount" Value='<%# Eval("StudentCount")%>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="True" Width="20px" />
                                                <HeaderStyle Wrap="True" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="ClspaddingL" HorizontalAlign="Left" Wrap="False" />
                                                <ItemStyle CssClass="ClspaddingL" HorizontalAlign="Left" Wrap="False" />
                                            </asp:TemplateField>
                                             <asp:BoundField DataField="OriginalName" HeaderText="Member / Child Name"
                                                HtmlEncode="false">
                                                <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" Width="50%" />
                                                <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle"
                                                    Width="50%" />
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="ID">
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundField>--%>
                                        </Columns>
                                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                        <RowStyle CssClass="ClsGridRow" />
                                        <PagerStyle ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" Position="Top"
                                            PreviousPageText="Previous" />
                                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="hidSortDirection" runat="server"  />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSelectedUserId" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidSelectedUserIdCc" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidIsIndivisualStudentId" Value='N' runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidSelectedUserNames" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidUserIds" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidUserIdsCc" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidUserNames" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidUserNamesCc" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False"/>
                                    <asp:HiddenField ID="hidUserCount" runat="server" Value="N" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidGrdViewSelectUserId" runat="server" Value="N" />
                                    <asp:HiddenField ID="hidIsCc" runat="server"  Value="0" ViewStateMode="Enabled"/>
                                     <asp:HiddenField ID="hidSelectedUserNamesCc" runat="server" ViewStateMode="Enabled"/>
                                     <asp:HiddenField ID="hidIds" runat="server" ViewStateMode="Enabled" Value=""/>
                                     <asp:HiddenField ID="hidIsLeftStudents" runat="server" ViewStateMode="Enabled" Value="N"/>
                                     <asp:HiddenField ID="hidIsPTAMember" runat="server" ViewStateMode="Enabled" Value="N"/>
                                    </td> </tr> </table>
                                    <!-- Data Insert End Here -->
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DDListStdDiv" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="imgBtnOk" Text="Ok" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" src="../Scripts/Validations.js"></script>

    <script language="javascript" type="text/javascript">
        _clientimgBtnOk = "<%=this.imgBtnOk.ClientID%>"
        _clientbtnClose = "<%=this.btnClose.ClientID%>"
        _clientimgBtnOKUp = "<%=this.imgBtnOKUp.ClientID%>"
        _clientbtnCloseUp = "<%=this.btnCloseUp.ClientID%>"
        _clientSelectedUserId = "<%=this.hidSelectedUserId.ClientID%>"
        _clientSelectedUserIdCc = "<%=this.hidSelectedUserIdCc.ClientID%>"
        _clienthidUserIds = "<%=this.hidUserIds.ClientID%>"
        _clienthidUserNames = "<%=this.hidUserNames.ClientID%>"
        _clienthidUserIdsCc = "<%=this.hidUserIdsCc.ClientID%>"
        _clienthidUserNamesCc = "<%=this.hidUserNamesCc.ClientID%>"
        _clienthidSelectedUserNames = "<%=this.hidSelectedUserNames.ClientID%>"
        _clienthidSelectedUserNamesCc = "<%=this.hidSelectedUserNamesCc.ClientID%>"
        _clientCmbStdId = "<%=this.DDListStdDiv.ClientID%>"
        _clientGridId = "<%=this.grdvwSelectUser.ClientID%>"
        _clienthidIsIndivisualStudentId = "<%=this.hidIsIndivisualStudentId.ClientID%>"
        _clienthidGrdViewSelectUserId = "<%=this.hidGrdViewSelectUserId.ClientID%>"
        _clienthidIsCc = "<%=this.hidIsCc.ClientID%>"
        _clientxtName = "<%=this.txtName.ClientID%>"
        _clienmiSchoolId = "<%=miSchoolId%>"
        _clienmiAcademicYearId = "<%=miAcademicYearId%>"
        _clienbtnSearch = "<%=this.btnSearch.ClientID%>"
        _clienthidIds = "<%=this.hidIds.ClientID %>"
        
    </script>
    <script src="../Scripts/Common/SelectUserName.js?version=1.0" type="text/javascript"></script>
</asp:Content>
