<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DocumentDetailsUI.aspx.cs" Inherits="DocumentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr style="height:30px;">
                            <td>                                
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center">
                                <asp:ListView ID="lstvwUserDocuments" runat="server" DataKeyNames="UserId, DocumentFilePath"
                                    OnItemDataBound="lstvwUserDocuments_ItemDataBound">
                                    <LayoutTemplate>
                                        <table width="50%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                            cellspacing="1" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="left" style="padding-left : 10px;">
                                                    Document Name
                                                </th>
                                                <th class="paddingLR" align="center">
                                                    Year
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="left" width="30%" style="padding-left : 10px;">
                                                <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("DocumentTypeName") %>' />
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:LinkButton ID="lnkDocumentName" runat="server" Text='<%#Eval("Year")%>' PostBackUrl="#"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="left" width="30%" style="padding-left : 10px;">
                                                <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("DocumentTypeName") %>' />
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:LinkButton ID="lnkDocumentName" runat="server" Text='<%#Eval("Year")%>' PostBackUrl="#"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table width="40%">
                                            <tr>
                                                <td class="LblNoRecord" align="center">
                                                    No record found.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">

            function OpenDocumentPopUp(FilePath) {
                window.open("../DOWNLOADS/User Documents/FormNo16/" + FilePath, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650');
                return false;
            }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
