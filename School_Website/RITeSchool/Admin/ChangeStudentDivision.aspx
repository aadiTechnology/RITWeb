<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="ChangeStudentDivision.aspx.cs" Inherits="ChangeStudentDivision" EnableEventValidation="false" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div class="MainBodyDiv" align="center">
            <table width="97%" id="trPrecondition" runat="server" visible="false">
                <tr>
                    <td>
                        <div style="width: 97%;" runat="server" id="divErr" visible="false">
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 97%;">
                <tr>
                    <td id="MainDataTable" align="center" valign="top">
                        <!-- Data Insert Here -->
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                            <tr>
								<td id="tdValidationSummary" runat="server" align="left" colspan="2">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                        <asp:ValidationSummary ID="ValSum" runat="server" ShowSummary="true" CssClass="ClsLabel">
                                        </asp:ValidationSummary>                                       
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdMessage" runat="server">
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="ClsMdtStar"></asp:Label>
                                    </ContentTemplate>                                    
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="middle">
									<table>
										<tr id="trLegend" runat="server">
                                            <td align="left">
                                                <asp:Label ID="Label14" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                    Text="Legend" EnableViewState="false"></asp:Label>
                                            </td>
                                            <td align="left" style="padding-right: 3px">
                                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                    BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label15" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text="Student Fee Paid"
                                                    EnableViewState="false"></asp:Label>
                                            </td>										
										</tr>
                                    </table>
                                </td>
                                <td class="ClsMdtStar" align="right">
                                    * Mandatory Fields
                                </td>
                            </tr>
                            
                            <tr>
                                <td align="center" style="width: 60%" valign="top" class="td-vertical-align-top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr id="trMiddle" runat="server">
                                            <td align="center" valign="top">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="1" style="width: 100%">
                                                            <tr>
                                                                <td colspan="6" style="width: 100%">
                                                                    <table id="tblTransferMessage" width="100%" runat="server">
                                                                        <tr>
                                                                            <td align="center" class="ClsHilightBGB">
                                                                                <asp:Label ID="lblTransferMessage" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr2" runat="server">
                                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                                    <asp:Label ID="lblCurrentStandard" runat="server" CssClass="ClsHilightTextB" Text="Current Standard : "></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlCurrentStandard" runat="server" AutoPostBack="true" Style="width: 90px;
                                                                        height: 20px;" CssClass="MidCombo" OnSelectedIndexChanged="ddlCurrentStandard_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                </td>
                                                                <td align="left" colspan="1" valign="middle" id="tdArrow1" runat="server">
                                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                                </td>
                                                                <td align="left" colspan="1" valign="middle" id="tdArrow2" runat="server">
                                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" id="tdTargerStdLabel" runat="server">
                                                                    <asp:Label ID="Label16" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                        Text="Target Standard : "></asp:Label>
                                                                </td>
                                                                <td id="tdTargetStdCombo" runat="server">
                                                                    <asp:DropDownList ID="ddlTargetStandard" runat="server" CausesValidation="false"
                                                                        Style="width: 90px;" CssClass="MidCombo" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetStandard_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CustomValidator ID="cstTargerStandard" runat="server" Display="None" ClientValidationFunction="ValidateTargetStandard"
                                                                        ErrorMessage="Target standard should be selected." CssClass="ClsMdtStar"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr id="Tr1" runat="server">
                                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                        Text="Current Division : "></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlCurrentDiv" runat="server" Style="width: 90px; height: 20px;"
                                                                        CssClass="MidCombo" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentDiv_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                </td>
                                                                <td align="left" colspan="1" valign="middle">
                                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                                </td>
                                                                <td align="left" colspan="1" valign="middle">
                                                                    <img src="../images/ArrowBlueDblNw.gif" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                        Text="Target Division : "></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTargetDiv" runat="server" Style="width: 90px;" CausesValidation="false"
                                                                        CssClass="MidCombo" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                    <span class="ClsMdtStar">*</span>
                                                                    <asp:CustomValidator ID="cstStandardDivision" runat="server" CssClass="ClsMdtStar"
                                                                        Display="None" ClientValidationFunction="CheckStandardDivision"></asp:CustomValidator>
                                                                     <asp:CustomValidator ID="CustValidator" runat="server" CssClass="ClsMdtStar" Enabled="false"
                                                                     OnServerValidate="Validate_Student" Display="None"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                                <ContentTemplate>
                                                                    <table width="100%" id="tblSearch" runat="server">
                                                                        <tr>
                                                                            <td width="130px" class="ClsBorderlight">
                                                                                <asp:Label ID="lblSearch" runat="server" CssClass="ClsHilightTextB" Text="Name / Reg. No. : "
                                                                                    EnableViewState="False"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="MidTxtBox" Style="width: 100%" autocomplete="off"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" width="100px">
                                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                                                                    CausesValidation="False" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrentDiv" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrentStandard" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center" valign="top">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                                            <ContentTemplate>
                                                                                <table width="100%">
                                                                                    <tr runat="server" id="trTotalRec" align="center">
                                                                                        <td>
                                                                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblTo" runat="server" Text=" To " CssClass="LblNormal" EnableViewState="False" />
                                                                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblOutOf" runat="server" Text=" Out Of " CssClass="LblNormal" EnableViewState="False" />
                                                                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblRecords" runat="server" Text="Records " CssClass="LblNormal" EnableViewState="False" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center" valign="top" id="tdGrid" runat="server">
                                                                                            <div id="divContainer" class="GridBorder" runat="server" visible="false" style="width: 500px;
                                                                                                height: 390px; overflow: scroll">
                                                                                                <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" DataKeyNames="YearWise_Student_Id"
                                                                                                    AutoGenerateColumns="False" AllowSorting="True" Width="100%" PageSize="100" CellPadding="0"
                                                                                                    CellSpacing="1" ForeColor="#333333" GridLines="None" OnSorting="grdStudents_Sorting"
                                                                                                    OnPageIndexChanging="grdStudents_PageIndexChanging" OnRowDataBound="grdStudents_RowDataBound">
                                                                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                                                    </PagerStyle>
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField>
                                                                                                            <HeaderTemplate>
                                                                                                                <input id="ChkAllDel" type="checkbox" runat="server" onclick="CheckUncheckAllGridItems(_sClientGridId,this)" /> <%--onclick="CheckAllOrUncheckAllGridItems(document,_sClientGridId,this,'ChkBoxDelete', false)" />--%>
                                                                                                            </HeaderTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="ChkBoxDelete" runat="server" />
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                                                                                            <HeaderStyle Width="1%" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="Enrolment_Number" HeaderText="Reg. No." SortExpression="Enrolment_Number">
                                                                                                            <ItemStyle Width="8%" CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                            <HeaderStyle Width="8%" CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle"
                                                                                                                Wrap="False" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Roll_No" HeaderText="Roll No." SortExpression="Roll_No">
                                                                                                            <ItemStyle Width="8%" CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                            <HeaderStyle Width="8%" CssClass="ClspaddingL" HorizontalAlign="Left" VerticalAlign="Middle"
                                                                                                                Wrap="False" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="First_Name">
                                                                                                            <ItemStyle Width="85%" HorizontalAlign="Left" CssClass="ClspaddingL" VerticalAlign="Middle" />
                                                                                                            <HeaderStyle HorizontalAlign="Left" CssClass="ClspaddingL" VerticalAlign="Middle"
                                                                                                                Width="85%" Wrap="False" />
                                                                                                        </asp:BoundField>
                                                                                                    </Columns>
                                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                                            <asp:HiddenField ID="hidBackUrl" runat="server" />
                                                                                            <asp:HiddenField ID="hidPaidFeesStudentIds" runat="server" />
																							<asp:HiddenField ID="hidIsAcrossStandard" runat="server" value="0"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                                            <ContentTemplate>
                                                                                <table align="center">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td align="right" id="tdTransfer" runat="server">
                                                                                                        <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="ClsBtn" Height="26px"
                                                                                                            Width="110px" OnClick="btnTransfer_Click" disable-page="true" Visible="false"  />
                                                                                                    </td>
                                                                                                    <td align="left">
                                                                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" Height="26px"
                                                                                                            CausesValidation="false" UseSubmitBehavior="false" OnClientClick="Page_BlockSubmit = false;" />
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
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center" style="width: 40%;" valign="top" id="tdInfo" runat="server" class="td-vertical-align-top">
                                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                            <table width="100%">
                                                <tr id="trStdChkLst" runat="server">
                                                    <td class="ClsBorderlight " style="background-color: #ffffc4;">
                                                    <asp:Label ID="Label17" runat="server" BorderWidth="0px" Text="Before proceeding to transfer student, please verify the following document:"
                                                     EnableViewState="False" CssClass="LblNrmlB" Font-Bold="True"></asp:Label>                                                   
                                                     <asp:HyperLink  ID="hlnkChecklist" CssClass="LblNrmlB" runat="server" Text="Student Transfer Document" NavigateUrl="~/RITeSchool/DOWNLOADS/StudentTransferChecklist.xlsx"/>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                                
                                               </td>
                                        </tr>
                                        <tr>
                                            <td style="height:3px;"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight " style="width: 30%; background-color: #ffffc4;">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                    CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; width: 70%">
                                                <asp:Label ID="Label4" runat="server" BorderWidth="0px" Text="If current division's subject is not in the target division then student will loose this subject's marks."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 3px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Note 2 :" CssClass="LblNrmlB"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label6" runat="server" BorderWidth="0px" Text="If current division's subject total marks is not equal to the target division's subject then also student(s) will loose this subject's marks."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 3px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Note 3 :" CssClass="LblNrmlB"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label13" runat="server" BorderWidth="0px" Text="If current and target division's subject exam configuration is different then also student(s) will loose this subject's marks."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 3px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Note 4 :" CssClass="LblNrmlB"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label7" runat="server" BorderWidth="0px" Text="If target division's attendance for particular date is not marked then student(s) will loose their attendance for that date."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 3px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Note 5 :" CssClass="LblNrmlB"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label9" runat="server" BorderWidth="0px" Text="If student's current division's attendance is not marked but target division's attendance is marked then student's attendance will marked as present for that date."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 3px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label10" runat="server" Text="Note 6 :" CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label11" runat="server" BorderWidth="0px" Text="Student(s) will get new roll number."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="background-color: #ffffc4">
                                                <asp:Label ID="Label18" runat="server" Text="Note 7 :" CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="Label19" runat="server" BorderWidth="0px" Text="Fee: All the fees related to student(s) will remain as it is and if target standard-division having extra fees then those fees will also be applied to transferred student(s)."
                                                    CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <script language="javascript" type="text/javascript">

            _sClientGridId = "<%=this.grdStudents.ClientID %>";
            _ClientbtnTransfer = "<%=this.btnTransfer.ClientID %>";
            _ClientbtnCancel = "<%=this.btnCancel.ClientID %>";

			_clienthidIsAcrossStandard = "<%=this.hidIsAcrossStandard.ClientID %>";
            _ClientddlSrcStandard = "<%=this.ddlCurrentStandard.ClientID %>";
            _ClientddlTargetStandard = "<%=this.ddlTargetStandard.ClientID %>";
            _ClientddlSrcDivision = "<%=this.ddlCurrentDiv.ClientID %>";
            _ClientddlTargetDivision = "<%=this.ddlTargetDiv.ClientID %>";
            _ClientvalStdDiv = "<%=this.cstStandardDivision.ClientID %>";
            _ClientvalTargetStd = "<%=this.cstTargerStandard.ClientID %>";

            var TargetStandardChanged = "Y";
            //This function is used to display confirmation message.
            var Page_IsValid = true;
            function ConfirmAction(iPageCount, sActionName) {
            	 Page_IsValid = true;
                var validationResult = true;
                TargetStandardChanged = "N";
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");

                if (validationResult == false)
                    return false;

                var bResult = true;
                if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'ChkBoxDelete', sActionName, 'false', iPageCount, 'true')) {
                	if ($get(_clienthidIsAcrossStandard).Value == "0") {
                        if (!window.confirm("Are you sure you want to change division of selected student(s)?"))
                            bResult = false;
							Page_IsValid = false;
                    } else {
			    if (!window.confirm("All of the paid fees of the student(s) will remain as it is after transfer. If the target class is having any different fees than the current class then those fees will be applied to student(s). Do you want to continue to transfer the student(s)?"))
                            bResult = false;
							Page_IsValid = false;
                    }
                }
                else {
                	bResult = false;
                	Page_IsValid = false;
                }
                return bResult;
            }
            //This function is used to disable buttons.
            function DisableButtons() {
                TargetStandardChanged = "Y";
                if (typeof (Page_ClientValidate) == 'function')
                    validationResult = Page_ClientValidate("");
                if (validationResult == false)
                    return false;
                else {
                    if (document.getElementById(_ClientbtnTransfer) != null)
                        document.getElementById(_ClientbtnTransfer).disabled = true;
                    document.getElementById(_ClientbtnCancel).disabled = true;
                }
            }
            //This function is used to validate standard and division.
            function CheckStandardDivision(oSrc, args) {
                if (document.getElementById(_sClientGridId) != null &&
                        document.getElementById(_ClientddlSrcDivision).value != 0) {
                    if (document.getElementById(_ClientddlSrcStandard) != null &&
                document.getElementById(_ClientddlSrcDivision) != null &&
                document.getElementById(_ClientddlTargetDivision) != null) {

                        var currentStd = document.getElementById(_ClientddlSrcStandard).value;
                        var currentDiv = document.getElementById(_ClientddlSrcDivision).value;
                        var TargetDiv = document.getElementById(_ClientddlTargetDivision).value;
                        var TargetStd = -1;
                        if (document.getElementById(_ClientddlTargetStandard) != null)
                            TargetStd = document.getElementById(_ClientddlTargetStandard).value;

                        if ((currentStd == 0 && currentDiv == 0 && TargetDiv == 0) || (currentStd != 0 && currentDiv != 0 && TargetStd == 0 && TargetDiv == 0 && TargetStandardChanged == "N")) {
                            document.getElementById(_ClientvalStdDiv).errormessage = "Target division should be selected.";
                            args.IsValid = false;
                            return true;
                        }
                        else if (currentStd != 0 && currentDiv != 0 && TargetStd != 0 && TargetStandardChanged == "N" && TargetDiv == 0 && TargetStandardChanged == "N") {
                            document.getElementById(_ClientvalStdDiv).errormessage = "Target division should be selected.";
                            args.IsValid = false;
                            return true;
                        }

                        else if (document.getElementById(_ClientddlTargetStandard) != null && TargetStandardChanged == "N") {
                            var TargetStd = document.getElementById(_ClientddlTargetStandard).value;
                            if (currentStd == TargetStd && currentDiv == TargetDiv) {
                                document.getElementById(_ClientvalStdDiv).errormessage = "Current class and target class should not be same.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                        else {
                            if (currentDiv == TargetDiv && TargetStandardChanged == "N") {
                                document.getElementById(_ClientvalStdDiv).errormessage = "Current Division and Target Division should not be same.";
                                args.IsValid = false;
                                return true;
                            }
                        }
                    }
                }
                args.IsValid = true;
                return false;
            }
            //This function is used to apply validation.
            function ValidateStandardDivisions() {
                TargetStandardChanged = "Y";
                if (document.getElementById(_sClientGridId) != null) {
                    if (typeof (Page_ClientValidate) == 'function') {
                        validationResult = Page_ClientValidate("");
                    }
                    if (validationResult == false) {
                        return false;
                    }
                }
                return true;
            }
            //This function is used to validate target standard.
            function ValidateTargetStandard(oSrc, args) {
                if (document.getElementById(_ClientddlTargetStandard) != null) {
                    var TargetStandard = document.getElementById(_ClientddlTargetStandard).value;
                    if (TargetStandardChanged == "N" && TargetStandard == "0") {
                        document.getElementById(_ClientvalTargetStd).errormessage = "Tagret standard shound be selected.";
                        args.IsValid = false;
                        return false;
                    }
                }
                args.IsValid = true;
                return false;
            }

            // This function is used to check/uncheck all ENABLED checkboxes in the given grid.
            function CheckUncheckAllGridItems(gridId, _chkbox) {
            	var grid = $get(gridId);
            	var chkboxes = grid.getElementsByTagName('input');
            	for (var i = 1; i < chkboxes.length; i++) {
            		var chkbox = chkboxes[i];
            		if (_chkbox.checked) {
            			if (!chkbox.disabled)
            				chkbox.checked = true;
            		}
            		else
            			chkbox.checked = false;
            	}
            }
        </script>
        <script language="javascript" type="text/javascript">

            $(document).ready(function () {
                AutoSearch();
            });

            function AutoSearch() {
                _clienttxtUserName = '#<%=txtSearch.ClientID%>';
                var SchoolId = "<%=miSchoolId %>";
                var AcademicYearId = "<%=miAcademicYearId %>"                
                var _clientddlStandard = '<%=ddlCurrentStandard.ClientID%>';
                var _clientddlDivision = '<%=ddlCurrentDiv.ClientID%>';

                BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtUserName, _clientddlStandard, _clientddlDivision, null, 0);
            }

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(EndRequestHandler);

            // This function is used to enabled controls once a postback is complete.
            function EndRequestHandler() {
                AutoSearch();
            }

            function SearchSelectedValue(val) {
                txt = document.getElementById("<%=this.txtSearch.ClientID %>");
                bt = document.getElementById("<%=this.btnSearch.ClientID %>");
                SearchResult(txt, val, bt);
            }

        </script>
    </div>
</asp:Content>
