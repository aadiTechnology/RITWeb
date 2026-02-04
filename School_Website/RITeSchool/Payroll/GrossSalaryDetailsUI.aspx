<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GrossSalaryDetailsUI.aspx.cs" Inherits="GrossSalaryDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
<style>
 </style>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnl1" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsLabel" />
                                        <asp:CustomValidator ID="cstValAsso" runat="server" Display="None" ClientValidationFunction="ValidateAssociation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValCategory" runat="server" Display="None" ClientValidationFunction="ValidateCategory"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValAmount" runat="server" Display="None" ClientValidationFunction="ValidateAmount"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr align="center" id="trMessage" runat="server" visible="false">
                                    <td align="left" style="width: 100%">
                                        <table align="center">
                                            <tr>
                                                <td align="center" id="tdMessage" runat="server">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Font-Bold="True" ForeColor="Blue" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                
                                <tr id="trRole" runat="server">
                                    <td align="center">
                                        <table align="center" width="100%">
                                            <tr>
                                                <td width="25%">
                                                </td>
                                                <td align="center">
                                                    <table align="center">
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight" width="100px">                                                                
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, StaffGroup%>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbStaffgroups" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                                    onchange="Page_BlockSubmit = false;" OnSelectedIndexChanged="cmbStaffgroups_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="25%">
                                                    <div class="ClsGreenBG" style="float: right;">
                                                        <asp:LinkButton ID="lnkPaymentCategories" runat="server" Text="<%$ Resources:LocalizedResources, PaymentCategories%>"
                                                            CssClass="SubTitle" Style="text-align: left;"></asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trPagerUserStaffGrAss" runat="server">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwAssociation">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To%>" />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>" />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>" />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr id="trlistview" runat="server" align="center">
                                    <td align="center">
                                        <asp:ListView ID="lstvwAssociation" runat="server" EnableViewState="true" DataKeyNames="Id,UserId, CategoryId"
                                            ondatabound="lstvwAssociation_DataBound" onitemdatabound="lstvwAssociation_ItemDataBound">
                                            <LayoutTemplate>
                                                <table width="80%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" class="locked" width="50px">
                                                        </th>
                                                        <th align="right" class="locked paddingLR" width="56px">
                                                            <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, SrNo%>"> </asp:Label>
                                                        </th>
                                                        <th align="left" class="locked paddingL">                                                            
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, StaffName%>"> </asp:Label>
                                                        </th>
                                                        <th align="left" class="locked paddingL" width="200px">                                                            
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, PaymentCategories%>"> </asp:Label>
                                                        </th>
                                                        <th align="right" class="locked paddingLR" width="150px">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>"> </asp:Label>                                                            
                                                        </th>
                                                    </tr>
                                                    <tr id="trHeaderContol" runat="server" class="ClsGridHeader">
                                                        <th align="center">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls(this)" />
                                                        </th>
                                                        <th align="right">
                                                        </th>
                                                        <th align="left">
                                                        </th>
                                                        <th align="left" class="paddingL">
                                                            <asp:DropDownList ID="cmbAllStaffGroups" runat="server" CssClass="MidCombo" Width="190px" onchange="SetValueToAll(this,'_cmbCategory',0)">
                                                            </asp:DropDownList>
                                                        </th>
                                                        <th align="right" class="locked paddingLR">
                                                            <asp:TextBox ID="txtAllAmount" runat="server" CssClass="MidTxtBox" MaxLength="10" Style="text-align: right;padding-right:2px;" 
                                                            onchange="SetValueToAll(this,'_txtAmount',1)" onblur="extractNumber(this,0,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                        <td colspan="5">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwAssociation"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage %>" runat="server" CssClass="LblNrmlB" />
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" onchange="Page_BlockSubmit = false;" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="right" class="paddingLR">
                                                        <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("SrNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:DropDownList ID="cmbCategory" Width="190px" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right" class="paddingLR">
                                                        <asp:TextBox ID="txtAmount" CssClass="MidTxtBox" runat="server" MaxLength="8" onblur="extractNumber(this,0,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false"
                                                            Text='<%#Eval("Amount") %>' Style="text-align: right;padding-right:2px;" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="right" class="paddingLR">
                                                        <asp:Label ID="lblRowNo" runat="server" Text='<%#Eval("SrNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingL">
                                                        <asp:DropDownList ID="cmbCategory" Width="190px" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right" class="paddingLR">
                                                        <asp:TextBox ID="txtAmount" CssClass="MidTxtBox" runat="server" MaxLength="8" onblur="extractNumber(this,0,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false"
                                                            Text='<%#Eval("Amount") %>' Style="text-align: right; padding-right:2px;" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.UserPaymentCategoryAssoBL" EnablePaging="True"
                                            ID="objdsPayments" runat="server" SelectMethod="GetAll" 
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="cmbStaffgroups" PropertyName="SelectedValue" Name="aiStaffGroupId" Type="int32" />                                                
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidUpdateGrid" runat="server" 
                                            onvaluechanged="hidUpdateGrid_ValueChanged"  />
                                        <asp:HiddenField ID="hidSelectedRows" runat="server" Value="" />
                                        <asp:HiddenField ID="hidvalAssociation" runat="server" Value="" />
                                        <asp:HiddenField ID="hidvalAssociationCategory" runat="server" Value="" />
                                        <asp:HiddenField ID="hidvalAssociationAmount" runat="server" Value="" />
                                        <asp:HiddenField ID="hidmsgUnsaveMessage" runat="server" Value="" />  
                                        <table width="80%">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If User - Payment Category Association is done then previous values for E/D will be changed permanently."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label5" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label6" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If value of earning deduction is changed from Earning Deduction configuration popup then that change will not be reflected on this page."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>                                      
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trButtons" runat="server" align="center">
                                    <td align="center">
                                        <asp:Button ID="BtnSave" Text="<%$ Resources:LocalizedResources, Save%>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                            disable-page="true" onclick="BtnSave_Click" />
                                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px" CausesValidation="false"
                                            PostBackUrl="~/RITeSchool/Payroll/UsersAndStaffGroupsAsso.aspx" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clientlstvwAssociation = "<%=this.lstvwAssociation.ClientID %>"

        // To open category popup.
        function OpenPopup() {
            window.open('EarningDeductionPercentagePopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=600').focus();
            return false;
        }

        // To check / uncheck all checkboxes according to header checkbox.
        function CheckAllUncheckAlls(obj) {
            var checkAll = obj.checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll

                var num = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lblRowNo")
                $get("<%=this.hidSelectedRows.ClientID %>").value = $get("<%=this.hidSelectedRows.ClientID %>").value + "," + num.innerHTML

                DisableFields(chk, iRowCount);
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        // Update textbox value to round it.
        function SetValueToAll(obj, field, isAmount) {
            var updateAll = true;
            if (isAmount == 1) {
                if (parseInt(obj.value) > 999999999) {
                    RoundValue(obj, 999999999);
                    updateAll = false;
                }
                else
                    RoundValue(obj, 999999999);
            }

            if (updateAll) {
                var iRowCount = 0
                var objChild = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + field)

                while (objChild != null) {

                    var chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
                    if (chk.checked)
                        objChild.value = obj.value
                    iRowCount = iRowCount + 1
                    objChild = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + field)
                }
            }
        }

        // Diasble field of respective row on check / uncheck of checkbox available at same row.
        function DisableFields(obj, rowNo) {

            var category = document.getElementById(_clientlstvwAssociation + "_ctrl" + rowNo + "_cmbCategory")
            var amount = document.getElementById(_clientlstvwAssociation + "_ctrl" + rowNo + "_txtAmount")

            if (obj.checked) {
                category.disabled = false;
                amount.disabled = false;
            }
            else {
                category.disabled = true;
                amount.disabled = true;
                category.value = "0"
                amount.value="0"
            }

            CheckUnCHeckHeaderCheckbox();
        }

        function DisableRespectiveField(obj, rowNo, num) {
            $get("<%=this.hidSelectedRows.ClientID %>").value = $get("<%=this.hidSelectedRows.ClientID %>").value + "," + num
            DisableFields(obj, rowNo);
        }

        // To check / uncheck header checkbox according to other checkboxes checkbox.
        function CheckUnCHeckHeaderCheckbox() {
            var chk
            var isFound = true;
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == false) {
                    isFound = false;
                    break;
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (isFound)            
                $get(_clientlstvwAssociation + "_ChkSelectAll").checked = true;
            else            
                $get(_clientlstvwAssociation + "_ChkSelectAll").checked = false;
        }


        DisableAll();
        // To disable all controls according to selection at page load.
        function DisableAll() {            
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                DisableFields(chk, iRowCount);
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        ///To validate category of selected row.
        function ValidateCategory(oSrc, args) {
            var chk
            var iRowCount = 0
            var rowNumbers = "";
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                var category = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_cmbCategory")
                var srNo = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lblRowNo").innerHTML

                if (chk.checked) {
                    if (category.value == 0) {
                        if (rowNumbers == "")
                            rowNumbers = "" + srNo
                        else
                            rowNumbers = rowNumbers + ", " + srNo
                    }
                }

                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (rowNumbers != "") {
                oSrc.errormessage = $get("<%=this.hidvalAssociationCategory.ClientID %>").value + rowNumbers + ".";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        // To validate amount of selected row.
        function ValidateAmount(oSrc, args) {
            var chk
            var iRowCount = 0
            var rowNumbers = "";
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                var amount = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_txtAmount")
                var srNo = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_lblRowNo").innerHTML
                if (chk.checked) {
                    if (amount.value.trim() == "" || parseInt(amount.value) == 0) {
                        if (rowNumbers == "")
                            rowNumbers = "" + srNo
                        else
                            rowNumbers = rowNumbers + ", " + srNo
                    }
                }

                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (rowNumbers != "") {
                oSrc.errormessage = $get("<%=this.hidvalAssociationAmount.ClientID %>").value + rowNumbers + ".";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        // To check whether at least one row is selected fr saveing.
        function ValidateAssociation(oSrc, args) {
            var chk
            var isFound = false;
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked) {
                    isFound = true;
                    break;
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwAssociation + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (!isFound) {
                oSrc.errormessage = $get("<%=this.hidvalAssociation.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            DisableAll();
        }

        // To display message on change of grid page.
        function PageChangeMessage(oCmb) {
            _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
            var bIsValid
            if (window.confirm($get("<%=this.hidmsgUnsaveMessage.ClientID %>").value))
                bIsValid = true
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }

        // Update grid for updated category list.
        function UpdateGrid() {
            _clienthidUpdateGrid = "<%=this.hidUpdateGrid.ClientID %>"
            document.getElementById(_clienthidUpdateGrid).value = document.getElementById(_clienthidUpdateGrid).value + 1;
            __doPostBack(document.getElementById(_clienthidUpdateGrid).name, '')
        }

        // To set row no to hidden field on change of category and amount.
        function UpdateRowSelection(obj, isAmount, rowNo) {
            if (isAmount == 1)
                RoundValue(obj, 999999999)

            $get("<%=this.hidSelectedRows.ClientID %>").value = $get("<%=this.hidSelectedRows.ClientID %>").value + "," + rowNo
        }

        function ResetFields() {
            if (document.getElementById("<%=this.trMessage.ClientID %>") != null)
                document.getElementById("<%=this.trMessage.ClientID %>").style.display = "none";
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
