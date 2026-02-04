<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="../MasterPages/PopupMaster.master"
    CodeFile="ExamToppersUI.aspx.cs" Inherits="ExamToppersUI" %>
<%@ Import Namespace="Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <style type="text/css">
        td[valign="top"] {
            vertical-align: top;
        }
    </style>
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <!-- Data Insert Here -->
                <table border="0" align="center" cellpadding="0" cellspacing="2" style="width: 95%;">
                    <tr>
                        <td align="left" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="ClsGrayMainTitle" width="98%" height="20px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; height: 15px">
                                            <tr>
                                                <td align="center">
                                                    <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" ID="UpdatePanel3"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblToppers" runat="server" BorderWidth="0px" CssClass="MainTitleHead"
                                                                EnableViewState="False">Toppers</asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" valign="top" height="10%">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" valign="top">
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" ID="UpdatePanel2"
                                            runat="server">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="1" style="height: 100%" class="ClsBorderlight">
                                                    <tr>
                                                        <td align="center" colspan="6" class="ClsBorderlight">
                                                            <asp:RadioButton ID="rbtnClassToppers" runat="server" AutoPostBack="True" Checked="True"
                                                                GroupName="Toppers" OnCheckedChanged="rbtnClassToppers_CheckedChanged" Text="Class Toppers" />
                                                            <asp:RadioButton ID="rbtnStdToppers" runat="server" AutoPostBack="True" GroupName="Toppers"
                                                                OnCheckedChanged="rbtnStdToppers_CheckedChanged" Text="Standard Toppers" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="1" runat="server" id="tdlblStdDiv">
                                                            <span id="lblStd" class="ClsLabel" style="width: 100px">Standard :</span>
                                                        </td>
                                                        <td id="tdcmbStandard" runat="server" align="left" colspan="1">
                                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="SmlCombo" CausesValidation="true"
                                                                AutoPostBack="True" OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="cmp_StandardDivision" runat="server" ControlToValidate="cmbStandard"
                                                                Display="None" ErrorMessage="Standard should be selected." Operator="NotEqual"
                                                                ValidationGroup="valGrpAddEduDetails" ValueToCompare="0"></asp:CompareValidator>
                                                        </td>
                                                        <td id="tdClass" runat="server" align="left" class="ClsBorderlight" colspan="1">
                                                            <span id="lblStdDiv" class="ClsLabel" style="width: 70px">Class :</span>
                                                        </td>
                                                        <td align="left" colspan="1" id="tdcmbClass" runat="server">
                                                            <asp:DropDownList ID="cmbStandardDivision" runat="server" AutoPostBack="True" CausesValidation="true"
                                                                CssClass="SmlCombo" OnSelectedIndexChanged="cmbStandardDivision_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbStandardDivision"
                                                                Display="None" ErrorMessage="Class should be selected." Operator="NotEqual" ValidationGroup="valGrpAddEduDetails"
                                                                ValueToCompare="0">
                                                            </asp:CompareValidator>
                                                        </td>
                                                        <td id="tdExam" runat="server" align="left" class="ClsBorderlight" colspan="1">
                                                            <span id="lblExam" class="ClsLabel" style="width: 100px">Select Exam :</span>
                                                        </td>
                                                        <td align="left" colspan="1" runat="server" id="tdcmbTest">
                                                            <asp:DropDownList ID="cmbTests" runat="server" AutoPostBack="True" CausesValidation="true"
                                                                CssClass="LrgCombo" OnSelectedIndexChanged="cmbTests_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="cmbTestComparator" runat="server" ControlToValidate="cmbTests"
                                                                Display="None" ErrorMessage="Exam should be selected." Operator="NotEqual" ValidationGroup="valGrpAddEduDetails"
                                                                ValueToCompare="0">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="always" ID="UpdatePanel1"
                                            runat="server">
                                            <ContentTemplate>
                                                <table runat="server" id="tblMsg" enableviewstate="false" visible="false" cellpadding="0"
                                                    cellspacing="0" align="center">
                                                    <tr>
                                                        <td style="height: 30px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 2px;" class="LblNoRecord" align="center">
                                                            <asp:Label ID="lblErrorsMsg" Style="text-align: left" runat="server" EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="always" ID="uPnl" runat="server">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                                    <tr>
                                                        <td align="center" valign="top">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center" class="ToprTblTitle" runat="server" id="trTestTitle">
                                                            <asp:Label ID="lblTestName" runat="server" CssClass="ClsTextLrg" ForeColor="black"
                                                                EnableViewState="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <asp:GridView CssClass="GridBorder" ID="grdTestToppers" runat="server" Width="100%"
                                                                Height="90%" AutoGenerateColumns="False" PageSize="20" CellPadding="0" CellSpacing="1"
                                                                ForeColor="#333333" GridLines="None" EnableViewState="False">
                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                </PagerStyle>
                                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                <Columns>
                                                                    <asp:ImageField DataImageUrlField="Rank_Image" HeaderText="Rank" SortExpression="TopperRank">
                                                                        <HeaderStyle Width="25px" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:ImageField>
                                                                    <asp:BoundField DataField="Standard" HeaderText="Class">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle Width="100px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Roll_No" HeaderText="Roll No.">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle Width="120px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Student_Name" HeaderText="Student Name" SortExpression="Student_Name">
                                                                        <HeaderStyle Width="60%" />
                                                                        <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Marks">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMarks" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Marks_Scored").ToDecimal() % 1 == 0? DataBinder.Eval(Container.DataItem,"Marks_Scored").ToInt() : DataBinder.Eval(Container.DataItem,"Marks_Scored")) + " /" + DataBinder.Eval(Container.DataItem,"Total_Marks")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" CssClass="Clspadding" Wrap="False" />
                                                                        <HeaderStyle Width="140px" />
                                                                    </asp:TemplateField>
                                                                    <%--<asp:BoundField DataField="Marks" HeaderText="Marks" SortExpression="Marks">
                                                                        <ItemStyle HorizontalAlign="Center" CssClass="Clspadding" Wrap="False" />
                                                                        <HeaderStyle Width="140px" />
                                                                    </asp:BoundField>--%>
                                                                </Columns>
                                                                <RowStyle CssClass="ToprGridRow" />
                                                                <HeaderStyle CssClass="ToprTotalHead" />
                                                                <AlternatingRowStyle CssClass="ToprGridRow" />
                                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="height: 15px">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trSubTitle">
                                                        <td align="left" class="ClsBorderlight ">
                                                            <span class="ClsLblLgnd" id="titalSubject">Subject Toppers</span>
                                                        </td>
                                                    </tr>
                                                    <tr visible="false" enableviewstate="false" id="Tr1">
                                                        <td align="center">
                                                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:Label ID="lblSubjectName" runat="server" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:GridView ID="grdSubjectTopper" runat="server" Width="100%" Height="100%" AutoGenerateColumns="False"
                                                                            DataKeyNames="TopperRank" EnableViewState="false" PageSize="20" CellPadding="0"
                                                                            CellSpacing="1" ForeColor="#333333" GridLines="None">
                                                                            <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                                FirstPageText="First" Position="TopAndBottom"></PagerSettings>
                                                                            <Columns>
                                                                                <asp:ImageField HeaderText="Rank" DataImageUrlField="Rank_Image" SortExpression="TopperRank">
                                                                                    <HeaderStyle Width="50px" />
                                                                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                                                                                </asp:ImageField>
                                                                                <asp:BoundField DataField="Standard" HeaderText="Class">
                                                                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                                                                                    <HeaderStyle Width="70px" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Roll_No" HeaderText="Roll No.">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="55px" Wrap="False" />
                                                                                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Student_Name" HeaderText="Student Name" SortExpression="Student_Name">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="230px" Wrap="False" />
                                                                                    <ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" Font-Bold="true" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Marks">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMarks" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Total_Marks_Scored").ToDecimal() % 1 == 0? DataBinder.Eval(Container.DataItem,"Total_Marks_Scored").ToInt() : DataBinder.Eval(Container.DataItem,"Total_Marks_Scored")) + " /" + DataBinder.Eval(Container.DataItem,"Subject_Total_Marks") %>'></asp:Label>
                                                                                     </ItemTemplate>
                                                                                     <HeaderStyle HorizontalAlign="Center" Width="70px" Wrap="False" />
                                                                                    <ItemStyle HorizontalAlign="Center" CssClass="Clspadding" Font-Bold= "true"/>
                                                                                </asp:TemplateField>
                                                                                <%--<asp:BoundField DataField="Marks" HeaderText="Marks" SortExpression="Marks">
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="70px" Wrap="False" />
                                                                                    <ItemStyle HorizontalAlign="right" CssClass="Clspadding" Font-Bold= "true"/>
                                                                                </asp:BoundField>--%>
                                                                            </Columns>
                                                                            <RowStyle CssClass="ToprMarkGrdAltRow" />
                                                                            <HeaderStyle CssClass="ToprTestHeader" ForeColor="black" />
                                                                            <AlternatingRowStyle CssClass="ToprMarkGrdRow" />
                                                                            <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table id="tblGrid" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                                                height: 100%;">
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="hidStdDivId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="img_Back" Text="Close" CssClass="ClsBtnSml" BorderStyle="Solid" runat="server"
                                            BorderWidth="1px" CausesValidation="true" />
                                    </td>
                                </tr>
                            </table>
                            <!-- Data Insert End Here -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script language="javascript">
        function closewindow() {
            window.close()
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
    </script>
</asp:Content>
