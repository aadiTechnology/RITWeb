<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TeacherSubjectListUI.aspx.cs" Inherits="TeacherSubjectListUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>

    
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <script type="text/javascript" language="javascript">
    </script>

    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                        <tr>
                            <td align="center" colspan="4" valign="top">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 80%" cellspacing="1" cellpadding="0" border="0">
                                            <tbody>
                                                <tr id="trClassTeacher" runat="server">
                                                    <td align="left" colspan="6">                                                  
                                                        <table cellpadding="0" cellspacing="1"  style="width: auto"  >
                                                            <tr>
                                                                <td align="left" class="ClsHilightText ClsBorderlight" style="width: auto">                                                                  
                                                                         <span style="font-family:Arial;font-size:10pt">Class Teacher :</span>
                                                                </td>
                                                                <td align="left"  class="ClsHilightBGB" style="width: auto">
                                                                    <asp:Label ID="lbClassTeacherName" runat="server" Font-Size="10pt" Width="100%"
                                                                        Font-Names="Arial"></asp:Label></td>
                                                               <td align="left">
                                                                </td>                                                             
                                                            </tr>
                                                        </table>                                                    
                                                    </td>
                                                </tr>                                               
                                                <tr>
                                                    <td align="center" colspan="8" valign="top">
                                                        <asp:GridView CssClass="GridBorder" ID="grdvwTeacherSubjects" runat="server"
                                                        DataKeyNames ="User_Id,TeacherName,TeacherUserId"
                                                            ForeColor="#333333" OnRowCreated="grdvwTeacherSubjects_RowCreated"
                                                            AllowSorting="False" OnSorting="grdvwTeacherSubjects_Sorting" 
                                                            GridLines="None" 
                                                            CellSpacing="1" CellPadding="0" PageSize="20" AutoGenerateColumns="False" Width="100%"
                                                            EmptyDataText="Teachers not yet associated." 
                                                            onrowcommand="grdvwTeacherSubjects_RowCommand" >
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                                                Font-Size="Small"></PagerStyle>
                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                            <Columns>
                                                                <asp:BoundField DataField="TeacherName" HeaderText="Teacher Name" >
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="ClspaddingL" Width="30%" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="ClspaddingL" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SubjectList" HeaderText="Subjects" SortExpression="SubjectList">
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="ClspaddingL" Width="40%" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" CssClass="ClspaddingL" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Message to Teacher">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" 
                                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Mail.gif" ToolTip="Message to Teacher"/>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" />
                                                                </asp:TemplateField>


                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 18%; height: 20px">
                                                    </td>
                                                    <td style="width: 18%; height: 20px" align="left">
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                                    </td>
                                                    <td align="left" colspan="1">
                                                    </td>
                                                    <td style="width: 23%; height: 20px" align="left">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
