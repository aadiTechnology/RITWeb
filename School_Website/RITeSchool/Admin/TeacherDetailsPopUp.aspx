<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/PopupMaster.master"
    CodeFile="TeacherDetailsPopUp.aspx.cs" Inherits="TeacherDetailsPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div style="width: 100%; overflow: auto">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 95%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" style="width: 100%;">
                        <tr>
                            <td align="center" colspan="4">
                                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                    <tr>
                                        <td align="left" colspan="6" style="padding-bottom: 5px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                                <tr>
                                                    <td class="ClsGrayMainTitle" style="height: 20px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                                                            <tr>
                                                                <td align="left" style="width: 90%">
                                                                    <span class="MainTitleHead" style="font-weight: bold">
                                                                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:LocalizedResources, TeacherPersonalDetails %>"></asp:Label></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblTeacherNameText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="5" class="ClsBorderlight">
                                            <asp:Label ID="lblTeacherName" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" style="width: 20%" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="lblDesignationText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Designation %>">
                                                </asp:Label><span class="colonPadding"> :</span> </span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="width: 5%">
                                        </td>
                                        <td align="left" colspan="1" style="width: 20%" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label4" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ServiceType %>">
                                                </asp:Label><span class="colonPadding"> :</span> </span>
                                        </td>
                                        <td align="left" colspan="2" style="width: 25%" class="ClsBorderlight">
                                            <asp:Label ID="lblServiceType" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label5" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Nationality %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblNationality" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label6" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Religion %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                        </td>
                                        <td align="left" colspan="2" class="ClsBorderlight">
                                            <asp:Label ID="lblReligion" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label8" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Category %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblCategory" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label9" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, CasteAndSubCaste %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                        </td>
                                        <td align="left" colspan="2" class="ClsBorderlight">
                                            <asp:Label ID="lblCasteSubCaste" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label10" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Email %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblEmail" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label11" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, DateOfBirth %>">
                                                </asp:Label><span class="colonPadding"> :</span> </span>
                                        </td>
                                        <td align="left" colspan="2" class="ClsBorderlight">
                                            <asp:Label ID="lblDateofBirth" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr1">
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label15" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PhoneNumber %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblPhoneNumber" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label17" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MobileNumber %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                        </td>
                                        <td align="left" colspan="2" class="ClsBorderlight">
                                            <asp:Label ID="lblResultMobileNumber" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr4">
                                        <td align="left" colspan="2" valign="top">
                                            <table id="Table1" width="100%" border="0" cellpadding="0" cellspacing="1">
                                                <tr id="Tr2">
                                                    <td align="left" colspan="2" valign="top" style="padding-top: 5px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="ClsLblLgnd" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LocalAddress %>"
                                                            Width="164px" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr6">
                                                    <td align="left" colspan="1" valign="top" style="width: 15%" class="ClsBorderlight">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label18" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Address %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                    </td>
                                                    <td align="left" colspan="1" style="width: 35%" class="ClsBorderlight">
                                                        <asp:Label ID="lblLocalAddress" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr3">
                                                    <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label20" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, City %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                    </td>
                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                        <asp:Label ID="lblLocalCity" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr9">
                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label21" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, State %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                    </td>
                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                        <asp:Label ID="lblLocalState" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr19">
                                                    <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label22" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Pincode %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                    </td>
                                                    <td align="left" colspan="1" class="ClsBorderlight">
                                                        <asp:Label ID="lblLocalPincode" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="3">
                                            <table id="tblPerAddress" runat="server" width="100%" border="0" cellpadding="0"
                                                cellspacing="1">
                                                <tr id="Tr5">
                                                    <td align="left" colspan="2" style="padding-top: 5px; height: 22px;">
                                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" CssClass="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, PermanentAddress %>"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr7">
                                                    <td align="left" colspan="1" valign="top" style="width: 15%" class="ClsBorderlight">
                                                    </td>
                                                    <td align="left" colspan="1" style="width: 35%" class="ClsBorderlight">
                                                        <asp:Label ID="lblPerAddress" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr8">
                                                    <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label23" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, City %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                    </td>
                                                    <td align="left" colspan="1" style="width: 15%" class="ClsBorderlight">
                                                        <asp:Label ID="lblPerCity" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr runat="server">
                                                    <td align="left" class="ClsBorderlight" colspan="1" valign="top">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label24" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, State %>">
                                                            </asp:Label><span class="colonPadding"> :</span> </span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="1">
                                                        <asp:Label ID="lblPerState" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="ClsBorderlight" colspan="1" valign="top">
                                                        <span class="ClsLabel">
                                                            <asp:Label ID="Label25" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Pincode %>">
                                                            </asp:Label><span class="colonPadding"> :</span> </span>
                                                    </td>
                                                    <td align="left" class="ClsBorderlight" colspan="1">
                                                        <asp:Label ID="lblPerPincode" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                                    <tr id="Tr10">
                                        <td align="left" colspan="2" valign="top">
                                            <asp:Label ID="Label12" runat="server" Font-Bold="True" CssClass="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, EducationalDetails %>"
                                                Width="164px" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr12" valign="top">
                                        <td align="center" colspan="4" style="" valign="top">
                                            <asp:GridView CssClass="GridBorder" ID="grdvwEducationDetails" runat="server" AutoGenerateColumns="False"
                                                CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" Height="100%"
                                                EnableViewState="False" Width="100%" DataKeyNames="Qualification_Id,Class_Id">
                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                <Columns>
                                                    <asp:BoundField DataField="Qualification_Name" HeaderText="<%$ Resources:LocalizedResources, Qualification %>"
                                                        SortExpression="Qualification_Name">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="Specialization" HeaderText="<%$ Resources:LocalizedResources, Specialization %>"
                                                        SortExpression="Specialization">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                    </asp:BoundField>
                                                  
                                                    <asp:BoundField DataField="Year_Of_Passing" HeaderText="<%$ Resources:LocalizedResources, YearOfPassing %>"
                                                        SortExpression="Year_Of_Passing">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Passing_University" HeaderText="<%$ Resources:LocalizedResources, University %>"
                                                        SortExpression="Passing_University">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
                                                            Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Class_Name" HeaderText="<%$ Resources:LocalizedResources, ClassGrade %>"
                                                        SortExpression="Class_Name">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle CssClass="ClsGridRow" />
                                                <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left" valign="top">
                                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                                    <tr id="Tr15">
                                        <td align="left" colspan="4" valign="bottom" style="padding-top: 5px">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" CssClass="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, ExperienceDetails %>"
                                                Width="164px" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr14">
                                        <td align="left" colspan="1" style="width: 20%" class="ClsBorderlight" valign="top">
                                            <asp:Label ID="Label34" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, DateOfRetirement %>">
                                            </asp:Label><span class="ClsLabel colonPadding"> :</span>
                                        </td>
                                        <td align="left" colspan="1" width="30%" class="ClsBorderlight">
                                            <asp:Label ID="lblDateofRetirement" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                        <asp:Label ID="Label16" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Achievements %>">
                                            </asp:Label><span class="ClsLabel colonPadding"> :</span>
                                        </td>
                                        <td align="left" colspan="4" width="100%" class="ClsBorderlight">
                                            <asp:Label ID="lblAchivements" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr13">
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                        <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, PastExperience %>">
                                            </asp:Label><span class="ClsLabel colonPadding"> :</span>
                                        </td>
                                        <td align="left" colspan="4" width="100%" class="ClsBorderlight">
                                            <asp:Label ID="lblYears" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                            <asp:Label ID="Label13" Text="yrs" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                            <asp:Label ID="lblMonths" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                            <asp:Label ID="Label14" Text="months" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="Tr11" width="100%">
                            <td align="center" colspan="4" style="" valign="top">
                                <asp:ListView ID="lstvwExpDetails" runat="server" DataKeyNames="SchoolName,JoiningDate,leftDate">
                                    <LayoutTemplate>
                                        <table align="center" width="100%" runat="server" id="tblExperienceInfo" style="color: #333333"
                                            cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                <th align="left" width="40%" style="padding-left: 9px;">
                                                    <asp:Label ID="Label25" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolName %>">
                                                    </asp:Label>
                                                </th>
                                                <th align="center">
                                                    <asp:Label ID="Label26" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoinedDate %>">
                                                    </asp:Label>
                                                </th>
                                                <th align="center">
                                                    <asp:Label ID="Label27" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeftDate %>">
                                                    </asp:Label>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="left" class="paddingL">
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="left" class="paddingL">
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr id="trEmpDetails">
                            <td align="left" colspan="4" valign="top">
                                <table id="tblEmpDetails" width="100%" border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td align="left" colspan="4" valign="top" style="padding-top: 5px">
                                            <asp:Label ID="lblEmployeeDetails" runat="server" CssClass="ClsLblLgnd" Font-Bold="True"
                                                Text="<%$ Resources:LocalizedResources, EmployeeDetails %>" Width="164px" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" valign="top" style="width: 20%" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label29" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PanNo %>">
                                                </asp:Label><span class="colonPadding"> :</span> </span>
                                        </td>
                                        <td align="left" colspan="1" style="width: 153px" class="ClsBorderlight">
                                            <asp:Label ID="lblPanNo" runat="server" CssClass="ClsLblRslt" Font-Bold="False" EnableViewState="false"> </asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="width: 5%">
                                        </td>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight" style="width: 20%">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label30" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoiningDate %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                            <asp:Label ID="lblJoiningDate" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label31" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PermanentDate %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <asp:Label ID="lblPermanentDate" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1">
                                        </td>
                                        <td align="left" colspan="1" valign="top" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label32" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ResignationDate %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <asp:Label ID="lblResignationDate" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label28" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ServiceType %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <asp:Label ID="lblJobType" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                         <td align="left" colspan="1">
                                        </td> 
                                        <td id="tdGrade1" runat="server" visible="false" align="left" colspan="1" valign="top" class="ClsBorderlight"  >
                                            <span class="ClsLabel">
                                                <asp:Label ID="Label35" runat="server" EnableViewState="False" Text="Grade Pay (Rs.)">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                        </td>
                                        <td id="tdGrade2" runat="server" visible="false" align="left" colspan="1" class="ClsBorderlight">
                                            <asp:Label ID="lblGrade" runat="server" CssClass="ClsLblRslt" Font-Bold="False"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table width="100%">
                                    <tr id="Tr21">
                                        <td align="left" colspan="2" valign="top" width="50%">
                                            <table id="Table4" width="100%" border="0" cellpadding="0" cellspacing="1">
                                                <tr id="Tr22">
                                                    <td align="left" colspan="4" style="padding-top: 5px">
                                                        <asp:Label ID="Label33" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                            Text="<%$ Resources:LocalizedResources, SubjectDetails %>" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr23">
                                                    <td align="left" colspan="4" class="ClsBorderlight">
                                                        <asp:Label ID="lblSubjectLists" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" colspan="1" style="width: 5%">
                                        </td>
                                        <td align="left" colspan="2">
                                            <table id="Table3" width="100%" border="0" cellpadding="0" cellspacing="1">
                                                <tr id="Tr17">
                                                    <td align="left" colspan="4" style="padding-top: 5px">
                                                        <asp:Label ID="Label19" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
                                                            Text="<%$ Resources:LocalizedResources, StandardDetails %>" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="Tr18">
                                                    <td align="left" colspan="4" class="ClsBorderlight">
                                                        <asp:Label ID="lblStandardsList" runat="server" CssClass="ClsLblRslt" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <!-- Data Insert End Here -->
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
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
        <tr>
            <td align="center" style="padding-right: 15px; padding-top: 10px;">
                <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnSml" BorderStyle="Solid"
                    OnClientClick="window.close(); return false;" CausesValidation="False" UseSubmitBehavior="False" />
            </td>
        </tr>
    </table>
</asp:Content>
