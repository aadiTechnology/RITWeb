<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="StandardExamScheduleConfigurationUI.aspx.cs" Inherits="StandardExamScheduleConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" align="center">
        <table width="97%"  align="center">
            <tr>
                <td align="center" colspan="1">
                    <table id="LegendTable" runat="server" cellspacing="2" cellpadding="2">
                        <tr>
                            <td align="left" colspan="1" style="height: 34px">
                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                    Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label></td>
                            <td align="left" colspan="1" style="padding-left: auto; padding-right: 3px; height: 34px;">
                                <asp:Label ID="TextBox1" Text= "<%$ Resources:LocalizedResources, NA%>" runat="server" CssClass="ClsGridNA" Height="20px"
                                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="50px" EnableViewState="False"></asp:Label>
                            </td>
                            <td align="left" colspan="1" style="height: 34px">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, ExamNotApplicable %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                            <td align="right" style="width: 5px; height: 34px;">
                            </td>
                            <td align="right" style="height: 34px">
                                <asp:Label ID="TextBox3" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left" style="height: 34px">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources,ScheduleNotConfigured %>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                            </td>
                            <td align="left" style="width: 5px; height: 34px;">
                            </td>
                            <td align="left" style="height: 34px">
                                <asp:Label ID="TextBox5" runat="server" BackColor="#aae2cd" Height="20px" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" height="20px" width="20px" /></asp:Label></td>
                            <td align="left" style="height: 34px">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources,EditExamSchedule%>"
                                    CssClass="ClsTextNormal" EnableViewState="false"></asp:Label></td>
                             <td align="left"  style="height: 30px; width:130px;" >
                                <asp:LinkButton ID="lnkbtnExamSchedulePopUp" runat="server" Font-Bold="True" ForeColor="Purple"
                                     Text="<%$ Resources:LocalizedResources,ViewExamSchedule%>"  BorderWidth="1px" Width="150px" Height="22px" BorderColor="black" 
                                CssClass="ClsHilightTextB ToprLinkHlilight"  EnableViewState="false" Font-Underline="True"> </asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
                <td align="left" colspan="1"> 
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" visible="true" runat="server" id="tdGrid">
                    <div id="GridViewScrollContainer" class="GridBorder" style="width: 635pt; overflow: scroll;" runat="server">
                        <asp:GridView ID="grdStandards" UseAccessibleHeader="true" runat="server" AutoGenerateColumns="False"
                           AllowPaging="False" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None" DataKeyNames="Standard_Id " OnRowDataBound="grdStandards_RowDataBound" EnableViewState="False">
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <PagerSettings NextPageText="<%$ Resources:LocalizedResources,Next%>" LastPageText="<%$ Resources:LocalizedResources,Last%>" PreviousPageText="<%$ Resources:LocalizedResources,Previous%>"
                                FirstPageText="<%$ Resources:LocalizedResources,First%>" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField  SortExpression="Standard_Name" DataField="Standard_Name">
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
                <td align="center" colspan="2">
                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources,Back%>" CssClass="ClsBtn"
                        UseSubmitBehavior="false" CausesValidation="False" 
                        onclick="btnBack_Click" />
                    <asp:HiddenField ID="hidPostbackUrl" runat="server" Value="" />  
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />  
                </td>
            </tr>
        </table>        
    </div>
    <script type="text/javascript" language="javascript">
        function GetExamScheduleInformation(sQryStr) {
            _sClientlnkSchedule = "<%=this.lnkbtnExamSchedulePopUp.ClientID %>";
            if ((document.getElementById(_sClientlnkSchedule) == null) || (document.getElementById(_sClientlnkSchedule) == "") || (document.getElementById(_sClientlnkSchedule).disabled))
                return false;
            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,height=600, width=850');
            return false;
        }
        
    </script>
</asp:Content>
