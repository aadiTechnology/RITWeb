<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    CodeFile="IssueBookPopUp.aspx.cs" Inherits="IssueBookPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
        <tr>
            <td style="background-color: white;" id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table style="width: 100%;" cellpadding="0" cellspacing="2" id="TABLE1" onclick="return TABLE1_onclick()">
                    <tr>
                        <td align="left" colspan="3">
                            <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                                padding-right: 5px;">
                                <tr>
                                    <td style="height: 20px">
                                        <%--<asp:Label ID="lblSelectUser" runat="server" Font-Bold="True" Text="Select User To Issue Book"
                                            EnableViewState="false"></asp:Label>--%>
                                            <span style="font-weight:bold">Select User To Issue Book</span></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="height: 5px">
                            <asp:Label ID="lblErrorMsg" runat="server"  CssClass="LblErrorMsg"
                                 EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table id="tblForStudentDiv" runat="server" align="center" cellpadding="0" cellspacing="2">
                                        <tr>
                                            <td class="ClsBorderlight" id="tdrdoStdDiv" visible="false">
                                                <asp:RadioButton ID="rdoStdDiv" GroupName="StudentListFilter" Checked="true" runat="server"
                                                    AutoPostBack="True" OnCheckedChanged="rdoStdDiv_CheckedChanged" />
                                            </td>
                                            <td class="ClsBorderlight">
                                                <%--<asp:Label ID="Label1" runat="server" Text="Class :" CssClass="ClsTextNormal"
                                                    EnableViewState="false"></asp:Label>--%>
                                                    <span class="ClsTextNormal">Class :</span></td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDListStdDiv" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDListStdDiv_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trRegFilter" runat="server" visible="false">
                                            <td class="ClsBorderlight">
                                                <asp:RadioButton ID="rdoStudentReg" GroupName="StudentListFilter" runat="server"
                                                    AutoPostBack="True" OnCheckedChanged="rdoStudentReg_CheckedChanged" />
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="1">
                                                <%--<asp:Label ID="lblSeachName" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                    Text="Name / Reg. No. :  "></asp:Label>--%>
                                                    <span class="ClsTextNormal">Name / Reg. No. :</span>
                                            </td>
                                            <td >
                                                <asp:TextBox ID="txtName" runat="server" CssClass="ExLrgTxtBox" Enabled="False"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="Search" OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Button Text="Ok" ID="imgBtnOKUp" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" />
                            <asp:Button Text="Close" ID="btnCloseUp" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Always" runat="server" ID="uPnl">
                                <ContentTemplate>
                                    <asp:GridView CssClass="GridBorder" ID="grdvwSelectUser" Width="100%" runat="server"
                                        AutoGenerateColumns="False" CellPadding="0" GridLines="none" CellSpacing="1"
                                        AllowSorting="True" OnSorting="grdvwSelectUser_Sorting" OnRowDataBound="grdvwSelectUser_RowDataBound"
                                        AllowPaging="False" OnRowCreated="grdvwSelectUser_RowCreated" BackColor="White"
                                        ForeColor="#333333" DataKeyNames="ID,Name" EmptyDataText="No Records Found">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="ChkBoxDelete" runat="server" value='<%# Eval("ID")%>' />
                                                    <asp:HiddenField ID="HidStudentCount" Value='<%# Eval("StudentCount")%>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="True" Width="20px" />
                                                <HeaderStyle Wrap="True" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" SortExpression="Name">
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="ClspaddingL" />
                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" CssClass="ClspaddingL" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ID">
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                        <RowStyle CssClass="ClsGridRow" />
                                        <PagerStyle ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" Position="Top"
                                            PreviousPageText="Previous" />
                                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EditRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSelectedUserId" runat="server" />
                                    <asp:HiddenField ID="hidIsIndivisualStudentId" Value='N' runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DDListStdDiv" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="imgBtnOk" Text="Ok" runat="server" CssClass="ClsBtnSml" OnClick="imgBtnOk_Click"
                                UseSubmitBehavior="false" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnSml" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" src="../Scripts/Validations.js"></script>

    <script language="javascript" type="text/javascript">
        _clientimgBtnOk = "<%=this.imgBtnOk.ClientID%>"
        _clientbtnClose = "<%=this.btnClose.ClientID%>"
        _clientimgBtnOKUp = "<%=this.imgBtnOKUp.ClientID%>"
        _clientbtnCloseUp = "<%=this.btnCloseUp.ClientID%>"
        _clientSelectedUserId = "<%=this.hidSelectedUserId.ClientID%>"
        _clientCmbStdId = "<%=this.DDListStdDiv.ClientID%>"
        _clientGridId = "<%=this.grdvwSelectUser.ClientID%>"
        getUserIds()
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
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
        function AlertMe(obj) {
            var bResult = true
            var grd = document.getElementById(_clientGridId)
            var grdlength = grd.rows.length
            for (var i = 2; i <= grdlength; i++) {
                if (i < 10)
                    var rid = _clientGridId + '_ctl0' + i + '_ChkBoxDelete'
                else
                    var rid = _clientGridId + '_ctl' + i + '_ChkBoxDelete'
                var Radiobtn = document.getElementById(rid)
                Radiobtn.checked = false
            }
            obj.checked = true
            return bResult
        }
        function trimAll(sString) {
            while (sString.substring(0, 1) == ' ') {
                sString = sString.substring(1, sString.length)
            }
            while (sString.substring(sString.length - 1, sString.length) == ' ') {
                sString = sString.substring(0, sString.length - 1)
            }
            while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
                sString = sString.substring(0, sString.length - 1)
            }
            return sString
        }
        function EndReqHandler(sender, args) {
            
            {
                getUserIds()
            } 
        }
        function getUserIds() {
            if (document.getElementById(_clientGridId)) {
                document.getElementById(_clientSelectedUserId).value = window.opener.GetUserIds()
                var arrIds = new Array()
                arrIds = document.getElementById(_clientSelectedUserId).value.split(';')
                var i, j
                var iCnt = document.getElementById(_clientGridId).rows.length
                var iSelectedCnt = arrIds.length
                var id = '0'
                var iCheckedCnt = 0
                var sRow, iRowIndex
                var sChkId = '_ChkBoxDelete'
                var sEleId = ''
                for (i = 1; i < iCnt; i++) {
                    id = document.getElementById(_clientGridId).rows[i].cells[2].innerHTML
                    for (j = 0; j < iSelectedCnt; j++) {
                        if (trimAll(id) == trimAll(arrIds[j])) {
                            iCheckedCnt = iCheckedCnt + 1
                            iRowIndex = parseInt(i) + 1
                            if (iRowIndex < 10) {
                                sRow = '0' + iRowIndex
                            }
                            else {
                                sRow = iRowIndex
                            }
                            sEleId = _clientGridId + '_ctl' + sRow + sChkId
                            if (document.getElementById(sEleId)) {
                                document.getElementById(sEleId).checked = true
                            }
                            break
                        } 
                    }
                    document.getElementById(_clientGridId).rows[i].cells[2].style.display = 'none'
                }
                if (document.getElementById(_clientGridId).rows.length > 1) {
                    document.getElementById(_clientGridId).rows[0].cells[2].style.display = 'none'
                } 
            } 
        }
        function TABLE1_onclick() { }
        function closewindow() {
            document.getElementById(_clientimgBtnOk).disabled = true
            document.getElementById(_clientbtnClose).disabled = true
            document.getElementById(_clientimgBtnOKUp).disabled = true
            document.getElementById(_clientbtnCloseUp).disabled = true
            window.close()
        }
        window.focus()
        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            var grdlength = document.getElementById(_clientGridId).rows.length
            start = getStartIndex(_clientGridId)
            var j = 0
            for (var i = start; i <= grdlength; i++) {
                if (i < 10)
                    var rid = _clientGridId + '_ctl0' + i + '_ChkBoxDelete'
                else
                    var rid = _clientGridId + '_ctl' + i + '_ChkBoxDelete'
                var Radiobtn = document.getElementById(rid)
                if (Radiobtn.checked == true) {
                    j++
                } 
            }
            if (j == 0) {
                alert("Select Atleast One user.")
                return false
            }
            return bResult
        }
    </script>
</asp:Content>
