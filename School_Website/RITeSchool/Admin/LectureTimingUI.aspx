<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LectureTimingUI.aspx.cs" Inherits="LectureTimingUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td align="left">
                    <table id="tblValSum" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <asp:ValidationSummary ID="valSumLectureTiming" runat="server" CssClass="LblErrorMsg" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UPanelErroMsg" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg"  EnableViewState="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        </table>
                </td>
            </tr>
            <tr style="width: 100%">
                <td align="center">
                    <asp:UpdatePanel ID="UPanelInput" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblLectureCntrol" runat="server" align="center" border="0" cellpadding="2"
                                cellspacing="1" style="width: 60%;">
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                            <span class="ClsLblLgnd">Standard :</span>
                                    </td>
                                    <td align="left" class="ClsHilightBG" valign="top">
                                        <asp:Label Font-Bold="True" ID="lblStandardName" runat="server" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                    <td align="right" Width="120px">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>   
                                                <asp:Label ID="lblMandatoryLegent" runat="server" CssClass="ClsMdtStar" 
                                                    Text="* Mandatory Fields" Width="120px" EnableViewState="False"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" style="height: 26px">
                                        <span class="ClsLabel">Section :</span>
                                    </td>
                                    <td style="height: 26px" colspan="2">
                                        <asp:DropDownList ID="ddlSection" runat="server" Width="110px" CssClass="SmlCombo"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Lecture Number :</span>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlLectureNo" runat="server" Width="110px" CssClass="SmlCombo"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlLectureNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Start Time :</span>
                                    </td>
                                    <td align="left" colspan="2" valign="top">
                                        <asp:DropDownList ID="ddlStartHr" runat="server">
                                            <asp:ListItem Value="1 AM">1 AM</asp:ListItem>
                                            <asp:ListItem Value="2 AM">2 AM</asp:ListItem>
                                            <asp:ListItem Value="3 AM">3 AM</asp:ListItem>
                                            <asp:ListItem Value="4 AM">4 AM</asp:ListItem>
                                            <asp:ListItem Value="5 AM">5 AM</asp:ListItem>
                                            <asp:ListItem Value="6 AM">6 AM</asp:ListItem>
                                            <asp:ListItem Value="7 AM">7 AM</asp:ListItem>
                                            <asp:ListItem Value="8 AM" Selected="True">8 AM</asp:ListItem>
                                            <asp:ListItem Value="9 AM">9 AM</asp:ListItem>
                                            <asp:ListItem Value="10 AM">10 AM</asp:ListItem>
                                            <asp:ListItem Value="11 AM">11 AM</asp:ListItem>
                                            <asp:ListItem Value="12 PM">12 PM</asp:ListItem>
                                            <asp:ListItem Value="1 PM">1 PM</asp:ListItem>
                                            <asp:ListItem Value="2 PM">2 PM</asp:ListItem>
                                            <asp:ListItem Value="3 PM">3 PM</asp:ListItem>
                                            <asp:ListItem Value="4 PM">4 PM</asp:ListItem>
                                            <asp:ListItem Value="5 PM">5 PM</asp:ListItem>
                                            <asp:ListItem Value="6 PM">6 PM</asp:ListItem>
                                            <asp:ListItem Value="7 PM">7 PM</asp:ListItem>
                                            <asp:ListItem Value="8 PM">8 PM</asp:ListItem>
                                            <asp:ListItem Value="9 PM">9 PM</asp:ListItem>
                                            <asp:ListItem Value="10 PM">10 PM</asp:ListItem>
                                            <asp:ListItem Value="11 PM">11 PM</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlStartMin" runat="server">
                                            <asp:ListItem Selected="True" Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="55">55</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" style="height: 5px">
                                        <span class="ClsLabel">End Time :</span>
                                    </td>
                                    <td colspan="2" style="height: 5px">
                                        <asp:DropDownList ID="ddlEndHr" runat="server">
                                            <asp:ListItem Value="1 AM">1 AM</asp:ListItem>
                                            <asp:ListItem Value="2 AM">2 AM</asp:ListItem>
                                            <asp:ListItem Value="3 AM">3 AM</asp:ListItem>
                                            <asp:ListItem Value="4 AM">4 AM</asp:ListItem>
                                            <asp:ListItem Value="5 AM">5 AM</asp:ListItem>
                                            <asp:ListItem Value="6 AM">6 AM</asp:ListItem>
                                            <asp:ListItem Value="7 AM">7 AM</asp:ListItem>
                                            <asp:ListItem Value="8 AM">8 AM</asp:ListItem>
                                            <asp:ListItem Value="9 AM" Selected="True">9 AM</asp:ListItem>
                                            <asp:ListItem Value="10 AM">10 AM</asp:ListItem>
                                            <asp:ListItem Value="11 AM">11 AM</asp:ListItem>
                                            <asp:ListItem Value="12 PM">12 PM</asp:ListItem>
                                            <asp:ListItem Value="1 PM">1 PM</asp:ListItem>
                                            <asp:ListItem Value="2 PM">2 PM</asp:ListItem>
                                            <asp:ListItem Value="3 PM">3 PM</asp:ListItem>
                                            <asp:ListItem Value="4 PM">4 PM</asp:ListItem>
                                            <asp:ListItem Value="5 PM">5 PM</asp:ListItem>
                                            <asp:ListItem Value="6 PM">6 PM</asp:ListItem>
                                            <asp:ListItem Value="7 PM">7 PM</asp:ListItem>
                                            <asp:ListItem Value="8 PM">8 PM</asp:ListItem>
                                            <asp:ListItem Value="9 PM">9 PM</asp:ListItem>
                                            <asp:ListItem Value="10 PM">10 PM</asp:ListItem>
                                            <asp:ListItem Value="11 PM">11 PM</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlEndMin" runat="server">
                                            <asp:ListItem Selected="True" Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="55">55</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="font-size: 9pt; color: #ff0000"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Description :</span>
                                            
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="LrgTxtBox" MaxLength="50"></asp:TextBox>
                                        <span id="lblMdtStarDescription" runat="server" class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="1">
                                    </td>
                                    <td align="left" style="height: 5px">
                                    </td>
                                    <td align="left" style="height: 5px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="1">
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click" CssClass="ClsBtn"
                                            BorderStyle="Solid" BorderWidth="1px" CausesValidation="true" UseSubmitBehavior="false" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderStyle="Solid"
                                            CausesValidation="false" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="height: 15px">
                                    </td>
                                    <td align="center" style="height: 15px">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdvwLectureTiming" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grdvwLectureTiming" EventName="Sorting" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>                
            </tr>              
            <tr>
                <td align="center" valign="top" class="ColorBg" style="width: 60%;">
                    <asp:UpdatePanel ID="UPanelGridView" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divGridView" runat="server" style="width: 100%;">
                                <asp:GridView ID="grdvwLectureTiming" runat="server" Width="100%" AutoGenerateColumns="False"
                                    PageSize="200" CellPadding="0" CellSpacing="1" GridLines="None" ForeColor="#333333"
                                    EmptyDataText="No Lecture Timing Record available." EmptyDataRowStyle-HorizontalAlign="Center"
                                    OnRowCommand="grdvwLectureTiming_RowCommand" OnRowCreated="grdvwLectureTiming_RowCreated"
                                    OnRowDataBound="grdvwLectureTiming_RowDataBound" OnSorting="grdvwLectureTiming_Sorting"
                                    DataKeyNames="School_LectureTimings_Detail_Id,School_LectureTimings_Id,Lecture_No,Description"
                                    AllowSorting="True" CssClass="TitleRBg">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                        Font-Size="Medium"></PagerStyle>
                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                    <Columns>
                                        <asp:BoundField HeaderText="Lectures" DataField="Lecture_No" ReadOnly="True" SortExpression="Lecture_No">
                                            <ItemStyle HorizontalAlign="Center" Width="25%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Start_Time" HeaderText="Start Time" SortExpression="sStart_Time">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="End_Time" HeaderText="End Time" SortExpression="sEnd_Time">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="EDIT_LECTURE_TIMING"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="DELETE_LECTURE_TIMING"
                                                    CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle CssClass="dataBG TxtNormal paddingL" />
                                    <HeaderStyle CssClass="ColorNewBg" />
                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlSection" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="grdvwLectureTiming" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="grdvwLectureTiming" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UPanelHiddenVar" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidIsAddMode" runat="server" />
                            <asp:HiddenField ID="hidLectureTimingId" runat="server" />
                            <asp:HiddenField ID="hidLibraryTimingDetailId" runat="server" />
                            <asp:HiddenField ID="hidRowIndex" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                        OnClick="btnBack_Click" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CustomValidator ID="cutValDescription" runat="server" ClientValidationFunction="ValidateDescription"
                        CssClass="LblErrorMsg" Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ClientValidationFunction="cstStartAndEndDate"
                        SetFocusOnError="True" Display="None" CssClass="LblErrorMsg"></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clienttxtDescriptionId = "<%=this.txtDescription.ClientID %>"
        _clientcutValDescriptionId = "<%=this.cutValDescription.ClientID %>"
        _clientcst_StartAndEndDate = "<%=this.cst_StartAndEndDate.ClientID %>"
        _clientddlStartHr = "<%=this.ddlStartHr.ClientID %>"
        _clientddlStartMin = "<%=this.ddlStartMin.ClientID %>"
        _clientddlEndHr = "<%=this.ddlEndHr.ClientID %>"
        _clientddlEndMin = "<%=this.ddlEndMin.ClientID %>"
        _clienthidServerDateId = "<%=this.hidServerDate.ClientID %>"
        _clientValSumId = "<%=this.valSumLectureTiming.ClientID %>"
        _clientErrorMsgId = "<%=this.lblErr.ClientID %>"
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnAdd)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnAdd).disabled = true
                    document.getElementById(_clientbtnCancel).disabled = true
                } 
            } 
        }
        function ConfirmDelete() {
            var bResult = ClearValSum()
            if (!window.confirm("Are you sure you want to delete this lecture timing ?")) {
                bResult = false
            }
            return bResult
        }
        function ValidateDescription(oSrc, args) {
            if (document.getElementById(_clienttxtDescriptionId).value == "" &&
document.getElementById(_clienttxtDescriptionId).disabled == false) {
                document.getElementById(_clientcutValDescriptionId).errormessage = "Description should not be blank."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            } 
        }
        function cstStartAndEndDate(aSrc, args) {
            var dtStartDate
            var dtEndDate
            var STime = document.getElementById(_clientddlStartHr).value.split(" ")
            var SMin = document.getElementById(_clientddlStartMin).value
            var ETime = document.getElementById(_clientddlEndHr).value.split(" ")
            var EMin = document.getElementById(_clientddlEndMin).value
            var isValid = true
            if (document.all) {
                if (isNaN(new Date((document.getElementById(_clienthidServerDateId).value).replace('-', ' '))))
                    isValid = false
            }
            else {
                if (isNaN(new Date((document.getElementById(_clienthidServerDateId).value).replace(/-/g, ' '))))
                    isValid = false
            }
            if (isValid) {
                if (document.all) {
                    dtStartDate = new Date((document.getElementById(_clienthidServerDateId).value).replace('-', ' ') + " " + STime[0] + ":" + SMin + " " + STime[1])
                    dtEndDate = new Date((document.getElementById(_clienthidServerDateId).value).replace('-', ' ') + " " + ETime[0] + ":" + EMin + " " + ETime[1])
                }
                else {
                    dtStartDate = new Date(convertdate((document.getElementById(_clienthidServerDateId).value) + " " + STime[0] + ":" + SMin + " " + STime[1]))
                    dtEndDate = new Date(convertdate((document.getElementById(_clienthidServerDateId).value) + " " + ETime[0] + ":" + EMin + " " + ETime[1]))
                }
                if (!(dtStartDate <= dtEndDate)) {
                    document.getElementById(_clientcst_StartAndEndDate).errormessage = "End time must be greater than start time."
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            } 
        }
        function ClearValSum() {
            if (document.getElementById(_clientValSumId) != null)
                document.getElementById(_clientValSumId).style.display = "none"
            if (document.getElementById(_clientErrorMsgId) != null)
                document.getElementById(_clientErrorMsgId).style.display = "none"
            return true
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
