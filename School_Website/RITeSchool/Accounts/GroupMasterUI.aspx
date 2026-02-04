<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="GroupMasterUI.aspx.cs" Inherits="GroupMasterUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<asp:UpdatePanel runat="server" ID="upnlMain">
	<ContentTemplate>
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 800px; vertical-align: top">
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="2" width="100%">
                    <tr>
                        <td align="right" colspan="2" style="padding-right: 10px; top: 20px; height: 19px;">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSumErrorMsg"
												   runat="server"
												   ValidationGroup="Save"
												   CssClass="ClsLabel"
												   ShowSummary="true" />
                            <asp:CustomValidator ID="cstvalDuplicateGroupName"
												 runat="server"
												 ClientValidationFunction="DuplicateGroupName"
												 ErrorMessage="Group Name should not be blank."
												 ValidationGroup="Save"
												 Display="None" />
                            <asp:CustomValidator ID="cstValidteGroupNature"
												 runat="server"
												 ClientValidationFunction="ValidateGroupNature"
												 ErrorMessage="Group Nature should be selected."
												 ValidationGroup="Save"
												 Display="None" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblError"
									   runat="server"
									   EnableViewState="false"
									   ForeColor="Red"
									   CssClass="ClsTextNormal" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 900px;">
                <asp:Label ID="lblUpdateSucess"
						   runat="server"
						   CssClass="ClsLabelUpdate"
						   EnableViewState="False"
						   Font-Bold="True"
						   ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <td align="center">
                 <table cellpadding="0" cellspacing="2" align="center" width="370px">
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">Group Name :</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGroupName"
										 runat="server"
										 MaxLength="100"
										 CssClass="LrgTxtBox" />
                            <span class="ClsMdtStar" style="position: absolute; margin-left: 5px;"> *</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">Parent Group :</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbParentGroup"
											  class="MidCombo"
											  runat="server"
											  Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">Group Nature** :</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbGroupNature"
											  runat="server"
											  class="MidCombo"
											  Width="150px" />
                            <span class="ClsMdtStar" runat="server" id="spanError" style="position: absolute; margin-left: 5px;"> *</span>
                        </td>
                    </tr>
                    <tr visible="false">
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">Consider In Trial Balance : </span>
                        </td>
                        <td class="ClsLabel" align="left">
                            <asp:CheckBox ID="chkTrialBalance"
										  runat="server"
										  Checked="false" />
                        </td>
                    </tr>
                    <tr visible="true">
                        <td align="left" class="ClsBorderlight">
                            <span class="ClsLabel">PAN Details Required? : </span>
                        </td>
                        <td class="ClsLabel" align="left">
                            <asp:CheckBox ID="chkIsPanRequired"
										  runat="server"
										  Checked="false" />
                        </td>
                    </tr>
					<tr>
						<td align="left" colspan="2">
                            <table cellspacing="2">
                                <tr>
                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4; width:20px">
                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">** </span>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                        <span class="LblSmlV">Group Nature is only applicable for Primary Groups.</span>
                                    </td>
                                </tr>
                            </table>
						</td>
					</tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="height: 36px">
                <asp:Button ID="btnSave"
							runat="server"
							Text="Save"
							ValidationGroup="Save"
							CssClass="ClsBtn"
							disable-page="true"
							OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel"
							runat="server"
							Text="Cancel"
							CssClass="ClsBtn"
							UseSubmitBehavior="false"
							CausesValidation="false"
							OnClick="btnCancel_Click" />
                <asp:ObjectDataSource ID="objdsGroupList"
									  runat="server"
									  TypeName="SchoolBusinessService.AccountGroupClient"
									  SelectMethod="GetPagedGroups"
									  SelectCountMethod="GetGroupsCount"
									  EnablePaging="true" >
                    <SelectParameters>
                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="Int32" />
                        <asp:SessionParameter Name="aiFinancialYearId" SessionField="S_FINANCIAL_YEAR_ID" Type="Int32" />
                        <asp:ControlParameter ControlID="hidSortExpression" PropertyName="Value" Name="sortExpression" Type="String" />
                        <asp:ControlParameter ControlID="hidSortDirection" PropertyName="Value" Name="sortDirection" Type="String" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                        <asp:Parameter Name="maximumRows" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="tblGroups">
                    <tr>
                        <td>
							<asp:UpdatePanel runat="server" UpdateMode="Conditional"><ContentTemplate>
                            <asp:ListView ID="lstvwGroupDetails"
										  runat="server"
										  DataKeyNames="GroupNature,IsConsideredForTrialBalance,Id,ParentGroup,IsPrimary,IsSystemDefined,IsPANDetailsRequired"
										  OnItemCommand="lstvwGroupDetails_ItemCommand"
										  OnItemDataBound="lstvwGroupDetails_ItemDataBound"
										  OnDataBound="lstvwGroupDetails_DataBound">
                                <LayoutTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwGroupDetails" PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false" Text="<%# Container.StartRowIndex + 1%>" />
                                                                <span class="LblNormal"> To </span>
                                                                <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount) ? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                <span class="LblNormal"> Out of </span>
                                                                <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                <span class="LblNormal"> Records</span>
                                                                <br />
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                    </table>
                                    <table align="center" width="700px" class="GridBorder" cellspacing="1" cellpadding="3">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" style="font-size: 9pt; width: 175px;">
                                                <asp:LinkButton ID="lnkGroupName"
																runat="server"
																CommandArgument="Name"
																CommandName="SORT_ROW"
																CausesValidation="false"
																ForeColor="#333333"
																Text="Group Name" />
                                            </th>
                                            <th align="left" style="font-size: 9pt; width: 175px;">
                                                <asp:LinkButton ID="lnkParentGroup"
																runat="server"
																CommandArgument="ParentName"
																CommandName="SORT_ROW"
																CausesValidation="false"
																ForeColor="#333333"
																Text="Parent Group" />
                                            </th>
											<th align="left" style="font-size: 9pt; width: 150px;">
                                                <asp:LinkButton ID="lnkbtnGroupNature"
																runat="server"
																CommandArgument="GroupNatureName"
																CommandName="SORT_ROW"
																CausesValidation="false"
																ForeColor="#333333"
																Text="Group Nature" />
											</th>
                                            <th align="center" style="font-size: 9pt; width: 70px;">
                                                <span>Is Primary?</span>
                                            </th>
                                            <th align="center" style="font-size: 9pt; width: 50px;">
                                                Action
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                        <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                            <td colspan="6" style="padding: 0px;">
                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="" PageSize="20">
                                                    <Fields>
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <span class="LblNrmlB">Select a page :</span>
                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged" />
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
                                    <tr id="trGridRow" runat="server" class="ClsGridRow">
                                        <td align="left">
                                            <asp:Label ID="lblGroupName"
													   runat="server"
													   Text='<%# Eval("Name") %>' />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblParentGroupName"
													   runat="server"
													   Text='<%# Eval("ParentGroup.Name") %>' />
                                        </td>
										<td align="left">
											<asp:Label ID="lblGroupNature"
													   runat="server"
													   Text='<%# Eval("GroupNature.Name") %>' />
										</td>
                                        <td align="center">
											<img id="imgIsPrimary" runat="server" src="../images/IconGrid_AssignTrue.gif" alt="Is Primary" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgBtnEdit"
															 runat="server"
															 CausesValidation="false"
															 CommandName="UpdateCommand"
															 ImageUrl="../images/IconGrid_Edit.gif"
															 style="vertical-align: middle;" />
                                            <asp:ImageButton ID="imgBtnDelete"
															 runat="server"
															 CausesValidation="false"
															 CommandName="RemoveCommand"
															 ImageUrl="../images/IconGrid_Delete.gif"
															 style="vertical-align: middle;" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
								<EmptyDataTemplate>
									<div class="LblNoRecord" style="margin: 10px 0;">No record found.</div>
								</EmptyDataTemplate>
                            </asp:ListView>
							</ContentTemplate></asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack"
							runat="server"
							Text="Back"
							CssClass="ClsBtn"
							CausesValidation="False"
							UseSubmitBehavior="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidMode" runat="server" />
                <asp:HiddenField ID="hidGroupId" runat="server" />
                <asp:HiddenField ID="hidRowNo" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidSortDirection" runat="server" />
				<asp:HiddenField ID="hidGroupsJSON" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
	</ContentTemplate>
	</asp:UpdatePanel>
    <script type="text/javascript" language="javascript">
        _clienttxtGroupName = "<%=this.txtGroupName.ClientID%>"
        _clientcstvalDuplicateGroupName = "<%=this.cstvalDuplicateGroupName.ClientID %>"
        _clientlstvwGroupDetails = "<%=this.lstvwGroupDetails.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID%>"
        _clientlblError = "<%=this.lblError.ClientID %>"
        _clientcmbParentGroup = "<%=this.cmbParentGroup.ClientID %>"
        _clientcmbGroupNature = "<%=this.cmbGroupNature.ClientID %>"
        //        _clientchkTrialBalance = "<%=this.chkTrialBalance.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>"
        _clientcstValidteGroupNature = "<%=this.cstValidteGroupNature.ClientID %>"
        _clientspanError = "<%=this.spanError.ClientID %>"
        _clienthidMode = "<%=this.hidMode.ClientID %>"
        _clienthidGroupId = '<%=this.hidGroupId.ClientID %>';

        _groups = eval('[' + $get('<%=this.hidGroupsJSON.ClientID%>').value + ']')[0];
        
		var prm = Sys.WebForms.PageRequestManager.getInstance();       
        prm.add_endRequest(EndRequestHandler);		

		function EndRequestHandler() {
			_groups = eval('[' + $get('<%=this.hidGroupsJSON.ClientID%>').value + ']')[0];
		}

        function ValidateGroupNature(oSrc, args) {
            var ParentGroupIndex = $get(_clientcmbParentGroup).selectedIndex;
            var GroupNatureIndex = $get(_clientcmbGroupNature).selectedIndex;
            $get(_clientlblUpdateSucess).innerText = "";
            $get(_clientlblUpdateSucess).innerHTML = "";
            if (ParentGroupIndex == 0) {
                if (GroupNatureIndex == 0) {
                    oSrc.errormessage = "Group Nature should be selected.";
                    $get(_clientcstValidteGroupNature).innerText = "Group Nature should be selected.";
                    args.IsValid = false
                    return true
                }
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ParentGroupOnChange(src) {
			// Check if the selected Parent Group is the same as the one being edited.
			if ($get(_clienthidGroupId).value == src.value) {
				alert('Parent Group should not be the same as the current Group.');
				src.selectedIndex = src.oldIndex;
				return;
			}
			
			var cmbGroupNature = $get(_clientcmbGroupNature);
			cmbGroupNature.disabled = false;
			if(src.selectedIndex != 0) {
				cmbGroupNature.value = _groups[src.value].Id;
				cmbGroupNature.disabled = true;
				$get(_clientspanError).style.display = "none";
			}
			else {
				cmbGroupNature.selectedIndex = 0;
				$get(_clientspanError).style.display = "";
			}

//			cmbGroupNature.disabled = src.selectedIndex != 0;
//			$get(_clientspanError).style.display = cmbGroupNature.disabled ? "none" : "";

//            var iParentIndex = $get(_clientcmbParentGroup).selectedIndex;
//            var iNatureIndex = $get(_clientcmbGroupNature).selectedIndex;
//            var SpanError = $get(_clientspanError);
//            if (iParentIndex != 0) {
//                $get(_clientcmbGroupNature).disabled = true;
//                $get(_clientcmbGroupNature).selectedIndex = 0;
//                $get(_clientspanError).style.display = "none";
//            }
//            else {

//                $get(_clientspanError).style.display = "";
//                $get(_clientcmbGroupNature).disabled = false;
//            }
        }

        function DuplicateGroupName(oSrc, args) {
            $get(_clientlblError).innerText = "";
            $get(_clientlblError).innerHTML = "";
            $get(_clientlblUpdateSucess).innerText = "";
            $get(_clientlblUpdateSucess).innerHTML = "";

            var GroupName = "";
            var sRowNo = "";
            var iRowNumber = 0;
            var iRowNo = $get(_clienthidRowNo).value
            var txtGroupName = ($get(_clienttxtGroupName).value).trim();

            var lblGroup = $get(_clientlstvwGroupDetails + "_ctrl" + iRowNumber + "_lblGroupName")
            if (txtGroupName != "") {
                while (lblGroup) {
                    if (iRowNo != "-999") {
                        if (txtGroupName.toLowerCase() == (lblGroup.innerHTML).toLowerCase() && iRowNumber != iRowNo) {
                            if (sRowNo == "")
                                sRowNo = (iRowNumber + 1);
                            else
                                sRowNo += ", " + (iRowNumber + 1);
                        }
                    }
                    else {
                        if (txtGroupName.toLowerCase() == (lblGroup.innerHTML).toLowerCase()) {
                            if (sRowNo == "")
                                sRowNo = (iRowNumber + 1);
                            else
                                sRowNo += ", " + (iRowNumber + 1);
                        }
                    }
                    iRowNumber += 1;
                    lblGroup = $get(_clientlstvwGroupDetails + "_ctrl" + iRowNumber + "_lblGroupName")
                }
            }
            if (sRowNo != "") {
                oSrc.errormessage = "Group Name should not be duplicated for row(s): " + sRowNo + ".";
                $get(_clientcstvalDuplicateGroupName).innerText = "Group Name should not be duplicated for row(s): " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else if (txtGroupName == "") {
                oSrc.errormessage = "Group Name should not be blank.";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this group?')) {
                bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>
