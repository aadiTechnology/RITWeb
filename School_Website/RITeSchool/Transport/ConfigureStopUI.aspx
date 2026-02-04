<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ConfigureStopUI.aspx.cs" Inherits="ConfigureStopUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style>
        .clsLabelC {
            font-family: open sans;
        }
    </style>
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
                                                                    Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="Add"
                                                                ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </td> </tr>
                        <tr>
                            <td colspan="1" class="ClsTextNormal" align="center">
                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </table>
                                <!--Stop Configuration starts here-->
                                <table id="tblStopname" runat="server" border="0" cellpadding="1" cellspacing="2"
                                    style="width: 45%;" align="center">
                                    <tr align="center">
                                        <td align="left" class="ClsBorderLight" style="width: 20%">
                                            <span class="ClsLabel">Stop Name :</span>
                                        </td>
                                        <td class="ClsMdtStar" align="left">
                                            <asp:TextBox ID="txtStopName" runat="server" MaxLength="100" CssClass="LrgTxtBox"
                                                Width="186px"></asp:TextBox>
                                            *
                                            <asp:RequiredFieldValidator ID="reqStopName" runat="server" ControlToValidate="txtStopName"
                                                ValidationGroup="Add" Display="None" ErrorMessage="Stop Name should not be blank."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                   
                                     <tr>
                                        <td colspan="2" align="left">
                                            <table>
                                                <tr>
                                                     <td align="left" colspan="1" class="ClsBorderlight " style="background-color: #ffffc4;">
                                                        <span class="LblNrmlB">Note :</span>
                                                    </td>
                                                    <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px;">
                                                        <asp:Label ID="lblVerifyNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Charges applicable (In Rs) is used to specify charges for a month."></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    
                                    <tr align="center">
                                        <td colspan="2" align="center">
                                            <asp:ListView ID="lstvwStopCharges" runat="server" DataKeyNames="RoleId,Charges,OneWayCharges">
                                                <LayoutTemplate>
                                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                            <th align="left" width="30%" class="paddingL" style="white-space: nowrap;">
                                                                User Role
                                                            </th>
                                                            <th align="center" width="35%" style="white-space: nowrap;">
                                                                Charges Applicable (In Rs)
                                                            </th>
                                                            <th align="center" width="35%" style="white-space: nowrap;">
                                                                One Way Charges (In %)
                                                            </th>
                                                        </tr>
                                                        <tr id="trHeaderControls" runat="server" class="ClsGridHeader">
                                                            <th>
                                                            </th>
                                                            <th align="center">
                                                                <asp:TextBox ID="txtAllCharges" runat="server" MaxLength="5" CssClass="LrgTxtBox"
                                                                    onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                   onkeyup="OnGridKeyUpNumber(this,0,false,event);" Width="80px"
                                                                    onchange="SetValueToAllCharges(this,'_txtCharges')"></asp:TextBox>
                                                            </th>
                                                            <th align="center">
                                                                <asp:TextBox ID="txtAllOneWayCharges" runat="server" MaxLength="3" CssClass="LrgTxtBox"
                                                                    onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                   onkeyup="OnGridKeyUpNumber(this,0,false,event);" Width="80px"
                                                                    onchange="SetValueToAllCharges(this,'_txtOneWayCharges')"></asp:TextBox>
                                                                <asp:CustomValidator ID="cstValHouseColor" runat="server" ClientValidationFunction="ValidatetxtCharges"
                                                                    SetFocusOnError="True" Display="None" ValidationGroup="Add"  ErrorMessage="One way charges % should not be greater than 100."></asp:CustomValidator>
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                        <td align="left">
                                                            <asp:Label ID="lblUserRole" runat="server" CssClass="clsLabelC" Text='<%#Eval("RoleName") %>'></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtCharges" runat="server" MaxLength="5" CssClass="LrgTxtBox" Width="80px"
                                                                onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                 Text="0" onkeyup="OnGridKeyUpNumber(this,0,false,event);"></asp:TextBox>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtOneWayCharges" runat="server" MaxLength="3" CssClass="LrgTxtBox"
                                                                onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                Width="80px" Text="0" onkeyup="OnGridKeyUpNumber(this,0,false,event);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                        <td align="left">
                                                            <asp:Label ID="lblUserRole" runat="server" CssClass="clsLabelC" Text='<%#Eval("RoleName") %>'></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtCharges" runat="server" MaxLength="5" CssClass="LrgTxtBox" Width="80px"
                                                                onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                 Text="0" onkeyup="OnGridKeyUpNumber(this,0,false,event);" ></asp:TextBox>
                                                        </td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtOneWayCharges" runat="server" MaxLength="3" CssClass="LrgTxtBox"
                                                                onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                               Width="80px" Text="0" onkeyup="OnGridKeyUpNumber(this,0,false,event);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No record found.
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                    style="width: 300px;" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                disable-page="true" CausesValidation="true" OnClick="btnAdd_Click" ValidationGroup="Add" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                CausesValidation="true" OnClick="btnCancel_Click" />&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblLinks" runat="server" align="center" width="70%">
                                    <tr>
                                        <td align="right" style="height: 25px" colspan="2">
                                            <asp:LinkButton ID="lnkDetails" runat="server" class="ClsGreenBG" Text="Transport Service Duration Setting"
                                                OnClientClick="OpenSettingPopup();" CssClass="SubTitle"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server" ID="upnl2">
                    <ContentTemplate>
                    <table  border="0" cellpadding="1" cellspacing="2"
                                    style="width: 449px;" align="center">
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 20%">
                                            <span class="ClsLabel">Stop Name :</span>
                                        </td>
                                        <td class="ClsMdtStar" align="left" style="width: 190px">
                                            <asp:TextBox ID="txtSearchStops" runat="server" MaxLength="100" CssClass="LrgTxtBox"
                                                Width="186px"></asp:TextBox>
                                         
                                        </td>
                                        <td>
                                        <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                                disable-page="true" CausesValidation="true" onclick="btnSearch_Click" />
                                        </td>
                                        
                                    </tr>
                                </table>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwConfigureStop"
                                        PageSize="2">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                        </table>
                            
                                    
                                    
                        <table id="tblStopList" align="center" width="70%">
                    
                            <tr align="center" style="width: 100%">
                                <td align="center" style="width: 100%">
                                    <asp:ListView ID="lstvwConfigureStop" runat="server" DataKeyNames="StopId, StopName"
                                        OnSorting="lstvwConfigureStop_Sorting" DataSourceID="ObjDSConfigureStop" OnItemDataBound="lstvwConfigureStop_ItemDataBound"
                                        OnDataBound="lstvwConfigureStop_DataBound" OnItemCommand="lstvwConfigureStop_ItemCommand">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" width="20%" style="padding-left: 9px;">
                                                        <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                            CausesValidation="false" ForeColor="Black"> Name </asp:LinkButton>
                                                    </th>
                                                    <th align="left" width="30%" style="padding-left: 9px;">
                                                        Charges (In Rs)
                                                    </th>
                                                    <th align="left" width="30%" style="padding-left: 9px;">
                                                        One Way Charges (In %)
                                                    </th>
                                                    <th align="center" width="10%">
                                                        Edit
                                                    </th>
                                                    <th align="center" width="10%">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="5">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfigureStop"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
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
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" StopName") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Charges") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("OneWayCharges") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESTOP"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVESTOP"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td class="paddingL" align="left">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" StopName") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Charges") %>'></asp:Label>
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("OneWayCharges") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESTOP"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="REMOVESTOP" CausesValidation="false"
                                                        runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                         <EmptyDataTemplate>
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No record found.
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource TypeName="BusinessLogic.StopMasterBL" EnablePaging="True" ID="ObjDSConfigureStop"
                            runat="server" SelectMethod="GetAll" SortParameterName="sortExpression" EnableCaching="False"
                            SelectCountMethod="Count">
                            <SelectParameters>
                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                    Type="int32" />
                                <asp:ControlParameter ControlID="txtSearchStops" Name ="asStopName" PropertyName = "Text" />
                                <asp:Parameter Name="sortExpression" Type="String" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidServerDate" runat="server" />
                        <asp:HiddenField ID="hidStopId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidStopName" runat="server" />
                      
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
    <div id="divSetting" align="center" runat="server" style="padding-top: 30%; visibility: hidden;
        display: none; position: absolute; margin: 0px; padding: 0px; width: 500px; height: 160px;
        border-width: 1px; left: 500px; top: 500px; line-height: normal; border: solid 2px darkgreen;
        margin: -110px 0px 0px 00px; background-color: white;">
        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
            background-repeat: repeat-x; color: Black; width: 500px; text-align: right">
            <div style="font-size: 12px; width: 450px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                Configure Start and End date for Transport Service
            </div>
            <span style="cursor: hand">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                    border="0" />
            </span>
        </div>
        <div>
            <table>
                <tr>
                    <td colspan="2" width="100%">
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Height="20px" Width="100%"
                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="ClsBorderlight" style="width: 40%" align="right">
                        <span class="LblRht colonPadding">:</span>
                        <asp:Label ID="lblStartDate" runat="server" CssClass="LblRht" Text="<%$ Resources:LocalizedResources, Start_Date%>"
                            EnableViewState="false"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 60%">
                        <asp:TextBox ID="txtStartDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                            TabIndex="1"></asp:TextBox>
                        <rjs:PopCalendar ID="cStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, CalMsgStartDate%>" />
                        <span class="ClsMdtStar" style="color: Red">* </span>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 40%" class="ClsBorderlight" align="right">
                        <span class="LblRht colonPadding">:</span>
                        <asp:Label ID="lblEndDate" runat="server" CssClass="LblRht" Text="<%$ Resources:LocalizedResources, End_Date%>"
                            EnableViewState="false"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 60%" ">
                        <asp:TextBox ID="txtEndDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                            TabIndex="2"></asp:TextBox>
                        <rjs:PopCalendar ID="cEndDate" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, CalMsgEndDate%>" />
                        <span class="ClsMdtStar" style="color: Red">* </span>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" valign="bottom" style="padding: 5px;">
                        <asp:Button ID="btnSavePopUp" runat="server" Text="Save" CssClass="ClsBtnMid" CausesValidation="false"
                            Width="75px" OnClick="btnSavePopup_Click" />
                        <asp:Button ID="btnClosePopUp" runat="server" Text="Close" CssClass="ClsBtnMid" CausesValidation="false"
                            Width="75px" OnClientClick="javascript:HidePopup();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidTransportStardDate" runat="server" />
                         <asp:HiddenField ID="hidTransportEndDate" runat="server" />
                    </td></tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientlblMessage = "<%=this.lblMsg.ClientID %>";
        _clientcstbtnSave = "<%=this.btnAdd.ClientID%>"
        _clientcstbtnCancel = "<%=this.btnCancel.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientStopId = "<%=this.hidStopId.ClientID %>"
        _ClientStopName = "<%=this.hidStopName.ClientID %>"
        _clientlbllblCheckDependency = "<%=this.lblCheckDependency.ClientID %>"
        _clientlstvwStopCharges = "<%=this.lstvwStopCharges.ClientID %>"
        _clienttxtAllCharges = _clientlstvwStopCharges + "_txtAllCharges";
        _clienttxtAllOneWayCharges = _clientlstvwStopCharges + "_txtAllOneWayCharges";
        _clientTransportStartDate = "<%=this.hidTransportStardDate.ClientID %>"
        _clientTransportEndDate = "<%=this.hidTransportEndDate.ClientID %>"
        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
            if (document.getElementById(_clientlbllblCheckDependency) != null) {
                document.getElementById(_clientlbllblCheckDependency).style.display = "none"
                document.getElementById(_clientlbllblCheckDependency).innerHTML = ""
            }
            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).style.display = "none"
                document.getElementById(_clientcst_LblErrMsg).innerHTML = ""
            }
        }

        function SetValueToAllCharges(obj, TextBox) {
            var Charges = obj.value
            var txt
            var iRowCount = 0
            txt = document.getElementById(_clientlstvwStopCharges + "_ctrl" + iRowCount + TextBox)
            while (txt != null) {
                txt.value = Charges
                iRowCount = iRowCount + 1
                txt = document.getElementById(_clientlstvwStopCharges + "_ctrl" + iRowCount + TextBox)
            }
        }

        function OpenSettingPopup() {
            _clientdivTemplates = "<%=this.divSetting.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divSetting.ClientID %>").style
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
        }

        function ClearSuccessfulMessage() {            
            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).style.display = "none"
                document.getElementById(_clientcst_LblErrMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblMessage) != null) {
                document.getElementById(_clientlblMessage).style.display = "none"
                document.getElementById(_clientlblMessage).innerHTML = ""
            }

        }

        function HidePopup() {
        	ClearSuccessfulMessage();        	
        	document.getElementById(_clienttxtStartDate).value = document.getElementById(_clientTransportStartDate).value;         
            document.getElementById(_clienttxtEndDate).value = document.getElementById(_clientTransportEndDate).value;
            $get("<%=this.divSetting.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divSetting.ClientID %>").style.display = "none"
        }

        function ValidatetxtCharges(oSrc, args) {    
            var txtOneWayCharges
            var iRowCount = 0
            args.IsValid = true;
                       
            txtOneWayCharges = document.getElementById(_clientlstvwStopCharges + "_ctrl" + iRowCount + "_txtOneWayCharges")            
            while (txtOneWayCharges!=null) 
            {
                if (txtOneWayCharges.value > 100) 
                {
                    args.IsValid = false;
                    break;
                }
                iRowCount = iRowCount + 1
                txtOneWayCharges = document.getElementById(_clientlstvwStopCharges + "_ctrl" + iRowCount + "_txtOneWayCharges")
            }
               return !args.IsValid;
           }
           function OnGridKeyUpNumber(obj, decimalPlaces, allowNegative, e) {
               extractNumber(obj, decimalPlaces, allowNegative);
               UpDownKeyPress(obj.id, e);
           }


    </script>
</asp:Content>
