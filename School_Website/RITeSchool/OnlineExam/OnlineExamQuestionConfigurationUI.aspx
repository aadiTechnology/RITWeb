<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamQuestionConfigurationUI.aspx.cs" Inherits="OnlineExamQuestionConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
<script type="text/javascript" src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
<script id="MathJax-script" type="text/javascript" async src="https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js">
</script>
    <table width="100%">
        <tr>
            <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
               <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                        <asp:ValidationSummary ID="valSumTaskDetails" CssClass="LblErrorMsg" ShowSummary="true"
                            runat="server" ValidationGroup="Save" ShowMessageBox="false" />
                        <asp:RequiredFieldValidator ID="reqStandard" runat="server" ValidationGroup="Save"
                            Display="None" InitialValue="0" ControlToValidate="ddlStandard" ErrorMessage="Standard should be selected."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqcmbDivision" ValidationGroup="Save" runat="server"
                            Display="None" InitialValue="0" ControlToValidate="ddlSubject" ErrorMessage="Subject should be selected."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="reqvalQuestion" runat="server" ControlToValidate="txtQuestion"
                            Display="None" ValidationGroup="Save" ErrorMessage="Question should not be blank"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOutOfMarks"
                            Display="None" ValidationGroup="Save" ErrorMessage="Out of Marks should not be blank"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cstvalAnser1" runat="server" ValidationGroup="Save" ClientValidationFunction="ValidateAnswerName"
                            Display="None" />
                        <asp:CustomValidator ID="cstvalOption" runat="server" ValidationGroup="Save" ClientValidationFunction="ValidaterdoOption"
                            Display="None" />
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="Save" ClientValidationFunction="ValidateQuestion"
                            Display="None" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="Save" ClientValidationFunction="ValidateAnswers"
                            Display="None" />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label>
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>           
            <td align="left" style="height: 25px; text-align:right; padding-right:50px; width:200px;">
                <asp:LinkButton ID="lnkMathFormula" runat="server" ViewStateMode="Enabled" Text="Math Formula"
                    CssClass="SubTitle"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="2">
                    <tr>
                        <td align="left" style="width: 100px" class="ClsBorderlight">
                            <span class="ClsLabel">Standard :</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStandard" class="MidCombo" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                        </td>
                        <td align="left" style="width: 100px" class="ClsBorderlight">
                            <span class="ClsLabel">Division :</span>
                        </td>
                        <td>
                            <%--<asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Always">
                                <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlDivision" class="MidCombo" runat="server" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                <%--</ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </td>
                        <td align="left" style="width: 100px" class="ClsBorderlight">
                            <span class="ClsLabel">Subject:</span>
                        </td>
                        <td>
                           <%-- <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlSubject" class="MidCombo" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <span class="ClsMdtStar">*</span>
                                <%--</ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr style="border: 1px solid gray;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                           <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>--%>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width:100px;">
                                                <span class="ClsLabel">Question :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" CssClass="ExLrgCombo"
                                                    Width="98%" MaxLength="1000"></asp:TextBox>
                                                <span class="ClsMdtStar">* </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" style="width:120px;">
                                                             <span class="ClsLabel">Upload File :</span>
                                                         </td>
                                                         <td>
                                                            <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>--%>
                                                                     <asp:FileUpload ID="fuQuestion" runat="server" Width="170px" />
                                                                     <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />
                                                                     <asp:ImageButton ID="imgDelete" runat="server"  CausesValidation="false" 
                                                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                                        onclick="imgDelete_Click" EnableViewState = "true"  />
                                                                      <asp:CustomValidator ID="CstValFileType" runat="server" ClientValidationFunction="validateFile"
                                                                        ControlToValidate="fuQuestion" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                                        ErrorMessage="Invalid file type." ValidationGroup="Save"></asp:CustomValidator>
                                                                <%--</ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="cmbAnswerType" EventName="SelectedIndexChanged" />                                                                                                                                    
                                                                </Triggers>
                                                            </asp:UpdatePanel>--%>
                                                         </td>
                                                         <td align="left" class="ClsBorderlight" style="width:100px;">
                                                             <span class="ClsLabel">Out Of Marks :</span>
                                                         </td>
                                                         <td id="tdoutofmarks" runat="server">
                                                             <asp:TextBox ID="txtOutOfMarks" runat="server" Style="width: 100PX" CssClass="ExLrgTxtBox"
                                                                MaxLength="2" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                onkeypress="return blockNonNumbers(this, event, false, false);" onkeyup="extractNumber(this,2,false);"
                                                                onpaste="event.returnValue=false"></asp:TextBox>
                                                             <span class="ClsMdtStar">*</span>
                                                         </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td align="left" style="width: 120px" class="ClsBorderlight" id="tdoutofmarks1" runat="server">
                                                <span class="ClsLabel">Answer Type:</span>
                                            </td>
                                            <td colspan="3" id="tdoutofmarks2" runat="server">
                                                <asp:DropDownList ID="cmbAnswerType" runat="server" CssClass="MidCombo" Width="180px">
                                                    <asp:ListItem Value="1">Free Text</asp:ListItem>
                                                    <asp:ListItem Value="2">File</asp:ListItem>
                                                    <asp:ListItem Value="3">Description Answer</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar" runat="server" id="span4" style="position: absolute; margin-left: 5px;">
                                                    *</span>
                                            </td>
                                        </tr>                                       
                                        <tr id="trAnswerDetails">
                                            <td colspan="2" align="center">
                                                <table align="center" style="margin-top: 10px;">
                                                    <tr id="trAns1" runat="server">
                                                        <td class="ClsBorderlight" style="width: 100px;">
                                                            <span class="clsLabel">Answer 1 : </span>
                                                        </td>
                                                        <td id="tdTxtAnswer1" runat="server">
                                                            <asp:TextBox ID="txtAns1" runat="server" CssClass="MidTxtBox" Width="260px" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                        <td id="tdFUAnswer1" runat="server" style="display:none;">
                                                             <asp:FileUpload ID="fuAnswer1" runat="server" Width="170px" />
                                                             <asp:ImageButton ID="btnImgAnsr1" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />
                                                             <asp:ImageButton ID="btnImgAnsDelete1" runat="server"  CausesValidation="false" 
                                                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                                        onclick="btnImgAnsDelete1_Click" EnableViewState = "true"  />
                                                             <asp:CustomValidator ID="cstValidateAnswerFiles" runat="server" ClientValidationFunction="ValidateAnswerFiles"
                                                                ControlToValidate="" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                                ErrorMessage="Invalid file type." ValidationGroup="Save"></asp:CustomValidator>
                                                         </td>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="rdOption1" runat="server" Text="Mark as Correct Answer" GroupName="a" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAns2" runat="server">
                                                        <td class="ClsBorderlight">
                                                            <span class="clsLabel">Answer 2 : </span>
                                                        </td>
                                                        <td id="tdTxtAnswer2" runat="server">
                                                            <asp:TextBox ID="txtAns2" runat="server" CssClass="MidTxtBox" Width="260px" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                         <td id="tdFUAnswer2" runat="server" style="display:none;">
                                                             <asp:FileUpload ID="fuAnswer2" runat="server" Width="170px" />
                                                             <asp:ImageButton ID="btnImgAnsr2" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />
                                                             <asp:ImageButton ID="btnImgAnsDelete2" runat="server"  CausesValidation="false" 
                                                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                                        onclick="btnImgAnsDelete2_Click" EnableViewState = "true"  />
                                                         </td>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="rdOption2" runat="server" Text="Mark as Correct Answer" GroupName="a" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAns3" runat="server">
                                                        <td class="ClsBorderlight">
                                                            <span class="clsLabel">Answer 3 : </span>
                                                        </td>
                                                        <td id="tdTxtAnswer3" runat="server">
                                                            <asp:TextBox ID="txtAns3" runat="server" CssClass="MidTxtBox" Width="260px" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                         <td id="tdFUAnswer3" runat="server" style="display:none;">
                                                             <asp:FileUpload ID="fuAnswer3" runat="server" Width="170px" />
                                                             <asp:ImageButton ID="btnImgAnsr3" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />
                                                             <asp:ImageButton ID="btnImgAnsDelete3" runat="server"  CausesValidation="false" 
                                                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                                        onclick="btnImgAnsDelete3_Click" EnableViewState = "true"  />
                                                         </td>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="rdOption3" runat="server" Text="Mark as Correct Answer" GroupName="a" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAns4" runat="server">
                                                        <td class="ClsBorderlight">
                                                            <span class="clsLabel">Answer 4 : </span>
                                                        </td>
                                                        <td id="tdTxtAnswer4" runat="server">
                                                            <asp:TextBox ID="txtAns4" runat="server" CssClass="MidTxtBox" Width="260px" TabIndex="7"></asp:TextBox>
                                                        </td>
                                                         <td id="tdFUAnswer4" runat="server" style="display:none;">
                                                             <asp:FileUpload ID="fuAnswer4" runat="server" Width="170px" />
                                                             <asp:ImageButton ID="btnImgAnsr4" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />
                                                             <asp:ImageButton ID="btnImgAnsDelete4" runat="server"  CausesValidation="false" 
                                                                        ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" 
                                                                        OnClientClick="return ConfirmDelete()" Visible = "false" 
                                                                        onclick="btnImgAnsDelete4_Click" EnableViewState = "true"  />
                                                         </td>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="rdOption4" runat="server" Text="Mark as Correct Answer" GroupName="a" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                <%--</ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>--%>
                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save%>"
                                        ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                        CausesValidation="false" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                               <%-- </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                        <table width="80%">
                            <tr id="tr1" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwConfigure"
                                        Visible="true">
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
                            <tr>
                                <td align="center">
                                    <asp:ListView ID="lstvwConfigure" runat="server" DataKeyNames="Id" OnItemDataBound="lstvwConfigure_ItemDataBound"
                                        DataSourceID="ObjDSQuestionDetails" OnDataBound="lstvwConfigure_DataBound" OnSorting="lstvwConfigure_Sorting"
                                        OnItemCommand="llstvwConfigure_ItemCommand">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th id="Th1" align="left" width="200px" style="padding-left: 10px;" visible="false"
                                                        runat="server">
                                                        Class
                                                    </th>
                                                    <th id="Th2" align="left" width="150px" style="padding-left: 10px;" visible="false"
                                                        runat="server">
                                                        <asp:LinkButton ID="lnkBtnDesignationName" runat="server" CausesValidation="false"
                                                            ForeColor="Black">Subject </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="padding-left: 10px;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Question"
                                                            CausesValidation="false" ForeColor="Black">Question </asp:LinkButton>
                                                    </th>
                                                    <th align="left" width="30%" style="padding-left: 10px;">
                                                        Correct Answer
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Out of Marks
                                                    </th>
                                                    <th align="center" width="100px">
                                                        Is Submitted?
                                                    </th>
                                                    <th align="center" width="50px">
                                                        Edit
                                                    </th>
                                                    <th align="center" width="50px">
                                                        Delete
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="8">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwConfigure" PageSize="20"
                                                            Visible="true">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
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
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                <td id="Td1" align="left" class="paddingL" visible="false" runat="server">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                </td>
                                                <td id="Td2" align="left" class="paddingL" visible="false" runat="server">
                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </td>
                                                <td id="tdColor" runat="server" align="left" class="paddingL">
                                                    <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value= '<%# Eval("Id") %>'/>
                                                </td>
                                                <td id="td5" runat="server" align="left" class="paddingL">
                                                    <asp:Label ID="lblCorrectAnswer" runat="server" Text='<%# Eval("CorrectAnswer") %>'></asp:Label>
                                                    <asp:Image ID="imgImage" runat="server" Height="50px" Width="50px" Visible="false" />
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Label ID="lblMotto" runat="server" Text='<%# Eval("OutOfMarks") %>'></asp:Label>
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                        Visible='<%# Eval("IsSubmitted") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="<%$ Resources:LocalizedResources, Delete%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CausesValidation="false"
                                                        CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                <td id="Td3" align="left" class="paddingL" visible="false" runat="server">
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                </td>
                                                <td id="Td4" align="left" class="paddingL" visible="false" runat="server">
                                                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                                </td>
                                                <td id="tdColor" runat="server" align="left" class="paddingL">
                                                    <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>'></asp:Label>
                                                    <asp:HiddenField ID="hidId" runat="server" Value= '<%# Eval("Id") %>'/>
                                                </td>
                                                <td id="td5" runat="server" align="left" class="paddingL">
                                                    <asp:Label ID="lblCorrectAnswer" runat="server" Text='<%# Eval("CorrectAnswer") %>'></asp:Label>
                                                    <asp:Image ID="imgImage" runat="server" Height="50px" Width="50px" Visible="false" />
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Label ID="lblMotto" runat="server" Text='<%# Eval("OutOfMarks") %>'></asp:Label>
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
                                                        Visible='<%# Eval("IsSubmitted") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="<%$ Resources:LocalizedResources, Delete%>"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" CausesValidation="false"
                                                        CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr style="width: 100%" visible="true">
                                                <td id="NoRecordFound" runat="server" class="LblNoRecord" align="center">
                                                    No record found.
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.OnlineExamQuestionConfigurationBL"
                                        EnablePaging="True" ID="ObjDSQuestionDetails" runat="server" SelectMethod="GetAll"
                                        SortParameterName="sortExpression" SelectCountMethod="Count" EnableCaching="False">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="hidSortExpression" Name="sortExpression" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter ControlID="hidSortDirection" Name="sortDirection" Type="String"
                                                PropertyName="Value" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            <asp:ControlParameter Name="aiStandardId" Type="int32" ControlID="ddlStandard" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="aiStandardDivisionId" Type="int32" ControlID="ddlDivision"
                                                PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="aiSubjectId" Type="int32" ControlID="ddlSubject" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td align="center">
               <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" BorderWidth="1px"
                            CausesValidation="False" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn" CausesValidation="false"
                            OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnUnsubmit" runat="server" Text="Un-Submit" CssClass="ClsBtn" CausesValidation="false"
                            OnClick="btnUnsubmit_Click" />
                    <%--</ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnUnsubmit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
               <%-- <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>--%>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidQuestionId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidIsConfigured" runat="server" Value="N" />
                        <asp:HiddenField ID="hidQuestionFilePath" runat="server" Value="" />
                        <asp:HiddenField ID="hidAnswerFilePath1" runat="server" Value="" />
                        <asp:HiddenField ID="hidAnswerFilePath2" runat="server" Value="" />
                        <asp:HiddenField ID="hidAnswerFilePath3" runat="server" Value="" />
                        <asp:HiddenField ID="hidAnswerFilePath4" runat="server" Value="" />
                        <asp:HiddenField ID="hidIsEditMode" runat="server" Value="N" />
                        <asp:HiddenField ID="hidAreYouSureYouWantDeleteEvent" runat="server" />
                        <asp:HiddenField ID="hidAnswerId1" runat="server" Value="0" />
                        <asp:HiddenField ID="hidAnswerId2" runat="server" Value="0" />
                        <asp:HiddenField ID="hidAnswerId3" runat="server" Value="0" />
                        <asp:HiddenField ID="hidAnswerId4" runat="server" Value="0" />
                   <%-- </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />                        
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStandard" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivision" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSubject" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwConfigure" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="imgDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnImgAnsDelete1" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnImgAnsDelete2" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnImgAnsDelete3" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnImgAnsDelete4" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
        </tr>    
        <%--<tr id="trCopy" runat="server" visible="false">
            <td align="center">
                <table>
                    <tr id="tr2" runat="server" visible="true">
                        <td align="right" class="ClsBorderlight" valign="middle">
                            <span class="LblRht colonPadding"></span>
                            <asp:Label ID="Label2" runat="server" Text="Applicable to selected Class(es) :" CssClass="LblRht"
                                EnableViewState="False"></asp:Label><br />
                            <asp:CheckBox ID="chkAllDivForVdo" runat="server" Text="<%$ Resources:LocalizedResources, SelectAll%>"
                                TabIndex="7" Style="padding-right: 5px" onclick="CheckAll1(this);" />
                        </td>
                        <td align="left">
                            <asp:ListView ID="lstvwVideoStandardDivision" runat="server" DataKeyNames="StandardId"
                                OnItemDataBound="lstvwVideoStandardDivision_ItemDataBound">
                                <LayoutTemplate>
                                    <table align="left" width="auto" runat="server" id="tblStaffInfo" style="color: #333333;"
                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                        </td>
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                CssClass="ClsLabel" RepeatColumns="6">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trGridRow" runat="server" class="ClsGridAltRow" style="height: 10px">
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBox ID="chkVdoStandard" runat="server" Text='<%# Eval("StandardName") %>' />
                                        </td>
                                        <td align="left" style="padding-left: 5px">
                                            <asp:CheckBoxList ID="chkvideoStandardDivLst" runat="server" RepeatDirection="Horizontal"
                                                CssClass="ClsLabel" RepeatColumns="6">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound%>"
                                                    EnableViewState="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <span class="ClsMdtStar" style="color: Red">*olor: Red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnCopy" runat="server" CssClass="ClsBtn" Text="Copy" CausesValidation="true"
                                OnClick="btnCopy_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>--%>
    </table>
    <script>
        var _clienttxtAns1 = '<%=this.txtAns1.ClientID %>';
        var _clienttxtAns2 = '<%=this.txtAns2.ClientID %>';
        var _clienttxtAns3 = '<%=this.txtAns3.ClientID %>';
        var _clienttxtAns4 = '<%=this.txtAns4.ClientID %>';
        var _clientcmbClass = '<%=this.ddlDivision.ClientID %>';
        var _clientcmbSubject = '<%=this.ddlSubject.ClientID %>';
        var _clientrdOption1 = '<%=this.rdOption1.ClientID %>';
        var _clientrdOption2 = '<%=this.rdOption2.ClientID %>';
        var _clientrdOption3 = '<%=this.rdOption3.ClientID %>';
        var _clientrdOption4 = '<%=this.rdOption4.ClientID %>';

        _clientcstvalAnser1 = "<%=this.cstvalAnser1.ClientID %>"
        _clientcstvalOption = "<%=this.cstvalOption.ClientID %>"

        _clienttxtQuestion = "<%=this.txtQuestion.ClientID %>"
        _clienthidQuestionId = '<%=this.hidQuestionId.ClientID %>'

        _clientfuQuestion = "<%=this.fuQuestion.ClientID %>"
        _clientcmbAnswerType = "<%=this.cmbAnswerType.ClientID %>"
        _clientfuAnswer1 = "<%=this.fuAnswer1.ClientID %>"
        _clientfuAnswer2 = "<%=this.fuAnswer2.ClientID %>"
        _clientfuAnswer3 = "<%=this.fuAnswer3.ClientID %>"
        _clientfuAnswer4 = "<%=this.fuAnswer4.ClientID %>"
        _clienthidQuestionFilePath = "<%=this.hidQuestionFilePath.ClientID %>"
        _clienthidAnswerFilePath1 = "<%=this.hidAnswerFilePath1.ClientID %>"
        _clienthidAnswerFilePath2 = "<%=this.hidAnswerFilePath2.ClientID %>"
        _clienthidAnswerFilePath3 = "<%=this.hidAnswerFilePath3.ClientID %>"
        _clienthidAnswerFilePath4 = "<%=this.hidAnswerFilePath4.ClientID %>"
        _clienthidIsEditMode = "<%=this.hidIsEditMode.ClientID %>"


        function ValidaterdoOption(oSrc, args) {            
            var option1 = document.getElementById(_clientrdOption1).checked;
            var option2 = document.getElementById(_clientrdOption2).checked;
            var option3 = document.getElementById(_clientrdOption3).checked;
            var option4 = document.getElementById(_clientrdOption4).checked;            

            var AnswerTypeId = document.getElementById(_clientcmbAnswerType).value;

            var bIsValid = true;

            if (AnswerTypeId == "1") {
                var txtAnswer1 = document.getElementById(_clienttxtAns1).value.trim();
                var txtAnswer2 = document.getElementById(_clienttxtAns2).value.trim();
                var txtAnswer3 = document.getElementById(_clienttxtAns3).value.trim();
                var txtAnswer4 = document.getElementById(_clienttxtAns4).value.trim();

                if (!((txtAnswer1 != '' && option1 == true) || (txtAnswer2 != '' && option2 == true) || (txtAnswer3 != '' && option3 == true) || (txtAnswer4 != '' && option4 == true))) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalOption).errormessage = "At least one answer should be marked as correct answer out of entered answers.";
                }
            }
            else if (AnswerTypeId == "2") {            
                var oFileName1 = document.getElementById(_clientfuAnswer1).value;
                var oFileName2 = document.getElementById(_clientfuAnswer2).value;
                var oFileName3 = document.getElementById(_clientfuAnswer3).value;
                var oFileName4 = document.getElementById(_clientfuAnswer4).value;

                var oHidFileName1 = document.getElementById(_clienthidAnswerFilePath1).value;
                var oHidFileName2 = document.getElementById(_clienthidAnswerFilePath2).value;
                var oHidFileName3 = document.getElementById(_clienthidAnswerFilePath3).value;
                var oHidFileName4 = document.getElementById(_clienthidAnswerFilePath4).value;

                var EditMode = document.getElementById(_clienthidIsEditMode).value;

                var file1 = '',
                    file2 = '',
                    file3 = '',
                    file4 = ''

                if (oFileName1 != '')
                    file1 = oFileName1
                else
                    file1 = oHidFileName1

                if (oFileName2 != '')
                    file2 = oFileName2
                else
                    file2 = oHidFileName2

                if (oFileName3 != '')
                    file3 = oFileName3
                else
                    file3 = oHidFileName3

                if (oFileName4 != '')
                    file4 = oFileName4
                else
                    file4 = oHidFileName4

                if (!((file1 != "" && option1 == true) || (file2 != "" && option2 == true) || (file3 != "" && option3 == true) || (file4 != "" && option4 == true))) {
                    bIsValid = false;
                    document.getElementById(_clientcstvalOption).errormessage = "At least one answer should be marked as correct answer out of entered answers.";
                }

//                if (EditMode == "Y") {
//                    if (!((oHidFileName1 != "" && option1 == true) || (oHidFileName2 != "" && option2 == true) || (oHidFileName3 != "" && option3 == true) || (oHidFileName4 != "" && option4 == true))) {
//                        bIsValid = false;
//                        alert('test 123')
//                        alert(option1)
//                        alert(option2)
//                        alert(option3)
//                        alert(option4)
//                        document.getElementById(_clientcstvalOption).errormessage = "At least one answer should be marked as correct answer out of entered answers.";
//                    }
//                }
//                else {
//                    if (!((oFileName1 != "" && option1 == true) || (oFileName2 != "" && option2 == true) || (oFileName3 != "" && option3 == true) || (oFileName4 != "" && option4 == true))) {                        
//                        bIsValid = false;                        
//                        document.getElementById(_clientcstvalOption).errormessage = "At least one answer should be marked as correct answer out of entered answers.";
//                    }
                //                }

            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateAnswerName(oSrc, args) {
            var iAnswerType = document.getElementById(_clientcmbAnswerType).value;
            var bIsValid = true;
            if (iAnswerType == "1") {
                var txtAnswer = document.getElementById(_clienttxtAns1).value;
                var txtAnswer2 = document.getElementById(_clienttxtAns2).value;                

                if (txtAnswer.trim() == "" || txtAnswer2.trim() == "") {
                    bIsValid = false;
                    document.getElementById(_clientcstvalAnser1).errormessage =  "At least 2 answers should be entered.";
                }
                args.IsValid = bIsValid;
                return !bIsValid;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this Question?')) {
                bResult = false
            }
            return bResult
        }
                
        function CheckAllForVideo(obj, index) {
            var id = 'ctrl' + index + '_chkvideoStandardDivLst_'
            $('[id*=' + id + ']').prop('checked', obj.checked)
            CheckMainForVideo();
        }

        function CheckMainForVideo() {
            if ($('[id$=chkVdoStandard]').length == $('[id$=chkVdoStandard]:checked').length)
                $('[id$=chkAllDivForVdo]').prop('checked', true)
            else
                $('[id$=chkAllDivForVdo]').prop('checked', false)

        }
        function CheckStdForVideo(index) {
            var classId = 'ctrl' + index + '_chkvideoStandardDivLst_'
            var stdId = 'ctrl' + index + '_chkVdoStandard'

            if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                $('[id$=' + stdId + ']').prop('checked', true)
            else
                $('[id$=' + stdId + ']').prop('checked', false)

            CheckMainForVideo();
        }

        function SelectAllDivisionsForVideo(obj) {
            $('[id$=chkVdoStandard]').prop('checked', obj.checked)
            $('[id*=chkvideoStandardDivLst]').prop('checked', obj.checked)

        }
        function CheckAll(obj, index) {
            var id = 'ctrl' + index + '_chkStandardDivLst_'
            if (obj.checked) {
                $('[id*=' + id + ']').attr('checked', 'checked')
            }
            else {
                $('[id*=' + id + ']').removeAttr('checked')
            }

            CheckMain();
        }

        function CheckMain() {
            if ($('[id$=chkStandard]').length == $('[id$=chkStandard]:checked').length)
                $('[id$=chkAllDivs]').attr('checked', 'checked')
            else
                $('[id$=chkAllDivs]').removeAttr('checked')
        }

        function CheckAllForVideo(obj, index) {
            var id = 'ctrl' + index + '_chkvideoStandardDivLst_'
            $('[id*=' + id + ']').prop('checked', obj.checked)
            CheckMainForVideo();
        }

        function CheckMainForVideo() {
            if ($('[id$=chkVdoStandard]').length == $('[id$=chkVdoStandard]:checked').length)
                $('[id$=chkAllDivForVdo]').prop('checked', true)
            else
                $('[id$=chkAllDivForVdo]').prop('checked', false)

        }

        function SelectAllDivisions(obj) {
            if (obj.checked) {
                $('[id$=chkStandard]').attr('checked', 'checked')
                $('[id*=chkStandardDivLst]').attr('checked', 'checked')
            }
            else {
                $('[id$=chkStandard').removeAttr('checked')
                $('[id*=chkStandardDivLst]').removeAttr('checked')
            }
        }

        function SelectAllDivisionsForVideo(obj) {
            $('[id$=chkVdoStandard]').prop('checked', obj.checked)
            $('[id*=chkvideoStandardDivLst]').prop('checked', obj.checked)

        }

        function CheckStd(index) {
            var classId = 'ctrl' + index + '_chkStandardDivLst_'
            var stdId = 'ctrl' + index + '_chkStandard'

            if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                $('[id$=' + stdId + ']').attr('checked', 'checked')
            else
                $('[id$=' + stdId + ']').removeAttr('checked')

            CheckMain();
        }

        function CheckStdForVideo(index) {
            var classId = 'ctrl' + index + '_chkvideoStandardDivLst_'
            var stdId = 'ctrl' + index + '_chkVdoStandard'

            if ($('[id*=' + classId + ']').length == $('[id*=' + classId + ']:checked').length)
                $('[id$=' + stdId + ']').prop('checked', true)
            else
                $('[id$=' + stdId + ']').prop('checked', false)

            CheckMainForVideo();
        }

        function ClearMessageText() {
            document.getElementById("<%=this.lblUpdateMessage.ClientID %>").innerHTML = ''
        }

        function ValidateQuestion(oSrc, args) {        
            var data = new Array();
            var found = false;

            var question = $('#' + _clienttxtQuestion).val().trim()
            var qstId = $('#' + _clienthidQuestionId).val()

            $('[id$=lblQuestion]').each(function () {
                var qst = $(this).html().trim()
                var hidId = $('#' + $(this)[0].id.replace('lblQuestion', 'hidId')).val()

                if (question == qst && qstId != hidId) {
                    found = true;                    
                }

            });

            if (found) {
                oSrc.errormessage = "Question should not be duplicate."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateAnswers(oSrc, args) {        
            var data = new Array();
            var found = false;
            $('[id*=txtAns]').each(function () {
                var ans = $(this).val().trim()

                if (ans != '') {
                    if (data.indexOf(ans) == -1)
                        data.push(ans)
                    else
                        found = true;
                }
            });

            if (found) {
                oSrc.errormessage = "Answer should not be duplicate."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function validateFile(oSrc, args) {            
            var oFileName = document.getElementById(_clientfuQuestion).value
            var oHidFileName = document.getElementById(_clienthidQuestionFilePath).value
            
            if (oFileName != "" && oHidFileName == "") {
                if (ValidateFileExtension(oFileName)) {
                    oSrc.errormessage = "File to upload should be in valid format."
                    args.IsValid = false
                    return true
                }
            }            
            args.IsValid = true
            return false
        }

        function ValidateAnswerFiles(oSrc, args) {                            
            var iAnswerType = document.getElementById(_clientcmbAnswerType).value;
            if (iAnswerType == "2") {
                var oFileName1 = document.getElementById(_clientfuAnswer1).value;
                var oFileName2 = document.getElementById(_clientfuAnswer2).value;
                var oFileName3 = document.getElementById(_clientfuAnswer3).value;
                var oFileName4 = document.getElementById(_clientfuAnswer4).value;
                var oHidFileName1 = document.getElementById(_clienthidAnswerFilePath1).value;
                var oHidFileName2 = document.getElementById(_clienthidAnswerFilePath2).value;
                var oHidFileName3 = document.getElementById(_clienthidAnswerFilePath3).value;
                var oHidFileName4 = document.getElementById(_clienthidAnswerFilePath4).value;

                var iCount = 0;
                var iFileUploadCount = 0;

                if (oFileName1 != "" && oHidFileName1 == "") {
                    iFileUploadCount = iFileUploadCount + 1;
                    if (ValidateFileExtension(oFileName1)) {
                        iCount = iCount + 1;                        
                    }
                }
                if (oFileName2 != "" && oHidFileName2 == "") {
                    iFileUploadCount = iFileUploadCount + 1;
                    if (ValidateFileExtension(oFileName2)) {
                        iCount = iCount + 1;                        
                    }
                }
                if (oFileName3 != "" && oHidFileName3 == "") {
                    iFileUploadCount = iFileUploadCount + 1;
                    if (ValidateFileExtension(oFileName3)) {
                        iCount = iCount + 1;                    
                    }
                }
                if (oFileName4 != "" && oHidFileName4 == "") {
                    iFileUploadCount = iFileUploadCount + 1;
                    if (ValidateFileExtension(oFileName4)) {
                        iCount = iCount + 1;                        
                    }
                }

                if (oHidFileName1 == "" && oHidFileName2 == "" && oHidFileName3 == "" && oHidFileName4 == "" && iFileUploadCount < 2) {
                    oSrc.errormessage = "At least 2 answer file should be uploaded."
                    args.IsValid = false
                    return true
                }

                if (iCount > 0) {
                    oSrc.errormessage = "File to upload should be in valid format."
                    args.IsValid = false
                    return true
                }
            }
        }

        function ValidateFileExtension(oFileName) {
            if (oFileName.toUpperCase().indexOf(".JPG") == -1 && oFileName.toUpperCase().indexOf(".PNG") == -1 && oFileName.toUpperCase().indexOf(".BMP") == -1 && oFileName.toUpperCase().indexOf(".JPEG") == -1) {                
                return true
            }
            else if (oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".JPG" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".PNG" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".BMP" && oFileName.toUpperCase().substring(oFileName.indexOf(".")) != ".JPEG") {                
                return true
            }
        }

        //This function is used to open popun on click on link annual planner.
        function OpenWindow(sfilepath) {
            window.open(sfilepath);
            return false;
        }

        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantDeleteEvent.ClientID %>").value)) {
                bResult = false;
            }
            return bResult;
        }

        function VisibleHideControls() {        
            var iAnswerType = document.getElementById(_clientcmbAnswerType).value;
            if (iAnswerType == "1") {
                $("[id$=trAnswerDetails]").show();
                $("[id$=tdTxtAnswer1]").show();
                $("[id$=tdTxtAnswer2]").show();
                $("[id$=tdTxtAnswer3]").show();
                $("[id$=tdTxtAnswer4]").show();
                $("[id$=tdFUAnswer1]").hide();
                $("[id$=tdFUAnswer2]").hide();
                $("[id$=tdFUAnswer3]").hide();
                $("[id$=tdFUAnswer4]").hide();
            }
            else if (iAnswerType == "2") {
                $("[id$=trAnswerDetails]").show();
                $("[id$=tdTxtAnswer1]").hide();
                $("[id$=tdTxtAnswer2]").hide();
                $("[id$=tdTxtAnswer3]").hide();
                $("[id$=tdTxtAnswer4]").hide();
                $("[id$=tdFUAnswer1]").show();
                $("[id$=tdFUAnswer2]").show();
                $("[id$=tdFUAnswer3]").show();
                $("[id$=tdFUAnswer4]").show();
            }
            else if (iAnswerType == "3") {
                $("[id$=trAnswerDetails]").hide();
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            VisibleHideControls();
        }

        function beginRequestHandler(sender, args) {
        }

        function OpenFormulaScreen() {
            window.open('MathFormula.aspx', '_new', '')
            return false;
        }

    </script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
