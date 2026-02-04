<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SubjectTestConfigurationUI.aspx.cs" Inherits="SubjectTestConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv" align="center">
    <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>
    
        <table width="97%" align="center">
            <tr>
                <td align="center" >
                   <table>
                     <tr>
                       <td align="left" class="ClsBorderLight" style="width: 150px">
                           <span class="clsLabel">Standard : </span>
                       </td>
                       <td>
                          <asp:DropDownList ID="cmbStandard" Width="121px"
                             runat="server" ViewStateMode="Enabled" CssClass="SmlCombo" Height="19px" AutoPostBack="True" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                          </asp:DropDownList>
                          <span class="ClsMdtStar">*</span>
                      </td>
                     <td align="left" class="ClsBorderLight" style="width: 150px">
                        <span class="clsLabel">Subject : </span>
                     </td>
                    <td>                    
                     <asp:DropDownList ID="CmbSubject" runat="server" ViewStateMode="Enabled" CssClass="ExLrgCombo" AutoPostBack="True" OnSelectedIndexChanged="CmbSubject_SelectedIndexChanged">
                     </asp:DropDownList>
                         <span class="ClsMdtStar">*</span>
                   </td>
                </tr>
             </table>
           </td>
        </tr>
            <tr>
                <td align="center">
                  <table id="LegendTable" runat="server">
                        <tr>
                            <td align="left" colspan="1">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                    Font-Bold="True" Text="<%$ Resources:LocalizedResources, Legend%>" EnableViewState="false"></asp:Label></td>
                            
                            <td align="right">
                                <asp:Label ID="TextBox2" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, ExamConfigurationNotDone%>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                            </td>
                            <td align="left" style="width: 5px">
                            </td>
                            <td align="left">
                                <asp:Label ID="TextBox3" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left">
                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources,SubjectNotAssignToDivision%>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="left" style="width: 5px">
                            </td>
                            <td align="left">
                                <asp:Label ID="TextBox4" runat="server" BackColor="#aae2cd" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, UpdateExamConfiguration%>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
           <tr>
                <td align="center" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" style="width: 1000pt; overflow: auto;">
                        <asp:GridView ID="grdSubjects" runat="server" AutoGenerateColumns="false" Height="100%" Width="100%"
                            PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames="Standard_Id,Schoolwise_Standard_Division_Id" EnableViewState ="false" OnRowDataBound="grdSubjects_RowDataBound">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources, Next%>" LastPageText="<%$ Resources:LocalizedResources, Last%>" PreviousPageText="<%$ Resources:LocalizedResources,Previous%>"
                                FirstPageText="<%$ Resources:LocalizedResources, First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField HeaderText="  " SortExpression="StandardDivision" DataField="StandardDivision">
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
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" 
                        OnClick="btnBack_Click" CausesValidation="False" /></td>
            </tr>
        </table>
        </ContentTemplate>
     </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdSubjects.ClientID %>"
        function saveChk(msg) {
            if (ChkIfAtleastOneCheckedInEachRow(document, _clientGridId)) {
                return true
            }
            else {
                alert(msg)
                return false
            } 
        }
        function ConfirmAction(iPageCountStandard, iPageCountDivision, sActionName) {
            var bResult = false
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCountDivision, 'true')) {
                bResult = true
            }
            else
            { bResult = false; }
            return bResult
        }
    </script>
</asp:Content>
