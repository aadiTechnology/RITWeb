<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="SchoolActivationUI.aspx.cs" Inherits="SchoolActivationUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center" valign="top">
                                <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                    Font-Size="Small" ForeColor="Blue" Visible="false" EnableViewState="false"></asp:Label>
                            </td>
                         </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td style="height: 20px">
                                            <%--<asp:Label ID="lblPageHeader" runat="server" BorderWidth="0px" Class="MainTitleHead">Activate School</asp:Label>--%>
                                            <span id="lblPageHeader" class="MainTitleHead">Activate School</span></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4" valign="top">
                                        <table style="width: 100%" cellspacing="1" cellpadding="0" border="0">
                                            <tbody>                                             
                                                <tr>
                                                    <td align="right" colspan="2" style="width: 50%" valign="middle">
                                                        <%--<asp:Label ID="lblHeadTotal" runat="server" Text="Total Record(s) :" CssClass="LblNrmlB"
                                                            Visible="true" Font-Bold="True"></asp:Label>--%>
                                                            <span id="lblHeadTotal" class="LblNrmlB" style="font-weight:bold">Total Record(s) :</span>                                                        
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 50%" valign="middle">
                                                        <asp:Label ID="lblTotalCount" runat="server" CssClass="ClsLabel"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td  valign="top" align="center" colspan="4">
                                                        <asp:GridView CssClass="GridBorder" ID="grdvwSchoolList" runat="server" ForeColor="#333333" OnRowDataBound="grdvwSchoolList_RowDataBound"
                                                            OnPageIndexChanging="grdvwSchoolList_PageIndexChanging" OnRowCreated="grdvwSchoolList_RowCreated"
                                                            AllowSorting="True" OnSorting="grdvwSchoolList_Sorting" GridLines="None" CellSpacing="1"
                                                            CellPadding="0" PageSize="20" AutoGenerateColumns="False" AllowPaging="True"
                                                            Width="100%" OnRowCommand="grdvwSchoolList_RowCommand">
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                                              CssClass="LblNormal" ></PagerStyle>
                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                            <Columns>
                                                                <asp:BoundField DataField="School_Name" HeaderText="School Name" SortExpression="School_Name">
                                                                    <ItemStyle HorizontalAlign="Left" Width="35%" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"/>
                                                                </asp:BoundField>
                                                                <asp:ButtonField HeaderText="Active/ DeActivate" ButtonType="Image" CommandName="ACTIVATE_ROW">
                                                                    <ItemStyle HorizontalAlign="Center" Wrap="True" Width="15%" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:ButtonField>
                                                                <asp:TemplateField HeaderText="Allowed SMS" SortExpression="AllowedSMS_Count">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox CssClass="SmlTxtBox" ID="nTxtAllowedSmS" runat="server" MaxLength="5" Width ="40px"
                                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            Text='<%# Eval("AllowedSMS_Count") %>' onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false"/>
                                                                    </ItemTemplate><ItemStyle HorizontalAlign="Center" Width="13%" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Exceeded SMS" DataField="SentSMS_Count" SortExpression="SentSMS_Count">
                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Subscription Date" SortExpression="Subscription_Date"
                                                                    DataField="Subscription_Date">
                                                                    <ItemStyle HorizontalAlign="Center" Wrap="True" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Is_Active" HeaderText="Is_Active" SortExpression="Is_Active" />
                                                                <asp:BoundField DataField="School_Id" HeaderText="School_Id" SortExpression="School_Id" />
                                                                <asp:ButtonField Text="Login" CommandName="login" ButtonType="Button" HeaderText="Login"  >
                                                                    <ControlStyle CssClass="ClsBtnSml" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="4">
                                                        <table width="100%">                                                            
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnClose" runat="server" Text="Back" CssClass="ClsBtnSml" BorderWidth="1px"
                                                                            BorderStyle="Solid" Visible="True" OnClick="btnClose_Click"></asp:Button>
                                                                        <asp:Button ID="btnSave" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                                                            OnClick="btnSave_Click" Text="Save" Visible="True" />                                                                        
                                                                      </td>
                                                                </tr>                                                            
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; height: 20px" align="left">
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidIsActive" runat="server" />
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

    <script type="text/javascript" language="javascript">
        _clienthidIsActive = "<%=this.hidIsActive.ClientID %>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        function EndReqHandler(sender, args) {
            var ActiveFlag = document.getElementById(_clienthidIsActive).value
            if (ActiveFlag == "Y") {
                document.getElementById(_clienthidIsActive).value = ""
                alert("School is Activated !!")
            }
            else if (ActiveFlag == "N") {
                document.getElementById(_clienthidIsActive).value = ""
                alert("School is Deactivated !!")
            }
            else
                document.getElementById(_clienthidIsActive).value = ""
        }
        function ConfirmAction(msg) {
            var bResult = true
            if (!window.confirm(msg))
                bResult = false
            return bResult
        }
</script>
</asp:Content>
