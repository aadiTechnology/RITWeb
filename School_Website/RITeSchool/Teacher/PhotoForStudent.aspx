<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhotoForStudent.aspx.cs" MasterPageFile="~/RITeSchool/MasterPages/BlankMaster.master" Inherits="PhotoForStudent" %>

<asp:Content ID="headContent" runat="server" ContentPlaceHolderID="headContentPlaceHolder">
    <style type="text/css">
        P.breakhere { page-break-before: always; }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" runat="server" ContentPlaceHolderID="bodyContentPlaceHolder">
	<div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;"
            align="center">
            <tr>
                <asp:ListView ID="lstVwMain" runat="server" OnItemDataBound="lstVwMain_ItemDataBound">
                    <LayoutTemplate>
                        <tr id="itemPlaceholder" runat="server"> </tr>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr align="center">
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr align="center">
                            <td style="color: Black; font-family: Arial; font-size: large; text-align: center">
                                <asp:Label ID="Label1" runat="server" Text="Class : " />
                                <asp:Label ID="lblClass" runat="server" Text='<%#Eval("Standard_Division_Name") %>' />
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="trPhoto" runat="server" align="center">
                            <td width="100%" align="left" id="tdPhoto" runat="server">
                                <asp:ListView runat="server" ID="groupListView" GroupItemCount="7" DataKeyNames="UserId" OnItemDataBound="groupListView_ItemDataBound">
                                    <LayoutTemplate>
                                        <table runat="server" id="table1">
                                            <tr runat="server" id="groupPlaceholder" width="100%"> </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <tr runat="server" id="tableRow">
                                            <td runat="server" id="itemPlaceholder" width="100%" />
                                        </tr>
                                    </GroupTemplate>
                                    <GroupSeparatorTemplate>
                                        <tr id="Tr1" runat="server">
                                            <td colspan="9">
                                                <div></div>
                                            </td>
                                        </tr>
                                    </GroupSeparatorTemplate>
                                    <ItemTemplate>
                                        <td class="ClsBorderBlue" height="120px" width="90px" style="border-color: LightGrey; border-width: thin; border-style: dotted;" align="center" valign="middle">                                           
                                             <img id="imgStudent1" runat="server" height="120" width="90" />
                                             <asp:HiddenField ID="hidUserId" runat="server" Value='<%# Eval("UserId") %>'  />
                                        </td>
                                    </ItemTemplate>
                                    <ItemSeparatorTemplate>
                                        <td id="Td3" runat="server">
                                            &nbsp;
                                        </td>
                                    </ItemSeparatorTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr id="trBreak" runat="server">
                            <td id="tdBreak" runat="server">
                                <p class="breakhere">
                                </p>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </tr>
            <tr id="trExactPhoto" runat="server" align="center">
                <td width="100%" align="left" id="tdExactPhoto" runat="server">
                    <asp:ListView runat="server" ID="LstVwExactPhoto" GroupItemCount="7" DataKeyNames="UserId" OnItemDataBound="LstVwExactPhoto_ItemDataBound">
                        <LayoutTemplate>
                            <table runat="server" id="table1">
                                <tr runat="server" id="groupPlaceholder" width="100%"></tr>
                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr runat="server" id="tableRow">
                                <td runat="server" id="itemPlaceholder" width="100%" />
                            </tr>
                        </GroupTemplate>
                        <GroupSeparatorTemplate>
                            <tr id="Tr1" runat="server">
                                <td colspan="9">
                                    <div></div>
                                </td>
                            </tr>
                        </GroupSeparatorTemplate>
                        <ItemTemplate>
                            <td class="ClsBorderBlue" height="120px" width="90px" style="border-color: LightGrey; border-width: thin; border-style: dotted;" align="center" valign="middle">                                
                                <img id="imgStudent1" runat="server" height="120" width="90" />
                            </td>
                        </ItemTemplate>
                        <ItemSeparatorTemplate>
                            <td id="Td3" runat="server">
                                &nbsp;
                            </td>
                        </ItemSeparatorTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidDivisionId" runat="server" />
                    <asp:HiddenField ID="hidStandardId" runat="server" />
                    <asp:HiddenField ID="hidRegNo" runat="server" />
                    <asp:HiddenField ID="hidName" runat="server" />
                    <asp:HiddenField ID="hidOperator" runat="server" />
                    <asp:HiddenField ID="hidPrefix" runat="server" />
                    <asp:HiddenField ID="hidIsExactMatch" runat="server" />
                </td>
            </tr>
        </table>
    </div>
	<script language="javascript" type="text/javascript">
		function PrintSheet() {
			window.print();
			return false;
		}
		PrintSheet();
	</script>
</asp:Content>