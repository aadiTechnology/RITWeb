<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RFIDDetailsUI.aspx.cs" Inherits="RFIDDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                        <asp:CustomValidator ID="cstStudentName" runat="server" ClientValidationFunction="ValidateName"
                            Display="None" SetFocusOnError="True" ErrorMessage="">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="cstRFID" runat="server" ClientValidationFunction="ValidateRFID"
                            Display="None" SetFocusOnError="True" ErrorMessage="">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="DuplicateRFID_Validate"
                            Display="None" SetFocusOnError="True" ErrorMessage="">
                        </asp:CustomValidator>
                    </ContentTemplate>
                    <Triggers>                        
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwUpdateRFIDDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center">
                <table id="tblUpdateRFID" width="80%" runat="server">
                    <tr align="center">
                        <td align="center">
                            <table id="Table1" runat="server" width="98%">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                                    CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwUpdateRFIDDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr align="center" style="text-align: center; margin: 0px auto;">
                                    <td align="center" style="text-align: center;">
                                        <table align="center">
                                            <tr>
                                                <td class="ClsBorderLight" align="left">
                                                    <span class="ClsLabel">Student Name / Enrolment No. / RFID : </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="ClsBtn" OnClick="btnSearch_Click"
                                                        CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trlink" runat="server">
                                    <td>
                                        <table id="tbllstRFID" align="center" width="95%" runat="server">
                                            <tr>
                                                <td align="center">
                                                    <asp:UpdatePanel ID="Up1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table align="center" width="100%">
                                                                <tr id="trDtPgCount" runat="server" visible="true">
                                                                    <td align="center">
                                                                        <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwUpdateRFIDDetails"
                                                                            PageSize="20">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                                        <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                                        <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                                        <br />
                                                                                    </PagerTemplate>
                                                                                </asp:TemplatePagerField>
                                                                            </Fields>
                                                                        </asp:DataPager>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPager" runat="server" width="100%">
                                                                    <td align="center">
                                                                        <asp:ListView ID="lstvwUpdateRFIDDetails" runat="server" ViewStateMode="Enabled"
                                                                            DataKeyNames="SchoolWiseStudentId, Id,UserId" OnItemCommand="lstvwUpdateRFIDDetails_ItemCommand"
                                                                            OnDataBound="lstvwUpdateRFIDDetails_DataBound">
                                                                            <LayoutTemplate>
                                                                                <table id="lstvwtable1" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                                    class="GridBorder" width="100%">
                                                                                    <tr id="trheader" runat="server" class="ClsGridHeader">
                                                                                        <th align="left" class="PaddingLR" width="150px">
                                                                                            <asp:Label ID="lblClass" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                                                Text="Class Name"></asp:Label>
                                                                                        </th>
                                                                                        <th align="left" class="paddingLR" width="150px">
                                                                                            <asp:Label ID="lblEnrolment" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                                                Text="Enrolment No"></asp:Label>
                                                                                        </th>
                                                                                        <th align="right" class="PaddingR" width="70px">
                                                                                            <asp:Label ID="lblRollNo" runat="server" CssClass="PaddingR" Style="padding-right: 5px;"
                                                                                                Text="Roll No"></asp:Label>
                                                                                        </th>
                                                                                        <th align="left" class="PaddingL">
                                                                                            <asp:Label ID="lblName" runat="server" Text="Student Name" Style="padding-left: 5px;"></asp:Label>
                                                                                        </th>
                                                                                        <th align="left" class="PaddingL" width="150px">
                                                                                            <asp:Label ID="lblRFID" runat="server" Text="RFID" Style="padding-left: 5px;"></asp:Label>
                                                                                        </th>
                                                                                        <th align="center" style="width: 50px;">
                                                                                            Select
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                    <tr id="trDataPager" class="ClsBorderPager">
                                                                                        <td colspan="6">
                                                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUpdateRFIDDetails"
                                                                                                PageSize="10">
                                                                                                <Fields>
                                                                                                    <asp:TemplatePagerField>
                                                                                                        <PagerTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td align="left">
                                                                                                                        <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                                                        <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                                                            AutoPostBack="true">
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
                                                                                <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                                    <td align="left" class="PaddingL">
                                                                                        <asp:Label ID="lblClass1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("ClassName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="left" class="paddingL">
                                                                                        <asp:Label ID="lblEnrolment1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("EnrolmentNumber") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="right" class="PaddingLR">
                                                                                        <asp:Label ID="lblRollNo1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("RollNo") %>'>
                                                                                        </asp:Label>
                                                                                        <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                                                    </td>
                                                                                    <td align="left" class="PaddingL">
                                                                                        <asp:Label ID="lblName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("StudentName") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="left" class="PaddingL">
                                                                                        <asp:Label ID="lblRFID" runat="server" Style="padding-left: 5px;" Text='<%# Eval("RFID") %>'>
                                                                                        </asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                            CausesValidation="false" CommandName="SelectDetails" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <EmptyDataTemplate>
                                                                                <tr style="width: 800px">
                                                                                    <td align="center" class="LblNoRecord">
                                                                                        No record Found
                                                                                    </td>
                                                                                </tr>
                                                                            </EmptyDataTemplate>
                                                                        </asp:ListView>
                                                                        <asp:ObjectDataSource TypeName="BusinessLogic.TransportBL.RFIDDetailsBL" EnablePaging="true"
                                                                            ID="objdsRFIDDetails" runat="server" SelectMethod="GetAllStudents" SortParameterName="SortExpression"
                                                                            SelectCountMethod="GetCountStudent" EnableCaching="false">
                                                                            <SelectParameters>
                                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                    Type="int32" />
                                                                                <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                                                <asp:Parameter Name="SortExpression" Type="String" />
                                                                                <asp:Parameter Name="SortDirection" Type="String" />
                                                                                <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                                                <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>                                                                        
                                                                        <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" />
                                                                        <asp:HiddenField ID="hidUserId" runat="server" />
                                                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center">
                            <table id="tbl" runat="server" width="98%">
                                <tr id="trControls" runat="server" style="text-align: center; margin: 0px auto;">
                                    <td align="center" style="text-align: center;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table align="center">
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left" style="width: 200px">
                                                            <asp:Label ID="lblStudentName" CssClass="ClsLabel" runat="server" Text="Student Name :"></asp:Label>
                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblStudentNameData" runat="server" CssClass="ClsLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">New RFID :</span>
                                                        </td>
                                                        <td class="txtNormal" align="left">
                                                            <asp:TextBox ID="txtRFID" runat="server" MaxLength="20" CssClass=""></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td colspan="2" align="center">
                                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                                class="ClsBtn" OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                                                class="ClsBtn" OnClick="btnCancel_Click" CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwUpdateRFIDDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        _clienttxtRFID = "<%=this.txtRFID.ClientID %>"
        _clientlblStudentName = "<%=this.lblStudentNameData.ClientID %>"

        function ValidateName(src, args) {
            if ($get(_clientlblStudentName).innerHTML == "") {
                src.errormessage = "Student Name should be selected."
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateRFID(src, args) {
            args.IsValid = true;
            if ($get(_clienttxtRFID).value == "") {
                src.errormessage = "RFID should not be blank."
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
