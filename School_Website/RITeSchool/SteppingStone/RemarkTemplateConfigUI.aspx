<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RemarkTemplateConfigUI.aspx.cs" Inherits="RemarkTemplateConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <asp:UpdatePanel ID="upnlError" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="right" class="TxtNormal" style="padding-right: 10px; top: 20px">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                            ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                            Width="100%" CssClass="ClsMdtStar"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td align="center" width="100%">
                <asp:UpdatePanel ID="upnlListView" runat="server">
                    <ContentTemplate>
                        <table width="680px">
                            <tr>
                                <td>
                                    <table align="center">
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="lblRemarkCategory" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, RemarkCategory%>"> </asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="LrgTxtBox" CausesValidation="true"
                                                    TabIndex="2">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td class="ClsBorderlight">
                                                <asp:Label ID="lblRemarkType" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, RemarkTemplate%>"> </asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRemarkTemplate" runat="server" CssClass="LrgTxtBox" TextMode="MultiLine"
                                                    Height="67px" TabIndex="3"></asp:TextBox><span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label ID="Label2" runat="server" class="ClsLabel" Text="Grade"> </asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbGrades" runat="server" CssClass="LrgTxtBox" CausesValidation="true"
                                                    TabIndex="2">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save%>" CausesValidation="true" disable-page="true"
                                                    TabIndex="4" OnClick="btnSave_Click" />
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Cancel%>" UseSubmitBehavior="false"
                                                    TabIndex="5" CausesValidation="false" OnClick="btnCancel_Click" />
                                                <asp:CustomValidator ID="cstvalValidateRemark" runat="server" ClientValidationFunction="validateRemark"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources,PleaseSelectAppropriateRemarkCategory%>">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="cstvalRemarkTemplate" runat="server" ClientValidationFunction="ValidateTemplate"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources,RemarkTemplateShouldNotBeBlank%>" SetFocusOnError="True">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validateGrade"
                                                    Display="None" ErrorMessage="Please select appropriate grade.">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="250px" valign="top">
                                    <table id="tblNotes" runat="server">
                                        <tr>
                                            <td align="center" class="ClsBorderlight" colspan="2">
                                             <asp:Label ID="Label1" runat="server" class="ClsLabel" style="float: none" Text="<%$ Resources:LocalizedResources, Keywords%>"> </asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table align="center" width="55%">
                            <tr>
                                <td colspan="3">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" class="style1" style="background-color: #ffffc4;">
                                                <asp:Label ID="lblNote1" runat="server" class="LblNrmlB" style="font-weight: bold" Text="<%$ Resources:LocalizedResources, Note1%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" class="style2" style="padding-left: 5px">
                                                <asp:Label ID="lblStudSpecificRemarkTepm" runat="server" class="LblSmlV" Text="<%$ Resources:LocalizedResources, ToCreateStudentSpecificRemarkTempleteuseAppropriateKeyword%>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
                                                <asp:Label ID="lblNote2" runat="server" class="LblNrmlB" style="font-weight: bold" Text="<%$ Resources:LocalizedResources, Note2%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="padding-left: 5px">
                                                <asp:Label ID="lblNote" runat="server" class="LblSmlV"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" colspan="3">
                                    <hr style="width: 780px; background-color: Silver" align="left" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="ClsBorderlight">
                                    <asp:Label ID="lblSearchRemarkCatTemp" runat="server" class="ClsLabel" style="width: 240px" Text="<%$ Resources:LocalizedResources, SearchRemarkCategoryRemarkTemplate%>"></asp:Label>
                                    <span class="colonPadding">:</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="false" MaxLength="300" CssClass="LrgTxtBox"
                                        TabIndex="5" Width="430px"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Search%>" CausesValidation="false"
                                        TabIndex="6" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                            <tr id="trItemCount" runat="server">
                                <td align="center" colspan="3">
                                    <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwTemplates" Visible="true">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, TO%>" EnableViewState="false" />
                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>" EnableViewState="false" />
                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                        CssClass="LblNrmlB" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>" EnableViewState="false" />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ListView ID="lstvwTemplates" runat="server" DataKeyNames="TemplateId,RemarkId"
                                        OnItemCommand="lstvwTemplates_ItemCommand" OnDataBound="lstvwTemplates_DataBound"
                                        OnItemDataBound="lstvwTemplates_ItemDataBound" OnSorting="lstvwTemplates_Sorting">
                                        <LayoutTemplate>
                                            <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333"
                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" style="width: 210px; padding-left: 7px;">
                                                        <asp:LinkButton ID="lnkRemarkType" runat="server" CommandName="Sort" CommandArgument="Name"
                                                            TabIndex="7" CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, RemarkCategory%>"></asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 450px; padding-left: 7px;">
                                                        <asp:LinkButton ID="lnkRemarkTemplate" runat="server" CommandName="Sort" CommandArgument="Template"
                                                            TabIndex="8" CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, RemarkTemplate%>"></asp:LinkButton>
                                                    </th>
                                                    <th align="center" style="width: 70px">
                                                        <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                    </th>
                                                    <th align="center" style="width: 70px; padding-left: 7px; text-align: center;">
                                                       <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager">
                                                    <td colspan="7">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwTemplates">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>" runat="server" CssClass="LblNrmlB" />
                                                                                    <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblRemarkName" runat="server" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblTemplate" runat="server" Text='<%# Eval("Template") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="<%$ Resources:LocalizedResources, Edit%>" ToolTip="<%$ Resources:LocalizedResources, Edit%>"
                                                        TabIndex="9" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="<%$ Resources:LocalizedResources, Delete%>" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                                        TabIndex="9" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 8px;" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblRemarkName" runat="server" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td align="left" style="padding-left: 7px">
                                                    <asp:Label ID="lblTemplate" runat="server" Text='<%# Eval("Template") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="<%$ Resources:LocalizedResources, Edit%>" ToolTip="<%$ Resources:LocalizedResources, Edit%>"
                                                        TabIndex="9" CausesValidation="false" CommandName="UpdateCommand" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="<%$ Resources:LocalizedResources, Delete%>" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                                        TabIndex="9" CausesValidation="false" CommandName="RemoveCommand" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                        Style="margin-left: 3px;" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                      <asp:Label ID="lblNoRecordFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidRemarkTemplateId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidUsersPageNo" runat="server" />
                                    <asp:HiddenField ID="hidTemplateLength" runat="server" />
                                    <asp:HiddenField ID="hidRemarkTemplateShouldNotBeBlank" runat="server" />
                                    <asp:HiddenField ID="hidRemarkTemplateShouldNotExceed" runat="server" />
                                    <asp:HiddenField ID="hidAreYouSureYouWantToDeleteRemarkTemplate" runat="server" />
                                    <asp:HiddenField ID="hidKeywordLimitNote" runat="server" />
                                    <asp:HiddenField ID="hidCharacters" runat="server" />
                                    <asp:HiddenField ID="hidSave" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table align="center" width="50%">
                    <tr id="trPrecondition" runat="server" visible="false">
                        <td align="left">
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        <asp:Button ID="btnExport" CssClass="ClsBtn" Text="Export" runat="server" CausesValidation="false"
                                TabIndex="11" onclick="btnExport_Click" />
                            <asp:Button ID="btnClose" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back%>" runat="server" CausesValidation="false"
                                TabIndex="10" />
                        </td>              
                   </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="../Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-blink.js"></script>
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/Validate2.js"></script>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>
    <style type="text/css">
        .class2
        {
            border: 1;
        }
        .style1
        {
            width: 10%;
            height: 19px;
        }
        .style2
        {
            background-color: #fff;
            border: 1px solid #ddd;
            font-size: 9pt;
            margin: 0;
            padding: 0;
            height: 19px;
        }
    </style>
    <script type="text/javascript">
        _clientcstvalRemarkTemplate = "<%=this.cstvalRemarkTemplate.ClientID %>"
        _clienttxtRemarkTemplate = "<%=this.txtRemarkTemplate.ClientID %>"
        _clientcmbCategory = "<%=this.cmbCategory.ClientID %>"
        _clientcmbGrade = "<%=this.cmbGrades.ClientID %>"
        _clientbtnSave = "<%=this.btnSave.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>"
        _clienthidTemplateLength = "<%=this.hidTemplateLength.ClientID %>"
        _clientNote = "<%=this.lblNote.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        function SetText(Description) {
            $get(_clienttxtRemarkTemplate).value = $get(_clienttxtRemarkTemplate).value + ' ' + Description;
            $get(_clientlblUpdateMessage).innerHTML = "";
        }

        function validateRemark(aSrc, args) {
            if (document.getElementById(_clientcmbCategory).value == 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function validateGrade(aSrc, args) {
            if (document.getElementById(_clientcmbGrade).value == 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidateTemplate(aSrc, args) {
            if ($get(_clienttxtRemarkTemplate).value.trim() == '') {
                document.getElementById(_clientcstvalRemarkTemplate).errormessage = document.getElementById("<%=hidRemarkTemplateShouldNotBeBlank.ClientID%>").value;
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                args.IsValid = false
                return true
            }
            else {
                var description = document.getElementById(_clienttxtRemarkTemplate).value; 
                if (description.length > $get(_clienthidTemplateLength).value) {
                    document.getElementById(_clientcstvalRemarkTemplate).errormessage = document.getElementById("<%=hidRemarkTemplateShouldNotExceed.ClientID%>").value + $get(_clienthidTemplateLength).value + document.getElementById("<%=hidKeywordLimitNote.ClientID%>").value;
                    document.getElementById(_clientlblUpdateMessage).innerHTML = "";
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=hidAreYouSureYouWantToDeleteRemarkTemplate.ClientID%>").value)) {
                bResult = false
                document.getElementById(_clientlblUpdateMessage).innerHTML = "";
            }
            return bResult
        }

        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }

        function EndRequestHandler() {
            showtooltip();
            ShowNote();
        }

        function ShowNote() {
            document.getElementById(_clientNote).innerHTML = document.getElementById("<%=hidKeywordLimitNote.ClientID%>").value.replace("%range%", $get(_clienthidTemplateLength).value);
        }
    </script>
    <script type="text/javascript">

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        width: 3,
                        radius: 5
                    },
                    tip: 'topRight',
                    width: 200
                },

                position: { adjust: { x: -210, y: 0} }
            });
        }
        showtooltip();
        ShowNote();
    </script>
</asp:Content>
