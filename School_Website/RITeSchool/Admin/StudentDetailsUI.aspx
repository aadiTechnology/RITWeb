<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentDetailsUI.aspx.cs" Inherits="StudentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table cellpadding="0" cellspacing="1" width="90%">
            <tr>
                <td align="right" colspan="2" style="float: right">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatory" runat="server" ForeColor="Red" EnableViewState="False"
                        CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:ValidationSummary ValidationGroup="Search" ID="ValidationSummary1" runat="server"
                        CssClass="ClsLabel" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" />
                </td>
                <td style="float: right;" align="right">
                    <asp:CustomValidator ID="cstReg" runat="server" ClientValidationFunction="validateReg"
                        CssClass="ClsMdtStar" ValidationGroup="Search" Display="None" EnableClientScript="true"
                        ErrorMessage="<%$ Resources:LocalizedResources, NameOrRegNumberBlank%>" Visible="true"></asp:CustomValidator>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td>
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                        <ContentTemplate>
                            <table id="tblSearchInput" runat="server">
                                <tr id="trlblError" runat="server">
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
                                            <ContentTemplate>
                                                <asp:Label ID="lblErr" CssClass="ClsLabel" ForeColor="red" runat="server"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblNameRegNo" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, NameOrReg%>"></asp:Label>
                                        <span class="ClsLabel colonpadding">:</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" autocomplete="off"></asp:TextBox>
                                        <span style="color: #ff0000">*</span>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" ValidationGroup="Search" CssClass="ClsBtn" runat="server"
                                            Text="<%$ Resources:LocalizedResources, Search%>" OnClick="btnSearch_Click" Width="100px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" valign="top" class="ClspaddingT">
                    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <table width="100%" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr id="Tr5">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstVwStudent">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To%>"
                                                                        EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>"
                                                                        EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>"
                                                                        EnableViewState="false" />
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
                                                        <asp:ListView ID="lstVwStudent" runat="server" DataKeyNames="Enrolment_Number,SchoolWise_Student_Id"
                                                            OnItemCommand="lstVwStudent_ItemCommand" OnDataBound="lstVwStudent_DataBound"
                                                            OnItemDataBound="lstVwStudent_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr id="Tr1" runat="server" class="ClsGridHeader">
                                                                                    <th id="Th2" runat="server" align="left" class="ClspaddingL">
                                                                                        <asp:Label ID="lblRegNo" runat="server" Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                                    </th>
                                                                                    <th id="Th3" runat="server" align="left" class="ClspaddingL">
                                                                                        <asp:Label ID="lblClass" runat="server" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                    </th>
                                                                                    <th id="Th1" runat="server" align="left" class="ClspaddingL">
                                                                                        <asp:Label ID="lblRollNo" runat="server" Text="<%$ Resources:LocalizedResources,RollNo%>"></asp:Label>
                                                                                    </th>
                                                                                    <th id="Th4" runat="server" align="left" class="ClspaddingL">
                                                                                        <asp:Label ID="lblStudentName" runat="server" Text="<%$ Resources:LocalizedResources,StudentName%>"></asp:Label>
                                                                                    </th>
                                                                                    <th id="Th6" runat="server">
                                                                                        <asp:Label ID="lblSelectStudent" runat="server" Text="<%$ Resources:LocalizedResources,SelectStudent%>"></asp:Label>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                            <table width="100%" runat="server" id="tblDataPager" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr class="ClsGridAltRow" id="trDataPager" runat="server">
                                                                                    <td>
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstVwStudent">
                                                                                            <Fields>
                                                                                                <asp:TemplatePagerField>
                                                                                                    <PagerTemplate>
                                                                                                        <table width="100%">
                                                                                                            <tr class="ClsBorderPager">
                                                                                                                <td>
                                                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources,SelectPage%>"
                                                                                                                        runat="server" CssClass="LblNrmlB" />
                                                                                                                    <span class="colonPadding">:</span>
                                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td align="right">
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
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                    <td align="left">
                                                                        <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Enrolment_Number") %>' CssClass="ClspaddingL" />
                                                                        <asp:HiddenField ID="hidisLeft" runat="server" Value='<%# Eval("SchoolLeft_Date") %>' />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("StandardDivision") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblRoll_No" runat="server" Text='<%# Eval("Roll_No") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblStudent_Name" runat="server" Text='<%# Eval("Name") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                            CommandName="SelectCommand" CommandArgument='<%# Eval("SchoolWise_Student_Id ") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                    <td align="left">
                                                                        <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("Enrolment_Number") %>' CssClass="ClspaddingL" />
                                                                        <asp:HiddenField ID="hidisLeft" runat="server" Value='<%# Eval("SchoolLeft_Date") %>' />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("StandardDivision") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblRoll_No" runat="server" Text='<%# Eval("Roll_No") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblStudent_Name" runat="server" Text='<%# Eval("Name") %>' CssClass="ClspaddingL" />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                            CommandName="SelectCommand" CommandArgument='<%# Eval("SchoolWise_Student_Id ") %>' />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:ListView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="lstDSobj"
                                                        runat="server" SelectMethod="GetAllStudents" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountRows" EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="txtRegNo" PropertyName="Text" Name="asName" Type="string" />
                                                            <asp:ControlParameter ControlID="hidStandardId" PropertyName="Value" Name="aiStandardId"
                                                                DefaultValue="0" />
                                                            <asp:ControlParameter ControlID="hidDivisionId" PropertyName="Value" Name="aiDivisionId"
                                                                DefaultValue="0" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: white; margin-left: -10px;" id="tblStudentDetails" runat="server"
                                                    align="left" valign="top" visible="false">
                                                    <asp:UpdatePanel runat="server" ID="UPnlStudentDetails">
                                                        <ContentTemplate>
                                                            <!-- Data Insert Here -->
                                                            <table id="Table2" runat="server" style="width: 95%;" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" class="StudentDOBHead" style="width: 85%">
                                                                            <asp:Label ID="Label9" runat="server" CssClass="LblNrmlB" Text="<%$ Resources:LocalizedResources, StudentDetails%>"></asp:Label>
                                                                        </td>
                                                                        <td align="center" class="ClsBorderlight" rowspan="2" style="width: 50%">
                                                                            <img id="imgPhoto" alt="image" runat="server" height="130" width="110" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 85%" align="center">
                                                                            <table cellpadding="0" cellspacing="1" style="width: 100%">
                                                                                <tr>
                                                                                    <td align="left" style="width: 27%" class="ClsBorderlight">
                                                                                        <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 73%" class="ClsBorderlight">
                                                                                        <asp:Label ID="lblStudentName" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="Label7" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="lblDOB" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="lblClass" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, DivClass%>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="lblRollNo" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="Label23" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, MobileNumber%>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight">
                                                                                        <asp:Label ID="lblMobileOne" CssClass="ClsHilightTextB" EnableViewState="false" runat="server"
                                                                                            Text=""></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <table cellpadding="0" cellspacing="1">
                                                                                    <tr style="padding: 5px">
                                                                                        <td id="tdRollNosGeneration" valign="middle" runat="server">
                                                                                            <div id="divStudentInfo" style="width: 210px; height: 18px; vertical-align: bottom;
                                                                                                padding-top: 4px" class="ClsGreenBG" runat="server">
                                                                                                <asp:HyperLink ID="hlnkStudentRollNos" runat="server" CssClass="SubTitle" Text="Student Information"
                                                                                                    NavigateUrl="#" ></asp:HyperLink>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td id="hyperlnk" valign="middle" style="padding: 5px;" runat="server">
                                                                                            <div style="width: 125px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                                                                                class="ClsGreenBG">
                                                                                                <asp:HyperLink ID="hlnkStudentFeeDetails" runat="server" CssClass="SubTitle" Text="Fees"
                                                                                                    NavigateUrl="#" ></asp:HyperLink>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td id="Td2" valign="middle" style="padding: 5px;">
                                                                                            <div style="width: 125px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                                                                                class="ClsGreenBG">
                                                                                                <asp:HyperLink ID="hlnkStudentAttendance" runat="server" CssClass="SubTitle" Text="Attendance"
                                                                                                    NavigateUrl="#" ></asp:HyperLink>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td id="Td1" valign="middle" style="padding: 5px;">
                                                                                            <div style="width: 125px; height: 18px; vertical-align: bottom; padding-top: 4px"
                                                                                                class="ClsGreenBG">
                                                                                                <asp:HyperLink ID="hlnkExam" runat="server" CssClass="SubTitle" Text="Exam"
                                                                                                    NavigateUrl="#" ></asp:HyperLink>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstVwStudent" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidSearch" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="HidBackUrl" runat="server" />
                    <asp:HiddenField ID="hidStudentId" runat="server" />
                    <asp:HiddenField ID="hidStandardId" runat="server" />
                    <asp:HiddenField ID="hidDivisionId" runat="server" />
                     <asp:HiddenField ID="hidHasDebitEntries" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        _clienttxtregNo = "<%=this.txtRegNo.ClientID%>";
        //This function is used to validate registration number.
        function validateReg(oSrc, args) {
            if (trimAll(document.getElementById(_clienttxtregNo).value) == '') {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            AutoSearch();
        });

        function AutoSearch() {
            var SchoolId = "<%=miSchoolId %>";
            _clienttxtRegNumber = '#<%=txtRegNo.ClientID%>';
            var AcademicYearId = "<%=miAcademicYearId %>"
            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 1);
        }

        function OpenFeePopup(obj, qrystr) {
            if (!obj.disabled)
                window.open(qrystr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=650');

            return false;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);       

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtRegNo.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
