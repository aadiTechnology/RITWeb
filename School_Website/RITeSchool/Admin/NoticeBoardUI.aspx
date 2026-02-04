<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoticeBoardUI.aspx.cs" MasterPageFile="../MasterPages/MasterPage.master"
    Inherits="NoticeBoardUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" width="100%">
        <tr>
            <td style="height: 26px" >
                            <div style="background-color: #fcdf8a; text-align: center; border: maroon 1px solid;
                                padding: 3px;">
                                <marquee style="cursor: progress; border-top-width: thin; vertical-align: bottom;
                                    color: MediumVioletRed; border-top-color: blue;" behavior="scroll" direction="left"
                                    scrollamount="2" scrolldelay="1" onmouseover="javascript:this.setAttribute('scrollamount','0');" onmouseout="javascript:this.setAttribute('scrollamount','2 ');">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
                                    <ContentTemplate>
                                        <asp:Label id="lblNoticeBoardMsg" runat="server" CssClass="LblNrmlB"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel></marquee>
                            </div>
                        </td>
        </tr>
        <tr>
            <td align="center">
                <table  cellpadding="0" cellspacing="0" style="width: 100%" id="tblNoticeMessage">
                    <tr>
                       
                         <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valsumNoticeBoard" runat="server" CssClass="LblErrorMsg" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdNoticeBoard" EventName="RowCommand" />
                        
                    </Triggers>
                </asp:UpdatePanel>
            </td>
                        <td width="15%" align="right" valign="top">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UPanelInput" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                    <ContentTemplate>
                        <table border="0" cellpadding="0">
                            <tr>
                                <td class="">
                                    <table>
                                        <tr>
                                            <td class="ClsBorderLight">
                                                    <span class="ClsLabel" style="width:67px">Message : </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMessage" runat="server" CssClass="LrgTxtBox" MaxLength="300"
                                                    Width="500px"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="">
                                    <table width="100%">
                                        <tr>
                                            <td width="10%" class="ClsBorderLight">
                                                 <span class="ClsLabel" style="width:67px">Start Date : </span>
                                            </td>
                                            <td width="25%" id="tdStartDate" >
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="97px" CssClass="LrgTxtBox" MaxLength="11"></asp:TextBox>                                               
                                                <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid start date." />
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                            <td align="left" rowspan="2" valign="top">
                                                <table width="100%" class="ClsBorderLight">
                                                    <tr>
                                                        <td width="20%">
                                                          <asp:Label ID="lblApplicableTo" runat="server" CssClass="ClsBorderLight" EnableViewState="False"
                                                                Text="Applicable To  :" Width="87px"></asp:Label> 
                                                        </td>
                                                        <td width="80%">
                                                            <asp:CheckBoxList ID="chkListRoles" runat="server" CellPadding="0" CellSpacing="0"
                                                                CssClass="ClsBorderLight" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="1%">
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%" class="ClsBorderLight">
                                                <span class="ClsLabel" style="width:67px">End Date : </span>
                                            </td>
                                            <td id="td2" width="25%" >
                                                <asp:TextBox ID="txtEndDate" runat="server" Width="97px" CssClass="LrgTxtBox" MaxLength="11"></asp:TextBox>
                                                <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtEndDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid End Date." Format="dd MMM yyyy" ShowWeekend="True" />
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqMessage" runat="server" ControlToValidate="txtMessage"
                                        CssClass="LblErrorMsg" Display="None" ErrorMessage="Message should not be blank."></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cstValGrid" runat="server" CssClass="LblErrorMsg" Display="None"
                                        EnableClientScript="true" SetFocusOnError="True" ControlToValidate="txtMessage"
                                        ClientValidationFunction="validateGridData"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="reqStartDt" runat="server" ControlToValidate="txtStartDate"
                                        CssClass="LblErrorMsg" Display="None" ErrorMessage="Start Date should not be blank."></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cstStartDate" Display="None" runat="server" CssClass="LblErrorMsg"
                                        EnableClientScript="true" SetFocusOnError="True" ControlToValidate="txtStartDate"
                                        ClientValidationFunction="checkStartDate"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="reqEndDt" runat="server" ControlToValidate="txtEndDate"
                                        CssClass="LblErrorMsg" Display="None" ErrorMessage="End Date should not be blank."></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cstEndDate" Display="None" runat="server" CssClass="LblErrorMsg"
                                        EnableClientScript="true" SetFocusOnError="True" ControlToValidate="txtEndDate"
                                        ClientValidationFunction="checkEndDate"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstRoleValidate" runat="server" ClientValidationFunction="CheckBoxListRoles"
                                        ErrorMessage="At least one user role should be selected." Display="None" CssClass="LblErrorMsg"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnAdd" runat="server" CssClass="ClsBtn" Text="Add" OnClick="btnAdd_Click" disable-page="true" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="Cancel" CausesValidation="false"
                                        OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdNoticeBoard" EventName="RowCommand" />
                        
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:UpdatePanel ID="UPanelGridView" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>                        
                        <div id="divGridView" runat="server" style="width: 100%;">
                            <table width="100%">
                                <tr id="trTotalRec" runat="server">
                                    <td width="45%" align="left">
                                        <table width="90%">
                                            <tr>
                                                <td width="7%">
                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                        Text="Legend : " EnableViewState="false"></asp:Label>
                                                </td>
                                                <td width="2%">
                                                    <asp:Label ID="Label1" runat="server" BackColor="LightBlue" Height="20px" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px" EnableViewState="False">
                                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblCurrentVisibleText" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                        EnableViewState="False" Font-Bold="True" Text="Active Notice"></asp:Label>
                                                </td>
                                                <td width="2%">
                                                    <asp:Label ID="lblDefaultNoticeColor" runat="server" BackColor="Aqua" Height="20px"
                                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                        EnableViewState="False">
                                                    <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblDefaultNoticeText" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd"
                                                        EnableViewState="false" Font-Bold="True" Text="Default Notice"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="55%" align="left">
                                        <table id="tblRowCounts" runat="server">
                                            <tr>
                                                <td width="55%" align="left">
                                                    <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNormal">To</span>
                                                    <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                     <span class="LblNormal">Out Of </span>
                                                    <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                    <span class="LblNormal">Records</span>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView CssClass="GridBorder" ID="grdNoticeBoard" runat="server" Width="100%"
                                AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="1"
                                ForeColor="#333333" GridLines="None" EmptyDataText="No notice available." EmptyDataRowStyle-HorizontalAlign="Center"
                                PageSize="20" DataKeyNames="Message_Id,Is_Default_Msg" OnRowCommand="grdNoticeBoard_RowCommand"
                                OnRowCreated="grdNoticeBoard_RowCreated" OnRowDataBound="grdNoticeBoard_RowDataBound"
                                OnSorting="grdNoticeBoard_Sorting" AllowPaging="True">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message"
                                        HtmlEncode="false">
                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" Width="65%" />
                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle"
                                            Width="65%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Start_Date" HeaderText="Start Date" SortExpression="Start_Date"
                                        HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="End Date" DataField="End_Date" SortExpression="End_Date"
                                        HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="EDIT_MESSAGE"
                                                CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" ToolTip="Edit"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DELETE_MESSAGE"
                                                CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="6%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="6%" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="ClsGridRow" />
                                <HeaderStyle CssClass="ClsGridHeader" />
                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
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
                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                            </td>
                                        </tr>
                                    </table>
                                </PagerTemplate>
                            </asp:GridView>
                            <asp:HiddenField ID="hidCurrentDate" runat="server" />
                            <asp:HiddenField ID="hidMessageId" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                            <asp:HiddenField ID="hidIsNewMessage" runat="server" />
                            <asp:HiddenField ID="hidIsDefaultMsg" runat="server" />
                            <asp:HiddenField ID="hidAcademicYrStartDt" runat="server" />
                            <asp:HiddenField ID="hidAcademicYrEndDt" runat="server" />
                            <asp:HiddenField ID="hidRowIndex" runat="server" />
                            <asp:HiddenField ID="hidUpdateMode" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" EnableViewState="true"></asp:HiddenField>
                            <asp:HiddenField ID="hidSortExpression" runat="server" EnableViewState="true"></asp:HiddenField>
                            <asp:ObjectDataSource ID="ObjectDataSet" runat="server" EnablePaging="True" OnSelected="ObjectDataSet_Selected"
                                SelectCountMethod="CountNoticeBoardDetails" SelectMethod="GetNoticeBoardDetails"
                                EnableCaching="false" TypeName="BusinessLogic.NoticeBoardCollectionBL">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                    <asp:SessionParameter Name="aiAccYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="Int32" />
                                    <asp:ControlParameter Name="sortExp" ControlID="hidSortExpression" Type="String"
                                        PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </div>                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdNoticeBoard" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clienttxtMessage = "<%=this.txtMessage.ClientID %>"
        _clientgrdNoticeBoardId = "<%=this.grdNoticeBoard.ClientID %>"
        _clientcstValGrid = "<%=this.cstValGrid.ClientID %>"
        _clienthidRowIndex = "<%=this.hidRowIndex.ClientID %>"
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clienthidIsNewMsgId = "<%=this.hidIsNewMessage.ClientID %>"
        _clientCalenderStartId = "<%=this.calStartDate.ClientID %>"
        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
        _clienthidServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientcstmsgStart = "<%=this.cstStartDate.ClientID %>"
        _clientcstmsgEnd = "<%=this.cstEndDate.ClientID %>"
        _clienthidIsDefaultMsg = "<%=this.hidIsDefaultMsg.ClientID %>"
        _clienthidAcademicYrStartDt = "<%=this.hidAcademicYrStartDt.ClientID %>"
        _clienthidAcademicYrEndDt = "<%=this.hidAcademicYrEndDt.ClientID %>"
        _clientValSum = "<%=this.valsumNoticeBoard.ClientID %>"
        _clientChkLstRoleId = "<%=this.chkListRoles.ClientID%>"
        _clientUpdateMode = "<%=this.hidUpdateMode.ClientID%>"
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID%>"
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_beginRequest(BeginReqHandler)
        prm.add_endRequest(EndReqHandler)
        function CheckBoxListRoles(source, args) {
            var j = 0
            var checks = document.forms[0].elements
            var boxLength = checks.length
            for (i = 0; i < boxLength; i++) {
                if (checks[i].type == 'checkbox') {
                    if (checks[i].checked == true) {
                        j++
                    } 
                } 
            }
            if (j > 0) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
        }

        function checkStartDate(oSrc, args) {
            if (document.getElementById(_clienttxtStartDate).value != "") {
                var dtStartDate
                if (document.all)
                    dtStartDate = new Date((document.getElementById(_clienttxtStartDate).value).replace('-', ' '))
                else
                    dtStartDate = new Date(convertdate(document.getElementById(_clienttxtStartDate).value))
                var AcadYrStartDt = document.getElementById(_clienthidAcademicYrStartDt).value
                var dtAcadYrStartDt = new Date(AcadYrStartDt)
                var strStartYear = getDateString(dtAcadYrStartDt);
                var AcadYrEndDt = document.getElementById(_clienthidAcademicYrEndDt).value
                var dtAcadYrEndDt = new Date(AcadYrEndDt)
                var strEndYear = getDateString(dtAcadYrEndDt);
                var serverdate = document.getElementById(_clienthidServerDate).value
                var today = new Date(serverdate)
                var IsDefaultMsg = document.getElementById(_clienthidIsDefaultMsg).value
                var IsInUpdateMode = document.getElementById(_clientUpdateMode).value
                if (dtStartDate < dtAcadYrStartDt && IsDefaultMsg == "false") {
                    document.getElementById(_clientcstmsgStart).errormessage = "Start Date must be within current academic year (i.e between " + strStartYear + " and " + strEndYear + ")."
                    args.IsValid = false
                    return true
                }
                else if (dtAcadYrEndDt < dtStartDate && IsDefaultMsg == "false") {
                    document.getElementById(_clientcstmsgStart).errormessage = "Start Date must be within current academic year (i.e between " + strStartYear + " and " + strEndYear + ")."
                    args.IsValid = false
                    return true
                }
                else if (dtStartDate < today && IsDefaultMsg == "false" && IsInUpdateMode != "True") {
                    document.getElementById(_clientcstmsgStart).errormessage = "Start Date should not be past date."
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
        }

        function checkEndDate(oSrc, args) {
        document.getElementById(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy")
        var TodayDate = document.getElementById(_clienthidCurrentDate).value
            var dtToday
            if (document.all)
                dtToday = new Date(TodayDate.replace('-', ' '))
            else
                dtToday = new Date(convertdate(TodayDate))
            if (document.getElementById(_clienttxtEndDate).value != "") {
                var EndDate = document.getElementById(_clienttxtEndDate).value
                var dtEndDate
                if (document.all)
                    dtEndDate = new Date(EndDate.replace('-', ' '))
                else
                    dtEndDate = new Date(convertdate(EndDate))
                var AcadYrStartDt = document.getElementById(_clienthidAcademicYrStartDt).value
                var dtAcadYrStartDt = new Date(AcadYrStartDt)
                var strStartYear = getDateString(dtAcadYrStartDt);
                var AcadYrEndDt = document.getElementById(_clienthidAcademicYrEndDt).value
                var dtAcadYrEndDt = new Date(AcadYrEndDt)
                var strEndYear = getDateString(dtAcadYrEndDt);
                if (document.getElementById(_clienttxtStartDate).value != "") {
                    var dtStartDate
                    if (document.all)
                        dtStartDate = new Date((document.getElementById(_clienttxtStartDate).value).replace('-', ' '))
                    else
                        dtStartDate = new Date(convertdate(document.getElementById(_clienttxtStartDate).value))
                }
                var serverdate = document.getElementById(_clienthidServerDate).value
                var today = new Date(serverdate)
                var IsDefaultMsg = document.getElementById(_clienthidIsDefaultMsg).value
                var IsInUpdateMode = document.getElementById(_clientUpdateMode).value
                
                if (dtEndDate > dtAcadYrEndDt && IsDefaultMsg == "false") 
                {
                    document.getElementById(_clientcstmsgEnd).errormessage = "End Date must be within current academic year (i.e between " + strStartYear + " and " + strEndYear + ")."
                    args.IsValid = false
                    return true
                }
                else if (dtEndDate < dtAcadYrStartDt && IsDefaultMsg == "false") 
                {
                    document.getElementById(_clientcstmsgEnd).errormessage = "End Date must be within current academic year (i.e between " + strStartYear + " and " + strEndYear + ")."
                    args.IsValid = false
                    return true
                }
                else if (dtEndDate < dtToday) 
                {
                    document.getElementById(_clientcstmsgEnd).errormessage = "End Date should not be past date."
                    args.IsValid = false
                    return true
                }
                else if (document.getElementById(_clienttxtStartDate).value != "") 
                {
                    if (dtEndDate < dtStartDate && IsDefaultMsg == "false") {
                        document.getElementById(_clientcstmsgEnd).errormessage = "End date should be greater than or equal to start date."
                        args.IsValid = false
                        return true
                    } 
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
        }

        function iftrueCusVal(args) {
            args.IsValid = true
            return false
        }

        function iffalseCusVal(args) {
            args.IsValid = false
            return true
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm("Are you sure you want to delete this message?")) {
                bResult = false
            }
            return bResult
        }

        function ClearValSum() {
            if (document.getElementById(_clientValSum) != null)
                document.getElementById(_clientValSum).style.display = "none"
            return true
        }

        function getDateString(obj) {
            var strDate = obj.getDate() + "-"
            var strMonth = parseInt(obj.getMonth())
            strMonth = months[strMonth]
            strDate = strDate + strMonth + "-"
            strDate = strDate + obj.getFullYear()
            return strDate
        }

        function validateGridData(oSrc, args) {
            var grdViewElement = document.getElementById(_clientgrdNoticeBoardId)
            var message = document.getElementById(_clienttxtMessage).value
            var cutvaltxtMessage = document.getElementById(_clientcstValGrid)
            var rowIndexId = document.getElementById(_clienthidRowIndex).value
            var isDuplicate = 0
            var iCnt = grdViewElement.rows.length
            if (message != "") {
                var sWhere = ""
                for (i = 1; i < iCnt; i++) {
                    var j = i
                    if (rowIndexId == "" + (j - 1)) {
                        sWhere = false
                    }
                    else {
                        sWhere = true
                    }
                    var msg = grdViewElement.rows[i].cells[0].innerHTML
                    var msg1 = msg.toLowerCase().trim()
                    var message1 = message.toLowerCase().trim()
                    if (message1 == msg1 && sWhere) {
                        isDuplicate = isDuplicate + 1
                        break
                    } 
                }
                if (isDuplicate > 0) {
                    cutvaltxtMessage.errormessage = "Message already exists."
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
        }

        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnAdd || postBackElement.id == _clientbtnCancel)
                DisableButtons(true, sender)
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnAdd || postBackElement.id == _clientbtnCancel)
                DisableButtons(false, sender)
        }

        function DisableButtons(action, sender) {
            var isPageValid = true
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnAdd) {
                if (typeof (Page_ClientValidate) == 'function' && action)
                    isPageValid = Page_ClientValidate()
            }
            if (isPageValid) {
                if (document.getElementById(_clientbtnAdd) != null)
                    document.getElementById(_clientbtnAdd).disable = action
                if (document.getElementById(_clientbtnCancel) != null)
                    document.getElementById(_clientbtnCancel).disable = action
            } 
        }
    </script>
</asp:Content>
