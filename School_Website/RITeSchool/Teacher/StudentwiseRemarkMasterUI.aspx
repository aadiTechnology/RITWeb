<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentwiseRemarkMasterUI.aspx.cs" Inherits="StudentwiseRemarkMasterUI"
    ValidateRequest="false" ViewStateMode="Disabled"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
    <div id="divPopup"  style="display:none;">
        <span style="font-weight:bold;color:Black;font-size:medium;font-family:Rockwell"><i class="fa fa-spinner fa-spin progress-spinner"></i>&nbsp;&nbsp;We are saving current Progress Remarks details. Please wait..</span>
    </div>
        <table width="98%" align="center">
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" ViewStateMode="Enabled" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnShowReport" />
                            <asp:PostBackTrigger ControlID="btnExport" />
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
                                ShowSummary="true" ViewStateMode="Enabled"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnShowReport" />
                            <asp:PostBackTrigger ControlID="btnExport" />
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
                                                            EnableViewState="False" Width="150px"></asp:Label>
                                                    </td>
                                                    <td id="tdTeacherList" runat="server" style="width: 290px">
                                                        <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                            OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" ViewStateMode="Enabled">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">&nbsp;*</span>
                                                        <asp:CompareValidator ID="cmp_Name" runat="server" ControlToValidate="cmbTeachers"
                                                            Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                                            ValueToCompare='0' ValidationGroup="SaveRemark"></asp:CompareValidator>
                                                    </td>
                                                    <td width="10%">
                                                    </td>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text="Student Name :"
                                                            EnableViewState="False" Width="150px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UPnlStudent" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbStudents" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                                    OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged" ViewStateMode="Enabled">
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
                                                            OnSelectedIndexChanged="cmbTermName_SelectedIndexChanged" ViewStateMode="Enabled">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="center" colspan="2">
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:UpdatePanel ID="UPnllblNorecord" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblNorecord" runat="server" CssClass="LblNoRecord" Visible="False" ViewStateMode="Enabled"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnShowReport"/>
                                                                <asp:PostBackTrigger ControlID="btnExport" />
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
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                                <tr runat="server" id="trdtPagetDtPgCnt" align="center">
                                                    <td valign="top">
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="updatePanelLib" ChildrenAsTriggers="false">
                                                            <ContentTemplate>
                                                                <table cellpadding="0" cellspacing="2" align="center" width="1000px">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <table cellspacing="2" width="850px">
                                                                                <tr>
                                                                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4; width: 145px">
                                                                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Suggested Adjectives
                                                                                            :</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                                        <span class="LblSmlV">Attentive, Capable, Careful, Cheerful, Confident, Cooperative,
                                                                                            Courteous, Creative, Dynamic, Eager, Energetic, Generous, Hardworking, Helpful,
                                                                                            Honest, Imaginative, Independent, Industrious, Motivated, Organized Outgoing, Pleasant,
                                                                                            Polite, Resourceful, Sincere, Unique.</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4; width: 145px">
                                                                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Suggested Adverbs
                                                                                            :</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                                        <span class="LblSmlV">Always, Commonly, Consistently, Daily, Frequently, Monthly, Never,
                                                                                            Occasionally, Often, Rarely, Regularly Typically, Usually, Weekly.</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4">
                                                                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">...</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                                        <span class="LblSmlV">Click on the button available for each student and remark type
                                                                                            to add configured Remark Templates.</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4">
                                                                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Note :</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                                        <span class="LblSmlV">After specific interval of time entered data will be saved automatically.</span>
                                                                                    </td>
                                                                                </tr>
                                                                                 <tr>
                                                                                    <td align="center" class="ClsBorderlight " style="background-color: #ffffc4">
                                                                                        <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Note :</span>
                                                                                    </td>
                                                                                    <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                                        <span class="LblSmlV">User can not change or update any data once summative exam is published.</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr id="trIsPassedAndPromted" runat="server" visible="false">
                                                                        <td align="center" class="ClsBorderlight" style="background-color: #ffffc4;width:160px">
                                                                            <span class="LblNrmlB" style="font-weight: bold; padding: 2px 5px;">Is Passed And Promoted?</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight" style="padding: 4px 5px;">
                                                                            <span class="LblSmlV">Select if student is passed and promoted to next standard.</span>
                                                                        </td>
                                                                    </tr>--%>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-left: 7%">
                                                                            <table>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <span class="ClsLblLgnd">
                                                                                            <asp:Label runat="server" ID="Label3" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                                                        </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <span style="background-color: Red; height: 20px; border: 1px solid black; width: 20px;">
                                                                                            <img src="../images/spacer.gif" width="20px" height="10px" />
                                                                                        </span>
                                                                                    </td>
                                                                                    <td class="ClsTextNormal" style="font-weight: bold">
                                                                                        <asp:Label runat="server" ID="Label4" Text="<%$ Resources:LocalizedResources, LeftStudents %>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPagerUser" runat="server" viewstatemode="Enabled">
                                                                        <td align="center">
                                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwStudentRemarkDetails" ViewStateMode="Enabled">
                                                                                <Fields>
                                                                                    <asp:TemplatePagerField>
                                                                                        <PagerTemplate>
                                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                                CssClass="LblNrmlB" ViewStateMode="Enabled"/>
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
                                                                    <tr id="trListView" runat="server" viewstatemode="Enabled">
                                                                        <td align="center" id="tdMainListView" runat="server">
                                                                        <div id="divStudentRemarkDetails" runat ="server"  style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                                                                            color: #333; overflow: auto; height: 500px;  margin-left: 1px" viewstatemode="Enabled">
                                                                            <asp:ListView ID="lstvwStudentRemarkDetails" runat="server" OnItemDataBound="lstvwStudentRemarkDetails_ItemDataBound"
                                                                                DataKeyNames="YearwiseStudentId,StandardDivisionId,StudentwiseRemarkId,StudentName,FName,MName,LName,SalutationId"
                                                                                OnItemCommand="lstvwStudentRemarkDetails_ItemCommand" ViewStateMode="Enabled">
                                                                                <LayoutTemplate>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <table id="tblDataPager" runat="server">
                                                                                <tr id="trDataPager" runat="server" class="ClsBorderPager" viewstatemode="Enabled">
                                                                                    <td align="center" id="tdPgr" runat="server" viewstatemode="Enabled">
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentRemarkDetails"
                                                                                            PageSize="20" ViewStateMode="Enabled">
                                                                                            <Fields>
                                                                                                <asp:TemplatePagerField>
                                                                                                    <PagerTemplate>
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged" ViewStateMode="Enabled">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td align="right" class="LblNormal">
                                                                                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" ViewStateMode="Enabled"/>
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
                                                                    <tr id="trNorecordFound" runat="server" visible="false" viewstatemode="Enabled">
                                                                        <td style="height: 10px;" align="center">
                                                                            <asp:Label ID="lblNoRcrdFnd" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                                                Text="No Record Found." EnableViewState="False" Width=" 800px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4" align="center">
                                                                            <asp:HiddenField ID="hidRollNo" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidRemarkNameList" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidRemarkListCount" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidStudentwiaseRemarkListCount" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidPageNo" runat="server" Value="1" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidListviewPageRowCnt" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidStdDivId" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidTextChanged" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidcmbStudentValue" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidcmbTeacherValue" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidcmbTermValue" runat="server" Value="0" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidRemarkLength" runat="server" ViewStateMode="Enabled"/>
                                                                            <asp:HiddenField ID="hidTeacherId" runat="server" ViewStateMode="Enabled"/>    
                                                                            <asp:HiddenField ID="hidTestId" runat="server" ViewStateMode="Enabled"/>    
                                                                             <asp:HiddenField ID="hidIsPrimary" runat="server" ViewStateMode="Enabled"/>    
                                                                            <asp:HiddenField ID="hidIsPreprimaryStandard" runat="server" ViewStateMode="Enabled" Value="0"/>
                                                                            <asp:CustomValidator ID="cstMaxLengthValidator" runat="server" ValidationGroup="SaveRemark"
                                                                                Display="None" SetFocusOnError="true" ClientValidationFunction="MaxLengthValidation"></asp:CustomValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnShowReport" />
                                                                <asp:PostBackTrigger ControlID="btnExport" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:PostBackTrigger ControlID="DtPgDropDown" />
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwStudentRemarkDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnPopupSave" EventName="Click" />                                                           
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
                                                                <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="Back" PostBackUrl="~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx" ViewStateMode="Enabled"/>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" ValidationGroup="SaveRemark"
                                                                    OnClick="btnSave_Click" ViewStateMode="Enabled"/>
                                                                <asp:Button ID="btnShowReport" runat="server" CssClass="ClsBtn" Width="154px" Text="Save and Show Report"
                                                                    ValidationGroup="SaveRemark" OnClick="btnShowReport_Click" ViewStateMode="Enabled" Visible="false"/>
                                                                <asp:Button ID="btnExport" runat="server" CssClass="ClsBtn" Text="Export" ValidationGroup="SaveRemark"
                                                                    OnClick="btnExport_Click" ViewStateMode="Enabled"/>
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
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTimer" runat="server">
                                                <ContentTemplate>
                                                    <asp:Timer ID="timer" runat="server" Interval="300000" Enabled="false" 
                                                        ontick="timer_Tick" ViewStateMode="Enabled">
                                                    </asp:Timer>                                                    
                                                    <asp:HiddenField ID="hidTimerStart" runat="server" ViewStateMode="Enabled"/>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
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
    </div>
    <div id="divTemplates" runat="server" style="visibility: hidden; display: none; position: fixed;
        margin: 0px; padding: 0px; width: 760px; height: 430px; border-width: 1px; left: 5px;
        top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
        background-color: white;" viewstatemode="Enabled">
        <div class="StudentWiseRemarkMasterPop">
            <div style="font-size: 12px; width: 350px; letter-spacing: 1px; padding-left: 8px;
                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                Select Appropriate Template
            </div>
            <span style="cursor: hand; float: right;" onclick="javascript:HidePopup();">
                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                    border="0" />
            </span>
        </div>
        <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
            color: #333; overflow: auto; height: 380px; width: 750px; margin-left: 1px" id="Div5">
            <asp:UpdatePanel ID="updSacLeave" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                        vertical-align: top">
                        <tr>
                            <td>
                                <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                    <tr>
                                        <td align="center">
                                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Student Name :</span>
                                                                </td>
                                                                <td class="ClsHilightBGB">
                                                                    <asp:Label CssClass="ClsLabel" runat="server" ViewStateMode="Enabled" ID="lblStudName"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Remark Category:</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbRemarksOnDiv" runat="server" AutoPostBack="true" CssClass="LrgCombo"
                                                                        OnSelectedIndexChanged="cmbRemarksOnDiv_SelectedIndexChanged" TabIndex="0" ViewStateMode="Enabled">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight">
                                                                    <span class="ClsLabel">Grades:</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbGradesOnDiv" runat="server" AutoPostBack="true" CssClass="LrgCombo"
                                                                        OnSelectedIndexChanged="cmbGradesOnDiv_SelectedIndexChanged" TabIndex="0" ViewStateMode="Enabled">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:ListView ID="lstvwTemplates" runat="server" DataKeyNames="TemplateId" OnItemDataBound="lstvwTemplates_ItemDataBound"
                                                                        OnSorting="lstvwTemplates_Sorting" ViewStateMode="Enabled">
                                                                        <LayoutTemplate>
                                                                            <table cellpadding="0" cellspacing="1" align="center" width="100%" id="tblPagerUserDetails"
                                                                                runat="server">
                                                                            </table>
                                                                            <table align="center" width="710px" runat="server" id="tblStaffInfo" style="color: #333333"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader" viewstatemode="Enabled">
                                                                                    <th align="center" style="width: 30px">
                                                                                    </th>
                                                                                    <th align="left" style="width: 600px; padding-left: 5px;">
                                                                                        <asp:LinkButton ID="lnkRemarkTemplate" runat="server" CommandName="Sort" CommandArgument="Template"
                                                                                            CausesValidation="false" ForeColor="Black"> Remark Template </asp:LinkButton>
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server" viewstatemode="Enabled">
                                                                                </tr>
                                                                                <tr class="ClsBorderPager" id="trDataPager" ViewStateMode="Enabled">
                                                                                    <td colspan="7">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="chkTemplate" runat="Server" ViewStateMode="Enabled"/>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px">
                                                                                    <asp:Label ID="lblTemplate" ViewStateMode="Enabled" runat="server" Text='<%# Eval("Template") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                                                <td align="center">
                                                                                    <asp:CheckBox ID="chkTemplate" runat="Server" ViewStateMode="Enabled"/>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 5px;">
                                                                                    <asp:Label ID="lblTemplate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Template") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table width="700px" align="center">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" style="text-align: center">
                                                                                        <span>No record found.</span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                    <asp:HiddenField ID="hidFname" runat="server" ViewStateMode="Enabled"/>
                                                                    <asp:HiddenField ID="hidMname" runat="server" ViewStateMode="Enabled"/>
                                                                    <asp:HiddenField ID="hidLname" runat="server" ViewStateMode="Enabled"/>
                                                                    <asp:HiddenField ID="hidSalutationId" runat="server" ViewStateMode="Enabled"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="bottom">
                                            <asp:HiddenField ID="hidSortExpression" runat="server" ViewStateMode="Enabled"/>
                                            <asp:HiddenField ID="hidSelectedRemarkLength" runat="server" ViewStateMode="Enabled"/>
                                            <asp:HiddenField ID="hidSortDirection" runat="server" ViewStateMode="Enabled"/>
                                            <asp:HiddenField ID="hidTextBoxId" runat="server" ViewStateMode="Enabled"/>
                                            <asp:Button ID="btnPopupSave" runat="server" Text="Select" CssClass="ClsBtn" OnClick="btnPopupSave_Click"
                                                OnClientClick="if(!CheckAtleastOneSelected()) return false;" PostBackUrl="~/RITeSchool/Teacher/StudentwiseRemarkMasterUI.aspx" ViewStateMode="Enabled"/>
                                            <asp:Button ID="btnClosePopUp" runat="server" Text="Close" CssClass="ClsBtnMid" CausesValidation="false"
                                                Width="75px" OnClientClick="javascript:HidePopup();return false;" ViewStateMode="Enabled"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnPopupSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lstvwStudentRemarkDetails" EventName="ItemCommand" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:HiddenField ID="hidRemarkTemplateKeywordsJSON" runat="server" ViewStateMode="Enabled" Value=""/>
        </div>
        <script language="javascript" type="text/javascript">
           


        </script>
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
        _clienthidTextChanged = "<%= this.hidTextChanged.ClientID %>"
        _clienthidcmbStudentValue = "<%= this.hidcmbStudentValue.ClientID %>"
        _clienthidcmbTeacherValue = "<%= this.hidcmbTeacherValue.ClientID %>"
        _clienthidcmbTermValue = "<%= this.hidcmbTermValue.ClientID %>"
        _clientcmbStudents = "<%= this.cmbStudents.ClientID %>"
        _clientcmbTeachers = "<%= this.cmbTeachers.ClientID %>"
        _clientcmbTermName = "<%= this.cmbTermName.ClientID %>"
        _clienthidRemarkLength = "<%=this.hidRemarkLength.ClientID %>"
        _clientTimer = "<%=this.timer.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"

        _clientlstvwTemplates = "<%=this.lstvwTemplates.ClientID %>"
        var _chkTemplate = '_chkTemplate';
        var _ctrl = '_ctrl';
        _clienthidSelectedRemarkLength = "<%=this.hidSelectedRemarkLength.ClientID %>"

        function CheckAtleastOneSelected() {
            var iRowCount = 0;
            var bSelected = false;
            var RemarksLength = parseInt($get(_clienthidSelectedRemarkLength).value);
            var MaxRemarksLength = $get(_clienthidRemarkLength).value;
            var chk = document.getElementById(_clientlstvwTemplates + _ctrl + iRowCount + _chkTemplate);
            while (chk != null) {
                if (chk.checked) {
                    bSelected = true;
                    break;
                }
                iRowCount++;
                chk = $get(_clientlstvwTemplates + _ctrl + iRowCount + _chkTemplate);
            }
            bSelected = false;
            $("input:checkbox[id*='_chkTemplate']").each(
                    function () {
                        if (this.checked) {
                            if (!bSelected)
                                bSelected = this.checked;
                            RemarksLength += parseInt($get(this.id.replace("_chkTemplate", "_lblTemplate")).innerHTML.length);
                        }
                    }
                );

            if (!bSelected)
                alert("At least one Remark Template should be selected.");
            else if (RemarksLength > MaxRemarksLength) {
                alert("Remarks length should not be more than " + MaxRemarksLength + ".");
                bSelected = false;
            }

            return bSelected;
        }

        function HidePopup1() {
            $get("<%=this.divTemplates.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divTemplates.ClientID %>").style.display = "none"
            return false
        }

        function MessageAlertPopUp() {
            var bIsValid = true;
            var iRowCount = 0;
            var bSelected = false;
            var chk = document.getElementById(_clientlstvwTemplates + _ctrl + iRowCount + _chkTemplate);
            while (chk != null) {
                if (chk.checked) {
                    bSelected = true;
                    break;
                }
                iRowCount++;
                chk = $get(_clientlstvwTemplates + _ctrl + iRowCount + _chkTemplate);
            }
            if (bSelected) {
                if (window.confirm("Data has been changed, with this action selected Remark Template on current page will get lost. Do you want to continue?"))
                    bIsValid = true
                else
                    bIsValid = false
            }
            return bIsValid
        }

        //To confirm whethere remark text is changed or not.
        function IsTextChange(cmbObject) {
            document.getElementById(_clienthidTextChanged).value = 1;

        }

        function alertMsgLength(e, txtRemarks) {
            if (txtRemarks.value.length > parseInt($get(_clienthidRemarkLength).value)) {
                txtRemarks.value = txtRemarks.value.substring(0, parseInt($get(_clienthidRemarkLength).value));
                return false;
            }
            if ($get(txtRemarks.id.replace("_txt", "_lbl")) != null) {
                updateTextBoxCounter(txtRemarks);
            }
        }

        function updateTextBoxCounter(txtRemarks) {
            var unicodeFlag = 0;
            var extraChars = 0;
            var msgCount = 0;
            var sMsgTxt = txtRemarks.value;
            var TotalCount = 0;
            var i = 0;
            for (; (i < sMsgTxt.length); i++) {
                if ((sMsgTxt.charAt(i) >= '0') && (sMsgTxt.charAt(i) <= '9')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'A') && (sMsgTxt.charAt(i) <= 'Z')) {
                }
                else if ((sMsgTxt.charAt(i) >= 'a') && (sMsgTxt.charAt(i) <= 'z')) {
                }
                else if (sMsgTxt.charAt(i) == '@') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA3) {
                }
                else if (sMsgTxt.charAt(i) == '$') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xEC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF2) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC7) {
                }
                else if (sMsgTxt.charAt(i) == '\r') {
                }
                else if (sMsgTxt.charAt(i) == '\n') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x394) {
                }
                else if (sMsgTxt.charAt(i) == '_') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x393) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39B) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A9) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A8) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A3) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x398) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39E) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC9) {
                }
                else if (sMsgTxt.charAt(i) == ' ') {
                }
                else if (sMsgTxt.charAt(i) == '!') {
                }
                else if (sMsgTxt.charAt(i) == '\"') {
                }
                else if (sMsgTxt.charAt(i) == '#') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA4) {
                }
                else if (sMsgTxt.charAt(i) == '%') {
                }
                else if (sMsgTxt.charAt(i) == '&') {
                }
                else if (sMsgTxt.charAt(i) == '\'') {
                }
                else if (sMsgTxt.charAt(i) == '(') {
                }
                else if (sMsgTxt.charAt(i) == ')') {
                }
                else if (sMsgTxt.charAt(i) == '*') {
                }
                else if (sMsgTxt.charAt(i) == '+') {
                }
                else if (sMsgTxt.charAt(i) == ',') {
                }
                else if (sMsgTxt.charAt(i) == '-') {
                }
                else if (sMsgTxt.charAt(i) == '.') {
                }
                else if (sMsgTxt.charAt(i) == '/') {
                }
                else if (sMsgTxt.charAt(i) == ':') {
                }
                else if (sMsgTxt.charAt(i) == ';') {
                }
                else if (sMsgTxt.charAt(i) == '<') {
                }
                else if (sMsgTxt.charAt(i) == '=') {
                }
                else if (sMsgTxt.charAt(i) == '>') {
                }
                else if (sMsgTxt.charAt(i) == '?') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xC4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xD1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xDC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xA7) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xBF) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF6) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xF1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xFC) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0xE0) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x391) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x392) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x395) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x396) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x397) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x399) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39A) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39C) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39D) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x39F) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A1) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A4) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A5) {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x3A7) {
                }
                else if (sMsgTxt.charAt(i) == '^') {
                }
                else if (sMsgTxt.charAt(i) == '{') {
                }
                else if (sMsgTxt.charAt(i) == '}') {
                }
                else if (sMsgTxt.charAt(i) == '\\') {
                }
                else if (sMsgTxt.charAt(i) == '[') {
                }
                else if (sMsgTxt.charAt(i) == '~') {
                }
                else if (sMsgTxt.charAt(i) == ']') {
                }
                else if (sMsgTxt.charAt(i) == '|') {
                }
                else if (sMsgTxt.charCodeAt(i) == 0x20AC) {
                }
                else {
                    unicodeFlag = 1;
                }
                TotalCount = parseInt(i + extraChars);
                if (TotalCount >= parseInt($get(_clienthidRemarkLength).value)) {
                    sMsgTxt = sMsgTxt.substring(0, i);
                    break;
                }
            }
            if (TotalCount >= parseInt($get(_clienthidRemarkLength).value))
                txtRemarks.value = sMsgTxt;
            if (unicodeFlag) {
                msgCount = sMsgTxt.length;
                if (msgCount <= 70) {
                    msgCount = 1;
                }
                else {
                    msgCount += (67 - 1);
                    msgCount -= (msgCount % 67);
                    msgCount /= 67;
                }
                $get(txtRemarks.id.replace("_txt", "_lbl")).innerHTML = "&nbsp;(" + (parseInt($get(_clienthidRemarkLength).value) - sMsgTxt.length) + ")";
            }
            else {
                msgCount = sMsgTxt.length + extraChars;
                if (msgCount <= 160) {
                    msgCount = 1;
                }
                else {
                    msgCount += (153 - 1);
                    msgCount -= (msgCount % 153);
                    msgCount /= 153;
                }
                $get(txtRemarks.id.replace("_txt", "_lbl")).innerHTML = "&nbsp;(" + (parseInt($get(_clienthidRemarkLength).value) - sMsgTxt.length) + ")";
            }
        }

        function MessageAlert(ddlCntObj) {
            var bIsValid = true;
            if (document.getElementById(_clienthidTextChanged).value != "") {
                if (window.confirm("Data modification for last minute is auto saved but entered progress remarks after auto save on the current page will get lost with your action. Do you want to continue?"))
                    bIsValid = true
                else {
                    if (document.getElementById(ddlCntObj).type == "select-one") {
                        document.getElementById(ddlCntObj).value = document.getElementById(_clienthidPageNo).value
                        document.getElementById(_clientcmbStudents).value = document.getElementById(_clienthidcmbStudentValue).value;
                        document.getElementById(_clientcmbTeachers).value = document.getElementById(_clienthidcmbTeacherValue).value;
                        document.getElementById(_clientcmbTermName).value = document.getElementById(_clienthidcmbTermValue).value;
                    }
                    bIsValid = false
                }
            }
            return bIsValid
        }

        function OpenPopup(btnShowPopup) {
            _clientdivTemplates = "<%=this.divTemplates.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divTemplates.ClientID %>").style
            var width = 750
            var height = 380
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
            setTotal();
        }

        function HidePopup() {
            $get("<%=this.divTemplates.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divTemplates.ClientID %>").style.display = "none"
            return false
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
                    ///Max length of remark should be 300
                    if (TextBoxName.value.length > $get(_clienthidRemarkLength).value) {
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
                oSrc.errormessage = "Remark length should not be greater than " + $get(_clienthidRemarkLength).value + " characters for the Roll No.(s) :<br/> " + sRollNoList;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }


        function CheckUncheckAllCheckBoxes(chkSelect) {
            $("input:checkbox[id*=chkIsPassedAndPromoted]").attr('checked', chkSelect.checked);
        }

        function ConfirmSave() {
            var bResult = true
            if (!window.confirm('This Action will show only saved details. Do you want to continue?')) {
                bResult = false
            }
            return bResult
        }


        function OpenWaitingPopup() {
            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Action - Save", visible: false, modal: true, resizable: false, width: '580px', actions: [] }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        function ClosePopup() {
            $("#divPopup").data("kendoWindow").close();
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnSave)
                ClosePopup();
        }
        function beginRequestHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
            if (postBackElement.id == _clientTimer || postBackElement.id == _clientbtnSave)
                OpenWaitingPopup();
        }

    </script>
    <script language="javascript" type="text/javascript">

        _cltdivTemplates = "<%=this.divTemplates.ClientID %>"

        var _totalWinHeight;
        var _adjWinHeight;
        var _rightFooterPos;
        var _bottomFooterPos;
        var _adjWinWidth;
        var _rightPosition;

        window.onresize = setTotal;
        window.onscroll = setTotal;
        window.onload = setTotal;

        function setTotal() {
            _totalWinHeight = document.body.scrollHeight;
            _adjWinHeight = _totalWinHeight; //-608;
            _adjWinWidth = document.body.scrollWidth;

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivTemplates).style.height);
                document.getElementById(_cltdivTemplates).style.top = _rightFooterPos;
            }

            if (document.getElementById(_cltdivTemplates) != null) {
                _rightPosition = parseInt(screen.width / 2) - parseInt(parseInt(document.getElementById(_cltdivTemplates).style.width) / 2);
                //_rightPosition = document.body.clientWidth - parseInt(document.getElementById(_cltdivTemplates).style.width) - 350;
                document.getElementById(_cltdivTemplates).style.left = _rightPosition;
            }

            window_onscroll();
        }

        function window_onscroll() {
            if (document.body.scrollTop <= _adjWinHeight) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.top = document.body.scrollTop + _rightFooterPos;
                }
            }

            if (document.body.scrollLeft <= _adjWinWidth) {
                if (document.getElementById(_cltdivTemplates) != null) {
                    document.getElementById(_cltdivTemplates).style.left = document.body.scrollLeft + _rightPosition;
                }
            }
        }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
