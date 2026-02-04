<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="GradeConfigurationUI.aspx.cs" Inherits="GradeConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                            CssClass="ClsLabel" ShowSummary="true" EnableViewState="false" />
                                        <asp:CustomValidator ID="CstGrade" runat="server" ClientValidationFunction="CheckAtListOne"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstTextValidation" runat="server" ClientValidationFunction="TextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" Width="96px"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CstDuplicateTextValidation" runat="server" ClientValidationFunction="DuplicateGradeNameValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" Width="96px"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstShortNameTextValidation" runat="server" ClientValidationFunction="ShortNameTextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" Width="96px"></asp:CustomValidator>
                                        
                                        <asp:CustomValidator ID="cstDuplicateShortName" runat="server" ClientValidationFunction="DuplicateShortNameValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" Width="96px"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cstGradeDescrptionTextValidation" runat="server" ClientValidationFunction="GradeDescritionTextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"
                                            CssClass="LblErrorMsg" Width="96px"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cstDuplicateSortOrder" runat="server" ClientValidationFunction="ValidateDuplicateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" ValidationGroup="Save"></asp:CustomValidator>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                      
                    </tr>
                    <tr>
                        <td align="center">
                            <table id="tblGradeList" align="center" width="95%">
                                <tr align="center">
                                    <td align="center">
                                              <asp:ListView ID="lstvwGradeConfiguration" runat="server" DataKeyNames="GradeId,OriginalGradeId,SchoolId,ConsideredAsAbsent,ConsideredAsExempted,SortOrder"
                                            OnItemDataBound="lstvwGradeConfiguration_ItemDataBound" OnDataBound="lstvwGradeConfiguration_DataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="70%" runat="server" id="tblGradeInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" id="chkAll" style="width: 95px" runat="server">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                        </th>
                                                        <th align="left" class="paddingL" style="width: 150px">
                                                            <asp:Label ID="lblGradeText" runat="server" Text="<%$ Resources:LocalizedResources, Grade %>"></asp:Label>
                                                        </th>
                                                        <th align="left" class="paddingL paddingLR" style="width: 170px">
                                                            <asp:Label ID="lblShortNameText" runat="server" Text="<%$ Resources:LocalizedResources, ShortName %>"></asp:Label>
                                                        </th>
                                                        <th align="left" class="paddingL paddingLR" style="width: 170px">
                                                            <asp:Label ID="lblDescriptionText" runat="server" Text="<%$ Resources:LocalizedResources, Description %>"></asp:Label>
                                                        </th>
                                                        <th align="center">
                                                            <asp:Label ID="lblSortOrderText" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trData" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 180px;">
                                                        <asp:TextBox ID="txtGradeName" CssClass="MidTxtBox" runat="server" MaxLength="50"
                                                            Text='<%#Eval("GradeName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 100px;">
                                                        <asp:TextBox ID="txtShortName" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                            Text='<%#Eval("ShortName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 250px;">
                                                        <asp:TextBox ID="txtGradeDescription" CssClass="LrgTxtBox" runat="server" MaxLength="100"
                                                            Text='<%#Eval("Description")%>'></asp:TextBox>
                                                    </td>
                                                     <td align="center">
                                                        <asp:DropDownList ID="cmbSortOrder" runat="server" AppendDataBoundItems="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trData" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 180px;">
                                                        <asp:TextBox ID="txtGradeName" CssClass="MidTxtBox" runat="server" MaxLength="50"
                                                            Text='<%#Eval("GradeName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 100px;">
                                                        <asp:TextBox ID="txtShortName" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                            Text='<%#Eval("ShortName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL" style="width: 250px;">
                                                        <asp:TextBox ID="txtGradeDescription" CssClass="LrgTxtBox" runat="server" MaxLength="100"
                                                            Text='<%#Eval("Description")%>'></asp:TextBox>
                                                    </td>
                                                     <td align="center">
                                                        <asp:DropDownList ID="cmbSortOrder" runat="server" AppendDataBoundItems="true">
                                                        </asp:DropDownList>
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
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" OnClick="btnSave_Click" CssClass="ClsBtn" disable-page="true"
                                ValidationGroup="Save" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" CausesValidation="False"
                                UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="hidValGradeNameDuplicated" runat="server" />
                    <asp:HiddenField ID="hidValShortNameDuplicated" runat="server" />
                    <asp:HiddenField ID="hidValGradeNameBlank" runat="server" />
                    <asp:HiddenField ID="hidValShortNameBlank" runat="server" />
                    <asp:HiddenField ID="hidValGradeDescriptioneBlank" runat="server" />
                    <asp:HiddenField ID="hidValAtLeastOneGrade" runat="server" />
                    <asp:HiddenField ID="hidValGradeShortOrder" runat="server" />
                    <asp:HiddenField ID="hidValGradeShortOrderSelected" runat="server" />
                </table>
            </ContentTemplate>            
        </asp:UpdatePanel>

    </div>

    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwGradeConfiguration.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _clientCstGrade = "<%=this.CstGrade.ClientID %>"
        _clientCstDuplicateTextValidation = "<%=this.CstDuplicateTextValidation.ClientID %>"
        _clientcstDuplicateShortName = "<%=this.cstDuplicateShortName.ClientID %>"
        _clientcstShortNameTextValidation = "<%=this.cstShortNameTextValidation.ClientID %>"
        _clientcstTextValidation = "<%=this.cstTextValidation.ClientID %>"
       
        _clientcstGradeDescritionTextValidation = "<%=this.cstGradeDescrptionTextValidation.ClientID%>"
        _clientcstSortOrder = "<%=this.cstSortOrder.ClientID %>";
        _clientcstDuplicateSortOrder = "<%=this.cstDuplicateSortOrder.ClientID %>";


        function DisableButtons() {
            document.getElementById(_clientbtnSave).disabled = true
            document.getElementById(_clientbtnCancel).disabled = true
            __doPostBack(document.getElementById(_clientbtnCancel).name, '')
        }

        function CheckAllUncheckAlls() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var enble
            var iRowCount = 0
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }
        function DuplicateGradeNameValidation(oSrc, args) {
            var chk;
            var txt;
            var txt1;
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var chkCount = 0;

            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value

                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtGradeName")
                          if (trimAll((txt.value).toUpperCase()) == trimAll((txt1.value).toUpperCase()) && iRowCount1 != iRowCount) {
                                iRowNo = iRowNo + 1;
                                oSrc.errormessage = document.getElementById("<%=this.hidValGradeNameDuplicated.ClientID %>").value;
                                document.getElementById(_clientCstDuplicateTextValidation).innerHTML = document.getElementById("<%=this.hidValGradeNameDuplicated.ClientID %>").value;
                                args.IsValid = false
                                return true
                            }
                        }
                        iRowCount1 = iRowCount1 + 1;
                        iRowCnt = iRowCnt - 1;
                    }
                    iRowCount1 = 0
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = true
            return false
        }
        function DuplicateShortNameValidation(oSrc, args) {
            var chk;
            var txt;
            var txt1;
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var chkCount = 0;
            var txtShortName = "";



            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtShortName")

                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtShortName")
                            if (trimAll(txt.value) != "" && trimAll(txt1.value) != "") {
                               if (trimAll((txt.value).toUpperCase()) == trimAll((txt1.value).toUpperCase()) && iRowCount1 != iRowCount) {
                                    iRowNo = iRowNo + 1;
                                    oSrc.errormessage = document.getElementById("<%=this.hidValShortNameDuplicated.ClientID %>").value;
                                    document.getElementById(_clientcstDuplicateShortName).innerHTML = document.getElementById("<%=this.hidValShortNameDuplicated.ClientID %>").value; 
                                    args.IsValid = false
                                    return true
                                }
                            }
                        }
                        iRowCount1 = iRowCount1 + 1;
                        iRowCnt = iRowCnt - 1;
                    }
                    iRowCount1 = 0
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = true
            return false
        }
        function TextValidation(oSrc, args) {

            var chk;
            var iRowCount = 0;
            var chkCount = 0;
            var txt;



            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            oSrc.errormessage = document.getElementById("<%=this.hidValGradeNameBlank.ClientID %>").value;
                            document.getElementById(_clientcstTextValidation).innerHTML = document.getElementById("<%=this.hidValGradeNameBlank.ClientID %>").value;
                            args.IsValid = false
                            return true
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }
        function ShortNameTextValidation(oSrc, args) {

            var chk;
            var iRowCount = 0;
            var chkCount = 0;
            var txt;
            var txtShrtNm = "";
            var txtGrade = "";
            var txtValue = "";



            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtGrade = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtShortName")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            oSrc.errormessage = document.getElementById("<%=this.hidValShortNameBlank.ClientID %>").value;
                            document.getElementById(_clientcstShortNameTextValidation).innerHTML = document.getElementById("<%=this.hidValShortNameBlank.ClientID %>").value; 
                            args.IsValid = false
                            return true
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = true
            return false
        }

        function GradeDescritionTextValidation(oSrc, args) {

            var chk;
            var iRowCount = 0;
            var chkCount = 0;
            var txt;
            var txtShrtNm = "";
            var txtGrade = "";
            var txtValue = "";



            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtGrade = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeDescription")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            oSrc.errormessage = document.getElementById("<%=this.hidValGradeDescriptioneBlank.ClientID %>").value;
                            document.getElementById(_clientcstGradeDescritionTextValidation).innerHTML = document.getElementById("<%=this.hidValGradeDescriptioneBlank.ClientID %>").value;
                            args.IsValid = false
                            return true
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            args.IsValid = true
            return false
        }


        var Page_IsValid = true;
        function CheckAtListOne() {
        	Page_IsValid = true;
            var chk;
            var iRowCount = 0;
            var chkCount = 0;



            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    chkCount = chkCount + 1;
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (chkCount == 0) {
            	alert(document.getElementById("<%=this.hidValAtLeastOneGrade.ClientID %>").value);
            	Page_IsValid = false;
                return false                
            }
            return true
        }

        function ValidateDuplicateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sortOrders = "";
            var isDuplicate = false;

            var sCnt = "";
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            cmb = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder");

            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value != 0) {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }

                }

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_ChkSelect")
                cmb = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }
            if (isDuplicate) {
                document.getElementById(_clientcstDuplicateSortOrder).errormessage = document.getElementById("<%=this.hidValGradeShortOrder.ClientID %>").value+"  : " + (sCnt) + ".";
                document.getElementById(_clientcstDuplicateSortOrder).innerHTML = document.getElementById("<%=this.hidValGradeShortOrder.ClientID %>").value+" : " + (sCnt) + ".";
                args.IsValid = false;
            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }

        function ValidateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sortOrders = "";
            var notSelected = true;
            var isDuplicate = false;
            var sCount = "";
            var sCnt = "";
            chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            cmb = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder");
            document.getElementById(_clientcstSortOrder).errormessage = "";
            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value == "0") {
                        notSelected = false;
                        if (sCount != "")
                            sCount = sCount + ", " + (iRowCount + 1);
                        else
                            sCount = (iRowCount + 1);
                    }
                    else {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }
                }
                else {
                    cmb.value = "0";
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_ChkSelect")
                cmb = document.getElementById(_clientListViewId + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }
            if (!notSelected) {
                document.getElementById(_clientcstSortOrder).errormessage = document.getElementById("<%=this.hidValGradeShortOrderSelected.ClientID %>").value +" : " + (sCount) + ".";
                document.getElementById(_clientcstSortOrder).innerHTML = document.getElementById("<%=this.hidValGradeShortOrderSelected.ClientID %>").value+" : " + (sCount) + ".";
                args.IsValid = false;

            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }


        function OnGridKeyUp(obj, e) {             
            UpDownKeyPress(obj.id, e);
        }
     
        
    </script>

</asp:Content>
