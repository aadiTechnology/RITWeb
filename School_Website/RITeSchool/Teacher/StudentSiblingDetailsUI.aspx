<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/PopupMaster.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="StudentSiblingDetailsUI.aspx.cs"
    Inherits="StudentSiblingDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td align="left" colspan="4" valign="top" style="height: 25px">
                <table border="0" cellpadding="0" cellspacing="0" width="96%">
                    <tr>
                        <td class="ClsGrayMainTitle" width="88%" height="20px">
                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; height: 15px">
                                <tr>
                                    <td align="center">
                                        <asp:label ID = "lblHeader" runat = "server" EnableViewState = "false" class="MainTitleHead" Text = "<%$ Resources:LocalizedResources, StudentSiblingDetails%>" ></asp:label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <asp:UpdatePanel ID="UPnlStudent" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Data Insert Here -->
                        <table border="0" align="center" cellpadding="0" cellspacing="2" style="width: 95%;">
                            <tr>
                                <td align="left" valign="top" height="10%">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" valign="top">
                                                <table id="Table3" runat="server" align="center" width="65%">
                                                    <tr align="center" style="width: 80%">
                                                        <td align="center" style="width: 600px">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Visible="False"
                                                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True" Width="493px"></asp:Label>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                                    <asp:AsyncPostBackTrigger ControlID="lstvwSiblingDetails" EventName="ItemCommand" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table id="LegendTable" runat="server">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                    Text= "<%$ Resources:LocalizedResources, Legend%>" EnableViewState="false"></asp:Label>
                                            </td>
                                            <td align="left" style="padding-right: 3px">
                                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                    BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text= "<%$ Resources:LocalizedResources, DeactivatedUser%>"
                                                    EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table id="Table2" runat="server" align="center" width="65%">
                                        <tr id="trStudentName" runat="server" style="width: 60%">
                                            <td align="center" style="width: 94px" class="ClsBorderlight">
                                                <asp:Label  ID="lblStudent" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td class="ClsHilightBGB" style="width: 200px">
                                                <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                                            </td>
                                            <td width="150px" align="right">
                                            </td>
                                        </tr>
                                        <tr align="center" style="width: 60%">
                                            <td align="center" style="width: 94px">
                                                <asp:Label CssClass = "ClsLblLgnd" ID="lblSiblingDetails" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SiblingDetails%>"></asp:Label>
                                                <span class="ClsLblLgnd colonPadding">:</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                 <table id="Table6" runat="server" align="center" width="65%">
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 600px">
                                                &nbsp;</td>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblSiblingDetails" runat="server" align="center" width="65%">
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 600px">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:ListView ID="lstvwSiblingDetails" runat="server" DataKeyNames="YearwiseStudentId,IsLeftStudent,StudentSiblingId"
                                                                    OnItemCommand="lstvwSiblingDetails_ItemCommand" OnItemDataBound="lstvwSiblingDetails_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" style="padding-left: 9px;">  
                                                                                    <asp:Label ID = "lblRegNo" runat = "server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                                </th> 
                                                                                <th align="left" style="padding-left: 9px;">
                                                                                    <asp:Label ID = "Label2" runat ="server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                                </th>
                                                                                <th align="center">
                                                                                   <asp:Label ID = "lblClass" runat ="server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                </th>
                                                                                <th align="center">
                                                                                  <asp:Label ID = "lblDelete" runat ="server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
                                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" class="paddingL">
                                                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
                                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                </asp:ListView>
                                                                <tr>
                                                                    <td align="center">
                                                                        <span>
                                                                            <asp:Label ID="lblNorecord" runat="server" CssClass="LblNoRecord" Visible="False"
                                                                                Width="550px"></asp:Label></span>
                                                                    </td>
                                                                </tr>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.StudentSiblingDetailsBL" EnablePaging="true"
                                                                    ID="lstvwSiblingDetailsDSobj" runat="server" SelectMethod="GetStudentSiblingList"
                                                                    SortParameterName="sortExpression" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="string" />
                                                                        <asp:ControlParameter ControlID="hidYearWiseStudentId" PropertyName="Value" Name="aiYearwiseStudentId" />
                                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table id="Table4" runat="server" align="center" width="65%">
                                        <tr align="center" style="width: 60%">
                                            <td align="center" style="width: 135px" colspan="3">
                                                &nbsp; 
                                                <asp:Label CssClass = "ClsLblLgnd" ID="lblSearchForSibling" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SearchForSibling%>"></asp:Label>
                                                <span class="ClsLblLgnd colonPadding">:</span>
                                            </td>
                                        </tr>
                                        <tr align="center" style="width: 60%">
                                            <td style="width: 150px" class="ClsBorderlight">
                                                <asp:Label CssClass = "ClsLabel" ID="lblNameRegNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NameOrReg%>"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td style="width: 222px">
                                                <asp:TextBox ID="txtStudentName" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="ClsBtn"
                                                    OnClick="btnSearch_Click" Text= "<%$ Resources:LocalizedResources, Search%>" UseSubmitBehavior="false" ValidationGroup="valGrpSiblingDetails" />
                                            </td>
                                            <td width="280px" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table1" runat="server" align="center" width="65%">
                                        <tr id="Tr5" runat="server">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentList">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, To%>" EnableViewState="false" />
                                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, OutOf%>"   EnableViewState="false" />
                                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                    CssClass="LblNrmlB" />
                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, Records%>" EnableViewState="false" />
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                        <tr align="center" style="width: 80%">
                                            <td align="center" style="width: 600px">
                                                &nbsp;
                                                <asp:ListView ID="lstvwStudentList" runat="server" OnDataBound="lstvwStudentList_DataBound"
                                                    DataKeyNames="YearwiseStudentId" OnItemDataBound="lstvwStudentList_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" width="10%">
                                                                    <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                                                </th>
                                                                <th align="left" style="padding-left: 9px;">
                                                                  <asp:Label ID = "lblRegNo" runat = "server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                </th>
                                                                <th align="left" style="padding-left: 9px;">
                                                                  <asp:Label ID = "lblStudnt" runat ="server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                </th>
                                                                <th align="center">
                                                                 <asp:Label ID = "lblClass" runat ="server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="5" align="left">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentList"
                                                                        PageSize="20">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="MessageLabel" Text= "<%$ Resources:LocalizedResources, SelectAPage%>" runat="server" CssClass="LblNrmlB" />
                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td align="right" class="LblNormal">
                                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="center">
                                                                <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                <asp:HiddenField ID="hidStudentId" runat="server" Value='<%# Eval("YearwiseStudentId") %>' />
                                                                <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" Value='<%# Eval("SchoolwiseStudentId") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="center">
                                                                <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                <asp:HiddenField ID="hidStudentId" runat="server" Value='<%# Eval("YearwiseStudentId") %>' />
                                                                <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" Value='<%# Eval("SchoolwiseStudentId") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblRollNum" runat="server" Text='<%# Eval("RegNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td class="LblNoRecord" align="center"> 
                                                               <asp:Label ID = "lblMsg" runat = "server" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, NoRecordFound%>" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.StudentSiblingDetailsBL" EnablePaging="true"
                                                    ID="lstvwStudentDSobj" runat="server" SelectMethod="GetStudentList" SortParameterName="sortExpression"
                                                    SelectCountMethod="CountTotalStudents" EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="string" />
                                                        <asp:ControlParameter ControlID="hidYearWiseStudentId" PropertyName="Value" Name="aiYearwiseStudentId" />
                                                        <asp:ControlParameter ControlID="txtStudentName" PropertyName="Text" Name="asFilter"
                                                            DefaultValue="NULL" />
                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSave" runat="server" Text= "<%$ Resources:LocalizedResources, Save%>" CssClass="ClsBtn" BorderStyle="Solid"
                                                            OnClick="btnSave_Click" UseSubmitBehavior="false" />
                                                        <asp:Button ID="btnClose" Text= "<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtn" BorderStyle="Solid" runat="server" OnClientClick="CloseWindow()" value="Close Window"
                                                            BorderWidth="1px" UseSubmitBehavior="false" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidYearWiseStudentId" runat="server" />
                                    <asp:HiddenField ID="hidMode" runat="server" />
                                    <asp:HiddenField ID="hidSiblingStudentId" runat="server" />
                                    <asp:HiddenField ID="hidStudentId" runat="server" />
                                    <asp:HiddenField ID="hidStandardId" runat="server" />
                                    <asp:HiddenField ID="hidClassName" runat="server" />
                                    <asp:HiddenField ID="hidDivisionId" runat="server" />
                                    <asp:HiddenField ID="hidPageNo" runat="server" />
                                    <asp:HiddenField ID="HidBackUrl" runat="server" />
                                    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" />
                                    <asp:HiddenField ID="hidSiblingRowCount" runat="server" />
                                    <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                    <asp:HiddenField ID="hidDummyName" runat="server" />

                                    <asp:HiddenField ID = "hidAlertSelectedStudentFromPageGetLost" runat = "server" />
                                    <asp:HiddenField ID = "hidAlertDeleterecord" runat = "server" />
                                    <asp:HiddenField ID = "hidAtLeastOneStudentSelectedForSibling" runat = "server" />
                                    <asp:HiddenField ID = "hidAlertMultipleSiblingSelected" runat = "server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwSiblingDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        _clientlstvwStudntList = "<%=this.lstvwStudentList.ClientID %>"
        _clientlstvwSiblingDetails = "<%=this.lstvwSiblingDetails.ClientID %>"
        _ClientChkAll = _clientlstvwStudntList + "_ChkSelectAll";
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";
        _clienttxtStudentName = "<%=this.txtStudentName.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clienthidYearWiseStudentId = "<%=this.hidYearWiseStudentId.ClientID %>"
        _clienthidSiblingRowCount = "<%=this.hidSiblingRowCount.ClientID %>"
        _clienthidSiblingStudentId = "<%=this.hidSiblingStudentId.ClientID %>"


        function MessageAboutUpload(oCmb) {
            var bIsValid;
            if (window.confirm(document.getElementById("<%=this.hidAlertSelectedStudentFromPageGetLost.ClientID %>").value))
                bIsValid = true;
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false;
            }
            return bIsValid;
        }
        function UpdateHidVariablesIfSiblingAdded(iCnt) {
            window.opener.UpdateHidVariablesIfSiblingAdded(iCnt);
        }
        function SubmitSiblingDetails(iCount) {
            
            var bResult = true
            var chk
            var iRowCount = 0
            var StudentIds = ""
            var SchoolwiseStudentId = ""
            var SiblingStudentName = ""
            chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    StudentIds += document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_hidStudentId").value + ",";
                    if (SiblingStudentName != "")
                        SiblingStudentName += "," + document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_lblStudentName").innerHTML;
                    else
                        SiblingStudentName = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_lblStudentName").innerHTML;
                    if (SchoolwiseStudentId == "")
                        SchoolwiseStudentId = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_hidSchoolwiseStudentId").value
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (document.getElementById(_clienthidSiblingStudentId).value != "" && (!StudentIds.match(document.getElementById(_clienthidSiblingStudentId).value))) {
                StudentIds += document.getElementById(_clienthidSiblingStudentId).value;
            }
            
            window.opener.GetSiblingStudentIds(StudentIds, SchoolwiseStudentId, SiblingStudentName, iCount);
        }


        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertDeleterecord.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }

        function ConfirmForStudent() {
            var bResult = true
            var iCount = 0
            var chk
            var chkSibling
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")

            while (chk != null) {
                if (chk.checked == true)
                    iCount = iCount + 1
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (iCount < 1) {
                if (document.getElementById(_clientlblUpdateSucess) != null) {
                    document.getElementById(_clientlblUpdateSucess).style.display = "none"
                }
                if (!window.alert(document.getElementById("<%=this.hidAtLeastOneStudentSelectedForSibling.ClientID %>").value))
                    bResult = false
            }
            else if (window.opener.OverwriteSiblingDetails() == "Y" && iCount > 1)
                if (!window.confirm(document.getElementById("<%=this.hidAlertMultipleSiblingSelected.ClientID %>").value))
                    bResult = true;
            //            else             
            //                bResult = true;
            if (document.getElementById(_clienthidYearWiseStudentId).value == "0" && bResult)
                SubmitSiblingDetails(iCount);
            else
                UpdateHidVariablesIfSiblingAdded(iCount)
            return bResult
        }

        function CloseWindow1(SiblingStudentName) {            
            SiblingStudentName = SiblingStudentName.replace("%*","'")
            var bResult = true
            var chk
            var iRowCount = 0
            var StudentIds = ""
            var SchoolwiseStudentId = ""
            chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    if (SiblingStudentName != "")
                        SiblingStudentName += "," + document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_lblStudentName").innerHTML;
                    else
                        SiblingStudentName = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_lblStudentName").innerHTML;
                    if (SchoolwiseStudentId == "")
                        SchoolwiseStudentId = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_hidSchoolwiseStudentId").value
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStudntList + "_ctrl" + iRowCount + "_ChkSelect")
            }
            alert('Selected sibling(s) - '+SiblingStudentName)
            window.opener.UpdateSibliStudentName(SiblingStudentName);
            window.close();
            window.opener.focus();
        }

        function CloseWindow() {
            var iSiblingRowCnt = 0
            var SiblingStudentName = ""
            var iCnt = 0
            iSiblingRowCnt = document.getElementById(_clienthidSiblingRowCount).value
            while (iCnt < iSiblingRowCnt) {
                chkSibling = document.getElementById(_clientlstvwSiblingDetails + "_ctrl" + iCnt + "_lblStudentName")
                if (SiblingStudentName != "")
                    SiblingStudentName += "," + chkSibling.innerHTML;
                else
                    SiblingStudentName = chkSibling.innerHTML;
                iCnt = iCnt + 1;
            }
            
            var isDeleted;
            if (document.getElementById(_clienthidSiblingStudentId).value == "") {
                window.opener.GetSiblingStudentIds('', '', '', '');
                isDeleted = 'Y';
            }
            window.opener.UpdateSibliStudentName(SiblingStudentName, isDeleted);
            window.close();
            window.opener.focus();
        }
       

    </script>
</asp:Content>
