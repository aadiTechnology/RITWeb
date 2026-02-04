<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="PerformanceGradeConfigUI.aspx.cs" Inherits="PerformanceGradeConfigUI" %>

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
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                            EnableViewState="false" />
                                        <asp:CustomValidator ID="cstGrade" runat="server" ClientValidationFunction="CheckAtListOne"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstTextValidation" runat="server" ClientValidationFunction="TextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CstDuplicateTextValidation" runat="server" ClientValidationFunction="DuplicateGradeNameValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstShortNameTextValidation" runat="server" ClientValidationFunction="ShortNameTextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDuplicateShortName" runat="server" ClientValidationFunction="DuplicateShortNameValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstGradeDescrptionTextValidation" runat="server" ClientValidationFunction="GradeDescritionTextValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDuplicateDescription" runat="server" ClientValidationFunction="DuplicateDescritionValidation"
                                            SetFocusOnError="True" Display="None" ErrorMessage="" CssClass="LblErrorMsg"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstDuplicateSortOrder" runat="server" ClientValidationFunction="ValidateDuplicateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                                    </td>
                                </tr>                                
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Label ID="lblErr" runat="server" ></asp:Label>
                    </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="95%">
                                <tr>
                                    <td align="center">
                                        <asp:ListView ID="lstvwGradeConfiguration" runat="server" DataKeyNames="GradeId,OriginalGradeId,SchoolId,SortOrder"
                                            OnItemDataBound="lstvwGradeConfiguration_ItemDataBound" OnDataBound="lstvwGradeConfiguration_DataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="70%" runat="server" id="tblGradeInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" id="chkAll" style="width: 95px" runat="server">
                                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                        </th>                                                       
                                                        <th align="left" class="paddingL" style="width: 150px">
                                                            Grade
                                                        </th>
                                                        <th align="left" class="paddingL" style="width: 170px">
                                                            Short Name
                                                        </th>
                                                        <th align="left" class="paddingL" style="width: 170px">
                                                            Description
                                                        </th>
                                                        <th align="center" width="100px">
                                                            Sort Order
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
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtGradeName" CssClass="MidTxtBox" runat="server" MaxLength="50"
                                                            Text='<%#Eval("GradeName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtShortName" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                            Text='<%#Eval("ShortName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL">
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
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtGradeName" CssClass="MidTxtBox" runat="server" MaxLength="50"
                                                            Text='<%#Eval("GradeName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:TextBox ID="txtShortName" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                            Text='<%#Eval("ShortName")%>'></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="paddingL">
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
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ClsBtn"
                                disable-page="true" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="False"
                                UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientListViewId = "<%=this.lstvwGradeConfiguration.ClientID %>"
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";

        function CheckAllUncheckAlls() {
            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAll).checked

            var iRowCount = 0
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect");
            }
        }

        function DuplicateGradeNameValidation(oSrc, args) {
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var rowNumbers = "";

            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value

                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            var txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtGradeName")
                            if (trimAll((txt.value).toUpperCase()) != "" && trimAll((txt.value).toUpperCase()) == trimAll((txt1.value).toUpperCase()) && iRowCount1 != iRowCount) {
                                iRowNo = iRowNo + 1;

                                if (rowNumbers.match((iRowCount + 1)) == null)
                                    rowNumbers = rowNumbers + ", " + (iRowCount + 1);
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

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Name should not be duplicate for row(s) : " + rowNumbers + ".";
                oSrc.innerHTML = "Name should not be duplicate for row(s) : " + rowNumbers + ".";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function DuplicateShortNameValidation(oSrc, args) {
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var rowNumbers = "";

            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value
                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtShortName")

                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            var txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtShortName")
                            if (trimAll(txt.value) != "" && trimAll(txt1.value) != "") {
                                if (trimAll((txt.value).toUpperCase()) != "" && trimAll((txt.value).toUpperCase()) == trimAll((txt1.value).toUpperCase()) && iRowCount1 != iRowCount) {
                                    iRowNo = iRowNo + 1;

                                    if (rowNumbers.match((iRowCount + 1)) == null)
                                        rowNumbers = rowNumbers + ", " + (iRowCount + 1)
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

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Short Name should not be duplicate for row(s)" + rowNumbers + ".";
                oSrc.innerHTML = "Short Name should not be duplicate for row(s)" + rowNumbers + ".";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function TextValidation(oSrc, args) {
            var iRowCount = 0;
            var rowNumbers = "";
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            rowNumbers = rowNumbers + ", " + (iRowCount + 1);
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Name should not be blank for row(s) :" + rowNumbers + ".";
                oSrc.innerHTML = "Name should not be blank for row(s) :" + rowNumbers + ".";
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

        function ShortNameTextValidation(oSrc, args) {
            var iRowCount = 0;
            var rowNumbers = "";
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var txtGrade = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtShortName")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            rowNumbers = rowNumbers + ", " + (iRowCount + 1);
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Short Name should not be blank for row(s) : " + rowNumbers + ".";
                oSrc.innerHTML = "Short Name should not be blank for row(s) : " + rowNumbers + ".";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function GradeDescritionTextValidation(oSrc, args) {
            var iRowCount = 0;
            var txtShrtNm = "";
            var rowNumbers = "";
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var txtGrade = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeName")
                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeDescription")
                    if (txt.value != undefined) {
                        if (txt.value == '') {
                            rowNumbers = rowNumbers + ", " + (iRowCount + 1);
                        }
                    }
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Description should not be blank for row(s) : " + rowNumbers + ".";
                oSrc.innerHTML = "Description should not be blank for row(s) : " + rowNumbers + ".";
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function DuplicateDescritionValidation(oSrc, args) {
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var rowNumbers = "";
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value
                    var txt = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_txtGradeDescription")

                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            var txt1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_txtGradeDescription")
                            if (trimAll(txt.value) != "" && trimAll(txt1.value) != "") {
                                if (trimAll((txt.value).toUpperCase()) != "" && trimAll((txt.value).toUpperCase()) == trimAll((txt1.value).toUpperCase()) && iRowCount1 != iRowCount) {
                                    iRowNo = iRowNo + 1;
                                    
                                    if (rowNumbers.match((iRowCount + 1)) == null)
                                        rowNumbers = rowNumbers + ", " + (iRowCount + 1)
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

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Description should not be duplicate for row(s)" + rowNumbers + ".";
                oSrc.innerHTML = "Description should not be duplicate for row(s)" + rowNumbers + ".";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckAtListOne(oSrc, args) {
            var iRowCount = 0;
            var isFound = false;

            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked) {
                    isFound = true;
                    break;
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (!isFound) {
                oSrc.errormessage = "At least one Grade should be selected.";
                args.IsValid = false;
                return false;
            }
            args.IsValid = true;
            return true
        }

        function ValidateDuplicateSortOrder(oSrc, args) {
            var iRowNo = 0
            var iRowCount = 0;
            var iRowCount1 = 0;
            var rowNumbers = "";
            var chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    var iRowCnt = document.getElementById(_clienthidRowCnt).value
                    var cmb = document.getElementById(_clientListViewId + "_ctrl" + iRowCount + "_cmbSortOrder")

                    while (iRowCnt > 0) {
                        chk = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_ChkSelect")
                        if (chk.checked == true) {
                            var cmb1 = document.getElementById(_clientListViewId + "_ctrl" + iRowCount1 + "_cmbSortOrder")
                            if (cmb.value != 0) {
                                if (cmb.value == cmb1.value && iRowCount1 != iRowCount) {
                                    iRowNo = iRowNo + 1;
                                    if (rowNumbers.match((iRowCount + 1)) == null)
                                        rowNumbers = rowNumbers + ", " + (iRowCount + 1)
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

            if (rowNumbers != "") {
                rowNumbers = rowNumbers.substring(1);
                oSrc.errormessage = "Sort Order should not be duplicate for row(s) : " + rowNumbers + ".";
                oSrc.innerHTML = "Sort Order should not be duplicate for row(s) : " + rowNumbers + ".";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
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
            oSrc.errormessage = "";
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
                oSrc.errormessage = "Sort Order should be selected for row(s) : " + (sCount) + ".";
                oSrc.innerHTML = "Sort Order should be selected for row(s) : " + (sCount) + ".";
                args.IsValid = false;

            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }        
        
    </script>
</asp:Content>
