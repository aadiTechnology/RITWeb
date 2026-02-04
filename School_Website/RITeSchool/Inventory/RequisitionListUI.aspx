<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RequisitionListUI.aspx.cs" Inherits="RequisitionListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        &nbsp;<table width="97%">
            <tr runat="server" id="trCombo">
                <td align="left">
                    <asp:UpdatePanel ID="UpdtpnlMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td class="TxtNormal" align="center" colspan="2">
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                            EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" colspan="1" width="20%">
                                        <span class="ClsLabel" id="lblStatus">Status :</span>
                                    </td>
                                    <td colspan="1" width="80%">
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" Width="185px"
                                            TabIndex="2" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="trLstItems" runat="server">
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdtpnlListView">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="Tr5" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwRequisition"
                                                                Visible="true">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <div>
                                                                <asp:ListView ID="lstvwRequisition" runat="server" DataKeyNames="RequisitionID,NextDesignationId,CreaterName,CreatedId,RequisitionCode,IsFinalApproval,StatusID,ExpiryDate"
                                                                    OnDataBound="lstvwRequisition_DataBound" OnItemDataBound="lstvwRequisition_ItemDataBound"
                                                                    OnItemCommand="lstvwRequisition_ItemCommand" OnSorting="lstvwRequisition_Sorting">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                            cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="ClspaddingL" style="width: 8%">
                                                                                    <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="Sort" CommandArgument="RequisitionCode"
                                                                                        ForeColor="Black">
                                                                                                  Code</asp:LinkButton>
                                                                                </th>
                                                                                <th id="thRequisition" align="left" class="ClspaddingL" style="width: 25%">
                                                                                    <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="RequisitionName"
                                                                                        ForeColor="Black">
                                                                                                  Requisition</asp:LinkButton>
                                                                                </th>
                                                                                <th class="ClspaddingL" style="width: 10%">
                                                                                    Status
                                                                                </th>
                                                                                <th class="ClspaddingL" style="width: 25%">
                                                                                    <asp:LinkButton ID="lnkCreaterName" runat="server" CommandName="Sort" CommandArgument="CreaterName"
                                                                                        ForeColor="Black">
                                                                                                Requestor</asp:LinkButton>
                                                                                </th>
                                                                                <th style="width: 11%">
                                                                                    <asp:LinkButton ID="lnkSortDate" runat="server" CommandName="Sort" CommandArgument="Created_Date"
                                                                                        ForeColor="Black">
                                                                                                  Request Date</asp:LinkButton>
                                                                                </th>
                                                                                <th style="width: 9%">
                                                                                    <asp:LinkButton ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="ExpiryDate"
                                                                                        ForeColor="Black">
                                                                                            Expiry Date</asp:LinkButton>
                                                                                </th>
                                                                                <th>
                                                                                    Edit/View
                                                                                </th>
                                                                                <th>
                                                                                    Delete
                                                                                </th>
                                                                                <th style="width: 3%">
                                                                                    Cancel
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                                <td colspan="10">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwRequisition"
                                                                                        PageSize="20">
                                                                                        <Fields>
                                                                                            <asp:TemplatePagerField>
                                                                                                <PagerTemplate>
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td align="right" cssclass="LblNormal">
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
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                            </td>
                                                                           <td id="tdReqName" runat="server" align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("RequisitionName") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCreaterName" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Created_Date","{0:dd-MMM-yyyy}")%>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate","{0:dd-MMM-yyyy}")%>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnEditReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("Editble"))%>' ToolTip="Edit" />
                                                                                <asp:ImageButton ID="imgbtnViewReq" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                    Visible='<%# !Convert.ToBoolean(Eval("Editble"))%>' ToolTip="View" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnDeleteReq" CommandArgument='<%# Eval("RequisitionID") %>'
                                                                                    runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("IsDelete"))%>' ToolTip="Delete" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="imgbtnCancelReq" runat="server" CommandName="CANCEL_COMMAND"
                                                                                    ToolTip="Cancel">Cancel</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                            </td>
                                                                            <td id="tdReqName" runat="server" align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("RequisitionName") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCreaterName" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Created_Date","{0:dd-MMM-yyyy}")%>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate","{0:dd-MMM-yyyy}")%>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnEditReq" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("Editble"))%>' ToolTip="Edit" />
                                                                                <asp:ImageButton ID="imgbtnViewReq" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                    Visible='<%# !Convert.ToBoolean(Eval("Editble"))%>' ToolTip="View" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnDeleteReq" CommandArgument='<%# Eval("RequisitionID") %>'
                                                                                    runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("IsDelete"))%>' ToolTip="Delete" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="imgbtnCancelReq" runat="server" CommandName="CANCEL_COMMAND"
                                                                                    ToolTip="Cancel">Cancel</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td class="LblNoRecord" align="center">
                                                                                    No record found.
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.RequisitionBL" EnablePaging="true"
                                                                ID="lstDSobj" runat="server" SelectMethod="GetRequisitionList" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountRowsOfRequisition" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter Name="aiStatus" Type="int32" ControlID="ddlStatus" PropertyName="SelectedValue" />
                                                                    <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnAdd" runat="server" CssClass="ClsBtnSml" Height="24px" CausesValidation="false"
                                            Text="Add" Visible="True" PostBackUrl="~/RITeSchool/Inventory/AddRequisitionUI.aspx"
                                            TabIndex="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidRequisitionId" runat="server" />
                                        <asp:HiddenField ID="hidStatusID" runat="server" />
                                        <asp:HiddenField ID="hidStatus" runat="server" />
                                        <asp:HiddenField ID="hidCode" runat="server" />
                                        <asp:HiddenField ID="hidRequistion" runat="server" />
                                        <asp:HiddenField ID="hidRequester" runat="server" />
                                        <asp:HiddenField ID="hidCreatedId" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="Sorting" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divCancelReason" runat="server" style="visibility: hidden; display: none;
                        position: absolute; margin: 0px; padding: 0px; width: 380px; height: 300px; border-width: 0px;
                        left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 180px;
                        background-color: white;">
                        <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; padding: 4px; color: Black; text-align: right;">
                            <div style="padding: 1px; font-size: 12px; font-weight: bold; color: Black; float: left;">
                                Cancel Approved Requisition Popup!!!</div>
                            <span style="cursor: hand" onclick="javascript:HidePopup(false);">
                                <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                            </span>
                        </div>
                        <div style="padding: 15px; text-align: left; height: 107px; width: 349px;" class="ClsLabel">
                            <table style="width: 353px; height: 120px">
                                <tr>
                                    <td class="ClsBorderlight" style="white-space: nowrap; width: 2px">
                                        <span class="LblNormal">Code :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <span id="lblCCode" runat="server" class="LblNormal" style="color: Purple" enableviewstate="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="white-space: nowrap">
                                        <span class="LblNormal">Requisition :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <span id="lblDivRequisition" runat="server" class="LblNormal" enableviewstate="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="white-space: nowrap">
                                        <span class="LblNormal">Status :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <span id="lblDivStatus" runat="server" class="LblNormal" enableviewstate="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight" style="white-space: nowrap">
                                        <span class="LblNormal">Requester :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <span id="lblDivRequester" runat="server" class="LblNormal" enableviewstate="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap">
                                        <span class="LblNormal">Reason to cancel :</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtReason" Width="336px" Height="75px" CssClass="MidTxtBox" MaxLength="500"
                                            TextMode="MultiLine" runat="server"></asp:TextBox>
                                        <span style="color: #ff0000">*</span>
                                    </td>
                                </tr>
                                <tr style="height: 40px;">
                                    <td align="center" colspan="2">
                                        <asp:Button ID="btnSave" runat="server" OnClientClick="if(!ValidateControl()) return false;"
                                            CssClass="ClsBtn" Style="margin-left: 5px; cursor: pointer;" Text="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                            OnClientClick="javascript:HidePopup(false);return false;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm('Are you sure you want to delete this Requisition?')) {
                bResult = false;
            }
            return bResult;
        }

        //This function is used to validate client side validations.
        function ValidateControl() {
            var ReasonText = $get("<%=this.txtReason.ClientID %>").value;
            var bResult = true;
            if (ReasonText.length == 0) {
                alert('Reason Should not be blank.');
                bResult = false;
            }

            if (bResult == true) {
                if (!window.confirm('Do you want to cancel this requisition?'))
                    bResult = false;
            }
            return bResult;
        }

        //This function is used to set load controls or labels of div.
        function setDivLabels() {
            var code = $get("<%=this.hidCode.ClientID %>").value;
            var Requisitionname = $get("<%=this.hidRequistion.ClientID %>").value
            var status = $get("<%=this.hidStatus.ClientID %>").value;
            var Requester = $get("<%=this.hidRequester.ClientID %>").value;

            $get("<%=this.lblCCode.ClientID %>").innerHTML = code;
            $get("<%=this.lblDivRequisition.ClientID %>").innerHTML = Requisitionname;
            $get("<%=this.lblDivStatus.ClientID %>").innerHTML = status;
            $get("<%=this.lblDivRequester.ClientID %>").innerHTML = Requester;
            $get("<%=this.txtReason.ClientID %>").innerHTML = '';

        }

        //This function is used to open Div controle or popup.
        function OpenReasonWindow() {
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divCancelReason.ClientID %>").style

            var pageWidth = window.screen.width
            var pageHeight = 400
            var left = parseInt((pageWidth / 4.5))
            var top = parseInt((pageHeight / 1.5))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            setDivLabels();
            return true;
        }

        //this function is used hid popup.
        function HidePopup() {
            $get("<%=this.divCancelReason.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divCancelReason.ClientID %>").style.display = "none"
            return false;
        }
    </script>
    <script language="javascript" type="text/javascript">

        _cltdivAttendanceAlert = "<%=this.divCancelReason.ClientID %>"
        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;

        window.onresize = setTotal;
        window.onscroll = setTotal;
        window.onload = setTotal;

        function setTotal() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;

            if (document.getElementById(_cltdivAttendanceAlert) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivAttendanceAlert).style.height);
                document.getElementById(_cltdivAttendanceAlert).style.top = _rightFooterPos;
            }
            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltdivAttendanceAlert) != null) {
                    document.getElementById(_cltdivAttendanceAlert).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }

        }

    </script>
</asp:Content>
