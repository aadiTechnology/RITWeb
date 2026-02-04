<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="AdmisionLotteryUI.aspx.cs" Inherits="AdmisionLotteryUI" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <script language="JavaScript" type="text/javascript">

        //This function is used to open division selection popup.
        function OpenDisionSelectionPopup(_IsWaitingListStudent) {

         var x, y, tt_ovr_
         document.getElementById("<%=this.hidIsWaitingList.ClientID %>").value = _IsWaitingListStudent;
         var cssstyle = $get("<%=this.divDivisionConfirmation.ClientID %>").style
         var standardname = document.getElementById("<%=this.lblStandardName.ClientID %>");
         var ddlReport = document.getElementById("<%=ddlStandard.ClientID%>");
         var Text = ddlReport.options[ddlReport.selectedIndex].text;
         standardname.innerHTML = Text;

         var pageWidth = window.screen.width
         var pageHeight = 400
         var left = parseInt((pageWidth / 4.5))
         var top = parseInt((pageHeight / 1.5))
         cssstyle.left = left + "px"
         cssstyle.top = top + "px"
         cssstyle.visibility = "visible"
         cssstyle.display = "block"
     }

     //this function is used hid popup.
     function HidePopup() {

         $get("<%=this.divDivisionConfirmation.ClientID %>").style.visibility = "hidden"
         $get("<%=this.divDivisionConfirmation.ClientID %>").style.display = "none"
         return false;
     }
	</script>
    <div class="MainBodyDiv">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="0" width="90%">
                
                    <tr id="trPrecondition" runat="server" align="center" visible="false">
                        <td>
                            <table align="center" width="90%">
                                <tr>
                                    <td id="tdError" runat="server">
                                        <div runat="server" id="divErr">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trLotttery" runat="server">
                        <td>
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="90%">
                                <tr>
                                    <td style="height: 20px">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" height="10%">
                                                    <div style="float: right; text-align: right; vertical-align: middle">
                                                        <span class="ClsMdtStar" style="width:200px">* Mandatory Fields</span>
                                                    </div>
                                                    <asp:ValidationSummary ID="valSumGenerateErrorMsg" runat="server" CssClass="ClsLabel"
                                                        ValidationGroup="grpGenerateLottery" />
                                                    <asp:ValidationSummary ID="valSumErrorMsg0" runat="server" CssClass="ClsLabel" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg"
                                                                    EnableViewState="false" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td class="ClsBorderLight">
                                                                    <span id="lblStandard" class="ClsLabel">Standard :</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:DropDownList ID="ddlStandard" runat="server" AutoPostBack="True" Width="149px"
                                                                    CausesValidation="false" OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span style="color: red">*
                                                                    <asp:RequiredFieldValidator ID="reqdStandard" runat="server" ControlToValidate="ddlStandard"
                                                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="Standard should be selected."
                                                                        InitialValue="0" ValidationGroup="grpGenerateLottery"></asp:RequiredFieldValidator>
                                                                </span>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnRegenerate" runat="server" Text="Regenerate" CssClass="ClsBtn"
                                                                    BorderWidth="1px" OnClick="btnRegenerate_Click" ValidationGroup="grpGenerateLottery"
                                                                    Visible="False" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="trCheckboxList" runat="server" visible="false">
                                                            <td class="ClsBorderLight">
                                                                    <span id="lblLocationArea" class="ClsLabel">Living Locations :</span>
                                                            </td>
                                                            <td colspan="5">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="ClsBorderLight">
                                                                            <asp:CheckBoxList ID="chklstLivingLocation" runat="server" CssClass="ClsLabel" RepeatColumns="5"
                                                                                RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="chklstLivingLocation_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                            <asp:CustomValidator ID="cstLocations" runat="server" ClientValidationFunction="ValidateResidences"
                                                                                CssClass="ClsMdtStar" Display="None" ErrorMessage="At least one Living Location should be selected."
                                                                                ValidationGroup="grpGenerateLottery"></asp:CustomValidator>
                                                                        </td>
                                                                        <td align="right">
                                                                            <span style="color: red">*
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trResidenceList" runat="server" visible="false">
                                                            <td class="ClsBorderLight">
                                                                    <span id="lblResidentTypes" class="ClsLabel">Residence Types :</span>
                                                            </td>
                                                            <td colspan="6">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="ClsBorderLight">
                                                                          <asp:CheckBoxList ID="chklstResidenceTypes" runat="server" CssClass="ClsLabel" RepeatColumns="5"
                                                                                RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                            <asp:CustomValidator ID="cstResidenceTypes" runat="server" ClientValidationFunction="ValidateLocations"
                                                                                CssClass="ClsMdtStar" Display="None" ErrorMessage="At least one Residence Types should be selected."
                                                                                ValidationGroup="grpGenerateLottery"></asp:CustomValidator>
                                                                        </td>
                                                                        <td align="right">
                                                                            <span style="color: red">*
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                        <tr id="trSiblingFilter" runat="server" visible="false">
                                                            <td class="ClsBorderLight">
                                                                    <span id="Span1" class="ClsLabel">Sibling Filter :</span>
                                                            </td>
                                                            <td colspan="5">
                                                                <asp:DropDownList ID="cmbSiblings" runat="server" CssClass="ExLrgCombo">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trGenerateLottery" visible="false">
                                                            <td class="ClsBorderLight">
                                                                    <span id="lblListCount" class="ClsLabel">Main List Count :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMainListCount" CssClass="SmlTxtBox" runat="server" MaxLength="3"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                                <span id="spnMainCountStar" runat="server" style="color: red">*</span>
                                                                <span style="color: red">
                                                                    <asp:CustomValidator ID="cstMainList" runat="server" ClientValidationFunction="ValidateMainListCount"
                                                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="Main list count should not be  greater than total student."
                                                                        ValidationGroup="grpGenerateLottery"></asp:CustomValidator>
                                                                </span>
                                                            </td>
                                                            <td class="ClsBorderLight">
                                                                    <span id="lblWaitingCount" class="ClsLabel">Waiting List Count :</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtWaitingListCount" CssClass="SmlTxtBox" runat="server" MaxLength="3"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                                <span id="spnWaitingCountStar" runat="server" style="color: red">*</span>
                                                                    <asp:CustomValidator ID="cstWaitingListCnt" runat="server" ClientValidationFunction="ValidateWaitingListCount"
                                                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="Main list count should not be  greater than total student."
                                                                        ValidationGroup="grpGenerateLottery"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cstTotalCnt" runat="server" ClientValidationFunction="ValidateTotalCount"
                                                                        CssClass="ClsMdtStar" Display="None" ErrorMessage="Main list count should not be  greater than total student."
                                                                        ValidationGroup="grpGenerateLottery"></asp:CustomValidator>
                                                                
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="ClsBtn" BorderWidth="1px"
                                                                    OnClick="btnGenerate_Click" ValidationGroup="grpGenerateLottery"  />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                                                                    OnClick="btnCancel_Click" CausesValidation="False" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trShowList" visible="true">
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td align="left" class="ClsBorderlight" colspan="1">
                                                                    <span id="lblSeachName" class="ClsLabel">Name / Form. No. :</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="ClsTxtLarge" Width="298px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" BorderWidth="1px"
                                                                    OnClick="btnShow_Click" />
																<asp:CustomValidator ID="cstStdSelectedValidator"
																					 runat="server"
																					 Display="None"
																					 ClientValidationFunction="ValidateStdSelection" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPublish" runat="server" BorderWidth="1px" CssClass="ClsBtn" 
                                                                    OnClick="btnPublish_Click" Text="Publish Lottery" Width="111px" 
                                                                    ValidationGroup="grpGenerateLottery" Visible="False" />
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="center" style="height: 20px; padding-top: 5px">
                                        <asp:HiddenField ID="hidTotalStudentOfStd" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsConfigured" runat="server" />
                                    </td>
                                </tr>
                               <tr>
				<td >
                <div id="divDivisionConfirmation" runat="server" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 430px; height: 200px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 180px;
                    background-color: white;">
                                <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                    background-repeat: repeat-x; padding: 4px; color: Black; text-align: right;">
                                    <div style="padding: 1px; font-size: 12px; font-weight: bold; color: Black; float: left;">
                                        Division Selection Popup!!!</div>
                                    <span style="cursor: hand" onclick="javascript:HidePopup(false);">
                                        <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                    </span>
                                </div>
                                <div style="padding: 10px; text-align: left;" class="ClsLabel">
                                    <table width="400px">
                                       <tr style="height:20px;">
                                       <td>
                                      
                                       </td>
                                       </tr>
                                     
                                          <tr >
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span2" class="LblNormal">Standard :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                               <asp:Label ID="lblStandardName" runat="server" CssClass="ClsLabel" 
                            EnableViewState="False" ></asp:Label>
                                             
                                            </td>
                                        </tr> 
                                                                             
                                        <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="lblDivision" class="LblNormal">Select Division :</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbStandardNamePopup" runat="server" CssClass="LrgCombo" Width="200"  AutoPostBack="false" CausesValidation="true"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span3" class="LblNormal">Confirmation Type :</span>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdoProvisional" runat="server" Text="Provisional" GroupName="Confirmation" AutoPostBack="false" />
                                                <asp:RadioButton ID="rdoFinal" runat="server" Text="Final" GroupName="Confirmation" />
                                            </td>
                                        </tr>
                                        
                                        <tr style="height:60px;">
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSave" runat="server"  CssClass="ClsBtn" 
                                                    Style="margin-left: 5px; cursor: pointer;" Text="Save" 
                                                    onclick="btnSave_Click"  />
                                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                    OnClientClick="javascript:HidePopup(false);return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
			</td>
			</tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnConsolidatedStudentList" runat="server" CssClass="ClsBtn" Visible="false" Width="200px" Text="Consolidated Student List" onclick="btnConsolidatedStudentList_Click" />
                                    </td>
                                </tr>
                                <tr id="trListviews" runat="server">
                                    <td align="center">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="uPnl"  ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:CheckBox ID="chkSendSMS" runat="server" CssClass="ClsLabel" Text="Send SMS to Confirmed Students"
                                                                Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" class="GridBorder">
                                                                <tr>
                                                                                <td align="center">
																				<label ID="lblMainList" class="ClsGrayMainTitle" style="font-weight:bold;" >Main List</label>                                                                                
                                                                                </td>
                                                                </tr>
                                                                <tr id="trMainListSmsButton" runat="server" visible="false">
                                                                    <td align="left">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left" class="ClsBorderlight">
                                                                                    <asp:CheckBox ID="chkDisplayMainListConfirmed" runat="server" AutoPostBack="true"
                                                                                        CssClass="ClsLabel" Text="Include Confirmed Students" OnCheckedChanged="chkDisplayMainListConfirmed_CheckedChanged" />
                                                                                </td>
                                                                                <td align="right" class="ClsBorderlight">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                    <span id="lblSMS" class="ClsLabel">To send SMS to all the students of Main List, click on</span>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Button ID="btnSendSmsToMainListStudent" runat="server" Text="Send SMS" CssClass="ClsBtn"
                                                                                                    OnClick="btnSendSmsToMainListStudent_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trItemCount" runat="server">
                                                                    <td align="center">
                                                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstvwMainList"
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
                                                                    <td align="center" valign="top">
                                                                        <asp:ListView ID="lstvwMainList" runat="server" OnDataBound="lstvwMainList_DataBound"
                                                                            DataKeyNames="Form_Number" OnSorting="lstvwMainList_Sorting" OnItemDataBound="lstvwMainList_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="center" width="15%">
                                                                                            Confirm
                                                                                        </th>
                                                                                        <th align="left" class="ClspaddingL" width="15%">
                                                                                            <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="Sort" CommandArgument="Form_Number"
                                                                                                ForeColor="Black">Form No.</asp:LinkButton>
                                                                                        </th>
                                                                                        <th align="left" class="ClspaddingL" width="65%">
                                                                                            <asp:LinkButton ID="lnlStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
                                                                                                ForeColor="Black">Student Name</asp:LinkButton>
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                                                        <td colspan="6">
                                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwMainList">
                                                                                                <Fields>
                                                                                                    <asp:TemplatePagerField>
                                                                                                        <PagerTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                                                                                <tr class="ClsGridRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                                            Visible="false" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="trItemWise" runat="server" class="ClsGridAltRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                                            Visible="false" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
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
                                                                <tr>
                                                                    <td align="center" style="height: 20px; padding-top: 5px">
                                                                        <asp:Button ID="btnAdd" runat="server" Text="Confirm" CssClass="ClsBtn" BorderWidth="1px"
                                                                             CausesValidation="False" Visible="false"  />
                                                                        <asp:Button ID="btnPrintMainList" runat="server" Text="Print Main List" CssClass="ClsBtn"
                                                                            Style="width: 110px;" OnClick="btnPrintMainList_Click" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.StudentAdmissionsBL" EnablePaging="true"
                                                                ID="lstvwObjDS" runat="server" SelectMethod="GetLotteryOfAllStudents" SortParameterName="sortExpression"
                                                                SelectCountMethod="GetCountOfLottery" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter Name="aiAcademicYearID" Type="Int16" ControlID="hidNextAcademiYearId"
                                                                        PropertyName="Value" DefaultValue="0" />
                                                                    <asp:ControlParameter Name="aiStandardID" Type="Int16" ControlID="ddlStandard" PropertyName="SelectedValue"
                                                                        DefaultValue="0" />
                                                                    <asp:Parameter Name="cSelectedInLottery" Type="Char" DefaultValue="M" />
                                                                    <asp:ControlParameter ControlID="txtName" PropertyName="Text" Name="asNameFormNo" />
                                                                    <asp:ControlParameter ControlID="chkDisplayMainListConfirmed" PropertyName="Checked"
                                                                        Name="abDisplayConfirmed" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                            <asp:HiddenField ID="hidMainSortDirection" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidMainSortExpression" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidNextAcademiYearId" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidShowGrid" runat="server" Value="N"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidMainListItemCount" runat="server" Value="0"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidWaitingListItemCount" runat="server" Value="0"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidLocationIds" runat="server" Value=""></asp:HiddenField>
                                                            <asp:HiddenField ID="hidResidenceIds" runat="server" Value=""></asp:HiddenField>
                                                            <asp:HiddenField ID="hidSchoolId" runat="Server" ViewStateMode="Enabled" Value="0"/>
                                                            <asp:HiddenField ID="hidPPSNSchoolId" runat="Server" ViewStateMode="Enabled" Value="0"/>
                                                            <asp:HiddenField ID="hidProvisionalConfirmation" runat="Server" Value=""/>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 20px;">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" runat="server" id="Table2" style="color: #333333" cellpadding="0"
                                                               class="GridBorder">
                                                                <tr>
                                                                    <td align="center">
																	<label ID="lblWaitingList" class="ClsGrayMainTitle" style="font-weight:bold;" >Waiting List</label>                                                                       
                                                                    </td>
                                                                </tr>
                                                                <tr id="trWaitingListSmsButton" runat="server" class="" visible="false">
                                                                    <td align="left">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left" class="ClsBorderlight">
                                                                                    <asp:CheckBox ID="chkDisplayWaitingListConfirmed" AutoPostBack="true" runat="server"
                                                                                        CssClass="ClsLabel" Text="Include Confirmed Students"/>
                                                                                </td>
                                                                                <td align="right" class="ClsBorderlight">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                    <span class="ClsLabel">To send SMS to all the students of Waiting List, click on</span>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Button ID="btnSendSmsToWaitingListStudent" runat="server" Text="Send SMS" CssClass="ClsBtn"
                                                                                                    OnClick="btnSendSmsToWaitingListStudent_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr1" runat="server">
                                                                    <td align="center">
                                                                        <asp:DataPager ID="DtWaitingListPgCount" runat="server" PageSize="5" PagedControlID="lstvwWaitingList"
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
                                                                        <asp:ListView ID="lstvwWaitingList" runat="server" OnDataBound="lstvwWaitingList_DataBound"
                                                                            DataKeyNames="Form_Number" OnSorting="lstvwWaitingList_Sorting" OnItemDataBound="lstvwWaitingList_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="center" width="15%">
                                                                                            <asp:Label ID="LinkButton1" runat="server" ForeColor="Black">Confirm</asp:Label>
                                                                                        </th>
                                                                                        <th align="left" class="ClspaddingL" width="15%">
                                                                                            <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="Sort" CommandArgument="Form_Number"
                                                                                                ForeColor="Black">Form No.</asp:LinkButton>
                                                                                        </th>
                                                                                        <th align="left" class="ClspaddingL" width="65%">
                                                                                            <asp:LinkButton ID="lnlStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
                                                                                                ForeColor="Black">Student Name</asp:LinkButton>
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                                                        <td colspan="6">
                                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwWaitingList">
                                                                                                <Fields>
                                                                                                    <asp:TemplatePagerField>
                                                                                                        <PagerTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt1_SelectedIndexChanged">
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
                                                                                <tr class="ClsGridRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                                            Visible="false" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="trItemWise" runat="server" class="ClsGridAltRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                                                        <asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                                                            Visible="false" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
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
                                                                <tr>
                                                                    <td>
                                                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentAdmissionsBL" EnablePaging="true"
                                                                            ID="lstvwWaitingListObj" runat="server" SelectMethod="GetLotteryOfAllStudents"
                                                                            SortParameterName="sortExpression" SelectCountMethod="GetCountOfLottery" EnableCaching="false">
                                                                            <SelectParameters>
                                                                                <asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                                <asp:ControlParameter Name="aiAcademicYearID" Type="Int16" ControlID="hidNextAcademiYearId"
                                                                                    PropertyName="Value" DefaultValue="0" />
                                                                                <asp:ControlParameter Name="aiStandardID" Type="Int16" ControlID="ddlStandard" PropertyName="SelectedValue"
                                                                                    DefaultValue="0" />
                                                                                <asp:Parameter Name="cSelectedInLottery" Type="Char" DefaultValue="W" />
                                                                                <asp:ControlParameter ControlID="txtName" PropertyName="Text" Name="asNameFormNo" />
                                                                                <asp:ControlParameter ControlID="chkDisplayWaitingListConfirmed" PropertyName="Checked"
                                                                                    Name="abDisplayConfirmed" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hidWaitingSortDirection" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidSendPublishSMS" runat="server" Value="false" />
                                                            <asp:HiddenField ID="hidWaitingSortExpression" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidSendMsg" runat="server" />
                                                            <asp:HiddenField ID="hidIsWaitingList" runat="server"  />
                                                            

                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwMainList" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAddWaitingList" EventName="Click" />
                                                <asp:PostBackTrigger ControlID="btnPrintMainList" />
                                                <asp:PostBackTrigger ControlID="btnPrintWaitingList" />
                                                <asp:PostBackTrigger ControlID="btnConsolidatedStudentList" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="height: 20px; padding-top: 5px">                                        
                                        <asp:Button ID="btnAddWaitingList" runat="server" Text="Confirm" CssClass="ClsBtn"
                                            BorderWidth="1px" Visible="false"  CausesValidation="False" 
                                            />
                                        <asp:Button ID="btnPrintWaitingList" runat="server" Text="Print Waiting List" CssClass="ClsBtn"
                                            Style="width: 110px;" Visible="false" OnClick="btnPrintWaitingList_Click" />                                            
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <asp:HiddenField ID="hidRenerateLottery" runat="server" Value="N" />
                    <asp:HiddenField ID="hidShowCountValidation" runat="server" Value="Y"></asp:HiddenField>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>        
    </div>
    <script type="text/javascript" language="javascript">
        _sClientxtMainListCount = "<%=this.txtMainListCount.ClientID %>"
		_clientddlStandard = '<%=this.ddlStandard.ClientID %>'
        _scstMainList = "<%=this.cstMainList.ClientID %>"
        _sClienttxtWaitingListCount = "<%=this.txtWaitingListCount.ClientID %>"
        _sClientcstWaitingListCnt = "<%=this.cstWaitingListCnt.ClientID %>"
        _sClientcstTotalCnt = "<%=this.cstTotalCnt.ClientID %>"
        _sClienthidTotalStudentOfStd = "<%=this.hidTotalStudentOfStd.ClientID %>"
        _clientlstvwMainList = "<%=this.lstvwMainList.ClientID %>"
        _clientlstvwWaitingList = "<%=this.lstvwWaitingList.ClientID %>"
        _clientbtnAdd = "<%=this.btnAdd.ClientID %>"
        _clientbtnAddWaitingList = "<%=this.btnAddWaitingList.ClientID %>"
        _clientchklstLivingLocation = "<%=this.chklstLivingLocation.ClientID %>"
        _clientchklstResidenceTypes = "<%=this.chklstResidenceTypes.ClientID %>"
        _clientchkSendSMS = "<%=this.chkSendSMS.ClientID %>"
        _clientbtnRegenerate = "<%=this.btnRegenerate.ClientID %>"
        _clienthidSendPublishSMS = "<%=this.hidSendPublishSMS.ClientID %>"
        _clienthidSendMsg = "<%=this.hidSendMsg.ClientID %>"
        _clienthidShowCountValidation = "<%=this.hidShowCountValidation.ClientID %>"
        _ClienthidResidenceIds = "<%=this.hidResidenceIds.ClientID %>"
        _clienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
        _clienthidPPSNSchoolId = "<%=this.hidPPSNSchoolId.ClientID %>"

        function ValidateMainListCount(oSrc, args) {
            if ($('#' + _clienthidShowCountValidation).val() == "Y") {
                if (document.getElementById(_sClientxtMainListCount).value == "") {
                    document.getElementById(_scstMainList).errormessage = "Main list count should not be blank."
                    args.IsValid = false
                    return true
                }
                var MainListCount = parseInt(document.getElementById(_sClientxtMainListCount).value)
                var TotalStudentsOfStd = parseInt(document.getElementById(_sClienthidTotalStudentOfStd).value)
                if (MainListCount != 0) {
                    if (MainListCount > TotalStudentsOfStd) {
                        document.getElementById(_scstMainList).errormessage = "Main list count should not be greater than total admissions of selected locations(" + TotalStudentsOfStd + ")."
                        args.IsValid = false
                        return true
                    }
                }
            }
            args.IsValid = true
            return false
        }
        function ValidateWaitingListCount(oSrc, args) {
            if ($('#' + _clienthidShowCountValidation).val() == "Y") {
                if (document.getElementById(_sClienttxtWaitingListCount).value == "") {
                    document.getElementById(_sClientcstWaitingListCnt).errormessage = "Waiting list count should not be blank."
                    args.IsValid = false
                    return true
                }
                var WaitingListCount = parseInt(document.getElementById(_sClienttxtWaitingListCount).value)
                var TotalStudentsOfStd = parseInt(document.getElementById(_sClienthidTotalStudentOfStd).value)
                if (WaitingListCount != 0) {
                    if (WaitingListCount > TotalStudentsOfStd) {
                        document.getElementById(_sClientcstWaitingListCnt).errormessage = "Waiting list count should not be greater than total admissions of selected locations(" + TotalStudentsOfStd + ")."
                        args.IsValid = false
                        return true
                    }
                }
            }
            args.IsValid = true
            return false
        }
        function ValidateTotalCount(oSrc, args) {
        if ($('#' + _clienthidShowCountValidation).val() == "Y") {
                var WaitingListCount = parseInt(document.getElementById(_sClienttxtWaitingListCount).value)
                var MainListCount = parseInt(document.getElementById(_sClientxtMainListCount).value)
                var TotalStudentsOfStd = parseInt(document.getElementById(_sClienthidTotalStudentOfStd).value)

                if (MainListCount == 0 && WaitingListCount == 0) {
                    document.getElementById(_sClientcstTotalCnt).errormessage = "Both Main and Waiting list count should not be zero."
                    args.IsValid = false
                    return true
                }
                else if (MainListCount <= TotalStudentsOfStd && WaitingListCount <= TotalStudentsOfStd) {
                    if ((MainListCount + WaitingListCount) > TotalStudentsOfStd) {
                        document.getElementById(_sClientcstTotalCnt).errormessage = "Addition of main list count and waiting list count should not be greater than total admissions of selected locations(" + TotalStudentsOfStd + ")."
                        args.IsValid = false
                        return true
                    }
                }
            }
            args.IsValid = true
            return false
        }
        function ConfirmationPublish(objBtn) {
            var bResult = true
            if (typeof (Page_ClientValidate) == 'function') {
                bResult = Page_ClientValidate("grpGenerateLottery")
            }
            if (bResult) {
                if (!confirm('Once you publish the lottery, it will be visible to parents. Are you sure you want continue?'))
                    bResult = false
                if (bResult) {
                    var chkSendSMS = document.getElementById(_clienthidSendPublishSMS)
                    if (chkSendSMS.checked == false) {
                        if (confirm("Do you want to send the following SMS to the students selected in the admission lottery?"))
                            chkSendSMS.value = "true"
                    } 
                } 
            }
            return bResult
        }
        function ShowConfirmation(objBtn, str) {            
            var bResult = true
            var listview
            var listindentifier = document.getElementById("<%=this.hidIsWaitingList.ClientID %>").value;
            var listviewName = ''
            if (listindentifier == '0') {
                listview = _clientlstvwMainList
                listviewName = 'main list'
            }
            else {
                listview = _clientlstvwWaitingList
                listviewName = 'waiting list'
            }
            if (CheckSelectionOfStudent(listview, '_chkSelect')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                if (bResult) {
                    if (!confirm('Are you sure you want to confirm admissions?')) {
                        bResult = false
                    }





                    if (bResult) {
                        var admissionProvisionalMsg = document.getElementById("<%=this.hidProvisionalConfirmation.ClientID %>").value;
                        var rdoProvisional = document.getElementById("<%=this.rdoProvisional.ClientID %>").checked;
                        var rdoFinal = document.getElementById("<%=this.rdoFinal.ClientID %>").checked;
                        var Confirmationmsg;

                        if (rdoFinal == true)
                            Confirmationmsg = str;
                        else if (rdoProvisional == true)
                            Confirmationmsg = admissionProvisionalMsg;

                        
                        var chkSendSMS = document.getElementById(_clientchkSendSMS)
                        var SendMsg = document.getElementById(_clienthidSendMsg)
                        if (confirm('Do you want to send the following SMS to student?\nMessage: ' + Confirmationmsg)) {
                            chkSendSMS.checked = true
                            SendMsg.value = "Y";
                        }
                        else {
                            SendMsg.value = "N";
                        }
                        
                    } 
                } 
            }
            else {
                alert("At least one student of " + listviewName + " should be selected.")
                bResult = false
            }
            return bResult
        }
        function CheckSelectionOfStudent(listview, ItemName) {
            var chk
            var isSelected = false
            var iRowCount = 0
            var itemCount
            if (listview == _clientlstvwMainList)
                itemCount = $get("<%=this.hidMainListItemCount.ClientID %>").value
            else
                itemCount = $get("<%=this.hidWaitingListItemCount.ClientID %>").value
            while (iRowCount < itemCount) {
                if (iRowCount < 10)
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
                else
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
                if (chk != null && chk.checked)
                    isSelected = true
                iRowCount = iRowCount + 1
                chk = document.getElementById(listview + "_ctrl" + iRowCount + ItemName)
            }
            return isSelected
        }
        function ValidateLocations(oSrc, args) {
            var j = 0
            var checks = document.forms[0].elements
            var boxLength = checks.length
            for (i = 0; i < boxLength; i++) {
                if (checks[i].type == 'checkbox') {
                    if (checks[i].checked == true) {
                        j++
                    } 
                } 
            }
            if (j > 0) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
        }
        function ValidateResidences(oSrc, args) {
            var j = 0
            var checks = document.forms[0].elements
            var boxLength = checks.length
            for (i = 0; i < boxLength; i++) {
                if (checks[i].type == 'checkbox') {
                    if (checks[i].checked == true) {
                        j++
                    }
                }
            }
            if (j > 0) {
                args.IsValid = true
                return false
            }
            else {
                args.IsValid = false
                return true
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_beginRequest(BeginReqHandler)
        prm.add_endRequest(EndReqHandler)
        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnAdd)
                DisableButtons(true)
        }
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientbtnAdd)
                DisableButtons(false)
        }
        function DisableButtons(action) {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function' && action)
                isPageValid = Page_ClientValidate()
            if (isPageValid) {
                if (document.getElementById(_clientbtnAdd) != null)
                    document.getElementById(_clientbtnAdd).disabled = action
                if (document.getElementById(_clientbtnAddWaitingList) != null)
                    document.getElementById(_clientbtnAddWaitingList).disabled = action
            } 
        }
        function ConfirmSMSSending(str) {
            if (document.getElementById(_clientbtnRegenerate) != null) {
                if (!confirm("Once SMS is sent, you can't regenerate lottery. Are you sure, you want to continue?"))
                    return false
            }
            var SendMsg = document.getElementById(_clienthidSendMsg)
            if (!window.confirm('Do you want to send the following SMS to students selected in the admission lottery? \nMessage: ' + str)) {
                SendMsg.value = "N";
            }
            else {
                SendMsg.value = "Y";
            }
            return true
        }

        function ValidateStdSelection(src, args) {
			args.IsValid = true;
			var ddlStandard = $get(_clientddlStandard);
			if(ddlStandard && ddlStandard.value == '0')
			{
				args.IsValid = false;
				src.errormessage = 'Standard should be selected.';
			}
			return !args.IsValid;
		}
		
    </script>
    <script language="javascript" type="text/javascript">

        _cltdivAttendanceAlert = "<%=this.divDivisionConfirmation.ClientID %>"
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
