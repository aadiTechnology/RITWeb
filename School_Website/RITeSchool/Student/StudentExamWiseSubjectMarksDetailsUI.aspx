<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    CodeFile="StudentExamWiseSubjectMarksDetailsUI.aspx.cs" Inherits="StudentExamWiseSubjectMarksDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <div style="float: right;">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="Mandatory Fields"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:LinkButton ID="lnkbtnGradeConfigurationDetails" runat="server" CssClass="SMSLblSMlBlue"
                        Style="vertical-align: bottom; padding-left: 10px; font-size: 9pt; font-weight: bold;
                        font-family: Verdana; text-decoration: underline;" Visible="false">Grade Configuration Details</asp:LinkButton>
                </td>
            </tr>
            <tr id="trControls" runat="server">
                <td align="center">
                    <table style="width: 100%">
                        <tr>
                            <td id="tdMessage" runat="server" align="center" style="height: 10px">
                                <asp:UpdatePanel ID="upnlSuccessMsg" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblmessage" runat="server" Text="" EnableViewState="false" ForeColor="Blue"
                                            Font-Bold="True" CssClass="LblNormal"></asp:Label><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr id="trComboboxes" runat="server">
                            <td align="center">
                                <table>
                                    <tr>
                                        <td align="right" class="ClsBorderlight">
                                            <span class="ClsLabel">Exam :</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbExam" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                AutoPostBack="True" OnTextChanged="cmbExam_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="ClsMdtStar">*</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <table style="max-width: 700px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ListView ID="lstvwExamwiseSubjectMarkDetails" runat="server" DataKeyNames="TestId"
                                                        OnItemDataBound="lstvwExamwiseSubjectMarkDetails_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table style="width: 100%" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr class="ClsGridHeader">
                                                                    <th align="left" style="width: 40%">
                                                                        <span class="ClsLabel">Subject Name</span>
                                                                    </th>
                                                                    <th align="center" style="width: 20%">
                                                                        <span class="ClsLabel" style="float: inherit;">Marks</span>
                                                                    </th>
                                                                    <th align="center" style="width: 20%">
                                                                        <span class="ClsLabel" style="float: inherit;">Out Of Marks</span>
                                                                    </th>
                                                                    <th align="center" style="width: 20%">
                                                                        <span class="ClsLabel" style="float: inherit;">Grade</span>
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceHolder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="Tr2" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                <td align="left">
                                                                    <asp:Label ID="lblClass" runat="server" CssClass="ClsLabel" Text='<%#Eval("SubjectName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblMarks" runat="server" CssClass="ClsLabel" Style="float: inherit;"
                                                                        Text='<%#Eval("Marks") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOutOfMarks" runat="server" CssClass="ClsLabel" Style="float: inherit;"
                                                                        Text='<%#Eval("OutOfMarks") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblGrades" runat="server" CssClass="ClsLabel" Style="float: inherit;"
                                                                        Text='<%#Eval("Grade") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    <asp:Label ID="lblNoRecFound" runat="server" Text="Exam is not published."></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbExam" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="divPopup" style="display: none; background-image: url(../images/BGline.gif);
                        background-repeat: repeat;">
                        <table align="center" style="width: 90%">
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwGradeConfigurationDetailsSubject" runat="server">
                                        <LayoutTemplate>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">Subjects :</span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                                <tr align="right" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="font-size: 9pt;">
                                                        Percentage
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 100px; font-size: 9pt;">
                                                        <span style="float: inherit">Grade Name</span>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 150px; font-size: 9pt;" id="thRemarkSub"
                                                        runat="server">
                                                        Remarks
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblStartingMarkRange" runat="server" Text='<%# Eval("Starting_Marks_Range") %>' />
                                                    -
                                                    <asp:Label ID="lblEndingMarkRange" runat="server" Text='<%# Eval("Ending_Marks_Range") %>' />
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Label ID="lblGradeName" runat="server" Style="float: inherit" Text='<%# Eval("Grade_Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL" id="tdRemark" runat="server">
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwGradingConfigurationDetailsCurricularSubject" runat="server">
                                        <LayoutTemplate>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">Co-Curricular Subjects :</span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                                <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="font-size: 9pt;">
                                                        Percentage
                                                    </th>
                                                    <th align="center" class="paddingL" style="width: 100px; font-size: 9pt;">
                                                        <span style="float: inherit">Grade Name</span>
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 150px; font-size: 9pt;" id="thRemarkSub"
                                                        runat="server">
                                                        Remarks
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblStartingMarkRange" runat="server" Text='<%# Eval("Starting_Marks_Range") %>' />
                                                    -
                                                    <asp:Label ID="lblEndingMarkRange" runat="server" Text='<%# Eval("Ending_Marks_Range") %>' />
                                                </td>
                                                <td align="center" class="paddingL">
                                                    <asp:Label ID="lblGradeName" runat="server" Style="float: inherit" Text='<%# Eval("Grade_Name") %>' />
                                                </td>
                                                <td align="left" class="paddingL" id="tdRemark" runat="server">
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false"
                                        OnClientClick="HidePopup();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script>
        function OpenPopup() {
            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Grade Configuration Details", visible: false, modal: true, resizable: false, width: '450px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }
        function HidePopup() {
            ContentWindow = $('#divPopup').kendoWindow({ title: "Grade Configuration Details", visible: false, modal: true, resizable: false, width: '450px' }).data("kendoWindow"); ContentWindow.close(); ContentWindow.center();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
