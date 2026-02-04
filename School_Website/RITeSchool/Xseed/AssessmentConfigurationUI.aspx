<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="AssessmentConfigurationUI.aspx.cs" Inherits="AssessmentConfigurationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td style="width: 77%">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                  <span class="ClsMdtStar">* </span>
                                   <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCheckdepandency" runat="server" Width="96%">
                                        <asp:Label ID="lblCheckDependency" Visible="true" Style="text-align: left" runat="server"
                                            ForeColor="Red" Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                        EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="Table1" runat="server" border="0" cellpadding="1" cellspacing="2" style="margin-left: 19px;">
                                        <tr>
                                            <td align="left" style="width: 41%" class="ClsBorderLight">
                                         <asp:Label CssClass = "ClsLabel" ID="lblAssessment" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Assessment%>"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td style="width: 59%">
                                                <asp:TextBox ID="txtAssessment" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                                    Width="180px"></asp:TextBox><span class="ClsMdtStar">&nbsp;&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqAssessment" runat="server" ControlToValidate="txtAssessment"
                                                    Display="None" ValidationGroup="Save" ErrorMessage= "<%$ Resources:LocalizedResources, ValAssessmentName%>"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 47%" class="ClsBorderLight">
                                          <asp:Label CssClass = "ClsLabel" ID="lblStartDate" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StartDate%>"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td style="width: 53%">
                                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" CssClass="SmlTxtBox"
                                                    AutoPostBack="false" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                <rjs:PopCalendar ID="calStartDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" 
                                                    InvalidDateMessage= "<%$ Resources:LocalizedResources, ValStartDateBlank%>" 
                                                    Culture="en" />
                                                <span class="ClsMdtStar">&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="txtStartDate"
                                                    Display="None" ValidationGroup="Save" ErrorMessage= "<%$ Resources:LocalizedResources, ValStartDateBlank%>" ></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 47%" class="ClsBorderLight">
                                         <asp:Label CssClass = "ClsLabel" ID="lblEndDate" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, EndDate%>"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td style="width: 53%">
                                                <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" CssClass="SmlTxtBox" AutoPostBack="false"
                                                    Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" 
                                                    InvalidDateMessage= "<%$ Resources:LocalizedResources, ValEndDateBlank%>" 
                                                    Culture="en" />
                                                <span class="ClsMdtStar">&nbsp;*</span>
                                                <asp:RequiredFieldValidator ID="reqEndDate" runat="server" ControlToValidate="txtEndDate"
                                                    Display="None" ValidationGroup="Save" ErrorMessage= "<%$ Resources:LocalizedResources, ValEndDateBlank%>" ></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cstDate" Display="None" runat="server" ValidationGroup="Save"
                                                    CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
                                                    ClientValidationFunction="DateValidations"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnSave" Text= "<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center" width="60%">
                            <tr>
                                <td>
                                    <div id="divAssessmentDetails" class="GridBorder" runat="server" style="overflow: auto;
                                        height: 400px;">
                                        <asp:ListView ID="lstvwAssessmentDetails" runat="server" DataKeyNames="AssessmentId,Name"
                                            OnItemCommand="lstvwAssessmentDetails_ItemCommand" OnItemDataBound="lstvwAssessmentDetails_ItemDataBound"
                                            OnSorting="lstvwAssessmentDetails_Sorting" 
                                            ondatabound="lstvwAssessmentDetails_DataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" width="40%" style="padding-left:7px;">
                                                            <asp:LinkButton ID="lnkBtnAssessmentName" runat="server" CommandName="Sort" CommandArgument="Name" Text = "<%$ Resources:LocalizedResources, AssessmentName%>"
                                                                CausesValidation="false" ForeColor="Black">Assessment Name </asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="20%" style="padding-left: 4px;">
                                                            <asp:LinkButton ID="lnkBtnStartDate" runat="server" CommandName="Sort" CommandArgument="StartDate" Text = "<%$ Resources:LocalizedResources, StartDate%>"
                                                                CausesValidation="false" ForeColor="Black">Start Date</asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="20%" style="padding-left: 5px;">
                                                            <asp:LinkButton ID="lnkBtnEndDate" runat="server" CommandName="Sort" CommandArgument="EndDate" Text = "<%$ Resources:LocalizedResources, EndDate%>"
                                                                CausesValidation="false" ForeColor="Black">End Date</asp:LinkButton>
                                                        </th>
                                                        <th align="center" width="10%">
                                                         <asp:Label ID="lblEdit" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                        </th>
                                                        <th align="center" width="10%">
                                                            <asp:Label ID="lblDelete" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Delete %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblAssessmentName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" >
                                                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateAssessment"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveAssessment"
                                                            ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblAssessmentName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateAssessment"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveAssessment" CausesValidation="false"
                                                            runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, ValDublicateAssessment%>" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidMode" runat="server" />
                                    <asp:HiddenField ID="hidAssesmentId" runat="server" />
                                    <asp:HiddenField ID="hidAssesmentName" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidScreenWidth" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidAcademicYearStartDate" runat="server" />
                                    <asp:HiddenField ID="hidAcademicYearEndDate" runat="server" />
                                    <asp:HiddenField ID = "hidAlertForAssessment" runat = "server" />
                                    <asp:HiddenField ID = "hidvalForEndDateGreater" runat = "server" />
                                    <asp:HiddenField ID ="hidValForDuplicateAssessment" runat = "server" />
                                    <asp:HiddenField ID = "hidValStartDateEndDate" runat = "server" />
                                    <asp:HiddenField ID = "hidAnd" runat = "server" />
                                    <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text= "<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" BorderWidth="1px"
                                CausesValidation="False" UseSubmitBehavior="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientlbl_CheckDependency = "<%=this.lblCheckDependency.ClientID %>"
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbl_ErrorMessage = "<%=this.lblErrorMsg.ClientID %>"
        _clientcstDate = "<%=this.cstDate.ClientID %>"
        _clienthidAcademicYearStartDate = "<%=this.hidAcademicYearStartDate.ClientID %>"
        _clienthidAcademicYearEndDatet = "<%=this.hidAcademicYearEndDate.ClientID %>"
        _clienttxtStartDate = "<%=this.txtStartDate.ClientID %>"
        _clienttxtAssessment = "<%=this.txtAssessment.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"
        _clientlstvwAssessmentDetailsId = "<%=this.lstvwAssessmentDetails.ClientID %>"
        _clientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=this.hidAlertForAssessment.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }

        function btnsaveonclick(varname) {
            var lbl = document.getElementById(_clientlbl_CheckDependency);
            lbl.innerHTML = "";
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            lbl1.innerHTML = "";
            var lbl1 = document.getElementById(_clientlbl_ErrorMessage);
            lbl1.innerHTML = "";
        }

        function DateValidations(oSrc, args) {
            var Startdate
            var EndDate
            var txtStartdate
            var txtEndDate
            var dtStartDate = document.getElementById(_clienthidAcademicYearStartDate).value.trim()
            var dtEndDate = document.getElementById(_clienthidAcademicYearEndDatet).value.trim()

            var dttxtStartDate = document.getElementById(_clienttxtStartDate).value;
            var dttxtEndDate = document.getElementById(_clienttxtEndDate).value;
            if (dtStartDate != null && dtStartDate != "" && dtEndDate != null && dtEndDate != ""
                    && dttxtStartDate != null && dttxtStartDate != "" && dttxtEndDate != null && dttxtEndDate != "") {
                if (document.all) {
                    EndDate = new Date(dtEndDate.replace('-', ' '));
                    Startdate = new Date(dtStartDate.replace('-', ' '));
                    txtEndDate = new Date(dttxtEndDate.replace('-', ' '));
                    txtStartdate = new Date(dttxtStartDate.replace('-', ' '));
                }
                else {
                    EndDate = new Date(convertdate(dtEndDate));
                    Startdate = new Date(convertdate(dtStartDate));
                    txtEndDate = new Date(convertdate(dttxtEndDate));
                    txtStartdate = new Date(convertdate(dttxtStartDate));
                }
            }
            if (txtStartdate > txtEndDate) {
                oSrc.errormessage = document.getElementById("<%=this.hidvalForEndDateGreater.ClientID %>").value;
                document.getElementById(_clientcstDate).innerText = document.getElementById("<%=this.hidvalForEndDateGreater.ClientID %>").value;
                args.IsValid = false
                return true
            }
            if (Startdate > txtStartdate || EndDate < txtStartdate
                        || Startdate > txtEndDate || EndDate < txtEndDate) {
                oSrc.errormessage = document.getElementById("<%=this.hidValStartDateEndDate.ClientID %>").value.replace("%startdate%", dtStartDate).replace("%enddate%", dtEndDate) + ".";
                document.getElementById(_clientcstDate).innerText = document.getElementById("<%=this.hidValStartDateEndDate.ClientID %>").value.replace("%startdate%", dtStartDate).replace("%enddate%", dtEndDate) + ".";
                args.IsValid = false
                return true
            }
        }

        function DuplicateValue(oSrc, args) {
            var lblLearningOutcome = "";
            var sRowNo = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtAssessment = document.getElementById(_clienttxtAssessment).value
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblAssessment = document.getElementById(_clientlstvwAssessmentDetailsId + "_ctrl" + iRowNumber + "_lblAssessmentName").innerHTML;
                if ((txtAssessment.trim()).toLowerCase() == lblAssessment.toLowerCase() && iRowNumber != (iRowNo - 1)) {
                    sRowNo += (iRowNumber + 1) + ", ";
                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = document.getElementById("<%=this.hidValForDuplicateAssessment.ClientID %>").value + sRowNo + ".";
                document.getElementById(_clientcstvalDuplicateValue).innerText = document.getElementById("<%=this.hidValForDuplicateAssessment.ClientID %>").value + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function SetWidth() {
            if (document.getElementById('hidScreenWidth') != null)
                $get('hidScreenWidth').value = "" + window.screen.width
        }
        SetWidth()

    </script>

</asp:Content>
