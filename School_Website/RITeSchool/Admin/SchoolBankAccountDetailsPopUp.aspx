<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMasterSml.master"
    CodeFile="SchoolBankAccountDetailsPopUp.aspx.cs" Inherits="SchoolBankAccountDetailsPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="left" colspan="2" rowspan="1">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                    <tr>
                        <td style="height: 20px">
                            <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True" Text = "<%$ Resources:LocalizedResources, SchoolBankDetails %>" ></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">*</span> 
                                <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                                        ValidationGroup="Save" runat="server" />
                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    <table id="Table1" runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 62%;
                                        margin-left: 19px;">
                                        <tr>
                                            <td align="left" style="width: 20%" class="ClsBorderlight">
                                                <asp:Label ID="lblTestlbl" runat="server"  Text= "<%$ Resources:LocalizedResources, BankName%>"
                                                    EnableViewState="False"></asp:Label>
                                                    <span class = "colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlBankName" runat="server" MaxLength="100" CssClass="LrgCombo"
                                                    Width="199px" TabIndex="0">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="ddlBankName"
                                                    Display="None" ValidationGroup="Save" InitialValue="0" ErrorMessage= "<%$ Resources:LocalizedResources, ValBankNameSelected%>"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 20%">
                                                <asp:Label ID="lblAccountNo" runat="server" Text= "<%$ Resources:LocalizedResources, AccountNo%>"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="width: 40%">
                                                <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="20" CssClass="LrgTxtBox"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" TabIndex="0"></asp:TextBox>
                                                <span style="color: red" class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqAccountNo" runat="server" CssClass="ClsMdtStar"
                                                    ValidationGroup="Save" ErrorMessage= "<%$ Resources:LocalizedResources, ValAccountNoBlank%>" Display="None"
                                                    ControlToValidate="txtAccountNo"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnSave" Text= "<%$ Resources:LocalizedResources, Save%>" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" TabIndex="0" />
                                                <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" TabIndex="0"/>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl2" runat="server">
                    <ContentTemplate>
                        <table id="tblVendername" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 100%; margin-left: 19px;">
                            <tr id="trPagerTransportStaff" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwBankAccount">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text = "<%$ Resources:LocalizedResources, To%>" />
                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, OutOf%>" />
                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text =  "<%$ Resources:LocalizedResources, Records%>" />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="center" width="90%">
                                        <tr align="center" style="width: 100%">
                                            <td align="center" style="width: 600">
                                                <asp:ListView ID="lstvwBankAccount" DataKeyNames="SchoolWiseBankAccountDetailsId,BankName,AccountNo,BankAssociationCount"
                                                    runat="server" DataSourceID="ObjDSSchoolBankAccountDetails" OnDataBound="lstvwBankAccount_DataBound"
                                                    OnItemDataBound="lstvwBankAccount_ItemDataBound" OnSorting="lstvwBankAccount_Sorting"
                                                    OnItemCommand="lstvwBankAccount_ItemCommand">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" width="50%" style="padding-left: 10px;">
                                                                    <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Bank_Name" Text = "<%$ Resources:LocalizedResources, BankName%>"
                                                                        CausesValidation="false" ForeColor="Black" TabIndex="0"></asp:LinkButton>
                                                                </th>
                                                                <th align="left" width="30%" style="padding-left: 10px;">
                                                                    <asp:LinkButton ID="lnkBtnAddress" runat="server" CommandName="Sort" CommandArgument="AccountNo" Text = "<%$ Resources:LocalizedResources, AccountNo%>"
                                                                        CausesValidation="false" ForeColor="Black" TabIndex="0"></asp:LinkButton>
                                                                </th>
                                                                <th align="center" width="10%" visible="true">
                                                              <asp:Label ID = "lblEdit" runat = "server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                                </th>
                                                                <th align="center" width="10%" visible="true">
                                                                     <asp:Label ID = "lblDelete" runat = "server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Delete %>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwBankAccount"
                                                                        PageSize="5">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="MessageLabel" Text= "<%$ Resources:LocalizedResources, SelectAPage%>" runat="server" CssClass="LblNrmlB" />
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
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" style="padding-left: 10px">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-left: 10px">
                                                                <asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateAccount"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF"/>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveAccount"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td style="padding-left: 10px" align="left">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("BankName") %>'></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 10px" align="left">
                                                                <asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateAccount"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAccount" CausesValidation="false"
                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.SchoolwiseBankAccountDetailsBL" EnablePaging="True"
                                            ID="ObjDSSchoolBankAccountDetails" runat="server" SelectMethod="GetSchoolwiseBankAccountDetailsBL"
                                            SortParameterName="sortExpression" SelectCountMethod="CountTotalSchoolwiseBankAccountBL"
                                            EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidMode" runat="server" />
                                        <asp:HiddenField ID="hidServerDate" runat="server" />
                                        <asp:HiddenField ID="hidSchoolWiseBankAccountDetailsId" runat="server" />
                                        <asp:HiddenField ID="hidBankName" runat="server" />
                                        <asp:HiddenField ID="hidAccountNo" runat="server" />
                                        <asp:HiddenField ID = "hidAlertDeleterecord" runat = "server" />
                                        <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    &nbsp;
                                    <asp:Button ID="btnBack" runat="server" Text= "<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" TabIndex="0"
                                        OnClientClick="window.close();"/>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbl_ErrorMessage = "<%=this.lblErrorMsg.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertDeleterecord.ClientID %>").value)) {
                bResult = false

            }
            return bResult
        }

        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }

        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }

        function btnsaveonclick(varname) {
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            lbl1.innerHTML = "";
            var lbl1 = document.getElementById(_clientlbl_ErrorMessage);
            lbl1.innerHTML = "";
        }
    </script>

</asp:Content>
