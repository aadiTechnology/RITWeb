<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="POUserDetailsPopup.aspx.cs" Inherits="RITeSchool_Transport_TransportDetails"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td class="ClsGrayMainTitle">
                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                    <tr>
                        <td align="center" class="MainTitleHead" style="height: 20px">
                            <span style="font-weight: bold">External PO Receivers</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                    vertical-align: top">
                    <tr>
                        <td id="MainDataTable" align="center">
                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwTransportDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True" meta:resourcekey="lblUpdateSucessResource1"></asp:Label>
                                                <table id="tblTransport" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                    align="center">
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 100px">
                                                            <span class="ClsLabel">Name :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtName" runat="server" MaxLength="100" CssClass="LrgTxtBox" meta:resourcekey="txtnameResource1"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                                                Display="None" ErrorMessage="Name should not be blank."></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="DuplicateNameValidation"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="custValName" runat="server" ErrorMessage="Name should not be duplicate." Display="None" OnServerValidate="Name_Validate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Address :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtaddress" runat="server" MaxLength="300" CssClass="ExLrgTxtBox"
                                                                TextMode="MultiLine" meta:resourcekey="txtaddressResource1">
                                                            </asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="regexpaddress" runat="server" ViewStateMode="Enabled"
                                                                Display="None" ControlToValidate="txtaddress" ErrorMessage="Length of remarks should not exceed 300 characters."
                                                                CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,300}$">
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">City :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtcity" runat="server" MaxLength="100" CssClass="MidTxtBox" meta:resourcekey="txtcityResource1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Pin Code :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtpincode" runat="server" MaxLength="6" CssClass="MidTxtBox" meta:resourcekey="txtpincodeResource1"
                                                            onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            <asp:CustomValidator ID="cstpincode" Display="None" CssClass="ClsMdtStar" runat="server"
                                                                ErrorMessage="">
                                                            </asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">Mobile Number :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtmobileno" runat="server" MaxLength="10" CssClass="MidTxtBox" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"
                                                                meta:resourcekey="txtmobilenoResource1"></asp:TextBox>
                                                            <%--<span class="ClsMdtStar">*</span>--%>
                                                            <%--<asp:RequiredFieldValidator ID="reqMobileNo" runat="server" ControlToValidate="txtName"
                                                                Display="None" ErrorMessage="Mobile number should not be blank."></asp:RequiredFieldValidator>--%>
                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Mobile Number should be of 10 digit."
                                                                Display="None" ClientValidationFunction="ValidateMobileNo"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight">
                                                            <span class="ClsLabel">GSTIN :</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtGSTIN" runat="server" MaxLength="15" CssClass="LrgTxtBox" meta:resourcekey="txtGSTINResource1"></asp:TextBox>
                                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="DuplicateGSTINValidation"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="GSTIN should not be duplicate." Display="None" OnServerValidate="GSTIN_Validate"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwTransportDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                    disable-page="true" OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="false" UseSubmitBehavior="False" meta:resourcekey="btnCancelResource1"
                                                    OnClick="btnCancel_Click" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwTransportDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td class="ClsBorderLight">
                            <span class="ClsLabel">Name :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameSearch" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" width="100%">
                            <tr runat="server" id="trTotalRec" align="center">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTransportDetails">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1 %>" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize %>" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount %>" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblnamelist" align="center" width="98%">
                                        <tr align="center" style="width: 100%">
                                            <td align="center" style="width: 800">
                                                <asp:ListView ID="lstvwTransportDetails" runat="server" DataKeyNames="Id" OnItemCommand="lstvwTransportDetails_ItemCommand"
                                                    OnItemDataBound="lstvwTransportDetails_ItemDataBound" OnDataBound="lstvwTransportDetails_DataBound"
                                                    OnSorting="lstvwTransport_Sorting">
                                                    <LayoutTemplate>
                                                        <table id="Table2" align="center" width="100%" runat="server" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th id="Th1" align="left" class="PaddingL">
                                                                    <asp:LinkButton ID="LnkBtnName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                        Style="padding-left: 5px;" CausesValidation="False" ForeColor="Black" Text="Name"></asp:LinkButton>
                                                                </th>
                                                                <th id="Th2" align="left" class="PaddingL" width="150px">
                                                                    <asp:Label ID="lblCity" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="City"></asp:Label>
                                                                </th>
                                                                <th id="Th4" align="center" width="120px" style="padding-left: 5px;" runat="server">
                                                                    <asp:Label ID="lblMobNo" runat="server" Text="Mobile Number" Style="padding-left: 5px;"></asp:Label>
                                                                </th>
                                                                <th id="Th5" align="left" width="100px" style="padding-left: 5px;" runat="server">
                                                                    <asp:LinkButton ID="lnkBtnGSTIN" runat="server" CommandName="Sort" CommandArgument="GSTIN"
                                                                        CausesValidation="False" ForeColor="Black" Text="GSTIN"></asp:LinkButton>
                                                                </th>
                                                                <th id="Th6" align="center" width="50px" runat="server">
                                                                    Edit
                                                                </th>
                                                                <th id="Th7" align="center" width="50px" runat="server">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr runat="server" id="Tr1">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                <td colspan="6" runat="server">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwTransportDetails">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" AutoPostBack="true"
                                                                                                    OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Name") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidServiceId" runat="server" Value= '<%# Eval("Id") %>' />
                                                            </td>
                                                            <td id="Td3" align="left" class="paddingL" runat="server">
                                                                <asp:Label ID="LBLCity" runat="server" Style="padding-left: 5px;" Text='<%# Eval("City") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td5" align="center" class="paddingL" runat="server">
                                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td6" align="left" class="paddingL" runat="server">
                                                                <asp:Label ID="lblGSTIN" runat="server" Style="padding-left: 5px;" Text='<%# Eval("GSTIN") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td7" align="center" runat="server">
                                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="UpdateCommand"
                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td id="Td8" align="center" runat="server">
                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="RemoveCommand"
                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td id="Td9" class="paddingL" align="left" runat="server">
                                                                <asp:Label ID="lblName" runat="server" Style="padding-left: 5px;" Text='<%# Eval("Name") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidServiceId" runat="server" Value= '<%# Eval("Id") %>' />
                                                            </td>
                                                            <td id="Td10" class="paddingL" align="left" runat="server">
                                                                <asp:Label ID="lblCity" runat="server" Style="padding-left: 5px;" Text='<%# Eval("City") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td12" align="center" class="paddingL" runat="server">
                                                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td13" class="paddingL" align="left" runat="server">
                                                                <asp:Label ID="lblGSTIN" runat="server" Style="padding-left: 5px;" Text='<%# Eval("GSTIN") %>'></asp:Label>
                                                            </td>
                                                            <td id="Td14" align="center" runat="server">
                                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="UpdateCommand"
                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td id="Td15" align="center" runat="server">
                                                                <asp:ImageButton ID="btnDelete" CommandName="RemoveCommand" CausesValidation="False"
                                                                    ToolTip="Delete" runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.POUserDetailsBL" EnablePaging="True"
                                                    ID="objdsPOUserDetails" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter ControlID="txtNameSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                        <asp:Parameter Name="sortDirection" Type="String" />
                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwTransportDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clienttxtmobileno = "<%=this.txtmobileno.ClientID %>"
        _clienttxtName = "<%=this.txtName.ClientID %>"
        _clienthidId = "<%=this.hidId.ClientID %>"
        _clienttxtGSTIN = "<%=this.txtGSTIN.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function CloseWindow(obj) {
            window.opener.location = "../Transport/POUserDetailsPopup.aspx?" + obj;
            window.opener.focus();
            window.close();
        }

        function hidepopup() {
            window.opener.RefreshData();
            window.close();
        }

        function ResetMessage() {
            $('[id$=lblUpdateSucess]').html('')
        }

        function ValidateMobileNo(oSrc, args) {
            var mobieNo = $('#' + _clienttxtmobileno).val()
            
//            if (mobieNo == '') {
//                oSrc.errormessage = 'Mobile Number should not be blank.';
//                args.IsValid = false;
//                return true;
//            }
//            else 
            
            if (mobieNo != '' && mobieNo.length != 10) {
                oSrc.errormessage = 'Mobile Number length should be 10 digit.';
                args.IsValid = false;
                return true;
            }

            args.IsValid = true
            return false
        }

        function DuplicateNameValidation(oSrc, args) {
            var name = $('#' + _clienttxtName).val().trim().toLowerCase()
            var id = $('#' + _clienthidId).val()

            var found = false;
            var index = 0
            $('[id$=lblName]').each(function () {

                var serviceId = $('[id$=' + index + '_hidServiceId]').val()

                if ($(this).html().toLowerCase() == name && id != serviceId) {
                    found = true;
                    return false
                }

                index++
            })

            if (found) {
                oSrc.errormessage = "Name should not be duplicate.";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function DuplicateGSTINValidation(oSrc, args) {
            var gstin = $('#' + _clienttxtGSTIN).val().trim().toLowerCase()

            if (gstin != '') {
                var id = $('#' + _clienthidId).val()

                var found = false;
                var index = 0
                $('[id$=lblGSTIN]:not(":empty")').each(function () {

                    var serviceId = $('[id$=' + index + '_hidServiceId]').val()

                    if ($(this).html().toLowerCase() == gstin && id != serviceId) {
                        found = true;
                        return false
                    }
                    
                    index++
                })

                if (found) {
                    oSrc.errormessage = "GSTIN should not be duplicate.";
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                args.IsValid = true
                return false
            }
        }
           
    </script>
</asp:Content>
