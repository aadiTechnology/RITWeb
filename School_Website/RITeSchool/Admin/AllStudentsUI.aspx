<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllStudentsUI.aspx.cs" Inherits="AllStudentsUI"
    MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" align="center">
        <table width="97%">
            <tr id="trStudent" runat="server">
                <td align="center" class="ClsGrayMainTitle" style="height: 20px">
                     <asp:Label ID="lblHeader" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentCount%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="1">
                    <table id="LegendTable" runat="server" style="width: 640pt;">
                        <tr>
                            <td align="left">
                                <table id="Table1" runat="server">
                                    <tr id="trLegend" runat="server">
                                        <td align="left" colspan="1" style="height: 24px; padding-right: 20px">                                          
                                         <asp:Label ID="lbllegend" runat="server" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, Legend%>"
                                                CssClass="ClsLblLgnd" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                            <asp:Label ID="TextBox1" runat="server"   Height="20px" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False" CssClass="clsLegendLbl">
                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                           <%-- <span class="ClsTextNormal" style="font-weight:bold">Legend</span>--%>
                                            <asp:Label ID="lblDivisionNotApplicable" runat="server" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, DivisionNotApplicable%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 5px; height: 24px;">
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/RITeSchool/images/GridIconSml_Add.gif"
                                                EnableViewState="False" />
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                        <asp:Label ID="lblAddStudentInClass" runat="server" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, AddStudentInClass%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 5px; height: 24px;">
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px; width: 23px;">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/RITeSchool/images/GridIconSml_View.gif"
                                                EnableViewState="False" />
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, ViewStudentsInClass%>"
                                                CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td id="td1" valign="middle">
                                <div style="width: 120px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                    class="ClsGreenBG">
                                    <asp:HyperLink ID="hlnkStudentDynmicExport" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Student/DynamicFieldDetailsUI.aspx" Text = "Dynamic Export"
                                        EnableViewState="False"></asp:HyperLink>
                                </div>
                            </td>--%>
                            <td id="tdRollNosGeneration" valign="middle">
                                <div style="width: 205px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                    class="ClsGreenBG">
                                    <asp:HyperLink ID="hlnkStudentRollNos" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Admin/RegenarateRollNoUI.aspx" Text = "<%$ Resources:LocalizedResources, Regenerate_ReassignRollNo%>"
                                        EnableViewState="False"></asp:HyperLink>
                                </div>
                            </td>
                            <td id="hyperlnk" valign="middle">
                                <div style="width: 125px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                    class="ClsGreenBG">
                                    <asp:HyperLink ID="HyperLink3" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Admin/ImportStudentUI.aspx" Text = "<%$ Resources:LocalizedResources, ImportStudent%>"
                                        EnableViewState="False"></asp:HyperLink>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divGridView" runat="server" style="width: 635pt; height: 100%;">
                        <asp:GridView CssClass="GridBorder" ID="grdStandards" runat="server" AutoGenerateColumns="False"
                            PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                            DataKeyNames="Standard_Id,Original_Standard_Id" EnableViewState="False">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField HeaderText="  " SortExpression="Standard_Name" DataField="Standard_Name">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="div1" runat="server" style="width: 635pt; height: 100%;">
                        <table width="100%" align="center">
                            <tr align="center">
                                <td align="left">
                                    <div style="width: 140px; height: 18px;  padding-top: 4px"
                                        class="ClsGreenBG" id="hlnkHouseAssignmentDiv" runat="server">
                                        <asp:HyperLink ID="hlnkHouseAssignment" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Admin/StudentsHouseAssignmentUI.aspx"
                                            EnableViewState="False">House Assignment</asp:HyperLink>
                                            
                                    </div>
                                    
                                </td>
                                <td align="left">
                                <div style="width:145px; height: 18px; padding-top: 4px"
                                        class="ClsGreenBG"  runat="server">
                                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Student/LeftStudentsDetailsUI.aspx"  Text = "Left Student Details"
                                            EnableViewState="False"></asp:HyperLink>
                                    </div>
                                </td>
                                <td align="left">
                                 
                                    <asp:Button ID="btnBack" runat="server" Text= "<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" OnClick="btnBack_Click"
                                        UseSubmitBehavior="false" CausesValidation="False" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnStudent" runat="server" Text= "<%$ Resources:LocalizedResources, SearchStudent%>" CssClass="ClsBtn"
                                        UseSubmitBehavior="false" Width="120px" />
                                   
                                </td>
                                <td align="left" >
                                    <div style="width: 118px; height: 18px; padding-top: 4px"
                                        class="ClsGreenBG" id="hyperlnkSanctionLeave" runat="server">
                                        <asp:HyperLink ID="HyperLinkSanctionLeave" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Student/StudentSanctionedLeaveDetailsUI.aspx" Text = "<%$ Resources:LocalizedResources, SanctionLeave%>"
                                            EnableViewState="False"></asp:HyperLink>
                                    </div>
                                </td>
                                <td align="left">
                                    <div style="width: 200px; height: 18px;  padding-top: 4px;
                                        margin-left: 3px;" class="ClsGreenBG" id="hyperLnkSecondLanguage" runat="server">
                                        <asp:HyperLink ID="HyperLink2" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Admin/SecondLanguageUI.aspx"  Text = "Set Second / Third Language"
                                            EnableViewState="False"></asp:HyperLink>
                                    </div>
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="left" colspan="2">
                                    <div style="height: 18px;  padding-top: 4px;" class="ClsGreenBG" id="Div3" runat="server">
                                        <asp:HyperLink ID="hlnkStudentAdditionalDetails" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Admin/UpdateStudentDetailsInBulkUI.aspx"  Text = "Update Students Additional Details"
                                            EnableViewState="False"></asp:HyperLink>
                                    </div>
                                </td>
                                <td colspan="3">
                                </td>
                                 <td align="right">                                    
                                    <div style="width: 210px; height: 18px;  padding-top: 4px;" class="ClsGreenBG" id="Div2" runat="server">
                                        <asp:HyperLink ID="HyperLink4" runat="server" CssClass="SubTitle" NavigateUrl="~/RITeSchool/Student/StudentBulkEmailUI.aspx"  Text = "Update Students Email In Bulk"
                                            EnableViewState="False"></asp:HyperLink>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                              <td align="center" colspan="3">                                    
                                    <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" Text= "<%$ Resources:LocalizedResources, Close%>" CausesValidation="false"
                                        Visible="false" UseSubmitBehavior="false" OnClientClick="window.close();" Style="margin-left: 5px;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>               
            </tr>
        </table>
        <asp:HiddenField ID="hidCanEdit" runat="server" Value="N" />
        <asp:HiddenField ID="hidIsManagmentUser" runat="server" Value="N" />
        <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
        <asp:HiddenField ID="hidTotalCount" runat="server" />
         <asp:HiddenField ID="hidHouseConId" runat="server" Value = "0" ViewStateMode="Enabled" />
  </div>
  <script type="text/javascript">
      _clientbtnSubmit = "<%=this.grdStandards.ClientID%>"
      var id = $('_clientbtnSubmit tr:last').attr('id');
      document.getElementById(id).firstChild.nodeValue =document.getElementById("<%=this.hidTotalCount.ClientID %>").value
  </script>
 </asp:Content>
