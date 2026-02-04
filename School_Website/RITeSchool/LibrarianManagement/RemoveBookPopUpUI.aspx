<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="RemoveBookPopUpUI.aspx.cs" Inherits="RemeveBookPopUpUI"
    Title="RemoveBookPopUpUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td class="ClsGrayMainTitle">
                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td align="center" style="width: 90%">
                                <%-- <asp:Label ID="lblRemoveBookDetails" CssClass="MainTitleHead" runat="server" Font-Bold="True"
                                    Text="Remove Book Details" EnableViewState="false"></asp:Label>--%>
                                <span class="MainTitleHead" style="font-weight: bold">Remove Book Details</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valsumRemoveBook" runat="server" CssClass="LblErrorMsg" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UPanelBookInfo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <br />
                            <table id="tbBookInfo" runat="server" cellpadding="2" cellspacing="2" width="75%">
                                <tr>
                                    <td align="left" class="ClsBorderlight" style="width: 35%">
                                        <%--<asp:Label ID="lblBookTitle" runat="server" Text="Book Title :" 
                                            CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Book Title : </span>
                                    </td>
                                    <td align="left" style="width: 65%" class="ClsBorderlight">
                                        <asp:Label ID="lblBookTitle1" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <%--<asp:Label ID="lblCategory" runat="server" Text="Class :" CssClass="ClsLabel" 
                                            EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Class : </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblCategory1" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <%--<asp:Label ID="lblAuthorName" runat="server" Text="Author Name :" 
                                            CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Author Name : </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblAuthor" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <%--<asp:Label ID="lblPublisher" runat="server" Text="Publisher :" 
                                            CssClass="ClsLabel" EnableViewState="False"></asp:Label>--%>
                                        <span class="ClsLabel">Publisher : </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblPublisher1" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdvwBook" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UPanelRemoveBook" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="60%">
                                <tr runat="server" id="trTotalRecId" align="center">
                                    <td>
                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                        <%--<asp:Label ID="lblTo" runat="server" Text=" to " CssClass="LblNormal" 
                                            EnableViewState="False" />--%>
                                        <span class="LblNormal">To</span>
                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                        <%--<asp:Label ID="lblOutOf" runat="server" Text=" out of " CssClass="LblNormal" 
                                            EnableViewState="False" />--%>
                                        <span class="LblNormal">Out Of</span>
                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                        <%--<asp:Label ID="lblRecords" runat="server" Text="records " CssClass="LblNormal" 
                                            EnableViewState="False" />--%>
                                        <span class="LblNormal">Records</span>
                                    </td>
                                </tr>
                                <tr id="Tr12">
                                    <td>
                                        <div id="divGridView" runat="server" style="width: 100%;">
                                            <asp:GridView CssClass="GridBorder" ID="grdvwBook" runat="server" Width="100%" AutoGenerateColumns="False"
                                                DataKeyNames="Book_No,Book_Detail_Id" AllowSorting="True" CellPadding="0" CellSpacing="1"
                                                ForeColor="#333333" GridLines="None" EmptyDataText="No Book Record available."
                                                EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="grdvwBook_RowDataBound"
                                                AllowPaging="True" DataSourceID="GrdDSobj" PageSize="20" OnPageIndexChanging="grdvwBook_PageIndexChanging"
                                                OnRowCreated="grdvwBook_RowCreated" OnSorting="grdvwBook_Sorting">
                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="Accession Number" DataField="Book_No">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText="Remove Book" ImageUrl="~/RIteSchool/images/IconGrid_Delete.gif"
                                                        CommandName="DELETE_BOOK">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image" HeaderText="Write Off" ImageUrl="~/RIteSchool/images/Bool_Lost_2.gif"
                                                        CommandName="WRITE_OFF_BOOK">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:ButtonField>
                                                    <%--
                                                    <asp:ButtonField ButtonType="Image" HeaderText="Write Off" ImageUrl="~/RITeSchool/images/Bool_Lost_2.gif"
                                                        CommandName="WRITE_OFF_BOOK">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"/>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:ButtonField>--%>
                                                </Columns>
                                                <PagerTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                    OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="GrdDSobj" runat="server" TypeName="BusinessLogic.BookBL"
                                                EnablePaging="true" SelectMethod="GetAllBooksForRemove" SortParameterName="sortExpression"
                                                SelectCountMethod="CountRemoveBookRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                    <asp:ControlParameter ControlID="hidBookId" Name="aiBookId" PropertyName="Value" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hidIsNewBook" runat="server" />
                                            <asp:HiddenField ID="hidRemoveBookNo" runat="server" />
                                            <asp:HiddenField ID="hidBookId" runat="server" />
                                            <asp:HiddenField ID="hidRowIndex" runat="server" />
                                            <asp:HiddenField ID="hidReason" runat="server" />
                                            <asp:HiddenField ID="HidWriteOff" runat="server" />
                                            
                                            <asp:HiddenField ID="hidBookName" runat="server" />
                                            <asp:HiddenField ID="hidMediaType" runat="server" />
                                            <asp:HiddenField ID="hidMainCategory" runat="server" />
                                            <asp:HiddenField ID="hidAuthorName" runat="server" />
                                            <asp:HiddenField ID="hidPublisher" runat="server" />
                                            <asp:HiddenField ID="hidAccessionNumber" runat="server" />
                                            <asp:HiddenField ID="hidDescription" runat="server" />
                                            <asp:HiddenField ID="hidStandardId" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBookRemove" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnWriteOffBook" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divMain" runat="server" class="overlay" style="visibility: hidden; display: none;">
                    </div>
                    <div id="updtpnlPopUp" runat="server" style="visibility: hidden; display: none; position: absolute;
                        margin: 0px; padding: 0px; width: 300px; height: 180px; border-width: 0px; left: 0px;
                        top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 0px 0px 0px 20px;
                        background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                        <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                            <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                Book Remove</div>
                            <span style="cursor: hand" onclick="javascript:HidePopup();">
                                <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                            </span>
                        </div>
                        <div style="padding: 10px; text-align: left;" class="ClsLabel">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                ID="UpdatePanel4">
                                <ContentTemplate>
                                    <table>
                                        <tr align="left">
                                            <td>
                                                <asp:Label ID="lblSchoolleaving" runat="server" Text="Reason for book remove:" CssClass="LblNormal" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtReason" CssClass="SmlCombo" runat="server" Height="80px" Width="92%"
                                                    TextMode="MultiLine"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="cstValBookRemove" runat="server" ErrorMessage="Reason for book remove should not be blank."
                                                    CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" Display="None"
                                                    ClientValidationFunction="validateBookRemove"></asp:CustomValidator>
                                                <%--<asp:RequiredFieldValidator ID="reqReason" runat="server" ControlToValidate="txtReason"
                                                    Display="None" CssClass="ClsMdtStar" ErrorMessage="Reason for book remove should not be blank."
                                                    EnableClientScript="true"></asp:RequiredFieldValidator>--%>
                                                <asp:RegularExpressionValidator ID="regExpReason" runat="server" ControlToValidate="txtReason"
                                                    Display="None" ErrorMessage="Reason for book remove should not exceed than 500 characters."
                                                    ValidationExpression="^[\s\S]{0,500}$" CssClass="ClsMdtStar"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnBookRemove" runat="server" Text="OK" CssClass="ClsBtn" OnClick="btnBookRemove_Click"
                                                    OnClientClick="if(!ConfirmRemove()){return false;}" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                    OnClientClick="javascript:HidePopup();return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divWriteOffMain" runat="server" class="overlay" style="visibility: hidden;
                        display: none;">
                    </div>
                    <div id="updtpnlWriteOffPopUp" runat="server" style="visibility: hidden; display: none;
                        position: absolute; margin: 0px; padding: 0px; width: 300px; height: 180px; border-width: 0px;
                        left: 0px; top: 0px; line-height: normal; width: auto; border: solid 1px black;
                        margin: 0px 0px 0px 20px; background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
                        <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                            <div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                                Write Off Book</div>
                            <span style="cursor: hand" onclick="javascript:HideWriteOffPopup();">
                                <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                            </span>
                        </div>
                        <div style="padding: 10px; text-align: left;" class="ClsLabel">
                            <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table>
                                        <tr align="left">
                                            <td>
                                                <asp:Label ID="lblWriteOffBook" runat="server" Text="Reason for write off book :"
                                                    CssClass="LblNormal" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtWriteOffBook" CssClass="SmlCombo" runat="server" Height="80px"
                                                    Width="92%" TextMode="MultiLine"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="cstvalWriteOffBook" runat="server" ErrorMessage="Reason for write off book should not be blank."
                                                    CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" Display="None"
                                                    ClientValidationFunction="validateWriteOffBook"></asp:CustomValidator>
                                                <%--<asp:RequiredFieldValidator ID="reqWriteOffBook" runat="server" ControlToValidate="txtWriteOffBook"
                                                    Display="None" CssClass="ClsMdtStar" ErrorMessage="Reason for write off book should not be blank."
                                                    EnableClientScript="true"></asp:RequiredFieldValidator>--%>
                                                <asp:RegularExpressionValidator ID="regWriteOffBook" runat="server" ControlToValidate="txtWriteOffBook"
                                                    Display="None" ErrorMessage="Reason for write off book should not exceed than 500 characters."
                                                    ValidationExpression="^[\s\S]{0,500}$" CssClass="ClsMdtStar"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnWriteOffBook" runat="server" Text="OK" CssClass="ClsBtn" OnClick="btnWriteOffBook_Click"
                                                    OnClientClick="if(!ConfirmWriteOffBook()){return false;}" />
                                                <asp:Button ID="btnCancelWriteOff" runat="server" Text="Cancel" CssClass="ClsBtn"
                                                    CausesValidation="false" OnClientClick="javascript:HideWriteOffPopup();return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtnSml" BorderStyle="Solid"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clienttxtReason = "<%=this.txtReason.ClientID %>"
        _clienttxtWriteOffResone = "<%=this.txtWriteOffBook.ClientID %>"
        _clientvalsumRemoveBook = "<%=this.valsumRemoveBook.ClientID %>"
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
        function closewindow() {
            window.opener.location.href = window.opener.location.href
            if (window.opener.progressWindow)
                window.opener.progressWindow.close()
            window.close()
        }
        function ConfirmRemove() {
            var bResult = true
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sMsg = "Are you sure you want to remove this book?"
            if (!window.confirm(sMsg)) {
                bResult = false
            } else
                HidePopup()
            return bResult
        }
        function ShowPopup(e, iRowIndex) {
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnBookRemove.ClientID %>")
            document.getElementById(_clienttxtReason).value = ''
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "visible"
            cssstyleMain.display = "block"
            var now = new Date()
            $get("<%=this.hidRowIndex.ClientID %>").value = iRowIndex
            var width = 200
            var height = 100
            var pageWidth = 680
            var pageHeight = 500
            var left = parseInt((pageWidth / 2) - (width / 2))
            var top = parseInt((pageHeight / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 500)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }
        function HidePopup() {
            $get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
            if (document.getElementById(_clientvalsumRemoveBook) != null)
                document.getElementById(_clientvalsumRemoveBook).style.display = "none"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') { }
            if (validationResult == false) {
                return false
            }
            var sReason = document.getElementById(_clienttxtReason).value
            $get("<%=this.hidReason.ClientID %>").value = sReason
            var cssstyleMain = $get("<%=this.divMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
        function ConfirmWriteOffBook() {
            var bResult = true
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("")
            }
            if (validationResult == false) {
                return false
            }
            var sMsg = "Are you sure you want to write off this book?"
            if (!window.confirm(sMsg)) {
                bResult = false
            } else
                HideWriteOffPopup()
            return bResult
        }
        function ShowWriteOffPopup(e, iRowIndex) {
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.updtpnlWriteOffPopUp.ClientID %>").style
            var btnReturn = $get("<%=this.btnWriteOffBook.ClientID %>")
            document.getElementById(_clienttxtWriteOffResone).value = ''
            var cssstyleMain = $get("<%=this.divWriteOffMain.ClientID %>").style
            cssstyleMain.visibility = "visible"
            cssstyleMain.display = "block"
            var now = new Date()
            $get("<%=this.hidRowIndex.ClientID %>").value = iRowIndex
            var width = 200
            var height = 100
            var pageWidth = 680
            var pageHeight = 500
            var left = parseInt((pageWidth / 2) - (width / 2))
            var top = parseInt((pageHeight / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 500)
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }
        function HideWriteOffPopup() {
            $get("<%=this.updtpnlWriteOffPopUp.ClientID %>").style.visibility = "hidden"
            $get("<%=this.updtpnlWriteOffPopUp.ClientID %>").style.display = "none"
            if (document.getElementById(_clientvalsumRemoveBook) != null)
                document.getElementById(_clientvalsumRemoveBook).style.display = "none"
            var validationResult = true
            if (typeof (Page_ClientValidate) == 'function') { }
            if (validationResult == false) {
                return false
            }
            var sWriteOffReason = document.getElementById(_clienttxtWriteOffResone).value
            $get("<%=this.HidWriteOff.ClientID %>").value = sWriteOffReason
            var cssstyleMain = $get("<%=this.divWriteOffMain.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"
            return false
        }
        function validateBookRemove(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "visible"
&& $get("<%=this.updtpnlWriteOffPopUp.ClientID %>").style.visibility == "hidden") {
                if (trimAll(document.getElementById(_clienttxtReason).value) == '') {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        function validateWriteOffBook(oSrc, args) {
            if ($get("<%=this.updtpnlPopUp.ClientID %>").style.visibility == "hidden"
&& $get("<%=this.updtpnlWriteOffPopUp.ClientID %>").style.visibility == "visible") {
                if (trimAll(document.getElementById(_clienttxtWriteOffResone).value) == '') {
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        function CloseWindow(queystring) {
            window.opener.location = window.opener.location.pathname + queystring;
            window.close();
            window.opener.focus();
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
