<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RegenarateRollNoUI.aspx.cs" Inherits="RegenarateRollNoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" runat="server" Visible="true"
            ID="UpdatePanel4">
            <ContentTemplate>
                <table style="background-color: white; width: 98%; height: 100%;" border="0" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td align="right">
                            <span class="ClsMdtStar" style="margin: 5px 0;">*
                                <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: white;" id="tblError" align="left" valign="top" runat="server">
                            <!-- Data Insert Here -->
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Always">
                                            <ContentTemplate>
                                                <table width="100%" id="tblMassage" runat="server" visible="true" style="color: Blue;
                                                    font-weight: bold;">
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                                                EnableViewState="false" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 17px">
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table style="margin: 5px 0 5px;">
                                <tr id="trReg" runat="server">
                                    <td align="center">
                                        <asp:RadioButton ID="rbtnRegenerate" Text="<%$ Resources:LocalizedResources, RegenerateRollNo %>"
                                            GroupName="Re" runat="server" Checked="True" CssClass="ClsLabel" onclick="ViewChange(false)" />
                                    </td>
                                    <td align="center">
                                        <asp:RadioButton ID="rbtnReassign" Text="<%$ Resources:LocalizedResources, ReassignRollNo %>"
                                            GroupName="Re" runat="server" CssClass="ClsLabel" onclick="ViewChange(true)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: white; height: 768px;" align="center" valign="top" class="td-vertical-align-top">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="uPnl">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="2" id="tblStdDivSelection">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel" id="lblStandard" style="width: 100px;">
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Standard %>"></asp:Label>
                                                                <span id="Span9" class="colonPadding">:</span> </span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbStandard" AutoPostBack="true" OnSelectedIndexChanged="cmbStd_SelectedIndexChanged"
                                                                runat="server" CssClass="MidCombo" Style="width: 120px;">
                                                            </asp:DropDownList>
                                                            <span id="stdMdtStar" runat="server" class="ClsMdtStar" style="visibility: hidden;">
                                                                * </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel" id="lblDivision" style="width: 100px;">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, Division %>"></asp:Label>
                                                                <span id="Span10" class="colonPadding">:</span> </span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbDivision" runat="server" CssClass="MidCombo" Style="width: 120px;">
                                                            </asp:DropDownList>
                                                            <span id="divMdtStar" runat="server" class="ClsMdtStar" style="visibility: hidden;">
                                                                * </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="bottom" colspan="2">
                                                            <asp:Button ID="btnShow" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Show %>"
                                                                OnClick="btnShow_Click" Width="100px" Style="margin-top: 8px;" />
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
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
                                    <ContentTemplate>
                                        <tr>
                                            <td align="left">
                                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                                <tr>
                                                                    <td align="center" class="MainTitleHead" style="height: 25px">
                                                                        <asp:Label ID="lblBuyer" runat="server" BorderWidth="0px" Text="<%$ Resources:LocalizedResources, CriteriaForRollNumberGeneration %>"
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
                                            <td>
                                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                    ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tbGrid" runat="server">
                                                            <tr>
                                                                <td align="left" colspan="6" class="ClsBorderlight" style="padding-left: 5px; width: 70%">
                                                                    <asp:Label ID="Label11" runat="server" BorderWidth="0px" Text="<%$ Resources:LocalizedResources, PleaseSelectAtleastOneFieldForGeneratingRollNumber %>"
                                                                        CssClass="LblSmlV" Font-Bold="True" EnableViewState="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span13" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldFirst" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderFirst" runat="server" GroupName="First" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="AscOrderFirst">
                                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderFirst" runat="server" GroupName="First" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="DescOrderFirst">
                                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span14" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldSecond" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderSecond" runat="server" GroupName="Second" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span1">
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderSecond" runat="server" GroupName="Second" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span5">
                                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span15" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldThird" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderThird" runat="server" GroupName="Third" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span2">
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderThird" runat="server" GroupName="Third" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span6">
                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span16" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldFourth" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderFourth" runat="server" GroupName="Fourth" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span3">
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderFourth" runat="server" GroupName="Fourth" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span7">
                                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span12" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldFifth" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderFifth" runat="server" GroupName="Fifth" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span4">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderFifth" runat="server" GroupName="Fifth" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span8">
                                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel">
                                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:LocalizedResources, Fields %>"></asp:Label>
                                                                        <span id="Span12" class="colonPadding">:</span></span>
                                                                </td>
                                                                <td align="left" colspan="1" width="30%">
                                                                    <asp:DropDownList ID="ddlFieldSix" runat="server" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optAscOrderSix" runat="server" GroupName="Six" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span5">
                                                                        <asp:Label ID="lblAscOrderSix" runat="server" Text="<%$ Resources:LocalizedResources, Ascending %>"></asp:Label></span>
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="5%">
                                                                    <asp:RadioButton ID="optDescOrderSix" runat="server" GroupName="Fifth" />
                                                                </td>
                                                                <td align="left" class="ClsBorderlight" colspan="1" width="20%">
                                                                    <span class="ClsLabel" id="Span9">
                                                                        <asp:Label ID="lblDescOrderSix" runat="server" Text="<%$ Resources:LocalizedResources, Descending %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="6">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="left" class="ClsBorderlight " style="width: 14%; background-color: #ffffc4;
                                                                                padding: 3px;">
                                                                                <span class="LblNrmlB" style="font-weight: bold">
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources,  Note %>"></asp:Label>
                                                                                    <span id="Span11" class="colonPadding">:</span></span>
                                                                            </td>
                                                                            <td align="left" class="ClsBorderlight" style="padding: 3px 5px;">
                                                                                <span class="LblSmlV">
                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources,  IfAnyExamResultIsPublishedThenUpdatedRollNumberWillNotNeDisplayedOnTheProgressReportOnScreen %>"></asp:Label></span>
                                                                                </span>
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
                                                                    <asp:Button ID="btnGenerate" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources,  RegenerateRollNo %>"
                                                                        Width="200px" OnClick="btnGenerate_Click" CausesValidation="false" />
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
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblStudent" runat="server">
                                                    <tr>
                                                        <td id="tdTotalRec" runat="server" align="center">
                                                            <asp:Label ID="lblStartIndex" runat="server" CssClass="LblNrmlB" />
                                                            <span class="LblNormal">
                                                                <asp:Label ID="lblTo" runat="server" Text="<%$ Resources:LocalizedResources, To %>"></asp:Label></span>
                                                            <asp:Label ID="lblEndIndex" runat="server" CssClass="LblNrmlB" />
                                                            <span class="LblNormal">
                                                                <asp:Label ID="lblOutOf" runat="server" Text="<%$ Resources:LocalizedResources, OutOf %>"></asp:Label></span>
                                                            <asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" />
                                                            <span class="LblNormal">
                                                                <asp:Label ID="lblRecords" runat="server" Text="<%$ Resources:LocalizedResources, Records %>"></asp:Label></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClspaddingT" align="center">
                                                            <asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" AllowPaging="True"
                                                                DataKeyNames="Roll_No,Is_Leave,Photo_file_Path,SchoolWise_Student_Id,SchoolLeft_Date, Standard_Id, Division_id,Joining_Date,YearWise_Student_Id"
                                                                AutoGenerateColumns="False" AllowSorting="True" OnRowCreated="grdStudents_RowCreated"
                                                                Width="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
                                                                GridLines="None" OnRowDataBound="grdStudents_RowDataBound" OnSorting="grdStudents_Sorting"
                                                                EmptyDataText="<%$ Resources:LocalizedResources, StudentsNotFound %>" >
                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                </PagerStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="Enrolment_Number" HeaderText="<%$ Resources:LocalizedResources, RegNo %>"
                                                                        SortExpression="Enrolment_Number">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="StandardDivision" HeaderText="<%$ Resources:LocalizedResources, Class %>"
                                                                        SortExpression="StandardDivision">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12%" CssClass="ClspaddingL" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" Width="12%"
                                                                            CssClass="ClspaddingL" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources, RollNo %>"
                                                                        SortExpression="Roll_No">
                                                                        <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingR" />
                                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingR" />
                                                                        <HeaderStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label runat="server" ID="lblNewRollNo" Text="<%$ Resources:LocalizedResources, NewRollNo %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtNewRollNo" runat="server" CssClass="ExSmlTxtBoxP" MaxLength="3"
                                                                                Width="35px" Text='<%#Eval("Roll_No")%>' />
                                                                            <asp:Label runat="server" ID="lblNewRoll_No" Text="" Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedResources, StudentName %>"
                                                                        SortExpression="First_Name">
                                                                        <ItemStyle Width="43%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                        <HeaderStyle Width="43%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                                            Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DOB" HeaderText="<%$ Resources:LocalizedResources, DateOfBirth %>"
                                                                        SortExpression="DOB">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Width="15%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False"
                                                                            Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Category_Name" HeaderText="<%$ Resources:LocalizedResources, Category %>"
                                                                        SortExpression="Category_Name">
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
                                                                    <table width="100%" cellspacing="0">
                                                                        <tr>
                                                                            <td width="70%" align="left" class="ClsBorderPager" valign="middle" style="padding: 5px;">
                                                                                <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectAPage %>"
                                                                                    runat="server" CssClass="LblNrmlB" />
                                                                                <asp:DropDownList ID="PageDropDownList" runat="server" AutoPostBack="true" CssClass="LblNormal"
                                                                                    OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td width="30%" align="right" class="ClsBorderPager" valign="middle" style="padding: 5px;">
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
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
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
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                        CssClass="ClsBtn" OnClick="btnSave_Click" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidStandardId" runat="server" />
                            <asp:HiddenField ID="hidDivisionId" runat="server" />
                            <asp:HiddenField ID="hidBlank" runat="server" Value="" />
                            <asp:HiddenField ID="hidDivisionName" runat="server" />
                            <asp:HiddenField ID="hidStandardName" runat="server" />
                            <asp:HiddenField ID="hidCanEdit" runat="server" Value="Y" />
                            <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                            <asp:HiddenField ID="hidPleaseSelectDifferentFieldsForGeneratingRollNumber" runat="server" />
                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                            <asp:HiddenField ID="hidStandardAndDivisionShouldBeSelected" runat="server" />
                            <asp:HiddenField ID="hidPleaseSelectAtleastOneFieldForGeneratingRollNumber" runat="server" />
                            <asp:HiddenField ID="hidAll" runat="server" />
                            <asp:HiddenField ID="hidItsAll" runat="server" />
                            <asp:HiddenField ID="hidDivisionsAreYouSureYouWantToContinue" runat="server" />
                            <asp:HiddenField ID="hidYouAreUpdatingRollNumbersOf" runat="server" />
                            <asp:HiddenField ID="hidStandardsAnd" runat="server" />
                            <asp:HiddenField ID="hidYouAreUpdatingRollNumbersOfStandard" runat="server" />
                            <asp:HiddenField ID="hidand" runat="server" />
                            <asp:HiddenField ID="hidAndDivision" runat="server" />
                            <asp:HiddenField ID="hidAreYouSureYouWantToContinue" runat="server" />
                            <asp:HiddenField ID="hidbtnShow" runat="server" />
                            <asp:HiddenField ID="HidDotInMarathi" runat="server" />
                            <asp:HiddenField ID="hidPageNo" runat="server" />
                            <asp:HiddenField ID="hidOf" runat="server" />
                            <asp:HiddenField ID="hidOutOflst" runat="server" />
                            <asp:HiddenField ID="hidValMsg" runat="server" />
                            <asp:HiddenField ID="hidValJsForRollNo" runat="server" />
                            <asp:HiddenField ID="hidValjsDuplicate" runat="server" />
                            <asp:HiddenField ID="hidMsgStandard" runat="server" />
                            <asp:CustomValidator ID="cstStdDivValidation" runat="server" ClientValidationFunction="ValidateStdDivSelection"
                                Display="None" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _clientddlFieldFirstId = "<%=this.ddlFieldFirst.ClientID %>";
        _clientddlFieldSecondId = "<%=this.ddlFieldSecond.ClientID %>";
        _clientddlFieldThirdId = "<%=this.ddlFieldThird.ClientID %>";
        _clientddlFieldFourthId = "<%=this.ddlFieldFourth.ClientID %>";
        _clientddlFieldFifthId = "<%=this.ddlFieldFifth.ClientID %>";
        _clientddlFieldSixthId = "<%=this.ddlFieldSix.ClientID %>";
        _clientlblErrorMsgId = "<%=this.lblErrorMsg.ClientID %>";
        _clientcmbStandardId = "<%=this.cmbStandard.ClientID %>";
        _clientcmbDivisionId = "<%=this.cmbDivision.ClientID %>";
        _clienthidDivisionName = "<%=this.hidDivisionName.ClientID %>";
        _clienthidStandardName = "<%=this.hidStandardName.ClientID %>";
        _clientrbtnReassign = '<%= this.rbtnReassign.ClientID %>';

        function DuplicateField() {
            var bIsValid = true;
            var arrIds = new Array(6);
            var i, j, k, cnt = 0;
            arrIds[0] = document.getElementById(_clientddlFieldFirstId).value;
            arrIds[1] = document.getElementById(_clientddlFieldSecondId).value;
            arrIds[2] = document.getElementById(_clientddlFieldThirdId).value;
            arrIds[3] = document.getElementById(_clientddlFieldFourthId).value;
            arrIds[4] = document.getElementById(_clientddlFieldFifthId).value;
            arrIds[5] = document.getElementById(_clientddlFieldSixthId).value;
            for (i = 0; i < arrIds.length; i++) {
                if (arrIds[i] != 0) {
                    for (j = i + 1; j < arrIds.length; j++) {
                        if (arrIds[i] == arrIds[j]) {
                            k = 0;
                            break;
                        }
                    }
                    if (k == 0) {
                        document.getElementById(_clientlblErrorMsgId).innerHTML = document.getElementById("<%=this.hidPleaseSelectDifferentFieldsForGeneratingRollNumber.ClientID %>").value;
                        bIsValid = false;
                        break;
                    }
                }
                else {
                    cnt++;
                }
            }
            if (cnt == 6) {
                document.getElementById(_clientlblErrorMsgId).innerHTML = document.getElementById("<%=this.hidPleaseSelectAtleastOneFieldForGeneratingRollNumber.ClientID %>").value;
                bIsValid = false;
            }
            else if (cnt != 6 && k != 0) {
                document.getElementById(_clientlblErrorMsgId).innerHTML = "";
                var Std;
                var Div;
                var Msg;
                if (document.getElementById(_clienthidStandardName).value == "-- All --") {
                    Std = document.getElementById("<%=this.hidAll.ClientID %>").value;
                    Div = document.getElementById("<%=this.hidItsAll.ClientID %>").value;
                    Msg = document.getElementById("<%=this.hidYouAreUpdatingRollNumbersOf.ClientID %>").value;
                                  

                }
                else if (document.getElementById(_clienthidDivisionName).value == "-- All --") {
                    Std = document.getElementById(_clienthidStandardName).value;
                    Div = document.getElementById("<%=this.hidItsAll.ClientID %>").value;
                    Msg = document.getElementById("<%=this.hidYouAreUpdatingRollNumbersOfStandard.ClientID %>").value.replace("%standard%", Std)
                }
                else {

                    Std = document.getElementById(_clienthidStandardName).value;
                    Div = document.getElementById(_clienthidDivisionName).value;
                    Msg = document.getElementById("<%=this.hidMsgStandard.ClientID %>").value + " " + Std + " " +
                        document.getElementById("<%=this.hidAndDivision.ClientID %>").value + " " + Div + document.getElementById("<%=this.HidDotInMarathi.ClientID %>").value + " " + document.getElementById("<%=this.hidAreYouSureYouWantToContinue.ClientID %>").value;
                }
                if (window.confirm(Msg)) {
                    bIsValid = true;
                }
                else {
                    bIsValid = false;
                }
            }
            return bIsValid;
        }


        function ValidatePage(SCheckBoxname, SLabelname, SRegLabelname, grdid, numSelects) {
            var lblmsg = '<%= lblMessage.ClientID %>';
            $get(lblmsg).style.visibility = "hidden";
            return ValidateRollNumbersInListViewWithCulture(SCheckBoxname, SLabelname, SRegLabelname, grdid, numSelects, document.getElementById("<%=this.hidValJsForRollNo.ClientID %>").value, document.getElementById("<%=this.hidValMsg.ClientID %>").value, document.getElementById("<%=this.hidValjsDuplicate.ClientID %>").value);
        }

        function ViewChange(isReassign) {
            var select = "<%= Utility.Constants.S_SELECT %>";
            var selectAll = "<%= Utility.Constants.S_SELECT_ALL %>";

            var cmbStd = $get(_clientcmbStandardId);
            var cmbDiv = $get(_clientcmbDivisionId);

            cmbStd.options[0].text = isReassign ? select : selectAll;

            if (cmbDiv.value == "0") {
                if (isReassign && cmbStd.selectedIndex != 0)
                    cmbDiv.remove(0);
                else
                    cmbDiv.options[0].text = isReassign ? select : selectAll;
            }
            else {
                if (isReassign) {
                    cmbDiv.remove(0);
                }
                else {
                    var option = document.createElement('option');
                    option.text = selectAll;
                    option.value = "0";
                    var oldOption = cmbDiv.options[cmbDiv.selectedIndex];
                    try {
                        cmbDiv.add(option, 0); // standards compliant; doesn't work in IE
                    }
                    catch (e) {
                        cmbDiv.add(option, 0); // IE only
                    }
                }
            }

            $('#tblStdDivSelection .ClsMdtStar').css("visibility", isReassign ? "visible" : "hidden");
        }

        ViewChange(false);

        function ValidateStdDivSelection(src, args) {
            if ($get(_clientrbtnReassign) && $get(_clientrbtnReassign).checked) {
                if ($get(_clientcmbStandardId).value == "0" || $get(_clientcmbDivisionId).value == "0")
                    args.IsValid = false;
            }

            if (!args.IsValid)
                alert(document.getElementById("<%=this.hidStandardAndDivisionShouldBeSelected.ClientID %>").value);

            return !args.IsValid;
        }
    </script>
</asp:Content>
