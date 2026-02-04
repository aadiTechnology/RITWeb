<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentSearchUI.aspx.cs" Inherits="StudentSearchUI" %>
      <%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" width="100%">
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:RadioButton ID="optSearch" runat="server" CssClass="ClsLabel" Text="Search"
                                    AutoPostBack="true" GroupName="Type" OnCheckedChanged="optSearch_CheckedChanged" />
                                <asp:RadioButton ID="optExport" runat="server" CssClass="ClsLabel" Text="Export"
                                    AutoPostBack="true" GroupName="Type" OnCheckedChanged="optExport_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="50%">
                        <tr id="tdSearch" runat="server">
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <asp:ValidationSummary ID="valSumStudent" runat="server" ValidationGroup="Student"
                                                        CssClass="ClsMdtStar" />
                                                    <asp:ValidationSummary ID="valSumReceipt" runat="server" ValidationGroup="ReceiptNo"
                                                        CssClass="ClsMdtStar" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Reg. No. - Name / Reg. No should not be blank."
                                                        Display="None" ControlToValidate="txtName" ValidationGroup="Student"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Receipt No. should not be blank."
                                                        Display="None" ControlToValidate="txtReceiptNumber" ValidationGroup="ReceiptNo"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Text="" CssClass="ClsMdtStar"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="180px">
                                                    <span class="ClsLabel">Reg. No. - Name / Reg. No. :</span>
                                                </td>
                                                <td align="left" width="300px">
                                                    <asp:TextBox ID="txtName" Width="290px" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnSearchStudent" runat="server" Text="Search" CssClass="ClsBtn"
                                                        OnClick="btnSearchStudent_Click" UseSubmitBehavior="false" ValidationGroup="Student" />
                                                    <asp:HiddenField ID="hidQueryString" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">
                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;" colspan="2">
                                                    <asp:Label ID="Label4" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="To search student please enter either only registration number or select record from auto search facility."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="height20">
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr style="color: #C0C0C0" />
                                                </td>
                                            </tr>
                                            <tr class="height20">
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="lblReceiptMessage" runat="server" EnableViewState="false" CssClass="ClsMdtStar"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" width="180px">
                                                    <span class="ClsLabel">Receipt No.:</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="ClsBtn" OnClick="btnPrint_Click"
                                                        UseSubmitBehavior="false" ValidationGroup="ReceiptNo" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:HiddenField ID="hidSearchMode" runat="server" Value="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="tdExport" runat="server">
                            <td align="center">
                                <table width="80%">
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <span class="ClsLabel" style="float: inherit; font-weight: bold; font-size: large;">
                                                            Fee Payment Details</span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td colspan="2" align="center">
                                                        <hr />
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                        <span class="ClsLabel">Standard : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbStandard" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <span class="ClsLabel">Division : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbDivision" runat="server" CssClass="LrgCombo">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                 <tr id="trDatefilter1" runat="server">
															<td class="ClsBorderLight " align="left">
																<span class="ClsLabel"> Start Date:</span>
															</td>
															<td >
																<asp:TextBox ID="txtStartDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="4" ReadOnly="true"></asp:TextBox>
																<rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="enabled"  Control="txtStartDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />																
															</td>															
                                                          
														</tr>
                                                <tr id="trDatefilter2" runat="server">
                                                <td class="ClsBorderLight" align="left">
																<span class="ClsLabel"> End Date :</span>
															</td>
															<td >
																<asp:TextBox ID="txtEndDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="5" ReadOnly="true"></asp:TextBox>
																<rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="enabled" Control="txtEndDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid to date." />																
															</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnExport_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="height20" id="trBreak" runat="server">
                                        <td align="center">
                                            <hr style="border-style: solid; border-width: thin" />
                                        </td>
                                    </tr>
                                    <tr id="trVoucher" runat="server" visible="false">
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <span class="ClsLabel" style="float: inherit; font-weight: bold; font-size: large;">
                                                            Fee Voucher Xml</span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td colspan="2" align="center">
                                                        <hr />
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" style="width: 150px;">
                                                        <span class="ClsLabel">Standard : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbExStandard" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cmbExStandard_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <span class="ClsLabel">Division : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbExDivision" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="cmbExDivision_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbExStandard" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight">
                                                        <span class="ClsLabel">Student : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbexStudent" runat="server" CssClass="LrgCombo">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbExStandard" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbExDivision" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnExportLedgers" runat="server" Text="Export Ledgers" CssClass="ClsBtn"
                                                            Width="150px" onclick="btnExportLedgers_Click" />
                                                        <asp:Button ID="btnExVouchers" runat="server" Text="Export Vouchers" CssClass="ClsBtn"
                                                            Width="150px" onclick="btnExVouchers_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>
                        </tr>                       
                        <tr id="trFeeSummary" runat="server" visible="false">
                           <td align="center">
                               <table>
                                   <tr class="height20" style="width:100%;">
                                       <td align="center" colspan="2">
                                           <hr style="border-style: solid; border-width: thin" />
                                       </td>
                                   </tr>
                                   <tr>
                                       <td colspan="2" align="center">
                                          <span class="ClsLabel" style="float: inherit; font-weight: bold; font-size: large;">
                                                Fee Summary</span>
                                       </td>
                                  </tr>                                                
                                  <tr>
                                      <td align="left" class="ClsBorderlight" style="width: 170px;">
                                          <span class="ClsLabel">Standard : </span>
                                      </td>                                      
                                      <td align="left" width="90%">
                                          <table width="100%">
                                            <tr>
                                                <td align="right" class="ClsBorderlight" style="width: 80px;" >
                                                    <asp:CheckBox ID="chkAll" runat="server" Text= "Select All" onclick="CheckAll1(this);"/> 
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="chkStandards" runat="server" CellPadding="0" CellSpacing="0"
                                                      CssClass="ClsBorderLight" RepeatColumns="4" RepeatDirection="Horizontal" Width="100%">
                                                    </asp:CheckBoxList>  
                                                </td>
                                            </tr>
                                          </table>                                                   
                                      </td>
                                  </tr>
                                  <tr>
                                      <td align="left" class="ClsBorderlight">
                                          <span class="ClsLabel">Fee Type : </span>
                                      </td>
                                      <td align="left">
                                        <asp:DropDownList ID="cmbFeeTypes" runat="server" CssClass="LrgCombo" AutoPostBack="true">
                                        </asp:DropDownList>                                                                                                               
                                      </td>
                                  </tr>                                                
                                  <tr>
                                      <td colspan="2" align="center">
                                          <asp:Button ID="btnFeeExport" runat="server" Text="Export Fee" CssClass="ClsBtn"
                                              Width="150px" onclick="btnFeeExport_Click"/>                                                        
                                      </td>
                                  </tr>
                              </table>
                           </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientbtnSearchStudent = "<%=this.btnSearchStudent.ClientID %>"
        _clientbtnPrint = "<%=this.btnPrint.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)

        function EndReqHandler(sender, args) {

            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement.id == _clientbtnSearchStudent || postBackElement.id == _clientbtnPrint) {
                var queryString = $get("<%=this.hidQueryString.ClientID %>").value
                var searchMode = $get("<%=this.hidSearchMode.ClientID %>").value

                if (queryString != "") {
                    if (searchMode == "StudentSearch")
                        window.open('StudentUI.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus();
                    else if (searchMode == "ReceiptSearch")
                        window.open('../Accountant/FeesMiniReceipt.aspx?' + queryString, '_new', 'left=0, top=0, height=650, width=850, status=no, resizable= no, scrollbars= yes').focus();

                }
                AutoSearch();
            }

        }

        function ClearMessages() {
            $get('<%=this.lblMessage.ClientID %>').innerHTML = "";
            $get('<%=this.lblReceiptMessage.ClientID %>').innerHTML = "";
        }

        function CheckAll1(Src) {         
            var chk = document.getElementById('<%= chkStandards.ClientID %>');
            var inputs = chk.getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox")
                        inputs[j].checked = Src.checked;
                }
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });
        function AutoSearch() {
            _clienttxtRegNumber = '#<%=txtName.ClientID%>';
            BindAutoCompleteEvent("<%=miSchoolId %>", "<%=miAcademicYearId %>", _clienttxtRegNumber, null, null, null, 0);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearchStudent.ClientID %>");
            SearchResult(txt, val, bt);
        }
               
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
