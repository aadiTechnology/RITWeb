<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentOnlineExamUI.aspx.cs" Inherits="StudentOnlineExamUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
<script type="text/javascript" src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
<script id="MathJax-script" type="text/javascript" async src="https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js">
</script>
    <%--<asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>--%>
            <table width="100%">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="true"
                            ShowSummary="false" ValidationGroup="Answer" />
                                <asp:CustomValidator ID="cstValidateAnswer" runat="server" ErrorMessage=""  ClientValidationFunction="ValidateAnswers"
                            Display="None" ValidationGroup="Answer"></asp:CustomValidator>
                        <asp:CustomValidator ID="cstValidateAnswerFiles" runat="server" ClientValidationFunction="ValidateFiles"
                            Display="None" ValidationGroup="Answer"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="CheckFileSize"
                            Display="None" ValidationGroup="Answer"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td id="tdMessage" runat="server" align="center">
                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center" style="text-align: center; margin: 0px auto;" width="80%">
                            <tr>
                                <td>
                                    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000" Enabled="false">
                                    </asp:Timer>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="80%" align="center" style="text-align: center; margin: 0px auto;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="clsBorderLight" style="width: 100px;">
                                                            <span class="clsLabel">Exam : </span>
                                                        </td>
                                                        <td class="ClsHilightBG">
                                                            <asp:Label ID="lblExam" runat="server" Text="" CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clsBorderLight">
                                                            <span class="clsLabel">Subject : </span>
                                                        </td>
                                                        <td class="ClsHilightBG">
                                                            <asp:Label ID="lblSubject" runat="server" Text="" CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="clsBorderLight">
                                                            <span class="clsLabel">Date / Time : </span>
                                                        </td>
                                                        <td class="ClsHilightBG">
                                                            <asp:Label ID="lblDateTime" runat="server" Text="" CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwQuestionDetails" runat="server" DataKeyNames="QuestionId, Marks, AnswerTypeId"
                                                    OnItemDataBound="lstvwQuestionDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                <th align="left" class="clsLabelgrd">
                                                                    <span><b>Question</b></span>
                                                                </th>
                                                                <th align="center" width="80px" class="clsLabelgrd">
                                                                    <span><b>Mark</b></span>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                            <td align="left" style="font-weight: bold;">
                                                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                    Text='<%#Eval("SerialNo") %>'></asp:Label>
                                                                <asp:Label ID="lblQuestion" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                    Text='<%#Eval("Question") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" style="font-weight: bold;">
                                                                <asp:Label ID="lblMarks" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                    Text='<%#Eval("Marks") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trQuestionAttachment" runat="server" visible="false">
                                                            <td align="left">
                                                                <asp:Image ID="imgQuestionAttachment" runat="server" Width="100%" Height="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trAnswerDetails" runat="server" >
                                                            <td id="tdAnswerDetails" runat="server" colspan="3">
                                                                <table width="100%" align="left">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:ListView ID="lstvwAnswerDetails" runat="server" DataKeyNames="AnswerId, IsCorrectAnswer, UserSelectedAnswer"
                                                                                OnItemDataBound="lstvwAnswerDetails_ItemDataBound">
                                                                                <LayoutTemplate>
                                                                                    <table width="100%" cellpadding="0">
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                        <td style="width: 50px;">
                                                                                            <asp:RadioButton ID="rdoCorrectAnswer" runat="server" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblAnswer" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("Answer") %>'></asp:Label>
                                                                                            <asp:Image ID="imgAttachment" runat="server" Visible="false" Width="50px" Height="50px" />                                                                                               
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="Tr3" runat="server" class="ClsGridRow">
                                                                                        <td style="width: 50px;">
                                                                                            <asp:RadioButton ID="rdoCorrectAnswer" runat="server" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblAnswer" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("Answer") %>'></asp:Label>
                                                                                            <asp:Image ID="imgAttachment" runat="server" Visible="false" Width="50px" Height="50px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="text-align:left; margin:0px auto; padding-left:22px;" >
                                                                <asp:LinkButton ID="lnkClearFields" runat="server"><b>Clear</b></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:10px;"></td>
                                        </tr>
                                        <tr id="trDescription" runat="server" visible="false">
                                            <td>
                                                 <table width="100%" align="left" style="text-align:left;">
                                                    <tr>
                                                        <td class="ClsBorderlight" style="text-align:left; width:150px;">
                                                            Upload File : 
                                                        </td>
                                                        <td class="ClsBorderlight" style="text-align:left;">
                                                            <asp:FileUpload ID="fuDescriptionAnswer" runat="server" Width="250px" />
                                                            <asp:ImageButton ID="btnView" runat="server"  CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible = "false"  />                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>                                                            
                                                        </td>
                                                        <td class="ClsBorderlight">
                                                            <span class="LblSmlGray"><b>(Supports only .PDF, .DOC & .DOCX file type. File size should not exceed 25MB.)</b>&nbsp; &nbsp;&nbsp;</span>
                                                        </td>
                                                    </tr>
                                                 </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:10px;"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 100px; background-color: #ffffc4;" class="ClsBorderlight">
                                                            <span class="clsLabel">Note 1</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="clsLabel">Save given answers continuously after some time interval.</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px; background-color: #ffffc4;" class="ClsBorderlight">
                                                            <span class="clsLabel">Note 2</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="clsLabel">On time out system will auto save and submit given answers.</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hidStdDivionId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidExamId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSubjectId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidQuestionId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidExamStartTime" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidExamEndTime" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidAnswerTypeId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidDescriptionFilePath" runat="server" Value="" />
                                    <asp:HiddenField ID="hidAreYouSureYouWantDeleteEvent" runat="server" Value="" />
                                       <asp:HiddenField ID="hidAnswerTypeId1" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
                <tr align="center" style="text-align: center; margin: 0px auto;">
                    <td align="center" style="text-align: center; margin: 0px auto;">
                        <asp:Button CssClass="ClsBtn" ID="btnBack" runat="server" Text="Back" BorderWidth="1px"
                            CausesValidation="false"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" BorderWidth="1px"
                            OnClick="btnSave_Click" ValidationGroup="Answer"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnClear" CausesValidation="false" runat="server"
                            Text="Clear" BorderWidth="1px" OnClick="btnClear_Click"></asp:Button>
                        <asp:Button CssClass="ClsBtn" ID="btnSubmit" runat="server" Text="Submit" BorderWidth="1px"
                            OnClick="btnSubmit_Click" Enabled="false"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                    </td>
                </tr>
            </table>
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <script language="javascript" type="text/javascript">

        _clientlstvwQuestionDetails = "<%=this.lstvwQuestionDetails.ClientID %>"
        _clientfuDescriptionAnswer = "<%=this.fuDescriptionAnswer.ClientID %>"
        _clienthidAnswerTypeId = "<%=this.hidAnswerTypeId.ClientID %>"
        _clientcstvalOption = "<%=this.cstValidateAnswer.ClientID %>"
        _cliencstValidateAnswerFiles = "<%=this.cstValidateAnswerFiles.ClientID %>"
   
        function CheckSelected(id, index) {
            var optId = id.replace(index + '_rdoCorrectAnswer', '')
            var x = 0;

            var newId = document.getElementById(optId + x + '_rdoCorrectAnswer')
            while (newId != null) {
                if (id != newId.id)
                    newId.checked = false;

                x++
                newId = document.getElementById(optId + x + '_rdoCorrectAnswer')
            }
        }


        function ValidateAnswers(oSrc, args) {
            var iAnswerTypId = document.getElementById(_clienthidAnswerTypeId).value;
                if (iAnswerTypId != "3") {
                    if ($('[id$=rdoCorrectAnswer]:checked').length == 0) {
                        document.getElementById(_clientcstvalOption).errormessage = "Atleast one answer should be selected.";
                        args.IsValid = false;
                        return true;
                    }
                }
        
            args.IsValid = true
            return false
        }


        function ValidateFiles(oSrc, args) {
            var iAnswerTypId = document.getElementById(_clienthidAnswerTypeId).value;
            if (iAnswerTypId == 3) {
                var oFileName = document.getElementById(_clientfuDescriptionAnswer).value;
                if (oFileName != "") {
                    if (oFileName.toUpperCase().indexOf(".PDF") == -1 && oFileName.toUpperCase().indexOf(".DOC") == -1 && oFileName.toUpperCase().indexOf(".DOCX") == -1) {
                        oSrc.errormessage = "File to upload should be in valid format."
                        args.IsValid = false
                        return true
                    }
                }
                else {
                    oSrc.errormessage = "File should be uploaded."
                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true
            return false
        }

        function CheckFileSize(oSrc, args) {            
            var iAnswerTypId = document.getElementById(_clienthidAnswerTypeId).value;
            if (iAnswerTypId == 3) {
                var FileUpload1 = document.getElementById('<%=fuDescriptionAnswer.ClientID %>')
                var File1Size = FileUpload1.files[0].size;
                var MaxSize = 26214400;
                if (File1Size > MaxSize) {
                    oSrc.errormessage = "File size should not be greater than 25MB."
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
            args.IsValid = true
            return false
        }


        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantDeleteEvent.ClientID %>").value)) {
                bResult = false;
            }
            return bResult;
        }

        //This function is used to open popun on click on link annual planner.
        function OpenWindow(sfilepath) {
            window.open(sfilepath);
            return false;
        }

        function ClearAnswerFields(id) {                
            var LnkId = id.replace('_lnkClearFields', '');
            var newId = LnkId + '_lstvwAnswerDetails_ctrl';
            if (newId != null) {
                newId = '[id^=' + newId + ']'
                var iCount = 0;
                $(newId).each(function () {
                    newId = newId.replace('[id^=', '')
                    newId = newId.replace(']', '')
                    var rdo = document.getElementById(newId + iCount + '_rdoCorrectAnswer')
                    if (rdo != null) {
                        if (rdo.checked = true) {
                            rdo.checked = false;
                        }
                        iCount++;
                    }
                });                
            }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
