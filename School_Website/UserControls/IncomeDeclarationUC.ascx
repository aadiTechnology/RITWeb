<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IncomeDeclarationUC.ascx.cs"
    Inherits="IncomeDeclarationUC" %>
<table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="center">
            <asp:ListView ID="lstvwMethods" runat="server" OnSorting="lstvwMethods_Sorting" OnItemDataBound="lstvwMethods_ItemDataBound"
                DataKeyNames="Id,InvestmentMethodId">
                <LayoutTemplate>
                    <table width="70%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                            <th align="right" width="50px">
                                Sr. No.
                            </th>
                            <th class="ClsPaddingL" style="text-align: left;" width="150px">
                                <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="Sort" CommandArgument="SectionName"
                                    CausesValidation="false" ForeColor="Black"> Section Name </asp:LinkButton>
                            </th>
                            <th align="left" class="clsLabelgrd" style="padding-right: 5px;">
                                <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                    CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                            </th>
                            <th align="right" width="110px" style="padding-right:5px;">
                                Amount
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="Tr2" runat="server" class="ClsGridRow">
                        <td align="right">
                            <asp:Label ID="lblRowNo" runat="server" CssClass="ClsLabelR"></asp:Label>
                        </td>
                        <td align="left" class="ClsPaddingL">
                            <asp:Label ID="lblSectionName" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionName") %>'></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 5px;">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Amount") %>'
                                Style="text-align: right; padding-right: 5px;" onblur="extractNumber(this,1,true);"
                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, true);"
                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                        <td align="right">
                            <asp:Label ID="lblRowNo" runat="server" CssClass="ClsLabelR"></asp:Label>
                        </td>
                        <td align="left" class="ClsPaddingL">
                            <asp:Label ID="lblSectionName" runat="server" CssClass="ClsLabel" Text='<%#Eval("SectionName") %>'></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 5px;">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Amount") %>'
                                Style="text-align: right; padding-right: 5px;" onblur="extractNumber(this,1,true);"
                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, true);"
                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <EmptyDataTemplate>
                    <tr>
                        <td class="LblNoRecord" align="center" width="80%">
                            No record found.
                        </td>
                    </tr>
                </EmptyDataTemplate>
            </asp:ListView>
        </td>
    </tr>
    <tr id="trNote" runat="server">
        <td align="center">
            <table id="tblNote" runat="server" align="center" width="70%">
                <tr>
                    <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                        <span class="LblNrmlB">Note :</span>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                        <span class="LblSmlV">Only non zero amount income declaration will be considered.
                            To remove the added income declaration, update it's amount to zero.</span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>    
    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
    <asp:HiddenField ID="hidHasFullAccess" runat="server" Value="N" />
</table>

<script type="text/javascript" language="javascript">

    _clientlstvwMethods = "<%=this.lstvwMethods.ClientID %>"

    function CheckIncomeValue(obj) {
        if (obj.value.trim() == "" || obj.value.trim() == "-")
            obj.value = "0"
        else {
            var floatValue = parseFloat(obj.value)
            var intValue = parseInt(obj.value)

            var multplyer = 1;
            if (floatValue < 0)
                multplyer = -1

            intValue = parseFloat(intValue)
            var difference = parseFloat((floatValue * 10) % 10)

            if (difference < 0)
                difference = -1 * difference;

            if (difference != 5 && difference != 0) {
                if (difference > 5)
                    difference = intValue + (multplyer * 1)
                else
                    difference = intValue + (multplyer * 0.5)

                obj.value = difference
            }
        }
    }

</script>
