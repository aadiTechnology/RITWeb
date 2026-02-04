<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ConfigureShiftUI.aspx.cs" Inherits="ConfigureShiftUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                            vertical-align: top">
                            <tr>
                                <td id="MainDataTable" align="center">
                                    <!--Insert Data Here-->
                                    <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 77%">
                                                            <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                               <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label></asp:Panel>
                                                                    </td>
                                                        
                                                            
                                                            <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                                    <td style="width: 77%">
                                                                     <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                                                     <asp:Label ID="lblCheckDependency" Style="text-align: left" runat="server" ForeColor="Red"
                                                                    Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false" ></asp:Label>
                                                                    </asp:Panel>
                                                                    </td>
                                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="ClsTextNormal" align="center">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </table>
                        <!--Shift Configuration starts here-->
                        <table id="tblShiftname" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 7%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span id="spnShiftHeader" runat="server" class="ClsLabel">Shift Name :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:TextBox ID="txtShiftName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                        Width="186px"></asp:TextBox>
                                    *&nbsp;
                                    <asp:RequiredFieldValidator ID="reqShiftName" runat="server" ControlToValidate="txtShiftName"
                                        Display="None" ErrorMessage="Shift Name should not be blank."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trJourneyType" runat="server" visible = "false">
                                <td style="width: 7%">
                                </td>
                                <td align="left" class="ClsBorderLight" style="width: 19%">
                                    <span class="ClsLabel">Journey Type :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                    <asp:DropDownList ID="cmbJourneyType" runat="server" CssClass="MidCombo">
                                    <asp:ListItem Text="-- Select" Value = "0"></asp:ListItem>
                                    <asp:ListItem Text="Pick Up" Value = "1"></asp:ListItem>
                                    <asp:ListItem Text="Drop" Value = "2"></asp:ListItem>
                                    </asp:DropDownList>  
                                    *&nbsp;
                                     <asp:RequiredFieldValidator ID="reqValJourneyType" runat="server" ControlToValidate="cmbJourneyType" InitialValue="0" Enabled = "false"
                                        Display="None" ErrorMessage="Journey Type should be selected."></asp:RequiredFieldValidator>                                  
                                </td>
                            </tr>
                        </table>
                        <table id="tblSaveShiftname" runat="server" border="0" cellpadding="1" cellspacing="2"
                            style="width: 46%;" align="center">
                            <tr>
                                <td style="width: 30%">
                                    &nbsp
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="true" OnClick="btnSave_Click" disable-page="true" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpnlListView" runat="server">
                    <ContentTemplate>
                        <table>                       
                            <tr id="tr1" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwConfigureShift">
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
                        </table>
                        <table id="tblShiftList" align="center" width="46%">
                            <tr align="center" style="width: 100%">
                                <td align="center" style="width: 800">
                                    <asp:ListView ID="lstvwConfigureShift" runat="server" DataKeyNames="TransportShiftId, TransportShiftName"
                                        OnSorting="lstvwConfigureShift_Sorting" DataSourceID="ObjDSConfigureShift" OnItemDataBound="lstvwConfigureShift_ItemDataBound"
                                        OnDataBound="lstvwConfigureShift_DataBound" OnItemCommand="lstvwConfigureShift_ItemCommand">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblShiftInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" style="padding-left: 9px;">
                                                        <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                            CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                    </th>
                                                    <th align="left" width="150px" style="padding-left: 9px;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="JourneyTypeId"
                                                            CausesValidation="false" ForeColor="Black"> Journey Type </asp:LinkButton>
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Edit
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="5">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfigureShift"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageCnt_SelectedIndexChanged">
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
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" TransportShiftName") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval(" JourneyType") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESHIFT"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVESHIFT"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" align="left">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" TransportShiftName") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval(" JourneyType") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESHIFT"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="REMOVESHIFT" CausesValidation="false"
                                                        runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>                                      
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource TypeName="BusinessLogic.ShiftMasterBL" EnablePaging="True"
                            ID="ObjDSConfigureShift" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                            EnableCaching="False" SelectCountMethod="CountTotalShiftMaster">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                    Type="int32" />
                                <asp:Parameter Name="sortExpression" Type="String" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidServerDate" runat="server" />
                        <asp:HiddenField ID="hidTransportShiftId" runat="server" />
                        <asp:HiddenField ID="hidTransportShiftName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                    CausesValidation="False" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientcstbtnSave = "<%=this.btnSave.ClientID%>"
        _clientcstbtnCancel = "<%=this.btnCancel.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbllblCheckDependency = "<%=this.lblCheckDependency.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientTransportShiftId = "<%=this.hidTransportShiftId.ClientID %>"
        _ClienthidTransportShiftName="<%=this.hidTransportShiftName.ClientID %>"
        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function ResetUpdateLbl() 
        {
            if (document.getElementById(_clientlblUpdateSucess) != null) 
            {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlbllblCheckDependency) != null) 
            {
                document.getElementById(_clientlbllblCheckDependency).style.display = "none"
                document.getElementById(_clientlbllblCheckDependency).innerHTML = ""
            }
            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).style.display = "none"
                document.getElementById(_clientcst_LblErrMsg).innerHTML = ""
            }

        }
           
    </script>

</asp:Content>
