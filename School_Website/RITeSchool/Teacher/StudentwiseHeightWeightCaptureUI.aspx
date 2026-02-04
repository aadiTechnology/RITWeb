<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentwiseHeightWeightCaptureUI.aspx.cs" Inherits="StudentwiseHeightWeightCaptureUI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">

    <div class="MainBodyDiv">
        <table width="98%" align="center">
            <tr id="trMandetory" runat="server">
                <td align="right" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">* Mandatory Fields </span>
                </td>
            </tr>
            <tr id="tr1" runat="server">
                <td align="center" valign="top">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Always">
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
                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                        Width="100%" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="NewClsLabel" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" align="center">
                  
                        <tr id="trFilters" runat="server">
                            <td width="100%">
                                <table align="center" cellpadding="1" cellspacing="1" width="100%">
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="1" border="0">
                                                <tr>
                                                    <td class="ClsBorderlight" id="tdTeacher" runat="server">
                                                        <asp:Label ID="lblTeacher" runat="server" CssClass="ClsLabel" Text="Class Teacher :"
                                                            EnableViewState="False" Width="125px"></asp:Label>
                                                    </td>
                                                    <td id="tdTeacherList" runat="server" style="width: 230px">
                                                        <asp:DropDownList ID="cmbTeachers" AutoPostBack="true" runat="server" CssClass="LrgCombo"
                                                            OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">&nbsp;*</span>
                                                        <asp:CompareValidator ID="cmp_Name" runat="server" ControlToValidate="cmbTeachers"
                                                            Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                                            ValueToCompare='0' ValidationGroup="SaveRemark"></asp:CompareValidator>
                                                    </td>
                                                    <td class="ClsBorderlight">
                                                        <asp:Label ID="lblStudentName" runat="server" CssClass="ClsLabel" Text="Term :" EnableViewState="False"
                                                            Width="125px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbTermName" runat="server" CssClass="LrgCombo" AutoPostBack="true"
                                                            OnSelectedIndexChanged="cmbTermName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                  
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                                <tr runat="server" id="trdtPagetDtPgCnt" align="center">
                                                    <td valign="top">
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="updatePanelLib" ChildrenAsTriggers="false">
                                                            <ContentTemplate>
                                                                <table cellpadding="0" cellspacing="2" align="center" width="80%">
                                                                     <tr>
                                                                        <td align="center">
                                                                            <table width="845px">
                                                                                <tr align="left">
                                                                                    <td style="width:5%">
                                                                                        <span class="ClsLblLgnd">
                                                                                            <asp:Label runat="server" ID="Label3" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label>
                                                                                        </span>
                                                                                    </td>
                                                                                    <td style="width:3%">
                                                                                        <span style="background-color: Red; height: 20px; border: 1px solid black; width: 20px;">
                                                                                            <img src="../images/spacer.gif" width="20px" height="10px" />
                                                                                        </span>
                                                                                    </td>
                                                                                    <td class="ClsTextNormal" style="font-weight: bold">
                                                                                        <asp:Label runat="server" ID="Label4" Text="<%$ Resources:LocalizedResources, LeftStudents %>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                    <tr id="trListView" runat="server">
                                                                        <td align="center" id="tdMainListView" runat="server">
                                                                            <asp:ListView ID="lstvwStudentDetails" ItemPlaceholderID="ContactRowContainer" runat="server"
                                                                                DataKeyNames="RollNo,YearWiseStudentId" 
                                                                                onitemdatabound="lstvwStudentDetails_ItemDataBound">
                                                                                <LayoutTemplate>
                                                                                    <table style="width: 845px; height: 100%; color: #333333" runat="server" id="tblContacts"
                                                                                        class="GridBorder" cellpadding="0" cellspacing="1">
                                                                                        <tr class="ClsGridHeader">
                                                                                            <th align="left" class="ClspaddingL" width="80px">
                                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, RollNo %>"></asp:Label>
                                                                                            </th>
                                                                                            <th align="left" class="ClspaddingL" width="450px">
                                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, StudentName %>"></asp:Label>
                                                                                            </th>
                                                                                            <th align="left" class="ClspaddingL" width="180px">
                                                                                                <asp:Label ID="Label5" runat="server" Text="Height (In Centimeters)"></asp:Label>
                                                                                            </th>
                                                                                            <th align="left" class="ClspaddingL" width="165px">
                                                                                                <asp:Label ID="Label1" runat="server" Text="Weight (In Kilograms)"></asp:Label>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr runat="server" id="ContactRowContainer" />
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="trStudentRow" runat="server" class="ClsGridRow">
                                                                                        <td class="ClspaddingL">
                                                                                            <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="left" class="ClspaddingL">
                                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="right" class="ClspaddingL">
                                                                                            <asp:TextBox ID="txtHeight" runat="server" MaxLength="5" Text='<%#Eval("Height")%>'
                                                                                                CssClass="MidTxtBox" Style="width: 95%; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="right" class="ClspaddingL">
                                                                                            <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("Weight")%>' MaxLength="5"
                                                                                                CssClass="MidTxtBox" Style="width: 95%; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="trStudentRow" runat="server" class="ClsGridAltRow">
                                                                                        <td class="ClspaddingL">
                                                                                            <asp:Label ID="lblRollNo" runat="server" Text='<%#Eval("RollNo")%>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="left" class="ClspaddingL">
                                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("StudentName")%>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="right" class="ClspaddingL">
                                                                                            <asp:TextBox ID="txtHeight" runat="server" MaxLength="5" Text='<%#Eval("Height")%>'
                                                                                                CssClass="MidTxtBox" Style="width: 95%; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="right" class="ClspaddingL">
                                                                                            <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("Weight")%>' MaxLength="5"
                                                                                                CssClass="MidTxtBox" Style="width: 95%; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                                                                                ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                           
                                                                            <asp:HiddenField ID="hidcmbTeacherValue" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                       <tr id="trNote" runat="server">
                                                                            <td align="center">
                                                                                <table id="tblNote" runat="server" align="center" style="width: 70%">
                                                                                    <tr>
                                                                                        <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                                                                CssClass="LblNrmlB"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 60%">
                                                                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="User can not change or update any data once summative exam is published."></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                         
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" ValidationGroup="SaveRemark"
                                                                                Visible="false" OnClick="btnSave_Click" />
                                                                            <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="Back" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="cmbTermName" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

//        function ResetValue(txt) {
//            
//        }
    </script>
</asp:Content>
