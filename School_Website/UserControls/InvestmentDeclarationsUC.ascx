<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InvestmentDeclarationsUC.ascx.cs"
    Inherits="InvestmentDeclarationsUC" %>
<table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr id="trFilefFormat" runat="server">
        <td align="center">
            <table width="100%">
                <tr>
                    <td>
                        <span class="LblSmlGray">(Attachment supports files of types - .BMP, .DOC, .DOCX, .JPG,
                            .JPEG, .PDF, .XLS, XLSX upto 1 MB.)</span>                        
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:ListView ID="lstvwMethods" runat="server" OnSorting="lstvwMethods_Sorting" OnItemDataBound="lstvwMethods_ItemDataBound"
                DataKeyNames="Id,InvestmentMethodId">
                <LayoutTemplate>
                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                            <th align="right" class="ClspaddingR" width="65px" style="padding-left: 0px;">
                                Sr. No.
                            </th>
                            <th class="ClspaddingL" style="text-align: left;" width="150px">
                                <asp:LinkButton ID="lnkSectionName" runat="server" CommandName="Sort" CommandArgument="SectionName" style="white-space:nowrap"
                                    CausesValidation="false" ForeColor="Black"> Section Name </asp:LinkButton>
                            </th>
                            <th align="left" class="ClspaddingL" style="padding-right: 5px;" width="450px">                           
                                <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                    CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                            </th>
                            <th align="right" class="ClspaddingR" width="100px">
                                Amount
                            </th>                            
                            <th align="center" class="">
                                Attachment Count
                            </th>                            
                            <th align="center" id="thIsDocSubmitted" runat="server" class="" style="white-space:nowrap">
                                Is Submitted?
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="Tr2" runat="server" class="ClsGridRow">
                        <td align="right" class="ClspaddingR">
                            <asp:Label ID="lblRowNo" runat="server" CssClass=""></asp:Label>
                        </td>
                        <td align="left" class="ClspaddingL">
                            <asp:Label ID="lblSectionName" runat="server" CssClass="" Text='<%#Eval("SectionName") %>'></asp:Label>
                        </td>
                        <td align="center" class="ClspaddingL">
                            <asp:Label ID="lblName" runat="server" CssClass="" Text='<%#Eval("Name") %>'></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 5px;" class="ClspaddingR">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Amount") %>'
                                Style="text-align: right; padding-right: 5px;" onblur="extractNumber(this,1,true);"
                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, true);"
                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="10"></asp:TextBox>
                        </td>                        
                        <td align="center">                            
                            <asp:LinkButton ID="lnkAttachment" runat="server" Text = '<%#Eval("DocumentCount") %>' CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                        </td>                        
                        <td align="center">
                            <asp:CheckBox ID="chkIsSubmitted" runat="server" Checked='<%#Eval("IsDocSubmitted") %>' />
                            <asp:Image ID="imgConfirm" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                Visible="false" />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                        <td align="right" class="ClspaddingR">
                            <asp:Label ID="lblRowNo" runat="server" CssClass=""></asp:Label>
                        </td>
                        <td align="left" class="ClspaddingL">
                            <asp:Label ID="lblSectionName" runat="server" CssClass="" Text='<%#Eval("SectionName") %>'></asp:Label>
                        </td>
                        <td align="center" class="ClspaddingL">
                            <asp:Label ID="lblName" runat="server" CssClass="" Text='<%#Eval("Name") %>'></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 5px;">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Amount") %>'
                                Style="text-align: right; padding-right: 5px;" onblur="extractNumber(this,1,true);"
                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, true);"
                                onkeyup="extractNumber(this,1,true);" onpaste="event.returnValue=false" MaxLength="10"></asp:TextBox>
                        </td>                        
                        <td align="center">                            
                            <asp:LinkButton ID="lnkAttachment" runat="server" Text = '<%#Eval("DocumentCount") %>' CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                        </td>                        
                        <td align="center">
                            <asp:CheckBox ID="chkIsSubmitted" runat="server" Checked='<%#Eval("IsDocSubmitted") %>' />
                            <asp:Image ID="imgConfirm" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                Visible="false" />
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
            </asp:CustomValidator>
            <asp:CustomValidator ID="CustomValidator2" runat="server" Display="None" ErrorMessage="Amount should not be empty or zero if document(s) are submitted."
                CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmountIdDocSubmitted">
            </asp:CustomValidator>
        </td>
    </tr>
    <tr id="trNote" runat="server">
        <td align="center">
            <table id="tblNote" runat="server" align="center" width="100%">
                <tr id="trFullAccess" runat="server">
                    <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                        <span class="LblNrmlB">Note1 :</span>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                        <span class="LblSmlV">If the hard copy of required document for an investment declaration
                            is received, select the ‘Is Submitted?’ checkbox to restrict user to update the
                            amount of it.</span>
                    </td>
                </tr>
                <tr id="trViewAccess" runat="server">
                    <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                        <span class="LblNrmlB">Note1 :</span>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                        <span class="LblSmlV">Once hard copy of document is submitted, amount updation will
                            not be allowed.</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                        <span class="LblNrmlB">Note2 :</span>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                        <span class="LblSmlV">Only non zero amount investment declaration will be considered.
                            To remove the added investment declaration, update it's amount to zero.</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                        <span class="LblNrmlB">Note3 :</span>
                    </td>
                    <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                        <span class="LblSmlV">If the Max. Amount Limit is displayed as '-' for an investment
                            then there is no limit on the amount to be added for that investment.</span>
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

    function CheckValue(obj) {
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

    function ValidateAmountIdDocSubmitted(oSrc, args) {
        var rowIndex = 0;
        var found = false;
        var rowNumbers = ''
        var chk = document.getElementById(_clientlstvwMethods + "_ctrl" + rowIndex + "_chkIsSubmitted");
        while (chk != null) {
            var txt = document.getElementById(_clientlstvwMethods + "_ctrl" + rowIndex + "_txtAmount");
            if (chk.checked && (txt.value.trim() == "" || parseFloat(txt.value.trim()) == 0)) {
                rowNumbers = rowNumbers + ", " + (rowIndex + 1)
            }
            rowIndex++;
            var chk = document.getElementById(_clientlstvwMethods + "_ctrl" + rowIndex + "_chkIsSubmitted");
        }

        if (rowNumbers != '') {
            oSrc.errormessage = "Amount should not be empty or zero if document(s) are submitted for row(s) : " + rowNumbers.substring(1) + '.';
            args.IsValid = false;
            return true;
        }

        args.IsValid = true;
        return false; ;
    }

    function OpenPopup(querystring) {
        window.open('InvestmentDocumentPopup.aspx?' + querystring, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500');
        return false;
    }

</script>
