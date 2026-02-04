<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="StudentResultList.aspx.cs" Inherits="StudentResultList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv" style="width: 97%;">
        <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
            ID="uPnl">
            <ContentTemplate>
                <table width="100%" align="center">
                    <tr runat="server" id="trValidation">
                        <td align="center">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                ShowSummary="true" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" CssClass="ClsLabel">
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
                            <asp:Panel ID="pnlFilter" runat="server" Width="100%">
                                <table cellpadding="0" cellspacing="1" width="100%">
                                    <tr runat="server" id="tdlblTeacher">
                                        <td class="ClsBorderlight" style="width: 16%"> 
                                                <asp:Label ID="lblSelectClassTeacher" runat="server"  class="ClsLabel" Text="<%$ Resources:LocalizedResources, SelectClassTeacher%>"></asp:Label>                                           
                                                <span class="ClsLabel colonPadding">:</span></td>
                                        <td style="padding-left: 3px">
                                            <asp:DropDownList ID="cmbTeachers" runat="server" CausesValidation="false" AutoPostBack="true"
                                                CssClass="ExLrgCombo" OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged"
                                                Width="260px">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeachers"
                                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ClassTeacherShouldBeSelected%>" Operator="NotEqual"
                                                ValueToCompare='0'></asp:CompareValidator>
                                            
                                                <asp:Button ID="btnPublishAll" runat="server" CssClass="BtnHLight" Enabled="false"
                                                    OnClick="btnPublishAll_Click" Text="<%$ Resources:LocalizedResources, PublishAll%>" UseSubmitBehavior="false" />
                                         </td>
                                        <td colspan="2" style="padding-top: 2px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  colspan="4">
                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                <tr>
                                                    <td runat="server" id="tdbtnShow" style="width: 100px">
                                                        <asp:Button ID="btnShow" runat="server" Text="<%$ Resources:LocalizedResources, GenerateAll%>" CssClass="ClsBtnMid"
                                                            OnClick="btnShow_Click" Enabled="False" EnableTheming="True" UseSubmitBehavior="false" />
                                                    </td>                                                    
                                                    <td align="left" runat="server" id="tdbtnViewAll" style="width: 100px">
                                                        <asp:Button ID="btnViewAll" runat="server" Text="<%$ Resources:LocalizedResources, ViewResultAll%>" CausesValidation="false"
                                                            CssClass="ClsBtnMid" OnClick="btnViewAll_Click" UseSubmitBehavior="false" />
                                                    </td>
                                                    <td align="left" runat="server" id="tdPublish" style="width: 80px">
                                                        <asp:Button ID="btnPublish" runat="server" Enabled="false" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Publish%>"
                                                            OnClick="btnPublish_Click" UseSubmitBehavior="false"></asp:Button>
                                                    </td>
                                                    <td align="left" runat="server" id="tdUnPublish" style="width: 100px">
                                                        <asp:Button ID="btnUnPublish" runat="server" Enabled="false" 
                                                            CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Unpublish%>"
                                                            OnClick="btnUnPublish_Click" UseSubmitBehavior="false"></asp:Button>
                                                    </td>
                                                    <td align="right" colspan="1" runat="Server" id="tdhlnkToppers">
                                                        <%--<asp:HyperLink ID="hlnkToppers" CssClass="ToprLinkHlilightSubSort LblNrmlB " style="padding-bottom:3px" Enabled="False"
                                                            NavigateUrl="../Student/ExamToppersUI.aspx" Target="_blank" runat="server"
                                                            Text="Toppers"></asp:HyperLink>--%>
                                                             <%--<span ID="hlnkToppers" style="cursor:pointer;" runat="server" class="ToprLinkHlilight LblNrmlB ClsPaddingGen"><u>Toppers</u></span>--%>
                                                            <asp:Label ID="hlnkToppers" runat="server" style="cursor:pointer;" class="ToprLinkHlilight LblNrmlB ClsPaddingGen" Text="<%$ Resources:LocalizedResources, Toppers%>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="100%">
                        <tr>
                            <td>
                                <table  width="100%" class="LblNoRecord" style="margin-left:5px;" id="tblErrorMsg" runat="server">
                                    <tr >
                                        <td align="left" class="ClsConfigText">
                                            <asp:Label ID="lblErrorMsg" runat="server" Width="98%" Visible="False" EnableViewState="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsConfigLink">
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RITeSchool/Admin/displayassignedclassteacherui.aspx"
                                                Visible="false" Text="<%$ Resources:LocalizedResources, ClassTeacherAssignment%>"></asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td align="left">
                            <table align="center">
                                <tr runat="server" id="trTotalRec" align="center">
                                    <td colspan="6">
                                        <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />  
                                            <asp:Label ID="lblTo" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label>                                   
                                        <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                         <asp:Label ID="lblOutOf" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>"></asp:Label>                                        
                                        <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" /> 
                                        <asp:Label ID="lblRecords" runat="server" class="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>"></asp:Label>                                        
                                    </td>
                                </tr>
                            </table>
                            <div id="GridViewScrollContainer" style="overflow: auto; width: 100%">
                                <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AllowPaging="True"
                                    PageSize="20" AutoGenerateColumns="False" OnRowCommand="grdStudents_RowCommand"
                                    OnSorting="grdStudents_Sorting" AllowSorting="True" OnRowCreated="grdStudents_RowCreated"
                                    OnRowDataBound="grdStudents_RowDatabound" Width="100%" CellPadding="0" CellSpacing="1"
                                    ForeColor="#333333" DataKeyNames="Student_Id,Is_ResultGenrated" GridLines="None"
                                    OnPageIndexChanging="grdStudents_PageIndexChanging" EmptyDataText="<%$ Resources:LocalizedResources, ThereAreNoStudentsInThisClass%>">
                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                    </PagerStyle>
                                    <Columns>
                                        <asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources, RollNo%>" SortExpression="Roll_No">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                Width="90px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedResources, StudentName%>" SortExpression="Name">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Marks" HeaderText="<%$ Resources:LocalizedResources,Total%>" SortExpression="Marks" NullDisplayText="N/A">
                                            <ItemStyle CssClass="Clspadding" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Percentage" HeaderText="%" SortExpression="Percentage"
                                            NullDisplayText="N/A">
                                            <ItemStyle CssClass="Clspadding" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Grade_Name" HeaderText="<%$ Resources:LocalizedResources, Grade%>" SortExpression="Grade_Name"
                                            NullDisplayText="N/A">
                                            <ItemStyle CssClass="Clspadding" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" HeaderText="<%$ Resources:LocalizedResources, Result%>" SortExpression="Result" NullDisplayText="N/A">
                                            <ItemStyle CssClass="Clspadding" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:HyperLinkField DataNavigateUrlFields="Student_Id" HeaderText="<%$ Resources:LocalizedResources, Generate%>"
                                            DataNavigateUrlFormatString="~/RITeSchool/Teacher/StudentProgressSheetEdit.aspx?StudentId={0}&amp;Mode=Edit"
                                            Text="Generate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:HyperLinkField>
                                        <asp:HyperLinkField DataNavigateUrlFields="Student_Id" DataNavigateUrlFormatString="~/RITeSchool/Teacher/StudentProgressSheetEdit.aspx?StudentId={0}&amp;Mode=View"
                                            HeaderText="<%$ Resources:LocalizedResources, View%>" Text="View Final Result">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:HyperLinkField>
                                        <asp:HyperLinkField DataNavigateUrlFields="Student_Id" DataNavigateUrlFormatString="~/RITeSchool/Teacher/StudentAnnualResultEdit.aspx?StudentId={0}&amp;Mode=View"
                                            HeaderText="<%$ Resources:LocalizedResources, Grace%>" Text="Add Grace">
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
                                                    <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>" runat="server" CssClass="LblNrmlB" />
                                                    <span class="colonPadding"> : </span>
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
                                    runat="server" SelectMethod="GetStudentsResultList" SortParameterName="sortExpression"
                                    SelectCountMethod="CountStudentsResultList" EnableCaching="false" OnSelected="GrdODStudent_Selected">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />                                        
                                        <asp:ControlParameter ControlID="hidStdDivId" PropertyName="Value" Name="aiStandardDivisionId" />
                                        <asp:SessionParameter Name="aiAcademicYrId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                            Type="string" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </td>
                    </tr>                    
                </table>
                <asp:HiddenField ID="hidSortDirection" runat="server" />
                <asp:HiddenField ID="hidSortExpression" runat="server" />
                <asp:HiddenField ID="hidStdDivId" runat="server" />
                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                <asp:HiddenField ID="hidAbsentStudentCount" runat="server" Value="0" />
                <asp:HiddenField ID="hidUserID" runat="server" />
                <asp:HiddenField ID="hidConfirmSms" runat="server" />
                <asp:HiddenField ID="hidResultOfStudentsNotGeneratedOnceYouPublish" runat="server" />
                <asp:HiddenField ID="hidOnceYouPublishTheResultItWillBeVisible" runat="server"/>
                <asp:HiddenField ID="hidDoYouWantToSendMessageToTheStudents" runat="server" />
                 <asp:HiddenField ID="hidThisActionWillOverwriteTheGraceMarksApplied" runat="server" />
                 <asp:HiddenField ID="hidCultureInfo" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
            _ClientbtnPublishAll = "<%=this.btnPublishAll.ClientID %>";
            _ClientbtnShow = "<%=this.btnShow.ClientID %>";
            _ClientbtnPublish = "<%=this.btnPublish.ClientID %>";
            _ClientbtnViewAll = "<%=this.btnViewAll.ClientID %>";
            _ClientcmbTeachers = "<%=this.cmbTeachers.ClientID %>";
            _ClienthidAbsentStudentCount = "<%=this.hidAbsentStudentCount.ClientID %>";
            _clienthidConfirmSms = "<%=this.hidConfirmSms.ClientID %>"


        function ShowToppers(sQryStr)
        {
            _sClienthlnkToppers = "<%=this.hlnkToppers.ClientID %>";
            if((document.getElementById(_sClienthlnkToppers) == null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false;

            window.open(sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=600').focus(); 
                return false;
        }

        function ConfirmAction(IsMiniSite) {
        	var iStudentCount = document.getElementById(_ClienthidAbsentStudentCount).value;
        	
            var bResult = false;
            if (iStudentCount > 0) {
                if (iStudentCount > 0 && window.confirm(document.getElementById("<%=hidResultOfStudentsNotGeneratedOnceYouPublish.ClientID%>").value.replace("%IStudentCount%", iStudentCount)))
                 {
                    if (document.getElementById(_ClientbtnPublishAll) != null)
                        document.getElementById(_ClientbtnPublishAll).disabled = true;
                    if (document.getElementById(_ClientbtnShow) != null)
                        document.getElementById(_ClientbtnShow).disabled = true;
                    if (document.getElementById(_ClientbtnPublish) != null)
                        document.getElementById(_ClientbtnPublish).disabled = true;
                    if (document.getElementById(_ClientbtnViewAll) != null)
                        document.getElementById(_ClientbtnViewAll).disabled = true;
                    if (document.getElementById(_ClientcmbTeachers) != null)
                        document.getElementById(_ClientcmbTeachers).disabled = true;
                    bResult = true;
                }
                else {
                    bResult = false;
                }
                  }
            else {
                var bResult = false;
                if (window.confirm(document.getElementById("<%=hidOnceYouPublishTheResultItWillBeVisible.ClientID%>").value)) 
                    {                                
                        if(document.getElementById(_ClientbtnPublishAll) != null)
                            document.getElementById(_ClientbtnPublishAll).disabled=true;
                        if(document.getElementById(_ClientbtnShow) != null)
                            document.getElementById(_ClientbtnShow).disabled=true; 
                            document.getElementById(_ClientbtnPublish).disabled=true; 
                        if(document.getElementById(_ClientbtnViewAll) != null)
                            document.getElementById(_ClientbtnViewAll).disabled=true;
                        if(document.getElementById(_ClientcmbTeachers) != null)
                            document.getElementById(_ClientcmbTeachers).disabled=true;

                        bResult = true;                        
                        if (IsMiniSite == 'True') {
                            if (window.confirm(document.getElementById("<%=hidDoYouWantToSendMessageToTheStudents.ClientID%>").value))
                        		document.getElementById(_clienthidConfirmSms).value = 1;
                        	else
                        		document.getElementById(_clienthidConfirmSms).value = 0;
                        }
                    }
                    else
                        bResult =false;
                    
                    return bResult;
                }
                return bResult;
        }
        
        function ShowGraceWarning()
        {
            var bResult = false;
            if (window.confirm(document.getElementById("<%=hidThisActionWillOverwriteTheGraceMarksApplied.ClientID %>").value))
            { 
                if(document.getElementById(_ClientbtnPublishAll) != null)
                    document.getElementById(_ClientbtnPublishAll).disabled=true;
                if(document.getElementById(_ClientbtnShow) != null)
                    document.getElementById(_ClientbtnShow).disabled=true; 
                if(document.getElementById(_ClientbtnPublish) != null)
                    document.getElementById(_ClientbtnPublish).disabled=true; 
                if(document.getElementById(_ClientbtnViewAll) != null)
                    document.getElementById(_ClientbtnViewAll).disabled=true;
                if(document.getElementById(_ClientcmbTeachers) != null)
                    document.getElementById(_ClientcmbTeachers).disabled=true;
                bResult= true;
            }
            else
            {   bResult =false;
            }
            return bResult;
        }
        
        
    function DisableButtons()
    {
        if(document.getElementById(_ClientbtnPublishAll) != null)
            document.getElementById(_ClientbtnPublishAll).disabled=true;
        if(document.getElementById(_ClientbtnShow) != null)
            document.getElementById(_ClientbtnShow).disabled=true; 
        if(document.getElementById(_ClientbtnPublish) != null)
            document.getElementById(_ClientbtnPublish).disabled=true; 
        if(document.getElementById(_ClientbtnViewAll) != null)
            document.getElementById(_ClientbtnViewAll).disabled=true; 
        if(document.getElementById(_ClientcmbTeachers) != null)
            document.getElementById(_ClientcmbTeachers).disabled=true;
    }     
        
    </script>

</asp:Content>
