<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OnlineExamStudentListUI.aspx.cs" Inherits="OnlineExamStudentListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="true"
                        ShowSummary="false" ValidationGroup="Answer" />
                    <asp:CustomValidator ID="cstValidateAnswer" runat="server" ErrorMessage="" ClientValidationFunction="ValidateMarks"
                        Display="None" ValidationGroup="Answer"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValidateAnswerFiles" runat="server" ClientValidationFunction="CompareOutOfMarks"
                        Display="None" ValidationGroup="Answer"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td id="tdMessage" runat="server" align="center">
                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label><br />
                </td>
            </tr>
            <tr>
                <td valign="top" style="background-color: white;" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListView ID="lstVwStudentList" runat="server" DataKeyNames="YearWise_Student_Id, DescriptionFileName"
                                OnItemCommand="lstVwStudentList_ItemCommand" OnItemDataBound="lstVwStudentList_ItemDataBound">
                                <LayoutTemplate>
                                    <table width="50%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder" align="center">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="center" class="ClspaddingL" width="10%">
                                                Roll No
                                            </th>
                                            <th align="left" class="ClspaddingL">
                                                Student Name
                                            </th>
                                            <th id="Th6" runat="server" width="10%">
                                                <asp:Label ID="lblSelectStudent" runat="server" Text="Action"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                        <td align="center">
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Roll_No") %>' />
                                        </td>
                                        <td align="left" class="ClspaddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>' />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                CommandName="SelectCommand" ToolTip="Select" />
                                        </td>
                                    </tr>
                                    <tr id="trtxtQty" runat="server" visible="false">
                                        <td id="tdtxtQty" runat="server" colspan="3" style="padding-right: 10px;">
                                            <table width="90%" align="center">
                                                <tr>
                                                    <td align="center">
                                                        <asp:ListView ID="lstVwQuestionDetails" DataKeyNames="QuestionId,AnswerId, QuestionAnswerId"
                                                            runat="server" OnItemDataBound="lstVwQuestionDetails_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder" align="center">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" class="ClspaddingL">
                                                                                        Question
                                                                                    </th>
                                                                                    <th align="center" width="100px">
                                                                                        Out Of Marks
                                                                                    </th>
                                                                                    <th align="center" width="80px">
                                                                                        Marks
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblOutOfMarks" CssClass="lbl" runat="server" Text='<%# Eval("OutOfMarks") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="txtMarks" runat="server" Text='<%# Eval("MarkScored") %>' CssClass="TxtAlignCenter"
                                                                            Width="80px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" ondrop="event.returnValue=false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trDescriptionAttachment" runat="server" visible="false">
                                        <td align="center" colspan="2">
                                            <b>
                                                <asp:LinkButton ID="lnkDescription" Width="300px" runat="server" CssClass="SMSLblSMlBlue"
                                                    Text="Answer Attachment" CausesValidation="false" />
                                            </b>
                                        </td>
                                    </tr>
                                    <tr id="trbtnSave" runat="server" visible="false">
                                        <td align="center" colspan="2">
                                            <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" ValidationGroup="Answer"
                                                Text="Save" BorderWidth="1px" OnClick="BtnSave_Click" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                        <td align="center">
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Roll_No") %>' />
                                        </td>
                                        <td align="left" class="ClspaddingL">
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentName") %>' />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                CommandName="SelectCommand" ToolTip="Select" />
                                        </td>
                                    </tr>
                                    <tr id="trtxtQty" runat="server" visible="false">
                                        <td id="tdtxtQty" runat="server" colspan="3" style="padding-right: 10px;">
                                            <table width="90%" align="center">
                                                <tr>
                                                    <td align="center">
                                                        <asp:ListView ID="lstVwQuestionDetails" runat="server" DataKeyNames="QuestionId,AnswerId, QuestionAnswerId"
                                                            OnItemDataBound="lstVwQuestionDetails_ItemDataBound">
                                                            <LayoutTemplate>
                                                                <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                    cellspacing="1" class="GridBorder" align="center">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1">
                                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                    <th align="left" class="ClspaddingL">
                                                                                        Question
                                                                                    </th>
                                                                                    <th align="center" width="100px">
                                                                                        Out Of Marks
                                                                                    </th>
                                                                                    <th align="center" class="ClspaddingL" width="80px">
                                                                                        Marks
                                                                                    </th>
                                                                                </tr>
                                                                                <tr id="itemPlaceholder" runat="server">
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                    <td align="left" class="ClspaddingL">
                                                                        <asp:Label ID="lblQuestion" runat="server" Text='<%# Eval("Question") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblOutOfMarks" runat="server" Text='<%# Eval("OutOfMarks") %>' />
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:TextBox ID="txtMarks" runat="server" Text='<%# Eval("MarkScored") %>' CssClass="TxtAlignCenter"
                                                                            Width="80px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" ondrop="event.returnValue=false"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trDescriptionAttachment" runat="server" visible="false">
                                        <td align="center" colspan="2">
                                            <b>
                                                <asp:LinkButton ID="lnkDescription" Width="300px" runat="server" CausesValidation="false"
                                                    CssClass="SMSLblSMlBlue" Text="Answer Attachment" /></b>
                                        </td>
                                    </tr>
                                    <tr id="trbtnSave" runat="server" visible="false">
                                        <td align="center" colspan="2">
                                            <asp:Button CssClass="ClsBtn" ID="BtnSave" runat="server" ValidationGroup="Answer"
                                                Text="Save" BorderWidth="1px" OnClick="BtnSave_Click" />
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstVwStudentList" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center; margin: 0px auto;">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
        <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
        <asp:HiddenField ID="hidIsPublished" runat="server" Value="N" />
    </div>
    <script language="javascript" type="text/javascript">

        //This function is used to open popun on click on link annual planner.
        function OpenWindow(sfilepath) {
            window.open(sfilepath);
            return false;
        }

        function ValidateMarks(oSrc, args) {
            var iCount = 0;
            $('[id$=txtMarks]').each(function () {
                var marks = $(this).val()
                if (marks == "") {
                    iCount = iCount + 1;
                }
            });

            if (iCount > 0) {
                oSrc.errormessage = "Marks Should not be blank.";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CompareOutOfMarks(oSrc, args) {
            var iCount = 0;
            $('[id$=txtMarks]').each(function () {
                var marks = $(this).val()
                var id = this.id.replace("txtMarks", "lblOutOfMarks")
                var outOfMarks = $('#' + id).html()

                if (marks != "" && (parseInt(marks) > parseInt(outOfMarks))) {
                    iCount = iCount + 1;
                }
            });

            if (iCount > 0) {
                oSrc.errormessage = "Marks Should not be grater than Out Of Marks.";
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

    </script>
</asp:Content>
