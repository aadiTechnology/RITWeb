<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="DebitEntryUI.aspx.cs" Inherits="DebitEntryUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
<style>
     .paddingRSML
     {
         text-align:right;
         padding-right:5px;
     }
</style>
    <table width="97%">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="show" runat="server"
                            CssClass="ClsLabel" />
                        <asp:ValidationSummary ID="valErrMsg" ValidationGroup="Save" runat="server" CssClass="ClsLabel"
                            ViewStateMode="Enabled" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ValidationGroup="Save" ClientValidationFunction="ValidateAmount"
                            ErrorMessage="Error msg"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstChequeNumber" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ValidationGroup="Save" ClientValidationFunction="ValidateChequeNo"
                            ErrorMessage="Cheque number should be selected."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstDueDate" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ValidationGroup="Save" ClientValidationFunction="ValidateDueDate"
                            ErrorMessage="Error msg"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstFeeType" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ValidationGroup="Save" ClientValidationFunction="ValidateFeeType"
                            ErrorMessage="Error msg"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstPayableFor" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ValidationGroup="Save" ClientValidationFunction="ValidatePayableFor"
                            ErrorMessage="Error msg"></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                    <ContentTemplate>
                        <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                            Visible="true"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Font-Bold="true" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table runat="server" id="tblInputFields" width="97%" class="ClsBorderlight">
                    <tr runat="Server" id="trStandard">
                        <td>
                            <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblSelectStandard" runat="server" class="clsLabel" Text="<%$ Resources:LocalizedResources, SelectStandards%>"></asp:Label>
                                                <span class="clsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left" style="margin-left: 40px">
                                                <asp:DropDownList ID="ddlStandard" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                    OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged" AppendDataBoundItems="True"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblSelectDivision" runat="server" class="clsLabel" Style="height: 16px"
                                                    Text="<%$ Resources:LocalizedResources, SelectDivision%>"></asp:Label>
                                                <span class="clsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="LrgCombo" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtRegNumber" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr id="Tr1">
                                            <td class="HilightBGGray" align="center">
                                                <asp:Label ID="lblSelectStandardDivision" runat="server" class="ClsHilightText" Text="<%$ Resources:LocalizedResources, SelectStandardDivision%>"></asp:Label>
                                                <img src="../images/ArrowBlueDblRev.gif" />
                                                <asp:Label ID="lblOr" runat="server" class="ClsHilightTextB" Text="<%$ Resources:LocalizedResources, OR%>"></asp:Label>
                                                <img src="../images/ArrowBlueDblNw.gif" />
                                                <asp:Label ID="lblSelectStudent" runat="server" Text="<%$ Resources:LocalizedResources,SelectStudent%>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="Tr2">
                                            <td align="center">
                                                <table cellpadding="0" cellspacing="2">
                                                    <tr>
                                                        <td class="ClsBorderlight" align="right">
                                                            <asp:Label ID="lblStudentNameRegNo" runat="server" class="clsLabel" Style="height: 16px"
                                                                Text="<%$ Resources:LocalizedResources,SelectstudentNameRegNo%>"></asp:Label>
                                                            <span class="clsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <%--The AutoPostBack event is explicitly set to false to avoid duplicate postback--%>
                                                            <asp:TextBox ID="txtRegNumber" TabIndex="3" Width="290px" runat="server" MaxLength="50"
                                                                CssClass="MidTxtBox" OnTextChanged="txtRegNumber_TextChanged" AutoPostBack="False"
                                                                autocomplete="off"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hidStandardId" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr id="Tr3">
                                            <td class="HilightBGGray" align="center">
                                                <span class="ClsHilightText"><span class="ClsHilightTextB"></span></span>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtRegNumber" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table id="Table1" cellpadding="0" cellspacing="1" width="100%">
                    <tr>
                        <td align="center" colspan="4" style="padding: 5px 0 5px 0">
                            &nbsp;<asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtnMid" Height="27px"
                                CausesValidation="true" ValidationGroup="show" TabIndex="5" OnClick="btnShow_Click" />
                            <asp:HiddenField ID="hidStudentId" runat="server" />
                            <asp:HiddenField ID="hidMode" runat="server" Value="New" />
                            <asp:HiddenField ID="hidDebitId" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidYearEndDate" runat="server" />
                            <asp:HiddenField ID="hidYearStartDate" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                            <asp:HiddenField ID="hidStdDivId" runat="server" />
                            <asp:HiddenField ID="hidSerialNo" runat="server" />
                            <asp:HiddenField ID="hidDebitEntryLevel" runat="server" />
                            <asp:HiddenField ID="hidCurrentLevel" runat="server" />
                            <asp:HiddenField ID="hidDivisionId" runat="server" />
                            <asp:HiddenField ID="hidStandardDivId" runat="server" />
                            <asp:HiddenField ID="hidStudentName" runat="server" />
                            <asp:HiddenField ID="hidIsChequeBounc" runat="server" />
                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                            <asp:HiddenField ID="hidShow" runat="server" />
                            <asp:HiddenField ID="hidAmountShouldNotBeBlank" runat="server" />
                            <asp:HiddenField ID="hidAmountShouldNotBeZero" runat="server" />
                            <asp:HiddenField ID="hidChequeNumberShouldBeSelected" runat="server" />
                            <asp:HiddenField ID="hidFeeTypeShouldNotBeBlank" runat="server" />
                            <asp:HiddenField ID="hidFeeTypeShouldBeSelected" runat="server" />
                            <asp:HiddenField ID="hidPayableForShouldNotBeBlank" runat="server" />
                            <asp:HiddenField ID="hidPayableForShouldBeSelected" runat="server" />
                            <asp:HiddenField ID="hidDueDateShouldNotBeBlank" runat="server" />
                            <asp:HiddenField ID="hidDoYouWantToSendFollowingSMSMessage" runat="server" />
                            <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisBounceChequeTransaction" runat="server" />
                            <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisDebitDetails" runat="server" />
                            <asp:HiddenField ID="hidDoYouWantToSendSMSTo" runat="server" />
                            <asp:HiddenField ID="hidDoYouWantToSendMessageTo" runat="server" />
                            <asp:HiddenField ID="hidRegNo" runat="server" />
                        </td>
                    </tr>
                </table>
                <table width="97%" runat="server" id="trStudents" visible="false">
                    <tr runat="server" id="trTotalRec" align="center" visible="false">
                        <td>
                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                            <asp:Label ID="lblTo" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources,To%>"></asp:Label>
                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                            <asp:Label ID="lblOutOf" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources,OutOf%>"></asp:Label>
                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                            <asp:Label ID="lblrecords" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources,Records%>"></asp:Label>
                        </td>
                    </tr>
                    <tr class="ClsBorderlight">
                        <td colspan="4">
                            <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AutoGenerateColumns="False"
                                Height="100%" PageSize="5" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                GridLines="None" DataKeyNames="Yearwise_Student_Id,SchoolLeft_Date" Width="100%"
                                OnRowDataBound="grdStudents_RowDataBound" ShowFooter="False" OnRowCommand="grdStudents_RowCommand"
                                EmptyDataText="No record found." EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="true">
                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                </PagerStyle>
                                <Columns>
                                    <asp:BoundField DataField="Enrolment_Number" HeaderText="<%$ Resources:LocalizedResources,RegNo%>"
                                        SortExpression="Enrolment_Number">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                            Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StandardDivision" HeaderText="<%$ Resources:LocalizedResources,Class%>"
                                        SortExpression="StandardDivision">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources,RollNo%>"
                                        SortExpression="Roll_No">
                                        <ItemStyle Width="70px" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        <HeaderStyle Width="70px" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                            Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedResources,StudentName%>"
                                        SortExpression="First_Name">
                                        <ItemStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                        <HeaderStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                            Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DOB" HeaderText="<%$ Resources:LocalizedResources,DateOfBirth%>"
                                        SortExpression="DOB">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SchoolLeft_Date" HeaderText="<%$ Resources:LocalizedResources,LeftDate%>"
                                        SortExpression="SchoolLeft_Date">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:ButtonField ButtonType="Image" CommandName="PAY_FEE" HeaderText="<%$ Resources:LocalizedResources,Selects%>"
                                        Text="Select" ImageUrl="~/RITeSchool/images/Selection5.gif">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:ButtonField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle CssClass="ClsGridRow" />
                                <HeaderStyle CssClass="ClsGridHeader" />
                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                <PagerTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources,SelectPage%>"
                                                    runat="server" CssClass="LblNrmlB" />
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
                                runat="server" SelectMethod="GetAllStudentsForFee" SortParameterName="sortExpression"
                                SelectCountMethod="CountStudentsForFee" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                        Type="string" />
                                    <asp:Parameter DefaultValue="0" DbType="Int32" Name="aiStandardId" />
                                    <asp:Parameter DefaultValue="0" DbType="Int32" Name="aiDivisionId" />
                                    <asp:ControlParameter ControlID="txtRegNumber" PropertyName="Text" Name="asName" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
                <table width="97%" id="tblDebitEntry" runat="server" visible="false" class="ClsBorderlight">
                    <tr id="tblStudentInfo" runat="server" visible="false">
                        <td colspan="4">
                            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblClass" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Class%>"></asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="HilightBGGray">
                                        <asp:Label ID="lblStandardDivision" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblStudentName1" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,StudentName%>"> </asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="HilightBGGray">
                                        <asp:Label ID="lblStudentName" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight">
                                        <asp:Label ID="lblRollNo" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,RollNo%>"> </asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="HilightBGGray">
                                        <asp:Label ID="lblRollNumber" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" id="tdJoinDate" runat="server">
                                        <asp:Label ID="lblJoiningDate1" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,JoiningDate%>"> </asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="HilightBGGray" id="tdJoiningDate" runat="server">
                                        <asp:Label ID="lblJoiningDate" runat="server" CssClass="LblNrmlB" Text="" EnableViewState="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trConcession" runat="server" visible="false">
                        <td colspan="8">
                            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="ClsBorderlight" align="left">
                                        <asp:Label ID="lblConcessionRule" runat="server" CssClass="Lbl10ptB" EnableViewState="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trLeftStudent" runat="server" visible="false">
                        <td colspan="4">
                            <table style="width: 100%;" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="ClsBorderlight" align="center">
                                        <blink>
											<asp:Label ID="lblLeft" runat="server" CssClass="ErrHeadNew" EnableViewState="true"></asp:Label>
										</blink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr5" runat="server">
                        <td align="center" colspan="4">
                            <asp:RadioButtonList ID="rdlstDisplayFeeType" runat="server" Style="float: center;
                                font-size: 9pt; font-family: Arial;" AutoPostBack="true" RepeatDirection="Horizontal"
                                Width="200px" OnSelectedIndexChanged="rdlstDisplayFeeType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="Student Fee" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Internal Fee" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trFeeType" runat="server">
                        <td align="left" colspan="3">
                            <asp:RadioButtonList ID="rdoFeeType" runat="server" CssClass="clsLabel" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rdoFeeType_SelectedIndexChanged" AutoPostBack="True"
                                TabIndex="6">
                                <asp:ListItem Selected="True" Text="<%$ Resources:LocalizedResources,NewFeeType%>"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:LocalizedResources,ExistingFeeType%>"></asp:ListItem>
                                <asp:ListItem Enabled="False" Text="<%$ Resources:LocalizedResources,ChequeBounceEntry%>"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                            <span class="ClsMdtStar">*</span>
                            <asp:Label ID="mandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources,MandatoryFields%>"></asp:Label>
                        </td>
                    </tr>
                     <tr runat="server" id="trMode" visible="false">
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="right" class="ClsBorderlight" style="width: 190px">
                                        <asp:Label ID="SelectMode" runat="server" class="ClsLabel" Text="Select Mode"></asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlMode" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlMode_SelectedIndexChanged">
                                           <%--  <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Cheque"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Swipe Card"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Electronic"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Online Transaction"></asp:ListItem>--%>
                                       </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 60%">
                                        <asp:Label ID="lblMasgBounce" runat="server" CssClass="ClsErrorMsg" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="trChequeNo" visible="false">
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="right" class="ClsBorderlight" style="width: 190px">
                                        <asp:Label ID="lblSelectChequeNo" runat="server" class="ClsLabel" Text="Cheque\Transaction No"></asp:Label>
                                        <span class="clsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
										<asp:DropDownList ID="ddlChequeNo" runat="server" TabIndex="7" AutoPostBack="True"
											OnSelectedIndexChanged="ddlChequeNo_SelectedIndexChanged">
										</asp:DropDownList>
										<span class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 60%">
                                        <asp:Label ID="lblMsgBounce" runat="server" CssClass="ClsErroMsg" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                   
                    <tr>
                        <td style="width: 105px" class="ClsBorderlight">
                            <asp:Label ID="lblAmount" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Amount%>"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmt" CssClass="SmlTxtBox" MaxLength="6" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" runat="server"
                                TabIndex="8"></asp:TextBox>
                            <span class="ClsMdtStar">*</span>
                        </td>
                        <td class="ClsBorderlight">
                            <asp:Label ID="lblDueDate" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,DueDate%>"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsTextNormal">
                            <table>
                                <tr>
                                    <td align="left" style="padding-top: 5px;" width="110px">
                                        <asp:CheckBox ID="chkNotApplicable" runat="server" CssClass="ClsLabel" AutoPostBack="true"
                                            CausesValidation="false" Text="Not Applicable" OnCheckedChanged="chkNotApplicable_CheckedChanged" />
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtDueDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                                    Width="90px" TabIndex="9"></asp:TextBox>
                                                <rjs:PopCalendar ID="cal_DueDate" runat="server" Control="txtDueDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources,ChequeDateShouldNotBeBlank%>"
                                                    Culture="en" />
                                                <span id="spnMandatory" runat="server" class="ClsMdtStar">*</span>&nbsp;&nbsp;
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkNotApplicable" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <asp:Label ID="lblFeeType" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,FeeType%>"></asp:Label>
                            <span class="clsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsTextNormal" style="padding-right: 10px;">
                            <asp:DropDownList ID="ddlFeeType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFeeType_SelectedIndexChanged"
                                TabIndex="10" />
                            <asp:DropDownList ID="ddlOtherFeeTypes" runat="server" TabIndex="11" OnChange="OtherFeeTypeOnChange(this);" />
                            <span id="feetypeSeparator" runat="server" class="ClsHilightText">
                                <img src="../images/ArrowBlueDblRev.gif" />
                                <asp:Label ID="lblOr1" runat="server" Style="font-weight: bold;" Text="<%$ Resources:LocalizedResources,OR%>"></asp:Label>
                                <img src="../images/ArrowBlueDblNw.gif" />
                            </span>
                            <asp:TextBox ID="txtFeeType" runat="server" CssClass="SmlTxtBox" Width="200px" TabIndex="12"
                                MaxLength="50" />
                            <span class="ClsMdtStar">*</span>
                        </td>
                        <td style="width: 105px" class="ClsBorderlight">
                            <asp:Label ID="lblPayableFor" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,PayableFor%>"></asp:Label>
                            <span class="clsLabel colonPadding">:</span>
                        </td>
                        <td style="width: 265px;">
                            <asp:TextBox ID="txtPayableFor" CssClass="SmlTxtBox" MaxLength="50" runat="server"
                                Width="250px" TabIndex="12"></asp:TextBox>
                            <asp:DropDownList ID="ddlPayableFor" runat="server" TabIndex="13" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPayableFor_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                            <asp:DropDownList ID="cmbAccountHeader" runat="server" TabIndex="14" AutoPostBack="True"
                                Visible="false">
                            </asp:DropDownList>
                            <span id="spnStar" runat="server" class="ClsMdtStar">*</span>
                            <asp:RequiredFieldValidator ID="ReqAccountHeader" runat="server" Display="None" ControlToValidate="cmbAccountHeader"
                             InitialValue="0" ErrorMessage="Account Header should be selected." ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px" class="ClsBorderlight">
                            <asp:Label ID="lblRemarks" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources,Remarks%>"></asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" CssClass="SmlTxtBox" MaxLength="50" runat="server" Width="320px"
                                TabIndex="14"></asp:TextBox>&nbsp;
                        </td>
                        <td colspan="2" class="ClsBorderlight" id="tdSMSLabel" runat="server" visible="true">
                            <%if (!Settings.IsMiniSite) %>
                            <%{ %>
                            <asp:UpdatePanel ID="upnlMessage" runat="server">
                                <ContentTemplate>
                                    <div style="float: left; width: 160px">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSendSms" runat="server" class="ClsLabel" Style="vertical-align: middle"
                                                        Text="<%$ Resources:LocalizedResources,SendSMSMsg%>"> </asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td>
                                                    <span class="ClsLabel" style="vertical-align: top">
                                                        <asp:CheckBox ID="chkSendSMS" runat="server" OnCheckedChanged="chkSendSMS_CheckedChanged"
                                                            TabIndex="15" AutoPostBack="True" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="float: right; width: 150px">
                                        <table id="tblMsg" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbSendMessage" runat="server" class="ClsLabel" Style="vertical-align: middle"
                                                        Text="<%$ Resources:LocalizedResources,SendMessage%>"> </asp:Label>
                                                    <span class="clsLabel colonPadding">:</span>
                                                </td>
                                                <td>
                                                    <span class="ClsLabel" style="vertical-align: middle">
                                                        <asp:CheckBox ID="chkSendMessage" runat="server" OnCheckedChanged="chkSendMessage_CheckedChanged"
                                                            TabIndex="16" AutoPostBack="True" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:HiddenField ID="hidSendMsg" runat="server" />
                                    <asp:HiddenField ID="hidSendSms" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="chkSendSMS" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkSendMessage" EventName="CheckedChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <%} %>
                        </td>
                    </tr>
                    <tr align="left">
                        <td style="width: 200px" class="ClsBorderlight">
                            <asp:Label ID="lblRTE" runat="server" class="ClsLabel" Style="vertical-align: middle"
                                Text="Consider For RTE Concession?"> </asp:Label>
                            <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <span class="ClsLabel" style="vertical-align: top">
                                <asp:CheckBox ID="chkRTEStudent" runat="server" TabIndex="15" />
                            </span>
                        </td>                      
                        <td colspan="2" id="tdInternalFeeOnlinePayment" runat="server" visible="false">
                            <table>
                                <tr id="trInterFeeOnline" runat="server">
                                    <td class="ClsBorderlight" style="width:200px;">
                                          <asp:Label ID="Label1" runat="server" class="ClsLabel" Style="vertical-align: middle"
                                          Text="Consider For Online Payment?"> </asp:Label>
                                          <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                         <span class="ClsLabel" style="vertical-align: top">
                                             <asp:CheckBox ID="chkConsiderForOnline" runat="server" TabIndex="15" />
                                         </span>
                                    </td>                              
                                </tr>
                            </table>
                        </td>                                                 
                    </tr>                   
                    <tr>
                        <td align="center" colspan="6">
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources,Save%>"
                                CssClass="ClsBtnMid" ValidationGroup="Save" OnClick="btnSave_Click" TabIndex="18" />
                            <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:LocalizedResources,Delete%>"
                                CausesValidation="False" CssClass="ClsBtnMid" OnClick="btnDelete_Click" TabIndex="19"
                                Visible="false" />
                            <asp:Button ID="btnDisableOnlinePayment" runat="server" Text="Disable Online Fee Payment"
                                  CssClass="ClsBtnMid" OnClick="btnDisableOnlinePayment_Click" Visible="false" />
                            <asp:Button ID="btnDelUnpaidFee" runat="server" Text="Delete Un-Paid Fee"
                                  CssClass="ClsBtnMid" OnClick="btnDelUnpaidFee_Click" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources,Cancel%>"
                                CausesValidation="False" CssClass="ClsBtnMid" OnClick="btnCancel_Click" TabIndex="20" />
                        </td>
                    </tr>
                    <tr id="trGrdDebitinfo" runat="server">
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td align="center" width="6%">
                                        <asp:Label ID="lblLegend" runat="server" class="ClsLblLgnd" Style="font-weight: bold;"
                                            Text="<%$ Resources:LocalizedResources,Legend%>"></asp:Label>
                                    </td>
                                    <td align="left" width="3%" valign="middle">
                                        <span class="BounceCheque" style="display: inline-block; border-color: Black; border-width: 1px;
                                            border-style: Solid; height: 20px;">
                                            <img height="20px" src="../images/spacer.gif" width="20px" />
                                        </span>
                                    </td>
                                    <td align="left" width="85%" valign="middle">
                                        <asp:Label ID="lblBouncedChequeTransaction" runat="server" class="ClsTextNormal"
                                            Style="font-weight: bold;" Text="<%$ Resources:LocalizedResources,BouncedChequeTransactions%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:GridView ID="grdDebitInfo" CssClass="GridBorder" runat="server" AutoGenerateColumns="False"
                                            AllowSorting="true" Height="100%" PageSize="20" CellPadding="0" CellSpacing="1"
                                            ForeColor="#333333" GridLines="None" DataKeyNames="Schoolwise_Student_Fee_Id,Standard_Div_Id,Serial_Number,DebitLevel,Is_Cheque_Bounce,IsPaid,IsInternalFee,IsConsiderForRTEStudent, AccountHeaderId,IsDueDateApplicable,IsOnlinePaymentApplicable,ShowUnPaidDisableButton,ShowUnPaidDeleteButton"
                                            Width="99%" BackColor="White" OnRowCommand="grdDebitInfo_RowCommand" OnRowCreated="grdDebitInfo_RowCreated"
                                            OnRowDataBound="grdDebitInfo_RowDataBound" OnSorting="grdDebitInfo_Sorting" EmptyDataText="<%$ Resources:LocalizedResources,NoRecordFound%>"
                                            EmptyDataRowStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,FeeType%>" DataField="Fee_Type"
                                                    SortExpression="Fee_Type">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                        Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,PayableFor%>" DataField="Payable_For"
                                                    SortExpression="Payable_For">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="true" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                        Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,DueDate%>" SortExpression="Paid_Date"
                                                    DataField="Paid_Date" DataFormatString="{0:dd MMM yyyy}">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,Amount%>" SortExpression="Amount"
                                                    DataField="Amount">
                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingRSML" />
                                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingRSML" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,Remarks%>" SortExpression="Remarks"
                                                    DataField="Remarks" HtmlEncode="False">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="True" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
                                                        Wrap="True" />
                                                </asp:BoundField>
                                                <asp:ButtonField ButtonType="Image" CommandName="Edit_Debit_Entry" HeaderText="<%$ Resources:LocalizedResources,Selects%>"
                                                    Text="Select" ImageUrl="~/RITeSchool/images/Selection5.gif">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:ButtonField>
                                                <asp:ButtonField ButtonType="Image" CommandName="Copy_Debit_Entry" HeaderText="<%$ Resources:LocalizedResources,Copy%>"
                                                    Text="Copy" ImageUrl="~/RITeSchool/images/Icon_BookAdd.gif">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                </asp:ButtonField>
                                                <asp:ButtonField ButtonType="Image" HeaderText="Consider For RTE Concession?" Text="Consider For RTE Concession?"
                                                    ImageUrl="../images/IconGrid_AssignTrue.gif">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <RowStyle CssClass="ClsGridAltRow" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridRow" />
                                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="trTotalAmt" visible="false">
                                    <td align="right" colspan="4">
                                        <table width="100%" cellpadding="0" cellspacing="1" class="ClsBorderlight">
                                            <tr>
                                                <td align="right" style="padding-right: 9px; width: 72%; background-color: #eaeaea;">
                                                </td>
                                                <td style="width: 124px; background-color: #b3def2;" align="left">
                                                    <asp:Label ID="lbltotalAmount" runat="server" class="LblNrmlB" Style="display: inline-block;
                                                        width: auto;" Text="<%$ Resources:LocalizedResources,TotalAmount%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" style="background-color: #eaeaea">
                                                    <asp:TextBox ID="txtAmtPaid" Width=" 95px" Height="25px" runat="server" CssClass="ClsHilightBGB"
                                                        Style="text-align: right" />
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
        <tr>
            <td align="center">
                <asp:Button ID="btnBackUp" runat="server" CssClass="ClsBtn" CausesValidation="False"
                    Text="<%$ Resources:LocalizedResources,Back%>" Height="24px" OnClick="btnBackUp_Click"
                    UseSubmitBehavior="false" TabIndex="21"></asp:Button>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clienttxtRegNumber = "<%=this.txtRegNumber.ClientID%>";
        _clienttxtRegNo = "<%=this.txtRegNumber.ClientID%>";
		_clientddlDivision = "<%=this.ddlDivision.ClientID%>";
		_clientddlStandard = "<%=this.ddlStandard.ClientID%>";
		_clienttxtAmt = "<%=this.txtAmt.ClientID%>";
		_clientcstPayableFor = "<%=this.cstPayableFor.ClientID%>";
		_clienttxtPayableFor = "<%=this.txtPayableFor.ClientID%>";
		_clienttxtFeeType = "<%=this.txtFeeType.ClientID%>";
		_clientcstDueDate = "<%=this.cstDueDate.ClientID%>";
		_clientcal_DueDate = "<%=this.cal_DueDate.ClientID%>";
		_clienttxtDueDate = "<%=this.txtDueDate.ClientID%>";
		_clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
		_clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
		_clientServerDate = "<%=this.hidServerDate.ClientID %>";
		_clientlblError = "<%=this.lblError.ClientID %>";
		_clientbtnSave = "<%=this.btnSave.ClientID %>";
		_clientbtnCancel = "<%=this.btnCancel.ClientID %>";
		_clientddlPayableFor = "<%=this.ddlPayableFor.ClientID %>";
		_clientddlFeeType = "<%=this.ddlFeeType.ClientID %>";
		_clientddlChequeNo = "<%=this.ddlChequeNo.ClientID %>";
		_clientcstChequeNumber = "<%=this.cstChequeNumber.ClientID %>";
		_sClientrdoFeeType = "<%=this.rdoFeeType.ClientID %>";
		_clientcstFeeType = "<%=this.cstFeeType.ClientID %>";
		_sClienbtnShow = "<%=this.btnShow.ClientID %>";
		_sClienbtnBackUp = "<%=this.btnBackUp.ClientID %>";
		_clientchkSendSMS = "<%=this.chkSendSMS.ClientID %>";
		_clienthidSendSms = "<%=this.hidSendSms.ClientID %>";
		_clientchkSendMsg = "<%=this.chkSendMessage.ClientID %>";
		_clienthidSendMsg = "<%=this.hidSendMsg.ClientID %>";
		_clientddlOtherFeeTypes = "<%=this.ddlOtherFeeTypes.ClientID %>";

		_clientchkNotApplicable = "<%=this.chkNotApplicable.ClientID %>"
		_clientrdlstDisplayFeeType = "<%=this.rdlstDisplayFeeType.ClientID %>";
		_clienthidRegNo = "<%=this.hidRegNo.ClientID %>";

		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_beginRequest(BeginReqHandler);
		prm.add_endRequest(EndReqHandler);

		function BeginReqHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement;
			if (postBackElement.id == _clientbtnSave)
				DisableButtons(true);
		}
		
		function EndReqHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement;
			if (postBackElement.id == _clientbtnSave)
			    DisableButtons(false);

			AutoSearch();
		}
		
		function DisableButtons(action) {
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function' && action)
				isPageValid = Page_ClientValidate();
			if (isPageValid) {
				if ($get(_clientbtnSave) != null)
					$get(_clientbtnSave).disabled = action;
				if ($get(_clientbtnCancel) != null)
					$get(_clientbtnCancel).disabled = action;
				if ($get(_sClienbtnShow) != null)
					$get(_sClienbtnShow).disabled = action;
				if ($get(_sClienbtnBackUp) != null)
					$get(_sClienbtnBackUp).disabled = action;
			}
		}
		
		function ValidateAmount(aSrc, args) {
			var txtAmt = $get(_clienttxtAmt).value;
			var rdoCheque = $get(_sClientrdoFeeType + '_2');

			if ($get(_clientlblError) != null)
			    $get(_clientlblError).innerHTML = "";

			if (txtAmt == "") {
			    aSrc.errormessage = document.getElementById("<%=hidAmountShouldNotBeBlank.ClientID%>").value;
				args.IsValid = false;
			}
			else if (txtAmt != "" && txtAmt <= 0 && rdoCheque != null && rdoCheque.checked == false) {
			    aSrc.errormessage = document.getElementById("<%=hidAmountShouldNotBeZero.ClientID%>").value;
				args.IsValid = false;
			}
		}
		
		function ValidateChequeNo(aSrc, args) {
			var rdoCheque = $get(_sClientrdoFeeType + '_2');
			if (rdoCheque && rdoCheque.checked) {
				var ddlChequeno = $get(_clientddlChequeNo).value;
				if (ddlChequeno == "0") {
					args.IsValid = false;
					if ($get(_clientlblError) != null)
					    $get(_clientlblError).innerHTML = "";
					$get(_clientcstChequeNumber).errormessage = document.getElementById("<%=hidChequeNumberShouldBeSelected.ClientID%>").value;
				}
			}
		}
		
		function ValidateFeeType(aSrc, args) {
			if ($get(_clienttxtFeeType) != null) {
			    var sFeetype = $get(_clienttxtFeeType).value;
			    if ($get(_clientddlOtherFeeTypes) != null) {

			        if (sFeetype.trim() == "" && $get(_clientddlOtherFeeTypes).selectedIndex == 0) {

			            if ($get(_clientlblError) != null)
			                $get(_clientlblError).innerHTML = "";

			            $get(_clientcstFeeType).errormessage = document.getElementById("<%=hidFeeTypeShouldNotBeBlank.ClientID%>").value;
			            args.IsValid = false;
			        }
			    }
			    else {
			        if (sFeetype.trim() == "" ) {

			            if ($get(_clientlblError) != null)
			                $get(_clientlblError).innerHTML = "";

			            $get(_clientcstFeeType).errormessage = document.getElementById("<%=hidFeeTypeShouldNotBeBlank.ClientID%>").value;
			            args.IsValid = false;
			        }
			    }
			}
			if ($get(_clientddlFeeType) != null) {
				if ($get(_clientddlFeeType).value == 0) {
					if ($get(_clientlblError) != null) {
						$get(_clientlblError).innerHTML = "";
	        	}
		            $get(_clientcstFeeType).errormessage = document.getElementById("<%=hidFeeTypeShouldBeSelected.ClientID%>").value;
					args.IsValid = false;
				}
			}
		}
		
		function ValidatePayableFor(aSrc, args) {
			if ($get(_clienttxtPayableFor) != null) {
				if (($get(_clienttxtPayableFor).value).trim() == "") {
					if ($get(_clientlblError) != null)
					    $get(_clientlblError).innerHTML = "";
					$get(_clientcstPayableFor).errormessage = document.getElementById("<%=hidPayableForShouldNotBeBlank.ClientID%>").value;
					args.IsValid = false;
				}
			}
			if ($get(_clientddlPayableFor) != null) {
				if ($get(_clientddlPayableFor).value == '--Select--' || $get(_clientddlPayableFor).value == 0) {
					if ($get(_clientlblError) != null)
					    $get(_clientlblError).innerHTML = "";
					$get(_clientcstPayableFor).errormessage = document.getElementById("<%=hidPayableForShouldBeSelected.ClientID%>").value;
					args.IsValid = false;
				}
			}
		}

		function ValidateDueDate(aSrc, args) {
		    if ($get(_clientchkNotApplicable).checked == false) {
		        if ($get(_clienttxtDueDate).value == "") {
		            if ($get(_clientlblError) != null)
		                $get(_clientlblError).innerHTML = "";

		            $get(_clientcstDueDate).errormessage = document.getElementById("<%=hidDueDateShouldNotBeBlank.ClientID%>").value;
		            args.IsValid = false;
                    return true
		        }
		    }

		    args.IsValid = true;
		    return false;
		}
		
		function ResetControls() {
			$get(_clientddlDivision).value = "0";
			$get(_clientddlStandard).value = "0";
		}

		function SendMessage(str) {
			var bResult = true;
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function') {
				isPageValid = Page_ClientValidate();
			}
			if (isPageValid) {
				var chkSendSMS = $get(_clientchkSendSMS);
				var SendMsg = $get(_clienthidSendSms);
				if (chkSendSMS.checked) {
					if (!window.confirm(document.getElementById("<%=hidDoYouWantToSendFollowingSMSMessage.ClientID%>").value + str))
						SendMsg.value = "N";
					else
						SendMsg.value = "Y";
				}
			}
			return bResult;
		}

		function ConfirmDelete(str, sMessage) {
			var bResult = true;
			var msg = "";
			if (str == "Y")
			    msg = document.getElementById("<%=hidAreYouSureYouWantToDeleteThisBounceChequeTransaction.ClientID%>").value;
			else
			    msg = document.getElementById("<%=hidAreYouSureYouWantToDeleteThisDebitDetails.ClientID%>").value;
			if (!window.confirm(msg))
				bResult = false;
			else
				bResult = ConfirmSendMessage(sMessage);

			return bResult;
		}
		
		function clickButton(e, buttonid) {
			var evt = e ? e : window.event;
			var bt = $get(buttonid);
			if (bt) {
				if (evt.keyCode == 13) {
					bt.click();
					return false;
				}
			}
		}
		
		function NoAction() {
			return false;
		}
		
		function blinkIt() {
			if (!document.all)
				return;
			
			var blinkElements = document.all.tags('blink');
			for (var i = 0; i < blinkElements.length; i++) {
				var blinkElement = blinkElements[i];
				blinkElement.style.visibility = (blinkElement.style.visibility == 'visible') ? 'hidden' : 'visible';
			}
		}
		
		function OpenPopup(sQueryString) {
			window.open('CopyFeeConfigurationPopup.aspx?' + sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600');
			return false;
		}

		function SendSms_Message(str) {		    
			var bResult = true;
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function')
			    isPageValid = Page_ClientValidate();
			if (isPageValid) {
			    var bValue = true;
			    bValue = ValidateZeroAmount();
			    if (bValue) {
			        ConfirmSendMessage(str);
			        bResult = true;
			    }
			    else
			        bResult = false;			    
			}
			return bResult;
		}

		function ConfirmSendMessage(str) {
			var bResult = true;
			var chkSendSMS = $get(_clientchkSendSMS);
			var SendSms = $get(_clienthidSendSms);
			var chkSendMsg = $get(_clientchkSendMsg);
			var SendMsg = $get(_clienthidSendMsg);
			if (chkSendSMS.checked && chkSendMsg.checked) {
			    if (!window.confirm(document.getElementById("<%=hidDoYouWantToSendSMSTo.ClientID%>").value.replace("%studentOfClass%", str) + '.')) {
					SendSms.value = "N";
					SendMsg.value = "N";
				}
				else {
					SendSms.value = "Y";
					SendMsg.value = "Y";
				}
			}
			else if (chkSendSMS.checked) {
			    if (!window.confirm(document.getElementById("<%=hidDoYouWantToSendSMSTo.ClientID%>").value.replace("%studentOfClass%", str) + '.'))
					SendSms.value = "N";
				else
					SendSms.value = "Y";
			}
        else if (chkSendMsg.checked) {
            if (!window.confirm(document.getElementById("<%=hidDoYouWantToSendMessageTo.ClientID %>").value.replace("%studentOfClass%", str) + '.'))
					SendMsg.value = "N";
				else
					SendMsg.value = "Y";
			}
			return bResult;
		}
		
		function OtherFeeTypeOnChange(src) {
			var txtFeeType = $get(_clienttxtFeeType);
			
			if (!txtFeeType)
				return;

			if (src.selectedIndex == 0)
				txtFeeType.disabled = false;
			else {
				txtFeeType.disabled = true;
				txtFeeType.value = "";
			}
		}

		function ValidateZeroAmount() {
		    var validationResult = true;
		    if (typeof (Page_ClientValidate) == 'function')
		        validationResult = Page_ClientValidate("");
		    if (validationResult == true) {

		        if ($get(_clientlblError) != null)
		            $get(_clientlblError).innerHTML = "";

		        var txtAmt = $get(_clienttxtAmt).value;
		        var rdoCheque = $get(_sClientrdoFeeType + '_2');
		        var txtRegNumber = $get(_clienttxtRegNo).value;
		        if (txtAmt != "" && txtAmt <= 0 && rdoCheque != null && rdoCheque.checked == false && txtRegNumber != "") {
		            return window.confirm("Are you sure you want to save this record with zero amount?")
		        }
		        else
		            return true;	       	        
		    }
		}
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            AutoSearch();
            openStudentPayFeeScreen();
        });

        function AutoSearch() {
            _clienttxtRegNumber = '#<%=txtRegNumber.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>";

            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 1);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtRegNumber.ClientID %>");
            bt = document.getElementById("<%=this.btnShow.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function openStudentPayFeeScreen() {
            if (document.getElementById(_clienthidRegNo).value != "" && (document.getElementById(_sClienbtnShow).value == "Show"))
                document.getElementById(_sClienbtnShow).click();
            document.getElementById(_clienthidRegNo).value = "";
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete all unpaid entries?');
        }

        function ConfirmDisable() {
            return confirm('Are you sure you want to disable all unpaid entries?');
        }

    </script>
</asp:Content>
