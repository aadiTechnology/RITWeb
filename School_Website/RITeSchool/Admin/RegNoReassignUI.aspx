<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="RegNoReassignUI.aspx.cs" Inherits="ReassignRegNoUI"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="97%">
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                    <ContentTemplate>
                        <table width="100%">
                            <tr runat="Server" id="trStandard">
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelCondition" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <tr align="left">
                                                <td style="height: 20px" class="ClsGrayMainTitle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="left" class="MainTitleHead" style="height: 20px">
                                                                <asp:Label ID="lblHeading" runat="server" BorderWidth="0px" Text= "<%$ Resources:LocalizedResources, StudentRegistrationNumber%>"
                                                                    Font-Bold="True" EnableViewState="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table align="right">
                                                        <tr>
                                                            <td align="right" valign="top">
                                                                <span class="ClsMdtStar">*</span>
                                                                 <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                                        <asp:UpdatePanel ID="upnlErrorMessage" runat="server">
                                                            <ContentTemplate>
                                                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="Filter"
                                                                    HeaderText= "<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 15px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table class="ClsBorderlight" width="100%">
                                                        <tr>
                                                            <td class="ClsHilightText " style="width: 68%" align="left">
                                                                <%--<asp:Label ID="Label2" runat="server" CssClass="clsLabel" Text="Blank Registration Number Count"
                                                                    Font-Bold="True" EnableViewState="False"></asp:Label>--%>
                                                                  <asp:Label CssClass = "clsLabel" ID="lblBlankRegNo" Font-Bold = "true" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, BlankRegistrationNumberCount%>"></asp:Label>
                                                            </td>
                                                            <td class="ClsHilightText " align="right" style="width: 9%">
                                                                <%--<asp:Label ID="Label4" runat="server" CssClass="clsLabel" Text="In School :" 
                                                                    EnableViewState="False"></asp:Label>--%>
                                                                    <asp:Label CssClass = "clsLabel" ID="lblInSchool" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, InSchool%>"></asp:Label>
                                                                    <span class="colonPadding">:</span>
                                                            </td>
                                                            <td class="ClsHilightBGB" align="left" style="width: 4%; text-align: center">
                                                                <asp:Label ID="lblBlankRegCount" runat="server" CssClass="ClsTextNormal"></asp:Label>
                                                            </td>
                                                            <td id="tdTitleRegCountFilter" class="ClsHilightText " align="right" style="width: 15%"
                                                                runat="server">
                                                                <%--<asp:Label ID="lblTitleRegCountFilter" runat="server" CssClass="clsLabel" 
                                                                    Text="Per Selected Filter :" EnableViewState="False"></asp:Label>--%>
                                                                  <asp:Label CssClass = "clsLabel" ID="lblSelectedFilter" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectedFilter%>"></asp:Label>
                                                                    <span class="colonPadding">:</span>
                                                            </td>
                                                            <td id="tdBlankRegCountFilter" class="ClsHilightBGB" align="left" style="width: 4%;
                                                                text-align: center" runat="server">
                                                                <asp:Label ID="lblBlankRegCountFilter" runat="server" CssClass="ClsTextNormal"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <%--<asp:Label ID="lblFilter" runat="server" CssClass="ClsLblLgnd" EnableViewState="False"
                                                        Text="Select Filter :" Height="20px"></asp:Label>--%>
                                                   <asp:Label CssClass = "ClsLblLgnd" ID="lblSelectFilter" Height = "20px" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectFilter%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table class="ClsBorderlight" width="100%">
                                                        <tr runat="Server" id="tr1">
                                                            <td colspan="4">
                                                                <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td class="ClsBorderlight" style="padding-left: 5px">
                                                                                    <%--<asp:Label ID="lblStandard" CssClass="clsLabel" Text="Select Standard : " runat="server"
                                                                                        EnableViewState="False"></asp:Label>--%>
                                                                                        <asp:Label CssClass = "clsLabel" ID="lblSelectStandard" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectStandard%>"></asp:Label>
                                                                    <span class="colonPadding">:</span>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:DropDownList ID="ddlStandard" AutoPostBack="true" runat="server" CssClass="SmlTxtBox"
                                                                                        OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged" AppendDataBoundItems="True"
                                                                                        TabIndex="1">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td class="ClsBorderlight" style="padding-left: 5px">
                                                                                    <%--<asp:Label ID="lblDivision" CssClass="clsLabel" Text="Select Division : " runat="server"
                                                                                        EnableViewState="False"></asp:Label>--%>
                                                                                    <asp:Label CssClass = "clsLabel" ID="lblSelectDivision" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectDivision%>"></asp:Label>
                                                                    <span class="colonPadding">:</span>
                                                                                </td>
                                                                                <td align="left" colspan="2">
                                                                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="SmlTxtBox" TabIndex="2"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr runat="Server">
                                                            <td class="HilightBGGray" align="center" colspan="4">
                                                                <%--<asp:Label CssClass="ClsHilightText" ID="lblStudRegOrName" Font-Bold="False" runat="server"
                                                                    EnableViewState="False">--%>
                                                                   <asp:Label CssClass = "ClsHilightText" ID="lblSelectStanardDivision" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectStanardDivision%>"></asp:Label>
                                                                        <img src="../images/ArrowBlueDblRev.gif" />
                                                                       <asp:Label CssClass = "ClsHilightTextB" ID="lblAnd" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, And%>"></asp:Label>
                                                                        <img src="../images/ArrowBlueDblNw.gif" />
                                                                       <asp:Label CssClass = "ClsHilightTextB" ID="lblStudentRegNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectstudentNameRegNo%>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr id="Tr3" runat="Server">
                                                                                <td align="center">
                                                                                    <table cellpadding="0" cellspacing="2">
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorderlight" width="25px">
                                                                                                <asp:RadioButton ID="optMain" runat="server" GroupName="Search" AutoPostBack="true"
                                                                                                    TabIndex="1" OnCheckedChanged="optMain_CheckedChanged" />
                                                                                            </td>
                                                                                            <td class="ClsBorderlight" align="left" style="padding-left: 5px" width="150px">
                                                                                                <%--<asp:Label ID="lblRegNumber" CssClass="clsLabel" Text="Student  Name / Reg.No. :"
                                                                                                    runat="server" EnableViewState="False"></asp:Label>--%>
                                                                                                <asp:Label CssClass = "clsLabel" ID="lblStudentNameRegNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentNameRegNo%>"></asp:Label>
                                                                    <span class="colonPadding">:</span>
                                                                                            </td>
                                                                                            <td align="Center" class="ClsBorderlight" width="80px">
                                                                                                <asp:Label ID="lblLike" runat="server" Style="font-weight: bold" Text= "<%$ Resources:LocalizedResources, Like%>"></asp:Label>
                                                                                            </td>
                                                                                            <td width="150px">
                                                                                                <% //The AutoPostBack event is explicitly set to false to avoid duplicate postback %>
                                                                                                <asp:TextBox ID="txtRegNumber" TabIndex="2" runat="server" MaxLength="20" CssClass="MidTxtBox"
                                                                                                    AutoPostBack="false" autocomplete="off"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="4">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr id="Tr2" runat="Server">
                                                                                <td align="center">
                                                                                    <table cellpadding="0" cellspacing="2">
                                                                                        <tr>
                                                                                            <td align="left" class="ClsBorderlight" width="25px">
                                                                                                <asp:RadioButton ID="optExact" runat="server" GroupName="Search" AutoPostBack="true"
                                                                                                    TabIndex="1" OnCheckedChanged="optExact_CheckedChanged" />
                                                                                            </td>
                                                                                            <td align="left" class="ClsBorderlight" colspan="1" style="padding-left: 5px" width="150px">
                                                                                                <asp:Label CssClass = "clsLabel" ID="lblRegNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                                                    <span class="colonPadding">:</span>
                                                                                                <asp:UpdatePanel ID="upnlOperation" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="cmbOperation" Style="width: 55px" runat="server" CssClass="SmlCombo"
                                                                                                            TabIndex="2" Height="19px">
                                                                                                        </asp:DropDownList>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td align="left" width="80px">
                                                                                                <asp:UpdatePanel ID="upnlPrefix" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:DropDownList ID="cmbPrefix" runat="server" TabIndex="3" CssClass="SmlCombo"
                                                                                                            Style="width: 80px">
                                                                                                        </asp:DropDownList>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td width="150px">
                                                                                                <asp:TextBox ID="txtReg" runat="server" CssClass="MidTxtBox" AutoPostBack="false"
                                                                                                    onblur="extractNumber(this,0,false);" CausesValidation="true" TabIndex="4" onkeyup="extractNumber(this,0,false);"
                                                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                                                                    ondrop="event.returnValue=false;"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="center">
                                                                <asp:CheckBox ID="chkIsStudBlankRegNo" runat="server" Text= "<%$ Resources:LocalizedResources, OnlyBlankRegNo%>"
                                                                    TabIndex="12" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btnShow" runat="server" CssClass="ClsBtnMid" CausesValidation="True"
                                        Text= "<%$ Resources:LocalizedResources, Show%>" Height="24px" TabIndex="13" OnClick="btnShow_Click" ValidationGroup="Filter">
                                    </asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CustomValidator ID="cstvalRegNo" runat="server" ClientValidationFunction="ValidateRegNo"
                                        ValidationGroup="Filter" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, RegNumberBlank%>"
                                        SetFocusOnError="True">
                                    </asp:CustomValidator>
                                </td>
                            </tr>
                            <tr id="trTopButtons" runat="server">
                                <td align="center" valign="bottom">
                                    <asp:Button ID="btnTopUpdate" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                        Text= "<%$ Resources:LocalizedResources, Update %>" TabIndex="18" OnClick="btnUpdate_Click" />
                                    <asp:Button ID="btnTopClose" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                        Text= "<%$ Resources:LocalizedResources, Close%>" TabIndex="18" OnClick="btnBack_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="UpdatePnlRegNoReAssign" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <table id="Table1" runat="server" width="100%">
                                                <tr>
                                                    <td id="tdMessage" runat="server" align="center">
                                                        <asp:Label ID="lblError" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <asp:GridView CssClass="GridBorder" ID="grdvwRegNo" runat="server" AutoGenerateColumns="False"
                                                            EmptyDataText= "<%$ Resources:LocalizedResources, NoRecordFound%>" 
                                                            Width="100%" CellPadding="0" CellSpacing="1"
                                                            ForeColor="#333333" GridLines="None" DataKeyNames="Student_Id,Enrolment_Number,Reg_No_Prefix,Roll_No,SchoolLeft_Date"
                                                            DataSourceID="GrdDSobj" OnRowDataBound="grdvwRegNo_RowDataBound" 
                                                            ondatabound="grdvwRegNo_DataBound">
                                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                            </PagerStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="Roll_No" HeaderText= "<%$ Resources:LocalizedResources, RollNo%>" >
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Name" HeaderText= "<%$ Resources:LocalizedResources, StudentName%>">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Class" HeaderText= "<%$ Resources:LocalizedResources, Class%>">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                        Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Enrolment_Number" HeaderText= "<%$ Resources:LocalizedResources, OldRegNo%>" HtmlEncode="false">
                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                   <asp:Label ID ="lblNewRegNo" runat ="server" Text = "<%$ Resources:LocalizedResources, NewRegNo%>" ></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNewRegNo" runat="server" type="TextBox" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"Enrolment_Number")))%>'
                                                                            MaxLength="15" CssClass="MidTxtBox" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle CssClass="ClsGridRow" />
                                                            <HeaderStyle CssClass="ClsGridHeader" />
                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="false" ID="GrdDSobj"
                                                            runat="server" SelectMethod="GetStudentsWithEnrolmentNumber" SelectCountMethod="GetCountStudents">
                                                            <SelectParameters>
                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                    Type="Int32" />
                                                                <asp:ControlParameter ControlID="txtRegNumber" Type="String" PropertyName="Text"
                                                                    Name="asEnrolmentNumber" />
                                                                <asp:ControlParameter ControlID="ddlStandard" Type="Int32" PropertyName="SelectedValue"
                                                                    Name="aiStandardId" DefaultValue="0" />
                                                                <asp:ControlParameter ControlID="ddlDivision" Type="Int32" PropertyName="SelectedValue"
                                                                    Name="aiDivisionId" DefaultValue="0" />
                                                                <asp:ControlParameter ControlID="chkIsStudBlankRegNo" Type="Boolean" PropertyName="Checked"
                                                                    Name="abIsStudBlankRegNo" />
                                                                <asp:ControlParameter ControlID="optExact" Type="Boolean" PropertyName="Checked"
                                                                    Name="abIsExact" />
                                                                <asp:ControlParameter ControlID="cmbOperation" PropertyName="SelectedValue" Name="asOperator" />
                                                                <asp:ControlParameter ControlID="cmbPrefix" PropertyName="SelectedValue" Name="asPrefix" />
                                                                <asp:ControlParameter ControlID="txtReg" PropertyName="Text" Name="asRegNo" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="grdvwRegNo" EventName="RowDataBound" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="UPanelHidFields" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidOperator" runat="server" />
                                            <asp:HiddenField ID="hidPrefix" runat="server" />
                                            <asp:HiddenField ID="hidPostfix" runat="server" />
                                            <asp:HiddenField ID = "hidValDuplicateRegNoForRollNo"  runat = "server" />
                                            <asp:HiddenField ID  ="hidValRegNoZeroForRollNo" runat ="server" />
                                            <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                              <asp:HiddenField ID = "HidEmptyRegNo" runat = "server" />

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="bottom">
                                    <asp:Button ID="btnUpdate" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                        Text= "<%$ Resources:LocalizedResources, Update %>" TabIndex="18" OnClick="btnUpdate_Click" />
                                    <asp:Button ID="btnBack" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                        Text="Back" TabIndex="18" OnClick="btnBack_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
   
        _clienttxtRegNumber = "<%=this.txtRegNumber.ClientID %>";
        _clientddlDivision = "<%=this.ddlDivision.ClientID%>";
        _clientddlStandard = "<%=this.ddlStandard.ClientID%>";
        _clientChkBlankRegNo = "<%=this.chkIsStudBlankRegNo.ClientID%>";
        _clientgrdvwRegNo = "<%=this.grdvwRegNo.ClientID%>";
        _clientlblError = "<%=this.lblError.ClientID%>";
        _clientoptExact = "<%=this.optExact.ClientID %>";
        _clientbtnShow = "<%=this.btnShow.ClientID %>";
        _clienttxtReg = "<%=this.txtReg.ClientID %>";

        function ValidateRegNosInGrid(TxtNewRegNo) {
            var grdvwRegNo = document.getElementById(_clientgrdvwRegNo);
            var olblError = document.getElementById(_clientlblError);
            if (olblError != null)
                olblError.innerHTML = '';
            if (grdvwRegNo != null) {
                var sMsg = "";
                var sZeroMsg = "";
                var grdNewRegNo = "";
                var iRowNum = "_ctl";
                var oTxtNewRegNo = TxtNewRegNo.toString();
                var iCnt = grdvwRegNo.rows.length;
                var positions = new Array(iCnt - 1);
                for (ix = 0; ix < iCnt - 1; ix++) {
                    positions[ix] = 0;
                }
                for (i = 1; i < iCnt; i++) {
                    var grdRollNo = grdvwRegNo.rows[i].cells[0].innerHTML;
                    var grdOldRegNo = grdvwRegNo.rows[i].cells[3].innerHTML;
                    iRow = i + 1;
                    if (iRow < 10)
                        grdNewRegNo = _clientgrdvwRegNo + iRowNum + "0" + iRow + "_" + oTxtNewRegNo;
                    else
                        grdNewRegNo = _clientgrdvwRegNo + iRowNum + +iRow + "_" + oTxtNewRegNo;
                    document.getElementById(grdNewRegNo).style.backgroundColor = "#fff";
                    grdNewRegNo = document.getElementById(grdNewRegNo).value;
                    positions[i - 1] = trimAll(RemoveLeadingZeroes(grdNewRegNo));
                }
                for (i = 1; i < iCnt; i++) {
                    if ((positions[i - 1]) != '') {
                        for (iInnerCount = 1; iInnerCount < iCnt; iInnerCount++) {
                            if ((i != iInnerCount && (positions[i - 1]) == (positions[iInnerCount - 1])) || (positions[i - 1]) == '0') {
                                var grdRollNo = grdvwRegNo.rows[i].cells[0].innerHTML;
                                iRow = i + 1;
                                if (iRow < 10)
                                    grdNewRegNo = _clientgrdvwRegNo + iRowNum + "0" + iRow + "_" + oTxtNewRegNo;
                                else
                                    grdNewRegNo = _clientgrdvwRegNo + iRowNum + +iRow + "_" + oTxtNewRegNo;
                                document.getElementById(grdNewRegNo).style.backgroundColor = "#ffddb8";

                                if ((positions[i - 1]) == '0') {
                                    sZeroMsg = sZeroMsg + ', ' + grdRollNo;
                                }
                                else {
                                    sMsg = sMsg + ', ' + grdRollNo;
                                }
                                break;
                            }
                        }
                    }
                }
                var sMessage = "";
                if (sMsg.length > 0) {
                    sMsg = sMsg.substring(1, sMsg.length);
                    sMessage = document.getElementById("<%=this.hidValDuplicateRegNoForRollNo.ClientID %>").value + sMsg + "\n\r";
                }
                if (sZeroMsg.length > 0) {
                    sZeroMsg = sZeroMsg.substring(1, sZeroMsg.length);
                    sMessage = sMessage + document.getElementById("<%=this.hidValRegNoZeroForRollNo.ClientID %>").value + sZeroMsg + "\n\r";
                }
                if (sMessage != '') {
                    alert(sMessage);
                    return false;
                }
                return true;
            }
        }

        function ValidateRegNo(aSrc, args) {
            var Buttontext = $get(_clientbtnShow).value;
          
//            var value = document.getElementById(_HidEmptyRegNo).val()
//            if (value = false)
//          {

            if (Buttontext != "Change Input" && !$get(_clientChkBlankRegNo).checked) {
                if ($get(_clientoptExact).checked) {
                    if ($get(_clienttxtReg).value == "") {
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                        return false;
                    }
                    return false;
                }
            }
////            else {
//              args.IsValid = true;
//                        return false;
////            }
        }

    </script>
    
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });
        function AutoSearch() {
            var SchoolId = "<%=miSchoolId %>";
            _clienttxtRegNumber = '#<%=txtRegNumber.ClientID%>';
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, _clientddlStandard, _clientddlDivision, null, 1);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtRegNumber.ClientID %>");
            bt = document.getElementById("<%=this.btnShow.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
