<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    EnableEventValidation="false" CodeFile="StudentsMonthWiseAttendanceListUIPopUp.aspx.cs"
    Inherits="StudentsMonthWiseAttendanceListUIPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td style="background-color: white;" id="Td1" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                <tr>
                                                    <td align="center" class="MainTitleHead" style="height: 20px">                                                        
                                                            <span style="font-weight:bold">
                                                                 <asp:Label ID="lblMonthWiseAttendance" runat ="server" Text="<%$ Resources:LocalizedResources,   MonthWiseAttendance %>" ></asp:Label>
                                                            </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:100px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.</td>
                                        <td>
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                                <ContentTemplate>
                                                    <table width="100%" runat="server" id="tblRecord">
                                                        <tr runat="server" id="trTotalRec" align="center">
                                                            <td>
                                                                <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />                                                                
                                                                    <span class="LblNormal">
                                                                        <asp:Label ID="Label1" runat ="server" Text= "<%$ Resources:LocalizedResources, To %>" ></asp:Label>
                                                                    </span>
                                                                <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />                                                                
                                                                    <span class="LblNormal"> <asp:Label ID="Label2" runat ="server" Text= "<%$ Resources:LocalizedResources, OutOf %>" ></asp:Label> </span>
                                                                <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />                                                                
                                                                    <span class="LblNormal"> <asp:Label ID="Label3" runat ="server" Text= "<%$ Resources:LocalizedResources, Records %>" ></asp:Label> </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server"
                                                        AllowPaging="True" AutoGenerateColumns="True" AllowSorting="false" OnRowCreated="grdStudents_RowCreated"
                                                        Width="1100px" PageSize="20" CellPadding="0" CellSpacing="1" 
                                                        ForeColor="#333333" EmptyDataText = "<%$ Resources:LocalizedResources, NoAttendanceAvailable %>"
                                                        GridLines="None" OnPageIndexChanging="grdStudents_PageIndexChanging" 
                                                        OnRowDataBound="grdStudents_RowDataBound" EnableViewState="False">
                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                        <Columns>
                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle CssClass="ClsGridRow ClspaddingL" />
                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle CssClass="ClsGridAltRow ClspaddingL" />
                                                        <PagerTemplate>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                                        <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectAPage %>" runat="server" CssClass="LblNrmlB" />
                                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                                            OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
                                                        runat="server" SelectMethod="GetStudentsMonthWiseAttendance" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountStudentsMonthWiseAttendance" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="hidStdDivId" PropertyName="Value" Name="aiStandardDivisionId" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>                                                    
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="RowCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="width:100px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;.</td>                                       
                                    </tr>
                                     <tr id="trNoRecordFound" runat="server">
                                            <td align="center" colspan ="3">
                                                <asp:Label ID="lblNoRecordFound" runat ="server" Text="<%$ Resources:LocalizedResources, NoAttendanceAvailable %>" CssClass="LblNoRecord"></asp:Label>
                                            </td>
                                    </tr>                                   
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tdBack" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" OnClick="btnBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel1">
            <ContentTemplate>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidStdDivId" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidSchoolId" runat="server" />
        <asp:HiddenField ID="hidAcademicYearId" runat="server" />
        <asp:HiddenField ID="hidIsConfig" runat="server" />
        <asp:HiddenField ID="hidAreYouSureDeleteStudent" runat="server" Value="<%$ Resources:LocalizedResources, AreYouSureDeleteStudent %>"/>
        <asp:HiddenField ID="hidAreYouSureDeleteThisStudent" runat="server" Value="<%$ Resources:LocalizedResources, AreYouSureDeleteThisStudent %>"/>
    </div>

    <script language="javascript" type="text/javascript">
        _sClientAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>";
        _sClientGridId = "<%=this.grdStudents.ClientID %>";
        _sClienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";


        function ConfirmDelete(iPageCount, sActionName) {
            var bResult = true;

            if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'chkDelete', sActionName, 'false', iPageCount, 'true')) {
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureDeleteStudent.ClientID %>").value)) {
                    bResult = false;
                }
            }
            else
            { bResult = false; }

            return bResult;
        }
        function test(sRef, sDestination, sTask) {

            var xmlHttpObj = CreateHTTPReqObj();
            if (xmlHttpObj) {

                var cntrl = document.getElementById(sRef); 
                var iSchoolId = document.getElementById(_sClienthidSchoolId).value;
                var iStandardId = cntrl.value;
                var iAcademicYearId = document.getElementById(_sClientAcademicYearId).value;
                var url = "../Ajax.ashx?SchoolId=" + iSchoolId + "&StandardId=" + iStandardId + "&AcademicYearId=" + iAcademicYearId + "&task=" + sTask;

                xmlHttpObj.open("GET", url, true);
                xmlHttpObj.onreadystatechange = function() {
                    if (xmlHttpObj.readyState == 4) {
                        if (xmlHttpObj.status == 200) {
                            var optionText = xmlHttpObj.responseText;
                            var cntrlDivision = document.getElementById(sDestination); 
                            cntrlDivision.options.length = 0;
                            var sArray = optionText.split("@@@");
                            var cnt = sArray.length;


                            var htmlCode = document.createElement("option");
                            htmlCode.text = "--All--";
                            htmlCode.value = "0";
                            cntrlDivision.options.add(htmlCode);
                            if (optionText != "") {

                                for (i = 0; i < cnt; i++) {
                                    var soption = sArray[i].split("###");

                                    var sText = soption[1];
                                    var sValue = soption[0];

                                    var htmlCode = document.createElement("option");
                                    htmlCode.text = sText;
                                    htmlCode.value = sValue;
                                    cntrlDivision.options.add(htmlCode);
                                }

                            }

                        }
                    }
                }
                xmlHttpObj.send(null);

            }
            else {
                alert('Sad!!');
            }

            document.getElementById(_sClientbtnAdd).style.display = 'none';
        }
        function assignDivision(obj) {

            document.getElementById(_sClienthidDivisionId).value = obj.value;

        }
        function assignStandard(obj) {

            if (obj.value == 0) {
                document.getElementById(_sClienthidStandardId).value = obj.value;
            }
            else {
                document.getElementById(_sClienthidStandardId).value = obj.value;
            }


        }
        function HideAddButton(objIdStd, objIdDiv) {

            if (document.getElementById(objIdDiv).value == 0) {

                document.getElementById(_sClientbtnAdd).style.display = 'none';
            }
            else {
                document.getElementById(_sClientbtnAdd).style.display = '';
            }


        }

        function refreshParent() {
            window.close();
        }

        function fnover(varname, doc) {

            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";            
        }

        function fnout(varname, doc) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";            
        }

        function ConfirmDelete() {
            var bResult = true;
            if (window.confirm(document.getElementById("<%=this.hidAreYouSureDeleteThisStudent.ClientID %>").value)) {
                bResult = true;
            }
            else {
                bResult = false;
            }

            return bResult;
        }       
          
    </script>

</asp:Content>
