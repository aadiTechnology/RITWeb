<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PTChallanDetails.aspx.cs" Inherits="PTChallanDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <!-- Data Insert Here -->
				<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
				<ContentTemplate>

                <table id="tblPTDetails" runat="server" border="0" cellpadding="0" cellspacing="2"
                    style="width: 97%;">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td width="80%">
                                        <asp:Panel ID="pnlErrorMsg" runat="server">
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                Height="20px" Width="100%" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label></asp:Panel>
                                    </td>
                                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                        <span class="ClsMdtStar">* Mandatory Fields</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" ValidationGroup="valPTRegCertifacateNO" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="1">
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="false" EnableViewState="false" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" class="ClsTextNormal" align="center">
                            <!-- User InfoTable starts here -->
                            <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                style="width: 48%;">
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 45%">
                                        <span class="ClsLabel">Professional Tax Registration Certificate No. :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar" style="width: 50%">
                                        <asp:TextBox ID="txtPTRegCertificateNo" runat="server" MaxLength="20" Width="186px"
                                            CssClass="MidTxtBox" TabIndex="1" ReadOnly="true"></asp:TextBox>*
                                        <asp:RequiredFieldValidator ID="reqValPTRegCertificateNo" runat="server" ControlToValidate="txtPTRegCertificateNo"
                                            ErrorMessage="Professional Tax Registration Certificate No. should not be blank."
                                            ValidationGroup="valPTRegCertifacateNO" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 45%">
                                        <span class="ClsLabel">Cheque No. :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="10" Width="186px" CssClass="MidTxtBox"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                            ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>
                                       <%--  <asp:RequiredFieldValidator ID="reqValChequeNo" runat="server" ControlToValidate="txtChequeNo"
                                            ErrorMessage="Cheque No. should not be blank." ValidationGroup="valPTRegCertifacateNO"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 45%">
                                        <span class="ClsLabel">CIN No. :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtCINNo" runat="server" MaxLength="20" Width="186px" CssClass="MidTxtBox"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                            ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>     
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 45%">
                                        <span class="ClsLabel">Bank Name :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo" TabIndex="3">
                                        </asp:DropDownList>
                                        *
                                        <asp:CompareValidator ID="cmpValBankName" runat="server" ControlToValidate="ddlBankName"
                                            ErrorMessage="Bank Name should be selected." Operator="NotEqual" ValueToCompare="0"
                                            ValidationGroup="valPTRegCertifacateNO" Display="None"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" align="center" style="width: 45%">
                                        <span class="ClsLabel">Month :</span>
                                    </td>
                                    <td align="left" valign="top" class="ClsMdtStar">
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="SmlCombo" TabIndex="4">
                                        </asp:DropDownList>
                                        *
                                        <asp:CompareValidator ID="cmpValMont" runat="server" ControlToValidate="ddlMonth"
                                            ErrorMessage="Month should be selected" Operator="NotEqual" ValueToCompare="0"
                                            ValidationGroup="valPTRegCertifacateNO" Display="None"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 45%">
                                        <span class="ClsLabel">Year :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="SmlCombo" TabIndex="5">
                                        </asp:DropDownList>
                                        *
                                        <asp:CompareValidator ID="cmpValYear" runat="server" ControlToValidate="ddlYear"
                                            ErrorMessage="Year should be selected." Operator="NotEqual" ValueToCompare="0"
                                            ValidationGroup="valPTRegCertifacateNO" Display="None"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>                                   
                                    <td align="center" colspan="2">
                                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" BorderWidth="1px" disable-page="true"
                                            OnClick="btnSave_Click"  TabIndex="6" ValidationGroup="valPTRegCertifacateNO">
                                        </asp:Button>&nbsp;
                                        <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                            Text="Cancel" BorderWidth="1px" TabIndex="7" OnClick="BtnCancel_Click"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="divErr" style="width: 50%" align="center">
                    <table class="LblNoRecord" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td class="ClsConfigText">
                                Please configure following details :
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Add Bank Name
                            </td>
                        </tr>
                    </table>
                </div>
                <table id="tbllstPTChallanDetails" runat="server">
                    <!-- User InfoTable ListView -->
                    <tr id="trPagerPTChallan" runat="server">
                        <td align="center" colspan="2">
                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwPTChallanDetails">
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
                                            <br />
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <table width="100%">
                                <tr style="width: 100%">
                                    <td style="width: 1024px">
                                        <asp:ListView ID="lstvwPTChallanDetails" runat="server" OnDataBound="lstvwPTChallanDetails_DataBound"
                                            DataKeyNames="MonthwiseProfessionalTaxDetailsId,PTRegCertificateId,PTRegCertificateNo,ChequeNo,Bank_Name,Year,Month,MonthId"
                                            OnItemDataBound="lstvwPTChallanDetails_ItemDataBound" OnItemCommand="lstvwPTChallanDetails_ItemCommand"
                                            OnSorting="lstvwPTChallanDetails_Sorting" DataSourceID="ObjDSPTChallanDetails">
                                            <LayoutTemplate>
                                                <table  runat="server" id="tblChallanDetails" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" width="300px" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnPTRegCertificateNo" runat="server" CommandName="Sort" CommandArgument="PTRegCertificateNo"
                                                                CausesValidation="false" ForeColor="Black"> P.T.  Registration Certificate No. </asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="150px" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnChequeNo" runat="server" CommandName="Sort" CommandArgument="ChequeNo"
                                                                CausesValidation="false" ForeColor="Black"> Cheque No.</asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="150px" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="CINNo"
                                                                CausesValidation="false" ForeColor="Black"> CIN No.</asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="30%" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnBankName" runat="server" CommandName="Sort" CommandArgument="Bank_Name"
                                                                CausesValidation="false" ForeColor="Black"> Bank Name</asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="10%" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkMonth" runat="server" CommandName="Sort" CommandArgument="Month"
                                                                CausesValidation="false" ForeColor="Black"> Month</asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="100px" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkYear" runat="server" CommandName="Sort" CommandArgument="Year"
                                                                CausesValidation="false" ForeColor="Black"> Year</asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="50px">
                                                            Edit
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                        <td colspan="6">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwPTChallanDetails"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("PTRegCertificateNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("ChequeNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("CINNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("Bank_Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("PTRegCertificateNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("ChequeNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("CINNo") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("Bank_Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ObjectDataSource TypeName="BusinessLogic.MonthwiseProfessionalTaxDetailsBL"
                                EnablePaging="True" ID="ObjDSPTChallanDetails" runat="server" SelectMethod="GetAllPTChallanDetails"
                                SortParameterName="sortExpression" SelectCountMethod="CountPTChallanDetails"
                                EnableCaching="False">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                    <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID" Type="int32" />                                    
                                    <asp:Parameter Name="sortExpression" Type="String" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidPTRegCertificateId" runat="server" />
                            <asp:HiddenField ID="hidMonthwiseProfessionalTaxDetailsId" runat="server" />
                            <asp:HiddenField ID="hidCurrentMonth" runat="server" />
                            <asp:HiddenField ID="hidCurrentYear" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="center">
                            &nbsp;
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                CausesValidation="False" UseSubmitBehavior="false" PostBackUrl=""
                                TabIndex="8" />
                        </td>
                    </tr>
                </table>
                <!-- User InfoTable end here -->
                <asp:HiddenField ID="hidUserId" runat="server" />
                <!-- Data Insert End Here -->
				</ContentTemplate>
				</asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlblErr = "<%=this.lblErrorMsg.ClientID %>"
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlblErr) != null) {
                document.getElementById(_clientlblErr).style.display = "none"
            }
        }
    </script>

</asp:Content>
