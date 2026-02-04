<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="BlockProgressReportUI.aspx.cs" Inherits="BlockProgressReportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
	<div>
		<div>
			<table width="100%">
				<tr>
					<td>
						<asp:ValidationSummary ID="valSummary" runat="server" CssClass="lblNormal" ShowSummary="true"
							ValidationGroup="Block" />
					</td>
				</tr>
			</table>
		</div>
		<div style="width: 500px; height: 20px">
			<asp:Label ID="lblErrorMesage" runat="server"></asp:Label>
			<asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
				Visible="true" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
		</div>
		<div style="margin-top: 10px">
			<table width="750px">
				<tr>
					<td align="center">
						<table>
							<tr>
								<td id="tdteacher" runat="server" class="ClsBorderlight" style="padding-left: 30px">
									<span class="ClsLabel">Class Teacher : </span>
								</td>
								<td id="tdteacherdropdown" runat="server">
									<asp:DropDownList ID="cmbTeachers" runat="server" Width="200px" AutoPostBack="true"
										CssClass="LrgCombo" OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
									</asp:DropDownList>
								</td>
								<td class="ClsBorderlight" style="padding-left: 30px">
									<span class="ClsLabel">Student Name :</span>
								</td>
								<td>
									<asp:DropDownList ID="cmbStudent" runat="server" Width="200px" AutoPostBack="true"
										CssClass="LrgCombo" OnSelectedIndexChanged="cmbStudent_SelectedIndexChanged">
									</asp:DropDownList>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</div>
		<div style="margin-top: 10px">
			<table width="660px">
				<tr>
					<td class="ClsBorderlight" style="padding-left: 100px">
						<span class="ClsLabel">Name / Reg. No. :</span>
					</td>
					<td class="ClsBorderlight" style="width: 300px;">
						<asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="LrgTxtBox" ClientIDMode="Static" autocomplete="off"></asp:TextBox>
						<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click" />
					</td>
				</tr>
			</table>
		</div>
            <table>
                <tr>
                    <td>
                        <asp:RadioButton ID="optBlocked" runat="server" Text="Show Blocked Students" AutoPostBack="true"
                            CssClass="ClsLabel" GroupName="student" OnCheckedChanged="optBlocked_CheckedChanged" />
                        <asp:RadioButton ID="optUnblocked" runat="server" Text="Show Unblocked Students"
                            CssClass="ClsLabel" GroupName="student" AutoPostBack="true" OnCheckedChanged="optBlocked_CheckedChanged" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width:55px">
                        <span class="ClsLblLgnd" style="font: Bold; width: 55px">Legend :</span>
                    </td>
                    <td style="width:25px">
                        <asp:Label ID="lblPendingFee" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" CssClass="PendingFees" EnableViewState="False" Height="20px"
                            Text=" " Width="20px">
					       <img height="20px" src="../images/spacer.gif" width="20px" />													          
                        </asp:Label>
                    </td>
                    <td style="width:675px">
                        <span class="ClsTextNormal" style="font-weight: bold;">Blocked student progress report
                            due to pending fees</span>
                    </td>
                </tr>
            </table>
		<div style="height:20px" >
			<asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwBlockedUnblockedStudent">
				<Fields>
					<asp:TemplatePagerField>
						<PagerTemplate>
							<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
								CssClass="LblNrmlB" />
							<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" to " />
							<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
								CssClass="LblNrmlB" />
							<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" out of " />
							<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
								CssClass="LblNrmlB" />
							<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="records " />
							<br />
						</PagerTemplate>
					</asp:TemplatePagerField>
				</Fields>
			</asp:DataPager>
		</div>
		<div>
			<div style="width: 750px;">
				<table style="float: left">
					<tr>
						<td align="center">
							<asp:ListView ID="lstvwBlockedUnblockedStudent" runat="server" DataKeyNames="YearwiseStudentId,Reason,RollNo,StudentName,HasFeesPending"
								OnDataBound="lstvwBlockedUnblockedStudent_DataBound" OnPreRender="lstvwBlockedUnblockedStudent_PreRender"
								OnSorting="lstvwBlockedUnblockedStudent_Sorting" OnItemDataBound="lstvwBlockedUnblockedStudent_ItemDataBound">
								<LayoutTemplate>
									<table width="780px" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder"
										id="tblStudent" runat="server">
										<tr id="trHeader" runat="server" class="ClsGridHeader">
											<th align="center">
												<asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckUncheckAllCheckBoxes(this);" />
											</th>
											<th align="left" style="padding-left: 7px; width: 140px;">
												<asp:LinkButton ID="lnkBtnRollNo" runat="server" CommandName="Sort" CommandArgument="RollNo"
													CausesValidation="false" ForeColor="Black">Roll No. </asp:LinkButton>
											</th>
											<th align="left" style="padding-left: 7px; width: 500px;">
												<asp:LinkButton ID="lnkBtnStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
													CausesValidation="false" ForeColor="Black">Student Name </asp:LinkButton>
											</th>
											<th>
												Reason
											</th>
										</tr>
										<tr id="itemPlaceholder" runat="server">
										</tr>
										<tr class="ClsBorderPager" id="trDataPager" runat="server">
											<td colspan="4">
												<asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwBlockedUnblockedStudent"
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
									<tr class='<%# (bool)Eval("HasFeesPending") && (Settings.BlockProgressReportIfFeesArePending) ?  "PendingFees" :  "ClsGridRow" %>'>
										<td align="center" style="width: 80px;">
											<asp:CheckBox ID="chkSelect" runat="server" onclick="EnableDisbleReasontextbox(this);" />
										</td>
										<td align="center" style="width: 100px;">
											<asp:Label ID="lblRollNumber" runat="server" Text='<%# Eval("RollNo") %>' />
										</td>
										<td class="paddingL" style="width: 250px;">
											<asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>' />
										</td>
										<td class="paddingL" style="width: 350px;">
											<asp:TextBox ID="txtReason" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
												Height="40px" Width="300px" Text='<%# Eval("Reason") %>'></asp:TextBox>
										</td>
									</tr>									
								</ItemTemplate>
								<AlternatingItemTemplate>
									<tr class='<%# (bool)Eval("HasFeesPending") && (Settings.BlockProgressReportIfFeesArePending) ?  "PendingFees" :"ClsGridAltRow" %>'>
										<td align="center" style="width: 80px;">
											<asp:CheckBox ID="chkSelect" runat="server" onclick="EnableDisbleReasontextbox(this);" />
										</td>
										<td align="center" style="width: 100px;">
											<asp:Label ID="lblRollNumber" runat="server" Text='<%# Eval("RollNo") %>' />
										</td>
										<td class="paddingL" style="width: 250px;">
											<asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>' />
										</td>
										<td class="paddingL" style="width: 350px;">
											<asp:TextBox ID="txtReason" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
												Height="40px" Width="300px" Text='<%# Eval("Reason") %>'></asp:TextBox>
										</td>
									</tr>
								</AlternatingItemTemplate>
								<EmptyDataTemplate>
									<tr>
										<td class="LblNoRecord" align="center" colspan="4" style="width: 750px; float: left">
											No record found.
										</td>
									</tr>
								</EmptyDataTemplate>
							</asp:ListView>
						</td>
					</tr>
				</table>
			</div>
		</div>
		<div>
			<div style="width: 750px;">
				<div style="">
					<asp:Button ID="btnBlockUnblock" runat="server" Text="Block" CssClass="ClsBtn" OnClick="btnBlockUnblock_Click" disable-page="true"
					    ValidationGroup="Block" />
					<asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="ClsBtn" OnClick="btnUpdate_Click"
					    ValidationGroup="Block" />
				</div>
			</div>
		</div>
		<div>
			<asp:ObjectDataSource TypeName="BusinessLogic.ProgressReportBL" SortParameterName="sortExpression"
				EnablePaging="True" EnableCaching="False" ID="objDSStudentList" runat="server"
				SelectMethod="GetAllBlockedUnBlockedStudents" SelectCountMethod="GetCount" OnObjectDisposing="objDSStudentList_ObjectDisposing"
				OnObjectCreating="objDSStudentList_ObjectCreating">
				<SelectParameters>
					<asp:ControlParameter ControlID="hidStdDivId" Name="aiStdDivId" PropertyName="Value"
						Type="Int32" DefaultValue="0" />
					<asp:ControlParameter ControlID="optBlocked" Name="abShowblocked" PropertyName="Checked"
						Type="Boolean" DefaultValue="false" />
					<asp:ControlParameter ControlID="cmbStudent" Name="aiStudentId" PropertyName="SelectedValue"
						Type="Int32" DefaultValue="0" />
					<asp:ControlParameter ControlID="txtSearch" Name="asSearch" PropertyName="Text" Type="String"
						DefaultValue="" />
					<asp:Parameter Name="sortExpression" Type="String" />
					<asp:Parameter Name="maximumRows" Type="Int32" />
					<asp:Parameter Name="startRowIndex" Type="Int32" />
				</SelectParameters>
			</asp:ObjectDataSource>
			<asp:HiddenField ID="hidSortDirection" runat="server" />
			<asp:HiddenField ID="hidSortExpression" runat="server" />
			<asp:HiddenField ID="hidAlert" runat="server" Value="0" />
			<asp:HiddenField ID="hidTeacherId" runat="server" Value="0" />
			<asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
			<asp:HiddenField ID="HidOptUnblocked" runat="server" Value="true" />
			<asp:HiddenField ID="HidTeacher" runat="server" Value="1" />
			<asp:HiddenField ID="HidStudent" runat="server" Value="1" />
			<asp:HiddenField ID="hidStdDivId" runat="server" />
		</div>
		<div>
			<asp:CustomValidator ID="cstReason" runat="server" Display="None" ClientValidationFunction="CheckLengthOfReason"
				ValidationGroup="Block"></asp:CustomValidator>
			<asp:CustomValidator ID="cstReasonempty" runat="server" Display="None" ClientValidationFunction="CheckEmptyReason"
				ValidationGroup="Block"></asp:CustomValidator>
		</div>
	</div>
	<script type="text/javascript">
		_clientlstvwBlockedUnblockedStudent = "<%=this.lstvwBlockedUnblockedStudent.ClientID %>"
		_clientErrorMessage = "<%=this.lblErrorMesage.ClientID %>"
		_clientcstReason = "<%=this.cstReason.ClientID%>";
		_clientcstReasonempty = "<%=this.cstReasonempty.ClientID%>";
		_clientHidvalue = "<%=this.hidAlert.ClientID%>";
		_clienthidPageNovalue = "<%=this.hidPageNo.ClientID%>";
		_clienthidOptUnblockedvalue = "<%=this.HidOptUnblocked.ClientID%>";
		_clienthidStdDivId = "<%=this.hidStdDivId.ClientID%>";
		_clienthidStudentvalue = "<%=this.HidStudent.ClientID%>";
		

  // This is used checkUncheck All Check boxes
		$("textarea[id*=txtReason]").attr("disabled", "disabled");
		$("input:checkbox[id*=chkSelect]").attr('checked', false);
		function CheckUncheckAllCheckBoxes(chkSelect) {
			$("[id*=valSummary]")[0].innerHTML = '';
			$("input:checkbox[id*=chkSelect]").attr('checked', chkSelect.checked);
			var ReasonAlltextbox = $("textarea[id*=txtReason]");
			if ($('input:radio[id*=optBlocked]')[0].checked) {
				ReasonAlltextbox.removeAttr("disabled");
				if (!chkSelect.checked) {
					ReasonAlltextbox.attr("disabled", "disabled");
				}
			}
			else {
				ReasonAlltextbox.removeAttr("disabled");
				if (!chkSelect.checked) {
					ReasonAlltextbox.val("");
					ReasonAlltextbox.attr("disabled", "disabled");
				}
			}
		}
  //This is used make as per check box check disable unable reason textbox
		function EnableDisbleReasontextbox(chkSelect) {
			var txtReason = $("textarea[id*=" + chkSelect.id.replace('_chkSelect', '_txtReason') + "]")
			if ($('input:radio[id*=optBlocked]')[0].checked) {
				if (!chkSelect.checked)
					txtReason.attr("disabled", "disabled");
				else
					txtReason.removeAttr("disabled");
			}
			else {
				if (!chkSelect.checked) {
					txtReason.val("");
					txtReason.attr("disabled", "disabled");
				}
				else
					txtReason.removeAttr("disabled");
			}
		}
		//this is used to Check atleast one student selected should selected to block ,unblock or update
		var Page_IsValid = true;
		function CheckAtLeastOneStudentIsSelected() {
			Page_IsValid = true;
			if ($("input:checkbox[id$=chkSelect]:checked").length == 0) {
				$("[id*=valSummary]")[0].innerHTML = '';
				alert("At least one student should be selected");
				Page_IsValid = false;
				return false;
			}

			return true;
		}
  //this is used to check reason more than 300 chars
		function CheckLengthOfReason(oSrc, args) {
			var sEmptyReasons = "";
			var sExceededReasons = "";
			$("input:checkbox[id$=chkSelect]:checked").each(
				function () {
					var txtReason = $("textarea[id$=" + this.id.replace('_chkSelect', '_txtReason') + "]")[0];
					if (txtReason.value.length > 300)
						sExceededReasons += (parseInt(txtReason.id.split('_')[3].replace('ctrl', '')) + 1) + ", ";
				}
				);

			sExceededReasons = sExceededReasons.substring(0, sExceededReasons.length - 2);
			if (sExceededReasons != "") {
				Clearlables();
				$get(_clientcstReason).errormessage = "Reason should not be greater than 300 characters for the row no(s) :" + sExceededReasons + ".";
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}
 // this used to check reason text box empty
		function CheckEmptyReason(oSrc, args) {
			var sEmptyReasons = '';
			var sExceededReasons = '';
			$("input:checkbox[id$=chkSelect]:checked").each(
				function () {
					var txtReason = $("textarea[id$=" + this.id.replace('_chkSelect', '_txtReason') + "]")[0];
					if (txtReason.value.trim() == '')
						sEmptyReasons += (parseInt(txtReason.id.split('_')[3].replace('ctrl', '')) + 1) + ", ";
				}
			);
			var sEmptyReasons1 = sEmptyReasons.substring(0, sEmptyReasons.length - 2);
			if (sEmptyReasons != "") {
				Clearlables();
				$get(_clientcstReasonempty).errormessage = "Reason should not be empty for the row no(s) : " + sEmptyReasons1 + ".";
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}
 //this is  used to update hidden field as per alert
		function HidvalueChangeforAlert() {
			$get(_clientHidvalue).value = 1;
		}
//this is used to clear the labels
		function Clearlables() {
			if ($get("<%=this.lblUpdateSucess.ClientID %>") != null)
				$get("<%=this.lblUpdateSucess.ClientID %>").style.display = "none";
		}
 //this is used to if ckeck box is checked and reason is entered and changing dropdown, page change or radiobutton change etc
		function DatalossAlert() {
			if ($get(_clientHidvalue).value == 1) {
				if (window.confirm("Data has been changed, with this action entered reason on current page will get lost. Do you want to continue?")) {
					$get(_clientHidvalue).value = 0;
					return true;
				}
				else {
					if ($("select[id*=cmbTeachers]").length > 0)
						$("select[id*=cmbTeachers]")[0].value = $get(_clienthidStdDivId).value;
					if ($("select[id*=cmbStudent]").length > 0) 
					$("select[id*=cmbStudent]")[0].value = $get(_clienthidStudentvalue).value;
					if ($("select[id*=ddlCnt]").length > 0) 
					$("select[id*=ddlCnt]")[0].value = $get(_clienthidPageNovalue).value;
					if (Boolean.parse($get(_clienthidOptUnblockedvalue).value)) {
						$('input:radio[id*=optBlocked]')[0].checked = false;
						$('input:radio[id*=optUnblocked]')[0].checked = true;
					}
					else {
						$('input:radio[id*=optBlocked]')[0].checked = true;
						$('input:radio[id*=optUnblocked]')[0].checked = false;
					}
					return false;
				}
			}
			else return true;
		}
	</script>     

     <script language="javascript" type="text/javascript">

         $(document).ready(function () {
             AutoSearch();
         });
         function AutoSearch() { 
             var SchoolId = "<%=miSchoolId %>";
             _clienttxtRegNumber = '#<%=txtSearch.ClientID%>';
             _clienttxtcmbTeachers = '<%=cmbTeachers.ClientID%>';
             var AcademicYearId = "<%=miAcademicYearId %>"
             BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, _clienthidStdDivId, 1);
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

</asp:Content>
