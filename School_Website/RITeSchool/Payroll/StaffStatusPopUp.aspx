<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    CodeFile="StaffStatusPopUp.aspx.cs" Inherits="StaffStatusPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 19px" align="left" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr id ="trPopupIdentity" runat ="server" >
                                            <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
                                                <span class="MainTitleHead" style="font-weight: bold">Service Type</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="center" id="trErrorMessage" runat="server">
                                <td align="left" style="width: 100%; height: 50px;">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" EnableViewState="False"
                                        ForeColor="Red"></asp:Label>
                                    <table align="center">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Font-Bold="True" ForeColor="Blue" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table id="LegendTable" runat="server">
                                        <tr>
                                            <td align="left" colspan="1">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                    Text="Legend :" EnableViewState="False" Width="60px"></asp:Label>
                                            </td>
                                            <td align="center" valign="middle" style="border: 1px solid #000000;">
                                                <asp:Label ID="Label7" runat="server" BackColor="White" CssClass="ClsLblLgnd" Text="Deleted User"
                                                    ForeColor="Red" Font-Bold="False" BorderStyle="None" BorderWidth="1px" ReadOnly="True"
                                                    Width="100px" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left" style="padding-right: 3px">
                                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                    BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Font-Bold="True" Text=" Deactivated User from Payroll"
                                                    EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table align="center">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 100px;">
                                                <span id="Span2" class="ClsLabel">User Role : </span>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="cmbUserRoles" runat="server" Width="205px" CssClass="MidCombo">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="width: 100px;">
                                                <span id="Span3" class="ClsLabel">Service Type : </span>
                                            </td>
                                            <td valign="top" style="width: 270px;" class="ClsBorderlight">
                                                <asp:CheckBoxList ID="chkStaffStatus" AppendDataBoundItems="true" CssClass="ClsLabel"
                                                    runat="server" RepeatColumns="3" RepeatDirection="Vertical">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <span id="Span1" class="ClsLabel">User Name : </span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtUserName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                            </td>
                                           <td align="left" class="ClsBorderlight">
                                                <span id="Span4" class="ClsLabel">Include Locked Users? : </span>
                                            </td>
                                            <td align="left">                                               
                                                    <asp:CheckBox ID="chkIncludeLockedUSer" runat="server" ViewStateMode="Enabled" AutoPostBack="false" TabIndex="7" />
                                            </td>                                         
                                        </tr>
                                        <tr>
											<td valign="top" colspan="4" align="center">
													<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" BorderWidth="1px"
                                                CausesValidation="false" OnClick="btnSearch_Click" />
											</td>
										</tr>
                                        <tr>
                                            <td colspan="4" style="height: 10px">
                                            </td>
                                        </tr>
                                        <tr id="trlistview" runat="server" align="center">
                                            <td align="center" colspan="4">
                                                <table width="100%">
                                                    <tr id="trPagerStaffStatus" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStaffStatusType">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListView ID="lstvwStaffStatusType" runat="server" EnableViewState="true" OnDataBound="lstvwStaffStatusType_DataBound"
                                                                DataKeyNames="UserId,DesignationId,StaffStatusDetailsId,IsDeleted,IsLocked" OnItemDataBound="lstvwStaffStatusType_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th align="left" style="width: 250px" class="paddingLSML">                                                                                
                                                                                User Name
                                                                            </th>
                                                                            <th align="left" class="paddingLSML">                                                                                
                                                                                Designation
                                                                            </th>
                                                                            <th align="left" class="paddingLSML">
                                                                                Service Type
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="trHeaderContol" runat="server" class="ClsGridHeader">
                                                                            <th align="left">
                                                                            </th>
                                                                            <th align="left">
                                                                            </th>
                                                                            <th align="left" class="paddingLSML">
                                                                                <asp:DropDownList ID="cmbAllStatusType" runat="server" Width="140px" CssClass="MidCombo"
                                                                                    onchange="ChangeAllStaffStatus()">
                                                                                </asp:DropDownList>
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceHolder" runat="server">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" id="trDataPager">
                                                                            <td colspan="5">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStaffStatusType"
                                                                                    PageSize="20">
                                                                                    <Fields>
                                                                                        <asp:TemplatePagerField>
                                                                                            <PagerTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left">
                                                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                                                        <td align="left" class="paddingLSML">
                                                                            <asp:Label ID="lblUserName" runat="server" Width="180px" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                        </td>
                                                                        <td class="paddingLSML" align="left">
                                                                            <asp:Label ID="lblDesignation" runat="server" Width="180px" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="paddingLSML">
                                                                            <asp:DropDownList ID="cmbStaffStatusType" Width="140px" runat="server" CssClass="MidCombo">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hidIsDeleted" runat="server" />
                                                                            <asp:HiddenField ID="hidIsLocked" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" class="paddingLSML">
                                                                            <asp:Label ID="lblUserName" runat="server" Width="180px" Text='<%#Eval("UserName") %>'></asp:Label>
                                                                        </td>
                                                                        <td class="paddingLSML" align="left">
                                                                            <asp:Label ID="lblDesignation" runat="server" Width="180px" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="paddingLSML">
                                                                            <asp:DropDownList ID="cmbStaffStatusType" Width="140px" runat="server" CssClass="MidCombo">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hidIsDeleted" runat="server" />
                                                                            <asp:HiddenField ID="hidIsLocked" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" id="trNorecordFoundSearch" runat="server">
                                            <td style="height: 10px;" align="center" colspan="4">
                                                <asp:Label ID="lblNoRcrdFnd" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                    Text="No Record Found." EnableViewState="False" Width=" 100%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="false" OnClick="btnSave_Click" disable-page="true" />
                                                &nbsp;
                                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="false" OnClientClick="window.close();" Visible ="false" />
                                                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="false"   Visible = "false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.StaffStatusBL" EnablePaging="True"
                                                    ID="ObjDSStaffStatus" runat="server" SelectMethod="GetStaffStatusDetails" SelectCountMethod="CountTotalStaffStatusDetails"
                                                    EnableCaching="False">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                            Type="string" />
                                                        <asp:Parameter Name="sortExpression" Type="String" />
                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                        <asp:ControlParameter ControlID="cmbUserRoles" PropertyName="SelectedValue" Name="aiUserRoleId"
                                                            DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="hidChkStaffStatus" PropertyName="Value" Name="asStatusType"
                                                            DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtUserName" PropertyName="Text" Name="asFilter"
                                                            DefaultValue="" />
                                                           <asp:ControlParameter Name="asLocked" ControlID="chkIncludeLockedUSer" PropertyName="Checked" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <asp:HiddenField ID="hidRowCnt" runat="server" />
                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                <asp:HiddenField ID="hidChkStaffStatus" runat="server" Value="" />
                                                <asp:HiddenField ID="hidPageNo" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientlstvwStaffStatusType = "<%=this.lstvwStaffStatusType.ClientID %>";
        _clienthidRowCnt = "<%=this.hidRowCnt.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";

        function ChangeAllStaffStatus() {

            var iRowCount = 0
            var cmAlllStatusType = document.getElementById(_clientlstvwStaffStatusType + "_cmbAllStatusType")
            iRowCount = document.getElementById(_clienthidRowCnt).value;
            while (iRowCount) {
                var hidIsDeleted = document.getElementById(_clientlstvwStaffStatusType + "_ctrl" + (iRowCount - 1) + "_hidIsDeleted").value
                var hidIsLocked = document.getElementById(_clientlstvwStaffStatusType + "_ctrl" + (iRowCount - 1) + "_hidIsLocked").value
                if (hidIsDeleted == "0" && hidIsLocked == "0") {
                    var cmbStaffGroup = document.getElementById(_clientlstvwStaffStatusType + "_ctrl" + (iRowCount - 1) + "_cmbStaffStatusType")
                    cmbStaffGroup.value = cmAlllStatusType.value;
                }
                iRowCount = iRowCount - 1;
            } 
        }
        //This function is used to display message when page index will be changed.
        function MessageAboutUpload(oCmb) {
            var bIsValid;
            if (window.confirm('If you change the page then selected values from current page will get lost. Do you want to continue?'))
                bIsValid = true;
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false;
            }
            return bIsValid;
        }
    </script>

</asp:Content>
