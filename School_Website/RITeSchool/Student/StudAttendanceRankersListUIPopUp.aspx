<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    EnableEventValidation="false" CodeFile="StudAttendanceRankersListUIPopUp.aspx.cs"
    Inherits="StudAttendanceRankersListUIPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
 <div class="MainBodyDiv">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
        <tr>
            <td style="background-color: white;" align="center" valign="top">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td align="left" colspan="4">
                            <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="height: 20px" class="ClsGrayMainTitle">
                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                            <tr>
                                                <td align="center" class="MainTitleHead" style="height: 20px">
                                                    <asp:Label ID="lblBuyer" runat="server" BorderWidth="0px" Text="Attendance Toppers"
                                                        Font-Bold="True" EnableViewState="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" valign="top">
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <table style="width: 100%; height: 100%">
                                        <tr>
                                            <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" width="100px" class="ClsBorderlight" id="tdacademicYr" runat="server">
                                                                    <span id="lblacademicYr" class="ClsLabel">Academic Year :</span>
                                                            </td>
                                                            <td align="left" width="100px">
                                                                <asp:DropDownList ID="cmbAcademicYrId" runat="server" AutoPostBack="true"
                                                                        Width="100px" 
                                                                    onselectedindexchanged="cmbAcademicYrId_SelectedIndexChanged"></asp:DropDownList>
                                                            </td >
                                                            <td class="ErrHeadNew" align="left">
                                                                <asp:Label ID="lblOldAcademicYear" runat="server" Text = "">
                                                                </asp:Label>
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td style="height:10px;">
                                                            </td>
                                                        </tr>
                                                   </table>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td align="left">
                                                    <span class="ToprTotalHead WorkingDayRslt" id="lblStudentRank">Your attendance :</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:GridView CssClass="GridBorder" ID="grdStudentRank" runat="server" AllowPaging="true"
                                                    AutoGenerateColumns="True" AllowSorting="false" Width="100%" PageSize="20" CellPadding="0"
                                                    CellSpacing="1" EmptyDataText="No attendance records available." 
                                                    ForeColor="#333333" GridLines="None" OnSorting="grdStudents_Sorting"
                                                    OnRowDataBound="grdStudentRank_RowDataBound" EnableViewState="False">
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                    </PagerStyle>
                                                    <Columns>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle CssClass="ToprMarkGrdAltRow" />
                                                    <HeaderStyle CssClass="ToprTestHeader" ForeColor="black" />
                                                    <AlternatingRowStyle CssClass="ToprMarkGrdRow" />
                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:20px;">
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                    <span class="ToprTotalHead WorkingDayRslt" id="lblAttenRankers">Following are the top three attendance rankers of your class :</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AllowPaging="true"
                                                    AutoGenerateColumns="True" AllowSorting="false" Width="100%" PageSize="20" CellPadding="0"
                                                    CellSpacing="1" ForeColor="#333333" GridLines="None" OnSorting="grdStudents_Sorting"
                                                    OnRowDataBound="grdStudents_RowDataBound" 
                                                    onpageindexchanging="grdStudents_PageIndexChanging" 
                                                    EnableViewState="False">
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                    </PagerStyle>
                                                    <Columns>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle CssClass="ToprMarkGrdAltRow" />
                                                    <HeaderStyle CssClass="ToprTestHeader" ForeColor="black" />
                                                    <AlternatingRowStyle CssClass="ToprMarkGrdRow" />
                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                &nbsp;
                                            </td>
                                        </tr>
                                       
                                        <tr align="center">
                                            <td align="center" colspan="1" id="tdBack" runat="server">                                                
                                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                    ID="UpdatePanel1">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                        <asp:HiddenField ID="hidStdDivId" runat="server" />
                                                        <asp:HiddenField ID="hidSchoolId" runat="server" />
                                                        <asp:HiddenField ID="hidAcademicYearId" runat="server" />
                                                        <asp:HiddenField ID="hidIsConfig" runat="server" />
                                                        <asp:HiddenField ID="hidmOldAttendanceToppers" runat="server" Value="false" />
                                                        
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="Sorting" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                    
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text="Close" CssClass="ClsBtn" OnClick="btnBack_Click" />
                        </td>
                    </tr>                                               
                </table>
            </td>
        </tr>
    </table>
</div>
    <script language="javascript" type="text/javascript">
        _sClientAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>"
        _sClientGridId = "<%=this.grdStudents.ClientID %>"
        _sClienthidSchoolId = "<%=this.hidSchoolId.ClientID %>"
        function ConfirmDelete(iPageCount, sActionName) {
            var bResult = true
            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'chkDelete', sActionName, 'false', iPageCount, 'true')) {
                if (!window.confirm("Are you sure you want to delete the selected students(s)?")) {
                    bResult = false
                } 
            }
            else
            { bResult = false; }
            return bResult
        }
        function test(sRef, sDestination, sTask) {
            var xmlHttpObj = CreateHTTPReqObj()
            if (xmlHttpObj) {
                var cntrl = document.getElementById(sRef)
                var iSchoolId = document.getElementById(_sClienthidSchoolId).value
                var iStandardId = cntrl.value
                var iAcademicYearId = document.getElementById(_sClientAcademicYearId).value
                var url = "../Ajax.ashx?SchoolId=" + iSchoolId + "&StandardId=" + iStandardId + "&AcademicYearId=" + iAcademicYearId + "&task=" + sTask
                xmlHttpObj.open("GET", url, true)
                xmlHttpObj.onreadystatechange = function() {
                    if (xmlHttpObj.readyState == 4) {
                        if (xmlHttpObj.status == 200) {
                            var optionText = xmlHttpObj.responseText
                            var cntrlDivision = document.getElementById(sDestination)
                            cntrlDivision.options.length = 0
                            var sArray = optionText.split("@@@")
                            var cnt = sArray.length
                            var htmlCode = document.createElement("option")
                            htmlCode.text = "--All--"
                            htmlCode.value = "0"
                            cntrlDivision.options.add(htmlCode)
                            if (optionText != "") {
                                for (i = 0; i < cnt; i++) {
                                    var soption = sArray[i].split("###")
                                    var sText = soption[1]
                                    var sValue = soption[0]
                                    var htmlCode = document.createElement("option")
                                    htmlCode.text = sText
                                    htmlCode.value = sValue
                                    cntrlDivision.options.add(htmlCode)
                                } 
                            } 
                        } 
                    } 
                }
                xmlHttpObj.send(null)
            }
            else {
                alert('Sad!!')
            }
            document.getElementById(_sClientbtnAdd).style.display = 'none'
        }
        function assignDivision(obj) {
            document.getElementById(_sClienthidDivisionId).value = obj.value
        }
        function assignStandard(obj) {
            if (obj.value == 0) {
                document.getElementById(_sClienthidStandardId).value = obj.value
            }
            else {
                document.getElementById(_sClienthidStandardId).value = obj.value
            } 
        }
        function HideAddButton(objIdStd, objIdDiv) {
            if (document.getElementById(objIdDiv).value == 0) {
                document.getElementById(_sClientbtnAdd).style.display = 'none'
            }
            else {
                document.getElementById(_sClientbtnAdd).style.display = ''
            } 
        }
        function refreshParent() {
            window.close()
        }
        function fnover(varname, doc) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname, doc) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
        function ConfirmDelete() {
            var bResult = true
            if (window.confirm("Are you sure you want to delete this Student?")) {
                bResult = true
            }
            else {
                bResult = false
            }
            return bResult
        }
        function ShowPopup(queryString) {
            if (queryString != null && queryString != '') {
                window.open(queryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=500')
            } 
        }
    </script>
</asp:Content>
