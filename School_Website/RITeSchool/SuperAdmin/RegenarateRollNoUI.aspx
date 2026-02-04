<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="RegenarateRollNoUI.aspx.cs" Inherits="RegenarateRollNoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
            ID="UpdatePanel4">
            <ContentTemplate>
                <table style="background-color: white; width: 98%; height: 100%;" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td style="background-color: white;" id="tblError" align="left" valign="top" runat="server">
                            <!-- Data Insert Here -->
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                            <asp:Label ID="lblErrorMsg"  runat="server" CssClass="LblErrorMsg"></asp:Label>
                                            <%--<asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="Dup" />--%>
                                            <%--<asp:CustomValidator ID="Duplicate" runat="server" Display="none" EnableClientScript="true"
                                                ClientValidationFunction="DuplicateField" ErrorMessage="Error message" ValidationGroup="Dup"></asp:CustomValidator>--%></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="Table1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <asp:Label ID="Label1" runat="server" BorderWidth="0px" Text="Searching Criteria"
                                                                    Font-Bold="True" EnableViewState="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" id="tblSearch" runat="server">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="uPnl">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="2" width="100%">
                                                    <tr id="trCombo">
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="lblStandard" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Standard : "></asp:Label>--%>
                                                                <span class="ClsLabel" id="lblStandard">Standard :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="cmbStandard" AutoPostBack="true" OnSelectedIndexChanged="cmbStd_SelectedIndexChanged"
                                                                runat="server" CssClass="LrgCombo" TabIndex="1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="lblDivision" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Division : "></asp:Label>--%>
                                                                <span class="ClsLabel" id="lblDivision">Division :</span>
                                                        </td>
                                                        <td width="30%">
                                                            <asp:DropDownList ID="cmbDivision" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="2">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="bottom" colspan="4">
                                                            &nbsp;<asp:Button ID="btnShow" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                                                Text="Show" OnClick="btnShow_Click" Width="100px" TabIndex="3" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel3">
                                            <ContentTemplate>
                                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                                <tr>
                                                                    <td align="center" class="MainTitleHead" style="height: 20px">
                                                                        <asp:Label ID="lblBuyer" runat="server" BorderWidth="0px" Text="Criteria For Roll Number Generation"
                                                                            Font-Bold="True" EnableViewState="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tbGrid" runat="server">
                                                    <tr>
                                                        <td align="left" colspan="6" class="ClsBorderlight" style="padding-left: 5px; width: 70%">
                                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" Text="Select atleast one field for generating roll number."
                                                                CssClass="LblSmlV" Font-Bold="True" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="lblField" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Fields : "></asp:Label>--%>
                                                                <span class="ClsLabel">Fields :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="ddlFieldFirst" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="4">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optAscOrderFirst" runat="server" GroupName="First" 
                                                                TabIndex="5" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="AscOrderFirst" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Ascending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="AscOrderFirst">Ascending</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optDescOrderFirst" runat="server" GroupName="First" 
                                                                TabIndex="6" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="DescOrderFirst" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Descending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="DescOrderFirst">Descending</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                           <%-- <asp:Label ID="lblFieldTwo" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Fields : "></asp:Label>--%>
                                                                <span class="ClsLabel">Fields :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="ddlFieldSecond" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="7">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optAscOrderSecond" runat="server" GroupName="Second" 
                                                                TabIndex="8" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label3" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Ascending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span1">Ascending</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optDescOrderSecond" runat="server" GroupName="Second" 
                                                                TabIndex="9" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label4" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Descending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span5">Descending</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label5" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Fields : "></asp:Label>--%>
                                                                <span class="ClsLabel">Fields :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="ddlFieldThird" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="10">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optAscOrderThird" runat="server" GroupName="Third" 
                                                                TabIndex="11" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label6" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Ascending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span2">Ascending</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optDescOrderThird" runat="server" GroupName="Third" 
                                                                TabIndex="12" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label7" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Descending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span6">Descending</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label8" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Fields : "></asp:Label>--%>
                                                                <span class="ClsLabel">Fields :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="ddlFieldFourth" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="13">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optAscOrderFourth" runat="server" GroupName="Fourth" 
                                                                TabIndex="14" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label9" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Ascending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span3">Ascending</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optDescOrderFourth" runat="server" GroupName="Fourth" 
                                                                TabIndex="15" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label10" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Descending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span7">Descending</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label12" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Fields : "></asp:Label>--%>
                                                                <span class="ClsLabel">Fields :</span>
                                                        </td>
                                                        <td align="left" colspan="1" width="30%">
                                                            <asp:DropDownList ID="ddlFieldFifth" runat="server" CssClass="LrgCombo" 
                                                                TabIndex="13">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optAscOrderFifth" runat="server" GroupName="Fifth" 
                                                                TabIndex="14" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label13" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Ascending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span4">Ascending</span>
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                            <asp:RadioButton ID="optDescOrderFifth" runat="server" GroupName="Fifth" 
                                                                TabIndex="15" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                            <%--<asp:Label ID="Label14" runat="server" CssClass="ClsLabel" EnableViewState="false"
                                                                Text="Descending"></asp:Label>--%>
                                                                <span class="ClsLabel" id="Span8">Descending</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                     <td colspan="4">
                                                      <table>
                                                       <tr>
                                                        <td align="left" colspan="1" class="ClsBorderlight " 
                                                               style="width: 14%; background-color: #ffffc4;">
                                                            <%--<asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                                CssClass="LblNrmlB" EnableViewState="false"></asp:Label>--%>
                                                                <span class="LblNrmlB" style="font-weight:bold">Note :</span>
                                                        </td>
                                                        <td align="left" colspan="6" class="ClsBorderlight" style="padding-left: 5px;">
                                                            <asp:Label ID="Label2" runat="server" BorderWidth="0px" Text="If at least one exam result is published then updated roll number will not be displayed."
                                                                CssClass="LblSmlV" EnableViewState="False" Width="500px"></asp:Label>
                                                                <%--<span class="LblSmlV" >If at least one exam result is published then updated roll number will not be displayed.</span>--%>
                                                        </td>
                                                        </tr>
                                                       </table>
                                                     </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6" align="center">
                                                            <asp:Button ID="btnGenerate" runat="server" CssClass="ClsBtn" Text="Regenerate Roll No."
                                                                Width="200px" OnClick="btnGenerate_Click" CausesValidation="false" 
                                                                TabIndex="16" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblStudent" runat="server">
                                                    <tr id="trTotal" runat="server">
                                                        <td id="tdTotalRec" runat="server" align="center" style="padding-left: 60px">
                                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                            <%--<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />--%>
                                                            <span class="LblNormal">To</span>
                                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                            <%--<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />--%>
                                                            <span class="LblNormal">Out Of</span>
                                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                            <%--<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />--%>
                                                             <span class="LblNormal">Records</span>
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1" runat="server" align="center">
                                                        <td class="ClspaddingT" align="center">
                                                            <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AllowPaging="True"
                                                                DataKeyNames="Roll_No,Is_Leave,Photo_file_Path,SchoolWise_Student_Id,SchoolLeft_Date, Standard_Id, Division_id,Joining_Date"
                                                                AutoGenerateColumns="False" AllowSorting="True" OnRowCreated="grdStudents_RowCreated"
                                                                Width="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                GridLines="None" OnRowDataBound="grdStudents_RowDataBound"
                                                                OnSorting="grdStudents_Sorting" EmptyDataText="Students not found." 
                                                                TabIndex="17">
                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                </PagerStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="Enrolment_Number" HeaderText="Reg. No." SortExpression="Enrolment_Number">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="10%"/>
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" Width="10%" />
                                                                    </asp:BoundField>
                                                                     <asp:BoundField DataField="StandardDivision" HeaderText="Class" SortExpression="StandardDivision">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12%" CssClass="ClspaddingL" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="12%" CssClass="ClspaddingL" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Roll_No" HeaderText="Roll No." SortExpression="Roll_No">
                                                                        <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingR" />
                                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>                                                                   
                                                                    <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="First_Name">
                                                                        <ItemStyle Width="43%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        <HeaderStyle Width="43%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DOB" HeaderText="DOB" SortExpression="DOB">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Width="15%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Category_Name" HeaderText="Category" SortExpression="Category_Name">
                                                                        <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        <HeaderStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
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
                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </PagerTemplate>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnGenerate" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
                                            runat="server" SelectMethod="GetAllStudents" SortParameterName="sortExpression"
                                            SelectCountMethod="CountRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                    Type="string" />
                                                <asp:ControlParameter ControlID="cmbStandard" PropertyName="SelectedValue" Name="aiStandardId" />
                                                <asp:ControlParameter ControlID="cmbDivision" PropertyName="SelectedValue" Name="aiDivisionId" />
                                                <asp:ControlParameter ControlID="hidBlank" PropertyName="Value" Name="asName" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidStandardId" runat="server" />
                                        <asp:HiddenField ID="hidDivisionId" runat="server" />
                                        <asp:HiddenField ID="hidBlank" runat="server" Value="" />
                                        <asp:HiddenField ID="hidDivisionName" runat="server" />
                                        <asp:HiddenField ID="hidStandardName" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="bottom">
                                        &nbsp;<asp:Button ID="btnBack" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                            Text="Back" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" 
                                            TabIndex="18" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientddlFieldFirstId = "<%=this.ddlFieldFirst.ClientID %>"
        _clientddlFieldSecondId = "<%=this.ddlFieldSecond.ClientID %>"
        _clientddlFieldThirdId = "<%=this.ddlFieldThird.ClientID %>"
        _clientddlFieldFourthId = "<%=this.ddlFieldFourth.ClientID %>"
        _clientddlFieldFifthId = "<%=this.ddlFieldFifth.ClientID %>"
        _clientlblErrorMsgId = "<%=this.lblErrorMsg.ClientID %>"
        _clientcmbStandardId = "<%=this.cmbStandard.ClientID %>"
        _clientcmbDivisionId = "<%=this.cmbDivision.ClientID %>"
        _clienthidDivisionName = "<%=this.hidDivisionName.ClientID %>"
        _clienthidStandardName = "<%=this.hidStandardName.ClientID %>"
        function DuplicateField() {
            var bIsValid = true
            var arrIds = new Array(5)
            var i, j, k, cnt = 0
            arrIds[0] = document.getElementById(_clientddlFieldFirstId).value
            arrIds[1] = document.getElementById(_clientddlFieldSecondId).value
            arrIds[2] = document.getElementById(_clientddlFieldThirdId).value
            arrIds[3] = document.getElementById(_clientddlFieldFourthId).value
            arrIds[4] = document.getElementById(_clientddlFieldFifthId).value
            for (i = 0; i < arrIds.length; i++) {
                if (arrIds[i] != 0) {
                    for (j = i + 1; j < arrIds.length; j++) {
                        if (arrIds[i] == arrIds[j]) {
                            k = 0
                            break
                        } 
                    }
                    if (k == 0) {
                        document.getElementById(_clientlblErrorMsgId).innerHTML = "Please select different fields for generating roll number."
                        bIsValid = false
                        break
                    } 
                }
                else {
                    cnt++
                } 
            }
            if (cnt == 5) {
                document.getElementById(_clientlblErrorMsgId).innerHTML = "Please select atleast one field for generating roll number."
                bIsValid = false
            }
            else if (cnt != 5 && k != 0) {
                document.getElementById(_clientlblErrorMsgId).innerHTML = ""
                var Std
                var Div
                var Msg
                if (document.getElementById(_clienthidStandardName).value == "-- All --") {
                    Std = "all"
                    Div = "its all"
                    Msg = "You are updating roll numbers of " + Std + " standards and " + Div + " divisions. Are you sure you want to continue?"
                }
                else if (document.getElementById(_clienthidDivisionName).value == "-- All --") {
                    Std = document.getElementById(_clienthidStandardName).value
                    Div = "its all"
                    Msg = "You are updating roll numbers of standard : " + Std + " and " + Div + " divisions. Are you sure you want to continue?"
                }
                else {
                    Std = document.getElementById(_clienthidStandardName).value
                    Div = document.getElementById(_clienthidDivisionName).value
                    Msg = "You are updating roll numbers of standard : " + Std + " and division : " + Div + ". Are you sure you want to continue?"
                }
                if (window.confirm(Msg)) {
                    bIsValid = true
                }
                else {
                    bIsValid = false
                } 
            }
            return bIsValid
        }
    </script>
</asp:Content>
