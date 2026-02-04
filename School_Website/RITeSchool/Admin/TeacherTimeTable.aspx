<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="TeacherTimeTable.aspx.cs" EnableEventValidation="false" Inherits="TeacherTimeTable" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
		.notice-popup-wrapper
		{
			position: absolute;
			left: 50%;
			top: 50%;
			border: solid 2px darkgreen;
			background-color: lightyellow;
			font-family: Tahoma;
			z-index:2000;
		}
		.notice-popup-title-text
		{
			margin: 0;
			text-align: left;
			font-size: 14px;
		}
		
		.notice-popup-title-closebtn
		{
			float: right;
			cursor: pointer;
		}
		
		.notice-popup-content
		{
			padding: 15px;
			text-align: left;
			vertical-align: top;
			overflow: auto;
		}
		.web_dialog_overlay
		{
			position: absolute;
			height: 100%;
			width: 100%;
			background:#000333 transparent;
			opacity: .15;
			filter: alpha(opacity=15);
			-moz-opacity: .15;
			z-index:1001;
			display: none;
		}
		.lblerrorr {
		    color:#000333;
		    font-size:9pt;
		    font-weight: normal;
		   
		}
		
	</style>
          <div id="overlay" class="web_dialog_overlay">
	         </div>
	                 <div id="divlectcount" runat="server" class="notice-popup-wrapper" style="z-index: 5000;
		                   width: 400px; height: auto; margin: -65px 0 0 -150px; background-color: white;
		                  visibility: hidden; display: none;">
		                       <div class="notice-popup-title">
			                        <span class="notice-popup-title-closebtn" onclick="HidePopup();">
				                     <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
			                        </span>
			                        <h4 class="notice-popup-title-text ">
				                          Increase Lecture Limit
			                       </h4>
		                          </div>
		                             <div id="popShow" class="notice-popup-content" >
			                                      <table width="100%">
                                                     <tr>
                                                       <div style="padding-left: 7px; font-size: 13;">
                                                            <asp:Label ID="ppErrMsg" runat="server" Visible="true"   />
                                                      </div> 
                                                      <div>
                                                            <caption style="margin-left:-104px;font-size: 13;">
                                                                <asp:Label ID="Label12" runat="server" Visible="true"  Text="Do you want to increase limit for 
                                                                   subject(s)?" />
                                                              
                                                           </caption>
                                                      </div>
                                                      </tr>
                                                         <tr align="center">
                                                            <td style="padding-left: 25px;">
                                                                 <asp:Button ID="btnIncreaseCnt" runat="server" CssClass="ClsBtn" Text="OK"  UseSubmitBehavior="false"
                                                                      ValidationGroup="Return" OnClick="btnIncreaseCnt_Click"  />
                                                                 <asp:Button ID="btnCancel" runat="server"  
                                                                      CssClass="ClsBtn" OnClientClick="javascript:HidePopup();return false;" Text="Cancel" />
                                                             </td>
                                                            </tr>
                                                       </table>
		                                            </div>
	                                            </div>
    <div class="MainBodyDiv">
        <a id="top"></a>
        <table width="95%" align="center">
            <tr>
                <td>
                    <asp:UpdatePanel  ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel4">
                        <ContentTemplate>
                            <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                                Visible="True"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID="btnIncreaseCnt" EventName="Click"/>
                             <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"/>
                        </Triggers>
                    </asp:UpdatePanel>
                    <div style="float: right">
                        <asp:Label ID="Label7" runat="server" CssClass="ClsMdtStar">* </asp:Label>
                        <asp:Label ID="lbl_Mandatory" runat="server" Font-Bold="False" ForeColor="Red" CssClass="LblNormalImg" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="show" runat="server"
                        CssClass="ClsLabel" />
                    <asp:CustomValidator ID="cstValidateLogo" ValidationGroup="show" Display="None" runat="server"
                        ClientValidationFunction="ValidateInput" ControlToValidate="cmbTeachers" CssClass="LblErrorMsg"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdateSucess" runat="server" CssClass="ClsLabelUpdate" EnableViewState="False"
                                            Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="btnIncreaseCnt" EventName="Click"/>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div runat="server" id="divErr">
                                </div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <table id="LegendTable" runat="server" align="left">
                                    <tr>
                                        <td align="left" colspan="1" style="height: 24px">                                         
                                                <span class="ClsLblLgnd">
                                                <asp:Label ID="lbl" runat="server" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                </span>
                                        </td>
                                        <td align="right" style="width: 5px;">
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                            <asp:Label ID="TextBox1" runat="server" Height="20px" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" ReadOnly="True" Width="20px" CssClass="TTNotAssignDark"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">                                          
                                                <span class="ClsTextNormal" style="font-weight:bold"> <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, LectureNotApplicable %>"></asp:Label></span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 15px; background-color: LightGrey; border-color: Black;
                                            border-style: Solid; border-width: 1px" class="ClsBorderlight">
                                            <asp:Label ID="Label11" runat="server" Height="15px" ReadOnly="True" Width="22px">&nbsp;</asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="height: 24px">
                                            <asp:Label ID="lblLegend" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, AssociatedAdditionalOptionalSubjectLectures %>" CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsHilightBGB" style="height: 24px">
                                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                ID="UpdatePanel5">
                                                <ContentTemplate>
                                                    <span id="Span1" class="ClsTextNormal" runat="server">
                                                    <asp:Label ID="lblWText" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, WeeklyTimetableFor %>"></asp:Label> </span>
                                                    <asp:Label ID="lblName" runat="server" CssClass="ClsTextNormal" Font-Bold="True"
                                                        Text="Teacher/Class Name"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
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
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                        ID="UpdatePanel3">
                        <ContentTemplate>
                            <table id="tblFilters" runat="server" width="100%">
                                <tr>
                                    <td colspan="2">
                                        <table class="ClsBorderlight" runat="server" id="tblInputFields" cellpadding="0"
                                            cellspacing="1" width="100%">
                                            <tr runat="Server" id="trStandard">
                                                <td class="ClsBorderlight" width="15%">
                                                    <span class="clsLabel" runat="server">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Teacher %>"></asp:Label>
                                                        <span class=" colonPadding"> :</span>
                                                     </span>
                                                </td>
                                                <td align="left" width="10%">
                                                    <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                        OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" colspan="2" width="80%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox runat="server" CssClass="clsLabel" ID="chkAssembly" AutoPostBack="true"
                                                                    OnCheckedChanged="chkAssembly_CheckedChanged" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" CssClass="clsLabel" ID="chkMPT" AutoPostBack="true"
                                                                    OnCheckedChanged="chkMPT_CheckedChanged" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" CssClass="clsLabel" ID="chkStayback" AutoPostBack="true"
                                                                    OnCheckedChanged="chkStayback_CheckedChanged" />
                                                            </td>
                                                             <td>
                                                                <asp:CheckBox runat="server" CssClass="clsLabel" ID="chkWeeklyTest" 
                                                                     AutoPostBack="true" oncheckedchanged="chkWeeklyTest_CheckedChanged" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trLable" runat="Server">
                                                <td class="HilightBGGray" align="center" colspan="4">
                                                    <span class="ClsHilightText">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, SelectTeacher %>"></asp:Label>
                                                        <img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB"><asp:Label ID="lblORText" runat="server" Text="<%$ Resources:LocalizedResources, OR %>"></asp:Label> </span>
                                                        <img src="../images/ArrowBlueDblNw.gif" />
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, SelectStandardDivision %>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr id="trStandardFilter" runat="Server">
                                                <td class="ClsBorderlight">
                                                    <span class="clsLabel">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                        <span class=" colonPadding"> :</span>
                                                     </span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStandard" AutoPostBack="true" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged"
                                                        runat="server" CssClass="SmlCombo">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="ClsBorderlight" style="width: 15%">
                                                    <span class="clsLabel">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                        <span class=" colonPadding"> :</span></span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDivision" runat="server" CssClass="SmlCombo" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label9" runat="server" CssClass="ClsMdtStar">*</asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table1" cellpadding="0" cellspacing="1" width="100%">
                                <tr>
                                    <td align="center" colspan="4" class="ClspaddingT">
                                        <asp:Button ID="btnShow" runat="server" Text="<%$ Resources:LocalizedResources, Show %>" CssClass="ClsBtnMid" Height="24px"
                                            CausesValidation="true" ValidationGroup="show" OnClick="btnShow_Click" />                                       
                                        <asp:HiddenField ID="hidEncrypt" runat="server" />
                                        <asp:HiddenField ID="hidTeacherId" runat="server" />
                                        <asp:HiddenField ID="hidStandardId" runat="server" />
                                        <asp:HiddenField ID="hidDivisionId" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                        <asp:HiddenField ID="hidBtnShow" runat="server" />
                            <table id="tblTimeTable" runat="server" width="100%">
                                <tr>
                                    <td align="center" colspan="2">
                                        <center>
                                            <div id="divStdTimeTable" visible="false" runat="server">
                                                <div id="GridViewScrollContainer" style="width: 845px; overflow: scroll">
                                                    <asp:GridView ID="grdStdTimeTable" Width="100%" runat="server" HorizontalAlign="Center"
                                                        EnableViewState="true" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                        BackColor="#5C6F7B" AutoGenerateColumns="False">
                                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                        </PagerStyle>
                                                        <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                            FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField DataField="Lecture_No" HeaderText="Weekdays >>" SortExpression="Lecture_No">
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlV" />
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <RowStyle CssClass="TTCells" />
                                                        <HeaderStyle CssClass="UsrGridHead" />
                                                        <AlternatingRowStyle CssClass="TTCells" />
                                                        <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <div id="divTeacherTT" visible="false" runat="server" style="width: 845px; overflow: scroll">
                                            <asp:GridView ID="grdTeacherTT" Width="100%" runat="server" AutoGenerateColumns="false"
                                                EnableViewState="true" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                BackColor="#5C6F7B" OnRowDataBound="grdTeacherTT_RowDataBound">
                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                </PagerStyle>
                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                <Columns>
                                                    <asp:BoundField DataField="Lecture_No" HeaderText="Weekdays >>" SortExpression="Lecture_No">
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlV" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="TTCells" />
                                                <HeaderStyle CssClass="UsrGridHead" />
                                                <AlternatingRowStyle CssClass="TTCells" />
                                                <EmptyDataRowStyle CssClass="LblNoRecord" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" class="ClspaddingMidT" colspan="2">
                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtn" Height="24px" 
                                            CausesValidation="true" TabIndex="1" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnReset" runat="server" Text="<%$ Resources:LocalizedResources, Reset %>" CssClass="ClsBtn" Height="24px"
                                            CausesValidation="true" TabIndex="1" OnClick="btnReset_Click" />
                                        <asp:Button ID="btnAdditionalLectures" runat="server" Text="<%$ Resources:LocalizedResources, AdditionalLectures %>"
                                            CssClass="ClsBtnExLrg" Height="24px" CausesValidation="true" TabIndex="1" />                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="ClspaddingssSmallT" style="width: 50%" runat="Server" id="tdHeadSubjectLect">
                                        <div runat="server" id="divSubjectLect" class="GrdTotal" visible="false" style="width: 90%">
                                        <span>
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, ClassSubjectLectureCount %>"></asp:Label>
                                        </span>
                                        </div>
                                    </td>
                                    <td align="center" class="ClspaddingSmallT" style="width: 50%" runat="Server" id="tdHeadAdditional">
                                        <div runat="server" id="divAdditionalLect" class="GrdTotal" visible="false" style="width: 90%">
										<asp:Label runat="server" ID="lblAdditionalLecture" Text="<%$ Resources:LocalizedResources, AdditionalLectures %>"></asp:Label>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 50%" runat="server" id="tdSubjectLect">
                                        <asp:GridView ID="grdSubjectLect" Width="90%" runat="server" AutoGenerateColumns="false"
                                            CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" BackColor="#5C6F7B"
                                            DataKeyNames="Teacher_Subject_Id,Subject_Id,Standard_Division_Id" OnRowDataBound="grdSubjectLect_RowDataBound">
                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                            </PagerStyle>
                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                            <Columns>
                                                <asp:BoundField DataField="Class_Subject" HeaderText="<%$ Resources:LocalizedResources, ClassSubjects %>">
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" Height="25px"
                                                        CssClass="LblSmlV" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Count" HeaderText="<%$ Resources:LocalizedResources, LectureCount %>">
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" Height="25px"
                                                        CssClass="LblSmlV" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                </asp:BoundField>
                                            </Columns>
                                            <RowStyle CssClass="TTCells" />
                                            <HeaderStyle CssClass="UsrGridHead" />
                                            <AlternatingRowStyle CssClass="TTCells" />
                                            <EmptyDataRowStyle CssClass="LblNoRecord" />
                                        </asp:GridView>
                                    </td>
                                    <td align="center" valign="top" style="width: 50%" runat="server" id="tdAdditional">
                                        <asp:GridView ID="grdAdditionalClasses" Width="90%" runat="server" AutoGenerateColumns="false"
                                            CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"  BackColor="#5C6F7B"
                                            DataKeyNames="SchoolTimeTableDetailId,SubjectId,WeekdayId,TeacherId" Visible="true"
                                            OnRowCommand="grdAdditionalClasses_RowCommand" OnRowDataBound="grdAdditionalClasses_RowDataBound"
                                            EmptyDataText="<%$ Resources:LocalizedResources, NoLecturesAssigned %>">
                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                            </PagerStyle>
                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                            <Columns>
                                                <asp:BoundField DataField="WeekDayName" HeaderText="<%$ Resources:LocalizedResources, WeekDay %>">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP"
                                                        Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LectureNumber" HeaderText="<%$ Resources:LocalizedResources, Lecture_hash %>">
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP"
                                                        Width="10%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ClassName" HeaderText="<%$ Resources:LocalizedResources, Class %>">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP"
                                                        Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SubjectName" HeaderText="<%$ Resources:LocalizedResources, Subject %>">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP"
                                                        Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:BoundField>
												 <asp:BoundField DataField="TeacherName" HeaderText="<%$ Resources:LocalizedResources, TeacherName %>">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" VerticalAlign="Middle" CssClass="LblSmlVP"
                                                        Width="15%" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:BoundField>
                                                <asp:ButtonField ButtonType="Image" CommandName="DELETE_LECT" HeaderText="<%$ Resources:LocalizedResources, Delete %>"
                                                    Text="<%$ Resources:LocalizedResources, Delete %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="LblSmlVP" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <RowStyle CssClass="TTCells" />
                                            <HeaderStyle CssClass="UsrGridHead" />
                                            <AlternatingRowStyle CssClass="TTCells" />
                                            <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" />
                                        </asp:GridView>
										<table width="100%">
										<tr>
											<td id="tdNoRecord" align="center" runat="server" class="LblNoRecord" visible="false">
                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, NoLecturesAssigned %>"></asp:Label></td>
										</tr
										</table>
                                    </td>
                                </tr>
								<tr>
								  <td>
								   
                                  </td>
								</tr>
                            </table>
                              
                            <asp:HiddenField ID="hidAreYouSureResetTimetable" runat="server" />
                            <asp:HiddenField ID="hidValDeleteAdditionallectures" runat="server" />
                            <asp:HiddenField ID="hidValDeleteOptionallecture" runat="server" />
                            <asp:HiddenField ID="hidWantToInrsCnt" runat="server" Value="0"/>
                            <asp:HiddenField ID="hidMaxLectCntMessage" runat="server" />
                        </ContentTemplate>
                          
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="chkAssembly" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkMPT" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkWeeklyTest" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkStayback" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="grdAdditionalClasses" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnIncreaseCnt" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click"/>
                            <asp:PostBackTrigger ControlID="cmbStandard" />
                            <asp:PostBackTrigger ControlID="cmbDivision" />    
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidHasFullAccess" runat="server" />
                	<asp:HiddenField ID="hidHidenLectures" Value="0" runat="server" />
                    <asp:HiddenField ID="hidValSelectTeacher" runat="server" />                  
                    <asp:HiddenField ID="hidValDivisionSelected" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientcmbTeachers = "<%=this.cmbTeachers.ClientID %>"
        _clientcmbStandard = "<%=this.cmbStandard.ClientID%>"
        _clientcmbDivision = "<%=this.cmbDivision.ClientID%>"
        _clientbtnSave = "<%=this.btnSave.ClientID%>"
        _clientlblError = "<%=this.lblError.ClientID%>"
        _clienthidEncryptId = "<%=this.hidEncrypt.ClientID%>"

        _clientbtnReset = "<%=this.btnReset.ClientID%>"
        _clientbtnAdditionalLectures = "<%=this.btnAdditionalLectures.ClientID%>"
        _clientcstValidateLogo = "<%=this.cstValidateLogo.ClientID %>"
        _clientcstlbl = "<%=this.ppErrMsg.ClientID %>"

        function HidePopUp(e) {
            $("#overlay").hide();
            $("#divlectcount").show();
            if (document.getElementById(_clientbtnSave) != null)
                document.getElementById(_clientbtnSave).Enable = true

            if (document.getElementById(_clientbtnReset) != null)
                document.getElementById(_clientbtnReset).Enable = true
            if (document.getElementById(_clientbtnAdditionalLectures) != null)
                document.getElementById(_clientbtnAdditionalLectures).Enable = true
            $get("<%=this.divlectcount.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divlectcount.ClientID %>").style.display = "none"
        
        }

        function ShowPopup(e, sMessage, sAllMessage) {
            $("#overlay").show();
            $("#divlectcount").hide();
            var x, y, tt_ovr_
            var ms = sMessage.toString();
            document.getElementById('<%=ppErrMsg.ClientID %>').visible = true
            document.getElementById(_clientcstlbl).innerText = ms
            document.getElementById(_clientcstlbl).innerHTML = ms
            var cssstyle = $get("<%=this.divlectcount.ClientID %>").style
            var btnReturn = $get("<%=this.btnIncreaseCnt.ClientID %>")
            var width = 150
            var height = 80
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            // Override the z-index of the topmost wz_dragdrop.js D&D item
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
            cssstyle.zIndex = 2000;
            cssstyle.visibility = "visible";
            cssstyle.display = "block";

        }


        function HidePopup() {
            $("#overlay").hide();
            $("#divlectcount").show();
           if (document.getElementById(_clientbtnSave) != null)
                document.getElementById(_clientbtnSave).Enable = true
            if (document.getElementById(_clientbtnBack) != null)
                document.getElementById(_clientbtnBack).Enable = true
            if (document.getElementById(_clientbtnReset) != null)
                document.getElementById(_clientbtnReset).Enable = true
            if (document.getElementById(_clientbtnAdditionalLectures) != null)
                document.getElementById(_clientbtnAdditionalLectures).Enable = true
            $get("<%=this.divlectcount.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divlectcount.ClientID %>").style.display = "none"
            return false
        }
         
       
        function DisableButtons(objBtn) {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') 
                isPageValid = Page_ClientValidate()
            if (isPageValid) 
             __doPostBack(objBtn.name, '')
            GoToTop()
        }

        function ValidateInput(oSrc, args) {
            ClearLabel()
            if (document.getElementById(_clientcmbTeachers).value == "0"
                && document.getElementById(_clientcmbStandard).value == "0"
                && document.getElementById(_clientcmbDivision).value == "0"
               ) {
                document.getElementById(_clientcstValidateLogo).errormessage =
                        document.getElementById("<%=this.hidValSelectTeacher.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (document.getElementById(_clientcmbStandard).value != "0" && document.getElementById(_clientcmbDivision).value == "0") {
            document.getElementById(_clientcstValidateLogo).errormessage = document.getElementById("<%=this.hidValDivisionSelected.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function upgradeSelect() {
            scs = document.getElementsByTagName('select')
            for (i = 0; i < scs.length; i++)
                new KLSelect(scs[i])
        }

        function KLSelect(sc) {
            kls = document.createElement('div')
            kls.className = sc.className
            for (i = 0; i < sc.options.length; i++) {
                with (popt = kls.appendChild(document.createElement('p'))) {
                    appendChild(document.createTextNode(sc.options[i].firstChild.nodeValue))
                    if (sc.options[i].selected) className = 'KLOptionSelected'
                    else className = 'KLOption'
                    attachEvent('onclick', KLOptionClicked)
                    attachEvent('onmouseover', KLOptionMouseOver)
                    attachEvent('onmouseout', KLOptionMouseOut)
                }
                popt.optionValue = sc.options[i].value
            }
            input = document.createElement('input')
            input.type = 'hidden'
            input.name = sc.name
            input.value = sc.value
            kls.hidinp = input
            if (sc.form) sc.form.appendChild(input)
            else sc.parentNode.appendChild(input)
            sc.parentNode.replaceChild(kls, sc)
            return
        }

        function KLOptionClicked(e) {
            KLSelectOption(e.srcElement)
            return
        }

        function KLOptionPopUpClicked(e) {
            KLSelectOption(e.srcElement.optionNode)
            e.srcElement.className = 'KLOptionPopUpSelected'
            return
        }
        function KLSelectOption(klopt) {
            with (klopt.parentNode)
                for (i = 0; i < childNodes.length; i++)
                    childNodes[i].className = 'KLOption'
                klopt.parentNode.hidinp.value = klopt.optionValue
                klopt.className = 'KLOptionSelected'
                return
            }
            function KLOptionMouseOver(e) {
                if (e.srcElement.offsetWidth > e.srcElement.parentNode.offsetWidth) {
                    e.srcElement.textPopUp = document.createElement('p')
                    e.srcElement.textPopUp.className = (e.srcElement.className == 'KLOptionSelected') ? 'KLOptionPopUpSelected' : 'KLOptionPopUp'
                    e.srcElement.textPopUp.style.left = getClientX(e) + 'px'
                    e.srcElement.textPopUp.style.top = getClientY(e) + 'px'
                    e.srcElement.textPopUp.attachEvent('onclick', KLOptionPopUpClicked)
                    e.srcElement.textPopUp.attachEvent('onmouseout', KLOptionPopUpMouseOut)
                    e.srcElement.textPopUp.appendChild(document.createTextNode(e.srcElement.firstChild.nodeValue))
                    e.srcElement.textPopUp.optionNode = e.srcElement
                    document.body.appendChild(e.srcElement.textPopUp)
                }
            }
            function KLOptionMouseOut(e) {
                if (e.srcElement.textPopUp)
                    if (e.toElement != e.srcElement.textPopUp)
                        document.body.removeChild(e.srcElement.textPopUp)
            }
            function KLOptionPopUpMouseOut(e) {
                document.body.removeChild(e.srcElement)
            }
            function getClientX(e) {
                return e.clientX - e.offsetX - document.body.clientLeft + document.body.scrollLeft
            }
            function getClientY(e) {
                return e.clientY - e.offsetY - document.body.clientTop + document.body.scrollTop
            }
            function ConfirmReset() {
                var bResult = true
                if (!window.confirm(document.getElementById("<%=this.hidAreYouSureResetTimetable.ClientID %>").value)) {
                    bResult = false
                }
                return bResult
            }
            function OpenAdditionalClassesPopup() {
                    window.open('AdditionalClassesInTimetablePopUp.aspx?' + document.getElementById(_clienthidEncryptId).value, '_blank', 'scrollbars=yes,statusbar=no,resizable=no,top=5,left=30,width=650,height=400')
                return false
            }

            function testValue() {
                theform = document.getElementById('form')
                for (i = 0; i < theform.elements.length; i++) {
                    if (theform.elements[i].name == 'test') alert(theform.elements[i].value)
                }
            }
            function ClearLabel() {
                if (document.getElementById(_clientlblError)) {
                    document.getElementById(_clientlblError).innerText = ""
                    document.getElementById(_clientlblError).innerHTML = ""
                }
            }
            function GoToTop() {
                var str
                str = window.location.href
                var iIndex = str.lastIndexOf("/#")
                if (iIndex != -1) {
                    str = str.substr(0, iIndex)
                }
                str = str + "/#top"
               }
               function ConfirmDelete(flag) {
            	var bResult = true
            	if (flag == "teacher") {
            	    if (!window.confirm(document.getElementById("<%=this.hidValDeleteAdditionallectures.ClientID %>").value)) {
            	        bResult = false
            	    }
            	}
            	else if (flag == "student") {
            	    if (!window.confirm(document.getElementById("<%=this.hidValDeleteOptionallecture.ClientID %>").value)) {
            	        bResult = false
            	    }
            	}
            	return bResult
            }
    </script>
</asp:Content>
