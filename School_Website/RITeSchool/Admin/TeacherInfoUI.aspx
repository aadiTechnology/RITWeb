<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TeacherInfoUI.aspx.cs" Inherits="TeacherInfoUI" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 97%; height: 100%">
                        <tr>
                            <td align="left" colspan="4" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <tr>
                                                <td colspan="1" style="vertical-align: text-top">
                                                    <asp:Label ID="lblError" ForeColor="Red" runat="server" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                                </td>
                                            </tr>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <tr>
                                        <td align="center" colspan="3" rowspan="">
                                        </td>
                                    </tr>
                                    <tr id="trLegend" runat="server">
                                        <td align="left" colspan="3">
                                            <table id="LegendTable" runat="server">
                                                <tr>
                                                    <td align="left">
                                                    <asp:Label ID="lblSelectDate" class="ClsLblLgnd" EnableViewState="false" BorderWidth="0px" Font-Bold="True"
                                                             runat="server" Text="<%$ Resources:LocalizedResources, Legend %>" />
                                                            <span id="Span1" class="ClsLblLgnd colonPadding">:</span>
                                                    </td>
                                                    <td align="left" style="padding-right: 3px">
                                                        <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                            BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" PaddingLeft="1px" Text="<%$ Resources:LocalizedResources, DeactivatedUser %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>                                                   
                                                    <td align="left" style="padding-right: 3px">
                                                        <asp:Label ID="Label2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                            BackColor="Pink" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label3" runat="server" CssClass="ClsTextNormal" Font-Bold="True" PaddingLeft="1px" Text="Transfered User"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                     <td align="left" style="padding-right: 3px">
                                                        <asp:Label ID="Label1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                            BackColor="Turquoise" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label4" runat="server" CssClass="ClsTextNormal" Font-Bold="True" PaddingLeft="1px" Text="Left User"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3" valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr id="trSearch" runat="server">
                                                            <td align="center" valign="middle">
                                                                <table width="50%">
                                                                    <tr align="center">
                                                                        <td align="left" class="ClsBorderlight">
                                                                        <asp:Label runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, TeacherName %>"></asp:Label>
                                                                         <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtName" TabIndex="1" runat="server" MaxLength="50" CssClass="MidTxtBox" autocomplete="off"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" TabIndex="2" CssClass="ClsBtnMid remove-margin-top"
                                                                                OnClick="btnSearch_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight">
                                                                                    <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="User Type"></asp:Label>
                                                                                     <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                         <td align="left">                                                      
                                                                                <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" Width="132px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">                                                                                        
                                                                                </asp:DropDownList>                                             
                                                                         </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>                                                        
                                                        <tr>
                                                         <td>
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" CssClass="LblErrorMsg" 
                                                                EnableViewState="false"></asp:Label></asp:Panel>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>                                                        
                                                        <tr id="trTeacherTable" runat="server">
                                                            <td align="center" valign="top">
                                                                <table>
                                                                    <tr runat="server" id="trTotalRec" align="center">
                                                                        <td >
                                                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />                                                                            
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To %>"
                                                                EnableViewState="false" />
                                                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf %>"
                                                                EnableViewState="false" />
                                                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records %>"
                                                                EnableViewState="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:GridView DataSourceID="GrdDSobj" CssClass="GridBorder" ID="grdvwTeacherDetails"
                                                                    runat="server" Width="100%" AllowPaging="True" AutoGenerateColumns="False" PageSize="5"
                                                                    CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" AllowSorting="True"
                                                                    OnRowCreated="grdvwTeacherDetails_RowCreated" OnSorting="grdvwTeacherDetails_Sorting"
                                                                    OnPageIndexChanging="grdvwTeacherDetails_PageIndexChanging" OnRowDataBound="grdvwTeacherDetails_RowDataBound"
                                                                    DataKeyNames="User_Id,Teacher_Id,Designation_Id,Is_Locked,WorkingStatusId" OnRowCommand="grdvwTeacherDetails_RowCommand"
                                                                    OnDataBound="grdvwTeacherDetails_DataBound" TabIndex="3">
                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                    </PagerStyle>
                                                                    <Columns>
                                                                        <asp:HyperLinkField DataNavigateUrlFields="User_Id,Teacher_Id" DataNavigateUrlFormatString="~/RITeSchool/Admin/TeacherDetailsPopUp.aspx?UserId={0} &amp;TeacherId={1} "
                                                                            DataTextField="Teacher_Name" HeaderText="<%$ Resources:LocalizedResources, TeacherName %>" SortExpression="Teacher_First_Name">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        </asp:HyperLinkField>
                                                                        <asp:BoundField DataField="Teacher_Designation_Name" HeaderText="<%$ Resources:LocalizedResources, Designation %>" SortExpression="Designation_Id">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Date_of_Birth" HeaderText="<%$ Resources:LocalizedResources, DateOfBirth %>" SortExpression="Date_of_Birth">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Qualification_Name" HeaderText="<%$ Resources:LocalizedResources, Qualification %>">
                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ClassAssign" HeaderText="<%$ Resources:LocalizedResources, AssignedClass %>">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, Edit %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                            Text="<%$ Resources:LocalizedResources, Edit %>">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:ButtonField>
                                                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, ClassSubjects %>" ImageUrl="~/RITeSchool/images/IconGrid_Subjects.gif"
                                                                            Text="Button">
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
                                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:ButtonField>                                                                        
                                                                        <asp:ButtonField ButtonType="Image" HeaderText="<%$ Resources:LocalizedResources, StdSubAssignment %>" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                            Text="<%$ Resources:LocalizedResources, StdSubAssignmentTooltip %>">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:ButtonField>
                                                                          <asp:ButtonField ButtonType="Image" HeaderText="Additional Details" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                            Text="Additional Details">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:ButtonField>
                                                                        <asp:ButtonField ButtonType="Image" CommandName="DELETE_TEACHER" HeaderText="<%$ Resources:LocalizedResources, Delete %>"
                                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Text="<%$ Resources:LocalizedResources, Delete %>">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                    <PagerTemplate>
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td width="70%" align="left" class="ClsBorderPager" valign="middle">                                                                                                
                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage %>" runat="server" CssClass="LblNrmlB" />
                                                                                    <span id="Span6" class="LblNrmlB colonPadding">:</span>
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
                                                                    ID="GrdDSobj" runat="server" SelectMethod="GetUserAsTeacherDetails" SortParameterName="sortExpression"
                                                                    SelectCountMethod="CountTeachers" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="string" />
                                                                        <asp:ControlParameter Name="asFilter" ControlID="hidFilter" Type="String" />
                                                                        <asp:ControlParameter Name="asUserType" ControlID="ddlUserType" propertyname="SelectedValue" Type="String" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <div runat="server" id="divErr">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Td1" align="center" valign="middle" enableviewstate="false">
                                                                <asp:Button ID="btnCancel" Text="Cancel" CssClass="ClsBtnSml" BorderStyle="Solid"
                                                                    runat="server" BorderWidth="1px" Visible="False" CausesValidation="False" TabIndex="8"
                                                                    UseSubmitBehavior="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr id="trButtons" runat="server">
                                        <td id="tdButton" runat="server" align="center" style="width: 100%;" valign="middle"
                                            colspan="3">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBack" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtnSml" BorderStyle="Solid" runat="server"
                                                        BorderWidth="1px" UseSubmitBehavior="false" CausesValidation="False" TabIndex="4" />
                                                    
                                                    <asp:Button ID="btnAddTeacher" CssClass="ClsBtnMid" BorderStyle="Solid" runat="server"
                                                        Text="<%$ Resources:LocalizedResources, AddTeacher%>" BorderWidth="1px" UseSubmitBehavior="false" 
                                                        TabIndex="5" />
                                                    <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:LocalizedResources, UploadPhoto%>" TabIndex="6" CssClass="ClsBtnMid" />
                                                    <asp:Button ID="btnIdentityCard" runat="server" Text="<%$ Resources:LocalizedResources, IdentityCard%>" TabIndex="7"
                                                        CssClass="ClsBtnMid" Visible="false"/>
                                                        <asp:Button ID="btnExport" runat="server" Text="Export" TabIndex="8" 
                                                        CssClass="ClsBtnMid" onclick="btnExport_Click"/>
                                                </ContentTemplate>
                                                <Triggers>
                                                <asp:PostBackTrigger ControlID="btnExport" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 1%; height: 20px">
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                    <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:HiddenField ID="hidHeadMFlag" runat="server" />
                                            <asp:HiddenField ID="hidIsConfigure" runat="server" />
                                            <asp:HiddenField ID="hidCanEdit" runat="server" />
                                            <asp:HiddenField ID="hidAreYouSureToDeleteThisRecords" runat="server" />
                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                            <asp:HiddenField ID="hidUserRoleId" runat="server" />
                                        </td>
                                        <td align="left" style="width: 23%; height: 20px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientbtnAddTeacher = "<%=this.btnAddTeacher.ClientID %>"
        _clientbtnBack = "<%=this.btnBack.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if (window.confirm(document.getElementById("<%=this.hidAreYouSureToDeleteThisRecords.ClientID %>").value)) {
                bResult = true
                
            }
            else
                bResult = false
            return bResult
        }

        function ShowIdentities(sQryStr) {
            _sClientbtnIdentityCard = "<%=this.btnIdentityCard.ClientID %>";
            if ((document.getElementById(_sClientbtnIdentityCard) == null) || (document.getElementById(_sClientbtnIdentityCard) == "") || (document.getElementById(_sClientbtnIdentityCard).disabled))
                return false;

            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,height=600, width=850');
            return false;
        }

        function ShowPhotos(sQryStr) {
            _sClientbtnUpload = "<%=this.btnUpload.ClientID %>";
            if ((document.getElementById(_sClientbtnUpload) == null) || (document.getElementById(_sClientbtnUpload) == "") || (document.getElementById(_sClientbtnUpload).disabled))
                return false;

            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,height=600, width=850');
            return false;
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
            var UserRole = '<%=hidUserRoleId.ClientID%>';
            $get(UserRole).value = 2;
            
            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, UserRole, 0);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
