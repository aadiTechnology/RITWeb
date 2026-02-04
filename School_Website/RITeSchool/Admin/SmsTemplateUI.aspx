<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SmsTemplateUI.aspx.cs" Inherits="SmsTemplateUI"
    MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">   
        <tr runat="server" id="trTitle" visible="false">
            <td align="left" colspan="3">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                    padding-right: 5px;">
                    <tr>
                        <td style="height: 20px">
                            <span style="font-weight: bold">Use Template</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td valign="bottom">
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true" />
            </td>
        </tr>
        <tr>
            <td valign="bottom">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ClsLabel"
                    ShowMessageBox="true" ShowSummary="false" ValidationGroup="OK" />
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 85%">
                <table id="tblSms" runat="server" border="0" cellpadding="1" cellspacing="2" style="width: 85%;
                    margin-left: 19px;">
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <table>
                                <tr style="height: 20px">
                                    
                                    <td align="left" class="ClsBorderLight" style="height: 20px; width: 150px;">
                                        <span class="ClsLabel">Name :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar" style="height: 20px;">
                                        <asp:TextBox runat="server" ID="txtTemplateName" CssClass="ClsTxtLarge" MaxLength="50"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="ClsLabel" style="height: 16px; width: 92px">Template :</span>
                                    </td>
                                    <td align="left" style="height: 100px;">
                                        <asp:TextBox ID="txtTemplate" runat="server" CssClass="ExLrgTxtBox" Style="height: 100px;
                                            width: 98%" TextMode="MultiLine"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                 <tr style="height: 20px">
                                    
                                    <td align="left" class="ClsBorderLight" style="height: 20px;">
                                        <span class="ClsLabel">Registration No. :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar" style="height: 20px;">
                                        <asp:TextBox runat="server" ID="txtRegNo" CssClass="ClsTxtLarge" MaxLength="100"></asp:TextBox>
                                       
                                    </td>
                                </tr>
                                  <tr >
                                       <td align="center" colspan="2">
                                           <table id="tblNote" runat="server" align="center" style="width: 100%">
                                                <tr>
                                                       <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                                                                CssClass="LblNrmlB"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 60%">
                                                                                          <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" 
                                                                                                Text="While updating system template, please do not modify or change system keywords. e.g. %FORMNUMBER% , %CHEQUENO% , %DUEDATE% etc." 
                                                                                                ></asp:Label>
                                                        </td>
                                                </tr>
                                          </table>
                                      </td>
                                  </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CustomValidator ID="cstName" runat="server" ClientValidationFunction="CheckName"
                                            ErrorMessage="Name should not be blank." Display="None" EnableClientScript="true"
                                            CssClass="ClsMdtStar"></asp:CustomValidator>
                                         <asp:CustomValidator ID="cstDuplicate" runat="server" ClientValidationFunction="CheckDuplicate"
                                            ErrorMessage="Name already exists." Display="None" EnableClientScript="true"
                                            CssClass="ClsMdtStar"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstTemplate" runat="server" ClientValidationFunction="CheckLength"
                                            ErrorMessage="Template should not be blank." Display="None" EnableClientScript="true"
                                            CssClass="ClsMdtStar"></asp:CustomValidator>
                                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" 
                                            CausesValidation="True" OnClick="btnSave_Click" disable-page="true" />
                                        <asp:Button CssClass="ClsBtn" ID="btnCancel" CausesValidation="false" runat="server"
                                             Text="Cancel" BorderWidth="1px" onclick="btnCancel_Click">
                                        </asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table runat="server" width="80%" align="center" id="tblMainBody">

                    <tr runat="server" id="trContacts">
                        <td align="center" valign="top" colspan="7">
                            <table align="center" valign="top" width="100%">
                                <tr>
                                  <td style="padding-left: 15%">
                                      <table>
                                          <tr align="left">
                                                 <td>
                                                    <span class="ClsLblLgnd">
                                                        <asp:Label runat="server" ID="Label3" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                     </span>
                                                 </td>
                                                 <td>
                                                     <span style="background-color: LightBlue; height: 20px; border: 1px solid black; width: 20px;">
                                                          <img src="../images/spacer.gif" width="20px" height="10px" />
                                                     </span>
                                                 </td>
                                                 <td class="ClsTextNormal" style="font-weight: bold">
                                                         <asp:Label runat="server" ID="Label4" Text="System Template"></asp:Label>
                                                 </td>
                                           </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">                                       
                                        <asp:ListView ID="lstvwTemplates" runat="server" ItemPlaceholderID="trItemPlaceholder"
                                            OnItemCommand="lstvwTemplates_ItemCommand" OnDataBound="lstvwTemplates_DataBound"
                                            OnItemDataBound="lstvwTemplates_ItemDataBound" OnSorting="lstvwTemplates_Sorting"
                                            DataSourceID="lstvwDSobj" DataKeyNames="TemplateId,IsSystemDefined">
                                            <LayoutTemplate>
                                                <table id="tblTachers" style="width: 100%; color: #333333" class="GridBorder" cellpadding="0"
                                                    cellspacing="1">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="center" class="Clspadding" id="thSelect" runat="server">

                                                        </th>
                                                        
                                                         <th  align="center" style="width:12%;" runat="server" >
                                                         Registration No.                                                    
                                                        </th>
                                                        <th class="ClspaddingL">
                                                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                CausesValidation="False" ForeColor="Black"> Name</asp:LinkButton>
                                                        </th>
                                                        <th class="ClspaddingL">
                                                            <asp:Label ID="lblTemplate" runat="server" ForeColor="Black" Text="Template"></asp:Label>
                                                        </th>
                                                       
                                                        <th class="Clspadding" id="thEdit" runat="server">
                                                            Edit
                                                        </th>
                                                        <th class="Clspadding" id="thDelete" runat="server">
                                                            Delete
                                                        </th>
                                                    </tr>
                                                    <tr id="trItemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <EmptyDataTemplate>
                                                <table style="width: 100%" align="center">
                                                    <tr>
                                                        <td class="LblNoRecord" align="center">
                                                            No record found.
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr id="trlistvw" runat = "server" class="ClsGridRow" >
                                                    <td align="center" class="Clspadding" id="tdSelect" runat="server">
                                                        <asp:RadioButton ID="rdoTemplate" runat="server" onclick="CheckOne(this);" AutoPostBack="false"></asp:RadioButton>
                                                    </td>
                                                     <td  align="center">
                                                       <asp:Label runat="server" ID="LblRegno" Text='<%#Eval("RegNo")%>'></asp:Label>
                                                    </td>
                                                    <td class="ClspaddingL">
                                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name")%>'></asp:Label>
                                                        <asp:HiddenField ID="hidTempId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                    </td>
                                                    <td class="ClspaddingL">
                                                        <asp:Label runat="server" ID="lblTemplate" Text='<%#Eval("Template")%>'></asp:Label>
                                                    </td>
                                                   
                                                    <td align="center" style="width: 5%;" class="Clspadding" id="tdEdit" runat="server">
                                                        <asp:ImageButton runat="server" ID="imgEdit" Text="Edit" CommandName="UpdateCommand"
                                                            CommandArgument='<%#Eval("TemplateId")%>' CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                    </td>
                                                    <td align="center" style="width: 5%;" class="Clspadding" id="tdDelete" runat="server">
                                                        <asp:ImageButton runat="server" ID="imgDelete" Text="Delete" CommandName="RemoveCommand"
                                                            CommandArgument='<%#Eval("TemplateId")%>' CausesValidation="false" ToolTip="Delete"
                                                            ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" OnClientClick="if(!ConfirmDelete()) return false;">
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                                  
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" Value="Name" />                                      
                                    </td>
                                </tr>
                         
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnOk" runat="server" Text="Add" CssClass="ClsBtn" Visible="false"
                                            CausesValidation="True" OnClick="btnOk_Click" ValidationGroup="OK" />&nbsp;
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" Visible="false"
                                            CausesValidation="False" OnClientClick="window.close(); return false;" />&nbsp;
                                        <asp:CustomValidator ID="cstValidateOK" runat="server" CssClass="LblErrorMsg" Display="None"
                                            ValidationGroup="OK" ClientValidationFunction="CheckValidOK"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.SmsTemplateBL" EnablePaging="true"
                                            ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                            EnableCaching="false">
                                            <SelectParameters>                                            
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:ControlParameter Name="sortDirection" Type="String" ControlID="hidSortDirection"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter Name="ShowSystemDefined" Type="String" ControlID="hidShowSystemDefined"
                                                    PropertyName="Value" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 128px">
                            &nbsp;
                        </td>
                        <td align="left" style="width: 107%">
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidTemplateId" runat="server" />
                            <asp:HiddenField ID="hidTemplateText" runat="server" />
                            <asp:HiddenField ID="hidUrl" runat="server" Value="" />
                            <asp:HiddenField ID="hidShowSystemDefined" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>    
    </table>
    <script type="text/javascript" language="javascript">
        _clientcstTemplate = "<%=this.cstTemplate.ClientID%>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _sTemplateText = "<%=this.txtTemplate.ClientID %>";
        _clienttxtTemplateName = "<%=this.txtTemplateName.ClientID %>";
        _clienthidTemplateId = "<%=this.hidTemplateId.ClientID %>";
        _clientlstvwTemplates = "<%=this.lstvwTemplates.ClientID %>";
        _clientcstValidateOK = "<%=this.cstValidateOK.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        
        function CheckLength(oSrc, args) {
            var sTemplateText = document.getElementById(_sTemplateText).value;
            var sTemplateText = sTemplateText.trim();
            ResetUpdateLbl();
            $get(_clientcstTemplate).errormessage = "";
            if (sTemplateText.length == 0) {
                $get(_clientcstTemplate).errormessage = "Template should not be blank."
                args.IsValid = false
                return true
            }
            if (sTemplateText.length > 459) {
                $get(_clientcstTemplate).errormessage = "Template should not exceed the length of 459 characters."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckName(oSrc, args) {
            var sName = $get(_clienttxtTemplateName).value;
            ResetUpdateLbl();
            sName = sName.trim();
            if (sName.length == 0) {
                oSrc.errormessage = "Name should not be blank.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function CheckDuplicate(oSrc, args) {
            var templateId = $get(_clienthidTemplateId).value;             
            var duplicate = false;
            var sName = $get(_clienttxtTemplateName).value;
            sName = sName.trim();

            var iRowCount = 0;
            var tempId = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_hidTempId");
            var Name = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_lblName");
            while (Name != null && tempId!=null) {
                if (Name.innerHTML == sName && tempId.value != templateId) {
                    duplicate = true;
                    break;
                }
                iRowCount = iRowCount + 1;
                Name = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_lblName");
                tempId = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_hidTempId");
            }

            if (duplicate) {
                oSrc.errormessage = "Name should not be duplicated.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return true;
        }

//        function ClearControls() {
//            $get(_clienthidTemplateId).value = "0";
//            $get(_sTemplateText).value = "";
//            $get(_clienttxtTemplateName).value = "";
//            $get(_clientbtnSave).value = "Save";
//            if ($get(_clientlblUpdateSucess) != null)
//                $get(_clientlblUpdateSucess).innerHTML = "";
//        }

        function CheckValidOK(oSrc, args) {
            var Selected = false;
            var iRowCount = 0;
            var select = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_rdoTemplate");
            while (select != null) {
                if (select.checked) {
                    Selected = true;
                    break;
                }
                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_rdoTemplate");
            }

            if (!Selected) {
                $get(_clientcstValidateOK).errormessage = "Please select Template.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return true;
        }

        function CheckOne(Src) {           
            var iRowCount = 0;
            var select = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_rdoTemplate");
            while (select != null) {
                if (select.name != Src.name)
                    select.checked = false;                
                iRowCount = iRowCount + 1;
                select = $get(_clientlstvwTemplates + "_ctrl" + iRowCount + "_rdoTemplate");
            }
        }

        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
            }
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this Template?')) {
                bResult = false
            }
            return bResult
        }

    </script>
</asp:Content>
