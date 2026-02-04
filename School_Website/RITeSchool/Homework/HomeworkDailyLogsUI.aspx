<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="HomeworkDailyLogsUI.aspx.cs" Inherits="HomeworkDailyLogsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="valSumErrorMsg" ValidationGroup="Save" runat="server"
                                            CssClass="ClsLabel" />
                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="Save" runat="server"
                                            ErrorMessage="Record for given date is already exist." OnServerValidate="Validate_Date"
                                            Display="None"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="ItemCommand" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right" width="150px">
                                <span class="ClsMdtStar">*</span>
                                <asp:Label ID="Label1" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="ClsBorderlight" style="width:150px;">
                                        <span class="clsLabel">Class :</span>
                                    </td>
                                    <td class="ClsHilightBGB ">
                                        <asp:Label ID="lblClassName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                       <span class="clsLabel">Date :</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                        <rjs:PopCalendar ID="calPassingDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                            Culture="en" ShowWeekend="True" ShowErrorMessage="false" ValidationGroup="Save"
                                            InvalidDateMessage="Date should not be blank." AutoPostBack="False" To-Today="true" />
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Date should not be blank."
                                            Display="None" ValidationGroup="Save" ClientValidationFunction="ValidateAssignDate"></asp:CustomValidator>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ClsBorderlight">
                                        <span class="clsLabel">Attachment :</span>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="flDocument" runat="server" />
                                        <span class="ClsMdtStar">*</span>
                                        <asp:CustomValidator ID="cstFileType1" runat="server" ErrorMessage="" ValidationGroup="Save"
                                            ClientValidationFunction="ValidateFile"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <span class="LblSmlGray">(Attachment supports files of types - .BMP, .DOC, .DOCX, .JPG,
                                            .JPEG, .PNG, .BMP, .PDF, .XLS, .XLSX upto 3 MB.)</span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnlBtns" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                CssClass="ClsBtn" OnClick="btnSave_Click" ValidationGroup="Save" />
                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel%>"
                                CssClass="ClsBtn" CausesValidation="false" onclick="btnCancel_Click1" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="80%">
                        <tr>
                            <td>
                                <hr style="border: thin solid #C0C0C0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderLight paddingL" style="width: 130px">
                                <span class="ClsLabel">Date:</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" ValidationGroup="Save"
                                    InvalidDateMessage="" AutoPostBack="False" />
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search%>"
                                    CssClass="ClsBtn" CausesValidation="false" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="80%">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwHomeworklogs">
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
                                        <asp:ListView ID="lstvwHomeworklogs" runat="server" DataKeyNames="Id" OnDataBound="lstvwHomeworklogs_DataBound"
                                            OnSorting="lstvwHomeworklogs_Sorting" OnItemCommand="lstvwHomeworklogs_ItemCommand"
                                            OnItemDataBound="lstvwHomeworklogs_ItemDataBound">
                                            <LayoutTemplate>
                                                <table id="tblhomework" align="center" width="80%" runat="server" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" width="100px">
                                                            <asp:LinkButton ID="Label4" runat="server" CausesValidation="false" Text="Date" CommandName="Sort"
                                                                CommandArgument="Date"></asp:LinkButton>
                                                        </th>
                                                        <th align="left" class="paddingL">
                                                            <asp:Label ID="Label5" runat="server" Text="Attachment"></asp:Label>
                                                        </th>
                                                        <th id="thPublish" align="center" class="paddingL" width="175px">
                                                            Published / UnPublish
                                                        </th>
                                                        <th align="center" width="100px">
                                                            <asp:Label ID="Label7" runat="server" Text="Edit"></asp:Label>
                                                        </th>
                                                        <th align="center" width="100px">
                                                            <asp:Label ID="lblAdd" runat="server" Text="Delete"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                        <td colspan="5">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwHomeworklogs"
                                                                PageSize="20">
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
                                                    <td align="center" class="paddingL">
                                                        <asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("Date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:HyperLink ID="lnkAttachment" runat="server" Text="Click Here"></asp:HyperLink>
                                                    </td>
                                                    <td align="center" id="tdPublish">
                                                        <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" CausesValidation="false"
                                                            CommandName="PublishCommand" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ToolTip="Edit homework" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ToolTip="Delete homework" ImageUrl="../images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                    <td align="center" class="paddingL">
                                                        <asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("Date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:HyperLink ID="lnkAttachment" runat="server" Text="Click Here"></asp:HyperLink>
                                                    </td>
                                                    <td align="center" id="tdPublish">
                                                        <asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" CausesValidation="false"
                                                            CommandName="PublishCommand" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            ToolTip="Edit homework" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            ToolTip="Delete homework" ImageUrl="../images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;" />
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.HomeworkDailyLogBL" EnablePaging="True"
                                            ID="objdsHomeworks" runat="server" SelectMethod="GetAll" SortParameterName="sortExpression"
                                            SelectCountMethod="Count" EnableCaching="False">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiUserRoleId" SessionField="S_USERLOGIN_ROLE_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="txtStartDate" Name="asFilter" Type="String" PropertyName="Text" />
                                                 <asp:ControlParameter ControlID="hidStdDivId" Name="asStdDivId" Type="String" PropertyName="Value" />
                                                <asp:Parameter Name="sortExpression" Type="String" />
                                                <asp:Parameter Name="sortDirection" Type="String" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="imgbtnBack" Text="Back" runat="server" CssClass="ClsBtnSml" BorderStyle="Solid"
                                            PostBackUrl="~/RITeSchool/Homework/HomeworkUI.aspx" Visible="True" BorderWidth="1px"
                                            CausesValidation="false" UseSubmitBehavior="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidFileUpload" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwHomeworklogs" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">


            _clienttxtAssignedDt = "<%=this.txtDate.ClientID %>";

            function ConfirmDelete() {
                return confirm('Are you sure you want to delete this record?');
            }
            function ValidateFile(oSrc, args) {
                var fl = $get("<%=this.flDocument.ClientID %>").value;
                var flName = $get('<%=this.hidFileUpload.ClientID %>').value

                if (fl == "" && flName == "") {
                    oSrc.errormessage = "Please select file to upload.";
                    args.IsValid = false;
                    return true;
                }

                if (fl != "") {
                    var file = $get("<%=this.flDocument.ClientID %>")
                    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOC" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOCX" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLS" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLSX" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                          fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF"
                        )) {
                        oSrc.errormessage = "Please select valid file type.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (file.files[0].size >= 5242880) {
                        oSrc.errormessage = "File size should be less than 3 MB."
                        args.IsValid = false
                        return true
                    }
                }

                args.IsValid = true;
                return false;
            }

            function ValidateAssignDate(oSrc, args) {
                var dt =  $("#" + _clienttxtAssignedDt).val()

                if (dt == "") {
                    oSrc.errormessage = "Date should not be blank.";
                    args.IsValid = false;
                    return true;
                }
                else {                    
                    var dtDate;
                    if (document.all)
                        dtDate = new Date(dt.replace('-', ' '));
                    else
                        dtDate = new Date(convertdate(dt));

                    var serverDate = $get("<%=this.hidServerDate.ClientID %>").value

                    var dtServerDate = ''
                    if (document.all)
                        dtServerDate = new Date(serverDate.replace('-', ' '));
                    else
                        dtServerDate = new Date(convertdate(serverDate));

                    if (dtDate > dtServerDate) {
                        oSrc.errormessage = 'Date should not be in future.'
                        args.IsValid = false;
                        return true;
                    }
                }

                args.IsValid = true;
                return false;
            }

            function OpenFile(file) {
                window.open(file, '_blank')
                return false;
            }
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
