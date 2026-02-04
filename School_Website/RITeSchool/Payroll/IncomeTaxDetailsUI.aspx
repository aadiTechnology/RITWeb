<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="IncomeTaxDetailsUI.aspx.cs" Inherits="IncomeTaxDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="80%">
            <tr id="trInvestmentDetails" runat="server">
                <td>
                    <asp:UpdatePanel ID="upnl21" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="LblErrorMsg" ShowSummary="true" />
                                    </td>
                                    <td align="left" width="150px">                                        
                                    </td>
                                </tr>
                            </table>
                            <table width="65%" align="center">
                                <tr>
                                    <td align="center" id="tdMessage" runat="server" width="50%">
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                            Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table align="center" width="675px">
                                            <tr>
                                                <td align="left" valign="middle" class="ClsBorderlight">
                                                    <span class="ClsLabel">Staff Group :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStaffGroups" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                        TabIndex="1" OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="100px">
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">User Name :</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="15" CssClass="MidTxtBox" autocomplete="off"
                                                        TabIndex="2"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                                        Text="Search" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" height="11px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table id="LegendTable" runat="server">
                                            <tr>
                                                <td align="left" width="55px" valign="middle">
                                                    <span class="ClsLblLgnd">Legend : </span>
                                                </td>
                                                <td align="left" style="padding-right: 3px">
                                                    <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                        TabIndex="3" BackColor="#FFCCCC" Height="20px" ReadOnly="True" Text=" " Width="20px"
                                                        EnableViewState="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <span class="ClsTextNormal" style="font-weight: bold">Unpublished Tax Details</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center" colspan="2">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTaxDetails">
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
                                    <td align="center">                                        
                                        <asp:ListView ID="lstvwTaxDetails" runat="server" DataKeyNames="Id,UserId,IsPublished"
                                            OnItemDataBound="lstvwTaxDetails_ItemDataBound" OnDataBound="lstvwTaxDetails_DataBound">
                                            <LayoutTemplate>
                                                <table width="800px" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                        <th align="left" width="300px" class="clsLabelgrd">
                                                            User Name
                                                        </th>
                                                        <th align="left" class="clsLabelgrd" width="180px">
                                                            Designation
                                                        </th>
                                                        <th align="center" class="clsLabelgrd" width="210px">
                                                            Investment / Income Declaration
                                                        </th>
                                                        <th align="center" class="clsLabelgrd" width="100px">
                                                            Tax Deduction
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="8">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTaxDetails"
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
                                                    <td align="center">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" CssClass="ClsLabelL" 
                                                            runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:HyperLink ID="lnkInvestmentDeclaration" CssClass="clsLabelgrd" ForeColor="Blue"
                                                            TabIndex="5" runat="server" NavigateUrl="InvestmentDeclarationUI.aspx?"> Declarations </asp:HyperLink>
                                                    </td>
                                                    <td align="center">
                                                        <asp:HyperLink ID="lnkTDSDetails" runat="server" CssClass="clsLabelgrd" ForeColor="Blue"
                                                            TabIndex="5" NavigateUrl="TaxDeductionUI.aspx?"> TDS </asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" CssClass="ClsLabelL" 
                                                            runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:HyperLink ID="lnkInvestmentDeclaration" CssClass="clsLabelgrd" ForeColor="Blue"
                                                            TabIndex="5" runat="server" NavigateUrl="InvestmentDeclarationUI.aspx?"> Declarations </asp:HyperLink>
                                                    </td>
                                                    <td align="center">
                                                        <asp:HyperLink ID="lnkTDSDetails" runat="server" CssClass="clsLabelgrd" ForeColor="Blue"
                                                            TabIndex="5" NavigateUrl="TaxDeductionUI.aspx?"> TDS </asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.IncomeTaxDetailsBL" EnablePaging="True"
                                            ID="objdsIncomeTax" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                            EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="int32" />
                                                <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID" 
                                                    Type="int32" />
                                                <asp:ControlParameter ControlID="cmbStaffGroups" PropertyName="SelectedValue" Name="aiStaffGroupId"
                                                    Type="Int32" />                                                
                                                <asp:ControlParameter ControlID="hidName" PropertyName="Value" Name="asSearchName" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>                                        
                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value="" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" />
                                        <asp:HiddenField ID="hidName" runat="server" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="650px">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button CssClass="ClsBtn" ID="btnPublish" CausesValidation="false" runat="server" disable-page="true" 
                                                        Width="80px" TabIndex="21" Text="Publish" OnClick="btnPublish_Click" />
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
                <td>
                    <div id="divErr" runat="server">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

    	var Page_IsValid = true;
    	function ConfirmDelete(obj) {
    		 Page_IsValid = true;
            str = obj.value;
            if (str == 'Unpublish')
                str = 'unpublish';
            else
                str = 'publish';
               if (!confirm('Are you sure you want to ' + str + ' this Income Tax Details?')) {
               		Page_IsValid = false;
               		return false;

               }
               else
               	return true;
        }

    </script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            _slienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"

            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, null, 1);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }


	</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
