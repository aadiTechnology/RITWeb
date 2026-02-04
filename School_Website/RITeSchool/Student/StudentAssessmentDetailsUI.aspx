<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentAssessmentDetailsUI.aspx.cs" Inherits="StudentAssessmentDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                            <asp:RequiredFieldValidator ID="ReqTestName" runat="server" Display="None" ControlToValidate="ddlTest"
                                InitialValue="0" ErrorMessage="Test Name should be selected."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequCategory" runat="server" Display="None" ControlToValidate="ddlCategory"
                                InitialValue="0" ErrorMessage="Category should be selected."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqStudentName" runat="server" Display="None" ControlToValidate="ddlStudentName"
                                InitialValue="0" ErrorMessage="Student Name should be selected."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqFavColour" runat="server" Display="None" ControlToValidate="txtFavColor"
                                ErrorMessage="Favourite Colour should not be blank."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqFavFood" runat="server" Display="None" ControlToValidate="txtFavFood"
                                ErrorMessage="Favourite Food should not be blank."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqFavSport" runat="server" Display="None" ControlToValidate="txtFavSport"
                                ErrorMessage="Favourite Sport should not be blank."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ReqFavSub" runat="server" Display="None" ControlToValidate="txtFavSubject"
                                ErrorMessage="Favourite Subject should not be blank."></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cstGrades" runat="server" ClientValidationFunction="ValidatesGrades"
                                Display="None" ErrorMessage="Grade should be assigned for all aspects."></asp:CustomValidator>
                            <asp:CustomValidator ID="cstComment" runat="server" ClientValidationFunction="ValidateComment"
                                 Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="cstcategorywiseComment" runat="server" ClientValidationFunction="ValidateCategorywiseComment"
                                 Display="None" ErrorMessage="Comment should not be blank for parameters."></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <div style="float: right; vertical-align: top;">
                                    <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:UpdatePanel ID="up11" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                                        ForeColor="Blue"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 120px;">
                                            <asp:Label ID="lblAcademicYear" runat="server" CssClass="ClsLabel" Text="Academic Year :"></asp:Label>
                                        </td>
                                        <td id="Td8" align="left" runat="server">
                                            <asp:DropDownList ID="ddlAcademicYear" runat="server" CssClass="MidCombo" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight" style="width: 120px;">
                                            <asp:Label ID="lblTest" runat="server" CssClass="ClsLabel" Text="Test Name :"></asp:Label>
                                        </td>
                                        <td id="Td1" align="left" runat="server">
                                            <asp:DropDownList ID="ddlTest" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTest_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight">
                                            <asp:Label ID="lblCategory" runat="server" CssClass="ClsLabel" Text="Category :"></asp:Label>
                                        </td>
                                        <td id="Td2" align="left" runat="server">      
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                           <ContentTemplate>                                     
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="LrgCombo" Width="300px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Self Assessment</asp:ListItem>
                                                        <asp:ListItem Value="2">Peer Feedback</asp:ListItem>
                                                        <asp:ListItem Value="3">Parent Feedback</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>       
                                                </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>                                         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight">
                                            <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text="Student Name :"></asp:Label>
                                        </td>
                                        <td id="Td3" align="left" runat="server">
                                            <asp:UpdatePanel ID="up3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlStudentName" runat="server" CssClass="LrgCombo" Width="300px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStudentName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*</span>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:UpdatePanel ID="up32" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table style="width: 100%">
                                                        <tr id="trFavColor" runat="server">
                                                            <td align="center" class="ClsBorderlight" style="width: 120px">
                                                                <asp:Label ID="lblColor" runat="server" CssClass="ClsLabel" Text="Favourite Color :"></asp:Label>
                                                            </td>
                                                            <td id="Td4" align="left" runat="server">
                                                                <asp:TextBox ID="txtFavColor" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trFavFood" runat="server">
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="lblFood" runat="server" CssClass="ClsLabel" Text="Favourite Food :"></asp:Label>
                                                            </td>
                                                            <td id="Td5" align="left" runat="server">
                                                                <asp:TextBox ID="txtFavFood" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trFavSport" runat="server">
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="lblSport" runat="server" CssClass="ClsLabel" Text="Favourite Sport :"></asp:Label>
                                                            </td>
                                                            <td id="Td6" align="left" runat="server">
                                                                <asp:TextBox ID="txtFavSport" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr id="trFavSubject" runat="server">
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="lblSubject" runat="server" CssClass="ClsLabel" Text="Favourite Subject :"></asp:Label>
                                                            </td>
                                                            <td id="Td7" align="left" runat="server">
                                                                <asp:TextBox ID="txtFavSubject" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="up22" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <table width="100%">                                    
                                        <tr id="TrStudentFavList" runat="server">
                                            <td valign="top" align="center">
                                                <asp:ListView ID="lstvwStudentFavDetails" runat="server" DataKeyNames="ParameterId" OnItemDataBound="lstvwStudentFavDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="Table2" align="center" width="80%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trFavHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" class="paddingLR" width="100px">
                                                                    Serial No.
                                                                </th>
                                                                <th align="left" class="paddingLR" width="500px">
                                                                    Parameter
                                                                </th>
                                                                <th  align="center" class="paddingLR">
                                                                    Comment
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                            <td align="center">
                                                                <asp:Label ID="lblSerialNoFav" runat="server" Style="float:inherit" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblAspects" runat="server" CssClass="ClsLabel" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="LrgTxtBox" Width="700px" Text='<%#Eval("Comment") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                        </asp:ListView>
                                            </td>
                                        </tr>
                                    </table>
                                   </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="up4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="center" width="80%">                                            
                                            <tr id="trLegend" runat="server" visible="false">
                                                <td align="left">
                                                    <table>
                                                        <tr>
                                                            <td class="ClsLblLgnd" style="width:75px;">
                                                                <span style="font-weight:bold;">Legend - </span>
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Proficient - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Proficient.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                            <td style="width:20px">
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Progressing - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Progressing.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                            <td style="width:20px">
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Beginner - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Beginner.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trLegendforPPSN" runat="server" visible="false">
                                                <td align="left">
                                                    <table>
                                                        <tr>
                                                            <td class="ClsLblLgnd" style="width:75px;">
                                                                <span style="font-weight:bold;">Legend - </span>
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Always - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Proficient.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                            <td style="width:20px">
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Sometimes - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Progressing.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                            <td style="width:20px">
                                                            </td>
                                                            <td align="left">
                                                                <span style="font-weight:bold;">Never - </span>
                                                            </td>
                                                            <td>
                                                                <img src="../images/Beginner.jpg" style="height:20px; width:20px;" />
                                                            </td>                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trAspectsHeader" runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Font-Bold="true" Text="Aspects :"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="center">
                                                    <asp:ListView ID="lstvwStudentAssessmentDetails" runat="server" DataKeyNames="ParameterId"
                                                        OnItemDataBound="lstvwStudentAssessmentDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table id="Table1" width="100%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" class="paddingLR" width="100px">
                                                                        Serial No.
                                                                    </th>
                                                                    <th align="left" class="paddingLR">
                                                                        Aspect
                                                                    </th>
                                                                    <th align="center" class="paddingLR" width="180px">
                                                                        Grade
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="center">
                                                                    <asp:Label ID="lblSerialNo" runat="server" Style="float:inherit"
                                                                        Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAspects" runat="server" CssClass="ClsLabel" Text='<%#Eval("Aspect") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:DropDownList ID="ddlGrade" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="LblNoRecord">
                                                                No Record Found.
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                            <tr id="trCategorywiseComment" runat="server">
                                                <td valign="top" align="center">
                                                    <asp:ListView ID="lstvwCategorywiseParameters" runat="server" DataKeyNames="ParameterId" OnItemDataBound="lstvwCategorywiseParameters_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table id="Table2" align="center" width="100%" runat="server" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trFavHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" class="paddingLR" width="100px">
                                                                        Serial No.
                                                                    </th>
                                                                    <th align="left" class="paddingLR">
                                                                        Parameter
                                                                    </th>
                                                                    <th  align="center" class="paddingLR" width="300px">
                                                                        Comment
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="center">
                                                                    <asp:Label ID="lblSerialNoForCategorywiseComment" runat="server" Style="float:inherit" Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAspects" runat="server" CssClass="ClsLabel" Text='<%#Eval("Parameter") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtCategorywiseComments" runat="server" TextMode="MultiLine" CssClass="LrgTxtBox" Width="100%" Height="75px" Text='<%#Eval("CommentForCategory") %>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBtn" CausesValidation="false" />
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click" Enabled="false" />
                                        <asp:Button ID="btnCancel" Text="Clear" runat="server" CssClass="ClsBtn" CausesValidation="false"
                                            Visible="false" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="ClsBtn" OnClick="btnSubmit_Click" Enabled="false"
                                            CausesValidation="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlStudentName" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlTest" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlCategory" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:HiddenField ID="hidStudentId" runat="server" />
                                <asp:HiddenField ID="hidStdId" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">

            function ValidatesGrades(src, args) {
                var isFound = false;
                $('[id$=_ddlGrade]').each(function () {

                    if ($(this).val() == '0') {
                        isFound = true;
                        return;
                    }
                })

                if (isFound) {
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }

            
             function ValidateComment(oSrc, args) {

                var found = false
                var maxlengthfound = false

                $('[id$=txtComments]').each(function () {
                    if ($(this).val().trim() != "") {


                        if ($(this).val().trim().length > 200) {
                            maxlengthfound = true;
                            $(this).css('background-color', '#FFFF00')
                        }
                        else
                            $(this).css('background-color', '#ffffff')
                    }
                    else {
                        found = true;
                        $(this).css('background-color', '#ffffff')
                    }
                });

                if (found) {
                    oSrc.errormessage = "Comment should be added for all parameters.";
                    args.IsValid = false;
                    return true;
                }
                else if (maxlengthfound) {
                    oSrc.errormessage = "Comment length should not exceed 200 characters for yellow color parameter."
                    args.IsValid = false
                    return true;
                }

               args.IsValid = true
               return false
           }

           function ValidateCategorywiseComment(src, args) {
               var isFound = false;
               $('[id$=_txtCategorywiseComments]').each(function () {

                   if ($(this).val() == "") {
                       isFound = true;
                       return;
                   }
               })

               if (isFound) {
                   args.IsValid = false;
                   return true;
               }
               else {
                   args.IsValid = true;
                   return false;
               }
           }

        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
