<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubjectsSortOrderPopUp.aspx.cs"
    Inherits="SubjectsSortOrderPopUp" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
   <div class="MainBodyDiv" style="vertical-align: top">
         <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top" width="100%" height="400px">
            <tr style="height: 10px" >
                <td align="left"  colspan="2" rowspan="1" >
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                          <td style="height: 10px">
                           <span class="MainTitleHead" style="font-weight:bold">
                               <asp:Label ID="lblSubjectSortOrder" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSortOrder %>"></asp:Label> 
                           </span></td>
                        </tr>
                    </table>
                  </td>
            </tr>
            <tr align="center" valign="top" style="height: 10px">
                <td align="right" valign="top"> <span style="color: red; font-family: Arial" enableviewstate="false" class="ClsMdtStar">
                                * <asp:Label ID="lblMandatoryField" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label> </span>
                </td>
            </tr>
            <tr align="center" valign="middle" style="height: 10px">
                <td>
                    <asp:Label ID="lblSuccess" runat="server" ForeColor="Blue" Height="20px" 
                        Width="100%" Visible="true" EnableViewState="true" CssClass="ClsLabel"></asp:Label>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="bottom" align="center" style="height: 15px;padding-bottom:5px">
                    <table style="vertical-align: top">
                        <tr >
                            <td align="center" colspan="1" class="ClsOnlyBorderlght">
                                <span class="ClsLabel">
                                    <asp:Label ID="lblStandard" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label> 
                                     <span id="Span1" class="colonPadding">:</span>
                                </span>
                                <span class="ClsMdtStar" style="color: #ff0000"></span>
                            </td>
                            <td align="center" colspan="1" class="">
                                <asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                    CssClass="SmlCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center" valign="top">
                <td valign="top" align="center">
                    <div id="div1" class="GridBorder" style="text-align:center; width: 50%; vertical-align:top; overflow: auto;">
                        <asp:GridView ID="grdSubjects" runat="server" style="vertical-align:top" Width="100%"  AutoGenerateColumns="False"
                            OnRowDataBound="grdGroupDetails_RowDataBound" PageSize="100" CellPadding="0"
                            CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Subject_Id"
                             EmptyDataText="<%$ Resources:LocalizedResources, NoRecordFound %>" OnRowCreated="grdSubjects_RowCreated">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="Subject_Name" HeaderText="<%$ Resources:LocalizedResources, SubjectName %>">
                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, SortOrder %>">
                                    <ItemTemplate>
                                        <select id="ddlOrder" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                        </asp:GridView>
                    </div>
                    <div style="padding-top:5px"> 
                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="btnSave_Click" UseSubmitBehavior="false" /> &nbsp;
                     <asp:Button ID="btnCancel"
                            runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False"
                            UseSubmitBehavior="false" />                    
                    <tr align="center" valign="top" height="40%">
                <td valign="top" align="center">
                    &nbsp;</tr>
                </div>
                </td>
            </tr>
        </table>
    </div>
       <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
    <script language="javascript" type="text/javascript">
        _clientGridId = "<%=this.grdSubjects.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons() {
            window.close()
        }
    </script>
</asp:Content>
