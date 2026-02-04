<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="TransportLateFeeSettingUI.aspx.cs" Inherits="TransportLateFeeSettingUI" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>
<script runat="server">

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" cellpadding="2" cellspacing="2" width="97%" style="margin-top: 10px">
		<tr align="center">
			<td align="left">
				<table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
					<tr>
						<td align="left">
							<!--lblError label insert here-->
							<asp:ValidationSummary ID="valsumLateFee" runat="server" CssClass="NewClsLabel" ShowSummary="true"
								 />
							
						</td>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                        <span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                    </td>
					</tr>
					<tr>
						<td align="center" style="padding-left:300px;">
							<asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"
								Style="width: 100%;" />
							<asp:Label ID="lblSuccessMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
								ForeColor="Blue" Font-Bold="true" Visible="false" Style="width: 100%;" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		        <tr>
                  <td  align="center">
                     <table style="width:auto;" >
                        <tr>
                          <td align="center" class="ClsHilightBGB" >
                           <asp:Label ID="lblServiceDuration" runat="server" BorderWidth="0px" CssClass="LblNrmlB" Font-Bold="True"
                               EnableViewState="false"></asp:Label>&nbsp; &nbsp;
                          </td>
                        </tr>
                    </table>
                  </td>
                 </tr>
				<tr>
                 <td align="center">
                   <table id="tblControls" runat="server">
                                
                                 <tr>
                                    <td class="ClsBorderlight paddingL" style="width: 200px;" runat="server" id="tdSubject">
										 <asp:Label ID="lblVlueFrType" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, ValueForType%>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;" runat="server"  id="tdSubjectCmb">
										<asp:TextBox runat="server" ID="txtValueForType"  CssClass="LrgTxtBox" MaxLength="2" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                          <span class="ClsMdtStar">*</span>
                                          <asp:RequiredFieldValidator ID="reqValueType" runat="server" ControlToValidate="txtValueForType"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValueForTypeSelect%>"></asp:RequiredFieldValidator>
                                    </td>
								</tr>
								<tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;">
									     <asp:Label ID="lblltfeetype" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, LateFeeType %>"
                                         EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;">
										 <asp:DropDownList ID="cmbFeeType" CssClass="LrgCombo" runat="server">
                                             <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                             <asp:ListItem Text="Day(s)" Value="1" ></asp:ListItem>
                                             <asp:ListItem Text="Month(s)" Value="2" ></asp:ListItem>
                                         </asp:DropDownList>
                                         <span class="ClsMdtStar">*</span>
                                         <asp:CustomValidator ID="cstValHouseColor" runat="server" ClientValidationFunction="ValidateLateFeeType"
                                          SetFocusOnError="True" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, LateFeeTypeSelect %>"></asp:CustomValidator>

									</td>
								</tr>
                                   <tr>
                                    <td class="ClsBorderlight paddingL" style="width: 200px;" runat="server" id="td1">
										 <asp:Label ID="lblamount" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, AmountRs %>"
                                          EnableViewState="false"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
									</td>
									<td class="ClsBorderlight paddingL" style="width: 250px;" runat="server"  id="td2">
										<asp:TextBox runat="server" ID="txtAmount"  CssClass="LrgTxtBox" MaxLength="9" onblur="extractNumber(this,1,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                        onkeyup="extractNumber(this,1,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:RequiredFieldValidator ID="reqTextAmount" runat="server" ControlToValidate="txtAmount"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AmountSelect%>"></asp:RequiredFieldValidator>
                                    </td>
								</tr>
                   </table>
			      </td>
                </tr>		
                <tr>
                  <td colspan="4" align="center">
                         <asp:ListView ID="lstvwltfee" runat="server" 
                                    DataKeyNames="Id">
                                    <LayoutTemplate>
                                        <table width="20%" runat="server" id="tblContacts"  style="color: #333333" cellpadding="0"
                                            cellspacing="1" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader" style="height:20px;">
                                                <th align="left"  class="paddingL" width="6px" style="white-space:nowrap;">
                                                    <asp:Label ID="lblMonth" runat="server" Text="<%$ Resources:LocalizedResources, Month %>"
                                                              EnableViewState="false"></asp:Label> 
                                                </th>
                                               <th align="left" class="paddingL" width="77%" >
                                                     <asp:Label ID="lblDueDate" runat="server" Text="<%$ Resources:LocalizedResources, DueDate %>"
                                                              EnableViewState="false"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate >
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td id="tdSubject" runat="server" align="left">
                                                <asp:Label ID="lblSubjects" runat="server" Text='<%#Eval("Month")%>' CssClass="ClspaddingL"> </asp:Label>
                                            </td>
                                            <td  align="left" style="padding-left:16px;white-space:nowrap;">
                                               <asp:TextBox ID="txtDueDate" CssClass="SmlCombo" runat="server" MaxLength="15" 
                                                    Text='<%#Convert.ToDateTime(Eval("DueDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtDueDate" Format="dd MMM yyyy"
												Culture="en" Visible="True" ShowWeekend="true" ShowErrorMessage="false" />
											    <rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer3" runat="server"
												Calendar="PopCalendar3" Visible="false" />                                          
                                           </td>
                                         </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                             <td id="tdSubject" runat="server" align="left">
                                                <asp:Label ID="lblSubjects" runat="server" Text='<%#Eval("Month")%>' CssClass="ClspaddingL"> </asp:Label>
                                            </td>
                                            <td  align="left"  style="padding-left:16px;white-space:nowrap;">
                                               <asp:TextBox ID="txtDueDate" CssClass="SmlCombo" runat="server" MaxLength="15" 
                                                    Text='<%#Convert.ToDateTime(Eval("DueDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'></asp:TextBox>
                                               <rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtDueDate" Format="dd MMM yyyy"
												Culture="en" Visible="True" ShowWeekend="true" ShowErrorMessage="false" />
											   <rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer3" runat="server"
												Calendar="PopCalendar3" Visible="false" />                                            
                                           </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table>
                                            <tr>
                                                <td align="center" class="LblNoRecord" style="width: 500px">
                                                    No Record found.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                  </td>
                  </tr>
                  <tr>
                      <td>
                         <asp:CustomValidator ID="cstvalDueDateBlank" runat="server" ClientValidationFunction="ValidateDueDate"
                            SetFocusOnError="True" Display="None" ErrorMessage="Due Date should be selected."></asp:CustomValidator>
                         <asp:HiddenField ID="hidDueDateShouldNotBlank" runat="server"  />
                         <asp:HiddenField ID="hidCultureInfo" runat="server" Value="0" />
                         <asp:HiddenField ID="hidServiceStartDate" runat="server"/>
                         <asp:HiddenField ID="hidServiceEndDate" runat="server" />
                         <asp:HiddenField ID="hidAcademicStartDate" runat="server"/>
                         <asp:HiddenField ID="hidAcademicEndDate" runat="server" />
                      </td>
                  </tr>
                  <tr align="center">
					  <td >
                         <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true"
                             CausesValidation="true" UseSubmitBehavior="false" />
                         <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" 
                             CausesValidation="False" onclick="btnCancel_Click"
                                    />
					  </td>
                  </tr>
           
      </table>
  <script type="text/javascript">
      _clientcmbFeeType = "<%=this.cmbFeeType.ClientID %>";
      _clientlstvwltfee = "<%=this.lstvwltfee.ClientID %>"
      _clientcstvalDueDateBlank = "<%=this.cstvalDueDateBlank.ClientID %>";

      function ValidateDueDate(oSrc, args) {
         var sMessage = false
         var sMonths = ''
         var iRowNo=0
         var iListVwRows = document.getElementById(_clientlstvwltfee + "_tblContacts").rows.length-1;
         while (iRowNo<iListVwRows) {
             txtDueDate = document.getElementById(_clientlstvwltfee + "_ctrl" + iRowNo + "_txtDueDate")
              if (txtDueDate.value.trim() == "") {
                  sMonths = sMonths + document.getElementById(_clientlstvwltfee + "_ctrl" + iRowNo + "_lblSubjects").innerHTML + "," 
                  sMessage = true
              }
              iRowNo = iRowNo+1
            }
         if (sMessage == true) {
             oSrc.errormessage = document.getElementById("<%=hidDueDateShouldNotBlank.ClientID%>").value + " " + sMonths.slice(0, -1) + " month(s)."
             args.IsValid = false
             return true
          }
          args.IsValid = true
          return false
      }

      function ValidateLateFeeType(oSrc, args) {
          var feetype = $get(_clientcmbFeeType).value;
          if (feetype == "0") {
              args.IsValid = false;
              return true;
          }
          args.IsValid = true;
          return false;
      }
  </script>
</asp:Content>

