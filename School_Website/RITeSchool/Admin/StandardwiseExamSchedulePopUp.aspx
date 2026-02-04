<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardwiseExamSchedulePopup.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" Inherits="StandardwiseExamSchedulePopup"
    ValidateRequest="false" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
     <style type="text/css">
		.notice-popup-wrapper
		{
			position: absolute;
			left: 130px !important;
			top: 169px !important; 
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
		/*---- Changes for theme classes override ---*/
		input[type="text"] {
            height: 20px !important;
            padding:0 4px;
        }
        
        select {
            height: 22px !important;
            padding: 0 4px;
        }
	</style>
          <div id="overlay" class="web_dialog_overlay">
	         </div>
	      <div id="divlectcount" runat="server" class="notice-popup-wrapper" style="z-index: 5000; width: 350px; height: auto; margin-left: 180px; margin-top: 210px; background-color: white;
		                  visibility: hidden; display: none;">
		           <div class="notice-popup-title">
			             <span class="notice-popup-title-closebtn" onclick="HidePopup();">
				              <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
			             </span>
			             <h4 class="notice-popup-title-text ">
				              Copy Exam Schedule
			             </h4>
		           </div>
		           <div id="popShow" class="notice-popup-content" >
		                <table width="100%">
                                 <tr align="center" >
                                  <td>
                                    <table class="ClsBorderlight">
                                       <tr>
                                          <td>
                                                <span class="ClsLblLgnd" style="width:120px;">Select  Standards :</span>
                                          </td>
                                            <td align="center" ><asp:CheckBoxList ID="chkListClasses" TabIndex="16" runat="server" CellPadding="0" 
                                                 CellSpacing="0"  RepeatColumns="4"  
                                                 RepeatDirection="Horizontal">
                                              </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                  </td>
                                 </tr>
                                 <tr >
                                   <td align="center"> 
                                               <asp:Button ID="btnCopy" CausesValidation="false" runat="server" 
                                                            CssClass="ClsBtn" Text="Copy Schedule" Width="91px" OnClick="btnCopy_Click"/>
                                               <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="ClsBtn"/>
                                               <asp:Button ID="btnCloseDiv" runat="server" Text="Close" CssClass="ClsBtn"/>
                                   </td>
                                </tr>
                        </table>
		              </div>
	          </div>   
    <div class="MainBodyDiv" style="vertical-align: top">
        <table width="100%" cellpadding="2px" style="vertical-align: top; height: 98%">
            <tr>
                <td align="left" colspan="2" rowspan="1" style="height: 20px">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblIndentDetails" runat="server" CssClass="MainTitleHead" Font-Bold="True"   
                                    Text="Standardwise Exam Schedule" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" id="trValidationSummary" runat="server" valign="top">
                    <asp:ValidationSummary ID="valSum" ShowSummary="true" runat="server" />
                    <asp:CustomValidator ID="cstSelectionCount" runat="server" Display="None" ClientValidationFunction="CheckSelection"></asp:CustomValidator>
                    <asp:CustomValidator Display="None" ID="cst_StartDate" runat="server" ClientValidationFunction="cstStartDate"
                        SetFocusOnError="True"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ClientValidationFunction="cstStartAndEndDate"
                        SetFocusOnError="True" Display="None"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" class="td-vertical-align-top">
                    <table width="100%">
                        <tr>
                            <td align="center" id="tdMessage" runat="server" colspan="4">
                                <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False" ></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Visible="true" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    </ContentTemplate>
                              
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table width="70%" align="center">
                                    <tr>
                                        <td align="left" style="width: 8%" valign="top">
                                            <span class="ClsLblLgnd" style="font: Bold">Standard :</span>
                                        </td>
                                        <td class="ClsHilightBG" align="left" style="width: 20%">
                                            <asp:Label ID="txtStandardName" runat="server" CssClass="LblNrmlB" Text="standard :"
                                                EnableViewState="true"></asp:Label>
                                        </td>
                                        <td style="width: 2%">
                                        </td>
                                        <td align="left" style="width: 10%">
                                            <span class="ClsLblLgnd" style="font: Bold">Exam Name :</span>
                                        </td>
                                        <td class="ClsHilightBG" align="left" style="width: 20%">
                                            <asp:Label ID="lblExamName" runat="server" CssClass="LblNrmlB" Text=""  EnableViewState="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInstruction" runat="server" visible="false">
                            <td align="center" colspan="4">
                                <asp:UpdatePanel UpdateMode="always" runat="server" ID="upnlInstruct">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <td align="center" valign="middle" id="tdlblInstructions" runat="server">
                                                    <asp:Label ID="lblInstructions" runat="server" CssClass="LblNrmlB" Text="Instructions :"></asp:Label>
                                                </td>
                                                <td align="center" valign="top" id="tdtxtInstructions" runat="server">
                                                    <asp:TextBox ID="txtInstructions" runat="server" CssClass="SmlCombo" Rows="4" TextMode="MultiLine"
                                                        Width="450px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td align="center" valign="middle">
                                                    <asp:HyperLink ID="hlnkInstructions" runat="server" Font-Bold="True" ForeColor="Purple"
                                                        Visible="false" NavigateUrl="~/RITeSchool/Admin/ExamScheduleInstructionsPopUp.aspx">Add Instructions</asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:ListView ID="lstvwExam" runat="server" OnItemDataBound="lstvwExam_ItemDataBound"
                                    DataKeyNames="SubjectWize_Standard_Exam_Schedule_Id,Subject_Id" OnItemCommand="lstvwExam_ItemCommand">
                                    <LayoutTemplate>
                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                            cellspacing="1" class="GridBorder">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="center" width="5%">
                                                </th>
                                                <th align="center" width="09%">
                                                    Subject
                                                </th>
                                                <th align="center" width="10%">
                                                    Exam Type
                                                </th>
                                                <th align="center" width="22%">
                                                    Exam Date
                                                </th>
                                                <th align="center" width="3%">
                                                    Time?
                                                </th>
                                                <th runat="server" width="16%">
                                                    Start Time
                                                </th>
                                                <th id="th1" runat="server" width="16%">
                                                    End Time
                                                </th>
                                                <th id="th2" runat="server" wrap="false" width="20%">
                                                    Description
                                                </th>
                                                <th>
                                                    New
                                                </th>
                                            </tr>
                                            <tr id="trHeaderControls" runat="server" class="ClsGridHeader">
                                                <th align="center" width="5%">
                                                    <asp:CheckBox ID="chkAll" runat="server" onclick="CheckAllOrUncheckAlls()" />
                                                </th>
                                                <th align="center">
                                                </th>
                                                <th align="center">
                                                    <asp:TextBox ID="txtAllExamTypes" runat="server" CssClass="MidTxtBox" Style="width: 100px"
                                                        MaxLength="50" onchange="SelectAllControls(_clientAllExamType,'txtExamTypes')" />
                                                </th>
                                                <th align="center">
                                                    <asp:TextBox ID="calAllstartdate" CssClass="SmlCombo" runat="server" MaxLength="15"
                                                        onchange="SelectAllControls(_clientAllExamDate,'calstartdate')"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cstartdate" runat="server" Control="calAllstartdate" Format="dd mmm yyyy" Culture="en"
                                                        ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="please select valid exam date." />
                                                </th>
                                                <th align="center" width="5%">
                                                    <asp:CheckBox ID="chkAllTimeApplicable" runat="server" onclick="SelectAllControls(_clientIsTimeApplicable,'chkIsTimeApplicable')" />
                                                </th>
                                                <th id="th3" align="center" runat="server">
                                                    <asp:DropDownList ID="ddlAllStartHr" runat="server" onchange="SelectAllControls(_clientAllStartHr,'ddlstarthr')">
                                                        <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                        <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                        <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                        <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                        <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                        <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                        <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                        <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                        <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                        <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                        <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                        <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                        <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                        <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                        <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                        <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                        <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                        <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                        <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                        <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                        <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                        <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                        <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlAllStartMin" runat="server" onchange="SelectAllControls(_clientAllStartMin,'ddlstartmin')">
                                                        <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                        <asp:ListItem Value="55">55</asp:ListItem>
                                                    </asp:DropDownList>
                                                </th>
                                                <th id="th4" align="center" runat="server">
                                                    <asp:DropDownList ID="ddlAllEndHr" runat="server" onchange="SelectAllControls(_clientAllEndHr,'ddlendhr')">
                                                        <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                        <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                        <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                        <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                        <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                        <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                        <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                        <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                        <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                        <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                        <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                        <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                        <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                        <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                        <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                        <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                        <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                        <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                        <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                        <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                        <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                        <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                        <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlAllEndMin" runat="server" onchange="SelectAllControls(_clientAllEndMin,'ddlendmin')">
                                                        <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                        <asp:ListItem Value="55">55</asp:ListItem>
                                                    </asp:DropDownList>
                                                </th>
                                                <th id="th5" runat="server" align="center">
                                                    <asp:TextBox ID="txtAllDescription" CssClass="MidTxtBox" Style="width: 155px" runat="server"
                                                        MaxLength="500" onchange="SelectAllControls(_clientAllDescription,'txtDescription')"></asp:TextBox>
                                                </th>
                                                <th>
                                                </th>
                                            </tr>
                                            <tr id="tr4" runat="server" style="height: 2px; background-color: Black">
                                                <th colspan="9">
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkRow" runat="server" onclick="EnableDisableControls" />
                                            </td>
                                            <td id="tdSubject" runat="server" align="left">
                                                <asp:Label ID="lblSubjects" runat="server" Text='<%#Eval("Subject_Name")%>' CssClass="ClspaddingL"> </asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtExamTypes" runat="server" Text='<%#Eval("TestType")%>' Style="width: 100px"
                                                    MaxLength="50" CssClass="MidTxtBox">
                                                </asp:TextBox>
                                            </td>
                                            <td align="center">
                                               <asp:TextBox ID="calstartdate" CssClass="SmlCombo" runat="server" MaxLength="15" 
                                                    Text='<%#Convert.ToDateTime(Eval("Start_DateTime")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'></asp:TextBox>
                                                <rjs:PopCalendar ID="cstartdate" runat="server" Control="calstartdate" Format="dd mmm yyyy" Culture="en"
                                                    ShowErrorMessage="false" ShowWeekend="true" InvalidDateMessage="please select valid exam date." />                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsTimeApplicable" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlstarthr" runat="server">
                                                    <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                    <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                    <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                    <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                    <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                    <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                    <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                    <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                    <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                    <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                    <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                    <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                    <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                    <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                    <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                    <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                    <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                    <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                    <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                    <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                    <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                    <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                    <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlstartmin" runat="server">
                                                    <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                    <asp:ListItem Value="05">05</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="35">35</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="45">45</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="55">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlendhr" runat="server">
                                                    <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                    <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                    <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                    <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                    <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                    <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                    <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                    <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                    <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                    <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                    <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                    <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                    <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                    <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                    <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                    <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                    <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                    <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                    <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                    <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                    <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                    <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                    <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlendmin" runat="server">
                                                    <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                    <asp:ListItem Value="05">05</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="35">35</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="45">45</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="55">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" CssClass="MidTxtBox"
                                                    Text='<%#Eval("Description")%>' Style="width: 155px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnNew" runat="server" CssClass="ClsBtn" Style="width: 20px;" CausesValidation="false"
                                                    CommandName="NEW" Text="+" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblSubjects" runat="server" Text='<%#Eval("Subject_Name")%>' CssClass="ClspaddingL"> </asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtExamTypes" runat="server" Text='<%#Eval("TestType")%>' Style="width: 100px"
                                                    MaxLength="50" CssClass="MidTxtBox">
                                                </asp:TextBox>
                                            </td>
                                            <td align="center">
                                              <asp:TextBox ID="calstartdate" CssClass="SmlCombo" runat="server" MaxLength="15"
                                                    Text='<%#Convert.ToDateTime(Eval("Start_DateTime")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'></asp:TextBox>
                                                <rjs:PopCalendar ID="cstartdate" runat="server" Control="calstartdate" Format="dd mmm yyyy" Culture="en"
                                                    ShowErrorMessage="false" ShowWeekend="true" />                                         
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsTimeApplicable" runat="server" />
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlstarthr" runat="server">
                                                    <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                    <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                    <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                    <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                    <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                    <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                    <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                    <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                    <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                    <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                    <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                    <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                    <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                    <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                    <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                    <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                    <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                    <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                    <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                    <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                    <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                    <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                    <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlstartmin" runat="server">
                                                    <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                    <asp:ListItem Value="05">05</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="35">35</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="45">45</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="55">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlendhr" runat="server">
                                                    <asp:ListItem Value="1 am">1 am</asp:ListItem>
                                                    <asp:ListItem Value="2 am">2 am</asp:ListItem>
                                                    <asp:ListItem Value="3 am">3 am</asp:ListItem>
                                                    <asp:ListItem Value="4 am">4 am</asp:ListItem>
                                                    <asp:ListItem Value="5 am">5 am</asp:ListItem>
                                                    <asp:ListItem Value="6 am">6 am</asp:ListItem>
                                                    <asp:ListItem Value="7 am">7 am</asp:ListItem>
                                                    <asp:ListItem Selected="true" Value="8 am">8 am</asp:ListItem>
                                                    <asp:ListItem Value="9 am">9 am</asp:ListItem>
                                                    <asp:ListItem Value="10 am">10 am</asp:ListItem>
                                                    <asp:ListItem Value="11 am">11 am</asp:ListItem>
                                                    <asp:ListItem Value="12 pm">12 pm</asp:ListItem>
                                                    <asp:ListItem Value="1 pm">1 pm</asp:ListItem>
                                                    <asp:ListItem Value="2 pm">2 pm</asp:ListItem>
                                                    <asp:ListItem Value="3 pm">3 pm</asp:ListItem>
                                                    <asp:ListItem Value="4 pm">4 pm</asp:ListItem>
                                                    <asp:ListItem Value="5 pm">5 pm</asp:ListItem>
                                                    <asp:ListItem Value="6 pm">6 pm</asp:ListItem>
                                                    <asp:ListItem Value="7 pm">7 pm</asp:ListItem>
                                                    <asp:ListItem Value="8 pm">8 pm</asp:ListItem>
                                                    <asp:ListItem Value="9 pm">9 pm</asp:ListItem>
                                                    <asp:ListItem Value="10 pm">10 pm</asp:ListItem>
                                                    <asp:ListItem Value="11 pm">11 pm</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlendmin" runat="server">
                                                    <asp:ListItem Selected="true" Value="00">00</asp:ListItem>
                                                    <asp:ListItem Value="05">05</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                    <asp:ListItem Value="35">35</asp:ListItem>
                                                    <asp:ListItem Value="40">40</asp:ListItem>
                                                    <asp:ListItem Value="45">45</asp:ListItem>
                                                    <asp:ListItem Value="50">50</asp:ListItem>
                                                    <asp:ListItem Value="55">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" CssClass="MidTxtBox"
                                                    Text='<%#Eval("Description")%>' Style="width: 155px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnNew" runat="server" CssClass="ClsBtn" Style="width: 20px;" Text="+"
                                                    CausesValidation="false" CommandName="NEW" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <table>
                                            <tr>
                                                <td align="center" class="LblNoRecord" style="width: 500px">
                                                    No Record found.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" disable-page="true"
                                    CausesValidation="true" />
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" disable-page="true"
                                    CausesValidation="true" onclick="btnSubmit_Click" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="False"
                                    OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr id="CopyExamDiv" runat="server" align="center" visible="false" >
                         <td >
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                   <ContentTemplate> 
                                        <asp:Button ID="btnCopyToShowDiv" CausesValidation="false" runat="server" 
                                                   CssClass="ClsBtn" Text="Copy Schedule" Width="94px"/>
                                   </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        </tr>

                        </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidYearEndDate" runat="server" />
                            <asp:HiddenField ID="hidYearStartDate" runat="server" />
                            <asp:HiddenField ID="hidStandardId" runat="server" />
                            <asp:HiddenField ID="hidActionFlag" runat="server" />
                            <asp:HiddenField ID="hidStandardTestId" runat="server" />
                            <asp:HiddenField ID="hidStandardwiseExamScheduleId" Value="0" runat="server" />
                            <asp:HiddenField ID="hidStandardName" runat="server" />
                            <asp:HiddenField ID="hidIsConfig" runat="server" />
                            <asp:HiddenField ID="hidLastRowId" runat="server" Value="-1" />
                            <asp:HiddenField ID="hidTempExamScheduleId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidExamId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidItemCount" runat="server" Value="0" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientExamListview = "<%=this.lstvwExam.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _clientIsTimeApplicable = _clientExamListview + "_chkAllTimeApplicable"
        _clientAllStartHr = _clientExamListview + "_ddlAllStartHr"
        _clientAllStartMin = _clientExamListview + "_ddlAllStartMin"
        _clientAllEndHr = _clientExamListview + "_ddlAllEndHr"
        _clientAllEndMin = _clientExamListview + "_ddlAllEndMin"
        _clientAllExamType = _clientExamListview + "_txtAllExamTypes"
        _clientAllExamDate = _clientExamListview + "_calAllstartdate"
        _clientAllCheckbobes = _clientExamListview + "_chkAll";
        _clientAllDescription = _clientExamListview + "_txtAllDescription";
        _clientcAllstartdate = _clientExamListview + "_cAllstartdate"
        _clienthlnkInstructions = "<%=this.hlnkInstructions.ClientID %>";
        _clientcstSelectionCount = "<%=this.cstSelectionCount.ClientID %>";
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
        _clientCstStartDate = "<%=this.cst_StartDate.ClientID %>";
        _clientcst_StartAndEndDate = "<%=this.cst_StartAndEndDate.ClientID %>";
        _clientErrorMEssage = "<%=this.lblErrorMsg.ClientID %>"
        _clientMode = "<%=this.hidActionFlag.ClientID %>"
        _clientCheckBoxList= "<%=this.chkListClasses.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);
        prm.add_beginRequest(beginRequestHandler)

      function ShowPopup() {
            var x, y, tt_ovr_
            $("#overlay").show();
            $("#divlectcount").hide();
            var cssstyle = $get("<%=this.divlectcount.ClientID %>").style
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
          $get("<%=this.divlectcount.ClientID %>").style.visibility = "hidden"
          $get("<%=this.divlectcount.ClientID %>").style.display = "none"
          return false
        }
      function  SelectCheckBox(){
          var flag
          var sScheduleMessage= "Exam schedule will be copied for the subjects which are applicable to selected standard(s) and it will override existing schedule of standards."
          var sCheckBoxMessage = "Please select standard(s) to copy Schedule"
          if ($get("<%= this.lblMessage.ClientID %>") != null)
               $get("<%= this.lblMessage.ClientID %>").style.display = "none";
          if (CheckForAllCheckBoxes()) 
             flag=confirm(sScheduleMessage)
          else {
              alert(sCheckBoxMessage)
              flag = false;
          }
          return flag;
        }

      function CheckForAllCheckBoxes() {
          var flag =false
          var checkStd = $("[id*='_chkListClasses_']");
          var listLenght = $("[id*='_chkListClasses_']").length; 
          for (var i = 0; i < listLenght; i++) {
              var chk = checkStd[i];
              if (chk.checked) {
                  flag =true
                  break;
              }
          }
          return flag
      }

     function ClearCheckBxList() {
         var checkStd = $("[id*='_chkListClasses_']");
         var listLenght = $("[id*='_chkListClasses_']").length;
         for (var i = 0; i < listLenght; i++) {
             var chk = checkStd[i];
             if (chk.checked) {
                 chk.checked=false
             }
         }
         ShowPopup()
         return false;
     }
      function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (document.getElementById(_clienthlnkInstructions) != null)
                document.getElementById(_clienthlnkInstructions).disabled = false
        }

        function beginRequestHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (document.getElementById(_clienthlnkInstructions) != null)
                document.getElementById(_clienthlnkInstructions).disabled = true
            else
                document.getElementById(_clienthlnkInstructions).disabled = false
        }

        var isPageValid = true;
        var mode = document.getElementById(_clientMode).value;

        // This method is used to change value of all checked respective control 
        // on change of header controls state/value.        
        function SelectAllControls(allType, objid) {
            var chk;
            var dropDownList;
            var i = 0;
            if (allType == _clientExamListview + "_calAllstartdate")
                HideMessage(true);

            if (i < 10)
                dropDownList = document.getElementById(_clientExamListview + "_ctrl" + i + "_" + objid)
            else
                dropDownList = document.getElementById(_clientExamListview + "_ctrl" + i + "_" + objid)

            while (dropDownList != null) {
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")

                if (chk.checked) {
                    if (allType == _clientExamListview + "_calAllstartdate")
                        dropDownList.value = ToDate(document.getElementById(allType).value);
                    else if (allType == _clientcAllstartdate)
                        dropDownList.value = ToDate(document.getElementById(allType).value);
                    else if (allType == _clientIsTimeApplicable) {
                        dropDownList.checked = document.getElementById(allType).checked;
                        DisableTimeControls(!dropDownList.checked, i);
                    }
                    else if (allType == _clientAllExamType || allType == _clientAllDescription)
                        dropDownList.value = document.getElementById(allType).value;
                    else {
                        if (document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable").checked)
                            dropDownList.value = document.getElementById(allType).value;
                    }
                }
                i = i + 1;
                if (i < 10)
                    dropDownList = document.getElementById(_clientExamListview + "_ctrl" + i + "_" + objid)
                else
                    dropDownList = document.getElementById(_clientExamListview + "_ctrl" + i + "_" + objid)
            }
        }

        // This method is used to check/uncheck and enable/disable checkboxes/all controls respectively.
        function CheckAllOrUncheckAlls() {
            var checkAll = document.getElementById(_clientAllCheckbobes).checked;
            var chk
            var i = 0;

            if (i < 10)
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            else
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")

            while (chk != null) {
                chk.checked = checkAll;
                DisableAllControls(!chk.checked, i, true);
                i = i + 1;
                if (i < 10)
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
                else
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            }
        }

        function closewindow() {
            document.getElementById(_clientbtnSave).disabled = true;
            document.getElementById(_clientbtnClose).disabled = true;
            window.close();
        }

        // This method is used for following purposes :-
        // 1. To check whether atleast on checkbox is checked.
        // 2. To check whether exam date is blank.
        // 3. Display confirmation message at the time of delete operation.
        function CheckSelection(oSrc, args) {
            var chk
            var isSelected = false;
            var i = 0;
            var subject
            var examDate
            var isValid = true;
            var startMin
            var endMin
            var sMsg = "";
            if (i < 10)
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            else
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")

            while (chk != null) {
                if (chk.checked) {
                    isSelected = true;
                    examDate = document.getElementById(_clientExamListview + "_ctrl" + i + "_calstartdate").value.trim();
                    if (convertvaliddate(examDate) == "") {
                        subject = document.getElementById(_clientExamListview + "_ctrl" + i + "_lblSubjects").innerHTML;
                        if (sMsg.match(subject) == null)
                            sMsg = sMsg + subject + ", ";
                    }
                }
                i = i + 1;
                if (i < 10)
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
                else
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            }
            if (!isSelected) {
                if (mode == "EDIT") {
                    if (!confirm("Are you sure you want to delete exam's schedule?"))
                        isValid = false;
                }
                else {
                    HideMessage(false);
                    document.getElementById(_clientErrorMEssage).style.display = "none";
                    document.getElementById(_clientcstSelectionCount).errormessage = "Atleast one subject should be selected.";
                    isValid = false;
                }
            }
            else if (sMsg != "") {
                HideMessage(false);
                sMsg = sMsg.substring(0, sMsg.length - 1);
                document.getElementById(_clientErrorMEssage).style.display = "none";
                document.getElementById(_clientcstSelectionCount).errormessage = "Exam Date(s) should be selected. Subjects : " + sMsg;
                isValid = false;
            }
            if (isValid) {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }

        // This method is used to check whether given date is valid or not.
        function convertvaliddate(date) {

            var bits;
            if (date != "") {
                if (date.match("-") != null) {
                    bits = date.split("-");
                }
                if (bits != undefined) {
                    bits[1] = bits[1].toLowerCase();
                    var month = 0, day = bits[0], year = bits[2];
                    for (var i = 0; i < 12; i++) {
                        if (bits[1] == months[i]) {
                            month = i + 1;
                        }
                    }
                    var newdate = month + "/" + day + "/" + year;
                    return newdate;
                }
                else
                    return "";
            }
            else
                return "";
        }

        function ToDate(date) {

            var bits;
            if (date != "") {
                if (date.match("-") != null)
                    bits = date.split("-");
                if (bits != undefined) {
                    bits[1] = bits[1].toLowerCase();
                    var month = 0, day = bits[0], year = bits[2];
                    for (var i = 0; i < 12; i++) {
                        if (bits[1] == months[i])
                            month = i + 1;
                    }
                    var newdate = date;
                    return newdate;
                }
                else
                    return "";
            }
            else
                return "";
        }

        // This method is used to disable all the available controls of unchecked row. 
        function DisableAll() {
            var chk
            var i = 0;
            var dt2 = new Date();
            if (i < 10)
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            else
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            while (chk != null) {
                if (!chk.checked)
                    DisableAllControls(true, i, true);
                i = i + 1;
                if (i < 10)
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
                else
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            }
        }

        function DisableAllControls(action, i) {
            document.getElementById(_clientExamListview + "_ctrl" + i + "_txtExamTypes").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_calstartdate").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_cstartdate").enabled = !action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_txtDescription").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_btnNew").disabled = action;
            if (action) {
                document.getElementById(_clientExamListview + "_ctrl" + i + "_txtExamTypes").value = "";
                document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable").checked = false;
                document.getElementById(_clientExamListview + "_ctrl" + i + "_txtDescription").value = "";
            }
            else
                document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable").checked = true;

            DisableTimeControls(action, i);
        }

        // Display instruction popup.
        function ShowInstructions(sQryStr) {
            if ((document.getElementById(_clienthlnkInstructions) == null) || (document.getElementById(_clienthlnkInstructions) == "") || (document.getElementById(_clienthlnkInstructions).disabled))
                return false;

            window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=300');
            return false;
        }

        DisableAll();

        // Enable/Disable selected row.
        function EnableDisableControls(listIndex) {
            var chk = document.getElementById(_clientExamListview + "_ctrl" + listIndex + "_chkRow");
            if (!chk.checked)
                DisableAllControls(true, listIndex);
            else
                DisableAllControls(false, listIndex);
        }

        //Enable/Disable time controls according to selected IsTimeApplicable checkbox.
        function EnableDisableTimeControls(listIndex) {
            var chk = document.getElementById(_clientExamListview + "_ctrl" + listIndex + "_chkIsTimeApplicable");
            if (!chk.checked)
                DisableTimeControls(true, listIndex);
            else
                DisableTimeControls(false, listIndex);
        }

        function DisableTimeControls(action, i) {
            document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstarthr").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstartmin").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendhr").disabled = action;
            document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendmin").disabled = action;
            if (action) {
                document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstarthr").value = "8 am";
                document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstartmin").value = "00";
                document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendhr").value = "8 am";
                document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendmin").value = "00";
            }
        }

        // check whether selected dates are in current academic year.
        function cstStartDate(aSrc, args) {
            var examDate
            var dtStartDate;
            var sMsg = "";
            var chk
            var subject
            var i = 0;
            if (i < 10)
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            else
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")

            while (chk != null) {
                if (chk.checked) {
                    examDate = document.getElementById(_clientExamListview + "_ctrl" + i + "_calstartdate");
                    if (document.all) {
                        dtStartDate = new Date(examDate.value.replace(/-/g, ' '));
                    }
                    else {
                        dtStartDate = new Date(examDate.value.replace(/-/g, ' '));
                    }
                    var dtYearStartDate = new Date(document.getElementById(_clientYearStartDate).value);
                    var dtYearEndDate = new Date(document.getElementById(_clientYearEndDate).value);
                    if ((dtStartDate < dtYearStartDate) || (dtStartDate > dtYearEndDate)) {
                        var strStartYear = getDateString(dtYearStartDate);
                        var strEndYear = getDateString(dtYearEndDate);
                        subject = document.getElementById(_clientExamListview + "_ctrl" + i + "_lblSubjects").innerHTML;
                        if (sMsg.match(subject) == null)
                            sMsg = sMsg + subject + ", ";
                    }
                }
                i = i + 1;
                if (i < 10)
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
                else
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkRow")
            }
            if (sMsg != "") {
                HideMessage(false);
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientErrorMEssage).style.display = "none";
                document.getElementById(_clientCstStartDate).errormessage = "Exam Date must be between " + strStartYear + " and " + strEndYear + ". subjects : " + sMsg;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function getDateString(obj) {
            var strDate = obj.getDate() + "-";
            var strMonth = parseInt(obj.getMonth()) + 1;
            strMonth = getMonthName(strMonth);
            strDate = strDate + strMonth + "-";
            strDate = strDate + obj.getFullYear();
            return strDate;
        }

        function getMonthName(month) {
            switch (month) {
                case 1:
                    return "Jan";
                    break;

                case 2:
                    return "Feb";
                    break;

                case 3:
                    return "March";
                    break;

                case 4:
                    return "April";
                    break;

                case 5:
                    return "May";
                    break;

                case 6:
                    return "June";
                    break;

                case 7:
                    return "july";
                    break;

                case 8:
                    return "Aug";
                    break;

                case 9:
                    return "Sep";
                    break;

                case 10:
                    return "Oct";
                    break;

                case 11:
                    return "Nov";
                    break;

                case 12:
                    return "Dec";
                    break;
            }
        }

        // Check whether exam end time is greater than start not or not.
        function cstStartAndEndDate(aSrc, args) {
            var dtStartDate;
            var dtEndDate;
            var isTimeApplicable
            var sMsg = "";
            var isValid = true;
            var chk
            var i = 0;
            if (i < 10)
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable")
            else
                chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable")

            while (chk != null) {
                if (chk.checked) {
                    var subject = subject = document.getElementById(_clientExamListview + "_ctrl" + i + "_lblSubjects").innerHTML;
                    var STime = document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstarthr").value.split(" ");
                    var SMin = document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlstartmin").value;
                    var ETime = document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendhr").value.split(" ");
                    var EMin = document.getElementById(_clientExamListview + "_ctrl" + i + "_ddlendmin").value;
                    var examDate = document.getElementById(_clientExamListview + "_ctrl" + i + "_calstartdate").value.trim();

                    if (document.all) {
                        if (isNaN(new Date(examDate.replace('-', ' '))))                        
                            isValid = false;
                    }
                    else {
                        if (isNaN(new Date(examDate.replace(/-/g, ' '))))
                            isValid = false;
                    }

                    if (isValid) {
                        if (document.all) {
                            dtStartDate = new Date(examDate.replace('-', ' ') + " " + STime[0] + ":" + SMin + " " + STime[1]);
                            dtEndDate = new Date(examDate.replace('-', ' ') + " " + ETime[0] + ":" + EMin + " " + ETime[1]);                           
                        }
                        else {                            
                            dtStartDate = new Date(convertdate(examDate) + " " + STime[0] + ":" + SMin + " " + STime[1]);
                            dtEndDate = new Date(convertdate(examDate) + " " + ETime[0] + ":" + EMin + " " + ETime[1]);
                        }
                        if (dtStartDate != null && dtEndDate != null) {
                            if (!(dtStartDate < dtEndDate) && sMsg.match(subject) == null) {
                                sMsg = sMsg + subject + ", ";
                            }
                        }
                    }
                }
                i = i + 1;
                if (i < 10)
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable")
                else
                    chk = document.getElementById(_clientExamListview + "_ctrl" + i + "_chkIsTimeApplicable")
            }
            if (sMsg != "") {
                HideMessage(false);
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientErrorMEssage).style.display = "none";
                document.getElementById(_clientcst_StartAndEndDate).errormessage = "End time must be greater than start time. Subject(s) : " + sMsg;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function HideMessage(action) {
            if (action) {
                $get("<%=this.trValidationSummary.ClientID %>").style.display = "none";
                $get("<%=this.trValidationSummary.ClientID %>").style.visibility = "hidden";
            }
            else {
                $get("<%=this.trValidationSummary.ClientID %>").style.display = "";
                $get("<%=this.trValidationSummary.ClientID %>").style.visibility = "visible";
            }
        }
        HideMessage(true);

        // Disable button on click of save button.
        function DisableButtons() {
            if ($get("<%= this.lblMessage.ClientID %>") != null)
                $get("<%= this.lblMessage.ClientID %>").style.display = "none";
            if (!isPageValid) {
                if (typeof (Page_ClientValidate) == 'function')
                    isPageValid = Page_ClientValidate();
                if (isPageValid) {
                    if (document.getElementById(_clientbtnSave) != null) {
                        document.getElementById(_clientbtnSave).disabled = true;
                        if (document.getElementById(_clientbtnClose) != null)
                            document.getElementById(_clientbtnClose).disabled = true;
                        __doPostBack(document.getElementById(_clientbtnSave).name, '');
                    }
                }
                else {
                    if ($get("<%=this.lblErrorMsg.ClientID %>") != null)
                        $get("<%=this.lblErrorMsg.ClientID %>").style.visibility = "hidden";
                    if ($get("<%=this.lblMessage.ClientID %>") != null)
                        $get("<%=this.lblMessage.ClientID %>").style.visibility = "hidden";
                }
            }
        }
        function OnGridKeyUpNumber(obj, decimalPlaces, allowNegative, e) {
            extractNumber(obj, decimalPlaces, allowNegative);
            UpDownKeyPress(obj.id, e);
        }

        
    </script>
</asp:Content>
