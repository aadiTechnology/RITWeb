<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LearningOutcomeConfigurationUI.aspx.cs"
    Inherits="LearnigOutcomeConfigurationUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td style="height:20px;">
                
            </td>
        </tr>
        <tr>
            <td>
                <div runat="server" id="divErr" style="width:90%">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table id="tblLearningOutcome" runat="server" border="0" cellpadding="0" cellspacing="2"
                            style="height: 100%; width: 100%;">
                            <tr>
                                <td style="width: 77%">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                    </asp:Panel>
                                </td>
                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                    <span class="ClsMdtStar">* 
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryField %>"></asp:Label>                                                        
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                    <asp:ValidationSummary ID="valSumErrorMsg2" runat="server" ValidationGroup="Copy"
                                        CssClass="ClsLabel" ShowSummary="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbStandards" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                    OnSelectedIndexChanged="cmbStandards_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvalcmbStandards" runat="server" ControlToValidate="cmbStandards"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, StandardShouldSelected %>" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources, Assessment %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbAssessment" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbAssessment_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvalcmbAssessment" runat="server" ControlToValidate="cmbAssessment"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AssessmentShouldSelected %>" InitialValue="0"
                                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbSubjects" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                    OnSelectedIndexChanged="cmbSubjects_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvalcmbSubjects" runat="server" ControlToValidate="cmbSubjects"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectShouldBeSelected %>" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSection %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbSubjectSection" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbSubjectSection_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvalcmbSubjectSection" runat="server" ControlToValidate="cmbSubjectSection"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValSubjectSection %>" InitialValue="0"
                                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <span class="ClsLabel"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <span style="font-weight: bold; font-weight: 700; font-size: 9pt">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, LearningOutcomeDetails %>"></asp:Label>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, LearningOutcome %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtLearningOutcome" MaxLength="500" BorderColor="Gray" BorderWidth="1px"
                                                    BorderStyle="Solid" Width="97.5%" runat="server"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvaltxtLearningOutcome" runat="server" ControlToValidate="txtLearningOutcome"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValLearningOutcome %>" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="2" CssClass="MidTxtBox"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="reqvaltxtSortOrder" runat="server" ControlToValidate="txtSortOrder"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SortOrderShouldNotBeBlank %>" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, AllowGradeAssignment %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsconsidered" runat="server" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>" CssClass="ClsBtn" BorderWidth="1px"
                                                    CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center" width="75%">
                            <tr>
                                <td>
                                    <div id="divLearningOutcomeDetails" runat="server" class="GridBorder" runat="server"
                                        style="overflow: auto; height: 400px;">
                                        <asp:ListView ID="lstvwLearningOutcomeDetails" runat="server" DataKeyNames="IsConsidered,LearningOutcomeConfigId"
                                            OnItemCommand="lstvwLearningOutcomeDetails_ItemCommand" OnItemDataBound="lstvwLearningOutcomeDetails_ItemDataBound"
                                            OnSorting="lstvwLearningOutcomeDetails_Sorting" OnDataBound="lstvwLearningOutcomeDetails_DataBound">
                                            <LayoutTemplate>
                                                <table align="center" width="100%" runat="server" id="tblLearningOutcomeInfo" style="color: #333333"
                                                    cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="10%">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, AllowGradeAssignment %>"></asp:Label>
                                                        </th>
                                                        <th width="5%">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, SrNo %>"></asp:Label>
                                                        </th>
                                                        <th align="left" width="30%" style="padding-left: 7px;">
                                                            <asp:LinkButton ID="lnkBtnLearningOutcome" runat="server" CommandName="Sort" CommandArgument="LearningOutcome"
                                                                CausesValidation="false" ForeColor="Black">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, LearningOutcome %>"></asp:Label>
                                                                </asp:LinkButton>
                                                        </th>
                                                        <th width="8%">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, SortOrder %>"></asp:Label>
                                                        </th>
                                                        <th align="center" width="5%">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>"></asp:Label>
                                                        </th>
                                                        <th align="center" width="5%">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="center">
                                                        <asp:Image ID="imgIsConsidered" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                            runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRowNo" runat="server" Text="<%$ Resources:LocalizedResources, No %>"></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblLearningOutcome" runat="server" Text='<%# Eval("LearningOutCome") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateLearningOutcome"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveLearningOutcome"
                                                            ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Image ID="imgIsConsidered" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
                                                            runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRowNo" runat="server" Text="<%$ Resources:LocalizedResources, No %>"></asp:Label>
                                                    </td>
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblLearningOutcome" runat="server" Text='<%# Eval("LearningOutCome") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateLearningOutcome"
                                                            ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveLearningOutcome" CausesValidation="false"
                                                            runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trNoRecordMsg" runat="server" visible="false">
                                <td style="height: 10px;" align="center">
                                    <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                        Text="<%$ Resources:LocalizedResources, NoRecordFound %>" EnableViewState="False" Width="85%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidMode" runat="server" />
                                    <asp:HiddenField ID="hidLearningOutcomeConfigId" runat="server" />
                                    <asp:HiddenField ID="hidIsSubmitted" runat="server" />
                                    <asp:HiddenField ID="hidLearningOutcome" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidScreenWidth" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowCount" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CustomValidator ID="cstvalCopyTo" runat="server" ClientValidationFunction="ValidateCopyToStandardAndAssessment"
                                        SetFocusOnError="True" ValidationGroup="Copy" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValStandardAssessment %>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DupEnteredSubject %>"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cstvalDuplicateSortOrder" runat="server" ClientValidationFunction="DuplicateSortOrder"
                                        SetFocusOnError="True" ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DupSortOrder %>"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                                        ValidationGroup="Save" CausesValidation="false" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" BorderWidth="1px"
                                        CausesValidation="False" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                </td>
                            </tr>
                        </table>
                         <asp:HiddenField ID="hidbtnSave" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>
                   
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="2" style="height: 100%;
                            width: 100%;">
                            <tr>
                                <td align="center">
                                    <table id="tblCopy" runat="server">
                                        <tr>
                                            <td class="ClsBorderlight" style="width: 115px">                                                
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:LocalizedResources, CopyToStandard %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbCopyStandards" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                    OnSelectedIndexChanged="cmbCopyStandards_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="cmbCopyStandards"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, StandardCondition %>" InitialValue="0" ValidationGroup="Copy"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:LocalizedResources, Assessment %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbCopyAssessment" runat="server" CssClass="MidCombo">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="cmbCopyAssessment"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AssessmentShouldSelected %>" InitialValue="0"
                                                    ValidationGroup="Copy"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources, Subject %>"></asp:Label>
                                                <span class="colonPadding"> :</span>
                                                </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbCopySubjects" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                    OnSelectedIndexChanged="cmbCopySubjects_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="cmbCopySubjects"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SubjectShouldBeSelected %>"  InitialValue="0" ValidationGroup="Copy"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ClsBorderlight">
                                                <span class="ClsLabel">
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:LocalizedResources, SubjectSection %>"></asp:Label>
                                                <span class="colonPadding"> :</span></span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbCopySubjectSection" runat="server" CssClass="MidCombo" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                                <asp:RequiredFieldValidator ID="req4" runat="server" ControlToValidate="cmbCopySubjectSection"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValSubjectSection %>" InitialValue="0"
                                                    ValidationGroup="Copy"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnCopy" runat="server" Text="<%$ Resources:LocalizedResources, Copy %>" CssClass="ClsBtn" BorderWidth="1px"
                                                    ValidationGroup="Copy" CausesValidation="true" OnClick="btnCopy_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="hidSubmitText" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                    </Triggers>                    
                </asp:UpdatePanel>
            </td>
        </tr>
        <asp:HiddenField runat="server" ID="hidValStandardSubjectAssessment" />
        <asp:HiddenField runat="server" ID="hidConfirmUnLearningOutcome" />
        <asp:HiddenField runat="server" ID="hidDupLearningOutcome" />
        <asp:HiddenField runat="server" ID="hidConfirmLearningOutcome" />
        <asp:HiddenField runat="server" ID="hidDupSortOder" />
        <asp:HiddenField runat="server" ID="hidValSelectedCopyTo" />
        <asp:HiddenField runat="server" ID="hidSubmit" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" />  
        
    </table>

    <script type="text/javascript" language="javascript">
        _clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clientlbl_ErrorMessage = "<%=this.lblErrorMsg.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidRowNo = "<%=this.hidRowNo.ClientID %>"
        _clientbtnSubmit = "<%=this.btnSubmit.ClientID %>"
        _clienttxtSortOrder = "<%=this.txtSortOrder.ClientID %>"
        _clienttxtLearningOutcome = "<%=this.txtLearningOutcome.ClientID %>"
        _clientlstvwLearningOutcomeDetails = "<%=this.lstvwLearningOutcomeDetails.ClientID %>"
        _ClientcstvalDuplicateSortOrder = "<%=this.cstvalDuplicateSortOrder.ClientID %>"
        _ClientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"
        _ClientcstvalCopyTo = "<%=this.cstvalCopyTo.ClientID %>"
        _clientcmbCopyStandards = "<%=this.cmbCopyStandards.ClientID %>"
        _clientcmbCopyAssessment = "<%=this.cmbCopyAssessment.ClientID %>"
        _clientcmbCopySubjects = "<%=this.cmbCopySubjects.ClientID %>"
        _clientcmbCopySubjectSection = "<%=this.cmbCopySubjectSection.ClientID %>"
        _clientcmbStandards = "<%=this.cmbStandards.ClientID %>"
        _clientcmbAssessment = "<%=this.cmbAssessment.ClientID %>"
        _clientcmbSubjects = "<%=this.cmbSubjects.ClientID %>"
        _clientcmbSubjectSection = "<%=this.cmbSubjectSection.ClientID %>"

        function ConfirmCopy() {
            SetLables();
            var isPageValid = true
            isPageValid = Page_ClientValidate("Copy")
            if (isPageValid) {
                var bResult = true
                if (!window.confirm(document.getElementById("<%=this.hidValStandardSubjectAssessment.ClientID %>").value)) {                    
                    bResult = false
                }
            }
            return bResult
        }

        function ConfirmDelete(msg) {
            SetLables();
            var bResult = true
            if (!window.confirm(msg)) {
                bResult = false
            }
            return bResult
        }

        function SetLables() {
            var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
            if (lbl1 != null)
                lbl1.innerHTML = "";
            var lbl2 = document.getElementById(_clientlbl_ErrorMessage);
            if (lbl2 != null)
                lbl2.innerHTML = "";
        }

        function ConfirmSubmit() {
            SetLables();
            var msg = ""
            if (document.getElementById(_clientbtnSubmit).value != document.getElementById("<%=this.hidSubmit.ClientID %>").value)
                msg = document.getElementById("<%=this.hidConfirmUnLearningOutcome.ClientID %>").value;
            else
                msg = document.getElementById("<%=this.hidConfirmLearningOutcome.ClientID %>").value;
            var bResult = true
            if (!window.confirm(msg)) {
                bResult = false
            }
            return bResult
        }


        function DuplicateValue(oSrc, args) {
            SetLables();
            var lblLearningOutcome = "";
            var sRowNo = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtLearningOutcome = document.getElementById(_clienttxtLearningOutcome).value
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                lblLearningOutcome = document.getElementById(_clientlstvwLearningOutcomeDetails + "_ctrl" + iRowNumber + "_lblLearningOutcome").innerHTML;
                if (txtLearningOutcome.toLowerCase() == lblLearningOutcome.toLowerCase() && iRowNumber != (iRowNo - 1)) {
                    sRowNo += (iRowNumber + 1) + ", ";
                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = document.getElementById("<%=this.hidDupLearningOutcome.ClientID %>").value+": " + sRowNo + ".";
                document.getElementById(_ClientcstvalDuplicateValue).innerText = document.getElementById("<%=this.hidDupLearningOutcome.ClientID %>").value+": " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }
        function DuplicateSortOrder(oSrc, args) {
            SetLables();
            var SortOrder = "";
            var sRowNo = "";
            var iRowCount = document.getElementById(_clienthidRowCount).value
            var iRowNo = document.getElementById(_clienthidRowNo).value
            var txtSortOrder = document.getElementById(_clienttxtSortOrder).value
            for (var iRowNumber = 0; iRowNumber < iRowCount; iRowNumber++) {
                SortOrder = document.getElementById(_clientlstvwLearningOutcomeDetails + "_ctrl" + iRowNumber + "_lblSortOrder").innerHTML;
                if (txtSortOrder == SortOrder && iRowNumber != (iRowNo - 1)) {
                    sRowNo += (iRowNumber + 1) + ", ";
                }
            }
            if (sRowNo != "") {
                sRowNo = sRowNo.substring(0, sRowNo.length - 2);
                oSrc.errormessage = document.getElementById("<%=this.hidDupSortOder.ClientID %>").value+": " + sRowNo + "."; 
                document.getElementById(_ClientcstvalDuplicateSortOrder).innerText = document.getElementById("<%=this.hidDupSortOder.ClientID %>").value+": " + sRowNo + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ValidateCopyToStandardAndAssessment(oSrc, args) {
            SetLables();
            var cmbCopyAssessment = document.getElementById(_clientcmbCopyAssessment).value
            var cmbCopyStandards = document.getElementById(_clientcmbCopyStandards).value
            var cmbCopySubjects = document.getElementById(_clientcmbCopySubjects).value
            var cmbCopySubjectSection = document.getElementById(_clientcmbCopySubjectSection).value
            var cmbAssessment = document.getElementById(_clientcmbAssessment).value
            var cmbStandards = document.getElementById(_clientcmbStandards).value
            var cmbSubjects = document.getElementById(_clientcmbSubjects).value
            var cmbSubjectSection = document.getElementById(_clientcmbSubjectSection).value


            if (cmbAssessment != cmbCopyAssessment || cmbStandards != cmbCopyStandards
                        || cmbSubjects != cmbCopySubjects || cmbSubjectSection != cmbCopySubjectSection) {
                args.IsValid = true
                return false
            }
            else {
                oSrc.errormessage = document.getElementById("<%=this.hidValSelectedCopyTo.ClientID %>").value;
                document.getElementById(_ClientcstvalCopyTo).innerText = document.getElementById("<%=this.hidValSelectedCopyTo.ClientID %>").value;
                args.IsValid = false
                return true
            }
        }
    </script>

</asp:Content>
