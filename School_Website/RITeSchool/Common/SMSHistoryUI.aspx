<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="SMSHistoryUI.aspx.cs" Inherits="SMSHistoryUI" ViewStateMode="Disabled"%>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%; height: 100%">
                        <tr>
                            <td align="left" colspan="4" valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ViewStateMode="Enabled">
                                    <ContentTemplate>
                                        <table style="width: 100%" cellspacing="1" cellpadding="0" border="0">
                                            <tbody>
                                                <tr id="trSMSStatus" runat="server" visible="false">
                                                    <td colspan="3" align="right">
                                                        <div style="padding-right: 5px; padding-top: 3px; width: 150px;" class="ClsGreenBG">
                                                             <asp:LinkButton ID="lnkSMSStatus" runat="server" Text="SMS Delivery Status "
                                                                        CssClass="SubTitle"></asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trHeaderMessage" runat="server">
                                                    <td class="ClsHilightBGB " id="tdlblUpdateMobile" runat="server" align="center" colspan="3">
                                                        <asp:Label ID="lblMobileNo" runat="server" EnableViewState="False" Text="School SMS will be sent to these number(s). To add/update the number, please send the information to Admin Staff via Message Center."></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="3">
                                                        <table cellpadding="0" cellspacing="3">
                                                            <tr runat="server" id="trMessage" visible="false">
                                                                <td align="center" colspan="4">
                                                                    <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                                        EnableViewState="False" ForeColor="Blue"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="trMobileSearch" runat="server">
                                                                <td id="tdlblMobileNo" runat="server" class="ClsBorderlight" colspan="1" width="130xp"
                                                                    valign="middle">
                                                                    <asp:Label ID="lblMob1" runat="server" EnableViewState="true" CssClass="ClsLabel"
                                                                        Text="Mobile Number1 :"></asp:Label>
                                                                </td>
                                                                <td class="ClsBorderlight" style="width: 100px; border: 01px solid #ddd;">
                                                                    <asp:Label ID="lblMobileOne" CssClass="ClsLabelNrml" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td id="tdlblMobile2" class="ClsBorderlight" runat="server" colspan="1" width="130px"
                                                                    valign="middle">
                                                                    <asp:Label ID="lblMob2" runat="server" CssClass="ClsLabel" Text="Mobile Number2 :"></asp:Label>
                                                                </td>
                                                                <td id="tdtxtMobile2" style="width: 100px;" runat="server" class="ClsBorderlight">
                                                                    <asp:Label ID="lblMobileTwo" CssClass="ClsLabelNrml" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" align="center">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table align="center" id="tblSearch" runat="server">
                                                            <tr>
                                                                <td class="ClsBorderlight" style="width: 223px">
                                                                    <span class="ClsLabel">Name / Reg. No. / User Name : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ClsBorderlight" style="width: 223px">
                                                                    <span class="ClsLabel">Content : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtContent" runat="server" MaxLength="100" CssClass="LrgTxtBox"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="center">
                                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtnMid" BorderWidth="1px"
                                                                        BorderStyle="Solid" OnClick="btnSearch_Click" Width="99px"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblError" runat="server" visible="false" align="left">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblErrorMesage" EnableViewState="false" runat="server" CssClass="LblErrorMsg" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left">
                                                                        &nbsp; &nbsp;&nbsp;
                                                                    </td>
                                                                    <td style="width: 25%" align="right">
                                                                        <asp:Button ID="imgBtnNewMessage" OnClick="imgBtnComposeMessage_Click" runat="server"
                                                                            Text="New SMS" CssClass="ClsBtnLrg" BorderWidth="1px" BorderStyle="Solid"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trTotalRec" align="center">
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">To</span>
                                                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">Out Of </span>
                                                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                        <span class="LblNormal">Records </span>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table id="tblLegend" runat="server" visible="false">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                                        Text="Legend : " EnableViewState="false"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="Label1" runat="server" BackColor="LightBlue" Height="20px" BorderColor="Black"
                                                                        BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False">
                                                        <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCurrentVisibleText" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                                        EnableViewState="False" Font-Bold="True" Text="Processed SMS"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" valign="top" align="right" colspan="3">
                                                        <asp:GridView CssClass="GridBorder" ID="grdvwMessageInbox" runat="server" ForeColor="#333333"
                                                            OnRowDataBound="grdvwMessageInbox_RowDataBound" OnPageIndexChanging="grdvwMessageInbox_PageIndexChanging"
                                                            OnRowCreated="grdvwMessageInbox_RowCreated" AllowSorting="True" OnSorting="grdvwMessageInbox_Sorting"
                                                            GridLines="None" CellSpacing="1" CellPadding="0" AllowPaging="True" Width="100%"
                                                            DataKeyNames="SMS_Id,SMS_Receiver_Details_Id,Read_Message_Flag,StatusId,SMSShootId" OnDataBound="grdvwMessageInbox_DataBound"
                                                            AutoGenerateColumns="False">
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                            </PagerStyle>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <input id="ChkAllDel" type="checkbox" runat="server" onclick="ChkAll(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                    <HeaderStyle Width="30px" />
                                                                </asp:TemplateField>
                                                                <asp:ButtonField HeaderText="Flag" ButtonType="Image">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="60px" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="SenderName" HeaderText="From">
                                                                    <ItemStyle HorizontalAlign="Left" Width="400px" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Left" Width="400px" VerticalAlign="Middle" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="To" SortExpression="SMS_Master.Sender_Name">
                                                                    <ItemStyle HorizontalAlign="Left" Width="400px" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" Width="400px" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Receiver_Mobile_No" HeaderText="Mobile No.">
                                                                    <ItemStyle HorizontalAlign="Center" Width="200px" VerticalAlign="Middle" CssClass="ClspaddingMinL" />
                                                                    <HeaderStyle HorizontalAlign="Center" Width="200px" VerticalAlign="Middle" CssClass="ClspaddingMinL" />
                                                                </asp:BoundField>
                                                                <asp:HyperLinkField DataNavigateUrlFields="SMS_Id,SMS_Receiver_Details_Id" DataNavigateUrlFormatString="~/Common/SMSUI.aspx?SMS_Id={0};SMSReceiverDetailsId={1}"
                                                                    DataTextField="Subject" HeaderText="SMS Text" SortExpression="SMS_Text">
                                                                    <ItemStyle HorizontalAlign="Left" Width="500px" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" Width="500px" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:HyperLinkField>
                                                                <asp:BoundField DataField="Insert_Date" HeaderText="Received Date" SortExpression="Insert_Date">
                                                                    <ItemStyle HorizontalAlign="Center" Width="180px" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="180px" VerticalAlign="Middle" />
                                                                    <HeaderStyle HorizontalAlign="Center" Width="180px" VerticalAlign="Middle" Wrap="False" />
                                                                </asp:BoundField>
                                                                 <asp:HyperLinkField Text="&lt;img src='../images/IconGrid_Edit.GIF' alt='alternate text' border='0'/&gt;" HeaderText="Resend">
                                                                 <ItemStyle HorizontalAlign="Center" Width="80px"  /> 
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:HyperLinkField>
                                                                <asp:HyperLinkField Text="&lt;img src='../images/iconGridSml_ViewGE.gif' alt='alternate text' border='0'/&gt;" HeaderText="Status">
                                                                 <ItemStyle HorizontalAlign="Center" Width="80px"  /> 
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                                                    </asp:HyperLinkField>
                                                            </Columns>
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                            <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
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
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource TypeName="BusinessLogic.SMSMasterCollectionBL" EnablePaging="true"
                                                            ID="GrdDSobj" runat="server" SelectMethod="GetReceivedSMSItemsForUser" SortParameterName="sortExpression"
                                                            SelectCountMethod="CountSMS" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                <asp:SessionParameter Name="aiUserRoleId" SessionField="S_USERLOGIN_ROLE_ID" Type="int32" />
                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                    Type="int32" />
                                                                <asp:ControlParameter Name="asName" PropertyName="Value" ControlID="hidName" DefaultValue=""
                                                                    Type="String" />
                                                                <asp:ControlParameter Name="asContent" PropertyName="Value" ControlID="hidContent"
                                                                    DefaultValue="" Type="String" />
                                                                <asp:ControlParameter Name="aiShowAllSMS" PropertyName="Value" ControlID="hidShowAllSendSMS"
                                                                    DefaultValue="0" Type="int32" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 15px" align="left" colspan="3">
                                                        <table height="100%" cellspacing="1" cellpadding="0" width="100%" align="center"
                                                            border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left" style="width: 80%;">
                                                                        <asp:Button ID="imgbtnBack" OnClick="imgbtnBack_Click" runat="server" Text="Back"
                                                                            CssClass="ClsBtn" Visible="True" CausesValidation="False">
                                                                        </asp:Button>
                                                                        <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Text="Delete"
                                                                            CssClass="ClsBtn" Visible="True"></asp:Button>
                                                                        <asp:Button ID="btnExport" runat="server" Text="Export"
                                                                            CssClass="ClsBtn" Visible="false"
                                                                            onclick="btnExport_Click"></asp:Button>
                                                                    </td>
                                                                    <td align="right" style="width: 20%;">
                                                                        <asp:Button ID="imgBtnComposeMessage" OnClick="imgBtnComposeMessage_Click" runat="server"
                                                                            Text="New SMS" CssClass="ClsBtnLrg" BorderWidth="1px" BorderStyle="Solid"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 5px; width: 20%" align="left">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td style="padding-right: 10px; width: 18%" align="right">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 18%; height: 20px" align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidQueryStrViewMode" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hidDeleteCnt" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hidCanEdit" runat="server" Value="N" />
                                                        <asp:HiddenField ID="hidName" runat="server" />
                                                        <asp:HiddenField ID="hidContent" runat="server" />
                                                        <asp:HiddenField ID="hidQuerryString" runat="server" />
                                                        <asp:HiddenField ID="hidShowAllSendSMS" runat="server" />
                                                    </td>
                                                    <td style="width: 23%; height: 20px" align="left">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientIdGrid = "<%=this.grdvwMessageInbox.ClientID%>"
        _clientIdhidDeleteCnt = "<%=this.hidDeleteCnt.ClientID%>"
        _clienttrMessage = "<%=this.trMessage.ClientID %>"
        _clientIdlblMessageCnt = "<%=this.lblMessage.ClientID%>"
        function ChkAll(obj) {
            CheckAllOrUncheckAllGridItems(document, _clientIdGrid, obj, 'ChkBoxDelete', false)
            if (obj.checked) {
                if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _clientIdGrid, 'ChkBoxDelete', 'a', 'false', 20, 'false'))
                    document.getElementById(_clientIdhidDeleteCnt).value = document.getElementById(_clientIdGrid).rows.length - 1;
                else
                    document.getElementById(_clientIdhidDeleteCnt).value = '0';
            }
            else {
                document.getElementById(_clientIdhidDeleteCnt).value = '0';
            }
        }

        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true
            var iCnt = parseInt(document.getElementById(_clientIdhidDeleteCnt).value)
            if (iCnt > 0) {
                if (sActionName == 'At least one SMS should be selected for deletion.') {
                    if (window.confirm("Are you sure you want to delete the selected SMS(s)?")) {
                        document.getElementById(_clientIdhidDeleteCnt).value = '0'
                    } else {
                        bResult = false
                    }
                }
            }
            else {
                if (sActionName == null)
                    alert("No checkbox selected for this action.")
                else
                    alert(sActionName)
                bResult = false
            }
            return bResult
        }
        function ConfirmDeArchive(iPageCount, sActionName, IsArchive) {
            var bResult = true
            var iCnt = parseInt(document.getElementById(_clientIdhidDeleteCnt).value)
            if (iCnt > 0) {
                if (sActionName == 'At least one message should be selected for trash.') {
                    if (!window.confirm("Are you sure you want to trash the selected message(s)?"))
                    { bResult = false }
                }
                else {
                    if (!window.confirm("Are you sure you want to Un-Delete the selected message(s)?"))
                    { bResult = false }
                }
            }
            else {
                if (sActionName == null)
                    alert("No checkbox selected for this action.")
                else
                    alert(sActionName)
                bResult = false
            }
            return bResult
        }
        function UpdateDeleteCount(aCheckBox) {

            var iCnt = parseInt(document.getElementById(_clientIdhidDeleteCnt).value)
            if (aCheckBox.checked == true)
                iCnt = iCnt + 1
            else {
                iCnt = iCnt - 1
            }
            var allChecked = ChkSelectStatus(_clientIdGrid.value);
            $('input[type=checkbox][id$=ChkAllDel]').get(0).checked = allChecked;
            document.getElementById(_clientIdhidDeleteCnt).value = iCnt
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
        function ChkSelectStatus(src) {
            var chkTotalCount = $('input[type=checkbox][id$=ChkBoxDelete]:not(:disabled)', src).length;
            var chkSelectedCount = $('input[type=checkbox][id$=ChkBoxDelete]:checked', src).length;
            return chkTotalCount == chkSelectedCount;
        }

        function OpenSMSStatusPopup() {
            window.open('SMSStatusPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=750').focus();
        }

    </script>
</asp:Content>
