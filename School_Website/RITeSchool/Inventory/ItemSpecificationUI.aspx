<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ItemSpecificationUI.aspx.cs" Inherits="ItemSpecificationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="2" align="center" width="70%">
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="background-color: white;" valign="top">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnItemSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemSpecificationDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                                            ForeColor="Red" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="100%" EnableViewState="False"
                                        CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnItemSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwItemSpecificationDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="70%">
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Item Code :</span>
                                            </td>
                                            <td class="ClsHilightBGB ">
                                                <span id="spnItemCode" runat="server" class="LblNormal" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Item Name :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                                <span id="spnItemName" class="LblNormal" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Current Stock :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                                <span id="spnCurrentStock" class="LblNormal" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" colspan="1">
                                                <span class="ClsLabel" style="width: 130px">Specification Code :</span>
                                            </td>
                                            <td colspan="1" valign="middle">
                                                <asp:TextBox ID="txtSpecificationCode" runat="server" CssClass="ExLrgTxtBox" MaxLength="30"
                                                    TabIndex="1"></asp:TextBox><span class="ClsMdtStar"> *</span>
                                                <asp:RequiredFieldValidator ID="reqItemCode" runat="server" ControlToValidate="txtSpecificationCode"
                                                    CssClass="ClsLabel" Display="None" ErrorMessage="Specification Code should not be blank."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                          <tr>
                                            <td class="ClsBorderLight" colspan="1">
                                                <span class="ClsLabel" style="width: 130px">Price :</span>
                                            </td>
                                            <td colspan="1" valign="middle">
                                                <asp:TextBox ID="txtItemPrice" runat="server" CssClass="MidTxtBox" onblur="extractNumber(this,2,true);"
                                                    onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false"
                                                    MaxLength="7" Style="text-align: right;
                                            padding-right: 5px"  TabIndex="2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Item Specification :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="ExLrgTxtBox" MaxLength="300"
                                                    Width="100%" TextMode="MultiLine" Height="100" TabIndex="3"></asp:TextBox><span class="ClsMdtStar">
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateDescription"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Is Damaged? :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:CheckBox ID="chkIsDamaged" runat="server" TabIndex="4" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Damaged Date :</span>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtDamagedDt" runat="server" CssClass="LrgTxtBox" Width="100" ReadOnly="true"
                                                    TabIndex="5"></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtDamagedDt" From-Date=""
                                                    Culture="en" ShowErrorMessage="False" To-Today="true" Format="dd mmm yyyy" />
                                                <span class="ClsMdtStar" id="spndmgdt" style="display:none">*</span>
                                                <asp:CustomValidator ID="cstDamageDateValidator" runat="server" ClientValidationFunction="DamageDateValidation"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                <span class="ClsLabel" style="width: 130px">Damage Description :</span>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtDamagedDiscription" runat="server" Width="98%" CssClass="LrgTxtBox"
                                                    Height="100" TextMode="MultiLine" MaxLength="300" TabIndex="6"></asp:TextBox><span
                                                        class="ClsMdtStar" id="spndmgDesc" style="display:none"> *</span>
                                                <asp:CustomValidator ID="cstDamageDescriptionValidator" runat="server" ClientValidationFunction="DamageDescriptionValidation"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:HiddenField ID="hidId" runat="server" />
                                                <asp:HiddenField ID="hidItemID" runat="server" />
                                                <asp:HiddenField ID="hidItemName" runat="server" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnItemSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwItemSpecificationDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnItemSave" runat="server" TabIndex="7" BorderStyle="Solid" CssClass="ClsBtnMid"
                                         Text="Save" UseSubmitBehavior="False" OnClick="btnItemSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" TabIndex="8" BorderStyle="Solid" CausesValidation="false"
                                         CssClass="ClsBtnMid" Text="Cancel" OnClick="btnCancel_Click"
                                         />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnItemSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwItemSpecificationDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center" width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="ClsLblLgnd" ID="lblLegend" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                        </td>
                                                        <td width="10px">
                                                        </td>
                                                        <td align="left" style="border-style:solid;border-width:1px;padding-left:5px;padding-right:5px">
                                                            <asp:Label CssClass="ClsTextNormal" ID="lblDeactivatedUser" runat="server" Text="Damaged Item" style="color:Gray;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwItemSpecificationDetails"
                                                    PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwItemSpecificationDetails" runat="server" DataKeyNames="Id,ItemID, Description,	IsDamaged,	DamageDescription,	IsIssued"
                                                    OnDataBound="lstvwItemSpecificationDetails_DataBound" DataSourceID="lstvwDSobj"
                                                    OnItemCommand="lstvwItemSpecificationDetails_ItemCommand" OnItemDataBound="lstvwItemSpecificationDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblItemsDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                            class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="paddingLSML" width="20%;">
                                                                    <asp:HiddenField ID="hidlstItemId" runat="server" />
                                                                    <asp:LinkButton ID="LinkBtnLinkName" runat="server" CausesValidation="false" CommandArgument="SpecificationCode"
                                                                        CommandName="SortRow" ForeColor="Black">Specification Code</asp:LinkButton>
                                                                </th>
                                                                 <th align="right" style="padding-right: 5px; text-align: right;" width="10%">
                                                                    <asp:LinkButton ID="lnkbtnPrice" runat="server" CausesValidation="false" CommandArgument="Price"
                                                                        CommandName="SortRow" ForeColor="Black">Price</asp:LinkButton>                                                                    
                                                                </th>
                                                                <th align="left" class="paddingLSML" width="15%">
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandArgument="DamagedDate"
                                                                        CommandName="SortRow" ForeColor="Black">Damaged Date</asp:LinkButton>  
                                                                </th>
                                                                <th align="left" class="paddingLSML" width="35%">
                                                                    Damage Description
                                                                </th>
                                                                <th align="center" style="width: 10%;">
                                                                   <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" CommandArgument="IsIssued"
                                                                        CommandName="SortRow" ForeColor="Black">Is Issued?</asp:LinkButton>   
                                                                </th>
                                                                <th align="center" style="width: 5%;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 5%;">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="9">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwItemSpecificationDetails"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td align="right" class="LblNormal">
                                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("SpecificationCode") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="right" class="paddingR">
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Price") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDisplayLocation" runat="server" Text='<%# Eval("DamagedDate") %>'> </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDamageDescription" runat="server"> </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Image ID="imgBtnIsIssued" runat="server" CommandName="UpdateItem" CausesValidation="false"
                                                                    ToolTip="Edit" SRC="../images/IconGrid_AssignTrue.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateItem" CausesValidation="false"
                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteItem" CausesValidation="false"
                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("SpecificationCode") %>'>
                                                                </asp:Label>
                                                            </td>
                                                              <td align="right" class="paddingR">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Price") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDisplayLocation" runat="server" Text='<%# Eval("DamagedDate") %>'> </asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDamageDescription" runat="server"> </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Image ID="imgBtnIsIssued" runat="server" CommandName="UpdateItem" CausesValidation="false"
                                                                    ToolTip="Edit" Src="../images/IconGrid_AssignTrue.gif" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateItem"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteItem"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record found.
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.ItemSpecificationBL" EnablePaging="true"
                                                    ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="GetCount"
                                                    EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter Name="aiItemId" Type="int32" ControlID="hidItemID" DefaultValue=""
                                                            PropertyName="Value" />
                                                        <asp:ControlParameter ControlID="hidSortExpression" Name="asSortExpression" Type="String"
                                                            PropertyName="Value" />
                                                        <asp:ControlParameter ControlID="hidSortDirection" Name="asSortDirection" Type="String"
                                                            PropertyName="Value" />
                                                        <asp:Parameter Name="MaximumRows" DefaultValue="20" Type="Int32" />
                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnItemSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwItemSpecificationDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" CausesValidation="false" TabIndex="9" CssClass="ClsBtnMid"
                    Text="Back" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientchkIsDamaged = "<%=this.chkIsDamaged.ClientID %>";
        _clienttxtDamagedDescription = "<%=this.txtDamagedDiscription.ClientID %>";
        _clientbtnItemSave = "<%=this.btnItemSave.ClientID %>";
        _clienthidId = "<%=this.hidId.ClientID %>";
        _clienttxtSpecificationCode = "<%=this.txtSpecificationCode.ClientID %>";
        _clienttxtDamagedDt = "<%=this.txtDamagedDt.ClientID %>";
        _clientcstDamageDateValidator = "<%=this.cstDamageDateValidator.ClientID %>";
        _clientcstDamageDescriptionValidator = "<%=this.cstDamageDescriptionValidator.ClientID %>";
        _clienttxtDescription = "<%=this.txtDescription.ClientID %>"
        _clienttxtDamagedDt = "<%=this.txtDamagedDt.ClientID %>"

        //Method is used to damage date validation.
        function DamageDateValidation(aSrc, args) {
            if ($get(_clientchkIsDamaged).checked) {
                if ($get(_clienttxtDamagedDt).value == "") {
                    document.getElementById(_clientcstDamageDateValidator).errormessage = "Damage Date should not be blank."
                    args.IsValid = false;
                    return true
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
        }

        //Method is used to validate Damage Description.
        function DamageDescriptionValidation(aSrc, args) {
            var description = $get(_clienttxtDamagedDescription).value.trim()

            if ($get(_clientchkIsDamaged).checked) {
                if (description == "") {
                    document.getElementById(_clientcstDamageDescriptionValidator).errormessage = "Damage Description should not be blank."
                    args.IsValid = false;
                    return true;
                }
                if (description.length > 500) {
                    document.getElementById(_clientcstDamageDescriptionValidator).errormessage = "Damage Description length should not be greater than 500 character(s)."
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        //Method is used to get delete confirmation.
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function ValidateDescription(oSrc, args) {
            var description = $('#' + _clienttxtDescription).val().trim()

            if (description.length > 500) {
                oSrc.errormessage = "Item Specification length should not be greater than 500 character(s).";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        $(document).ready(function () {
            var chk = $get(_clientchkIsDamaged)
            SetFieldState(chk.checked);
        })

        function SetFieldStatus(obj) {

            SetFieldState(obj.checked)
        }
        function SetFieldState(Status) {
            var description = $get(_clienttxtDamagedDescription)
            var dt = $get(_clienttxtDamagedDt)

            if (Status == false) {
                description.disabled = true;
                description.value = "";

                dt.disabled = true;
                dt.value = "";

                $('#spndmgdt').hide()
                $('#spndmgDesc').hide()

            }
            else {
                description.disabled = false
                dt.disabled = false
                $('#spndmgdt').show()
                $('#spndmgDesc').show()
            }
        }




        function ClearMessages() {
            $('#' + "<%=this.lblMessage.ClientID %>").text("")
            $('#' + "<%=this.lblError.ClientID %>").text("")
        }

        
    </script>
</asp:Content>
