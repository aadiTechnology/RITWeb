<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SubjectTestConfigurationDetails.aspx.cs" Inherits="SubjectTestConfigurationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    


    <table width="100%">
       <tr align="center" style="width:500px"  >
          <td>
              <div id="divErr" runat="server" style="width:450px;" visible="false">
                 <table class="LblNoRecord"   cellpadding="0" cellspacing="0">
                      <tr>
                         <td class="ClsConfigText">Please configure following details for School :
                         </td>
                      </tr>
                      <tr align="left">
                         <td > 
                             <asp:HyperLink ID="hlGradeConfiguration" Text="Percentage Grades" runat="server" ForeColor="Blue" Visible="false" />
                         </td>
                      </tr>
                      <tr align="left">
                      <td >
                             <asp:HyperLink ID="hlFailCriteria" Text="Fail Criteria" runat="server" ForeColor="Blue" Visible="false" />
                         </td>
                      </tr>
                       
                 </table>
               </div> 
          </td>
          
       </tr>
        <tr>
              <td align="center">
                        <asp:Button ID="btnClose" CssClass="ClsBtn" Text="Back" Visible="false"  
                            runat="server" CausesValidation="false"
                             TabIndex="10" onclick="btnClose_Click" />
                </td>
         </tr>
          
       <tr id="mainDiv" runat="server">
        <td>
       <table cellpadding="2" cellspacing="2" width="100%" >
       
        <tr align="center">
            <td align="left">
                <asp:ValidationSummary ID="valsumCopyConfig" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                    ShowSummary="true" ValidationGroup="ValidateCopy" />
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="ValidateAdd" />
                <asp:ValidationSummary ID="valSumUpdateFactor" runat="server" CssClass="ClsLabel"
                    ValidationGroup="UpdateFactor" />
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" EnableViewState="false"
                            Font-Bold="true" Font-Size="Small" ForeColor="Blue" Visible="true" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="BtnUpdateFactor" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                          <asp:AsyncPostBackTrigger ControlID="btndelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td align="left">
                <table style="width: 100%;">
                    <tr class="ClsBtmBorderGray">
                        <td align="left" style="width: 50%">
                            <table cellpadding="1" cellspacing="2">
                                <tr>
                                    <td>                                       
                                            <span class="ClsLblLgnd" style="font:Bold;border-width:0px">Class : </span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <asp:Label ID="lblStdDivValue" runat="server" EnableViewState="True" />
                                    </td>
                                    <td>                                        
                                            <span class="ClsLblLgnd" style="font:Bold;border-width:0px"> Subject : </span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <asp:Label ID="lblSubjectValue" runat="server" EnableViewState="True" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="1" cellspacing="2" border="0" runat="server" id="tblARFactor">
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLblLgnd" style="font-weight: bold;">Final Result Factor :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox CssClass="TxtBoxNOL" ID="txtFactor" runat="server" MaxLength="5" onblur="extractNumber(this,2,false);"
                                                    onkeyup="extractNumber(this,2,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                <span class="ClsMdtStar" style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="cstvalUpdateFactor" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    Visible="true" ErrorMessage="Final Result Factor should be between 0 to 99" ClientValidationFunction="ValidateARF"
                                                    ValidationGroup="UpdateFactor">
                                                </asp:CustomValidator>
                                                <asp:CustomValidator ID="cstValUpdateFactorOnUpdate" Display="None" runat="server"
                                                    CssClass="ClsMdtStar" Visible="true" ErrorMessage="Final Result Factor should be between 0 to 99"
                                                    ClientValidationFunction="ValidateARF" ValidationGroup="ValidateAdd">
                                                </asp:CustomValidator>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="BtnUpdateFactor" CausesValidation="true" runat="server" CssClass="ClsBtn"
                                                    Text="Update Factor" Width="90px" Enabled="False" ValidationGroup="UpdateFactor"
                                                    OnClick="BtnUpdateFactor_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td align="right">
                            <span class="ClsMdtStar">* Mandatory Fields</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 50%" colspan="3">
                            <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="1" cellspacing="2" width="100%">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 50%">
                                                <asp:CheckBox runat="server" ID="chkTotalConsider" Text="Should be considered in totals" />
                                            </td>
                                            <td align="left" class="ClsBorderlight" style="width: 50%">
                                                <asp:RadioButton ID="optMarks" runat="server" Text="Marking System" OnCheckedChanged="optMarks_CheckedChanged"
                                                    GroupName="MarkGrade" AutoPostBack="true" />
                                                <asp:RadioButton ID="optGrade" runat="server" Text="Grading System" OnCheckedChanged="optGrades_CheckedChanged"
                                                    AutoPostBack="true" GroupName="MarkGrade" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ChkRslt" EventName="CheckedChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnCopy" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnUpdateFactor" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="btndelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Panel ID="pnlContainer" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table width="800px" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td class="ClsBorderlight" style="width: 30%">
                                        <asp:Label ID="lblGroupName" runat="server" CssClass="LblNrmlB" Text="Select Exam :"
                                            maxlength="20" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td align="left" valign="top" class="ClsBorderlight">
                                        <span>
                                            <asp:DropDownList AutoPostBack="true" ID="cmbExams" runat="server" CssClass="LrgCombo"
                                                OnSelectedIndexChanged="cmbExams_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;<span style="color: red"> * </span></span>
                                    </td>
                                    <asp:CompareValidator  ID="CompareValidator1" runat="server" Display="None" ErrorMessage="Exam should be selected."
                                        CssClass="ClsMdtStar" ControlToValidate="cmbExams" Operator="NotEqual" ValidationGroup="ValidateAdd"
                                        ValueToCompare="0"></asp:CompareValidator>
                                </tr>
                                <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <tr runat="server" id="trConsiderAR">
                                            <td class="ClsBorderlight">
                                                <span class="LblNrmlB">Consider in Final result?</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="3">
                                                <span style="color: red">
                                                    <asp:CheckBox Checked="true" ID="ChkRslt" runat="server" CssClass="LblSmlGray" AutoPostBack="true"
                                                        Style="vertical-align: middle;" Text="(Deselect this option if you do not want the marks of this exam to be considered in final result.)"
                                                        OnCheckedChanged="ChkRslt_CheckedChanged" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trGrade">
                                            <td class="ClsBorderlight">
                                                <span class="LblNrmlB">Passing Grade :</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight">
                                                <span style="color: red">
                                                    <asp:DropDownList ID="cmbPassingGrade" runat="server"  CssClass="MidCombo" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                    &nbsp;<span style="color: #ff0000">*</span> </span>
                                                <asp:CustomValidator ID="CustomValidator1"  runat="server" CssClass="ClsMdtStar" Display="None"
                                                    Visible="true" ClientValidationFunction="ValidateGrade" ValidationGroup="ValidateAdd"
                                                    ErrorMessage="Passing grade should be selected.">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderlight">
                                                <span class="LblNrmlB">Is Exam Status Applicable?</span>
                                            </td>
                                            <td class="ClsBorderlight" align="left">
                                                <asp:CheckBox ID="chkExamStatus" runat="server" CssClass="LblSmlGray" Checked="true"
                                                    Style="vertical-align: middle;" Text="(Deselect this option if you do not want to allow exam status selection for this exam.)" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trDecimal">
                                            <td class="ClsBorderlight">
                                                <span class="LblNrmlB">Allow Decimal Numbers?</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="3">
                                                <span style="color: red">
                                                    <asp:CheckBox Checked="false" ID="chkAllowDecimal" runat="server" CssClass="LblSmlGray"
                                                        AutoPostBack="true" Style="vertical-align: middle;" Text="(Select this option to allow mark assignment in decimal numbers.)" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr id="trDisplayGradeRow" runat="server">
                                            <td class="ClsBorderlight">
                                                <span class="LblNrmlB">Display Grade on Report?</span>
                                            </td>
                                            <td class="ClsBorderlight" align="left">
                                                <asp:CheckBox ID="chkDisplayGrade" runat="server" CssClass="LblSmlGray" Style="vertical-align: middle;"
                                                    Text="(Select this option if you want to show grade instead of marks for third language (if applicable) on the progress reports. Third language will not be considered in the total.)" />
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCopy" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                            <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btndelete" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:CustomValidator ID="cst_CheckCount" Display="None" runat="server" CssClass="ClsMdtStar"
                    Visible="true" ErrorMessage="At least 1 exam type should be selected." ClientValidationFunction="ValidateChkCount"
                    ValidationGroup="ValidateAdd"> </asp:CustomValidator>
          
        <tr align="center">
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 800px;" cellpadding="0" cellspacing="0" runat="server" id="tblTestTypeGrid">
                            <tr>
                                <td valign="top" class="GridBorder">
                                    <table cellpadding="0" cellspacing="1" style="width: 100%">
                                        <tr>
                                            <td colspan="5" style="height: 5px" class="ClsGridBG">
                                                <asp:GridView ID="grdTestTypes" DataKeyNames="TestType_Id,TestType_Total_Marks,TestType_Passing_Marks,TestTypeOutOfMarks,TestType_Name,AllowDecimal"
                                                    AllowPaging="false" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="50"
                                                    CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" OnRowDataBound="grdTestTypes_rowDatabound">
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Underline="False" Font-Names="Arial"
                                                        Font-Size="Small"></PagerStyle>
                                                    <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                        FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkTestType" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                                            <HeaderStyle Width="1%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exam Types" HeaderStyle-HorizontalAlign="Left"
                                                            SortExpression="TestType_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTestTypeName" runat="server" />                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle Width="75%" />
                                                            <HeaderStyle Width="75%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Marks" SortExpression="TestType_Total_Marks">
                                                            <EditItemTemplate>
                                                                &nbsp;
                                                            </EditItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtTotMarks" runat="server" MaxLength="5" CssClass="SmlTxtBox" onblur="extractNumber(this,0,false);"
                                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Passing Marks" SortExpression="TestType_Passing_Marks">
                                                            <EditItemTemplate>
                                                                &nbsp;
                                                            </EditItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassingMarks" CssClass="SmlTxtBox" runat="server" MaxLength="5"
                                                                    onblur="extractNumber(this,AllowDecimal(this),false);" onkeyup="extractNumber(this,AllowDecimal(this),false);"
                                                                    onkeypress="return blockNonNumbers (this, event, AllowDecimal(this), false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Out of Marks**" SortExpression="OutOfMarks">
                                                            <EditItemTemplate>
                                                                &nbsp;
                                                            </EditItemTemplate>
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtOutOfMarks" CssClass="SmlTxtBox" runat="server" MaxLength="5"
                                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <SelectedRowStyle Font-Bold="True" ForeColor="#333333" />
                                                    <RowStyle CssClass="ClsGridRow" />
                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                    <EmptyDataRowStyle CssClass="LblNoRecord" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="ClsGridBG" runat="server" id="trTotalMarks">
                                            <td style="width: 490px; padding-right: 15px;" align="right">
                                                <asp:Label ID="totalMarks" runat="server" CssClass="LblNrmlB" EnableViewState="false">Total:</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtAllTotalMarks" Width=" 95px" Height="25px" runat="server" ReadOnly="true"
                                                    TabIndex="-1" CssClass="ClsHilightBGB" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                            </td>
                                            <td>
                                                <img alt="spacer" src="../images/spacer.gif" width="1px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAlltotPassingMarks" Width=" 95px" Height="25px" runat="server"
                                                    TabIndex="-1" ReadOnly="true" CssClass="ClsHilightBGB" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                            </td>
                                            <td id="tdtxtOutOfMarks" runat="server" valign="top">
                                                <asp:TextBox ID="txtTestOutOfMarks" runat="server" MaxLength="3" CssClass="SmlTxtBox"
                                                    Style="height: 25px" ReadOnly="false" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hidMode" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
      </td> </tr>
    </table>
    <asp:CustomValidator ID="cst_Marks" Display="None" runat="server" CssClass="ClsMdtStar"
        Visible="true" ClientValidationFunction="ValidateMarks" ValidationGroup="ValidateAdd"> </asp:CustomValidator>
    <asp:CustomValidator ID="cstDecimal" Display="None" runat="server" CssClass="ClsMdtStar"
        Visible="true" ClientValidationFunction="ValidateAllowDecimalMarks" ValidationGroup="ValidateAdd"> </asp:CustomValidator>
    <asp:CustomValidator ID="cstValidateforCopy" Display="None" runat="server" CssClass="ClsMdtStar"
        Visible="true" ErrorMessage="At least one exam should be configured." EnableClientScript="true"
        ClientValidationFunction="ValidateForCopy" ValidationGroup="ValidateCopy"></asp:CustomValidator>
    <asp:Panel ID="pnlSubjectTest" runat="server">
        <table cellpadding="1" cellspacing="2" style="width: 100%">
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="divOutOfMarksUpdPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divOutOfMarksNote" runat="server" style="width: 800px; text-align: left;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" colspan="1" class="ClsBorderlight " style="background-color: #ffffc4;
                                            width: 20px">
                                            <span class="LblNrmlB" style="font-weight: bold; padding: 2px 4px;">** </span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight" style="padding: 2px 4px;">
                                            <span class="LblSmlV">You can enter Out of Marks either for Exam or individual Exam
                                                Type. If Out of Marks is not entered, then Total Marks will be considered as Out
                                                of Marks for that Exam or Exam Type.</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="optGrade" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="optMarks" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAdd" ValidationGroup="ValidateAdd" runat="server" OnClick="btnAdd_Click"
                                CssClass="ClsBtn" Text="Add" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbExams" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 800px;" class="ClsGridBG GridBorder" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView AllowSorting="true" runat="server" ID="grdSubjectTestConfiguration"
                                            DataKeyNames="SchoolWise_Test_Id,TestWise_Subject_Marks_Id,Grade_Or_Marks, Result_Consideration,IsSubmitted,IsPublished,IsExamMarkEntered,IsStudentWiseProgressReportPublished"
                                            OnRowCommand="grdSubjectTestConfiguration_rowCommand" EmptyDataText="Exam for this subject not yet configured."
                                            OnRowDataBound="grdSubjectTestConfiguration_RowDataBound" Width="100%" AutoGenerateColumns="False"
                                            PageSize="30" CellPadding="2" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                            OnSorting="grdSubjectTestConfiguration_Sorting" OnRowCreated="grdSubjectTestConfiguration_RowCreated">
                                            <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                            </PagerStyle>
                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Consider ">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgConsider" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Exam Name" SortExpression="SortOrder" DataField="SchoolWise_Test_Name">
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Total Marks" DataField="Subject_Total_Marks">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Total Passing Marks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPassingMarks" runat="server" Text='<%# Convert.ToBoolean(Eval("AllowDecimal"))? Convert.ToDecimal(Eval("Passing_Total_Marks")) : (Eval("Grade_Or_Marks").ToString() == "M") ? Convert.ToInt32(Convert.ToDecimal(Eval("Passing_Total_Marks"))) : Eval("Passing_Total_Marks")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Out of Marks" DataField="OutOfMarks">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:BoundField>
                                                <asp:ButtonField ButtonType="Image" ItemStyle-HorizontalAlign="Center" HeaderText="Delete"
                                                    CommandName="DeleteRow" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Text="Delete" />
                                                <asp:ButtonField ButtonType="Button" HeaderText="Delete Exam Marks" Text="Delete"
                                                    CommandName="DELETE_EXAM_MARKS">
                                                    <ControlStyle CssClass="ClsBtnSml" />
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <RowStyle CssClass="ClsGridRow" />
                                            <HeaderStyle CssClass="ClsGridHeader" />
                                            <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                            <EmptyDataRowStyle BackColor="#E6EEFC" HorizontalAlign="Center" CssClass="LblNoRecord" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="trRslt">
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3" style="width: 35%" align="right">
                                                    <span class="LblNrmlB">Passing Grade :</span>
                                                </td>
                                                <td align="left" style="width: 145px">
                                                    <asp:TextBox ID="txtRsltTot" Width="145px" Height="25px" runat="server" ReadOnly="true"
                                                        TabIndex="-1" CssClass="ClsHilightBGB" />
                                                </td>
                                                <td>
                                                    <img alt="spacer" src="../images/spacer.gif" width="3px" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRsltPassing" Width="145px" Height="25px" runat="server" ReadOnly="true"
                                                        TabIndex="-1" CssClass="ClsHilightBGB" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <asp:HiddenField ID="HidField_URL" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidDeleteStudentWiseSavedMarks" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidIsDisplayGradeApplicable" runat="server" />
                                        <asp:HiddenField ID="hidSelectedStdId" runat="server" Value="0"></asp:HiddenField>
                                        <asp:HiddenField ID="hidSelectedSubjectId" runat="server" Value="0"></asp:HiddenField>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btndelete" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnUpdateFactor" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidTestOutOfMarksApplicable" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hidTestTypeOutOfMarksApplicable" runat="server"></asp:HiddenField>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBack" CausesValidation="false" runat="server" OnClick="btnBack_Click"
                                CssClass="ClsBtn" Text="Back" UseSubmitBehavior="false" />
                            <asp:Button ID="btnCopy" CausesValidation="false" runat="server" OnClick="btnCopy_Click"
                                CssClass="ClsBtnExLrg" Text="Copy Subject Exam Configuration" ValidationGroup="ValidateCopy" />
                                 <asp:Button ID="btnDelete"  runat="server" onClick="btnDelete_Click" 
                                CssClass="ClsBtn" Text="Delete All"  />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="grdSubjectTestConfiguration" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script language="javascript" type="text/javascript">
		_clientGridId = "<%=grdSubjectTestConfiguration.ClientID %>";
		_clientTestTypeGridId = "<%=grdTestTypes.ClientID %>";
		_clientoptMarksId = "<%=optMarks.ClientID %>";
		_clientoptGradeId = "<%=optGrade.ClientID %>";
		_clienttxtFactorId = "<%=txtFactor.ClientID %>";
		_clientErrLabelId = "<%=lblError.ClientID %>";
		_clientLblError = "<%=lblError.ClientID %>";
		_clientlblSuccessMsg = "<%=lblSuccessMsg.ClientID %>";        
		_clientCstmarks = "<%=cst_Marks.ClientID %>";
		_clientbtnBack = "<%=btnBack.ClientID %>";
		_clientbtnAdd = "<%=btnAdd.ClientID %>";
		_clientcstvalUpdateFactor = "<%=cstvalUpdateFactor.ClientID %>";
		_clientcmbPassingGrade = "<%=cmbPassingGrade.ClientID %>";
		_clientValsumCopyConfig = "<%=valsumCopyConfig.ClientID %>";
		_clientValSumErrorMsg = "<%=valSumErrorMsg.ClientID %>";
		_clientcmbExams = "<%=cmbExams.ClientID %>";
		_clienttxtAllTotalMarks = "<%=txtAllTotalMarks.ClientID %>";
		_clienttxtAlltotPassingMarks = "<%=txtAlltotPassingMarks.ClientID %>";
		_clienthidDeleteStudentWiseSavedMarks = "<%=hidDeleteStudentWiseSavedMarks.ClientID %>";
		_clienttxtTestOutOfMarks = "<%=txtTestOutOfMarks.ClientID %>";
		_clienthidTestOutOfMarksApplicable = "<%=hidTestOutOfMarksApplicable.ClientID %>";
		_clienthidTestTypeOutOfMarksApplicable = "<%=hidTestTypeOutOfMarksApplicable.ClientID %>";

		_rowCount = <%= this.grdTestTypes.Rows.Count %>;

		function disableButtons(objBtn) {
			var isPageValid = true;
			
			if (objBtn == $get(_clientbtnAdd)) {
				if (typeof (Page_ClientValidate) == 'function') {
					isPageValid = Page_ClientValidate("ValidateAdd");
				}
			}

			if (isPageValid) {                            
				var txtTestOutOfMarks = $get(_clienttxtTestOutOfMarks);
				var optMarks = $get(_clientoptMarksId);
				if(optMarks && optMarks.checked) {
					var bStatus = false;
					var iStart = 2;
					var iCount = $get(_clientTestTypeGridId).rows.length + 1;
					var sRow = "";
					var iTotalMarks = 0;
					var iTotalPassingMarks = 0;

					for (var i = iStart; i < iCount; i++) {
						if (i < 10) 
							sRow = "0" + i;
						else 
							sRow = i;
						var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
						var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
						var txtOutOfMarks = $get(_clientTestTypeGridId + "_ctl" + sRow + "_txtOutOfMarks");
						var sIdChkTestType = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";

						if ($get(sIdChkTestType).checked
								&& (txtTestOutOfMarks.value == '0' || txtTestOutOfMarks.value == '')
									&& (txtOutOfMarks.value != '0' && txtOutOfMarks.value != '')
										&& !bStatus) {
								bStatus = true;
								$get(_clienthidTestTypeOutOfMarksApplicable).value = 'Y';
								$get(_clienthidTestOutOfMarksApplicable).value = 'N';
								i = iStart;
								break;
						}
					}

					if((txtTestOutOfMarks.value == '' || txtTestOutOfMarks.value == '0') && !bStatus)
					{
						txtTestOutOfMarks.value = $get(_clienttxtAllTotalMarks).value;
						$get(_clienthidTestTypeOutOfMarksApplicable).value = 'N';
						$get(_clienthidTestOutOfMarksApplicable).value = 'Y';
					}
					else if(!bStatus)
					{
						$get(_clienthidTestTypeOutOfMarksApplicable).value = 'N';
						$get(_clienthidTestOutOfMarksApplicable).value = 'Y';
					}
				}
				return true;
			} 
		}

		function VisibleSummary() {
			$get(_clientValSumErrorMsg).style.display = 'none';
			$get(_clientLblError).style.display = 'none';
			$get(_clientlblSuccessMsg).style.display = 'none';
		}

		function resetErrorLabel() {
			$get(_clientLblError).innerText = "";
			$get(_clientLblError).innerHTML = "";
			$get(_clientlblSuccessMsg).innerHTML = "";            
		}

		function ValidateForCopy(oSrc, args) {
			var bResult = true;
			var iGridRowCount = 0;

			if ($get(_clientGridId)) {
				iGridRowCount = parseInt($get(_clientGridId).rows.length);
			}

			if (iGridRowCount <= 2) {
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}
		
		function ConfirmCopy() {
			var bResult = true;
			var iGridRowCount = 0;

			if ($get(_clientGridId)) 
				iGridRowCount = parseInt($get(_clientGridId).rows.length);

			var iTestCount = parseInt($get(_clientcmbExams).options.length);

			if ((iGridRowCount != 0) && (iGridRowCount < iTestCount)) {
				if (!window.confirm("You have not configured all exams, do you still want to copy subject exam configuration?"))
				  bResult = false; 
			}
			return bResult;
		}
        function ConfirmCopy1() {
			var bResult = true;
			var iGridRowCount = 0;

			if ($get(_clientGridId)) 
				iGridRowCount = parseInt($get(_clientGridId).rows.length);

			var iTestCount = parseInt($get(_clientcmbExams).options.length);

			if ((iGridRowCount != 0) && (iGridRowCount < iTestCount)) {
				if (!window.confirm("Are you sure you want to delete exam configuration of all exams?"))
				  bResult = false; 
			}
			return bResult;
		}

		function EnableDisableAllTextBoxes(obj) {
			var bGrade = $get(_clientoptGradeId).checked;

			if (!bGrade) {
				var bAction;
				if (obj.checked) 
					bAction = false;				
				else 
					bAction = true;
				
				var iStart = 2;
				var iCount = $get(_clientTestTypeGridId).rows.length + 1;
				var sRow = "";
				var iTotalMarks = 0;
				var iTotalPassingMarks = 0;

				for (var i = iStart; i < iCount; i++) {
					if (i < 10) 
						sRow = "0" + i;
					else 
						sRow = i;
					var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
					var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
					var sIdChkTestType = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";

					$get(sIdTotalMarks).disabled = bAction;
					$get(sIdPassingMarks).disabled = bAction;

					if (bAction) {
						$get(sIdTotalMarks).value = "0";
						$get(sIdPassingMarks).value = "0";
					}
				}
			}
		}
		
		function EnableDisableGridTextBox(obj, iRowIndex) {
			var bGrade = $get(_clientoptGradeId).checked;

			if (!bGrade) {
				var sRow;
				var iStart = 2;
				iRowIndex = iRowIndex + iStart;
				if (iRowIndex < 10)
					sRow = "0" + iRowIndex;
				else
					sRow = iRowIndex;

				var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
				var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
				var sIdOutOfMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtOutOfMarks";
				if (obj.checked) {
					$get(sIdTotalMarks).disabled = false;
					$get(sIdPassingMarks).disabled = false;
					$get(sIdOutOfMarks).disabled = false;
				}
				else {

					$get(sIdTotalMarks).disabled = true;
					$get(sIdPassingMarks).disabled = true;
					$get(sIdOutOfMarks).disabled = true;
					$get(sIdOutOfMarks).value = "0";
					$get(sIdTotalMarks).value = "0";
					$get(sIdPassingMarks).value = "0";
					SetTotals();
				}
			}
		}

		function SetTotals(obj) {
			var bGrade = $get(_clientoptGradeId).checked;

			if (!bGrade) {
				var iStart = 2;
				var iCount = $get(_clientTestTypeGridId).rows.length + 1;

				var sRow = "";
				var iTotalMarks = 0;
				var iTotalPassingMarks = 0;
				for (var i = iStart; i < iCount; i++) {
					if (i < 10) 
						sRow = "0" + i;	
					else 
						sRow = i;
					var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
					var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
					var sIdOutOfMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtOutOfMarks";
					var sIdChkTestType = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";

					if ($get(sIdTotalMarks).value == "") 
						$get(sIdTotalMarks).value = 0;					
					if ($get(sIdPassingMarks).value == "" || $get(sIdPassingMarks).value == ".") 
						$get(sIdPassingMarks).value = 0;					
					if ($get(sIdChkTestType).checked) {
						iTotalMarks = parseInt(iTotalMarks);
						iTotalPassingMarks = parseFloat(iTotalPassingMarks)
						iMarks = parseInt(RemoveLeadingZeroes($get(sIdTotalMarks).value));
						iPassingMarks = parseFloat(RemoveLeadingZeroes($get(sIdPassingMarks).value));
						iTotalMarks = iMarks += iTotalMarks;
						iTotalPassingMarks = iPassingMarks += iTotalPassingMarks;
					}
				}
				$get(_clienttxtAllTotalMarks).value = iTotalMarks;
				$get(_clienttxtAlltotPassingMarks).value = iTotalPassingMarks;
			    ShowPassingMarksInDecimals();
			}
		}
		
        function ShowPassingMarksInDecimals() {
           $("input:text[id*=txtPassingMarks]").each(
                function() {
                    this.value = parseFloat(this.value).toFixed(!$("input:checkbox[id*=chkAllowDecimal]")[0].checked && parseFloat(this.value) % 1 == 0? 0 : 1);
                }
            );
        }

		function ResetGrid() {
			return false;
			var iStart = 2;
			var iCount = $get(_clientTestTypeGridId).rows.length + 1; //$get("grdTestTypes").rows.length + 1;

			var sRow = "";
			var iTotalMarks = 0;
			var iTotalPassingMarks = 0;
			for (var i = iStart; i < iCount; i++) {
				if (i < 10) 
					sRow = "0" + i;
				else 
					sRow = i;
				var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
				var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
				var sIdChtTestType = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";

				$get(sIdTotalMarks).value = "0";
				$get(sIdTotalMarks).disabled = true;
				$get(sIdPassingMarks).value = "0";
				$get(sIdPassingMarks).disabled = true;
				$get(sIdChtTestType).checked = false;
				$get(sIdChtTestType).disabled = true;
			}
			$get(_clienttxtAllTotalMarks).value = "0";
			$get(_clienttxtAlltotPassingMarks).value = "0";
		}

		function ValidateGrade(oSrc, args) {
			args.IsValid = $get(_clientoptGradeId).checked ? $get(_clientcmbPassingGrade).value != 0 : true;
		
			return !args.IsValid;
		}
		
		function ValidateMarks(oSrc, args) {
			resetErrorLabel();
			var bReturn = false;
			var bGrade = $get(_clientoptGradeId).checked;

			if (!bGrade) {
				var iStart = 2;
				var iCount = $get(_clientTestTypeGridId).rows.length + 1;
				var iChkCount = 0;
				var sRow;
				var completeMessage = "";
				for (var i = iStart; i < iCount; i++) {
					if (i < 10)
						sRow = "0" + i;
					else 
						sRow = i;
					var sIdCheckBox = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";

					if ($get(sIdCheckBox).checked) {
						iChkCount = 1;
						var sIdTotalMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtTotMarks";
						var sIdPassingMarks = _clientTestTypeGridId + "_ctl" + sRow + "_txtPassingMarks";
						var cnt = i - iStart + 1;
						var sBlankTotalErrMessage = "Row No." + cnt + ": Total marks should not be 0.";
						var sErrMessage = "Row No." + cnt + ": Passing marks should be less than total marks.";
						var sTot = RemoveLeadingZeroes($get(sIdTotalMarks).value);
						var sPass = RemoveLeadingZeroes($get(sIdPassingMarks).value);

						if (parseInt(sTot) == 0) {
							if (parseInt(sTot) == 0) {
								if (completeMessage != "")
									completeMessage = completeMessage + " <BR>" + sBlankTotalErrMessage;
								else 
									completeMessage = sBlankTotalErrMessage;
							}
							bReturn = true;
						}
						else {
							if (parseInt(sTot) <= parseInt(sPass)) {
								if (completeMessage != "") 
									completeMessage = completeMessage + "<BR>" + sErrMessage;
								else 
									completeMessage = sErrMessage;
								bReturn = true;
							}
						}
					}
				}
			}
			if (bReturn) {
				$get(_clientCstmarks).errormessage = completeMessage;
				args.IsValid = false;
				return true;
			}
			$get(_clientCstmarks).errormessage = completeMessage;
			args.IsValid = true;
			return false;
		}
		
		function ValidateARF(oSrc, args) {
			var bGrade = $get(_clientoptGradeId).checked;
			if ($get(_clienttxtFactorId) == null)
				args.IsValid = true;
			else {
				var sFactor = $get(_clienttxtFactorId).value;

				if (!bGrade) {
					if (sFactor == "") {
						oSrc.errormessage = "Update factor should not be blank.";
						args.IsValid = false;
					}
					else if (sFactor == ".") {
						oSrc.errormessage = "Invalid update factor.";
						$get(_clienttxtFactorId).value = "";
						args.IsValid = false;
					}
					else {
						var iFactor = parseFloat(sFactor);
						if (iFactor < 0 || iFactor > 99) {
							oSrc.errormessage = "Final Result Factor should be between 0 to 99.";
							args.IsValid = false;
						}
					}
				}
				else
					args.IsValid = true;
			}
			
			return !args.IsValid;
		}

		function ValidateChkCount(oSrc, args) {

			var bReturn = false;
			var iStart = 2;
			var bGrade = $get(_clientoptGradeId).checked;

			if (!bGrade) {
				var iCount = $get(_clientTestTypeGridId).rows.length + 1;
				var iChkCount = 0;
				var sRow;
				for (var i = iStart; i < iCount; i++) {
					if (i < 10) 
						sRow = "0" + i;
					else 
						sRow = i;
					var sIdCheckBox = _clientTestTypeGridId + "_ctl" + sRow + "_chkTestType";
					if ($get(sIdCheckBox).checked) {
						iChkCount = 1;
						args.IsValid = true;
						return false;
					}
				}
				if (iChkCount == 0) {
					args.IsValid = false;
					return true;
				}
				args.IsValid = true;
				return false;
			}
			else {
				args.IsValid = true;
				return false;
			}
		}
		
		function ConfirmDelete() {
			return window.confirm("Are you sure you want to delete this Subject exam configuration?");
		}

		function ConfirmDeleteExamMarksMessage(aIsPublish, sIsSubmitted, sExamName, sIsStudentWiseProgressReportPublished) {
			var bResult = true;
			var lblErrorMessage = $get(_clientLblError);
			lblErrorMessage.className = "LblErrorMsg";
			if (aIsPublish == "Y") {
				lblErrorMessage.innerHTML = sExamName + " exam marks can not be deleted. Since, exam is already published.";
				lblErrorMessage.innerText = sExamName + " exam marks can not be deleted. Since, exam is already published.";
				return false;
			}
			else if (sIsSubmitted == "N") {
				if (!window.confirm("Are you sure you want to delete exam marks?"))
					return false;
			}
			else if (sIsSubmitted == "Y") {
				if (!window.confirm(sExamName + " exam marks are already submitted. Are you sure you want to delete exam marks?"))
					return false;
			}
			
			if (sIsStudentWiseProgressReportPublished == "Y" ) {
				if (window.confirm("Marks which are published for student wise progress report will not be deleted. Are you sure you want to delete exam marks?")) {
					$get(_clienthidDeleteStudentWiseSavedMarks).value = "False";
					return true;
				}
				else
					return false;
			}

			if (!window.confirm("Delete marks saved for student wise progress report?"))
				$get(_clienthidDeleteStudentWiseSavedMarks).value = "False";
			else
				$get(_clienthidDeleteStudentWiseSavedMarks).value = "True";
			
			return bResult;
		}

		function resetValSummery() {
			if ($get(_clientValsumCopyConfig) != null)
				$get(_clientValsumCopyConfig).style.display = "none";
			if ($get(_clientValSumErrorMsg) != null)
				$get(_clientValSumErrorMsg).style.display = "none";
		}

		function chkTestTypeOnClick(src) {
			var txtOutOfMarks = $get(_clienttxtTestOutOfMarks);
			var optMarks = $get(_clientoptMarksId);
			if (optMarks.checked) {
				if (txtOutOfMarks.disabled && src.checked) {
					txtOutOfMarks.disabled = false;
				}
				if (!txtOutOfMarks.disabled && !src.checked) {
					var bChecked = false;
					for(var i = 0; i < _rowCount; i++) {
						var chkBox = $get(_clientTestTypeGridId + '_ctl0' + i + '_chkTestType');
						if(chkBox && chkBox.checked) {
							bChecked = true;
							break;
						}
					}
					if(!bChecked) {
						txtOutOfMarks.value = '';
						txtOutOfMarks.disabled = true;
					}
				}
			}	
		}

		function SetOutOfMarks(obj, objName) {
			var TestOutOfMarks = obj.value;
			
			if(objName == "TestOutOfMarks") {
				if(TestOutOfMarks.trim() != "" && TestOutOfMarks.trim() != "0")
					$("input:text[id*=txtOutOfMarks]").val('0');
				else
					obj.value= "0";
			}
			else
				$get(_clienttxtTestOutOfMarks).value = 0;
		}
		
        function AllowDecimal(obj) {
            return !$("input:checkbox[id*=chkAllowDecimal]")[0].checked && parseFloat(obj.value) % 1 == 0? 0 : 1;
        }
        
        SetControlProperties();
        
        function SetControlProperties() {
            $("input:text[id*=txtPassingMarks]").each(
                function() {
                    this.maxLength = $("input:checkbox[id*=chkAllowDecimal]")[0].checked ? 4 : 3;
                    this.value = parseFloat(this.value).toFixed(!$("input:checkbox[id*=chkAllowDecimal]")[0].checked && parseFloat(this.value) % 1 == 0? 0 : 1);
                }
            );
        }

        function ValidateAllowDecimalMarks(oSrc, args) {
            var iRowIndex = 1;
            var sExamTypes = '';
            var sErrorMsg = 'Passng marks cannot be in decimals for exam type(s): ';
            if($("input:radio[id*=optGrade]").checked)
            {
            if(!$("input:checkbox[id*=chkAllowDecimal]")[0].checked)
             {
                $("input:text[id*=txtPassingMarks]").each(
                    function() {
                        if(parseFloat(this.value) % 1 != 0) {
                            
                            sExamTypes += $get(this.id.replace('txtPassingMarks','lblTestTypeName')).innerHTML + ', ';
                        }
                        iRowIndex++;
                    }
                );
             }
            }
            
            if(sExamTypes != '') {
                sErrorMsg = sErrorMsg + sExamTypes.substring(0,sExamTypes.length - 2);
                oSrc.errormessage = sErrorMsg;
				args.IsValid = false;
            }
            else
                args.IsValid = true;
            
            return !args.IsValid;
        }

    </script>
     </td></tr>
     </table> 
</asp:Content>
