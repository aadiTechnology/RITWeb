<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ParentTeacherAssociationUI.aspx.cs" Inherits="ParentTeacherAssociationUI" ViewStateMode="Disabled"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanelValidator" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
            <ContentTemplate>
                <table width="100%" align="center">
                    <tr>
                        <td align="right">
                            <span class="ClsMdtStar" runat="server" id="MandatoryMark">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:ValidationSummary ID="valPSumErrorMsg" ShowSummary="True" runat="server" CssClass="LblNormal"
                                ValidationGroup="PValidate" />
                            <asp:ValidationSummary ID="valTSumErrorMsg" ShowSummary="True" runat="server" CssClass="LblNormal"
                                ValidationGroup="TValidate" />
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="ClsMdtStar" EnableViewState="false"></asp:Label>
                            <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"
                                ValidationGroup="PValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cst_MobileNumber2" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" EnableClientScript="true" ClientValidationFunction="MobileNumber2Validation"
                                ValidationGroup="PValidate"></asp:CustomValidator>                            
                            <asp:CustomValidator ID="cstDuplicateTeacherValidation" Display="None" runat="server"
                                CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" ClientValidationFunction="DuplicateTeacherValidation"
                                ValidationGroup="TValidate"></asp:CustomValidator>
							<asp:CustomValidator ID="cstValidateDesignation" Display="None" runat="server"
                                CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" ClientValidationFunction="ValidateDesignation"
                                ValidationGroup="TValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstValidate" Display="None" runat="server" CssClass="ClsMdtStar"
                                ClientValidationFunction="DuplicateUserPValidate" Visible="true" EnableClientScript="true"
                                ValidationGroup="PValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstDuplicateParentValidation" Display="None" runat="server"
                                CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" ClientValidationFunction="DuplicateParentValidator"
                                ValidationGroup="PValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstTeacher" Display="None" runat="server" CssClass="ClsMdtStar"
                                ClientValidationFunction="DuplicateUserValidate" Visible="true" EnableClientScript="true"
                                ValidationGroup="TValidate"></asp:CustomValidator>
                            <asp:CustomValidator ID="cstManualNos" runat="server" ClientValidationFunction="CheckValidMobileNos"
                                 CssClass="LblErrorMsg" Display="None" ErrorMessage="" ValidationGroup="PValidate"></asp:CustomValidator>							
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" Font-Bold="true"
                                EnableViewState="false" Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr id="trLegend" runat="server">
                                    <td align="left" style="padding-left: 28px;">
                                        <table>
                                            <tr id="trLgndDisplayOrNot" runat="server">
                                                <td width="60px">
                                                    <asp:Label ID="lblLegend" runat="server" CssClass="ClsLblLgnd" Text="Legend : "></asp:Label>
                                                </td>
                                                <td width="20px" align="left">
                                                    <asp:Label ID="lblDefaultNoticeColor" runat="server" BackColor="#FFCCCC" Height="20px"
                                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
                                                        EnableViewState="False"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                </td>
                                                <td width="620px">                                                    
                                                    <span class="ClsLblLgnd"  >Committee member associated with the section and standard division</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table width="100%" runat="server" id="tblMain">
                                            <tr id="trSearchControlsAndLstvw" runat="server">
                                                <td align="center" valign="top">
                                                    <table width="90%" id="tblSearchAndControls" runat="server">
                                                        <tr runat="server" id="trSerchEditControls">
                                                            <td align="center">
                                                                <table runat="server" id="tblSerchEditControls" style="height: 52px; width: 500px">
                                                                    <tr>
                                                                        <td align="right" style="width: 50%">
                                                                            <asp:RadioButton ID="optTeacher" runat="server" GroupName="Filter" Checked="true"
                                                                                TabIndex="1" Text="Teacher/Admin Staff" CssClass="LblNormal" AutoPostBack="true" OnCheckedChanged="optTeacher_OnCheckedChanged" />
                                                                        </td>
                                                                        <td align="left" style="width: 50%">
                                                                            <asp:RadioButton ID="optParent" runat="server" AutoPostBack="true" GroupName="Filter"
                                                                                TabIndex="1" Text="Parent" CssClass="LblNormal" OnCheckedChanged="optParent_OnCheckedChanged" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="ClsBorderlight">
                                                                            <span class="ClsLabel">Search By Name :</span>
                                                                        </td>
                                                                        <td style="padding-left: 8px">
                                                                            <asp:TextBox ID="txtSearchByName" runat="server" CssClass="MidTxtBox" MaxLength="50"
                                                                                Width="190px" TabIndex="3"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="2">
                                                                            <asp:Button ID="btnSearch" Text="Search" CssClass="ClsBtn" runat="server" TabIndex="4"
                                                                                Width="85px" OnClick="btnSearch_Click" CausesValidation="false"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPager1" runat="server">
                                                            <td align="center">
                                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwSearchByCategory">
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
                                                        <tr id="trNorecordFoundSearch" runat="server">
                                                            <td style="height: 10px;" align="center">                                                                
                                                                    <span class="LblNoRecord" style="font:Bold;width:800px">No Record Found.</span>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trSerchListView">
                                                            <td align="center" style="border: 1">
                                                                <div id="divSearch" runat="server" style="width: 80%; height: Auto;">
                                                                    <asp:ListView ID="lstvwSearchByCategory" runat="server" OnItemDataBound="lstvwSearchByCategory_ItemDataBound"
                                                                        OnItemCommand="lstvwSearchByCategory_ItemCommand" OnSelectedIndexChanged="lstvwSearchByCategory_SelectedIndexChanged"
                                                                        OnSelectedIndexChanging="lstvwSearchByCategory_SelectedIndexChanging" DataKeyNames="DesignationId,StudentId,Id,MobileNumber1,MobileNumber2">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" id="tblSearchLstvw" runat="server" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" style="padding-left: 12px; width: 300px">
                                                                                        Name
                                                                                    </th>
                                                                                    <th align="left" style="padding-left: 12px; width: 300px" runat="server" id="tdStudentName">
                                                                                        Student Name
                                                                                    </th>
                                                                                    <th align="left" style="padding-left: 12px; width: 200px" runat="server" id="tdDesignation">
                                                                                        Designation
                                                                                    </th>
                                                                                    <th align="center" style="width: 170px" runat="server" id="tdClassName">
                                                                                        Class
                                                                                    </th>
                                                                                    <th align="center">
                                                                                        Select
                                                                                    </th>
                                                                                    <th align="center" visible="false">
                                                                                        MotherName
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                    <td id="Td21" runat="server" colspan="6  ">
                                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5">
                                                                                            <Fields>
                                                                                                <asp:TemplatePagerField>
                                                                                                    <PagerTemplate>
                                                                                                        <table width="100%" class="ClsBorderPager">
                                                                                                            <tr id="tr" runat="server">
                                                                                                                <td align="left" valign="middle">
                                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbSearchListViewPageCnt_SelectedIndexChanged">
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
                                                                            <tr id="Tr1" runat="server" class="ClsGridRow">
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblName" runat="server" CssClass="LblNormal" Text='<%#Eval("Name") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px;" runat="server" id="tdStudName">
                                                                                    <asp:Label ID="lblStudentName" runat="server" Width="200px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px" runat="server" id="tdDesigName">
                                                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdClsName">
                                                                                    <asp:Label ID="lblClassName" runat="server" Width="170px" CssClass="LblNormal" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton runat="server" ID="lnkSelect" Text="Select" CommandName="Select"
                                                                                        CommandArgument="" CausesValidation="false" ToolTip="Select" OnClick="lnkSelect_Click"
                                                                                        ImageUrl="~/RITeSchool/images/selection5.gif"></asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidUId" runat="server" />
                                                                                    <asp:HiddenField ID="hidFatherName" runat="server" Value = '<%#Eval("FatherName") %>' />
                                                                                </td>
                                                                                <td align="center" visible="false">
                                                                                    <asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr1" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblName" runat="server" CssClass="LblNormal" Text='<%#Eval("Name") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px;" runat="server" id="tdStudName">
                                                                                    <asp:Label ID="lblStudentName" runat="server" Width="200px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px" runat="server" id="tdDesigName">
                                                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdClsName">
                                                                                    <asp:Label ID="lblClassName" runat="server" Width="170px" CssClass="LblNormal" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center">
                                                                                    <asp:ImageButton runat="server" ID="lnkSelect" Text="Select" CommandName="Select"
                                                                                        CommandArgument="" CausesValidation="false" ToolTip="Select" OnClick="lnkSelect_Click"
                                                                                        ImageUrl="~/RITeSchool/images/selection5.gif"></asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidUId" runat="server" />
                                                                                    <asp:HiddenField ID="hidFatherName" runat="server" Value = '<%#Eval("FatherName") %>'/>
                                                                                </td>
                                                                                <td align="center" visible="false">
                                                                                    <asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" align="center">
                                                                                        No record found.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trTeacherControls" runat="server">
                                    <td align="center">
                                        <table width="70%">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr id="trlblTeacherDetail" runat="server">
                                                <td align="center">
                                                    <span style="font-weight: bold; color: #066; font-weight: 700; font-size: 9pt">Executive Committee (School)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top">
                                                    <table id="tblTeacherMain" runat="server" width="100%">
                                                        <tr id="trTeacherEditControls" runat="server">
                                                            <td align="center">
                                                                <table width="500px" runat="server" id="tblTeacherEditControls">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width:235px; padding-left: 5px" class="ClsBorderlight">
                                                                            <span class="LblNormal">Name :</span>
                                                                        </td>
                                                                        <td style="padding-left: 12px; height: 28px;">
                                                                            <asp:TextBox ID="txtTeacherName" runat="server" CssClass="MidTxtBox" Text="" Width="190px"
                                                                                ReadOnly="true"></asp:TextBox>                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 5px" class="ClsBorderlight">
                                                                            <span class="LblNormal">Committee Designation :</span>
                                                                        </td>
                                                                        <td style="padding-left: 12px">
                                                                            <asp:DropDownList ID="cmbTDesignation" runat="server" AppendDataBoundItems="true"
                                                                                CssClass="MidCombo" TabIndex="4" Width="190px">
                                                                            </asp:DropDownList>
																			<span class="ClsMdtStar">*</span>															
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 5px" class="ClsBorderlight">
                                                                            <span class="LblNormal">Section :</span>
                                                                        </td>
                                                                        <td style="padding-left: 12px">
                                                                            <asp:DropDownList ID="cmbSection" runat="server" TabIndex="5" CssClass="MidCombo"  Width="190px">
                                                                            <%--<asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Pre-Primary" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Primary and Secondary" Value="2"></asp:ListItem>--%>
                                                                             </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Button ID="btnSaveTeacherDetails" Text="Save" CssClass="ClsBtn" runat="server" disable-page="true"
                                                                                TabIndex="6" ValidationGroup="TValidate" OnClick="btnSaveTeacherDetails_Click" />
                                                                            <asp:Button ID="btnTCancel" Text="Cancel" CssClass="ClsBtn" runat="server" TabIndex="7" 
                                                                                CausesValidation="False" UseSubmitBehavior="False" 
                                                                                onclick="btnTCancel_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" runat="server" id="tdTeacherListView">
                                                                <asp:Panel ID="pnlTeacherDetails" runat="server" ScrollBars="None">
                                                                    <asp:ListView ID="lstvwTeacherDetails" runat="server" DataKeyNames="TeacherAssociationDetailsId,DesignationId,RelatedSection"
                                                                        OnItemCommand="lstvwTeacherDetails_ItemCommand" OnItemDataBound="lstvwTeacherDetails_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table id="tblTeacherListview" runat="server" style="color: #333333" width="100%"
                                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                <tr id="trTHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" width="360px" style="padding-left: 12px; padding-right: 12px">
                                                                                        Name
                                                                                    </th>
                                                                                    <th align="left" width="250px" style="padding-left: 12px; padding-right: 12px">
                                                                                        Committee Designation
                                                                                    </th>
                                                                                    <th align="center" width="170px" runat="server" id="thSection">
                                                                                        Section
                                                                                    </th>
                                                                                    <th align="center" style="padding-left: 12px; padding-right: 12px" width="50px" id="TeacherEdit"
                                                                                        runat="server">
                                                                                        Edit
                                                                                    </th>
                                                                                    <th align="center" style="padding-left: 12px; padding-right: 12px" width="50px" id="TeacherDelete"
                                                                                        runat="server">
                                                                                        Delete
                                                                                    </th>
                                                                                    <th align="center" visible="false">
                                                                                        TeacherId
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="trTeacher" runat="server" class="ClsGridRow">
                                                                                <td align="left" style="padding-left: 8px;">
                                                                                    <asp:Label ID="lblName" runat="server" CssClass="LblNormal" Text='<%#Eval("TeacherName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px;">
                                                                                    <asp:Label ID="lblDesignationName" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" id="tdRelatedSection" runat="server">
                                                                                    <asp:Label ID="lblRelatedSection" runat="server" CssClass="LblNormal" 
                                                                                        Text='<%#Eval("RelatedSectionName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdTimgEdit">
                                                                                    <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="TEACHER_EDIT"
                                                                                        CommandArgument='<%#Eval("TeacherAssociationDetailsId")%>' CausesValidation="false"
                                                                                        ToolTip="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"></asp:ImageButton>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdTimgDelete">
                                                                                    <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="TEACHER_DELETE"
                                                                                        OnClientClick="if(!ConfirmDelete()) return false;" CommandArgument='<%#Eval("TeacherAssociationDetailsId")%>'
                                                                                        CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete">
                                                                                    </asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidDesigId" runat="server" />
                                                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                                                    <asp:HiddenField ID="hidSectionId" runat="server" />
                                                                                </td>
                                                                                <td align="center" visible="false">
                                                                                    <asp:Label ID="lblTeacherID" runat="server" Text='<%#Eval("TeacherId") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="trTeacher" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" style="padding-left: 8px;">
                                                                                    <asp:Label ID="lblName" runat="server" CssClass="LblNormal" Text='<%#Eval("TeacherName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px;">
                                                                                    <asp:Label ID="lblDesignationName" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" id="tdRelatedSection" runat="server">
                                                                                    <asp:Label ID="lblRelatedSection" runat="server" CssClass="LblNormal"
                                                                                        Text='<%#Eval("RelatedSectionName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdTimgEdit">
                                                                                    <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="TEACHER_EDIT"
                                                                                        CommandArgument='<%#Eval("TeacherAssociationDetailsId")%>' CausesValidation="false"
                                                                                        ToolTip="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"></asp:ImageButton>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdTimgDelete">
                                                                                    <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="TEACHER_DELETE"
                                                                                        OnClientClick="if(!ConfirmDelete()) return false;" CommandArgument='<%#Eval("TeacherAssociationDetailsId")%>'
                                                                                        CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" ToolTip="Delete">
                                                                                    </asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidDesigId" runat="server" />
                                                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                                                    <asp:HiddenField ID="hidSectionId" runat="server" />
                                                                                </td>
                                                                                <td align="center" visible="false">
                                                                                    <asp:Label ID="lblTeacherID" runat="server" Text='<%#Eval("TeacherId") %>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" align="center">
                                                                                        No record found.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trNoRecordMsg" runat="server">
                                                <td style="height: 10px;" align="center">                                                    
                                                        <span class="LblNoRecord" style="font:Bold;width:800px">No Record Found.</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 70%;">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr id="trlblParentDetail" runat="server">
                                                <td align="center">
                                                    <span style="font-weight: bold; color: #066; font-weight: 700; font-size: 9pt">Executive Committee (Parents)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top">
                                                    <table width="80%" runat="server" id="tblParentMain">
                                                        <tr runat="server" id="trParentEditControls">
                                                            <td align="center">
                                                                <table width="800px" runat="server" id="tblParentEditControls" align="center">
                                                                    <tr id="trTControl">
                                                                        <td style="width: 220px; padding-left: 5px;" align="left" class="ClsBorderlight">                                                                            
                                                                                <span class="LblNormal" >Father Name :</span>
                                                                        </td>
                                                                        <td style="width: 220px;" class="Padding14">
                                                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="MidTxtBox" Text="" Width="190px"
                                                                                ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight">                                                                            
                                                                                <span class="LblNormal" >Mother Name :</span>
                                                                        </td>
                                                                        <td class="Padding14">
                                                                            <asp:TextBox ID="txtMotherName" runat="server" CssClass="MidTxtBox" Text="" Width="190px"
                                                                                ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight">
                                                                            <asp:Label ID="lblConsideredAsParent" runat="server" Text="Considered As Parent :"
                                                                                CssClass="LblNormal" EnableViewState="false"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 7px;">
                                                                            <asp:RadioButton ID="optFatherAsParent" runat="server" TabIndex="8" Text="Father"
                                                                                GroupName="Parent" CssClass="ClsLabel" Checked="true" />
                                                                            <asp:RadioButton ID="optMotherAsParent" runat="server" TabIndex="9" Text="Mother"
                                                                                GroupName="Parent" CssClass="ClsLabel" />
                                                                        </td>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight">                                                                           
                                                                                <span class="LblNormal" >Mobile No.1 :</span>
                                                                        </td>
                                                                        <td style="width: 230px;" align="left">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="width: 100px; padding-left: 12px;" align="left">
                                                                                        <asp:TextBox ID="txtMobileNumber1" MaxLength="10" TabIndex="15" runat="server" Width="75px"
                                                                                            CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" ondrop="event.returnValue=false"></asp:TextBox><span
                                                                                                class="ClsMdtStar">*</span>
                                                                                        <asp:RequiredFieldValidator ID="req_MNo1" runat="server" ControlToValidate="txtMobileNumber1"
                                                                                            CssClass="ClsLabel" Display="None" ErrorMessage="Mobile Number1 should not be blank." ValidationGroup="PValidate"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td style="width: 100px;" align="left">
                                                                                        <asp:CheckBox ID="chkMobileNumber1" TabIndex="16" runat="server" CssClass="LblNormal"
                                                                                            Text="Include" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight">
                                                                            <asp:Label ID="lblDesignation" runat="server" Text="Designation :" CssClass="LblNormal"
                                                                                EnableViewState="false"></asp:Label>
                                                                        </td>
                                                                        <td class="Padding14">
                                                                            <asp:DropDownList ID="cmbPDesignation" TabIndex="10" Width="190px" runat="server"
                                                                                AppendDataBoundItems="true" CssClass="MidCombo">
                                                                            </asp:DropDownList>
                                                                            <span class="ClsMdtStar" style="color:Red">*</span>
                                                                            <asp:RequiredFieldValidator ID="reqcmbPDdesignation" runat="server" ControlToValidate="cmbPDesignation"
                                                                                Display="None" ValidationGroup="PValidate" ErrorMessage="Designation should be selected."
                                                                                CssClass="ClsLabel" InitialValue="0"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight">                                                                            
                                                                                 <span class="LblNormal" >Mobile No.2 :</span>
                                                                        </td>
                                                                        <td style="width: 230px;" align="left">
                                                                            <table>
                                                                                <tr>
                                                                        <td style="width: 100px; padding-left: 12px;" align="left">
                                                                                        <asp:TextBox ID="txtMobileNumber2" runat="server" MaxLength="10" TabIndex="17" Width="75px"
                                                                                            CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" ondrop="event.returnValue=false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 100px;" align="left">
                                                                                        <asp:CheckBox ID="chkMobileNumber2" TabIndex="18" CssClass="LblNormal" runat="server"
                                                                                            Text="Include" />
                                                                                    </td></tr></table></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-left: 5px;" align="left" class="ClsBorderlight" rowspan="2">
                                                                            <asp:Label ID="lblContactTiming" runat="server" Text="Contact Timing :" CssClass="LblNormal"
                                                                                EnableViewState="false"></asp:Label>
                                                                        </td>
                                                                        <td class="Padding14" rowspan="2">
                                                                            <asp:TextBox ID="txtContactTiming" CssClass="MidTxtBox" runat="server" TabIndex="13"
                                                                                MaxLength="100" Text="" Width="190px" TextMode="MultiLine" Height="72px"></asp:TextBox><span
                                                                                    class="ClsMdtStar" style="color:Red">*</span>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                                                                ControlToValidate="txtContactTiming" CssClass="LblNormal" ErrorMessage="Length of Contact Timing should not exceed 100 characters."
                                                                                ValidationGroup="PValidate" ValidationExpression="^[\s\S]{0,100}$"> </asp:RegularExpressionValidator>
                                                                            <asp:RequiredFieldValidator ID="reqContactTiming" runat="server" ControlToValidate="txtContactTiming"
                                                                                Display="None" ValidationGroup="PValidate" ErrorMessage="Contact Timing should not be blank."
                                                                                CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td style="width: 220px; padding-left: 5px;" align="left" class=" ClsBorderlight"
                                                                            rowspan="2">
                                                                            <asp:Label ID="lblResidenceArea" runat="server" Text="Residence Area :" CssClass="LblNormal"
                                                                                EnableViewState="false"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 220px;" class="Padding14" rowspan="2">
                                                                            <asp:TextBox ID="txtResidenceArea" runat="server" MaxLength="150" TabIndex="14" Text=""
                                                                                Width="190px" CssClass="ExLrgTxtBox" TextMode="MultiLine" Height="72px"></asp:TextBox>
                                                                                <asp:RegularExpressionValidator ID="Regu_Vali_ResAdd" runat="server" Display="None" ControlToValidate="txtResidenceArea"
                                                                                        CssClass="LblNormal" ErrorMessage="Length of Residence Area should not exceed 200 characters."
                                                                                        ValidationGroup="PValidate" ValidationExpression="^[\s\S]{0,200}$"> </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4" align="center">
                                                                            <asp:Button ID="btnSaveParentDetails" Text="Save" CssClass="ClsBtn" runat="server" disable-page="true"
                                                                                TabIndex="20" ValidationGroup="PValidate" OnClick="btnSaveParentDetails_Click" />
                                                                            <asp:Button ID="btnPCancel" Text="Cancel" CssClass="ClsBtn" runat="server" TabIndex="21"
                                                                                CausesValidation="False" UseSubmitBehavior="False" 
                                                                                onclick="btnPCancel_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="tr3" runat="server">
                                                            <td align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" runat="server" id="tdParentListView">
                                                                <div id="divContainer" class="GridBorder" runat="server" style="width: 80%; height: 300px;
                                                                    overflow: scroll">
                                                                    <asp:ListView ID="lstvwParentDetails" runat="server" DataKeyNames="ParentAssociationDetailsId,StudentId,DesignationId,MotherName,FatherName,MobileNumber1,MobileNumber2,IsMobileNo1,IsMobileNo2"
                                                                        OnItemCommand="lstvwParentDetails_ItemCommand" OnItemDataBound="lstvwParentDetails_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table width="80%" id="tblSearchLstvw" runat="server" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="center" width="10px" style="padding-left: 12px; padding-right: 12px">
                                                                                        Sr.No.
                                                                                    </th>
                                                                                    <th align="center" style="width: 170px;padding-left: 12px; padding-right: 12px">
                                                                                        Class
                                                                                    </th>
                                                                                    <th align="left" style="width: 210px; padding-left: 12px; padding-right: 12px">
                                                                                        Student Name
                                                                                    </th>
                                                                                    <th align="left" style=" padding-left: 12px; width : 10px; padding-right: 12px">
                                                                                        Parent Name
                                                                                    </th>
                                                                                    <th align="center" style="width: 210px; padding-left: 12px; padding-right: 12px" runat="server" id="thConsideredAsParent">
                                                                                        Considered As Parent
                                                                                    </th>
                                                                                    <th align="center" style="width: 90px; padding-left: 12px; padding-right: 12px">
                                                                                        Mobile No.
                                                                                    </th>
                                                                                    <th align="left" style=" padding-left: 12px; padding-right: 12px">
                                                                                        Designation
                                                                                    </th>
                                                                                   
                                                                                    <th align="left" style="padding-left: 12px; width:250px; padding-right: 12px">
                                                                                        Contact Timing
                                                                                    </th>
                                                                                    <th align="center" style="padding-left: 12px; padding-right: 12px" id="ParentEdit"
                                                                                        runat="server">
                                                                                        Edit
                                                                                    </th>
                                                                                    <th align="center" style="padding-left: 12px; padding-right: 10px" id="ParentDelete"
                                                                                        runat="server">
                                                                                        Delete
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <tr id="trParent" runat="server" class="ClsGridRow">
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblRowIndex" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                </td>
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblClass" runat="server" Width="170px" CssClass="LblNormal" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label Width="220px" ID="lblStudentName" runat="server" CssClass="LblNormal"
                                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label Width="100px" ID="lblParentName" runat="server" CssClass="LblNormal" Text='<%#Eval("ParentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px" runat="server" id="tdConsideredAsParent">
                                                                                    <asp:Label ID="lblConsideredAsParent" runat="server" Width="170px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("ConsideredAsParent") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblContactNumber" Width="90px" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" visible="false" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblResidenceArea" runat="server" Width="300px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("ResidenceArea") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblContactTiming" runat="server" Width="300px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("ContactTiming") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdPimgEdit" style="width: 50px">
                                                                                    <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="PARENT_EDIT"
                                                                                        CommandArgument='<%#Eval("ParentAssociationDetailsId")%>' CausesValidation="false"
                                                                                        ToolTip="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"></asp:ImageButton>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdPimgDelete">
                                                                                    <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="PARENT_DELETE"
                                                                                        OnClientClick="if(!ConfirmDelete()) return false;" CommandArgument='<%#Eval("ParentAssociationDetailsId")%>'
                                                                                        CausesValidation="false" ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                                                    </asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidDesigId" runat="server" />
                                                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="trParent" runat="server" class="ClsGridAltRow">
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblRowIndex" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                </td>
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblClass" runat="server" CssClass="LblNormal" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label Width="220px" ID="lblStudentName" runat="server" CssClass="LblNormal"
                                                                                        Text='<%#Eval("StudentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style=" padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label Width="80px" ID="lblParentName" runat="server" CssClass="LblNormal" Text='<%#Eval("ParentName") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdConsideredAsParent">
                                                                                    <asp:Label ID="lblConsideredAsParent" runat="server" CssClass="LblNormal" Text='<%#Eval("ConsideredAsParent") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblContactNumber" Width="90px" runat="server" CssClass="LblNormal"></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px">
                                                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="LblNormal" Text='<%#Eval("DesignationName") %>'></asp:Label>
                                                                                </td>
                                                                               <td align="left" visible="false" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblResidenceArea" runat="server" Width="300px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("ResidenceArea") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="padding-left: 8px; padding-right: 8px">
                                                                                    <asp:Label ID="lblContactTiming" runat="server" Width="300px" CssClass="LblNormal"
                                                                                        Text='<%#Eval("ContactTiming") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdPimgEdit" style="width: 50px">
                                                                                    <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="PARENT_EDIT"
                                                                                        CommandArgument='<%#Eval("ParentAssociationDetailsId")%>' CausesValidation="false"
                                                                                        ToolTip="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"></asp:ImageButton>
                                                                                </td>
                                                                                <td align="center" runat="server" id="tdPimgDelete">
                                                                                    <asp:ImageButton runat="server" ID="ImageButton1" Text="Delete" CommandName="PARENT_DELETE"
                                                                                        OnClientClick="if(!ConfirmDelete()) return false;" CommandArgument='<%#Eval("ParentAssociationDetailsId")%>'
                                                                                        CausesValidation="false" ToolTip="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
                                                                                    </asp:ImageButton>
                                                                                    <asp:HiddenField ID="hidDesigId" runat="server" />
                                                                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                        <EmptyDataTemplate>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td class="LblNoRecord" align="center">
                                                                                        No record found.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </EmptyDataTemplate>
                                                                    </asp:ListView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNoRecordParentMsg" runat="server">
                                                            <td style="height: 10px;" align="center">                                                                
                                                                    <span class="LblNoRecord" style="width:800px;font:Bold">No Record Found.</span>
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
                                                    <asp:HiddenField ID="hidTeacherId" runat="server" />
                                                    <asp:HiddenField ID="hidStudentId" runat="server" />
                                                    <asp:HiddenField ID="hidOriginalStandardId" runat="server" />
                                                    <asp:HiddenField ID="hidTeacherRowCount" runat="server" />
                                                    <asp:HiddenField ID="hidParentRowCount" runat="server" />
                                                    <asp:HiddenField ID="hidTempUserId" runat="server" />
                                                    <asp:HiddenField ID="hidMode" runat="server" Value="NEW_MODE" />
                                                    <asp:HiddenField ID="hidTeacherAssociationId" runat="server" />
                                                    <asp:HiddenField ID="hidParentAssociationDetailsId" runat="server" />
                                                    <asp:HiddenField ID="hidSelectedRow" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hidUserHasEditAccess" runat="server" />                                                   
                                                </td>   
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSaveTeacherDetails" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSaveParentDetails" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lstvwSearchByCategory" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwSearchByCategory" EventName="ItemDataBound" />
                <asp:AsyncPostBackTrigger ControlID="lstvwTeacherDetails" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwTeacherDetails" EventName="ItemDataBound" />
                <asp:AsyncPostBackTrigger ControlID="btnTCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPCancel" EventName="Click" />                 
            </Triggers>
        </asp:UpdatePanel>
        <style type="text/css">
            .Padding14
            {
                padding-left: 14px;
            }
        </style>

        <script type="text/javascript" language="javascript">

            _clientListViewId = "<%= this.lstvwTeacherDetails.ClientID  %>";
            _clientParentListViewId = "<%= this.lstvwParentDetails.ClientID  %>";
            _clientlblErrorMsg = "<%= this.lblErrorMsg.ClientID  %>";
            _clientoptTeacher = "<%= this.optTeacher.ClientID  %>";
            _clientoptParent = "<%= this.optParent.ClientID  %>";
            _clienttxtSearchByName = "<%= this.txtSearchByName.ClientID  %>";
            _clienttxtTeacherName = "<%= this.txtTeacherName.ClientID  %>";
            _clienttxtFatherName = "<%= this.txtFatherName.ClientID  %>";
            _clienttxtMotherName = "<%= this.txtMotherName.ClientID  %>";
            _clienttxtContactTiming = "<%=this.txtContactTiming.ClientID %>"
            _clientcmbPDesignation = "<%=this.cmbPDesignation.ClientID %>"
            _clientcmbTDesignation = "<%=this.cmbTDesignation.ClientID %>"            
            _clienttxtResidenceArea = "<%=this.txtResidenceArea.ClientID %>"
            _clientvalPSumErrorMsg = "<%=this.valPSumErrorMsg.ClientID %>"
            _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID%>"
            _clientbtnSaveTeacherDetails = "<%=this.btnSaveTeacherDetails.ClientID %>"
            _clientbtnTCancel = "<%=this.btnTCancel.ClientID%>"
            _clientbtnSaveParentDetails = "<%=this.btnSaveParentDetails.ClientID %>"
            _clientbtnPCancel = "<%=this.btnPCancel.ClientID%>"
            _clientoptFather = "<%= this.optFatherAsParent.ClientID %>";
            _clientoptMother = "<%= this.optMotherAsParent.ClientID %>";
            _clientcmbSection = "<%= this.cmbSection.ClientID %>";
            _clientcstManualNos = "<%= this.cstManualNos.ClientID %>";
            _clientcstDuplicateTeacherValidation = "<%= this.cstDuplicateTeacherValidation.ClientID %>";
            _clientcstDuplicateParentValidation = "<%= this.cstDuplicateParentValidation.ClientID %>";            
            _clienthidTempUserId = "<%= this.hidTempUserId.ClientID %>";
            _clienthidTeacherRowCount = "<%= this.hidTeacherRowCount.ClientID %>";
            _clienthidParentRowCount = "<%= this.hidParentRowCount.ClientID %>";
            _clienthidMode = "<%= this.hidMode.ClientID %>";
            _clientcstValidate = "<%= this.cstValidate.ClientID %>";
            _clientcstTeacher = "<%= this.cstTeacher.ClientID %>";
            _clienhidStudentId = "<%=this.hidStudentId.ClientID %>";
            _clienhidSelectedRow = "<%=this.hidSelectedRow.ClientID %>";
            _clientchkMobileNumber1 = "<%=this.chkMobileNumber1.ClientID %>";
            _clientchkMobileNumber2 = "<%=this.chkMobileNumber2.ClientID %>";
            _clienttxtMobileNumber1 = "<%=this.txtMobileNumber1.ClientID %>";
            _clienttxtMobileNumber2 = "<%=this.txtMobileNumber2.ClientID %>";            
            _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
            _clientcst_MobileNumber2 = "<%=this.cst_MobileNumber2.ClientID %>";

            function ClearText(oSrc, arg) {
                var TRadioBtn = document.getElementById(_clientoptTeacher);
                var PRadioBtn = document.getElementById(_clientoptParent);
                var txtSearchName = document.getElementById(_clienttxtSearchByName);
                txtSearchName.value = "";
            }

            function ValidateParent() {
                if (document.getElementById(_clientvalPSumErrorMsg) != null)
                    document.getElementById(_clientvalPSumErrorMsg).style.display = "none";
                return true;
               }

               function ValidateDesignation(oSrc, args) {
			   var cmbDesignation = $get(_clientcmbTDesignation);
               	if (cmbDesignation != null) {

               		if (cmbDesignation.selectedIndex == 0) {
               			oSrc.errormessage = "Committee designation should be selected.";
               			args.IsValid = false;
               			return true;
               		}
               	}
               	args.IsValid = true;
               	return false;
               }
	

            function ConfirmDelete() {
                var bFlag = false
                bFlag = confirm('Are you sure you want to delete this record?')
                return bFlag
               }
            function ClearErrorMsg() {
                if (document.getElementById(_clientlblErrorMsg) != null)
                    document.getElementById(_clientlblErrorMsg).style.display = "none";
                if (document.getElementById(_clientvalPSumErrorMsg) != null)
                    document.getElementById(_clientvalPSumErrorMsg).style.display = "none";

                return true;
            }

            function ClearTeacherControls(oSrc, args) {
                document.getElementById(_clienttxtTeacherName).value = "";
                document.getElementById(_clientcmbTDesignation).selectedIndex = "0";
                document.getElementById(_clientbtnSaveTeacherDetails).value = "Save";
                document.getElementById(_clientcmbSection).selectedIndex = "0";

                DisableTeacherControls();
                ClearErrorMsg()
            }
            function DisableTeacherControls() {
                document.getElementById(_clienttxtTeacherName).disabled = true;
                document.getElementById(_clientcmbTDesignation).disabled = true;
                document.getElementById(_clientcmbSection).disabled = true;
                document.getElementById(_clientbtnSaveTeacherDetails).disabled = true;
                document.getElementById(_clientbtnTCancel).disabled = true;
            }

            function ClearParentControls(oSrc, args) {
                document.getElementById(_clienttxtFatherName).value = "";
                document.getElementById(_clienttxtMotherName).value = "";
                document.getElementById(_clienttxtResidenceArea).value = "";
                document.getElementById(_clienttxtContactTiming).value = "";
                document.getElementById(_clientoptFather).checked = true;
                document.getElementById(_clientoptMother).checked = false;
                document.getElementById(_clientcmbPDesignation).selectedIndex = "0";
                document.getElementById(_clientcmbPDesignation).selectedIndex = "0";
                document.getElementById(_clientbtnSaveParentDetails).value = "Save";
                document.getElementById(_clienttxtMobileNumber1).value = "";
                document.getElementById(_clienttxtMobileNumber2).value = "";
                document.getElementById(_clientchkMobileNumber1).checked = false;
                document.getElementById(_clientchkMobileNumber2).checked = false;
                DisableParentControls();
                ClearErrorMsg();
            }
            function DisableParentControls() {
                document.getElementById(_clienttxtFatherName).disabled = true;
                document.getElementById(_clienttxtMotherName).disabled = true;
                document.getElementById(_clienttxtResidenceArea).disabled = true;
                document.getElementById(_clienttxtContactTiming).disabled = true;
                document.getElementById(_clientcmbPDesignation).disabled = true;
                document.getElementById(_clientbtnSaveParentDetails).disabled = true;
                document.getElementById(_clientbtnPCancel).disabled = true;
                document.getElementById(_clientoptFather).disabled = true;
                document.getElementById(_clientoptMother).disabled = true;
                document.getElementById(_clientchkMobileNumber1).disabled = true;
                document.getElementById(_clientchkMobileNumber2).disabled = true;
                document.getElementById(_clienttxtMobileNumber1).disabled = true;
                document.getElementById(_clienttxtMobileNumber2).disabled = true;
                
            }
            function MobileNumberValidation(oSrc, args) {

                var sMobileNumber = document.getElementById(_clienttxtMobileNumber1).value;
                sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
                document.getElementById(_clientcst_MobileNumber).errormessage = "";

                if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                    document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile Number1 should be of 10 digits.";
                    args.IsValid = false;
                    return true;
                }
                else if (sMobileNumber.substring(0, 1) == '0') {
                    document.getElementById(_clientcst_MobileNumber).errormessage = "Mobile Number1 should not start with zero.";
                    args.IsValid = false;
                    return true;
                }
            }
            function MobileNumber2Validation(oSrc, args) {

                var sMobileNumber = document.getElementById(_clienttxtMobileNumber2).value;
                var sMobileNumber1 = document.getElementById(_clienttxtMobileNumber1).value;
                sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
                document.getElementById(_clientcst_MobileNumber2).errormessage = "";

                if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                    document.getElementById(_clientcst_MobileNumber2).errormessage = "Mobile Number2 should be of 10 digits.";
                    args.IsValid = false;
                    return true;
                }
                else if (sMobileNumber.substring(0, 1) == '0') {
                    document.getElementById(_clientcst_MobileNumber2).errormessage = "Mobile Number2 should not start with zero.";
                    args.IsValid = false;
                    return true;
                }
                if (document.getElementById(_clientchkMobileNumber2).checked && sMobileNumber.length == 0) {
                    document.getElementById(_clientcst_MobileNumber2).errormessage = "Mobile Number2 should not be blank.";
                    args.IsValid = false;
                    return true;
                }

            }
            function CheckValidMobileNos(oSrc, args) {
                if ((document.getElementById(_clientchkMobileNumber1).checked == false) && (document.getElementById(_clientchkMobileNumber2).checked == false)) {
                    document.getElementById(_clientcstManualNos).errormessage = "At least one mobile number should be selected.";
                    args.IsValid = false; 
                    return true;                   
                }

                args.IsValid = true;
                return false;
            }
                     
             function CheckValidateManualNos(oSrc, args) {
            args.IsValid = true;
            return false;
           }

            function DuplicateTeacherValidation(oSrc, args) {
                var iRowId = document.getElementById(_clienhidSelectedRow).value;
                var UserId = document.getElementById(_clienthidTempUserId).value;
                
                var iRowCount = document.getElementById(_clienthidTeacherRowCount).value;
                var sTempUId = document.getElementById(_clienthidTempUserId).value;
                var sTempDeisgId = document.getElementById(_clientcmbTDesignation).value;
                var iNo = 0;
                var iDesignationId = document.getElementById(_clientcmbTDesignation).value;
                if (document.getElementById(_clienthidMode).value == "NEW_MODE") {
                    if (iRowCount > 0) {
                        for (iNo = 0; iNo < iRowCount; iNo++) {
                                var sDegignationId = document.getElementById(_clientListViewId + "_ctrl" + iNo + "_hidDesigId").value;
                                var sUserId = document.getElementById(_clientListViewId + "_ctrl" + iNo + "_hidUserId").value;
                                if (sTempUId == sUserId && iDesignationId == sDegignationId) {
                                    document.getElementById(_clientcstDuplicateTeacherValidation).errormessage = "Teacher designation should not be duplicated.";
                                    args.IsValid = false; return true;
                                }
                        }
                    }
                }
                args.IsValid = true;
                return false;
            }
            function DuplicateUserValidate(oSrc, args) {           

                var iRowId = document.getElementById(_clienhidSelectedRow).value;
                var UserId = document.getElementById(_clienthidTempUserId).value;

                var iRowCount = document.getElementById(_clienthidTeacherRowCount).value;
                var iDesignationId = document.getElementById(_clientcmbTDesignation).value;
                if (document.getElementById(_clienthidMode).value == "EDIT_MODE") {
                    if (iRowCount > 0) {
                        for (iNo = 0; iNo < iRowCount; iNo++) {
                            if (iNo != iRowId) {
                                var sDegignationId = document.getElementById(_clientListViewId + "_ctrl" + iNo + "_hidDesigId").value;
                                var sUserId = document.getElementById(_clientListViewId + "_ctrl" + iNo + "_hidUserId").value;
                                if (UserId == sUserId && iDesignationId == sDegignationId) {
                                    oSrc.errormessage = "Teacher designation should not be duplicated.";
                                    document.getElementById(_clientcstTeacher).errormessage = "Teacher designation should not be duplicated.";
                                    args.IsValid = false; return true;
                                }
                            }
                        }
                    }
                }
                args.IsValid = true;
                return false;
            }
            function DuplicateParentValidator(oSrc, args) {
                var iRowCount = document.getElementById(_clienthidParentRowCount).value;
                var sTempUId = document.getElementById(_clienthidTempUserId).value;
                var sTempDeisgId = document.getElementById(_clientcmbPDesignation).value;
                var iNo = 0;
                var iDesignationId = document.getElementById(_clientcmbPDesignation).value;
                if (document.getElementById(_clienthidMode).value == "NEW_MODE") {
                    if (iRowCount > 0) {
                        for (iNo = 0; iNo < iRowCount; iNo++) {
                            var sDegignationId = document.getElementById(_clientParentListViewId + "_ctrl" + iNo + "_hidDesigId").value;
                            var sUserId = document.getElementById(_clientParentListViewId + "_ctrl" + iNo + "_hidUserId").value;
                            if (sTempUId == sUserId && iDesignationId == sDegignationId) {
                                document.getElementById(_clientcstDuplicateParentValidation).errormessage = "Parent designation should not be duplicated.";
                                args.IsValid = false; return true;
                            }
                        }
                    }
                }
                args.IsValid = true;
                return false;
            }

            function DuplicateUserPValidate(oSrc, args) {
                var UserId = document.getElementById(_clienhidStudentId).value;
                var iRowId = document.getElementById(_clienhidSelectedRow).value;


                var iRowCount = document.getElementById(_clienthidParentRowCount).value;
                var iDesignationId = document.getElementById(_clientcmbPDesignation).value;
                if (document.getElementById(_clienthidMode).value == "EDIT_MODE") {
                    if (iRowCount > 0) {
                        for (iNo = 0; iNo < iRowCount; iNo++) {
                            if (iNo != iRowId) {
                                var sDegignationId = document.getElementById(_clientParentListViewId + "_ctrl" + iNo + "_hidDesigId").value;
                                var sUserId = document.getElementById(_clientParentListViewId + "_ctrl" + iNo + "_hidUserId").value;
                                if (UserId == sUserId && iDesignationId == sDegignationId) {
                                    document.getElementById(_clientlblErrorMsg).visible = true;
                                    document.getElementById(_clientcstValidate).errormessage = "Parent designation should not be duplicated.";
                                    args.IsValid = false;
                                    return true;
                                }
                            }
                       }
                        args.IsValid = true;
                        return false;
                    }
                }
            }

            function blockNonPhNumbers1(obj, e) {
                var key;
                var isCtrl = false;
                var keychar;
                var reg;
                if (window.event) {
                    key = e.keyCode;
                    isCtrl = window.event.ctrlKey
                }
                else if (e.which) {
                    key = e.which;
                    isCtrl = e.ctrlKey;
                }

                if (isNaN(key)) return true;

                keychar = String.fromCharCode(key);

                // check for backspace or delete, or if Ctrl was pressed
                if (key == 8 || isCtrl) {
                    return true;
                }
                // check for space
                if (key == 32) 
                    return false;

                reg = /\d/;
                reg1 = /\s/;
                reg2 = /,/;
                reg3 = /-/;
                return reg.test(keychar) || reg1.test(keychar) || reg2.test(keychar) || reg3.test(keychar);
            }
        </script>

    </div>
</asp:Content>
