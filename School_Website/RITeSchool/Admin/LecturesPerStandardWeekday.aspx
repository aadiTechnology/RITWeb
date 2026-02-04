<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LecturesPerStandardWeekday.aspx.cs"
    MasterPageFile="../MasterPages/MasterPage.master" Inherits="LecturesPerStandardWeekday" %>

<%@ OutputCache VaryByParam="none" Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="center">
                    <asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" Text="" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <span>
                        <table cellpadding="1" cellspacing="2" id="tblLegend" runat="server">
                            <tr>
                                <td align="left" colspan="1">                                    
                                    <span class="ClsLblLgnd" style="Font-Weight:bold"><asp:Label ID="Label" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label></span>
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="TextBox1" runat="server" BorderStyle="Solid" BorderWidth="1px" BackColor="#FFFFC4"
                                        Height="20px" ReadOnly="True" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                </td>
                                <td align="left" colspan="1">
                                    <span class="ClsTextNormal" style="Font-Weight:bold">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, NoOfLectures %>"></asp:Label>
                                    </span>
                                </td>
                                <td align="right">
                                </td>
                                <td align="right">
                                </td>
                                <td align="right">
                                    <asp:Label ID="TextBox3" runat="server" BorderStyle="Solid" BorderWidth="1px" BackColor="#FFCCCC"
                                        Height="20px" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                </td>
                                <td align="right">
                                    <span class="ClsTextNormal" style="Font-Weight:bold">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, MaximumLecturesAllowedTeacher %>"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divGridView" runat="server" class="GridBorder">
                        <asp:GridView ID="grdStandardWeekDay" runat="server" AutoGenerateColumns="False" 
                            Height="43px"  Width = "100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                            GridLines="None">
                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                            <Columns>
                                <asp:BoundField DataField="Standard_Id" HeaderText="<%$ Resources:LocalizedResources, StandardID %>" SortExpression="Standard_Id">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Standard_Name" SortExpression="Standard_Name" HeaderImageUrl="~/RITeSchool/images/GridHeader_StdWeekday.gif">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                            </Columns>
                            <RowStyle CssClass="ClsGridRow" />
                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                            </PagerStyle>
                            <HeaderStyle CssClass="ClsGridHeader" />
                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" disable-page="true" OnClick="BtnSave_Click"  />
                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" OnClick="btnCancel_Click"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidSchoolId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidStayBackApplicable" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidConfigType" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidUserId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidRowCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidColumnCount" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidValMaximumLectures" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidValMaximumLecturesCondition" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidValMaximumLecturesBlank" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidPleaseFixFollowingError" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidForStandard" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidWeekday" runat="server" />
    </div>

    <script type="text/javascript" language="javascript">
        _clientgrdStandardWeekDayGridId = "<%=this.grdStandardWeekDay.ClientID %>"
        _clienthidColumnCount = "<%=this.hidColumnCount.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        var Page_IsValid = true;

        function validatetextbox(aiCount, objBtn) {
        	 Page_IsValid = true;
            var iRowCount = document.getElementById(_clientgrdStandardWeekDayGridId).rows.length + 1
            var srow = ""
            var srow1 = ""
            var breturn = false
            var breturn1 = false
            var bMaxLectForBlank = false
            var bMaxLectForZero = false
            var completemessage = ""
            var completemessage1 = ""
            var completemessage4 = ""
            var completemessage5 = ""
            var chkCount = 0
            var iStandardCnt1 = 0
            var iStandardCnt2 = 0
            var bCount = false
            var msg = ""
            serrmessage = document.getElementById("<%=this.hidValMaximumLectures.ClientID %>").value
            for (var i = 2; i < iRowCount; i++) {
                if (i < 10) {
                    srow = "0" + i
                }
                else {
                    srow = i
                }
                srow1 = "0" + i
                chkCount = 0
                iStandardCnt1 = 0
                iStandardCnt2 = 0
                for (var j = 0; j <= aiCount - 1; j++) {
                    var sidtxtLect = _clientgrdStandardWeekDayGridId + "_ctl" + srow + "_txt_" + srow1 + "_" + j
                    var shidStandard = _clientgrdStandardWeekDayGridId + "_ctl" + srow + "_hids_" + srow1 + "_" + j
                    var shidWeekday = _clientgrdStandardWeekDayGridId + "_ctl" + srow + "_hidw_" + srow1 + "_" + j
                    if (document.getElementById(sidtxtLect).value == "") {
                        if (iStandardCnt1 == 0) {
                            var iRow = document.getElementById(shidStandard).value
                            sblanktotalerrmessage = "\n\r" + document.getElementById("<%=this.hidForStandard.ClientID %>").value + ": " + iRow + "\n\r" + document.getElementById("<%=this.hidWeekday.ClientID %>").value + ":"
                            iStandardCnt1++
                            completemessage = completemessage + sblanktotalerrmessage
                        }
                        var iRow = document.getElementById(shidWeekday).value
                        completemessage = completemessage + " \n  - " + iRow
                        breturn = true
                    }
                    if (!document.getElementById(sidtxtLect).value == 0) {
                        var iValue = parseInt(document.getElementById(sidtxtLect).value)
                        chkCount = chkCount + iValue
                    } 
                }
                var sidtxtLect2 = _clientgrdStandardWeekDayGridId + "_ctl" + srow + "_txt2_" + srow1 + "_" + (j - 1)
                if (document.getElementById(sidtxtLect2).value == "") {
                    var iRow = document.getElementById(shidStandard).value
                    sblanktotalerrmessage = "\n\r" + document.getElementById("<%=this.hidForStandard.ClientID %>").value+": " + iRow
                    completemessage4 = completemessage4 + sblanktotalerrmessage
                    bMaxLectForBlank = true
                }
                if ((document.getElementById(sidtxtLect2).value) == 0 && bMaxLectForBlank == false) {
                    var iRow = document.getElementById(shidStandard).value
                    sblanktotalerrmessage = "\n\r" + document.getElementById("<%=this.hidForStandard.ClientID %>").value+": " + iRow
                    completemessage5 = completemessage5 + sblanktotalerrmessage
                    bMaxLectForZero = true
                }
                if (document.getElementById(sidtxtLect2).value > chkCount) {
                    var iRow = document.getElementById(shidStandard).value
                    sblanktotalerrmessage = "\n\r" + document.getElementById("<%=this.hidForStandard.ClientID %>").value+": " + iRow
                    msg = msg + sblanktotalerrmessage
                    bCount = true
                } 
            }
            if (breturn) {
            	alert(document.getElementById("<%=this.hidPleaseFixFollowingError.ClientID %>").value+" \n\r" + serrmessage + completemessage)
            	 Page_IsValid = false;
                return false
            }
            if (bMaxLectForBlank) {
            	alert(document.getElementById("<%=this.hidPleaseFixFollowingError.ClientID %>").value+" \n\r" + document.getElementById("<%=this.hidValMaximumLecturesBlank.ClientID %>").value + completemessage4)
            	Page_IsValid = false;
                return false
            }
            if (bCount) {
                alert(document.getElementById("<%=this.hidPleaseFixFollowingError.ClientID %>").value + " \n\r" + document.getElementById("<%=this.hidValMaximumLecturesCondition.ClientID %>").value + msg)
            	Page_IsValid = false;
                return false
            }
            else {
                
                __doPostBack(objBtn.name, '')
                return true
            } 
        }
        function DisableButtons() {
            if (document.getElementById(_clientbtnSave)) {
                document.getElementById(_clientbtnSave).disabled = true
                document.getElementById(_clientbtnCancel).disabled = true
            }
        }
        </script>
</asp:Content>
