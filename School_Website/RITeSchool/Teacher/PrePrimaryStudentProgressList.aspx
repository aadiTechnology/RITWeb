<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="PrePrimaryStudentProgressList.aspx.cs" Inherits="PrePrimaryStudentProgressList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="width: 97%;">
        <table width="100%" align="center">
            <tr runat="server" id="trValidation">
                <td align="left">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                        ShowSummary="true" HeaderText="Please fix following error(s):" CssClass="ClsLabel">
                    </asp:ValidationSummary>
                </td>
            </tr>
            <tr id="trPrecondition" runat="server" visible="false">
                <td align="left">
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>            
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td align="center">
                                <table id="LegendTable" runat="server" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td align="left" colspan="1" rowspan="2" style="padding-right: 5px">
                                                <span class="ClsLblLgnd">Legend</span>
                                                </td>
                                        <td rowspan="2" style="width: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1">
                                            &nbsp;<asp:Image ID="Image1" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label4" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="Progress Report entry not started"></asp:Label></td>
                                        <td align="left" colspan="1">
                                            &nbsp;<asp:Image ID="Image2" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif" />
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label8" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="Progress Report entry partially done"></asp:Label></td>
                                        <td align="left" colspan="1">
                                            &nbsp;
                                            <asp:Image ID="Image3" runat="server" EnableViewState="False" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" /></td>
                                        <td align="left" colspan="1">
                                            <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" EnableViewState="false"
                                                Font-Bold="True" Text="Progress Report entry Completed"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" align="center">
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <table style="width: 100%" align="center">
                        <tr id="trcmb" runat="server" visible="false">
                            <td align="center">
                                <table id="pnlFilter" runat="server" cellpadding="0" cellspacing="1" width="100%">
                                    <tr runat="server" id="tdlblTeacher">
                                        <td class="ClsBorderlight" style="width: 16%">
                                                <span id="lblTeacher" class="ClsLabel" style="width:133px">Select Class Teacher :</span></td>
                                        <td style="padding-left: 3px">
                                            <asp:DropDownList ID="cmbTeachers" runat="server" CausesValidation="False" AutoPostBack="true"
                                                CssClass="ExLrgCombo" OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged"
                                                Width="260px">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeachers"
                                                Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                                ValueToCompare='0'></asp:CompareValidator>
                                            <span style="color: #ff0000" runat="server" id="spnMandatory">* &nbsp;&nbsp; </span>
                                        </td>
                                        <td colspan="2" style="padding-top: 2px">
                                        </td>
                                    </tr>
                                    <tr id="trTest" runat="server">
                                        <td valign="top" class="ClsBorderlight">
                                             <span id="lbltest" class="ClsLabel" >Select Exam :</span>
                                        </td>
                                        <td style="padding-left: 3px">
                                            <asp:DropDownList ID="cmbTests" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbTests_SelectedIndexChanged" CausesValidation="true">
                                                <asp:ListItem Text="-- Select --" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="cstValcmbTests" runat="server" CssClass="ClsLabel" Display="None"
                                                ErrorMessage="Exam should be selected." ControlToValidate="cmbTests" ValueToCompare="0"
                                                Operator="NotEqual"></asp:CompareValidator>
                                            <span style="color: #ff0000" runat="server" id="Span1">* &nbsp;&nbsp; </span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  colspan="4">
                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                <tr>
                                                    <td runat="server" id="tdPublish" style="width: 100px">
                                                        <asp:Button ID="btnPublish" runat="server" Enabled="false" CssClass="ClsBtnMid" Text="Publish"
                                                            OnClick="btnPublish_Click" UseSubmitBehavior="false"></asp:Button>
                                                    </td>
                                                    <td align="right" colspan="1" runat="Server" id="tdhlnkToppers">
                                                        <asp:Button ID="btnViewAll" runat="server" CausesValidation="false" CssClass="ClsBtnMid"
                                                            OnClick="btnViewAll_Click" Text="View Result All" UseSubmitBehavior="false" Visible="False" />
                                                        <asp:Button ID="btnPublishAll" runat="server" CssClass="BtnHLight" Enabled="false"
                                                            OnClick="btnPublishAll_Click" Text="Publish All" UseSubmitBehavior="false" Visible="False" />&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table cellpadding="0" cellspacing="1" runat="server" id="tblHeading" visible="False">
                                    <tr>
                                        <td class="ClsPaddingR">
                                                <span class="ClsLblLgnd">Class Teacher :</span> </td>
                                        <td class="ClsHilightBGB">
                                            <asp:Label ID="lblTeacherHeading" runat="server" EnableViewState="True"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table align="center">
                                    <tr runat="server" id="trTotalRec" align="center">
                                        <td colspan="6">
                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                <span class="LblNormal">To</span>
                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                <span class="LblNormal">Out Of </span>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                  <span class="LblNormal">Records </span>
                                        </td>
                                    </tr>
                                </table>
                                <div id="GridViewScrollContainer" style="overflow: auto; width: 100%">
                                    <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AllowPaging="True"
                                        PageSize="20" AutoGenerateColumns="False" 
                                        OnSorting="grdStudents_Sorting" AllowSorting="True" OnRowCreated="grdStudents_RowCreated"
                                        OnRowDataBound="grdStudents_RowDatabound" Width="100%" CellPadding="0" CellSpacing="1"
                                        ForeColor="#333333" DataKeyNames="Student_Id,Status" GridLines="None" OnPageIndexChanging="grdStudents_PageIndexChanging"
                                        EmptyDataText="There are no students in this class or there is no subject assignment to this teacher." OnDataBinding="grdStudents_DataBinding">
                                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                        </PagerStyle>
                                        <Columns>
                                            <asp:BoundField DataField="Roll_No" HeaderText="Roll No." SortExpression="Roll_No">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                    Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                    Wrap="False" />
                                            </asp:BoundField>
                                            <asp:HyperLinkField DataNavigateUrlFields="Student_Id" HeaderText="Generate" DataNavigateUrlFormatString="~/RITeSchool/Teacher/PrePrimaryProgressSheetEntry.aspx?StudentId={0}"
                                                Text="Generate">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:HyperLinkField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle CssClass="ClsGridRow" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                        <PagerTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="70%" align="left" class="ClsBorderPager" valign="middle">
                                                        <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                        <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
                                                            OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="30%" align="right" class="ClsBorderPager" valign="middle">
                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                        <PagerSettings PageButtonCount="1" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdODStudent"
                                        runat="server" SelectMethod="getPrePrimaryProgressSheetStudentList" SortParameterName="sortExpression"
                                        SelectCountMethod="CountPrePrimaryProgressSheetStudentList" EnableCaching="false"
                                        OnSelected="GrdODStudent_Selected">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter ControlID="hidStdDivId" PropertyName="Value" Name="aiStandardDivisionId" />
                                            <asp:SessionParameter Name="aiAcademicYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="string" />
                                            <asp:ControlParameter ControlID="cmbTests" PropertyName="SelectedValue" Name="aiTestId" />
                                            <asp:ControlParameter ControlID="hidIsMonthConfig" PropertyName ="Value" Name = "abIsMonthConfig" Type="Boolean" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </div>
                                <asp:HiddenField ID="hidBackUrl" runat="server" />
                            </td>
                        </tr>
                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="blue"
                                        Width="100%" Visible="False" CssClass="MainTitleHead" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RITeSchool/Admin/displayassignedclassteacherui.aspx"
                                        Visible="false">Class Teacher Assignment</asp:HyperLink>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr><td>
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidStdDivId" runat="server" />
                        <asp:HiddenField ID="hidIsReadOnly" runat="server" />
                        <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                        <asp:HiddenField ID="hidIsMonthConfig" runat="server" Value="False" />
                        </td></tr>
                    </table>
                </td>
            </tr>
        </table>
        <table align="center">
            <tr>
                <td align="center">
                    <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                        CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back" Visible="True" UseSubmitBehavior="false" /></td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
            _ClientbtnPublishAll = "<%=this.btnPublishAll.ClientID %>";            
            _ClientbtnPublish = "<%=this.btnPublish.ClientID %>";            
            _ClientbtnCancel = "<%=this.btnCancel.ClientID %>";
            _ClientcmbTeachers = "<%=this.cmbTeachers.ClientID %>";
        
        function ShowToppers(sQryStr)
        {
            if((document.getElementById(_sClienthlnkToppers)==null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false;                
            window.open(sQryStr , '_new','scrollbars=yes,resizable=no,top=0,left=0,width=900,height=600'); 
                return false;
        }   
          
        function ConfirmAction()
        {
            var bResult = false;
            if (window.confirm("Once you publish the result it will be visible to parents/students. Are you sure you want to continue?") )
            {
                DisableButtons()
                bResult= true;
            }
            else
              bResult =false;            
            return bResult;
        }
        
        function ShowGraceWarning()
        {
            var bResult = false;
            if (window.confirm("This action will overwrite the grace marks applied. Are you sure you want to continue?") )
            {
                DisableButtons()
                bResult= true;
            }
            else
               bResult =false;
           
            return bResult;
        }
        
    function DisableButtons()
    {
        if(document.getElementById(_ClientbtnPublishAll) != null)
            document.getElementById(_ClientbtnPublishAll).disabled=true;
        if(document.getElementById(_ClientbtnPublish) != null)
            document.getElementById(_ClientbtnPublish).disabled=true; 
        if(document.getElementById(_ClientbtnCancel) != null)
            document.getElementById(_ClientbtnCancel).disabled=true; 
        if(document.getElementById(_ClientcmbTeachers) != null)
            document.getElementById(_ClientcmbTeachers).disabled=true;
    }     
        
    </script>

</asp:Content>
