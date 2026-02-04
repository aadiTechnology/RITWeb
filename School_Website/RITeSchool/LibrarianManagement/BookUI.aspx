<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="BookUI.aspx.cs" Inherits="BookUI" EnableEventValidation="False" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td id="MainDataTable" align="center" valign="top">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2">
                    <tr style="width: 900px">
                        <td style="width: 900px">
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" id="trBtnBack">
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" PostBackUrl="~/RITeSchool/LibrarianManagement/LibraryManagementUI.aspx" />
                        </td>
                    </tr>
                    <tr id="trMsgs" runat="server">
                        <td align="left" valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" EnableViewState="false"
                                                    Font-Bold="true" Font-Size="Small" ForeColor="Blue" Visible="true"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="valsumBooks" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                    ShowSummary="true" />
                                                <asp:ValidationSummary ID="valsumBookNo" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                    ShowSummary="true" ValidationGroup="GrpBookNo" />
                                                <asp:CustomValidator ID="cstAccessionNo" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ValidationGroup="GrpBookNo" Visible="true" ErrorMessage="Accession number should not be blank."
                                                    ClientValidationFunction="ValidateAccessionNo"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstPrice" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ValidationGroup="GrpBookNo" Visible="true" ErrorMessage="Price (Rs.) should not be blank."
                                                    ClientValidationFunction="ValidatePrice"></asp:CustomValidator>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="LblErrorMsg"></asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                <ContentTemplate>
                                    <asp:Wizard ID="wizard_BookDetails" runat="server" DisplaySideBar="False" ActiveStepIndex="0"
                                        DisplayCancelButton="True" Width="100%" OnActiveStepChanged="wizard_BookDetails_ActiveStepChanged"
                                        OnCancelButtonClick="wizard_BookDetails_CancelButtonClick" OnFinishButtonClick="wizard_BookDetails_FinishButtonClick"
                                        OnNextButtonClick="wizard_BookDetails_NextButtonClick" 
                                        OnPreviousButtonClick="wizard_BookDetails_PreviousButtonClick">
                                        <WizardSteps>
                                            <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1" StepType="Start">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" class="ClsBtmBorderGray" style="height: 21px">
                                                            <span class="ClsLblLgnd" style="font-weight: bold">Book Details :</span>
                                                        </td>
                                                        <td align="right" style="width: 23%; padding-right: 30px; height: 21px;" class="ClsBtmBorderGray"
                                                            valign="top">
                                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="Table1" width="90%" border="0" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td style="height: 20px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr7">
                                                        <td class="ClsBorderlight paddingL" style="width: 110px;" runat="server">
                                                            <span class="ClsLabel">Book Title :</span>
                                                        </td>
                                                        <td style="width: 300px;">
                                                            <asp:TextBox TabIndex="1" CssClass="ExLrgTxtBox" ID="txtBookName" runat="server"
                                                                MaxLength="100"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqBookName" runat="server" ControlToValidate="txtBookName"
                                                                Display="None" ErrorMessage="Book Title should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="ClsBorderLight" style="border:none">
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Category :</span>
                                                        </td>
                                                        <td id="Td18" style="width: 267px">
                                                            <asp:DropDownList TabIndex="2" ID="cmbMainCategory" runat="server" CssClass="ExLrgTxtBox">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:CustomValidator ID="cstCategory" Display="None" runat="server" CssClass="ClsMdtStar"
                                                                Visible="true" ErrorMessage="Category should be selected." EnableClientScript="true"
                                                                ClientValidationFunction="ValidateBookCategory"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1">
                                                        <td class="ClsBorderLight paddingL" id="Td10">
                                                            <span class="ClsLabel">Author(s) :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox TabIndex="3" CssClass="ExLrgTxtBox" ID="txtAuthorName" runat="server"
                                                                MaxLength="100"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqAuthor" runat="server" ControlToValidate="txtAuthorName"
                                                                Display="None" ErrorMessage="Author(s) should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="ClsBorderLight" style="border:none">
                                                        </td>
                                                        <td id="Td5" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Language :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox TabIndex="4" ID="txtLanguage" runat="server" MaxLength="50" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqLanguage" runat="server" ControlToValidate="txtLanguage"
                                                                Display="None" ErrorMessage="Language should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr2">
                                                        <td class="ClsBorderLight paddingL" id="Td2">
                                                            <span class="ClsLabel">Publisher :</span>
                                                        </td>
                                                        <td id="Td3">
                                                            <asp:TextBox TabIndex="5" CssClass="ExLrgTxtBox" ID="txtPublisherName" runat="server"
                                                                MaxLength="100"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqPublisher" runat="server" ControlToValidate="txtPublisherName"
                                                                Display="None" ErrorMessage="Publisher should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="ClsBorderLight" id="Td4" style="border:none">
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Shelf :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox TabIndex="6" ID="txtShelf" runat="server" CssClass="ExLrgTxtBox" MaxLength="10"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqShelf" runat="server" ControlToValidate="txtShelf"
                                                                Display="None" ErrorMessage="Shelf should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td1" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Rack Number :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRackNumber" TabIndex="7" runat="server" MaxLength="10" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqRackNumber" runat="server" ControlToValidate="txtRackNumber"
                                                                Display="None" ErrorMessage="Rack Number should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Fine if lost(%) :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLostPercentage" TabIndex="8" runat="server" CssClass="ExLrgTxtBox"
                                                                MaxLength="5" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                                onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,2,false);"
                                                                onpaste="event.returnValue=false"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:CustomValidator ID="cstLostPercentage" Display="None" runat="server" CssClass="ClsMdtStar"
                                                                Visible="true" ErrorMessage="Percentage lost should not be greater than 100."
                                                                ClientValidationFunction="ValidateLostPercentage"></asp:CustomValidator>
                                                            <asp:RequiredFieldValidator ID="reqLostPercentage" runat="server" ControlToValidate="txtLostPercentage"
                                                                Display="None" ErrorMessage="Lost Percentage should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Description :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="9" MaxLength="100" CssClass="ExLrgTxtBox"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                            <asp:CustomValidator ID="cstRemark" runat="server" ClientValidationFunction="ValidateRemark"
                                                                CssClass="ClsMdtStar" Display="None"></asp:CustomValidator>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Remark :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" TabIndex="10" MaxLength="100" CssClass="ExLrgTxtBox"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                            <asp:CustomValidator ID="cstDescription" runat="server" ClientValidationFunction="ValidateDescription"
                                                                CssClass="ClsMdtStar" Display="None"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td6" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Classification :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtClassification" TabIndex="11" runat="server" MaxLength="50" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqClassification" runat="server" ControlToValidate="txtClassification"
                                                                Display="None" ErrorMessage="Classification should not be blank." CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 100px;">
                                                            <span class="ClsLabel">ISBN :</span></td>
                                                        <td style="width: 300px;" valign="top">
                                                            <asp:TextBox ID="txtISBN" runat="server" CssClass="ExLrgTxtBox" MaxLength="25" TabIndex="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td id="Td7" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Book Edition :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEdition" TabIndex="11" runat="server" MaxLength="50" CssClass="ExLrgTxtBox"></asp:TextBox>                                                            
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 100px;">
                                                            <span class="ClsLabel">Book Year :</span></td>
                                                        <td style="width: 300px;" valign="top">
                                                            <asp:TextBox ID="txtBookYear" runat="server" CssClass="ExLrgTxtBox" MaxLength="25" TabIndex="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td id="Td11" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Call Number :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcallnumber" TabIndex="11" runat="server" MaxLength="10" CssClass="ExLrgTxtBox"></asp:TextBox>                                                            
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 100px;">
                                                            <span class="ClsLabel">Series :</span></td>
                                                        <td style="width: 300px;" valign="top">
                                                            <asp:TextBox ID="txtseries" runat="server" CssClass="ExLrgTxtBox" MaxLength="10" TabIndex="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td id="Td12" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Status :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtstatus" TabIndex="11" runat="server" MaxLength="100" CssClass="ExLrgTxtBox"></asp:TextBox>                                                            
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL" style="width: 100px;">
                                                            <span class="ClsLabel">Publication Date :</span></td>
                                                        <td style="width:250px;" valign="top">
                                                            <asp:TextBox ID="txtpublicationdate" runat="server" CssClass="ExLrgTxtBox"  style="width:200px;" TabIndex="20"></asp:TextBox>
                                                             <rjs:PopCalendar ID="PopCalendar1" runat="server" Culture="en-US" Control="txtpublicationdate" 
                                                        Enabled="true" ShowErrorMessage="false" Format="dd MMM yyyy"
                                                        ShowWeekend="True"  />

                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td valign="middle">
                                                            <span class="ClsLabel" style="width: 92px">For Standards :</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="ChkAllStandards" runat="server" TabIndex="14" Text="All" 
                                                                  onclick="UncheckOrCheckAll()"  />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span class="ClsLabel">Media Type :</span></td>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="optPrintable" runat="server" AutoPostBack="True" 
                                                                Checked="True" CssClass="ClsLabel" GroupName="MediaType" 
                                                                OnCheckedChanged="optPrintable_CheckedChanged" TabIndex="12" Text="Printable" />
                                                            <asp:RadioButton ID="optNonPrintable" runat="server" AutoPostBack="True" 
                                                                CssClass="ClsLabel" GroupName="MediaType" 
                                                                OnCheckedChanged="optPrintable_CheckedChanged" TabIndex="13" 
                                                                Text="NonPrintable" />
                                                        </td>
                                                        <tr>
                                                            <td >
                                                               </td>
                                                            <td valign="top">
                                                                <asp:CheckBoxList ID="chkListClasses" runat="server" CellPadding="0" 
                                                                    CellSpacing="0" CssClass="ClsBorderLight" onclick="UnCheck()" RepeatColumns="3" 
                                                                    RepeatDirection="Horizontal" TabIndex="16">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            
                                                            <td style="width: 300px;"valign="top">
                                                                <span class="ClsLabel">Can be issued?</span></td>
                                                                <td valign="top">
                                                                    <asp:CheckBox ID="chkForReadong" runat="server" TabIndex="15" />
                                                            </td>
                                                        </tr>
                                                        
                                                        
                                                    </tr>
                                                    <tr id="Tr5">
                                                        <td colspan="5" id="Td14" style="height: 18px">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr id="Tr6">
                                                        <td style="height: 15px" id="Td15">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:WizardStep>
                                            <asp:WizardStep ID="WizardStep2" runat="server" StepType="Finish" Title="Step 2">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" class="ClsBtmBorderGray">
                                                            <asp:Label ID="lblDisplayBookNo" runat="server" BorderWidth="0px" Font-Bold="True"
                                                                Text="Accession Details :" CssClass="ClsLblLgnd" Width="100%" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td align="right" style="width: 23%; padding-right: 30px;" class="ClsBtmBorderGray"
                                                            valign="top">
                                                            <span style="color: red;" class="ClsMdtStar">* Mandatory Fields</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="Table2" width="900px" border="0" cellpadding="0" cellspacing="1">
                                                    <tr>
                                                        <td id="Td9" style="height: 20px" runat="server">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <asp:Label ID="lblBookNo" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                                Text="Accession Number :"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBookNo" TabIndex="17" runat="server" CssClass="ExLrgTxtBox" MaxLength="10"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Total Pages :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTotPages" TabIndex="18" runat="server" CssClass="ExLrgTxtBox"
                                                                MaxLength="5" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                                ondrop="event.returnValue=false;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Price (Rs.) :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="ExLrgTxtBox" TabIndex="19" ID="txtBookPrice" runat="server"
                                                                MaxLength="8" onblur="extractNumber(this,2,true);" onkeyup="extractNumber(this,2,true);"
                                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                                ondrop="event.returnValue=false"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Bill Number :</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBillNo" TabIndex="20" runat="server" CssClass="ExLrgTxtBox" MaxLength="15"
                                                                onkeypress="return blockNonAlphanumericCharacter (this, event);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Vendor Name :</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbVendorName" TabIndex="21" runat="server" CssClass="ExLrgTxtBox">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="Td8" class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Date of Purchase :</span>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:TextBox ID="txtPurchaseDate" runat="server" TabIndex="22" CssClass="SmlTxtBox"
                                                                MaxLength="11"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cPurchageDate" runat="server" Control="txtPurchaseDate" Format="dd MMM yyyy"
                                                                To-Today="true" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
                                                                ControlFocusOnError="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight paddingL">
                                                            <span class="ClsLabel">Is Gifted?</span>
                                                        </td>
                                                        <td>
                                                            
                                                            <asp:CheckBox ID="chkIsGifted" runat="server" AutoPostBack="True" 
                                                                TabIndex="23" />
                                                            
                                                        </td>
                                                        
                                                    </tr>
                                                    
                                                </table>
                                                <table border="0" align="left" width="100%">
                                                    <tr>
                                                        <td style="height: 15px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnAdd" TabIndex="24" runat="server" Text="Add" OnClick="btnAdd_Click"
                                                                ValidationGroup="GrpBookNo" CssClass="ClsBtn" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdBookNo" runat="server" valign="top" align="center">
                                                            <div id="div1" runat="server" style="margin-bottom: 1px;">
                                                                <asp:Label runat="server" ID="lblNewAccessions" Text="New Accessions" class="ClsLblLgnd"
                                                                    Style="font-weight: bold" EnableViewState="true" Visible="false"></asp:Label><br />
                                                            </div>
                                                            <asp:GridView ID="grdBookNo" TabIndex="25" runat="server" Width="900px" AutoGenerateColumns="False"
                                                                DataKeyNames="Book_No,Book_Detail_Id,Book_Issue_Status,VendorId ,IsGifted, PurchaseDate"
                                                                AllowSorting="True" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                                                EmptyDataText="No Record Available." OnRowCommand="grdBookNo_RowCommand" OnRowDataBound="grdBookNo_RowDataBound"
                                                                CssClass="GridBorder" OnRowCreated="grdBookNo_RowCreated" OnSorting="grdBookNo_Sorting">
                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                                                    Font-Size="Small"></PagerStyle>
                                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                <Columns>
                                                                    <asp:BoundField DataField="Book_No" HeaderText="Accession Number" SortExpression="Book_No"
                                                                        HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="190px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Book_Price" HeaderText="Book Price" SortExpression="Book_Price"
                                                                        HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="130px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TotalPages" HeaderText="Total Pages" SortExpression="TotalPages"
                                                                        NullDisplayText="" HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="130px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IsGifted" HeaderText="Is Gifted?" SortExpression="IsGifted"
                                                                        HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="100px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate"
                                                                        HtmlEncode="False" DataFormatString="{0:dd MMM yyyy}">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="150px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="BillNo" HeaderText="Bill Number" SortExpression="BillNo"
                                                                        HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="120px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName"
                                                                        HtmlEncode="False">
                                                                        <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="180px" VerticalAlign="Middle" />
                                                                        <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                    <asp:ButtonField ButtonType="Image" CommandName="EDIT_ROW" HeaderText="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif">
                                                                        <ItemStyle HorizontalAlign="Center" Width="3%" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField ButtonType="Image" HeaderText="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                        CommandName="DELETE_ROW">
                                                                        <ItemStyle HorizontalAlign="Center" Width="3%" VerticalAlign="Middle" Wrap="False" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                                    </asp:ButtonField>
                                                                </Columns>
                                                                <RowStyle CssClass="ClsGridRow" />
                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td runat="server" align="center" style="height: 5px" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdAddBookGrd" runat="server" align="center" valign="top">
                                                            <div id="divPendingFees" runat="server">
                                                                <div id="divPendingHeader" runat="server" style="margin-bottom: 1px;">
                                                                    <asp:Label runat="server" ID="Label1" Text="Existing Accessions" class="ClsLblLgnd"
                                                                        Style="font-weight: bold" EnableViewState="False"></asp:Label><br />
                                                                </div>
                                                                <asp:GridView ID="grdvwAddBookNo" TabIndex="26" runat="server" Width="900px" AutoGenerateColumns="False"
                                                                    DataKeyNames="Book_No,Book_Detail_Id,PurchaseDate" AllowSorting="True" CellPadding="0"
                                                                    CellSpacing="1" ForeColor="#333333" GridLines="None" EmptyDataText="No Record Available."
                                                                    CssClass="GridBorder" OnRowCreated="grdvwAddBookNo_RowCreated" OnRowDataBound="grdvwAddBookNo_RowDataBound"
                                                                    OnSorting="grdvwAddBookNo_Sorting">
                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                        NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Book_No" HeaderText="Accession Number" HtmlEncode="False"
                                                                            SortExpression="Book_No">
                                                                            <ItemStyle CssClass="paddingLSML" Width="16%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Book_Price" HeaderText="Book Price" SortExpression="Book_Price"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="11%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="TotalPages" HeaderText="Total Pages" SortExpression="TotalPages"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="11%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="IsGifted" HeaderText="Is Gifted?" SortExpression="IsGifted"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="11%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" SortExpression="PurchaseDate"
                                                                            HtmlEncode="False" DataFormatString="{0:dd MMM yyyy}">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="13%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="BillNo" HeaderText="Bill Number" SortExpression="BillNo"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="12%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName"
                                                                            HtmlEncode="False">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="15%" VerticalAlign="Middle" />
                                                                            <HeaderStyle CssClass="paddingLSML" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        </asp:BoundField>
                                                                        <asp:ButtonField HeaderText="Edit">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="3%" VerticalAlign="Middle" />
                                                                        </asp:ButtonField>
                                                                        <asp:ButtonField HeaderText="Delete">
                                                                            <ItemStyle CssClass="paddingLSML" HorizontalAlign="Left" Width="3%" VerticalAlign="Middle" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                    <PagerStyle Font-Names="Arial" Font-Size="Small" Font-Underline="False" ForeColor="Black"
                                                                        HorizontalAlign="Right" />
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:WizardStep>
                                        </WizardSteps>
                                        <FinishNavigationTemplate>
                                            <asp:Button ID="FinishPreviousButton" TabIndex="27" runat="server" CausesValidation="False"
                                                CommandName="MovePrevious" CssClass="ClsBtnMid" Text="Previous" />
                                            <asp:Button ID="FinishButton" TabIndex="28" runat="server" CommandName="MoveComplete"
                                                CssClass="ClsBtnMid" Text="Finish" />
                                            <asp:Button ID="CancelButton" TabIndex="29" runat="server" CausesValidation="False"
                                                CommandName="Cancel" CssClass="ClsBtnMid" Text="Cancel" />
                                        </FinishNavigationTemplate>
                                        <StartNavigationTemplate>
                                            <asp:Button ID="StartNextButton" runat="server" TabIndex="30" CausesValidation="True"
                                                CommandName="MoveNext" CssClass="ClsBtnMid" Text="Next" />&nbsp;
                                            <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                                CssClass="ClsBtnMid" Text="Cancel" />
                                        </StartNavigationTemplate>
                                        <StepNavigationTemplate>
                                            <asp:Button ID="StepPreviousButton" runat="server" TabIndex="31" CausesValidation="False"
                                                CommandName="MovePrevious" CssClass="ClsBtnMid" Text="Previous" />
                                            <asp:Button ID="StepNextButton" runat="server" TabIndex="32" CommandName="MoveNext"
                                                CssClass="ClsBtnMid" Text="Next" />
                                            <asp:Button ID="CancelButton" runat="server" TabIndex="33" CausesValidation="False"
                                                CommandName="Cancel" CssClass="ClsBtnMid" Text="Cancel" />
                                        </StepNavigationTemplate>
                                        <StepStyle ForeColor="#333333" />
                                        <SideBarStyle BackColor="#507CD1" VerticalAlign="Top" />
                                        <NavigationButtonStyle CssClass="ClsBtnMid" />
                                        <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" ForeColor="White" />
                                        <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                                            Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    </asp:Wizard>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidBookId" runat="server" />
                        <asp:HiddenField ID="hidBookSrNo" runat="server" />
                        <asp:HiddenField ID="hidIsNewBook" runat="server" />
                        <asp:HiddenField ID="hidIndexNo" runat="server" />
                        <asp:HiddenField ID="hidIsNewBookNo" runat="server" />
                        <asp:HiddenField ID="hidIsAddQuantity" runat="server" />
                        <asp:HiddenField ID="hidModeType" runat="server" />
                        <asp:HiddenField ID="hidPurchaseDate" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientGridBookNo = "<%=this.grdBookNo.ClientID %>";
        _clientLableMsg = "<%=this.lblMessage.ClientID %>";
        _clientddlCategoryId = "<%=this.cmbMainCategory.ClientID %>";
        _clientvalsumBookNo = "<%=this.valsumBookNo.ClientID %>";
        _clienttxtPurchaseDate = "<%=this.txtPurchaseDate.ClientID %>"
        _clientchkIsGifted = "<%=this.chkIsGifted.ClientID %>"
        _clientcmbVendorName = "<%=this.cmbVendorName.ClientID %>"
        _clienttxtBillNo = "<%=this.txtBillNo.ClientID %>"
        _clienttxtLostPercentage = "<%=this.txtLostPercentage.ClientID %>"
        _clientChkAllStandards = "<%=this.ChkAllStandards.ClientID %>"
        _clientchkListClasses = "<%=this.chkListClasses.ClientID %>"
        _clienttxtBookNo = "<%=this.txtBookNo.ClientID %>"
        _clienttxtBookPrice = "<%=this.txtBookPrice.ClientID %>"
        _clienttxtTotPages = "<%=this.txtTotPages.ClientID %>"
        _clienttxtDescription = "<%=this.txtDescription.ClientID %>"
        _clienttxtRemark = "<%=this.txtRemark.ClientID %>"
        _clientCheckStandard = "<%=this.chkListClasses.ClientID %>"

        function UnCheck() {
            var checkStd = $("[id*='_chkListClasses_']");
            var listLenght = $("[id*='_chkListClasses_']").length;
            var chkCount = 0;
            for (var i = 0; i < listLenght; i++) {
              var chk = checkStd[i];
              if (!chk.checked) {
                  document.getElementById(_clientChkAllStandards).checked = chk.checked;
                  break; 
                 }
              else
                  chkCount = i;
          }
          if (chkCount==listLenght-1) {
            document.getElementById(_clientChkAllStandards).checked =true;
          }
        }
        function UncheckOrCheckAll() {
           var checkStd = $("[id*='_chkListClasses_']");
           var listLenght=$("[id*='_chkListClasses_']").length;
           var checkAll = document.getElementById(_clientChkAllStandards);
           for (var i = 0; i < listLenght; i++) 
                checkStd[i].checked = checkAll.checked
         
            }
        
        function ValidateVendoreName(oSrc, args) {
            var iVendoreIndex = document.getElementById(_clientcmbVendorName).selectedIndex
            if ((document.getElementById(_clientchkIsGifted).checked) == false) {
                if (iVendoreIndex <= 0) {
                    oSrc.errormessage = "Vendor name should be selected."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }

        function ValidateAccessionNo(oSrc, args) {

            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtBookNo).value) == "") {
                oSrc.errormessage = "Accession number should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ValidatePrice(oSrc, args) {
            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtBookPrice).value) == "") {
                oSrc.errormessage = "Price (Rs.) should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ValidateTotalPage(oSrc, args) {
            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtTotPages).value) == "") {
                oSrc.errormessage = "Total pages should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ValidateBillNo(oSrc, args) {

            if ((document.getElementById(_clientchkIsGifted).checked) == false) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtBillNo).value) == "") {
                    oSrc.errormessage = "Bill number should not be blank."
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        function ValidatePurchaseDate(oSrc, args) {
            var today = new Date()
            var dtPurchaseDate
            if ((document.getElementById(_clientchkIsGifted).checked) == false) {
                if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtPurchaseDate).value) == "") {
                    oSrc.errormessage = "Purchase Date should not be blank."
                    args.IsValid = false
                    return true
                }
            }
            if (stripLeadingTrailingBlanks(document.getElementById(_clienttxtPurchaseDate).value) != "") {
                if (document.all)
                    dtPurchaseDate = new Date((document.getElementById(_clienttxtPurchaseDate).value).replace('-', ' '))
                else
                    dtPurchaseDate = new Date(convertdate(document.getElementById(_clienttxtPurchaseDate).value))
                if (today < dtPurchaseDate) {
                    oSrc.errormessage = "Purchase Date should not be future date."
                    args.IsValid = false
                    return true
                }
            }


            args.IsValid = true
            return false
        }

        function ValidateLostPercentage(oSrc, args) {
            var iLostPercentage = document.getElementById(_clienttxtLostPercentage).value.trim();
            if (iLostPercentage != "" && parseFloat(iLostPercentage) > 100) {
                oSrc.errormessage = "Percentage lost should not be greater than 100."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateRemark(oSrc, args) {
            var txtRemark = document.getElementById(_clienttxtRemark).value.trim();
            if (txtRemark.length > 100) {
                oSrc.errormessage = "Remark should not be greater than 100."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateDescription(oSrc, args) {
            var txtDescription = document.getElementById(_clienttxtDescription).value.trim();
            if (txtDescription.length > 100) {
                oSrc.errormessage = "Description should not be greater than 100."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm('Are you sure you want to delete this Book(s)?')) {
                bResult = false;
            }
            return bResult;
        }

        function SetdefaultButton() {
            if (document.getElementById(_clientPnlFields)) {
                document.getElementById(_clientPnlFields).DefaultButton = _clientbtnSearch1;
            }
        }

        function ConfirmAction(iPageCount, sActionName) {
            var bResult = true;

            if (!window.confirm("Are you sure you want to delete this Book(s)?")) {
                bResult = false;
            }

            return bResult;
        }

        function ValidateBookCategory(oSrc, args) {
            var bResult = true;
            ddlCategory = document.getElementById(_clientddlCategoryId);
            var iVendoreIndex = document.getElementById(_clientddlCategoryId).selectedIndex
            if (ddlCategory.length <= 0 || iVendoreIndex <= 0) {
                bResult = false;
            }
            if (bResult == false) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

       

        function ClearErrorMsg() {            
            if (document.getElementById(_clientLableMsg) != null)
                document.getElementById(_clientLableMsg).style.display = "none";
            if (document.getElementById(_clientvalsumBookNo) != null)
                document.getElementById(_clientvalsumBookNo).style.display = "none";
            return true;
        }

        function validateGridData(oSrc, args) {
            var grdViewElement = document.getElementById(_clientGridBookNo)
            if (null == grdViewElement) {
                if (document.getElementById(_clientLableMsg).style.display != "none")
                    document.getElementById(_clientLableMsg).style.display = "none";

                args.IsValid = false;
                return true;
            }
            else {
                if (grdViewElement.rows.length > 0) {
                    alert("grid filled");
                    args.IsValid = true;
                    return false;
                }
                else {
                    alert("grid empty");
                    args.IsValid = false;
                    return true;
                }
            }
        }
        //This function is used to disable edit and delete button.
        function NoAction() {
            return false;
        }

        function blockNonAlphanumericCharacter(obj, e) {

            var isCtrl = false;
            var key;
            var isCtrl = false;
            var keychar;

            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }
            // check for backspace or delete or <- or ->, or if Ctrl was pressed
            if (key == 8 || key == 127 || key == 28 || key == 29 || isCtrl) {
                return true;
            }
            keychar = String.fromCharCode(key)
            var txtBillNo = keychar;
            var bIsValid = true
            if (txtBillNo == "")
                bIsValid = false
            else {
                var regExp = /([a-z]|[A-Z]|[0-9])/;
                var matchArray = regExp.exec(txtBillNo)
                if (matchArray == null) {
                    bIsValid = false;
                    return false;
                }
            }
        }

    </script>
</asp:Content>
