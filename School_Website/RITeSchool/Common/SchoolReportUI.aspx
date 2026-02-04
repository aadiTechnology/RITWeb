<%@ Page Language="C#" AutoEventWireup="true" CodeFile="../Common/SchoolReportUI.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits=" SchoolReportsUI" ViewStateMode="Disabled"%>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register TagPrefix="cmb" TagName="Combobox" Src="~/UserControls/ComboRpt.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <script type="text/javascript" src="../Scripts/dtree.js"></script>

    <table style="width: 100%;" align="center">
        <tr align="center">
            <td style="text-align: left; width: 35%; height: 100;" valign="top" class="ClsBorderlight">
                <asp:Panel ID="reportsNavContainer" runat="server" ScrollBars="Auto" Height="390px">
                    <asp:Label ID="lblreportsNav" runat="server" ViewStateMode="Enabled" Text=""></asp:Label>
                </asp:Panel>
            </td>
            <td style="width: 62%; height: 100;" valign="top" class="ClsBorderlight td-vertical-align-top">
                <div id="divDisplayParameters" runat="server" visible="true">
                    <table id="tblGridView" style="width: 100%;" cellpadding="0" cellspacing="0" align="center"
                        runat="server" viewstatemode="Enabled" >
                        <tr>
                            <td colspan="2" align="center">
                                <table width="100%" cellpadding="0" cellspacing="3" runat="server" viewstatemode="Enabled"  id="tblHeader">
                                    <tr>
                                        <td align="left">
                                            <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                                                <tr>
                                                    <td style="height: 20px">
                                                        <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True"
                                                            EnableViewState="false">Report Parameters</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px" align="right">
                                            <span class="ClsMdtStar" runat="server" viewstatemode="Enabled" id="lblManFld" visible="false">* Mandatory Fields</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsHilightBGB">
                                            <asp:Label ID="lblDesc" EnableViewState="false" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:UpdatePanel ID="Gridpnl" runat="server">
                                    <ContentTemplate>
                                        <table runat="server" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" viewstatemode="Enabled" CssClass="ClsLabel" ShowMessageBox="true"
                                                        ShowSummary="false" HeaderText="Please fix following error(s):" />
                                                    <asp:GridView CssClass="GridBorder" ID="grdDisplayParameter" runat="server" viewstatemode="Enabled" CellPadding="2"
                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                                                        DataKeyNames="Data_type,View_Name_For_Filter_values,Display_name,Field_name,Display_Filter_Values,OrderBYColumn,Is_Requried,Parent_Field_Id,Is_Dependent,Report_Field_Id,Is_Parent,Filter_Field_Name,Is_Report_Filter_Field,
                                                        Additional_Parent_Field_Id,Additional_Filter_Field_Name,SchemaName"
                                                        Width="99%">
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                                        <RowStyle CssClass="ClsGridRow" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Check All" Visible="false">
                                                                <ItemTemplate>
                                                                    <input id="ChkAll" type="checkbox" runat="server" viewstatemode="Enabled" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="5%"
                                                                    CssClass="ClspaddingL" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Display_name" HeaderText="Parameter" Visible="true">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Filter">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRptParameter" runat="server" ViewStateMode="Enabled" Text="lblRptParameter" Visible="False"
                                                                        CssClass="ClsLabel"></asp:Label>
                                                                    <asp:TextBox ID="txtRptParameter" runat="server" ViewStateMode="Enabled" Visible="False" Text="" MaxLength="50"></asp:TextBox>
                                                                    <cmb:Combobox ID="DDLRptParameter" runat="server" ViewStateMode="Enabled" Visible="false" />
                                                                    <asp:RequiredFieldValidator ID="RFVTxtParamReport" runat="server" ViewStateMode="Enabled" ControlToValidate="txtRptParameter"
                                                                        Display="None" Visible="False" ErrorMessage="Registration number should not be blank."></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="RFVDDLParamReport" runat="server" ViewStateMode="Enabled" ControlToValidate="DDLRptParameter"
                                                                        Display="None" Visible="False" ErrorMessage="should not be blank"></asp:RequiredFieldValidator>
                                                                    <asp:TextBox ID="cRptParameter" CssClass="SmlCombo" runat="server" ViewStateMode="Enabled" AutoPostBack="True"
                                                                        Visible="False"></asp:TextBox>
                                                                    <rjs:PopCalendar ID="CalenderRptParameter" runat="server" ViewStateMode="Enabled" Control="cRptParameter" AutoPostBack="True" Culture="en"
                                                                        ShowErrorMessage="false" InvalidDateMessage="Please select valid date." Format="dd MMM yyyy" OnSelectionChanged="oPopCalendar_SelectionChanged"
                                                                        Visible="False" />
                                                                    <asp:RequiredFieldValidator ID="RFVDatetime" runat="server" ViewStateMode="Enabled" ControlToValidate="cRptParameter"
                                                                        Display="None" Visible="False" ErrorMessage="Date should not be blank."></asp:RequiredFieldValidator>
                                                                    <asp:Label ID="lblDDLMandatory" runat="server" ViewStateMode="Enabled" ForeColor="red" Text="*" Visible="true"></asp:Label>
                                                                    <asp:CheckBoxList ID="ChkRptParameter" runat="server" ViewStateMode="Enabled" RepeatDirection="Horizontal"
                                                                        Visible="False" CssClass="ClsLabel" RepeatColumns="7">
                                                                    </asp:CheckBoxList>
                                                                    <asp:RegularExpressionValidator ID="Reg_Expr_ValidContent" runat="server" ViewStateMode="Enabled" Display="None"
                                                                        ControlToValidate="txtRptParameter" ErrorMessage="Note should be of length less than 300."
                                                                        ValidationExpression="^[\s\S]{0,300}$" CssClass="ClsLabel" Visible="false"> </asp:RegularExpressionValidator>
                                                                    <asp:RegularExpressionValidator ID="Reg_Expr_ForProgressCardRemark" runat="server" ViewStateMode="Enabled" 
                                                                        Display="None" ControlToValidate="txtRptParameter" ErrorMessage="Amount should less than or equals to 10."
                                                                        ValidationExpression="^[\s\S]{0,10}$" CssClass="ClsLabel" Visible="false"> </asp:RegularExpressionValidator>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                <HeaderStyle HorizontalAlign="Left" CssClass="paddingLSML" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Criteria" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hidValues" runat="server" ViewStateMode="Enabled" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hidChkboxlenngth" runat="server" ViewStateMode="Enabled" Value="" />
                                                    <asp:HiddenField ID="hidStandardwise" runat="server" ViewStateMode="Enabled" Value="N" />
                                                    <asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                    <asp:HiddenField ID="hidHasFullAccess" runat="server" ViewStateMode="Enabled" Value="0" />
                                                    <asp:HiddenField ID="hidStandardDivisionId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                    <asp:HiddenField ID="hidUserRolId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                    <asp:HiddenField ID="hidSchemaName" runat="server" ViewStateMode="Enabled" Value="" />
                                                    <asp:HiddenField ID="hidRegNo" runat="server" ViewStateMode="Enabled" Value="" />
                                                    <asp:HiddenField ID="HidPrintDate" runat="server" ViewStateMode="Enabled" Value="" />
                                                    <asp:HiddenField ID="hidIsReportDescription" runat="server" ViewStateMode="Enabled" Value="" />
                                                    <asp:HiddenField ID="hidIsReportGenerated" runat="server" ViewStateMode="Enabled" Value="" />
                                                </td>
                                            </tr>
                                            <tr id="trITReport" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr id="trBookSearch" runat="server" viewstatemode="Enabled" visible="false">
                                                                        <td colspan="2" valign="top">
                                                                            <table width="100%" class="ClsBorderlight">
                                                                                <tr>
                                                                                <td  width="75px"><span class="ClsLabel">Search By : </span></td>                                                                                        
                                                                                    <td align="left" width="60px">
                                                                                        <asp:RadioButton ID="optSearchByBook" runat="server" ViewStateMode="Enabled" AutoPostBack="true"
                                                                                                Checked="true" CssClass="LblNormal" GroupName="Filter" Text="Book" 
                                                                                            oncheckedchanged="optSearchByBook_CheckedChanged" />
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    <asp:RadioButton ID="optSearchByUser" runat="server" ViewStateMode="Enabled" AutoPostBack="true"
                                                                                                CssClass="LblNormal" GroupName="Filter" Text="User" 
                                                                                            oncheckedchanged="optSearchByUser_CheckedChanged" />
                                                                                    </td>
                                                                                    </caption>
                                                                                </tr>
                                                                            </table>                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdName" runat="server" viewstatemode="Enabled" align="left" width="110px">
                                                                            <asp:Label ID="lblName" runat="server" Text="Name / Reg. No. :" CssClass="ClsLabel"
                                                                                EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="300px" runat="server">
                                                                            <asp:TextBox ID="txtName" runat="server" ViewStateMode="Enabled" CssClass="MidCombo" Style="width: 300px;"></asp:TextBox>
                                                                        </td>
                                                                        <td align="left" runat="server">
                                                                            <asp:Button ID="btnSearch" runat="server" ViewStateMode="Enabled" Text="Search" CssClass="ClsBtn" CausesValidation="false"
                                                                                OnClick="btnShow_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <div id="DivStudentDetailsContainer" runat="server" viewstatemode="Enabled" visible="false" class="GridBorder"
                                                                    style="width: 100%; height: 150px; overflow: auto;">
                                                                    <asp:GridView CssClass="GridBorder" ID="grdStudentDetails" runat="server" ViewStateMode="Enabled" CellPadding="2"
                                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                                                                        DataKeyNames="SchoolWise_Standard_Division_Id,Standard_Id,Student_Id,Enrolment_Number,Division_Id" Width="100%"
                                                                        OnRowCommand="grdStudentDetails_RowCommand" 
                                                                        EmptyDataText="No Record Found." ondatabound="grdStudentDetails_DataBound" 
                                                                        onrowdatabound="grdStudentDetails_RowDataBound">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Enrolment_Number" HeaderText="Reg. No." Visible="true">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Standard_Division_Name" HeaderText="Class" Visible="true">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Name" HeaderText="Student Name" Visible="true">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Select" Visible="true">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnSelect" runat="server" ViewStateMode="Enabled" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                        CausesValidation="false" ImageUrl="../images/Selection5.gif" />
                                                                                    <asp:HiddenField ID="hidUserRoleId" runat="server" ViewStateMode="Enabled" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <div id="DivStaffDetailsContainer" runat="server" viewstatemode="Enabled" visible="false" class="GridBorder"
                                                                    style="width: 100%; height: 150px; overflow: auto;">
                                                                    <asp:GridView CssClass="GridBorder" ID="grdStaff" runat="server" ViewStateMode="Enabled" CellPadding="2"
                                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                                                                        DataKeyNames="UserId,StaffGroupId,Teacher_Id,MonthId,Year" Width="100%" OnRowCommand="grdStaff_RowCommand"
                                                                        OnRowDataBound = "grdStaff_RowDataBound"
                                                                        EmptyDataText="No Record Found.">
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" Visible="true">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Designation" HeaderText="Designation" Visible="true">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Select" Visible="true">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnSelect" runat="server" ViewStateMode="Enabled" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                        CausesValidation="false" ImageUrl="../images/Selection5.gif" />
                                                                                        <asp:HiddenField ID="hidUser_RoleId" runat="server" ViewStateMode="Enabled" />
                                                                                        <asp:HiddenField ID="hidUserId" runat="server" ViewStateMode="Enabled" />
                                                                                        <asp:HiddenField ID="hidDesignationId" runat="server" ViewStateMode="Enabled" />
                                                                                        <asp:HiddenField ID="hidDDL1" runat="server"  ViewStateMode="Enabled" Value = "0"/>
                                                                                        <asp:HiddenField ID="hidDDL2" runat="server"  ViewStateMode="Enabled" Value = "0"/>
                                                                                        <asp:HiddenField ID="hidDDL3" runat="server"  ViewStateMode="Enabled" Value = "0" />
                                                                                        <asp:HiddenField ID="hidDDL4" runat="server" ViewStateMode="Enabled" Value = "0"/>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 15px">
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 15px" align="center">
                                <asp:UpdatePanel ID="lblPnl" runat="server" >
                                    <ContentTemplate>
                                        <asp:Label ID="lblNorecord" runat="server" ViewStateMode="Enabled" CssClass="LblNoRecord" Visible="true"
                                            Width="99%"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="grdDisplayParameter" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 18px" align="center">
                                <asp:Label ID="lblErrorMesg" runat="server" ViewStateMode="Enabled" CssClass="LblNoRecord" Visible="False"
                                    Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 18px" align="center">
                                <asp:Label ID="lblSNSErrorMsg" runat="server" ViewStateMode="Enabled" CssClass="LblNoRecord" Visible="False"
                                    Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="Table1" cellpadding="0" cellspacing="2" align="center" runat="server"
                                    width="99%">
                                    <tr id="trFontNote" runat="server" visible="false">
                                        <td align="center">
                                            <span class="ClsLabel" style="font-weight:bold;color:Maroon;float:inherit;">Please install font SHREE-ENG7-0252 to view this report in proper format.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="lblSelectType" runat="server" viewstatemode="Enabled" class="ClsBorderlight" style="width: 27%" visible="false">
                                            <label id="lblSelect" runat="server" viewstatemode="Enabled" class="ClsLabel">
                                                Select Display Type :</label>
                                        </td>
                                        <td align="center" class="ClsBorderlight" style="width: 73%; padding-left: 5px;">
                                            <asp:DropDownList ID="DDLFormatType" runat="server" ViewStateMode="Enabled" Visible="False" CssClass="MidCombo">
                                                <asp:ListItem Selected="True">PDF</asp:ListItem>
                                                <asp:ListItem>MS Word</asp:ListItem>
                                                <asp:ListItem>Excel</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnDisplayReport" runat="server" ViewStateMode="Enabled" Text="Display Report" Visible="False"
                                                CssClass="ClsBtnMid" OnClick="btnDisplayReport_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" viewstatemode="Enabled" id="trNote" visible="false">
                                        <td class="ClsBorderlight" colspan="2" style="width: 73%; padding-left: 5px;">
                                            Problem viewing report? <a class="ClsPhotoGal" target="_blank" href="http://get.adobe.com/reader/">
                                                Click here</a> to download the Acrobat Reader
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <!-- DataView TD End-->
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        _sClientGridId = "<%=this.grdDisplayParameter.ClientID %>"
        _sClienthidChkboxlenngth = "<%=this.hidChkboxlenngth.ClientID %>"
        _sClienlblNorecord = "<%=this.lblNorecord.ClientID %>"

        function VisibleOrHideControls() {
            if (document.getElementById(_sClienlblNorecord) != null) 
            	document.getElementById(_sClienlblNorecord).style.display = "none"
            if ($get("<%=this.lblErrorMesg.ClientID %>") != null)
            	$get("<%=this.lblErrorMesg.ClientID %>").style.display = "none";
        }
        function CheckAllOrUncheckChkBox(irowcnt) {
            var i = irowcnt + 2;
            var chkSelectAll = $("input:checkbox[id*=" + i + "_ChkAll]")[0];
            $("input:checkbox[id*=" + i + "_ChkRptParameter]").attr('checked', chkSelectAll.checked);
        }

        function clickButton(e, buttonid) {
            var evt = e ? e : window.event
            var bt = document.getElementById(buttonid)
            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click()
                    return false
                }
            }
        }
       
        

    </script>

</asp:Content>
