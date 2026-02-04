<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentwiseRemarkUI.aspx.cs" Inherits="StudentwiseRemarkUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trMandetory" runat="server">
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UPnlValSum" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" ValidationGroup="SaveRemark"
                                ShowSummary="true" />
                            <%--<asp:CustomValidator ID="cst_Remark" runat="server" ValidationGroup="SaveRemark"
                                Display="None" ClientValidationFunction="StudentRemarkValidation" CssClass="ClsLabel"></asp:CustomValidator>--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" align="center">
                        <tr id="trFilters" runat="server">
                            <td width="100%">
                                <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="1" border="0">
                                                <tr>
                                                    <td class="ClsBorderlight" id="tdTeacher" runat="server">
                                                        <asp:Label ID="lblTeacher" runat="server" CssClass="ClsLabel" Text="Class Teacher :"
                                                            EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td id="tdTeacherList" runat="server">
                                                        <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                            OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                        </asp:DropDownList><span class="ClsMdtStar">&nbsp;*</span>
                                                        <asp:CompareValidator ID="cmp_Name" runat="server" ControlToValidate="cmbTeachers"
                                                            Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                                            ValueToCompare='0' ValidationGroup="SaveRemark"></asp:CompareValidator>
                                                    </td>
                                                    <td width="10%">
                                                    </td>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text="Student Name :"
                                                            EnableViewState="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UPnlStudent" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbStudents" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                                    OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged">
                                                                    <asp:ListItem Text="-- All --" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <table>
                                                <tr align="center">
                                                    <td class="ClsBorderlight">
                                                        <span class="clsLabel">Term : </span>
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="cmbTermName" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbTermName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" colspan="2">
                                            <table>
                                                <tr>
                                                    <td colspan="2" style="height: 18px" align="center">
                                                        <asp:UpdatePanel ID="UPnllblNorecord" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblNorecord" runat="server" CssClass="LblNoRecord" Visible="False"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                                <tr runat="server" id="trdtPagetDtPgCnt" align="center">
                                                    <td valign="top">
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                                            <ContentTemplate>
                                                                <table cellpadding="0" cellspacing="2" align="center" width="100%">
                                                                    <tr id="trPagerUser" runat="server">
                                                                        <td align="center">
                                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwStudentRemarkDetails">
                                                                                <Fields>
                                                                                    <asp:TemplatePagerField>
                                                                                        <PagerTemplate>
                                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                                CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                                CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                                CssClass="LblNrmlB" />
                                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                                            <br />
                                                                                        </PagerTemplate>
                                                                                    </asp:TemplatePagerField>
                                                                                </Fields>
                                                                            </asp:DataPager>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trListView" runat="server">
                                                                        <td align="center" id="tdMainListView" runat="server">
                                                                            <asp:ListView ID="lstvwStudentRemarkDetails" runat="server" OnItemDataBound="lstvwStudentRemarkDetails_ItemDataBound"
                                                                                DataKeyNames="YearwiseStudentId,StandardDivisionId,StudentwiseRemarkId">
                                                                                <LayoutTemplate>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <table id="tblDataPager" runat="server" >
                                                                                <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                                                                    <td align="center" id="tdPgr" runat="server">
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentRemarkDetails"
                                                                                            PageSize="20">
                                                                                            <Fields>
                                                                                                <asp:TemplatePagerField>
                                                                                                    <PagerTemplate>
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td align="left">
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
                                                                        </td>
                                                                    </tr>                                                                    
                                                                    <tr id="trNorecordFound" runat="server" visible="false">
                                                                        <td style="height: 10px;" align="center">
                                                                            <asp:Label ID="lblNoRcrdFnd" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                                                Text="No Record Found." EnableViewState="False" Width=" 800px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4" align="center">
                                                                            <asp:HiddenField ID="hidRollNo" runat="server" />
                                                                            <asp:HiddenField ID="hidUserRollNo" runat="server" />
                                                                            <asp:HiddenField ID="hidRemarkNameList" runat="server" />
                                                                            <asp:HiddenField ID="hidRemarkListCount" runat="server" />
                                                                            <asp:HiddenField ID="hidStudentwiaseRemarkListCount" runat="server" />
                                                                            <asp:HiddenField ID="hidPageNo" runat="server" Value="1" />
                                                                            <asp:HiddenField ID="hidListviewPageRowCnt" runat="server" Value="0" />
                                                                            <asp:CustomValidator ID="cstMaxLengthValidator" runat="server" ValidationGroup="SaveRemark"
                                                                                Display="None" SetFocusOnError="true" ClientValidationFunction="MaxLengthValidation"></asp:CustomValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center">
                                            <table>
                                                <tr align="center" width="100%">
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" ValidationGroup="SaveRemark"
                                                                    OnClick="btnSave_Click" />
                                                                <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="Back" PostBackUrl="~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx" />
                                                                <%--<asp:Button ID="btnShowReport" runat="server" CssClass="ClsBtn" Width="154px" Text="Save and Show Report"
                                                                    ValidationGroup="SaveRemark" onclick="btnShowReport_Click" />--%>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
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
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientlstvwStudentRemarkDetails = "<%= this.lstvwStudentRemarkDetails.ClientID %>"
        _clienthidRollNo = "<%= this.hidRollNo.ClientID %>"
        _clienthidRemarkListCount = "<%= this.hidRemarkListCount.ClientID %>"
        _clienthidStudentwiaseRemarkListCount = "<%= this.hidStudentwiaseRemarkListCount.ClientID %>"
        _clienthidRemarkNameList = "<%= this.hidRemarkNameList.ClientID %>"
        _clientlblUpdateSucess = "<%= this.lblUpdateSucess.ClientID %>"
        _clienthidPageNo = "<%= this.hidPageNo.ClientID %>"
        _clienthidListviewPageRowCnt = "<%= this.hidListviewPageRowCnt.ClientID %>"


        function MessageAlert(ddlCntObj) {
            var bIsValid
            if (window.confirm("If you change the page then entered progerss remarks on current page will get lost. Do you want to continue?"))
                bIsValid = true
            else {
                document.getElementById(ddlCntObj).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false
            }
            return bIsValid
        }

        function MaxLengthValidation(oSrc, args) {

            var RemarkListCount = document.getElementById(_clienthidRemarkListCount).value;
            var StudentwiseRemarkListCount = document.getElementById(_clienthidStudentwiaseRemarkListCount).value;
            var iCount = 0;
            var iRemarkCount = 0;
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var sRemarkNm = "";
            var sStudentNm = "";
            var sStudentName = "";
            var sRollNo = "";
            var sRollNoList = "";
            var sRemarkName = "";
            var sRemarkList = "";
            var TextBoxName = "";
            var sRArrayRemark = (document.getElementById(_clienthidRemarkNameList).value).split(',')

            if ((document.getElementById(_clientlblUpdateSucess) != null) && (document.getElementById(_clientlblUpdateSucess) != "undefined"))
                document.getElementById(_clientlblUpdateSucess).innerHTML = ""

            while (iCount < document.getElementById(_clienthidListviewPageRowCnt).value) {
                sRollNo = document.getElementById(_clientlstvwStudentRemarkDetails + "_ctrl" + iCount + "_lblRollNo").innerHTML;
                sStudentName = document.getElementById(_clientlstvwStudentRemarkDetails + "_ctrl" + iCount + "_lblName").innerHTML;
                iRemarkCount = 0;
                while (iRemarkCount < RemarkListCount) {
                    TextBoxName = document.getElementById(_clientlstvwStudentRemarkDetails + "_ctrl" + iCount + "_txt" + sRArrayRemark[iRemarkCount])
                    if (TextBoxName.value.length > 100) {
                        if (sRemarkList == "")
                            sRemarkList = sRArrayRemark[iRemarkCount];
                        else
                            sRemarkList = sRemarkList + ", " + sRArrayRemark[iRemarkCount];
                    }
                    TextBoxName = "";
                    iRemarkCount++;
                }
                if (sRemarkList != "") {
                    if (sRollNoList == "")
                        sRollNoList = sRollNo + " - " + sRemarkList;
                    else
                        sRollNoList = sRollNoList + "<br/>" + sRollNo + " - " + sRemarkList;
                    sRemarkList = "";
                }
                iCount++;
            }
            if (sRollNoList != "") {
                oSrc.errormessage = "Remark length should not be greater than 100 characters for the Roll No.(s) :<br/> " + sRollNoList;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

       
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
