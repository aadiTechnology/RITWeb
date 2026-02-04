<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PrivateTransportDetailsUI.aspx.cs" Inherits="PrivateTransportDetailsUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table id="tblMainBody" width="90%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center" cellpadding="1" cellspacing="2" width="100%">
                            <tr>
                                <td>
                                    <asp:MultiView ID="mltvwContainer" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="vwStudentDetails" runat="server">
                                            <table id="tblTransportDetails" runat="server" align="center" width="60%">
                                                <tr align="center">
                                                    <td align="center">
                                                        <asp:Label ID="lblDeleteMsg" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td align="center">
                                                        <table>
                                                            <tr>
                                                                <td id="lblStandard" runat="server" align="center" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Standard :</span>
                                                                    <asp:DropDownList AutoPostBack="true" ID="ddlStandard" runat="server" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td id="lblDivsion" runat="server" align="center" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Division :</span>
                                                                    <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="Td1" runat="server" align="center" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Name :</span>
                                                                </td>
                                                                <td id="Td2" runat="server" align="center" class="ClsBorderlight">
                                                                    <asp:TextBox ID="txtRegNoName" runat="server" CssClass="MidTxtBox" autocomplete="off"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="Td3" runat="server" align="center">
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" BorderWidth="1px"
                                                            CausesValidation="False" UseSubmitBehavior="false" OnClick="btnSearch_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTravelersDetails">
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
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr align="center" style="width: 70%">
                                                                <td align="center" style="width: 70%">                                                                   
                                                                    <asp:ListView ID="lstvwTravelersDetails" runat="server" DataSourceID="ObjDSTravelersDetails"
                                                                        OnDataBound="lstvwTravelersDetails_DataBound" DataKeyNames="UserId,PrivateTransportDetailsId,UserName"
                                                                        OnItemCommand="lstvwTravelersDetails_ItemCommand" OnItemDataBound="lstvwTravelersDetails_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table align="center" width="100%" runat="server" id="tblTravlerInfo" style="color: #333333"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" width="35%" style="padding-left: 9px;">
                                                                                        <asp:LinkButton ID="lnkSortName" CommandName="Sort" CommandArgument="UserName" runat="server"
                                                                                            CausesValidation="false" ForeColor="Black">Travelers Name </asp:LinkButton>
                                                                                    </th>
                                                                                    <th align="center" width="15%">
                                                                                        <asp:Label ID="lblAddEdit" runat="server" CausesValidation="false" ForeColor="Black">Add/Edit</asp:Label>
                                                                                    </th>
                                                                                    <th align="center" width="15%">
                                                                                        <asp:Label ID="lblDelete" runat="server" CausesValidation="false" ForeColor="Black">Delete </asp:Label>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr runat="server" id="itemPlaceholder">
                                                                                </tr>
                                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                                    <td colspan="3">
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTravelersDetails"
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
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton ID="imgBtnSelect" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                        ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" class="paddingL">
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton ID="imgBtnSelect" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                        ImageUrl="~/RITeSchool/images/Selection5.gif" />
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                        ImageUrl="../images/IconGrid_Delete.gif" />
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
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ObjectDataSource TypeName="BusinessLogic.PrivateTransportDetailsBL" EnablePaging="True"
                                                                        ID="ObjDSTravelersDetails" runat="server" SelectMethod="GetTravelersList" SortParameterName="sortExpression"
                                                                        SelectCountMethod="GetTravelersListCount" EnableCaching="False">
                                                                        <SelectParameters>
                                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                Type="int32" />
                                                                            <asp:ControlParameter ControlID="ddlStandard" PropertyName="SelectedValue" Type="Int32"
                                                                                Name="aiStandardId" DefaultValue="0" />
                                                                            <asp:ControlParameter ControlID="ddlDivision" PropertyName="SelectedValue" Type="Int32"
                                                                                Name="aiDivisionId" DefaultValue="0" />
                                                                            <asp:ControlParameter ControlID="txtRegNoName" PropertyName="Text" Type="string"
                                                                                Name="asUserName" DefaultValue="0" />
                                                                            <asp:Parameter Name="sortExpression" Type="String" />
                                                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                        </SelectParameters>
                                                                    </asp:ObjectDataSource>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vwAddTransportDetails" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="2" style="height: 403px; width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 77%">
                                                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                            Visible="false" Height="20px" Width="100%" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" class="ClsTextNormal" align="center">
                                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True">
                                                        </asp:Label>
                                                        <!-- User InfoTable starts here -->
                                                        <table id="tblVehicleDetails" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                            style="width: 70%; margin-left: 19px;">
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Student Name :</span>
                                                                </td>
                                                                <td align="left" style="width: 31%;">
                                                                    <asp:Label ID="lblStudentName" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                        Width="250px" ForeColor="Blue"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Stop Name :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%;">
                                                                    <asp:TextBox ID="txtStopName" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                        Width="145px"></asp:TextBox>
                                                                    *&nbsp;
                                                                    <asp:RequiredFieldValidator ID="reqvalStopName" runat="server" ControlToValidate="txtStopName"
                                                                        Display="None" ErrorMessage="Stop Name should not be blank."></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Vehicle Number :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%;">
                                                                    <asp:TextBox ID="txtVehicleNumber" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                        Width="145px"></asp:TextBox>
                                                                    *&nbsp;
                                                                    <asp:RequiredFieldValidator ID="reqVehicleNumber" runat="server" ControlToValidate="txtVehicleNumber"
                                                                        Display="None" ErrorMessage="Vehicle Number should not be blank."></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Vehicle Type :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                                    <asp:TextBox ID="txtVehicleType" runat="server" MaxLength="20" CssClass="MidTxtBox"></asp:TextBox>
                                                                    *<asp:RequiredFieldValidator ID="reqVehicleType" runat="server" ControlToValidate="txtVehicleType"
                                                                        Display="None" ErrorMessage="Vehicle Type should not be blank."></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Transport Staff 1 :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                                    <asp:TextBox ID="txtTransportStaff1" runat="server" MaxLength="100" CssClass="MidTxtBox"
                                                                        Width="250px"></asp:TextBox>
                                                                    *<asp:RequiredFieldValidator ID="reqValtxtTransportStaff1" runat="server" ControlToValidate="txtTransportStaff1"
                                                                        Display="None" ErrorMessage="Transport Staff 1 should not be blank."></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Mobile No :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                                    <asp:TextBox ID="txtMobile1" CssClass="MidTxtBox" runat="server" MaxLength="10" onblur="extractNumber(this,0,false);"
                                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                                    *<asp:CustomValidator ID="cstMobileNumber1" runat="server" ClientValidationFunction="MobileNumber1Validation"
                                                                        Display="None" ErrorMessage="">
                                                                    </asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Transport Staff 2 :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                                    <asp:TextBox ID="txtTransportStaff2" runat="server" MaxLength="100" CssClass="MidTxtBox"
                                                                        onblur="ResetLable()" onkeypress="ResetLable()" onkeyup="ResetLable()" onpaste="ResetLable()"
                                                                        Width="250px" ondrop="ResetLable()"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderLight" style="width: 11%">
                                                                    <span class="ClsLabel">Mobile No :</span>
                                                                </td>
                                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                                    <asp:TextBox ID="txtMobile2" CssClass="MidTxtBox" runat="server" MaxLength="10" onblur="extractNumber(this,0,false);"
                                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                                    <asp:CustomValidator ID="cstMobileNumber2" runat="server" ClientValidationFunction="MobileNumber2Validation"
                                                                        Display="None" ErrorMessage="">
                                                                    </asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSave" runat="server">
                                                                <td>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                                        CausesValidation="true" OnClick="btnSave_Click" />
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidPrivateTransportDetailsId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                    <asp:HiddenField ID="hidUserName" runat="server" />
                                    <asp:HiddenField ID="hidMode" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _clientcst_MobileNumber = "<%=this.cstMobileNumber1.ClientID%>";
        _clientcst_MobileNumber1 = "<%=this.cstMobileNumber2.ClientID%>";
        _client_TransportStaff2 = "<%=this.txtTransportStaff2.ClientID%>";
        var _clienttxtRegNumber = '#<%=txtRegNoName.ClientID%>';

     
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobile1.ClientID %>"
        function MobileNumber1Validation(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            document.getElementById(_clientcst_MobileNumber).errormessage = ""
            if (sMobileNumber.length == 0) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. for Transport Staff 1 should not be blank."
                args.IsValid = false
                return true
            }
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. for Transport Staff 1 should be of 10 digits."
                args.IsValid = false
                return true
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile No. for Transport Staff 1 should not start with zero."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        _sClienttxtMobilePhoneNumber1Id = "<%=this.txtMobile2.ClientID %>"
        function MobileNumber2Validation(oSrc, args) {

            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumber1Id).value
            var StaffName = document.getElementById(_client_TransportStaff2)
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            document.getElementById(_clientcst_MobileNumber1).errormessage = ""
            if (sMobileNumber.length == 0 && (StaffName).value != "") {
                document.getElementById(_clientcst_MobileNumber1).errormessage = "Mobile No. for Transport Staff 2 should not be blank."
                args.IsValid = false
                return true
            }
            if (sMobileNumber.length < 10 && (StaffName).value != "") {
                document.getElementById(_clientcst_MobileNumber1).errormessage = "Mobile No. for Transport Staff 2 should be of 10 digits."
                args.IsValid = false
                return true
            }
            else if (sMobileNumber.substring(0, 1) == '0' && (StaffName).value != "") {
                document.getElementById(_clientcst_MobileNumber1).errormessage = "Mobile No. for Transport Staff 2 should not start with zero."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetLable() {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumber1Id).value
            var StaffName = document.getElementById(_client_TransportStaff2)
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)

            if ((StaffName).value == "") {
                document.getElementById(_sClienttxtMobilePhoneNumber1Id).value = ""
                document.getElementById(_sClienttxtMobilePhoneNumber1Id).disabled = true;
            }
            else if ((StaffName).value != "")
                document.getElementById(_sClienttxtMobilePhoneNumber1Id).disabled = false;
        }
         
    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() { 
            _clienttxtRegNumber = '#<%=txtRegNoName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>";
            var ddlDivision = '#<%=ddlDivision.ClientID%>';
            var ddlStandard = '#<%=ddlStandard.ClientID%>';

            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, ddlStandard, ddlDivision, null, 0);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtRegNoName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
