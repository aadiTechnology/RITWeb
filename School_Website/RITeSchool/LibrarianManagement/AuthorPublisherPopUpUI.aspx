<%@ Page Language="C#" MasterPageFile="../MasterPages/PopupMaster.master" AutoEventWireup="true"
    CodeFile="AuthorPublisherPopUpUI.aspx.cs" Inherits="AuthorPublisherUI" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table>
        <tr>
            <td align="left">
                <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="LblErrorMsg"></asp:Label>
                <asp:RequiredFieldValidator ID="reqPublisher" runat="server" ControlToValidate="txtPublisher"
                    CssClass="ClsLabel" ErrorMessage="'&quot;+txtAuthorPublisher.Text+&quot;'  Should not be blank."></asp:RequiredFieldValidator>&nbsp;
                
                <div style="float: right" class="LblErrorMsg">
                    * Mandatory Fields
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
            </td>
        </tr>
        <tr>
            <td style="width: 515px">
            </td>
        </tr>
        <tr>
            <td style="width: 515px">
                <table>
                    <tr>
                        <td colspan="2">
                            <table id="Table2" align="center" runat="server" border="0" cellpadding="2" cellspacing="2"
                                style="width: 100%;">
                                <tr>
                                    <td class="ClsBorderLight">
                                        <asp:Label ID="lblName" runat="server" Font-Bold="False"
                                            CssClass="ClsLabel"></asp:Label>
                                    </td>
                                    <td colspan="1">
                                        <asp:TextBox ID="txtAuthorPublisher" runat="server" MaxLength="100" EnableViewState="False"
                                            CssClass="LrgTxtBox" Width="250px"></asp:TextBox>
                                        <span style="font-size: 9pt; color: #ff0000">* </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderLight" style="height: 29px">
                                    </td>
                                    <td colspan="1" style="height: 29px">
                                        <asp:Button ID="btnNew" runat="server" Text="New" CssClass="ClsBtn" CausesValidation="False" />
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" OnClick="btnAdd_Click" />
                                    </td>
                                </tr>
                            </table>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderLight" colspan="2" style="width: 383px; height: 257px">
                            <div id="divGridView" runat="server" style="width: 100%;">
                                <asp:GridView CssClass="GridBorder" ID="grdAuthorPublisher" runat="server" Width="100%"
                                    AutoGenerateColumns="False" DataKeyNames="Category_ID" AllowSorting="True" CellPadding="0"
                                    CellSpacing="1" ForeColor="#333333" GridLines="None" EmptyDataText="No Category Record available."
                                    EmptyDataRowStyle-HorizontalAlign="Center"
                                    OnRowDataBound="grdAuthorPublisher_RowDataBound">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                        Font-Size="Small"></PagerStyle>
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:BoundField HtmlEncode="False">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                        </asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" HeaderText="Edit" ImageUrl="~/images/IconGrid_Edit.GIF"
                                            CommandName="EDIT_CATEGORY">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <RowStyle CssClass="ClsGridRow" />
                                    <HeaderStyle CssClass="ClsGridHeader" />
                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidPublisherId" runat="server" />
                                <asp:HiddenField ID="hidAuthorId" runat="server" />
                                <asp:HiddenField ID="hidIsNewAuthor" runat="server" />
                                <asp:HiddenField ID="hidIsNewPublisher" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="ClsBorderLight">
                <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" Text="Close" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">        
        function ClearText() {
            document.getElementById(_clientAuthoreid).value = ""
            document.getElementById(_clientbtnAddAuthor).value = "Add"
            return false
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
