<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadNewsUI.aspx.cs" Inherits="UploadNewsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
            <table width="100%">
                <tr>
                    <td align="right">
                        <span class="ClsMdtStar">* Mandatory Fields</span>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:ValidationSummary ID="valSumErrorMsgText" runat="server" CssClass="ClsLabel"
                            ShowSummary="true" ValidationGroup="TextNotice" />
                    </td>
                </tr>
                <tr style="width: 100%;">
                    <td align="center">
                        <table>
                            <tr>
                                <td class="TxtNormal" align="center" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                                EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="ClsLabel margin-bottom-5">News Display Type :</span>
                                </td>
                                <td>
                                    <asp:RadioButton ID="optText" runat="server" GroupName="Notice" Text="School News"
                                        AutoPostBack="True" Checked="True" OnCheckedChanged="optText_CheckedChanged">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="optLink" runat="server" AutoPostBack="True" GroupName="Notice"
                                        Text="School in News" OnCheckedChanged="optLink_CheckedChanged"></asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Width="100%"
                            CssClass="ClsMdtStar" EnableViewState="False"></asp:Label>
                    </td>
                </tr>
                <tr id="trTextNoticeControls" runat="server">
                    <td align="center">
                        <table id="tblTextNoticeControls" width="100%" runat="server" visible="true">
                            <tr>
                                <td align="center">
                                    <table style="width: 80%">
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 200px">
                                                <span class="ClsLabel">News Heading :</span>
                                            </td>
                                            <td style="width: 250px" align="left">
                                                <asp:TextBox ID="txtNewsHeading" class="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox><span
                                                    class="ClsMdtStar">&nbsp;*&nbsp;</span>
                                                <asp:RequiredFieldValidator ID="reqNoticName" runat="server" ErrorMessage="News Heading should not be blank."
                                                    ValidationGroup="TextNotice" ControlToValidate="txtNewsHeading" Display="None"> </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">News Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtNewsDate" CssClass="MidTxtBox" runat="server" AutoPostBack="True"> </asp:TextBox>
                                                <rjs:PopCalendar ID="calStartDtText" runat="server" Control="txtNewsDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid Date."
                                                    To-Today="true" />
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                                <asp:CustomValidator ID="cstValStartDateText" runat="server" ClientValidationFunction="IsStartEndDateValidForText"
                                                    Display="None" ValidationGroup="TextNotice"> </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight paddingL" align="left" style="width: 111px">
                                                <span class="ClsLabel">Sort Order :</span>
                                            </td>
                                            <td class="TxtNormal" align="left">
                                                <asp:TextBox runat="server" ID="txtSortOrder" CssClass="MidTxtBox" MaxLength="3"
                                                    onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"> </asp:TextBox>
                                                <span class="ClsMdtStar">*&nbsp;</span>
                                                <asp:RequiredFieldValidator ID="reqValSortOrderText" runat="server" ValidationGroup="TextNotice"
                                                    ErrorMessage="Sort order should not be blank." ControlToValidate="txtSortOrder"
                                                    Display="None"> </asp:RequiredFieldValidator>
                                            </td>
                                            <td align="left" style="width: 111px">
                                                &nbsp;
                                            </td>
                                            <td class="TxtNormal">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id = "trfileuploadcontrol" runat = "server" visible = "false">
                                       <td class="ClsBorderlight paddingL" align="left">
                                                <span class="ClsLabel">File Path :</span>
                                            </td>
                                            <td align="left" >
                                                <asp:FileUpload ID="fileUploadItems" runat="server" ToolTip="Only PDF files are allowed" />
                                                <span class="ClsMdtStar">*</span>
                                                <asp:CustomValidator ID="cstFileNameValidation" ControlToValidate="fileUploadItems"
                                                    runat="server" ClientValidationFunction="IsFileUploaded" Display="None" ValidateEmptyText="True" ValidationGroup="TextNotice"> </asp:CustomValidator>
                                             </td>
                                        </tr>
                                         <tr id = "trfileuploadnote" runat = "server" visible ="false">
                                        
                                         <td align="left" class="paddingL" colspan="2">
                                                <span class="LblSmlGray">(Supports only .JPG or .PNG file type. File size should not exceed
                                                    500KB.)</span>
                                            </td>
                                        
                                        </tr>
                                        <tr id = "trfckeditor" runat = "server">
                                            <td colspan="4">
                                                <CKEditor:CKEditorControl ID="FCKNoticeContent" Toolbar="Bold|Italic|Underline|Strike|-|Subscript|Superscript NumberedList|BulletedList|-|Outdent|Indent / Styles|Format|Font|FontSize|TextColor|BGColor"
                                                    BasePath="../ckeditor/" Width="99%" ReadOnly="false" runat="server" Height="250px"></CKEditor:CKEditorControl>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" Text="Save" class="ClsBtn" ID="btnSaveText" disable-page="true"
                                        ValidationGroup="TextNotice" OnClick="btnSaveText_Click" />
                                    <asp:Button runat="server" Text="Cancel" class="ClsBtn" ID="btnCancelText" CausesValidation="False"
                                        OnClick="btnCancelText_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               
                <tr id="trLink" runat="server">
                    <td>
                        <table id="tblLstvwLinkNotic" align="center" width="70%" runat="server">
                            <tr id="trNote" runat="server">
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" class="ClsBorderlight " colspan="1" style="width: 10%; background-color: #ffffc4;">
                                                <asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="LblNrmlB" EnableViewState="False"
                                                    Font-Bold="True" Height="16px" Text="Note :" Width="46px"></asp:Label>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                                <span class="LblSmlV" style="width: 100%; border-width: 0px;">Select the news from the
                                                    list to be displayed on School web site under School News.</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 100%">
                                    <table align="center" width="100%">
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwNewsDetails" DataKeyNames="NewsId" runat="server" 
                                                    OnItemCommand="lstvwNewsDetails_ItemCommand" 
                                                    onitemdatabound="lstvwNewsDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblNoticeDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                            class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="paddingLSML" width="10%;">
                                                                    News Heading
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    News Date
                                                                </th>
                                                                <th align="center" class="paddingL" style="width: 5%;">
                                                                    Sort Order
                                                                </th>
                                                                <th id ="thFileName" runat = "server" align="left" class="paddingL" style="width: 5%;" > 
                                                                    File Name
                                                                </th>
                                                                <th style="width: 2%">
                                                                    <asp:Label ID="Label1" runat="server" Text="Select"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 2%;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 2%;">
                                                                    Delete
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("NewsHeading") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblStartDt" runat="server" Text='<%# Eval("NewsDate","{0:dd-MMM-yyyy}") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id ="tdFileName" align="left" class="paddingL" runat ="server" >
                                                                <asp:LinkButton ID="hlnkFileName" runat="server"  Text='<%# Eval("FileName") %>' ForeColor = "Blue" style = "cursor:pointer">
                                                                </asp:LinkButton>
                                                              
                                                            </td>
                                                            <td align="center">
                                                                <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select News to display under School News"
                                                                    Checked='<%# Eval("IsSelected") %>'></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CommandName="UpdateNews" CausesValidation="false"
                                                                    ToolTip="Edit" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteNews" CausesValidation="false"
                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblLinkName" runat="server" Text='<%# Eval("NewsHeading") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblStartDt" runat="server" Text='<%# Eval("NewsDate","{0:dd-MMM-yyyy}") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center" class="paddingL">
                                                                <asp:Label ID="lblSortOrder" runat="server" Text='<%# Eval("SortOrder") %>'>
                                                                </asp:Label>
                                                            </td >
                                                                <td id ="tdFileName" align="left" class="paddingL"  runat = "server">
                                                                <asp:LinkButton ID="hlnkFileName" runat="server" Text='<%# Eval("FileName") %>' ForeColor = "Blue" style = "cursor:pointer">
                                                                </asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ToolTip="Select News to display under School News"
                                                                    Checked='<%# Eval("IsSelected") %>'></asp:CheckBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateNews"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteNews"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" OnClientClick="return ConfirmDelete()" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record found.
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidNewsId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidRowNo" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidFileName" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trSave" runat="server">
                    <td align="center">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSaveSelected" runat="server" Text="Save" CssClass="ClsBtn" CausesValidation="false"
                                        disable-page="true" onclick="btnSaveSelected_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
    <script language="javascript" type="text/javascript">

        _ClientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _ClientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _ClienttxtStartTimeTextNotice = "<%=this.txtNewsDate.ClientID %>"
        _ClientcstValStartDateText = "<%=this.cstValStartDateText.ClientID %>"
        _ClientfileUploadItems = "<%=this.fileUploadItems.ClientID %>";
        _ClientcstFileNameValidation = "<%=this.cstFileNameValidation.ClientID %>";
        _ClienthidFileName = "<%=this.hidFileName.ClientID %>";
        _ClientcstFileNameValidationforUpdate = "<%=this.cstFileNameValidation.ClientID %>";

        //This function is used to validate for future date selection and empty date .
        function IsStartEndDateValidForText(oSrc, args) {
        
            if (document.getElementById(_ClientlblErrorMsg)) {
                document.getElementById(_ClientlblErrorMsg).innerText = "";
                document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            }
            if (document.getElementById(_ClientlblUpdateSucess)) {
                document.getElementById(_ClientlblUpdateSucess).innerHTML = "";
                document.getElementById(_ClientlblUpdateSucess).innerText = "";
            }
            var sStrtDate = (document.getElementById(_ClienttxtStartTimeTextNotice).value)
            if (sStrtDate == "") {
                document.getElementById(_ClientcstValStartDateText).errormessage = "News date should not be blank.";
                args.IsValid = false;
                return true;
            }

            var dob;
            if (document.all)
                dob = new Date(sStrtDate.replace('-', ' '));
            else
                dob = new Date(convertdate(sStrtDate));

            var today = new Date();
            if (dob > today) {
                document.getElementById(_ClientcstValStartDateText).errormessage = "News date should not be future date.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;

        }

        //This function is used to validate is file uploaded by user or not
        function IsFileUploaded(oSrc, args) {
           
            if (document.getElementById(_ClientlblErrorMsg)) {
                document.getElementById(_ClientlblErrorMsg).innerText = "";
                document.getElementById(_ClientlblErrorMsg).innerHTML = "";
            }

            if (document.getElementById(_ClientlblUpdateSucess)) {
                document.getElementById(_ClientlblUpdateSucess).innerHTML = "";
                document.getElementById(_ClientlblUpdateSucess).innerText = "";
            }

            var lblUFileNameval = "";
            var myImage = document.getElementById(_ClientfileUploadItems).value;
            if (myImage == "") {
                if (_ClienthidFileName == "") {
                    oSrc.errormessage = "";
                    document.getElementById(_ClientcstFileNameValidation).errormessage = "File to be uploaded should be selected.";
                    args.IsValid = false;
                    return false;
                }
            }
            else {
                if (myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || myImage.substr(myImage.lastIndexOf('.'), 4).toUpperCase() == ".PNG") {
                        args.IsValid = true;
                        return false;
                    }
                    else {
                        oSrc.errormessage = "Invalid File Type.";
                        document.getElementById(_ClientcstFileNameValidationforUpdate).errormessage = "Invalid file type.";
                        args.IsValid = false;
                        return true;
                    }
            }
        }
              
        //This function is used to open popun on click on link news.
        function OpenWindow(sfilepath) {
            window.open( sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
            return false;
        }

        //This function is used to confirm action about delete.
        function ConfirmDelete() {
            return window.confirm('Are you sure you want to delete this record?')
        }      

    </script>
</asp:Content>
